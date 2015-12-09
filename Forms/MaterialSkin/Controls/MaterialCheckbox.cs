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

        private const int CheckboxSizeHalf = CheckboxSize/2;

        private const int TextOffset = 22;

        private static readonly Point[] CheckmarkLine = {new Point(3, 8), new Point(7, 12), new Point(14, 5)};

        private readonly AnimationManager animationManager;

        private readonly AnimationManager rippleAnimationManager;

        private int boxOffset;

        private Rectangle boxRectangle;

        private bool ripple;

        public MaterialCheckBox()
        {
            animationManager = new AnimationManager {AnimationType = AnimationType.EaseInOut, Increment = 0.05};
            rippleAnimationManager = new AnimationManager(false)
            {
                AnimationType = AnimationType.Linear,
                Increment = 0.10,
                SecondaryIncrement = 0.08
            };
            animationManager.OnAnimationProgress += sender => Invalidate();
            rippleAnimationManager.OnAnimationProgress += sender => Invalidate();

            CheckedChanged +=
                (sender, args) =>
                {
                    animationManager.StartNewAnimation(
                        Checked ? AnimationDirection.In : AnimationDirection.Out);
                };

            Ripple = true;
            MouseLocation = new Point(-1, -1);
        }

        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set
            {
                base.AutoSize = value;
                if (value)
                {
                    Size = new Size(10, 10);
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
            get { return ripple; }
            set
            {
                ripple = value;
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
            var w = boxOffset + CheckboxSize + 2
                    + (int) CreateGraphics().MeasureString(Text, SkinManager.RobotoMedium10).Width;
            return Ripple ? new Size(w, 30) : new Size(w, 20);
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
                    rippleAnimationManager.SecondaryIncrement = 0;
                    rippleAnimationManager.StartNewAnimation(
                        AnimationDirection.InOutIn,
                        new object[] {Checked});
                }
            };
            MouseUp += (sender, args) =>
            {
                MouseState = MouseState.Hover;
                rippleAnimationManager.SecondaryIncrement = 0.08;
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

            var checkboxCenter = boxOffset + CheckboxSizeHalf - 1;

            var animationProgress = animationManager.GetProgress();

            var colorAlpha = Enabled
                ? (int) (animationProgress*255.0)
                : SkinManager.GetCheckBoxOffDisabledColor().A;
            var backgroundAlpha = Enabled
                ? (int) (SkinManager.GetCheckboxOffColor().A*(1.0 - animationProgress))
                : SkinManager.GetCheckBoxOffDisabledColor().A;

            var brush =
                new SolidBrush(
                    Color.FromArgb(
                        colorAlpha,
                        Enabled
                            ? SkinManager.ColorScheme.AccentColor
                            : SkinManager.GetCheckBoxOffDisabledColor()));
            var brush3 =
                new SolidBrush(
                    Enabled
                        ? SkinManager.ColorScheme.AccentColor
                        : SkinManager.GetCheckBoxOffDisabledColor());
            var pen = new Pen(brush.Color);

            // draw ripple animation
            if (Ripple && rippleAnimationManager.IsAnimating())
            {
                for (var i = 0; i < rippleAnimationManager.GetAnimationCount(); i++)
                {
                    var animationValue = rippleAnimationManager.GetProgress(i);
                    var animationSource = new Point(checkboxCenter, checkboxCenter);
                    var rippleBrush =
                        new SolidBrush(
                            Color.FromArgb(
                                (int) (animationValue*40),
                                (bool) rippleAnimationManager.GetData(i)[0] ? Color.Black : brush.Color));
                    var rippleHeight = Height%2 == 0 ? Height - 3 : Height - 2;
                    var rippleSize = rippleAnimationManager.GetDirection(i) == AnimationDirection.InOutIn
                        ? (int) (rippleHeight*(0.8d + 0.2d*animationValue))
                        : rippleHeight;
                    using (
                        var path = DrawHelper.CreateRoundRect(
                            animationSource.X - rippleSize/2,
                            animationSource.Y - rippleSize/2,
                            rippleSize,
                            rippleSize,
                            rippleSize/2))
                    {
                        g.FillPath(rippleBrush, path);
                    }

                    rippleBrush.Dispose();
                }
            }

            brush3.Dispose();

            var checkMarkLineFill = new Rectangle(boxOffset, boxOffset, (int) (17.0*animationProgress), 17);
            using (var checkmarkPath = DrawHelper.CreateRoundRect(boxOffset, boxOffset, 17, 17, 1f))
            {
                var brush2 =
                    new SolidBrush(
                        DrawHelper.BlendColor(
                            Parent.BackColor,
                            Enabled
                                ? SkinManager.GetCheckboxOffColor()
                                : SkinManager.GetCheckBoxOffDisabledColor(),
                            backgroundAlpha));
                var pen2 = new Pen(brush2.Color);
                g.FillPath(brush2, checkmarkPath);
                g.DrawPath(pen2, checkmarkPath);

                g.FillRectangle(
                    new SolidBrush(Parent.BackColor),
                    boxOffset + 2,
                    boxOffset + 2,
                    CheckboxInnerBoxSize - 1,
                    CheckboxInnerBoxSize - 1);
                g.DrawRectangle(
                    new Pen(Parent.BackColor),
                    boxOffset + 2,
                    boxOffset + 2,
                    CheckboxInnerBoxSize - 1,
                    CheckboxInnerBoxSize - 1);

                brush2.Dispose();
                pen2.Dispose();

                if (Enabled)
                {
                    g.FillPath(brush, checkmarkPath);
                    g.DrawPath(pen, checkmarkPath);
                }
                else if (Checked)
                {
                    g.SmoothingMode = SmoothingMode.None;
                    g.FillRectangle(
                        brush,
                        boxOffset + 2,
                        boxOffset + 2,
                        CheckboxInnerBoxSize,
                        CheckboxInnerBoxSize);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                }

                g.DrawImageUnscaledAndClipped(DrawCheckMarkBitmap(), checkMarkLineFill);
            }

            // draw checkbox text
            var stringSize = g.MeasureString(Text, SkinManager.RobotoMedium10);
            g.DrawString(
                Text,
                SkinManager.RobotoMedium10,
                Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush(),
                boxOffset + TextOffset,
                Height/2 - stringSize.Height/2);

            // dispose used paint objects
            pen.Dispose();
            brush.Dispose();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            boxOffset = Height/2 - 9;
            boxRectangle = new Rectangle(boxOffset, boxOffset, CheckboxSize - 1, CheckboxSize - 1);
        }

        private Bitmap DrawCheckMarkBitmap()
        {
            var checkMark = new Bitmap(CheckboxSize, CheckboxSize);
            var g = Graphics.FromImage(checkMark);

            // clear everything, transparant
            g.Clear(Color.Transparent);

            // draw the checkmark lines
            using (var pen = new Pen(Parent.BackColor, 2))
            {
                g.DrawLines(pen, CheckmarkLine);
            }

            return checkMark;
        }

        private bool IsMouseInCheckArea()
        {
            return boxRectangle.Contains(MouseLocation);
        }
    }
}