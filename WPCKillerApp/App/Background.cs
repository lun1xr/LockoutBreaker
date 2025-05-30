﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpcmon.App
{
    public partial class Background
    {

        private LaunchOpSettings? settingsWindow;

        private void ShowWindow()
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                settingsWindow.Show();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                settingsWindow!.Focus();
            }
            catch (Exception e) when (e is NullReferenceException || e is InvalidOperationException)
            {
                settingsWindow = new LaunchOpSettings();
                settingsWindow.Show();
                settingsWindow.Focus();
            }
        }

        internal void TaskbarIcon_DoubleClick(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }

        internal void MenuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }

        internal void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}