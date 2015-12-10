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

    public class MaterialFlatButton : Button, IMaterialControl
    {
        private readonly AnimationManager animationManager;

        private readonly AnimationManager hoverAnimationManager;

        private SizeF textSize;

        public MaterialFlatButton()
        {
            this.Primary = false;

            this.animationManager = new AnimationManager(false)
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            this.hoverAnimationManager = new AnimationManager {Increment = 0.07, AnimationType = AnimationType.Linear};

            this.hoverAnimationManager.OnAnimationProgress += sender => this.Invalidate();
            this.animationManager.OnAnimationProgress += sender => this.Invalidate();

            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            //AutoSize = true;
            this.Margin = new Padding(4, 6, 4, 6);
            this.Padding = new Padding(0);
        }

        [Browsable(false)]
        public int Depth { get; set; }

        [Browsable(false)]
        public MouseState MouseState { get; set; }

        public bool Primary { get; set; }

        [Browsable(false)]
        public MaterialSkinManager SkinManager
        {
            get { return MaterialSkinManager.Instance; }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                this.textSize = this.CreateGraphics().MeasureString(value.ToUpper(), this.SkinManager.RobotoMedium10);
                if (this.AutoSize)
                {
                    this.Size = this.GetPreferredSize();
                }
                this.Invalidate();
            }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return new Size((int) this.textSize.Width + 8, 36);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            if (this.DesignMode)
            {
                return;
            }

            this.MouseState = MouseState.Out;
            this.MouseEnter += (sender, args) =>
            {
                this.MouseState = MouseState.Hover;
                this.hoverAnimationManager.StartNewAnimation(AnimationDirection.In);
                this.Invalidate();
            };
            this.MouseLeave += (sender, args) =>
            {
                this.MouseState = MouseState.Out;
                this.hoverAnimationManager.StartNewAnimation(AnimationDirection.Out);
                this.Invalidate();
            };
            this.MouseDown += (sender, args) =>
            {
                if (args.Button == MouseButtons.Left)
                {
                    this.MouseState = MouseState.Down;

                    this.animationManager.StartNewAnimation(AnimationDirection.In, args.Location);
                    this.Invalidate();
                }
            };
            this.MouseUp += (sender, args) =>
            {
                this.MouseState = MouseState.Hover;

                this.Invalidate();
            };
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(this.Parent.BackColor);

            //Hover
            var c = this.SkinManager.GetFlatButtonHoverBackgroundColor();
            using (
                Brush b =
                    new SolidBrush(
                        Color.FromArgb((int) (this.hoverAnimationManager.GetProgress() * c.A), c.RemoveAlpha())))
            {
                g.FillRectangle(b, this.ClientRectangle);
            }

            //Ripple
            if (this.animationManager.IsAnimating())
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                for (var i = 0; i < this.animationManager.GetAnimationCount(); i++)
                {
                    var animationValue = this.animationManager.GetProgress(i);
                    var animationSource = this.animationManager.GetSource(i);

                    using (
                        Brush rippleBrush =
                            new SolidBrush(Color.FromArgb((int) (101 - animationValue * 100), Color.Black)))
                    {
                        var rippleSize = (int) (animationValue * this.Width * 2);
                        g.FillEllipse(
                            rippleBrush,
                            new Rectangle(
                                animationSource.X - rippleSize / 2,
                                animationSource.Y - rippleSize / 2,
                                rippleSize,
                                rippleSize));
                    }
                }
                g.SmoothingMode = SmoothingMode.None;
            }
            g.DrawString(this.Text.ToUpper(), this.SkinManager.RobotoMedium10, this.Enabled
                ? (this.Primary ? this.SkinManager.ColorScheme.PrimaryBrush : this.SkinManager.GetPrimaryTextBrush())
                : this.SkinManager.GetFlatButtonDisabledTextBrush(), this.ClientRectangle,
                new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center});
        }

        private Size GetPreferredSize()
        {
            return this.GetPreferredSize(new Size(0, 0));
        }
    }
}