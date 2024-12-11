using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Numerics;
using System.Configuration;
using System.Windows.Controls.Primitives;
using System.Security.AccessControl;

namespace WPCKillerApp.App
{
    public partial class LaunchOpSettings : Window
    {
        public LaunchOpSettings()
        {
            InitializeComponent();
            this.Width = 371;
            this.Height = 415;
            LoadSettings();
        }
        // Autorun/Keybind Exclusivity Logic
        private void AutorunCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (KeybindCheckBox != null)
            {
                KeybindCheckBox.IsChecked = false;
            }
        }
        private void KeybindCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            AutorunCheckBox.IsChecked = false;
        }

        // Operation Ties

        private void AutoLaunchCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (AutoLaunchCheckBox.IsChecked == true)
            {
                AddStartupShortcut();
            }
            else
            {
                RemoveStartupShortcut();
            }
        }
        private static void AddStartupShortcut()
        {
            // from https://stackoverflow.com/questions/234231/creating-application-shortcut-in-a-directory
            var t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); // Windows Script Host Shell Object
            dynamic shell = Activator.CreateInstance(t!)!;
            try
            {
                var lnk = shell.CreateShortcut(Constants.AppShortcutPath);
                try
                {
                    lnk.TargetPath = Constants.ExePath;
                    lnk.IconLocation = $"{Constants.ExePath}, 0";
                    lnk.Save();
                }
                finally
                {
                    Marshal.FinalReleaseComObject(lnk);
                }
            }
            finally
            {
                Marshal.FinalReleaseComObject(shell);
            }
        }

        private static void RemoveStartupShortcut()
        {
            File.Delete(Constants.AppShortcutPath);
        }
        private void BackgroundRunCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (BackgroundRunCheckBox.IsChecked == true)
            {

            }
        }

        private bool isRecording = false;
        private string recordedKeys = string.Empty;
        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            isRecording = !isRecording;
            RecordButton.Content = isRecording ? "Stop Recording" : "Start Recording";
            if (!isRecording)
            {
                RecordedKeybindTextBox.Text = recordedKeys;
                recordedKeys = string.Empty;
            }
        }

        private bool isRecording2 = false;
        private string recordedKeys2 = string.Empty;
        private void RecordButton2_Click(object sender, RoutedEventArgs e)
        {
            isRecording2 = !isRecording2;
            RecordButton2.Content = isRecording2 ? "Stop Recording" : "Start Recording";
            if (!isRecording2)
            {
                RecordedKeybindTextBox2.Text = recordedKeys2;
                recordedKeys2 = string.Empty;
            }
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (isRecording)
            {
                if (!string.IsNullOrEmpty(recordedKeys))
                {
                    recordedKeys += " + ";
                }
                recordedKeys += e.Key.ToString();
                RecordedKeybindTextBox.Text = recordedKeys; // Update the TextBox live
                e.Handled = true;
            }
            if (isRecording2)
            {
                if (!string.IsNullOrEmpty(recordedKeys2))
                {
                    recordedKeys2 += " + ";
                }
                recordedKeys2 += e.Key.ToString();
                RecordedKeybindTextBox2.Text = recordedKeys2; // Update the TextBox live
                e.Handled = true;
            }
        }
        private void Popup_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (sender is Thumb thumb)
            {
                var parent = thumb.Parent;
                while (parent is FrameworkElement frameworkElement && frameworkElement is not Popup)
                {
                    parent = frameworkElement.Parent;
                }

                if (parent is Popup popup)
                {
                    var horizontalOffset = popup.HorizontalOffset + e.HorizontalChange;
                    var verticalOffset = popup.VerticalOffset + e.VerticalChange;

                    popup.HorizontalOffset = horizontalOffset;
                    popup.VerticalOffset = verticalOffset;
                }
            }
        }
        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        { 
            ImageCheck.IsChecked = false;
        }

        // Empty references for missing interactions
        private void AutorunCheckBox_Unchecked(object sender, RoutedEventArgs e) { }
        private void AutorunCheckBox_Click(object sender, RoutedEventArgs e) { }
        private void KeybindCheckBox_Unchecked(object sender, RoutedEventArgs e) { }
        private void KeybindCheckBox_Click(object sender, RoutedEventArgs e) { }
        private void SafeModeCheckBox_Checked(object sender, RoutedEventArgs e) { }
        private void SafeModeCheckBox_Unchecked(object sender, RoutedEventArgs e) { }
        private void SafeModeCheckBox_Click(object sender, RoutedEventArgs e) { }

        // Save Settings
        private async void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
            SaveButton.IsEnabled = false;
            await Task.Delay(500);
            SaveButton.IsEnabled = true;
        }

        private void SaveSettings()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["AutoLaunch"].Value = AutoLaunchCheckBox.IsChecked == true ? "true" : "false";
            config.AppSettings.Settings["SafeMode"].Value = SafeModeCheckBox.IsChecked == true ? "true" : "false";
            config.AppSettings.Settings["LaunchMinimized"].Value = BackgroundRunCheckBox.IsChecked == true ? "true" : "false";
            config.AppSettings.Settings["Autorun"].Value = AutorunCheckBox.IsChecked == true ? "true" : "false";
            config.AppSettings.Settings["Keybind"].Value = KeybindCheckBox.IsChecked == true ? "true" : "false";
            config.AppSettings.Settings["RecordedKeybind"].Value = RecordedKeybindTextBox.Text;
            config.AppSettings.Settings["RecordedKeybind2"].Value = RecordedKeybindTextBox2.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        private void LoadSettings()
        {
            var autoLaunch = ConfigurationManager.AppSettings["AutoLaunch"];
            var safeMode = ConfigurationManager.AppSettings["SafeMode"];
            var launchMinimized = ConfigurationManager.AppSettings["LaunchMinimized"];
            var autorun = ConfigurationManager.AppSettings["Autorun"];
            var keybind = ConfigurationManager.AppSettings["Keybind"];
            var recordedKeybind = ConfigurationManager.AppSettings["RecordedKeybind"];
            var recordedKeybind2 = ConfigurationManager.AppSettings["RecordedKeybind2"];

            AutoLaunchCheckBox.IsChecked = autoLaunch?.ToLower() == "true";
            SafeModeCheckBox.IsChecked = safeMode?.ToLower() == "true";
            BackgroundRunCheckBox.IsChecked = launchMinimized?.ToLower() == "true";
            AutorunCheckBox.IsChecked = autorun?.ToLower() == "true";
            KeybindCheckBox.IsChecked = keybind?.ToLower() == "true";
            RecordedKeybindTextBox.Text = recordedKeybind;
            RecordedKeybindTextBox2.Text = recordedKeybind2;
        }
    }
}