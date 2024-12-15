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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace WPCKillerApp.App
{
    /// <summary>
    /// Interaction logic for RE2.xaml
    /// </summary>
    public partial class RE2 : Page
    {
        public RE2()
        {
            InitializeComponent();
        }
        private void RecoveryMode_Click(object sender, RoutedEventArgs e)
        {
            StartShutDown("/r /o /f /t 0");
        }
        private static void StartShutDown(string param)
        {
            ProcessStartInfo proc = new()
            {
                FileName = "cmd",
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = "/C shutdown " + param 
            };
            Process.Start(proc);
        }
    }
}
