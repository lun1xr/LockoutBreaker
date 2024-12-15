using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace WPCKillerApp.App
{
    /// <summary>
    /// Interaction logic for EnterRE.xaml
    /// </summary>
    public partial class EnterRE : Page
    {
        public EnterRE()
        {
            InitializeComponent();
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            var recordedPath = ConfigurationManager.AppSettings["RecordedPath"];
            Bash.Text = $" >C:{Environment.NewLine}{Environment.NewLine} >cd {recordedPath}{Environment.NewLine}{Environment.NewLine} >elevateuser.bat";
        }
    }
}
