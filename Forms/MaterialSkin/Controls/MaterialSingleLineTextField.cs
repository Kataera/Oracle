using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Tarot.Forms.MaterialSkin.Animations;

namespace Tarot.Forms.MaterialSkin.Controls
{

    #region Using Directives

    #endregion

    public class MaterialSingleLineTextField : Control, IMaterialControl
    {
        private readonly AnimationManager animationManager;

        private readonly BaseTextBox baseTextBox;

        public MaterialSingleLineTextField()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer, true);

            this.animationManager = new AnimationManager
            {
                Increment = 0.06,
                AnimationType = AnimationType.EaseInOut,
                InterruptAnimation = false
            };
            this.animationManager.OnAnimationProgress += sender => this.Invalidate();

            this.baseTextBox = new BaseTextBox
            {
                BorderStyle = BorderStyle.None,
                Font = this.SkinManager.RobotoRegular11,
                ForeColor = this.SkinManager.GetPrimaryTextColor(),
                Location = new Point(0, 0),
                Width = this.Width,
                Height = this.Height - 5
            };

            if (!this.Controls.Contains(this.baseTextBox) && !this.DesignMode)
            {
                this.Controls.Add(this.baseTextBox);
            }

            this.baseTextBox.GotFocus +=
                (sender, args) => this.animationManager.StartNewAnimation(AnimationDirection.In);
            this.baseTextBox.LostFocus +=
                (sender, args) => this.animationManager.StartNewAnimation(AnimationDirection.Out);
            this.BackColorChanged += (sender, args) =>
            {
                this.baseTextBox.BackColor = this.BackColor;
                this.baseTextBox.ForeColor = this.SkinManager.GetPrimaryTextColor();
            };

