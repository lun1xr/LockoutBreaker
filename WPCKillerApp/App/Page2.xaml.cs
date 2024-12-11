using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPCKillerApp
{
    public partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            UpdateBatCode();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                // Use the selected path
                string selectedPath = dialog.SelectedPath;
                FolderPath.Text = selectedPath;
            }
        }
        public string BatPath
        {
            get { return FolderPath.Text; }
        }
        public static string UserPath
        {
            get { return Environment.UserName; }
        }
        private void UpdateBatCode()
        {
            BatCode.Text = $"File content:{Environment.NewLine}net localgroup administrators " + $"{UserPath}" + $" /add";
        }
    }
}