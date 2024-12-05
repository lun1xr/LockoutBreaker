using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace WPCKillerApp.App
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isSafeMode;

        public MainViewModel()
        {
            // Load SafeMode setting from App.config
            var safeModeSetting = ConfigurationManager.AppSettings["SafeMode"];
            _isSafeMode = !string.IsNullOrEmpty(safeModeSetting) && bool.Parse(safeModeSetting);
        }

        public bool IsSafeMode
        {
            get => _isSafeMode;
            set
            {
                if (_isSafeMode != value)
                {
                    _isSafeMode = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
