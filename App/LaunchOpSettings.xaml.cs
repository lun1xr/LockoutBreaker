using System.Windows;
using System.Windows.Controls;

namespace WPCKillerApp.App 
{
    public partial class LaunchOpSettings : Window 
    {
        public LaunchOpSettings() 
        {
            InitializeComponent();
        }
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
    }
}