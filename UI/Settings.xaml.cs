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

        internal Settings()
        {
            InitializeComponent();
            SourceInitialized += Settings_SourceInitialized;
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        protected void HideAllButtons()
        {
            if (windowHandle == null)
            {
                throw new InvalidOperationException("The window has not yet been completely initialised.");
            }

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

        private void MoveWindowOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void NavigationPanel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
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