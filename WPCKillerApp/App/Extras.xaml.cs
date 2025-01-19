using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Wpcmon.App
{
    /// <summary>
    /// ¯\_(ツ)_/¯
    /// </summary>
    public partial class Extras : System.Windows.Window
    {
        public Extras()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(10);
            LoadSettings();
        }



        private HashSet<Key> recordedKeySet = new HashSet<Key>();

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
                recordedKeySet.Clear();
            }
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.IsRepeat)
            {
                return;
            }

            if (isRecording)
            {
                if (!recordedKeySet.Contains(e.Key))
                {
                    if (IsModifierKey(e.Key) || !recordedKeySet.Any(k => !IsModifierKey(k)))
                    {
                        if (!string.IsNullOrEmpty(recordedKeys))
                        {
                            recordedKeys += " + ";
                        }
                        recordedKeys += e.Key.ToString();
                        RecordedKeybindTextBox.Text = recordedKeys;
                        recordedKeySet.Add(e.Key);
                    }
                }
                e.Handled = true;
            }
        }

        private bool IsModifierKey(Key key)
        {
            return key == Key.LeftShift || key == Key.RightShift ||
                   key == Key.LeftCtrl || key == Key.RightCtrl ||
                   key == Key.LeftAlt || key == Key.RightAlt ||
                   key == Key.LWin || key == Key.RWin;
        }
        private ModifierKeys GetModifierKey(Key key)
        {
            return key switch
            {
                Key.LeftShift or Key.RightShift => ModifierKeys.Shift,
                Key.LeftCtrl or Key.RightCtrl => ModifierKeys.Control,
                Key.LeftAlt or Key.RightAlt => ModifierKeys.Alt,
                Key.LWin or Key.RWin => ModifierKeys.Windows,
                _ => ModifierKeys.None,
            };
        }
        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {

            SaveSettings(sender, e);
        }

        private void SafeModeCheckBox_Click(object sender, RoutedEventArgs e) 
        {
            if (SafeModeCheckBox.IsChecked == true)
            {

            }
            else
            {

            }
        }
        private void SafeModeCheckBox_Checked(object sender, RoutedEventArgs e) { }
        private void SafeModeCheckBox_Unchecked(object sender, RoutedEventArgs e) { }

        private void LoadSettings()
        {
            var recordedKeybind = ConfigurationManager.AppSettings["RecordedKeybind"];
            var safeMode = ConfigurationManager.AppSettings["SafeMode"];

            RecordedKeybindTextBox.Text = recordedKeybind;
            SafeModeCheckBox.IsChecked = safeMode?.ToLower() == "true";
        }
        private void SaveSettings(object? sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["SafeMode"].Value = SafeModeCheckBox.IsChecked == true ? "true" : "false";
            config.AppSettings.Settings["RecordedKeybind"].Value = RecordedKeybindTextBox.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            
            System.Windows.Application.Current.Windows
                .OfType<KeybindRegisterExecute>()
                .FirstOrDefault()?.Close();
            var KeybindRegisterExecute = new KeybindRegisterExecute();
            KeybindRegisterExecute.Show();
            KeybindRegisterExecute.Hide();
        }
    }
}
