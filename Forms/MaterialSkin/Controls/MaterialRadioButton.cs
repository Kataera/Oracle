using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

using MaterialSkin.Animations;

// ReSharper disable CheckNamespace

namespace MaterialSkin.Controls
{

    #region Using Directives

    #endregion

    public class MaterialRadioButton : RadioButton, IMaterialControl
    {
        private const int RadiobuttonInnerCircleSize = RadiobuttonSize - 2 * RadiobuttonOuterCircleWidth;

        private const int RadiobuttonOuterCircleWidth = 2;

        // size constants
        private const int RadiobuttonSize = 19;

        private const int RadiobuttonSizeHalf = RadiobuttonSize / 2;

        // animation managers
        private readonly AnimationManager animationManager;

        private readonly AnimationManager rippleAnimationManager;

        private int boxOffset;

        // size related variables which should be recalculated onsizechanged
        private Rectangle radioButtonBounds;

        private bool ripple;

        public MaterialRadioButton()
        {
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            this.animationManager = new AnimationManager {AnimationType = AnimationType.EaseInOut, Increment = 0.06};
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
                    this.animationManager.StartNewAnimation(this.Checked ? AnimationDirection.In : AnimationDirection.Out);

            this.SizeChanged += this.OnSizeChanged;

            this.Ripple = true;
            this.MouseLocation = new Point(-1, -1);
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
            var width = this.boxOffset + 20
                        + (int) this.CreateGraphics().MeasureString(this.Text, this.SkinManager.RobotoMedium10).Width;
            return this.Ripple ? new Size(width, 30) : new Size(width, 20);
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

            var radiobuttonCenter = this.boxOffset + RadiobuttonSizeHalf;

            var animationProgress = this.animationManager.GetProgress();

            var colorAlpha = this.Enabled
                ? (int) (animationProgress * 255.0)
                : this.SkinManager.GetCheckBoxOffDisabledColor().A;
            var backgroundAlpha = this.Enabled
                ? (int) (this.SkinManager.GetCheckboxOffColor().A * (1.0 - animationProgress))
                : this.SkinManager.GetCheckBoxOffDisabledColor().A;
            var animationSize = (float) (animationProgress * 8f);
            var animationSizeHalf = animationSize / 2;
            animationSize = (float) (animationProgress * 9f);

            var brush =
                new SolidBrush(
                    Color.FromArgb(
                        colorAlpha, this.Enabled
                            ? this.SkinManager.ColorScheme.AccentColor
                            : this.SkinManager.GetCheckBoxOffDisabledColor()));
            var pen = new Pen(brush.Color);

            // draw ripple animation
            if (this.Ripple && this.rippleAnimationManager.IsAnimating())
            {
                for (var i = 0; i < this.rippleAnimationManager.GetAnimationCount(); i++)
                {
                    var animationValue = this.rippleAnimationManager.GetProgress(i);
                    var animationSource = new Point(radiobuttonCenter, radiobuttonCenter);
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

            // draw radiobutton circle
            var uncheckedColor = DrawHelper.BlendColor(this.Parent.BackColor,
                this.Enabled ? this.SkinManager.GetCheckboxOffColor() : this.SkinManager.GetCheckBoxOffDisabledColor(),
                backgroundAlpha);

            using (
                var path = DrawHelper.CreateRoundRect(this.boxOffset, this.boxOffset,
                    RadiobuttonSize,
                    RadiobuttonSize,
                    9f))
            {
                g.FillPath(new SolidBrush(uncheckedColor), path);

                if (this.Enabled)
                {
                    g.FillPath(brush, path);
                }
            }

            g.FillEllipse(
                new SolidBrush(this.Parent.BackColor),
                RadiobuttonOuterCircleWidth + this.boxOffset,
                RadiobuttonOuterCircleWidth + this.boxOffset,
                RadiobuttonInnerCircleSize,
                RadiobuttonInnerCircleSize);

            if (this.Checked)
            {
                using (
                    var path = DrawHelper.CreateRoundRect(
                        radiobuttonCenter - animationSizeHalf,
                        radiobuttonCenter - animationSizeHalf,
                        animationSize,
                        animationSize,
                        4f))
                {
                    g.FillPath(brush, path);
                }
            }
            var stringSize = g.MeasureString(this.Text, this.SkinManager.RobotoMedium10);
            g.DrawString(this.Text, this.SkinManager.RobotoMedium10,
                this.Enabled ? this.SkinManager.GetPrimaryTextBrush() : this.SkinManager.GetDisabledOrHintBrush(), this.boxOffset + 22,
                this.Height / 2 - stringSize.Height / 2);

            brush.Dispose();
            pen.Dispose();
        }

        private bool IsMouseInCheckArea()
        {
            return this.radioButtonBounds.Contains(this.MouseLocation);
        }

        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            this.boxOffset = this.Height / 2 - (int) Math.Ceiling(RadiobuttonSize / 2d);
            this.radioButtonBounds = new Rectangle(this.boxOffset, this.boxOffset, RadiobuttonSize, RadiobuttonSize);
        }
    }
}