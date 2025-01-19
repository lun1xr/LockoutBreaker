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
using Constants = Wpcmon.App.Constants;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace Wpcmon.App
{
    public partial class LaunchOpSettings : Window
    {
        //private TimesUp_ _timesUp = null!;
        private CancellationTokenSource? _cancellationTokenSource;
        public LaunchOpSettings()
        {
            InitializeComponent();
            this.Width = 371;
            this.Height = 415;
            LoadSettings();
            this.Closed += SaveSettings;
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
        private void ImageCheck_Check(object sender, RoutedEventArgs e)
        {
            if (ImageCheck.IsChecked == true)
            {
                Extras extras = new();
                extras.Owner = this;
                extras.ShowDialog();
            }
        }

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
                recordedKeySet2.Clear();
            }
        }

        private HashSet<Key> recordedKeySet2 = new HashSet<Key>();

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (isRecording2)
            {
                if (!recordedKeySet2.Contains(e.Key))
                {
                    if (IsModifierKey(e.Key) || !recordedKeySet2.Any(k => !IsModifierKey(k)))
                    {
                        if (!string.IsNullOrEmpty(recordedKeys2))
                        {
                            recordedKeys2 += " + ";
                        }
                        recordedKeys2 += e.Key.ToString();
                        RecordedKeybindTextBox2.Text = recordedKeys2;
                        recordedKeySet2.Add(e.Key);
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

        private void AutorunCheckBox_Click(object sender, RoutedEventArgs e) 
        {
            if (AutorunCheckBox.IsChecked == true)
            {
                ProcessMonitor();
            }
            else
            {
                KillYOURSELF();
            }
        }
        private void ProcessMonitor()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(1000);
                    Process[] processes = Process.GetProcessesByName("Wpcmon");
                    if (processes.Length > 0)
                    {
                        ThatOneThing();
                    }
                }
            }, token);
        }
        private void KillYOURSELF() //Do it
        {
            _cancellationTokenSource?.Cancel();
        }

        public static void ThatOneThing()
        {
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Verb = "runas", 
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                };

                using (Process? process = Process.Start(processInfo))
                {
                    if (process != null)
                    {
                        using (StreamWriter sw = process.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                // Write your series of commands here
                                sw.WriteLine("takeown /F %systemroot%\\System32\\wpcmon.exe");
                                sw.WriteLine("icacls \"%systemroot%\\System32\\wpcmon.exe\" /grant Administrators:F");
                                sw.WriteLine("taskkill /F /IM wpcmon.exe /T");
                                sw.WriteLine("ren %systemroot%\\System32\\wpcmon.exe wpcmon_disbaled.exe");
                                sw.WriteLine("SCHTASKS /Delete /TN \"\\Microsoft\\Windows\\Shell\\FamilySafetyMonitor\" /F");
                                sw.WriteLine("SCHTASKS /Delete /TN \"\\Microsoft\\Windows\\Shell\\FamilySafetyRefreshTask\" /F");
                            }
                        }

                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();

                        process.WaitForExit();

                        MessageBox.Show(error);
                        Console.WriteLine(output);
                        Console.WriteLine(error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        // Empty references for missing interactions
        private void AutorunCheckBox_Unchecked(object sender, RoutedEventArgs e) { }
        private void KeybindCheckBox_Unchecked(object sender, RoutedEventArgs e) { }
        private void KeybindCheckBox_Click(object sender, RoutedEventArgs e) { }



        private async void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings(sender, e);
            SaveButton.IsEnabled = false;
            await Task.Delay(500);
            SaveButton.IsEnabled = true;
        }

        private void SaveSettings(object? sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["AutoLaunch"].Value = AutoLaunchCheckBox.IsChecked == true ? "true" : "false";
            config.AppSettings.Settings["LaunchMinimized"].Value = BackgroundRunCheckBox.IsChecked == true ? "true" : "false";
            config.AppSettings.Settings["Autorun"].Value = AutorunCheckBox.IsChecked == true ? "true" : "false";
            config.AppSettings.Settings["Keybind"].Value = KeybindCheckBox.IsChecked == true ? "true" : "false";
            config.AppSettings.Settings["RecordedKeybind2"].Value = RecordedKeybindTextBox2.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            if (ConfigurationManager.AppSettings["Autorun"] == "false")
            {
                KillYOURSELF();
            }
            System.Windows.Application.Current.Windows
                .OfType<KeybindRegisterExecute>()
                .FirstOrDefault()?.Close();
            var KeybindRegisterExecute = new KeybindRegisterExecute();
            KeybindRegisterExecute.Show();
            KeybindRegisterExecute.Hide();
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
            BackgroundRunCheckBox.IsChecked = launchMinimized?.ToLower() == "true";
            AutorunCheckBox.IsChecked = autorun?.ToLower() == "true";
            KeybindCheckBox.IsChecked = keybind?.ToLower() == "true";
            RecordedKeybindTextBox2.Text = recordedKeybind2;

            if (ConfigurationManager.AppSettings["Autorun"] == "true")
            {
                ProcessMonitor();
            }
            if (ConfigurationManager.AppSettings["Autorun"] == "false")
            {
                KillYOURSELF();
            }
        }
    }
}