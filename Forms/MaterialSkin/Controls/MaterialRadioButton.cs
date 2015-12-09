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
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            this.animationManager = new AnimationManager {AnimationType = AnimationType.EaseInOut, Increment = 0.06};
            this.rippleAnimationManager = new AnimationManager(false)
            {
                AnimationType = AnimationType.Linear,
                Increment = 0.10,
                SecondaryIncrement = 0.08
            };
            this.animationManager.OnAnimationProgress += sender => Invalidate();
            this.rippleAnimationManager.OnAnimationProgress += sender => Invalidate();

            CheckedChanged +=
                (sender, args) =>
                    this.animationManager.StartNewAnimation(Checked ? AnimationDirection.In : AnimationDirection.Out);

            SizeChanged += OnSizeChanged;

            Ripple = true;
            MouseLocation = new Point(-1, -1);
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
                AutoSize = AutoSize; //Make AutoSize directly set the bounds.

                if (value)
                {
                    Margin = new Padding(0);
                }

                Invalidate();
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
                        + (int) CreateGraphics().MeasureString(Text, SkinManager.RobotoMedium10).Width;
            return Ripple ? new Size(width, 30) : new Size(width, 20);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            Font = SkinManager.RobotoMedium10;

            if (DesignMode)
            {
                return;
            }

            MouseState = MouseState.Out;
            MouseEnter += (sender, args) => { MouseState = MouseState.Hover; };
            MouseLeave += (sender, args) =>
            {
                MouseLocation = new Point(-1, -1);
                MouseState = MouseState.Out;
            };
            MouseDown += (sender, args) =>
            {
                MouseState = MouseState.Down;

                if (Ripple && args.Button == MouseButtons.Left && IsMouseInCheckArea())
                {
                    this.rippleAnimationManager.SecondaryIncrement = 0;
                    this.rippleAnimationManager.StartNewAnimation(
                        AnimationDirection.InOutIn,
                        new object[] {Checked});
                }
            };
            MouseUp += (sender, args) =>
            {
                MouseState = MouseState.Hover;
                this.rippleAnimationManager.SecondaryIncrement = 0.08;
            };
            MouseMove += (sender, args) =>
            {
                MouseLocation = args.Location;
                Cursor = IsMouseInCheckArea() ? Cursors.Hand : Cursors.Default;
            };
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            // clear the control
            g.Clear(Parent.BackColor);

            var radiobuttonCenter = this.boxOffset + RadiobuttonSizeHalf;

            var animationProgress = this.animationManager.GetProgress();

            var colorAlpha = Enabled
                ? (int) (animationProgress * 255.0)
                : SkinManager.GetCheckBoxOffDisabledColor().A;
            var backgroundAlpha = Enabled
                ? (int) (SkinManager.GetCheckboxOffColor().A * (1.0 - animationProgress))
                : SkinManager.GetCheckBoxOffDisabledColor().A;
            var animationSize = (float) (animationProgress * 8f);
            var animationSizeHalf = animationSize / 2;
            animationSize = (float) (animationProgress * 9f);

            var brush =
                new SolidBrush(
                    Color.FromArgb(
                        colorAlpha,
                        Enabled
                            ? SkinManager.ColorScheme.AccentColor
                            : SkinManager.GetCheckBoxOffDisabledColor()));
            var pen = new Pen(brush.Color);

            // draw ripple animation
            if (Ripple && this.rippleAnimationManager.IsAnimating())
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
                    var rippleHeight = Height % 2 == 0 ? Height - 3 : Height - 2;
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
            var uncheckedColor = DrawHelper.BlendColor(
                Parent.BackColor,
                Enabled ? SkinManager.GetCheckboxOffColor() : SkinManager.GetCheckBoxOffDisabledColor(),
                backgroundAlpha);

            using (
                var path = DrawHelper.CreateRoundRect(this.boxOffset, this.boxOffset,
                    RadiobuttonSize,
                    RadiobuttonSize,
                    9f))
            {
                g.FillPath(new SolidBrush(uncheckedColor), path);

                if (Enabled)
                {
                    g.FillPath(brush, path);
                }
            }

            g.FillEllipse(
                new SolidBrush(Parent.BackColor),
                RadiobuttonOuterCircleWidth + this.boxOffset,
                RadiobuttonOuterCircleWidth + this.boxOffset,
                RadiobuttonInnerCircleSize,
                RadiobuttonInnerCircleSize);

            if (Checked)
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
            var stringSize = g.MeasureString(Text, SkinManager.RobotoMedium10);
            g.DrawString(
                Text,
                SkinManager.RobotoMedium10,
                Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush(), this.boxOffset + 22,
                Height / 2 - stringSize.Height / 2);

            brush.Dispose();
            pen.Dispose();
        }

        private bool IsMouseInCheckArea()
        {
            return this.radioButtonBounds.Contains(MouseLocation);
        }

        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            this.boxOffset = Height / 2 - (int) Math.Ceiling(RadiobuttonSize / 2d);
            this.radioButtonBounds = new Rectangle(this.boxOffset, this.boxOffset, RadiobuttonSize, RadiobuttonSize);
        }
    }
}