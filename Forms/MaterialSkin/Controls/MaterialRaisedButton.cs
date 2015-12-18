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

    public class MaterialRaisedButton : Button, IMaterialControl
    {
        private readonly AnimationManager animationManager;

        public MaterialRaisedButton()
        {
            this.Primary = true;

            this.animationManager = new AnimationManager(false)
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            this.animationManager.OnAnimationProgress += sender => this.Invalidate();
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

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            this.animationManager.StartNewAnimation(AnimationDirection.In, mevent.Location);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(this.Parent.BackColor);

            using (
                var backgroundPath = DrawHelper.CreateRoundRect(this.ClientRectangle.X, this.ClientRectangle.Y,
                    this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1,
                    1f))
            {
                g.FillPath(this.Primary
                    ? this.SkinManager.ColorScheme.PrimaryBrush
                    : this.SkinManager.GetRaisedButtonBackgroundBrush(),
                    backgroundPath);
            }

            if (this.animationManager.IsAnimating())
            {
                for (var i = 0; i < this.animationManager.GetAnimationCount(); i++)
                {
                    var animationValue = this.animationManager.GetProgress(i);
                    var animationSource = this.animationManager.GetSource(i);
                    var rippleBrush = new SolidBrush(Color.FromArgb((int) (51 - animationValue * 50), Color.White));
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

            g.DrawString(this.Text.ToUpper(), this.SkinManager.RobotoMedium10, this.SkinManager.GetRaisedButtonTextBrush(this.Primary),
                this.ClientRectangle,
                new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center});
        }
    }
}