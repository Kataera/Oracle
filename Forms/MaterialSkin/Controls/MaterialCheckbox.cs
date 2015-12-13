using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

using Tarot.Forms.MaterialSkin.Animations;

namespace Tarot.Forms.MaterialSkin.Controls
{

    #region Using Directives

    #endregion

    public class MaterialCheckBox : CheckBox, IMaterialControl
    {
        private const int CheckboxInnerBoxSize = CheckboxSize - 4;

        private const int CheckboxSize = 18;

        private const int CheckboxSizeHalf = CheckboxSize / 2;

        private const int TextOffset = 22;

        private static readonly Point[] CheckmarkLine = {new Point(3, 8), new Point(7, 12), new Point(14, 5)};

        private readonly AnimationManager animationManager;

        private readonly AnimationManager rippleAnimationManager;

        private int boxOffset;

        private Rectangle boxRectangle;

        private bool ripple;

        public MaterialCheckBox()
        {
            this.animationManager = new AnimationManager {AnimationType = AnimationType.EaseInOut, Increment = 0.05};
            this.rippleAnimationManager = new AnimationManager(false)
            {
                AnimationType = AnimationType.Linear,
                Increment = 0.10,
                SecondaryIncrement = 0.08
            };
            this.animationManager.OnAnimationProgress += sender => this.Invalidate();
            this.rippleAnimationManager.OnAnimationProgress += sender => this.Invalidate();

            this.CheckedChanged +=
                (sender, args) =>
                {
                    this.animationManager.StartNewAnimation(this.Checked ? AnimationDirection.In : AnimationDirection.Out);
                };

            this.Ripple = true;
            this.MouseLocation = new Point(-1, -1);
        }

        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set
            {
                base.AutoSize = value;
                if (value)
                {
                    this.Size = new Size(10, 10);
                }
            }
        }

        [Browsable(false)]
        public int Depth { get; set; }

        [Browsable(false)]
        public Point MouseLocation { get; set; }

        [Browsable(false)]
        public MouseState MouseState { get; set; }

        [Category("Behavior")]
        public bool Ripple
        {
            get { return this.ripple; }
            set
            {
                this.ripple = value;
                this.AutoSize = this.AutoSize; //Make AutoSize directly set the bounds.

                if (value)
                {
                    this.Margin = new Padding(0);
                }

                this.Invalidate();
            }
        }

