using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WPCKillerApp.App
{
    public partial class LaunchOpSettings : Window
    {
        public LaunchOpSettings()
        {
            InitializeComponent();
        }

        // ImageClick Event

        private bool _isImageClicked;

        public bool IsImageClicked
        {
            get => _isImageClicked;
            set
            {
                _isImageClicked = value;
                OnPropertyChanged();
            }
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            IsImageClicked = !IsImageClicked;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Combobox Exclusivity Logic
        private void KeybindComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KeybindComboBox?.SelectedItem is ComboBoxItem selectedItem)
            {
                var selectedContent = selectedItem.Content.ToString();
                foreach (ComboBoxItem item in KeybindComboBox2.Items)
                {
                    item.IsEnabled = item.Content.ToString() != selectedContent;
                }
            }
        }
        private void KeybindComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KeybindComboBox2?.SelectedItem is ComboBoxItem selectedItem)
            {
                var selectedContent = selectedItem.Content.ToString();
                foreach (ComboBoxItem item in KeybindComboBox.Items)
                {
                    item.IsEnabled = item.Content.ToString() != selectedContent;
                }
            }
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

        private void KeybindTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (isRecording)
            {
                if (!string.IsNullOrEmpty(recordedKeys))
                {
                    recordedKeys += " + ";
                }
                recordedKeys += e.Key.ToString();
                e.Handled = true;
            }
        }

        // Empty references for missing interactions
        private void AutorunCheckBox_Unchecked(object sender, RoutedEventArgs e) { }
        private void AutorunCheckBox_Click(object sender, RoutedEventArgs e) { }
        private void KeybindCheckBox_Unchecked(object sender, RoutedEventArgs e) { }
        private void KeybindCheckBox_Click(object sender, RoutedEventArgs e) { }
        private void SafeModeCheckBox_Checked(object sender, RoutedEventArgs e) { }
        private void SafeModeCheckBox_Unchecked(object sender, RoutedEventArgs e) { }
        private void SafeModeCheckBox_Click(object sender, RoutedEventArgs e) { }
    }
}