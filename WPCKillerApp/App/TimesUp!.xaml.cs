using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace Wpcmon.App
{
    /// <summary>
    /// Interaction logic for TimesUp_.xaml
    /// </summary>
    public partial class TimesUp_ : Window
    {
        public TimesUp_()
        {
            InitializeComponent();
            LoadMsg();
        }

        private void LoadMsg()
        {
            string filePath = Environment.ExpandEnvironmentVariables(@"%PROGRAMDATA%\Microsoft\Windows\Parental Controls\settings\settings.bin");

            if (!File.Exists(filePath))
            {
                Message.Text = "Settings file not found.";
                return;
            }

            byte[] fileBytes = File.ReadAllBytes(filePath);
            string settings = Encoding.Unicode.GetString(fileBytes); // UTF-16 encoding

            string startRead = "Begin_\":";
            string endRead = ",";

            int startIndex = settings.IndexOf(startRead);
            string formattedTime = string.Empty;
            if (startIndex != -1)
            {
                startIndex += startRead.Length;

                int endIndex = settings.IndexOf(endRead, startIndex);
                if (endIndex != -1)
                {
                    string result = settings.Substring(startIndex, endIndex - startIndex);
                    if (int.TryParse(result, out int minutesAfterMidnight))
                    {
                        TimeSpan time = TimeSpan.FromMinutes(minutesAfterMidnight);
                        formattedTime = time.ToString("hh\\:mm");
                    }
                }
            }

            Message.Text = $"This device is now locked until {formattedTime} on {DateTime.Today.AddDays(1):MM/dd/yyyy}, because of your Family Safety{Environment.NewLine}settings.";
        }
    }
}
