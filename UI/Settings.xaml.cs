using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Oracle.UI
{
    /// <summary>
    ///     Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {

        public Settings()
        {
            InitializeComponent();

            SourceInitialized += win_SourceInitialized;
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

        [DllImport("user32")]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, MonitorInfo lpmi);

        private void MaximiseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
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

        [DllImport("User32")]
        private static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        private void NavigationPanel_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void win_SourceInitialized(object sender, EventArgs e)
        {
            var handle = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(handle)?.AddHook(WindowProc);
        }

        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg != 0x0024) return (IntPtr) 0;

            WmGetMinMaxInfo(hwnd, lParam);
            handled = true;

            return (IntPtr) 0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = (MinMaxInfo) Marshal.PtrToStructure(lParam, typeof(MinMaxInfo));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            const int monitorDefaultToNearest = 0x00000002;
            var monitor = MonitorFromWindow(hwnd, monitorDefaultToNearest);

            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new MonitorInfo();
                GetMonitorInfo(monitor, monitorInfo);

                var rcWorkArea = monitorInfo.rcWork;
                var rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MinMaxInfo
        {
            private readonly Point ptReserved;
            public Point ptMaxSize;
            public Point ptMaxPosition;
            private readonly Point ptMinTrackSize;
            private readonly Point ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class MonitorInfo
        {
            public readonly Rect rcMonitor = new Rect();
            public readonly Rect rcWork = new Rect();
            public int cbSize = Marshal.SizeOf(typeof(MonitorInfo));
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Point
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        private struct Rect
        {
            private bool Equals(Rect other)
            {
                return left == other.left && top == other.top && right == other.right && bottom == other.bottom;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;

                return obj is Rect && Equals((Rect) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = left;
                    hashCode = (hashCode * 397) ^ top;
                    hashCode = (hashCode * 397) ^ right;
                    hashCode = (hashCode * 397) ^ bottom;
                    return hashCode;
                }
            }

            public readonly int left;
            public readonly int top;
            public readonly int right;
            public readonly int bottom;
            private static readonly Rect Empty;

            public Rect(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public Rect(Rect rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }

            public static bool operator ==(Rect rect1, Rect rect2)
            {
                return rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom;
            }

            public static bool operator !=(Rect rect1, Rect rect2)
            {
                return !(rect1 == rect2);
            }
        }
    }
}