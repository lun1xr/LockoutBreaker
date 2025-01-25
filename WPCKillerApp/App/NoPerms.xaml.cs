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

namespace Wpcmon.App
{
    public partial class NoPerms : Window
    {
        private int currentPageIndex = 0;
        private readonly Page[] pages;

        public NoPerms()
        {
            InitializeComponent();
            pages = new Page[] { new DetectedBasic(), new Step1(), new Page2(this), new Page1(), new EnterRE(), new RE2() };
            MainFrame.Content = pages[currentPageIndex];
            this.Closed += Shutdown();
        }
        private EventHandler Shutdown()
        {
            return (s, e) => Environment.Exit(0);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                MainFrame.Content = pages[currentPageIndex];
            }
            if (currentPageIndex == 1) 
            {
                Next.IsEnabled = true;
            }
            if (currentPageIndex == 4)
            {
                Next.IsEnabled = true;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPageIndex < pages.Length - 1)
            {
                currentPageIndex++;
                MainFrame.Content = pages[currentPageIndex];
            }
            if (currentPageIndex == 5)
            {
                Next.IsEnabled = false;
            }
            if (currentPageIndex == 2)
            {
                Next.IsEnabled = false;
            }
        }
    }
}