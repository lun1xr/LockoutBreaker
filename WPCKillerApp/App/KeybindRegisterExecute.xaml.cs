using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Wpcmon.App
{
    /// <summary>
    /// Interaction logic for KeybindRegisterExecute.xaml
    /// </summary>
    public partial class KeybindRegisterExecute : Window
    {
        private TimesUp_ _timesUp = null!;
        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_ID = 9000;
        private const int HOTKEY_ID2 = 9001;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        public KeybindRegisterExecute()
        {
            InitializeComponent();
            this.Loaded += KeyLoaded;
            this.Closing += KeyClosing;
        }
        private void KeyLoaded(object sender, RoutedEventArgs e)
        {

            RegisterKeybind1();
            RegisterKeybind2();
        }
        private void KeyClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            UnregisterKeybind1();
            UnregisterKeybind2();
        }
        private bool KeybindSet = false;
        private void RegisterKeybind1()
        {
            string? keysString = ConfigurationManager.AppSettings["RecordedKeybind"];
            string separator = " + ";
            if (!string.IsNullOrEmpty(keysString))
            {
                string[] keyArray = keysString.Split(new string[] { separator }, StringSplitOptions.None);
                List<Key> keys = new List<Key>();
                ModifierKeys modifiers = ModifierKeys.None;

                foreach (string keyString in keyArray)
                {
                    if (Enum.TryParse(keyString, true, out Key key))
                    {
                        if (IsModifierKey(key))
                        {
                            modifiers |= GetModifierKey(key);
                        }
                        else
                        {
                            keys.Add(key);
                        }
                    }
                }

                if (keys.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("No valid keys found for the hotkey.");
                    return;
                }

                var helper = new WindowInteropHelper(this);
                IntPtr handle = helper.Handle;
                if (handle == IntPtr.Zero)
                {
                    System.Diagnostics.Debug.WriteLine("Invalid window handle.");
                    return;
                }

                var source = HwndSource.FromHwnd(handle);
                source.AddHook(HwndHook1);

                uint vk = (uint)KeyInterop.VirtualKeyFromKey(keys[0]);
                uint fsModifiers = (uint)modifiers;

                System.Diagnostics.Debug.WriteLine($"Attempting to register hotkey: Modifiers={fsModifiers}, Key={vk}");

                if (!RegisterHotKey(handle, HOTKEY_ID, fsModifiers, vk))
                {
                    System.Diagnostics.Debug.WriteLine("Failed to register hotkey. It might be already in use.");
                }
                else
                {
                    KeybindSet = true;
                    System.Diagnostics.Debug.WriteLine("Hotkey registered successfully.");
                }
            }
        }
        private void UnregisterKeybind1()
        {
            try
            {
                var helper = new WindowInteropHelper(this);
                IntPtr handle = helper.Handle;
                if (handle == IntPtr.Zero)
                {
                    System.Diagnostics.Debug.WriteLine("Invalid window handle.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine("Attempting to unregister hotkey.");

                if (KeybindSet && !UnregisterHotKey(handle, HOTKEY_ID))
                {
                    System.Diagnostics.Debug.WriteLine("Failed to unregister hotkey.");
                }
                else
                {
                    KeybindSet = false;
                    System.Diagnostics.Debug.WriteLine("Hotkey unregistered successfully.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred while unregistering the hotkey: {ex.Message}");
            }
        }
        private IntPtr HwndHook1(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            try
            {
                if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
                {
                    if (_timesUp == null)
                    {
                        _timesUp = new TimesUp_();
                    }
                    if (_timesUp.IsVisible)
                    {
                        _timesUp.Hide();
                    }
                    else
                    {
                        _timesUp.Show();
                    }
                    handled = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred in HwndHook1: {ex.Message}");
            }
            return IntPtr.Zero;
        }
        private void RegisterKeybind2()
        {
            string? keysString = ConfigurationManager.AppSettings["RecordedKeybind2"];
            string separator = " + ";
            if (!string.IsNullOrEmpty(keysString))
            {
                string[] keyArray = keysString.Split(new string[] { separator }, StringSplitOptions.None);
                List<Key> keys = new List<Key>();
                ModifierKeys modifiers = ModifierKeys.None;

                foreach (string keyString in keyArray)
                {
                    if (Enum.TryParse(keyString, true, out Key key))
                    {
                        if (IsModifierKey(key))
                        {
                            modifiers |= GetModifierKey(key);
                        }
                        else
                        {
                            keys.Add(key);
                        }
                    }
                }

                if (keys.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("No valid keys found for the hotkey.");
                    return;
                }

                var helper = new WindowInteropHelper(this);
                IntPtr handle = helper.Handle;
                if (handle == IntPtr.Zero)
                {
                    System.Diagnostics.Debug.WriteLine("Invalid window handle.");
                    return;
                }

                var source = HwndSource.FromHwnd(handle);
                source.AddHook(HwndHook2);

                uint vk = (uint)KeyInterop.VirtualKeyFromKey(keys[0]);
                uint fsModifiers = (uint)modifiers;

                System.Diagnostics.Debug.WriteLine($"Attempting to register hotkey: Modifiers={fsModifiers}, Key={vk}");

                if (!RegisterHotKey(handle, HOTKEY_ID2, fsModifiers, vk))
                {
                    System.Diagnostics.Debug.WriteLine("Failed to register hotkey. It might be already in use.");
                }
                else
                {
                    KeybindSet = true;
                    System.Diagnostics.Debug.WriteLine("Hotkey registered successfully.");
                }
            }
        }
        private void UnregisterKeybind2()
        {
            try
            {
                var helper = new WindowInteropHelper(this);
                IntPtr handle = helper.Handle;
                if (handle == IntPtr.Zero)
                {
                    System.Diagnostics.Debug.WriteLine("Invalid window handle.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine("Attempting to unregister hotkey.");

                if (KeybindSet && !UnregisterHotKey(handle, HOTKEY_ID2))
                {
                    System.Diagnostics.Debug.WriteLine("Failed to unregister hotkey.");
                }
                else
                {
                    KeybindSet = false;
                    System.Diagnostics.Debug.WriteLine("Hotkey unregistered successfully.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred while unregistering the hotkey: {ex.Message}");
            }
        }
        private IntPtr HwndHook2(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            try
            {
                if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID2)
                {
                    LaunchOpSettings.ThatOneThing();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred in HwndHook2: {ex.Message}");
            }
            return IntPtr.Zero;
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
    }
}
