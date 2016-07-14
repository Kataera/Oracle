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
        public Settings()
        {
            InitializeComponent();
            SourceInitialized += SourceInitialised;
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NavigationPanel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
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

        private void ControlPanel_OnMouseDownPanel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void SourceInitialised(object sender, EventArgs e)
        {
            var handle = new WindowInteropHelper(this).Handle;
            var hwndSource = HwndSource.FromHwnd(handle);
            hwndSource?.AddHook(WindowProc);
        }

        private static IntPtr WindowProc(
              IntPtr hwnd,
              int msg,
              IntPtr wParam,
              IntPtr lParam,
              ref bool handled)
        {
            if (msg != 0x0024) return (IntPtr) 0;

            WmGetMinMaxInfo(hwnd, lParam);
            handled = true;

            return (IntPtr) 0;
        }

        private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {
            // Adjust the maximized size and position to fit the work area of the correct monitor.
            var minMaxInfo = (MinMaxInfo)Marshal.PtrToStructure(lParam, typeof(MinMaxInfo));
            const int monitorDefaultToNearest = 0x00000002;
            var monitor = MonitorFromWindow(hwnd, monitorDefaultToNearest);

            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new MonitorInfo();
                GetMonitorInfo(monitor, monitorInfo);
                var rcWorkArea = monitorInfo.rcWork;
                var rcMonitorArea = monitorInfo.rcMonitor;
                minMaxInfo.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                minMaxInfo.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                minMaxInfo.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                minMaxInfo.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }

            Marshal.StructureToPtr(minMaxInfo, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Point
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MinMaxInfo
        {
            private readonly Point ptReserved;
            public Point ptMaxSize;
            public Point ptMaxPosition;
            private readonly Point ptMinTrackSize;
            private readonly Point ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MonitorInfo
        {        
            public int cbSize = Marshal.SizeOf(typeof(MonitorInfo));           
            public Rectangle rcMonitor = new Rectangle();          
            public Rectangle rcWork = new Rectangle();         
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct Rectangle
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public static readonly Rectangle Empty;

            public int Width => Math.Abs(right - left);

            public int Height => bottom - top;

            public Rectangle(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public Rectangle(Rectangle rectangle)
            {
                this.left = rectangle.left;
                this.top = rectangle.top;
                this.right = rectangle.right;
                this.bottom = rectangle.bottom;
            }

            public bool IsEmpty => left >= right || top >= bottom;
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MonitorInfo lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
    }
}