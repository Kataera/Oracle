using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

using MaterialDesignThemes.Wpf;

namespace Oracle.UI
{
    /// <summary>
    ///     Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings
    {
        private const int GwlStyle = -16;

        private const int WsSysmenu = 0x80000;
        private IntPtr windowHandle;

        public Settings()
        {
            InitializeComponent();
            SourceInitialized += Settings_SourceInitialized;

            // Add expander events after initialising the window to prevent null references.
            GeneralSettingsExpander.Expanded += GeneralSettingsExpander_OnExpanded;
            FateSettingsExpander.Expanded += FateSettingsExpander_OnExpanded;
            NavigationSettingsExpander.Expanded += NavigationSettingsExpander_OnExpanded;
            BlacklistSettingsExpander.Expanded += BlacklistSettingsExpander_OnExpanded;
            AuthenticationSettingsExpander.Expanded += AuthenticationSettingsExpander_OnExpanded;
        }

        private void AuthenticationSettingsExpander_OnExpanded(object sender, RoutedEventArgs e)
        {
            GeneralSettingsExpander.IsExpanded = false;
            FateSettingsExpander.IsExpanded = false;
            NavigationSettingsExpander.IsExpanded = false;
            BlacklistSettingsExpander.IsExpanded = false;
        }

        private void BlacklistSettingsExpander_OnExpanded(object sender, RoutedEventArgs e)
        {
            GeneralSettingsExpander.IsExpanded = false;
            FateSettingsExpander.IsExpanded = false;
            NavigationSettingsExpander.IsExpanded = false;
            AuthenticationSettingsExpander.IsExpanded = false;
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ControlPanel_OnMouseDownPanel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void FateSettingsExpander_OnExpanded(object sender, RoutedEventArgs e)
        {
            GeneralSettingsExpander.IsExpanded = false;
            NavigationSettingsExpander.IsExpanded = false;
            BlacklistSettingsExpander.IsExpanded = false;
            AuthenticationSettingsExpander.IsExpanded = false;
        }

        private void GeneralSettingsExpander_OnExpanded(object sender, RoutedEventArgs e)
        {
            FateSettingsExpander.IsExpanded = false;
            NavigationSettingsExpander.IsExpanded = false;
            BlacklistSettingsExpander.IsExpanded = false;
            AuthenticationSettingsExpander.IsExpanded = false;
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        protected void HideAllButtons()
        {
            if (windowHandle == null)
                throw new InvalidOperationException("The window has not yet been completely initialized");

            SetWindowLong(windowHandle, GwlStyle, GetWindowLong(windowHandle, GwlStyle) & ~WsSysmenu);
        }

        private void MaximiseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:

                    MaximiseWindowIcon.Kind = PackIconKind.WindowRestore;
                    WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:

                    MaximiseWindowIcon.Kind = PackIconKind.WindowMaximize;
                    WindowState = WindowState.Normal;
                    break;
                case WindowState.Minimized:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void MinimiseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void NavigationPanel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void NavigationSettingsExpander_OnExpanded(object sender, RoutedEventArgs e)
        {
            GeneralSettingsExpander.IsExpanded = false;
            FateSettingsExpander.IsExpanded = false;
            BlacklistSettingsExpander.IsExpanded = false;
            AuthenticationSettingsExpander.IsExpanded = false;
        }

        private void Settings_SourceInitialized(object sender, EventArgs e)
        {
            windowHandle = new WindowInteropHelper(this).Handle;
            HideAllButtons();
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    }
}