        [Browsable(false)]
        public MaterialSkinManager SkinManager
        {
            get { return MaterialSkinManager.Instance; }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var w = this.boxOffset + CheckboxSize + 2
                    + (int) this.CreateGraphics().MeasureString(this.Text, this.SkinManager.RobotoMedium10).Width;
            return this.Ripple ? new Size(w, 30) : new Size(w, 20);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            this.Font = this.SkinManager.RobotoMedium10;

            if (this.DesignMode)
            {
                return;
            }

            this.MouseState = MouseState.Out;
            this.MouseEnter += (sender, args) => { this.MouseState = MouseState.Hover; };
            this.MouseLeave += (sender, args) =>
            {
                this.MouseLocation = new Point(-1, -1);
                this.MouseState = MouseState.Out;
            };
            this.MouseDown += (sender, args) =>
            {
                this.MouseState = MouseState.Down;

                if (this.Ripple && args.Button == MouseButtons.Left && this.IsMouseInCheckArea())
                {
                    this.rippleAnimationManager.SecondaryIncrement = 0;
                    this.rippleAnimationManager.StartNewAnimation(
                        AnimationDirection.InOutIn,
                        new object[] {this.Checked});
                }
            };
            this.MouseUp += (sender, args) =>
            {
                this.MouseState = MouseState.Hover;
                this.rippleAnimationManager.SecondaryIncrement = 0.08;
            };
            this.MouseMove += (sender, args) =>
            {
                this.MouseLocation = args.Location;
                this.Cursor = this.IsMouseInCheckArea() ? Cursors.Hand : Cursors.Default;
            };
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            // clear the control
            g.Clear(this.Parent.BackColor);

            var checkboxCenter = this.boxOffset + CheckboxSizeHalf - 1;

            var animationProgress = this.animationManager.GetProgress();

            var colorAlpha = this.Enabled
                ? (int) (animationProgress * 255.0)
                : this.SkinManager.GetCheckBoxOffDisabledColor().A;
            var backgroundAlpha = this.Enabled
                ? (int) (this.SkinManager.GetCheckboxOffColor().A * (1.0 - animationProgress))
                : this.SkinManager.GetCheckBoxOffDisabledColor().A;

            var brush =
                new SolidBrush(
                    Color.FromArgb(
                        colorAlpha, this.Enabled
                            ? this.SkinManager.ColorScheme.AccentColor
                            : this.SkinManager.GetCheckBoxOffDisabledColor()));
            var brush3 =
                new SolidBrush(this.Enabled
                    ? this.SkinManager.ColorScheme.AccentColor
                    : this.SkinManager.GetCheckBoxOffDisabledColor());
            var pen = new Pen(brush.Color);

            // draw ripple animation
            if (this.Ripple && this.rippleAnimationManager.IsAnimating())
            {
                for (var i = 0; i < this.rippleAnimationManager.GetAnimationCount(); i++)
                {
                    var animationValue = this.rippleAnimationManager.GetProgress(i);
                    var animationSource = new Point(checkboxCenter, checkboxCenter);
                    var rippleBrush =
                        new SolidBrush(
                            Color.FromArgb(
                                (int) (animationValue * 40),
                                (bool) this.rippleAnimationManager.GetData(i)[0] ? Color.Black : brush.Color));
                    var rippleHeight = this.Height % 2 == 0 ? this.Height - 3 : this.Height - 2;
                    var rippleSize = this.rippleAnimationManager.GetDirection(i) == AnimationDirection.InOutIn
                        ? (int) (rippleHeight * (0.8d + 0.2d * animationValue))
                        : rippleHeight;
                    using (
                        var path = DrawHelper.CreateRoundRect(
                            animationSource.X - rippleSize / 2,
                            animationSource.Y - rippleSize / 2,
                            rippleSize,
                            rippleSize,
                            rippleSize / 2))
                    {
                        g.FillPath(rippleBrush, path);
                    }

                    rippleBrush.Dispose();
                }
            }

            brush3.Dispose();

            var checkMarkLineFill = new Rectangle(this.boxOffset, this.boxOffset, (int) (17.0 * animationProgress), 17);
            using (var checkmarkPath = DrawHelper.CreateRoundRect(this.boxOffset, this.boxOffset, 17, 17, 1f))
            {
                var brush2 =
                    new SolidBrush(
                        DrawHelper.BlendColor(this.Parent.BackColor, this.Enabled
                            ? this.SkinManager.GetCheckboxOffColor()
                            : this.SkinManager.GetCheckBoxOffDisabledColor(),
                            backgroundAlpha));
                var pen2 = new Pen(brush2.Color);
                g.FillPath(brush2, checkmarkPath);
                g.DrawPath(pen2, checkmarkPath);

                g.FillRectangle(
                    new SolidBrush(this.Parent.BackColor), this.boxOffset + 2, this.boxOffset + 2,
                    CheckboxInnerBoxSize - 1,
                    CheckboxInnerBoxSize - 1);
                g.DrawRectangle(
                    new Pen(this.Parent.BackColor), this.boxOffset + 2, this.boxOffset + 2,
                    CheckboxInnerBoxSize - 1,
                    CheckboxInnerBoxSize - 1);

                brush2.Dispose();
                pen2.Dispose();

                if (this.Enabled)
                {
                    g.FillPath(brush, checkmarkPath);
                    g.DrawPath(pen, checkmarkPath);
                }
                else if (this.Checked)
                {
                    g.SmoothingMode = SmoothingMode.None;
                    g.FillRectangle(
                        brush, this.boxOffset + 2, this.boxOffset + 2,
                        CheckboxInnerBoxSize,
                        CheckboxInnerBoxSize);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                }

                g.DrawImageUnscaledAndClipped(this.DrawCheckMarkBitmap(), checkMarkLineFill);
            }

            // draw checkbox text
            var stringSize = g.MeasureString(this.Text, this.SkinManager.RobotoMedium10);
            g.DrawString(this.Text, this.SkinManager.RobotoMedium10,
                this.Enabled ? this.SkinManager.GetPrimaryTextBrush() : this.SkinManager.GetDisabledOrHintBrush(),
                this.boxOffset + TextOffset, this.Height / 2 - stringSize.Height / 2);

            // dispose used paint objects
            pen.Dispose();
            brush.Dispose();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            this.boxOffset = this.Height / 2 - 9;
            this.boxRectangle = new Rectangle(this.boxOffset, this.boxOffset, CheckboxSize - 1, CheckboxSize - 1);
        }

        private Bitmap DrawCheckMarkBitmap()
        {
            var checkMark = new Bitmap(CheckboxSize, CheckboxSize);
            var g = Graphics.FromImage(checkMark);

            // clear everything, transparant
            g.Clear(Color.Transparent);

            // draw the checkmark lines
            using (var pen = new Pen(this.Parent.BackColor, 2))
            {
                g.DrawLines(pen, CheckmarkLine);
            }

            return checkMark;
        }

        private bool IsMouseInCheckArea()
        {
            return this.boxRectangle.Contains(this.MouseLocation);
        }
    }
}