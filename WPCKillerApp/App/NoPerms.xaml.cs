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
using System.Windows.Shapes;

namespace WPCKillerApp.App
{
    public partial class NoPerms : Window
    {
        private int currentPageIndex = 0;
        private readonly Page[] pages = { new DetectedBasic(), new Step1(), new Page2() };

        public NoPerms()
        {
            InitializeComponent();
            MainFrame.Content = pages[currentPageIndex];
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                MainFrame.Content = pages[currentPageIndex];
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex < pages.Length - 1)
            {
                currentPageIndex++;
                MainFrame.Content = pages[currentPageIndex];
            }
        }
    }
}