            //Fix for tabstop
            this.baseTextBox.TabStop = true;
            this.TabStop = false;
        }

        public event EventHandler AcceptsTabChanged
        {
            add { this.baseTextBox.AcceptsTabChanged += value; }
            remove { this.baseTextBox.AcceptsTabChanged -= value; }
        }

        public new event EventHandler AutoSizeChanged
        {
            add { this.baseTextBox.AutoSizeChanged += value; }
            remove { this.baseTextBox.AutoSizeChanged -= value; }
        }

        public new event EventHandler BackgroundImageChanged
        {
            add { this.baseTextBox.BackgroundImageChanged += value; }
            remove { this.baseTextBox.BackgroundImageChanged -= value; }
        }

        public new event EventHandler BackgroundImageLayoutChanged
        {
            add { this.baseTextBox.BackgroundImageLayoutChanged += value; }
            remove { this.baseTextBox.BackgroundImageLayoutChanged -= value; }
        }

        public new event EventHandler BindingContextChanged
        {
            add { this.baseTextBox.BindingContextChanged += value; }
            remove { this.baseTextBox.BindingContextChanged -= value; }
        }

        public event EventHandler BorderStyleChanged
        {
            add { this.baseTextBox.BorderStyleChanged += value; }
            remove { this.baseTextBox.BorderStyleChanged -= value; }
        }

        public new event EventHandler CausesValidationChanged
        {
            add { this.baseTextBox.CausesValidationChanged += value; }
            remove { this.baseTextBox.CausesValidationChanged -= value; }
        }

        public event UICuesEventHandler ChangeUiCues
        {
            add { this.baseTextBox.ChangeUICues += value; }
            remove { this.baseTextBox.ChangeUICues -= value; }
        }

        public new event EventHandler Click
        {
            add { this.baseTextBox.Click += value; }
            remove { this.baseTextBox.Click -= value; }
        }

        public new event EventHandler ClientSizeChanged
        {
            add { this.baseTextBox.ClientSizeChanged += value; }
            remove { this.baseTextBox.ClientSizeChanged -= value; }
        }

        public new event EventHandler ContextMenuChanged
        {
            add { this.baseTextBox.ContextMenuChanged += value; }
            remove { this.baseTextBox.ContextMenuChanged -= value; }
        }

        public new event EventHandler ContextMenuStripChanged
        {
            add { this.baseTextBox.ContextMenuStripChanged += value; }
            remove { this.baseTextBox.ContextMenuStripChanged -= value; }
        }

        public new event ControlEventHandler ControlAdded
        {
            add { this.baseTextBox.ControlAdded += value; }
            remove { this.baseTextBox.ControlAdded -= value; }
        }

        public new event ControlEventHandler ControlRemoved
        {
            add { this.baseTextBox.ControlRemoved += value; }
            remove { this.baseTextBox.ControlRemoved -= value; }
        }

        public new event EventHandler CursorChanged
        {
            add { this.baseTextBox.CursorChanged += value; }
            remove { this.baseTextBox.CursorChanged -= value; }
        }

        public new event EventHandler Disposed
        {
            add { this.baseTextBox.Disposed += value; }
            remove { this.baseTextBox.Disposed -= value; }
        }

        public new event EventHandler DockChanged
        {
            add { this.baseTextBox.DockChanged += value; }
            remove { this.baseTextBox.DockChanged -= value; }
        }

        public new event EventHandler DoubleClick
        {
            add { this.baseTextBox.DoubleClick += value; }
            remove { this.baseTextBox.DoubleClick -= value; }
        }

        public new event DragEventHandler DragDrop
        {
            add { this.baseTextBox.DragDrop += value; }
            remove { this.baseTextBox.DragDrop -= value; }
        }

        public new event DragEventHandler DragEnter
        {
            add { this.baseTextBox.DragEnter += value; }
            remove { this.baseTextBox.DragEnter -= value; }
        }

        public new event EventHandler DragLeave
        {
            add { this.baseTextBox.DragLeave += value; }
            remove { this.baseTextBox.DragLeave -= value; }
        }

        public new event DragEventHandler DragOver
        {
            add { this.baseTextBox.DragOver += value; }
            remove { this.baseTextBox.DragOver -= value; }
        }

        public new event EventHandler EnabledChanged
        {
            add { this.baseTextBox.EnabledChanged += value; }
            remove { this.baseTextBox.EnabledChanged -= value; }
        }

        public new event EventHandler Enter
        {
            add { this.baseTextBox.Enter += value; }
            remove { this.baseTextBox.Enter -= value; }
        }

        public new event EventHandler FontChanged
        {
            add { this.baseTextBox.FontChanged += value; }
            remove { this.baseTextBox.FontChanged -= value; }
        }

        public new event EventHandler ForeColorChanged
        {
            add { this.baseTextBox.ForeColorChanged += value; }
            remove { this.baseTextBox.ForeColorChanged -= value; }
        }

        public new event GiveFeedbackEventHandler GiveFeedback
        {
            add { this.baseTextBox.GiveFeedback += value; }
            remove { this.baseTextBox.GiveFeedback -= value; }
        }

        public new event EventHandler GotFocus
        {
            add { this.baseTextBox.GotFocus += value; }
            remove { this.baseTextBox.GotFocus -= value; }
        }

        public new event EventHandler HandleCreated
        {
            add { this.baseTextBox.HandleCreated += value; }
            remove { this.baseTextBox.HandleCreated -= value; }
        }

        public new event EventHandler HandleDestroyed
        {
            add { this.baseTextBox.HandleDestroyed += value; }
            remove { this.baseTextBox.HandleDestroyed -= value; }
        }

        public new event HelpEventHandler HelpRequested
        {
            add { this.baseTextBox.HelpRequested += value; }
            remove { this.baseTextBox.HelpRequested -= value; }
        }

        public event EventHandler HideSelectionChanged
        {
            add { this.baseTextBox.HideSelectionChanged += value; }
            remove { this.baseTextBox.HideSelectionChanged -= value; }
        }

        public new event EventHandler ImeModeChanged
        {
            add { this.baseTextBox.ImeModeChanged += value; }
            remove { this.baseTextBox.ImeModeChanged -= value; }
        }

        public new event InvalidateEventHandler Invalidated
        {
            add { this.baseTextBox.Invalidated += value; }
            remove { this.baseTextBox.Invalidated -= value; }
        }

        public new event KeyEventHandler KeyDown
        {
            add { this.baseTextBox.KeyDown += value; }
            remove { this.baseTextBox.KeyDown -= value; }
        }

        public new event KeyPressEventHandler KeyPress
        {
            add { this.baseTextBox.KeyPress += value; }
            remove { this.baseTextBox.KeyPress -= value; }
        }

        public new event KeyEventHandler KeyUp
        {
            add { this.baseTextBox.KeyUp += value; }
            remove { this.baseTextBox.KeyUp -= value; }
        }

        public new event LayoutEventHandler Layout
        {
            add { this.baseTextBox.Layout += value; }
            remove { this.baseTextBox.Layout -= value; }
        }

        public new event EventHandler Leave
        {
            add { this.baseTextBox.Leave += value; }
            remove { this.baseTextBox.Leave -= value; }
        }

        public new event EventHandler LocationChanged
        {
            add { this.baseTextBox.LocationChanged += value; }
            remove { this.baseTextBox.LocationChanged -= value; }
        }

        public new event EventHandler LostFocus
        {
            add { this.baseTextBox.LostFocus += value; }
            remove { this.baseTextBox.LostFocus -= value; }
        }

        public new event EventHandler MarginChanged
        {
            add { this.baseTextBox.MarginChanged += value; }
            remove { this.baseTextBox.MarginChanged -= value; }
        }

        public event EventHandler ModifiedChanged
        {
            add { this.baseTextBox.ModifiedChanged += value; }
            remove { this.baseTextBox.ModifiedChanged -= value; }
        }

        public new event EventHandler MouseCaptureChanged
        {
            add { this.baseTextBox.MouseCaptureChanged += value; }
            remove { this.baseTextBox.MouseCaptureChanged -= value; }
        }

        public new event MouseEventHandler MouseClick
        {
            add { this.baseTextBox.MouseClick += value; }
            remove { this.baseTextBox.MouseClick -= value; }
        }

        public new event MouseEventHandler MouseDoubleClick
        {
            add { this.baseTextBox.MouseDoubleClick += value; }
            remove { this.baseTextBox.MouseDoubleClick -= value; }
        }

        public new event MouseEventHandler MouseDown
        {
            add { this.baseTextBox.MouseDown += value; }
            remove { this.baseTextBox.MouseDown -= value; }
        }

        public new event EventHandler MouseEnter
        {
            add { this.baseTextBox.MouseEnter += value; }
            remove { this.baseTextBox.MouseEnter -= value; }
        }

        public new event EventHandler MouseHover
        {
            add { this.baseTextBox.MouseHover += value; }
            remove { this.baseTextBox.MouseHover -= value; }
        }

        public new event EventHandler MouseLeave
        {
            add { this.baseTextBox.MouseLeave += value; }
            remove { this.baseTextBox.MouseLeave -= value; }
        }

        public new event MouseEventHandler MouseMove
        {
            add { this.baseTextBox.MouseMove += value; }
            remove { this.baseTextBox.MouseMove -= value; }
        }

        public new event MouseEventHandler MouseUp
        {
            add { this.baseTextBox.MouseUp += value; }
            remove { this.baseTextBox.MouseUp -= value; }
        }

        public new event MouseEventHandler MouseWheel
        {
            add { this.baseTextBox.MouseWheel += value; }
            remove { this.baseTextBox.MouseWheel -= value; }
        }

        public new event EventHandler Move
        {
            add { this.baseTextBox.Move += value; }
            remove { this.baseTextBox.Move -= value; }
        }

        public event EventHandler MultilineChanged
        {
            add { this.baseTextBox.MultilineChanged += value; }
            remove { this.baseTextBox.MultilineChanged -= value; }
        }

        public new event EventHandler PaddingChanged
        {
            add { this.baseTextBox.PaddingChanged += value; }
            remove { this.baseTextBox.PaddingChanged -= value; }
        }

        public new event PaintEventHandler Paint
        {
            add { this.baseTextBox.Paint += value; }
            remove { this.baseTextBox.Paint -= value; }
        }

        public new event EventHandler ParentChanged
        {
            add { this.baseTextBox.ParentChanged += value; }
            remove { this.baseTextBox.ParentChanged -= value; }
        }

        public new event PreviewKeyDownEventHandler PreviewKeyDown
        {
            add { this.baseTextBox.PreviewKeyDown += value; }
            remove { this.baseTextBox.PreviewKeyDown -= value; }
        }

        public new event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
        {
            add { this.baseTextBox.QueryAccessibilityHelp += value; }
            remove { this.baseTextBox.QueryAccessibilityHelp -= value; }
        }

        public new event QueryContinueDragEventHandler QueryContinueDrag
        {
            add { this.baseTextBox.QueryContinueDrag += value; }
            remove { this.baseTextBox.QueryContinueDrag -= value; }
        }

        public event EventHandler ReadOnlyChanged
        {
            add { this.baseTextBox.ReadOnlyChanged += value; }
            remove { this.baseTextBox.ReadOnlyChanged -= value; }
        }

        public new event EventHandler RegionChanged
        {
            add { this.baseTextBox.RegionChanged += value; }
            remove { this.baseTextBox.RegionChanged -= value; }
        }

        public new event EventHandler Resize
        {
            add { this.baseTextBox.Resize += value; }
            remove { this.baseTextBox.Resize -= value; }
        }

        public new event EventHandler RightToLeftChanged
        {
            add { this.baseTextBox.RightToLeftChanged += value; }
            remove { this.baseTextBox.RightToLeftChanged -= value; }
        }

        public new event EventHandler SizeChanged
        {
            add { this.baseTextBox.SizeChanged += value; }
            remove { this.baseTextBox.SizeChanged -= value; }
        }

        public new event EventHandler StyleChanged
        {
            add { this.baseTextBox.StyleChanged += value; }
            remove { this.baseTextBox.StyleChanged -= value; }
        }

        public new event EventHandler SystemColorsChanged
        {
            add { this.baseTextBox.SystemColorsChanged += value; }
            remove { this.baseTextBox.SystemColorsChanged -= value; }
        }

        public new event EventHandler TabIndexChanged
        {
            add { this.baseTextBox.TabIndexChanged += value; }
            remove { this.baseTextBox.TabIndexChanged -= value; }
        }

        public new event EventHandler TabStopChanged
        {
            add { this.baseTextBox.TabStopChanged += value; }
            remove { this.baseTextBox.TabStopChanged -= value; }
        }

        public event EventHandler TextAlignChanged
        {
            add { this.baseTextBox.TextAlignChanged += value; }
            remove { this.baseTextBox.TextAlignChanged -= value; }
        }

        public new event EventHandler TextChanged
        {
            add { this.baseTextBox.TextChanged += value; }
            remove { this.baseTextBox.TextChanged -= value; }
        }

        public new event EventHandler Validated
        {
            add { this.baseTextBox.Validated += value; }
            remove { this.baseTextBox.Validated -= value; }
        }

        public new event CancelEventHandler Validating
        {
            add { this.baseTextBox.Validating += value; }
            remove { this.baseTextBox.Validating -= value; }
        }

        public new event EventHandler VisibleChanged
        {
            add { this.baseTextBox.VisibleChanged += value; }
            remove { this.baseTextBox.VisibleChanged -= value; }
        }

        //Properties for managing the material design properties
        [Browsable(false)]
        public int Depth { get; set; }

        public string Hint
        {
            get { return this.baseTextBox.Hint; }
            set { this.baseTextBox.Hint = value; }
        }

        public int MaxLength
        {
            get { return this.baseTextBox.MaxLength; }
            set { this.baseTextBox.MaxLength = value; }
        }

        [Browsable(false)]
        public MouseState MouseState { get; set; }

        public char PasswordChar
        {
            get { return this.baseTextBox.PasswordChar; }
            set { this.baseTextBox.PasswordChar = value; }
        }

        public string SelectedText
        {
            get { return this.baseTextBox.SelectedText; }
            set { this.baseTextBox.SelectedText = value; }
        }

        public int SelectionLength
        {
            get { return this.baseTextBox.SelectionLength; }
            set { this.baseTextBox.SelectionLength = value; }
        }

        public int SelectionStart
        {
            get { return this.baseTextBox.SelectionStart; }
            set { this.baseTextBox.SelectionStart = value; }
        }

        [Browsable(false)]
        public MaterialSkinManager SkinManager
        {
            get { return MaterialSkinManager.Instance; }
        }

        public new object Tag
        {
            get { return this.baseTextBox.Tag; }
            set { this.baseTextBox.Tag = value; }
        }

        public override string Text
        {
            get { return this.baseTextBox.Text; }
            set { this.baseTextBox.Text = value; }
        }

        public int TextLength
        {
            get { return this.baseTextBox.TextLength; }
        }

        public bool UseSystemPasswordChar
        {
            get { return this.baseTextBox.UseSystemPasswordChar; }
            set { this.baseTextBox.UseSystemPasswordChar = value; }
        }

        public void Clear()
        {
            this.baseTextBox.Clear();
        }

        public void SelectAll()
        {
            this.baseTextBox.SelectAll();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            this.baseTextBox.BackColor = this.Parent.BackColor;
            this.baseTextBox.ForeColor = this.SkinManager.GetPrimaryTextColor();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.Clear(this.Parent.BackColor);

            var lineY = this.baseTextBox.Bottom + 3;

            if (!this.animationManager.IsAnimating())
            {
                //No animation
                g.FillRectangle(this.baseTextBox.Focused
                    ? this.SkinManager.ColorScheme.PrimaryBrush
                    : this.SkinManager.GetDividersBrush(), this.baseTextBox.Location.X,
                    lineY, this.baseTextBox.Width, this.baseTextBox.Focused ? 2 : 1);
            }
            else
            {
                //Animate
                var animationWidth = (int) (this.baseTextBox.Width * this.animationManager.GetProgress());
                var halfAnimationWidth = animationWidth / 2;
                var animationStart = this.baseTextBox.Location.X + this.baseTextBox.Width / 2;

                //Unfocused background
                g.FillRectangle(this.SkinManager.GetDividersBrush(), this.baseTextBox.Location.X,
                    lineY, this.baseTextBox.Width,
                    1);

                //Animated focus transition
                g.FillRectangle(this.SkinManager.ColorScheme.PrimaryBrush,
                    animationStart - halfAnimationWidth,
                    lineY,
                    animationWidth,
                    2);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.baseTextBox.Location = new Point(0, 0);
            this.baseTextBox.Width = this.Width;

            this.Height = this.baseTextBox.Height + 5;
        }

        private class BaseTextBox : TextBox
        {
            private const char EmptyChar = (char) 0;

            private const int EmSetcuebanner = 0x1501;

            private const char NonVisualStylePasswordChar = '\u002A';

            private const char VisualStylePasswordChar = '\u25CF';

            private string hint = string.Empty;

            private char passwordChar = EmptyChar;

            private char useSystemPasswordChar = EmptyChar;

            public BaseTextBox()
            {
                MaterialContextMenuStrip cms = new TextBoxContextMenuStrip();
                cms.Opening += this.ContextMenuStripOnOpening;
                cms.OnItemClickStart += this.ContextMenuStripOnItemClickStart;

                this.ContextMenuStrip = cms;
            }

            public string Hint
            {
                get { return this.hint; }
                set
                {
                    this.hint = value;
                    SendMessage(this.Handle, EmSetcuebanner, (int) IntPtr.Zero, this.Hint);
                }
            }

            public new char PasswordChar
            {
                get { return this.passwordChar; }
                set
                {
                    this.passwordChar = value;
                    this.SetBasePasswordChar();
                }
            }

            public new bool UseSystemPasswordChar
            {
                get { return this.useSystemPasswordChar != EmptyChar; }
                set
                {
                    if (value)
                    {
                        this.useSystemPasswordChar = Application.RenderWithVisualStyles
                            ? VisualStylePasswordChar
                            : NonVisualStylePasswordChar;
                    }
                    else
                    {
                        this.useSystemPasswordChar = EmptyChar;
                    }

                    this.SetBasePasswordChar();
                }
            }

            public new void SelectAll()
            {
                this.BeginInvoke(
                    (MethodInvoker) delegate
                    {
                        this.Focus();
                        base.SelectAll();
                    });
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

            private void ContextMenuStripOnItemClickStart(
                object sender,
                ToolStripItemClickedEventArgs toolStripItemClickedEventArgs)
            {
                switch (toolStripItemClickedEventArgs.ClickedItem.Text)
                {
                    case "Undo":
                        this.Undo();
                        break;
                    case "Cut":
                        this.Cut();
                        break;
                    case "Copy":
                        this.Copy();
                        break;
                    case "Paste":
                        this.Paste();
                        break;
                    case "Delete":
                        this.SelectedText = string.Empty;
                        break;
                    case "Select All":
                        this.SelectAll();
                        break;
                }
            }

            private void ContextMenuStripOnOpening(object sender, CancelEventArgs cancelEventArgs)
            {
                var strip = sender as TextBoxContextMenuStrip;
                if (strip != null)
                {
                    strip.Undo.Enabled = this.CanUndo;
                    strip.Cut.Enabled = !string.IsNullOrEmpty(this.SelectedText);
                    strip.Copy.Enabled = !string.IsNullOrEmpty(this.SelectedText);
                    strip.Paste.Enabled = Clipboard.ContainsText();
                    strip.Delete.Enabled = !string.IsNullOrEmpty(this.SelectedText);
                    strip.selectAll.Enabled = !string.IsNullOrEmpty(this.Text);
                }
            }

            private void SetBasePasswordChar()
            {
                base.PasswordChar = this.UseSystemPasswordChar ? this.useSystemPasswordChar : this.passwordChar;
            }
        }

        private class TextBoxContextMenuStrip : MaterialContextMenuStrip
        {
            public readonly ToolStripItem Copy = new MaterialToolStripMenuItem {Text = "Copy"};

            public readonly ToolStripItem Cut = new MaterialToolStripMenuItem {Text = "Cut"};

            public readonly ToolStripItem Delete = new MaterialToolStripMenuItem {Text = "Delete"};

            public readonly ToolStripItem Paste = new MaterialToolStripMenuItem {Text = "Paste"};

            public readonly ToolStripItem selectAll = new MaterialToolStripMenuItem {Text = "Select All"};

            public readonly ToolStripItem Seperator1 = new ToolStripSeparator();

            public readonly ToolStripItem Seperator2 = new ToolStripSeparator();

            public readonly ToolStripItem Undo = new MaterialToolStripMenuItem {Text = "Undo"};

            public TextBoxContextMenuStrip()
            {
                this.Items.AddRange(
                    new[]
                    {
                        this.Undo, this.Seperator1, this.Cut, this.Copy, this.Paste, this.Delete, this.Seperator2,
                        this.selectAll
                    });
            }
        }
    }
}