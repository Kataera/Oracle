using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Tarot.Forms.MaterialSkin.Controls
{
    public class MaterialForm : Form, IMaterialControl
    {
        public const int HtCaption = 0x2;
        public const int WmLbuttondblclk = 0x0203;
        public const int WmLbuttondown = 0x0201;
        public const int WmLbuttonup = 0x0202;
        public const int WmMousemove = 0x0200;

        public const int WmNclbuttondown = 0xA1;
        public const int WmRbuttondown = 0x0204;
        private const int ActionBarHeight = 40;
        private const int BorderWidth = 7;
        private const int Htbottom = 15;
        private const int Htbottomleft = 16;
        private const int Htbottomright = 17;
        private const int Htleft = 10;
        private const int Htright = 11;
        private const int Httop = 12;
        private const int Httopleft = 13;
        private const int Httopright = 14;

        private const int MonitorDefaulttonearest = 2;

        private const int StatusBarButtonWidth = StatusBarHeight;
        private const int StatusBarHeight = 24;

        private const uint TpmLeftalign = 0x0000;
        private const uint TpmReturncmd = 0x0100;

        private const int WmSyscommand = 0x0112;
        private const int WmszBottom = 6;
        private const int WmszBottomleft = 7;
        private const int WmszBottomright = 8;
        private const int WmszLeft = 1;
        private const int WmszRight = 2;

        private const int WmszTop = 3;
        private const int WmszTopleft = 4;
        private const int WmszTopright = 5;
        private const int WsMinimizebox = 0x20000;
        private const int WsSysmenu = 0x00080000;

        private readonly Cursor[] resizeCursors =
        {
            Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeWE,
            Cursors.SizeNS
        };

        private readonly Dictionary<int, int> resizingLocationsToCmd = new Dictionary<int, int>
        {
            {Httop, WmszTop},
            {Httopleft, WmszTopleft},
            {Httopright, WmszTopright},
            {Htleft, WmszLeft},
            {Htright, WmszRight},
            {Htbottom, WmszBottom},
            {Htbottomleft, WmszBottomleft},
            {Htbottomright, WmszBottomright}
        };

        private Rectangle actionBarBounds;
        private ButtonState buttonState = ButtonState.None;
        private bool headerMouseDown;
        private Rectangle maxButtonBounds;

        private bool maximized;

        private Rectangle minButtonBounds;
        private Point previousLocation;
        private Size previousSize;
        private ResizeDirection resizeDir;
        private Rectangle statusBarBounds;
        private Rectangle xButtonBounds;

        public MaterialForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            Sizable = true;
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            // This enables the form to trigger the MouseMove event even when mouse is over another control
            Application.AddMessageFilter(new MouseMessageFilter());
            MouseMessageFilter.MouseMove += OnGlobalMouseMove;
        }

        private enum ButtonState
        {
            XOver,
            MaxOver,
            MinOver,
            XDown,
            MaxDown,
            MinDown,
            None
        }

        private enum ResizeDirection
        {
            BottomLeft,
            Left,
            Right,
            BottomRight,
            Bottom,
            None
        }

        [Browsable(false)]
        public int Depth { get; set; }

        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            set { base.FormBorderStyle = value; }
        }

        [Browsable(false)]
        public MouseState MouseState { get; set; }

        public bool Sizable { get; set; }

        [Browsable(false)]
        public MaterialSkinManager SkinManager
        {
            get { return MaterialSkinManager.Instance; }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var par = base.CreateParams;

                // WS_SYSMENU: Trigger the creation of the system menu
                // WS_MINIMIZEBOX: Allow minimizing from taskbar
                par.Style = par.Style | WsMinimizebox | WsSysmenu; // Turn on the WS_MINIMIZEBOX style flag
                return par;
            }
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] Monitorinfoex info);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        protected void OnGlobalMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDisposed)
            {
                // Convert to client position and pass to Form.MouseMove
                var clientCursorPos = PointToClient(e.Location);
                var newE = new MouseEventArgs(MouseButtons.None, 0, clientCursorPos.X, clientCursorPos.Y, 0);
                OnMouseMove(newE);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (DesignMode)
            {
                return;
            }
            UpdateButtons(e);

            if (e.Button == MouseButtons.Left && !this.maximized)
            {
                ResizeForm(this.resizeDir);
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (DesignMode)
            {
                return;
            }
            this.buttonState = ButtonState.None;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (DesignMode)
            {
                return;
            }

            if (Sizable)
            {
                //True if the mouse is hovering over a child control
                var isChildUnderMouse = GetChildAtPoint(e.Location) != null;

                if (e.Location.X < BorderWidth && e.Location.Y > Height - BorderWidth && !isChildUnderMouse &&
                    !this.maximized)
                {
                    this.resizeDir = ResizeDirection.BottomLeft;
                    Cursor = Cursors.SizeNESW;
                }
                else if (e.Location.X < BorderWidth && !isChildUnderMouse && !this.maximized)
                {
                    this.resizeDir = ResizeDirection.Left;
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Location.X > Width - BorderWidth && e.Location.Y > Height - BorderWidth &&
                         !isChildUnderMouse && !this.maximized)
                {
                    this.resizeDir = ResizeDirection.BottomRight;
                    Cursor = Cursors.SizeNWSE;
                }
                else if (e.Location.X > Width - BorderWidth && !isChildUnderMouse && !this.maximized)
                {
                    this.resizeDir = ResizeDirection.Right;
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Location.Y > Height - BorderWidth && !isChildUnderMouse && !this.maximized)
                {
                    this.resizeDir = ResizeDirection.Bottom;
                    Cursor = Cursors.SizeNS;
                }
                else
                {
                    this.resizeDir = ResizeDirection.None;

                    //Only reset the cursor when needed, this prevents it from flickering when a child control changes the cursor to its own needs
                    if (this.resizeCursors.Contains(Cursor))
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }

            UpdateButtons(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (DesignMode)
            {
                return;
            }
            UpdateButtons(e, true);

            base.OnMouseUp(e);
            ReleaseCapture();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(SkinManager.GetApplicationBackgroundColor());
            g.FillRectangle(SkinManager.ColorScheme.DarkPrimaryBrush, this.statusBarBounds);
            g.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, this.actionBarBounds);

            //Draw border
            using (var borderPen = new Pen(SkinManager.GetDividersColor(), 1))
            {
                g.DrawLine(borderPen, new Point(0, this.actionBarBounds.Bottom), new Point(0, Height - 2));
                g.DrawLine(borderPen, new Point(Width - 1, this.actionBarBounds.Bottom),
                    new Point(Width - 1, Height - 2));
                g.DrawLine(borderPen, new Point(0, Height - 1), new Point(Width - 1, Height - 1));
            }

            // Determine whether or not we even should be drawing the buttons.
            var showMin = MinimizeBox && ControlBox;
            var showMax = MaximizeBox && ControlBox;
            var hoverBrush = SkinManager.GetFlatButtonHoverBackgroundBrush();
            var downBrush = SkinManager.GetFlatButtonPressedBackgroundBrush();

            // When MaximizeButton == false, the minimize button will be painted in its place
            if (this.buttonState == ButtonState.MinOver && showMin)
            {
                g.FillRectangle(hoverBrush, showMax ? this.minButtonBounds : this.maxButtonBounds);
            }

            if (this.buttonState == ButtonState.MinDown && showMin)
            {
                g.FillRectangle(downBrush, showMax ? this.minButtonBounds : this.maxButtonBounds);
            }

            if (this.buttonState == ButtonState.MaxOver && showMax)
            {
                g.FillRectangle(hoverBrush, this.maxButtonBounds);
            }

            if (this.buttonState == ButtonState.MaxDown && showMax)
            {
                g.FillRectangle(downBrush, this.maxButtonBounds);
            }

            if (this.buttonState == ButtonState.XOver && ControlBox)
            {
                g.FillRectangle(hoverBrush, this.xButtonBounds);
            }

            if (this.buttonState == ButtonState.XDown && ControlBox)
            {
                g.FillRectangle(downBrush, this.xButtonBounds);
            }

            using (var formButtonsPen = new Pen(SkinManager.ActionBarTextSecondary, 2))
            {
                // Minimize button.
                if (showMin)
                {
                    var x = showMax ? this.minButtonBounds.X : this.maxButtonBounds.X;
                    var y = showMax ? this.minButtonBounds.Y : this.maxButtonBounds.Y;

                    g.DrawLine(
                        formButtonsPen,
                        x + (int) (this.minButtonBounds.Width * 0.33),
                        y + (int) (this.minButtonBounds.Height * 0.66),
                        x + (int) (this.minButtonBounds.Width * 0.66),
                        y + (int) (this.minButtonBounds.Height * 0.66)
                        );
                }

                // Maximize button
                if (showMax)
                {
                    g.DrawRectangle(
                        formButtonsPen, this.maxButtonBounds.X + (int) (this.maxButtonBounds.Width * 0.33),
                        this.maxButtonBounds.Y + (int) (this.maxButtonBounds.Height * 0.36),
                        (int) (this.maxButtonBounds.Width * 0.39),
                        (int) (this.maxButtonBounds.Height * 0.31)
                        );
                }

                // Close button
                if (ControlBox)
                {
                    g.DrawLine(
                        formButtonsPen, this.xButtonBounds.X + (int) (this.xButtonBounds.Width * 0.33),
                        this.xButtonBounds.Y + (int) (this.xButtonBounds.Height * 0.33),
                        this.xButtonBounds.X + (int) (this.xButtonBounds.Width * 0.66),
                        this.xButtonBounds.Y + (int) (this.xButtonBounds.Height * 0.66)
                        );

                    g.DrawLine(
                        formButtonsPen, this.xButtonBounds.X + (int) (this.xButtonBounds.Width * 0.66),
                        this.xButtonBounds.Y + (int) (this.xButtonBounds.Height * 0.33),
                        this.xButtonBounds.X + (int) (this.xButtonBounds.Width * 0.33),
                        this.xButtonBounds.Y + (int) (this.xButtonBounds.Height * 0.66));
                }
            }

            //Form title
            //g.DrawString(Text, SkinManager.ROBOTO_MEDIUM_12, SkinManager.ColorScheme.TextBrush, new Rectangle(SkinManager.FORM_PADDING, STATUS_BAR_HEIGHT, Width, ACTION_BAR_HEIGHT), new StringFormat { LineAlignment = StringAlignment.Center });
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.minButtonBounds = new Rectangle(Width - SkinManager.FormPadding / 2 - 3 * StatusBarButtonWidth, 0,
                StatusBarButtonWidth, StatusBarHeight);
            this.maxButtonBounds = new Rectangle(Width - SkinManager.FormPadding / 2 - 2 * StatusBarButtonWidth, 0,
                StatusBarButtonWidth, StatusBarHeight);
            this.xButtonBounds = new Rectangle(Width - SkinManager.FormPadding / 2 - StatusBarButtonWidth, 0,
                StatusBarButtonWidth, StatusBarHeight);
            this.statusBarBounds = new Rectangle(0, 0, Width, StatusBarHeight);
            this.actionBarBounds = new Rectangle(0, StatusBarHeight, Width, ActionBarHeight);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (DesignMode || IsDisposed)
            {
                return;
            }

            if (m.Msg == WmLbuttondblclk)
            {
                MaximizeWindow(!this.maximized);
            }
            else if (m.Msg == WmMousemove && this.maximized &&
                     (this.statusBarBounds.Contains(PointToClient(Cursor.Position))
                      || this.actionBarBounds.Contains(PointToClient(Cursor.Position))) &&
                     !(this.minButtonBounds.Contains(PointToClient(Cursor.Position))
                       || this.maxButtonBounds.Contains(PointToClient(Cursor.Position))
                       || this.xButtonBounds.Contains(PointToClient(Cursor.Position))))
            {
                if (this.headerMouseDown)
                {
                    this.maximized = false;
                    this.headerMouseDown = false;

                    var mousePoint = PointToClient(Cursor.Position);
                    if (mousePoint.X < Width / 2)
                    {
                        Location = mousePoint.X < this.previousSize.Width / 2
                            ? new Point(Cursor.Position.X - mousePoint.X, Cursor.Position.Y - mousePoint.Y)
                            : new Point(Cursor.Position.X - this.previousSize.Width / 2,
                                Cursor.Position.Y - mousePoint.Y);
                    }
                    else
                    {
                        Location = Width - mousePoint.X < this.previousSize.Width / 2
                            ? new Point(Cursor.Position.X - this.previousSize.Width + Width - mousePoint.X,
                                Cursor.Position.Y - mousePoint.Y)
                            : new Point(Cursor.Position.X - this.previousSize.Width / 2,
                                Cursor.Position.Y - mousePoint.Y);
                    }

                    Size = this.previousSize;
                    ReleaseCapture();
                    SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
                }
            }
            else if (m.Msg == WmLbuttondown &&
                     (this.statusBarBounds.Contains(PointToClient(Cursor.Position))
                      || this.actionBarBounds.Contains(PointToClient(Cursor.Position))) &&
                     !(this.minButtonBounds.Contains(PointToClient(Cursor.Position))
                       || this.maxButtonBounds.Contains(PointToClient(Cursor.Position))
                       || this.xButtonBounds.Contains(PointToClient(Cursor.Position))))
            {
                if (!this.maximized)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
                }
                else
                {
                    this.headerMouseDown = true;
                }
            }
            else if (m.Msg == WmRbuttondown)
            {
                var cursorPos = PointToClient(Cursor.Position);

                if (this.statusBarBounds.Contains(cursorPos) && !this.minButtonBounds.Contains(cursorPos) &&
                    !this.maxButtonBounds.Contains(cursorPos) && !this.xButtonBounds.Contains(cursorPos))
                {
                    // Show default system menu when right clicking titlebar
                    var id = TrackPopupMenuEx(
                        GetSystemMenu(Handle, false),
                        TpmLeftalign | TpmReturncmd,
                        Cursor.Position.X, Cursor.Position.Y, Handle, IntPtr.Zero);

                    // Pass the command as a WM_SYSCOMMAND message
                    SendMessage(Handle, WmSyscommand, id, 0);
                }
            }
            else if (m.Msg == WmNclbuttondown)
            {
                // This re-enables resizing by letting the application know when the
                // user is trying to resize a side. This is disabled by default when using WS_SYSMENU.
                if (!Sizable)
                {
                    return;
                }

                byte bFlag = 0;

                // Get which side to resize from
                if (this.resizingLocationsToCmd.ContainsKey((int) m.WParam))
                {
                    bFlag = (byte) this.resizingLocationsToCmd[(int) m.WParam];
                }

                if (bFlag != 0)
                {
                    SendMessage(Handle, WmSyscommand, 0xF000 | bFlag, (int) m.LParam);
                }
            }
            else if (m.Msg == WmLbuttonup)
            {
                this.headerMouseDown = false;
            }
        }

        private void MaximizeWindow(bool maximize)
        {
            if (!MaximizeBox || !ControlBox)
            {
                return;
            }

            this.maximized = maximize;

            if (maximize)
            {
                var monitorHandle = MonitorFromWindow(Handle, MonitorDefaulttonearest);
                var monitorInfo = new Monitorinfoex();
                GetMonitorInfo(new HandleRef(null, monitorHandle), monitorInfo);
                this.previousSize = Size;
                this.previousLocation = Location;
                Size = new Size(monitorInfo.rcWork.Width(), monitorInfo.rcWork.Height());
                Location = new Point(monitorInfo.rcWork.left, monitorInfo.rcWork.top);
            }
            else
            {
                Size = this.previousSize;
                Location = this.previousLocation;
            }
        }

        private void ResizeForm(ResizeDirection direction)
        {
            if (DesignMode)
            {
                return;
            }
            var dir = -1;
            switch (direction)
            {
                case ResizeDirection.BottomLeft:
                    dir = Htbottomleft;
                    break;
                case ResizeDirection.Left:
                    dir = Htleft;
                    break;
                case ResizeDirection.Right:
                    dir = Htright;
                    break;
                case ResizeDirection.BottomRight:
                    dir = Htbottomright;
                    break;
                case ResizeDirection.Bottom:
                    dir = Htbottom;
                    break;
            }

            ReleaseCapture();
            if (dir != -1)
            {
                SendMessage(Handle, WmNclbuttondown, dir, 0);
            }
        }

        private void UpdateButtons(MouseEventArgs e, bool up = false)
        {
            if (DesignMode)
            {
                return;
            }
            var oldState = this.buttonState;
            var showMin = MinimizeBox && ControlBox;
            var showMax = MaximizeBox && ControlBox;

            if (e.Button == MouseButtons.Left && !up)
            {
                if (showMin && !showMax && this.maxButtonBounds.Contains(e.Location))
                {
                    this.buttonState = ButtonState.MinDown;
                }
                else if (showMin && showMax && this.minButtonBounds.Contains(e.Location))
                {
                    this.buttonState = ButtonState.MinDown;
                }
                else if (showMax && this.maxButtonBounds.Contains(e.Location))
                {
                    this.buttonState = ButtonState.MaxDown;
                }
                else if (ControlBox && this.xButtonBounds.Contains(e.Location))
                {
                    this.buttonState = ButtonState.XDown;
                }
                else
                {
                    this.buttonState = ButtonState.None;
                }
            }
            else
            {
                if (showMin && !showMax && this.maxButtonBounds.Contains(e.Location))
                {
                    this.buttonState = ButtonState.MinOver;

                    if (oldState == ButtonState.MinDown)
                    {
                        WindowState = FormWindowState.Minimized;
                    }
                }
                else if (showMin && showMax && this.minButtonBounds.Contains(e.Location))
                {
                    this.buttonState = ButtonState.MinOver;

                    if (oldState == ButtonState.MinDown)
                    {
                        WindowState = FormWindowState.Minimized;
                    }
                }
                else if (MaximizeBox && ControlBox && this.maxButtonBounds.Contains(e.Location))
                {
                    this.buttonState = ButtonState.MaxOver;

                    if (oldState == ButtonState.MaxDown)
                    {
                        MaximizeWindow(!this.maximized);
                    }
                }
                else if (ControlBox && this.xButtonBounds.Contains(e.Location))
                {
                    this.buttonState = ButtonState.XOver;

                    if (oldState == ButtonState.XDown)
                    {
                        Close();
                    }
                }
                else
                {
                    this.buttonState = ButtonState.None;
                }
            }

            if (oldState != this.buttonState)
            {
                Invalidate();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public int Width()
            {
                return this.right - this.left;
            }

            public int Height()
            {
                return this.bottom - this.top;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        public class Monitorinfoex
        {
            public int cbSize = Marshal.SizeOf(typeof (Monitorinfoex));
            public Rect rcMonitor = new Rect();
            public Rect rcWork = new Rect();
            public int dwFlags = 0;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szDevice = new char[32];
        }
    }

    public class MouseMessageFilter : IMessageFilter
    {
        private const int WmMousemove = 0x0200;

        public static event MouseEventHandler MouseMove;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WmMousemove)
            {
                if (MouseMove != null)
                {
                    int x = Control.MousePosition.X, y = Control.MousePosition.Y;

                    MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
                }
            }
            return false;
        }
    }
}