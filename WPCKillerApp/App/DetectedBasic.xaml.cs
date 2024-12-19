using System;
using System.Collections.Generic;
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

namespace Wpcmon.App
{
    /// <summary>
    /// Interaction logic for DetectedBasic.xaml
    /// </summary>
    public partial class DetectedBasic : Page
    {
        public DetectedBasic()
        {
            InitializeComponent();
        }
        private void AccessButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
