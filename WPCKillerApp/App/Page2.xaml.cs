using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
using System.Configuration;
using WPCKillerApp.App;
using System.Windows.Forms.VisualStyles;

namespace WPCKillerApp
{
    public partial class Page2 : Page
    {
        private readonly NoPerms noPermsWindow;
        public Page2(NoPerms noPerms)
        {
            InitializeComponent();
            noPermsWindow = noPerms;
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
        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = FolderPath.Text;
            if (!string.IsNullOrWhiteSpace(folderPath))
            {
                string filePath = System.IO.Path.Combine(folderPath, "elevateuser.bat");
                System.IO.File.WriteAllText(filePath, BatCode.Text);
                System.Windows.MessageBox.Show("File saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                noPermsWindow.Next.IsEnabled = true;
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a folder first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["RecordedPath"].Value = FolderPath.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
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
            BatCode.Text = $"net localgroup administrators \"{UserPath}\" /add";
        }
        public bool IsCurrentPage()
        {
            return NavigationService?.Content == this;
        }
    }
}