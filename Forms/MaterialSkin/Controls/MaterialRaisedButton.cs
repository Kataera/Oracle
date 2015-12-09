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

    public class MaterialRaisedButton : Button, IMaterialControl
    {
        private readonly AnimationManager animationManager;

        public MaterialRaisedButton()
        {
            Primary = true;

            this.animationManager = new AnimationManager(false)
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            this.animationManager.OnAnimationProgress += sender => Invalidate();
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

            g.Clear(Parent.BackColor);

            using (
                var backgroundPath = DrawHelper.CreateRoundRect(
                    ClientRectangle.X,
                    ClientRectangle.Y,
                    ClientRectangle.Width - 1,
                    ClientRectangle.Height - 1,
                    1f))
            {
                g.FillPath(
                    Primary
                        ? SkinManager.ColorScheme.PrimaryBrush
                        : SkinManager.GetRaisedButtonBackgroundBrush(),
                    backgroundPath);
            }

            if (this.animationManager.IsAnimating())
            {
                for (var i = 0; i < this.animationManager.GetAnimationCount(); i++)
                {
                    var animationValue = this.animationManager.GetProgress(i);
                    var animationSource = this.animationManager.GetSource(i);
                    var rippleBrush = new SolidBrush(Color.FromArgb((int) (51 - animationValue * 50), Color.White));
                    var rippleSize = (int) (animationValue * Width * 2);
                    g.FillEllipse(
                        rippleBrush,
                        new Rectangle(
                            animationSource.X - rippleSize / 2,
                            animationSource.Y - rippleSize / 2,
                            rippleSize,
                            rippleSize));
                }
            }

            g.DrawString(
                Text.ToUpper(),
                SkinManager.RobotoMedium10,
                SkinManager.GetRaisedButtonTextBrush(Primary),
                ClientRectangle,
                new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center});
        }
    }
}