using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

using Oracle.Forms.MaterialSkin.Animations;

namespace Oracle.Forms.MaterialSkin.Controls
{

    #region Using Directives

    #endregion

    public class MaterialTabSelectorVertical : Control, IMaterialControl
    {
        private const int TabHeaderPadding = 24;

        private const int TabIndicatorWidth = 4;

        private readonly AnimationManager animationManager;

        private Point animationSource;

        private MaterialTabControl baseTabControl;

        private int previousSelectedTabIndex;

        private List<Rectangle> tabRects;

        public MaterialTabSelectorVertical()
        {
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            this.Height = 48;

            this.animationManager = new AnimationManager {AnimationType = AnimationType.EaseOut, Increment = 0.04};
            this.animationManager.OnAnimationProgress += sender => this.Invalidate();
        }

        public MaterialTabControl BaseTabControl
        {
            get { return this.baseTabControl; }
            set
            {
                this.baseTabControl = value;
                if (this.baseTabControl == null)
                {
                    return;
                }
                this.previousSelectedTabIndex = this.baseTabControl.SelectedIndex;
                this.baseTabControl.Deselected +=
                    (sender, args) => { this.previousSelectedTabIndex = this.baseTabControl.SelectedIndex; };
                this.baseTabControl.SelectedIndexChanged += (sender, args) =>
                {
                    this.animationManager.SetProgress(0);
                    this.animationManager.StartNewAnimation(AnimationDirection.In);
                };
                this.baseTabControl.ControlAdded += delegate { this.Invalidate(); };
                this.baseTabControl.ControlRemoved += delegate { this.Invalidate(); };
            }
        }

        [Browsable(false)]
        public int Depth { get; set; }

        [Browsable(false)]
        public MouseState MouseState { get; set; }

        [Browsable(false)]
        public MaterialSkinManager SkinManager
        {
            get { return MaterialSkinManager.Instance; }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (this.tabRects == null)
            {
                this.UpdateTabRects();
            }
            for (var i = 0; i < this.tabRects.Count; i++)
            {
                if (this.tabRects[i].Contains(e.Location))
                {
                    this.baseTabControl.SelectedIndex = i;
                }
            }

            this.animationSource = e.Location;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(this.SkinManager.ColorScheme.PrimaryColor);

            if (this.baseTabControl == null)
            {
                return;
            }

            if (!this.animationManager.IsAnimating() || this.tabRects == null
                || this.tabRects.Count != this.baseTabControl.TabCount)
            {
                this.UpdateTabRects();
            }

            var animationProgress = this.animationManager.GetProgress();

            //Click feedback
            if (this.animationManager.IsAnimating())
            {
                var rippleBrush = new SolidBrush(Color.FromArgb((int) (51 - animationProgress * 50), Color.White));
                var rippleSize =
                    (int) (animationProgress * this.tabRects[this.baseTabControl.SelectedIndex].Width * 1.75);

                g.SetClip(this.tabRects[this.baseTabControl.SelectedIndex]);
                g.FillEllipse(
                    rippleBrush,
                    new Rectangle(this.animationSource.X - rippleSize / 2, this.animationSource.Y - rippleSize / 2,
                        rippleSize,
                        rippleSize));
                g.ResetClip();
                rippleBrush.Dispose();
            }

            //Draw tab headers
            foreach (TabPage tabPage in this.baseTabControl.TabPages)
            {
                var currentTabIndex = tabPage.TabIndex;
                Brush textBrush =
                    new SolidBrush(
                        Color.FromArgb(this.CalculateTextAlpha(currentTabIndex, animationProgress), this.SkinManager.ColorScheme.TextColor));

                g.DrawString(
                    tabPage.Text.ToUpper(), this.SkinManager.RobotoMedium10,
                    textBrush, this.tabRects[currentTabIndex],
                    new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center});
                textBrush.Dispose();
            }

            //Animate tab indicator
            var previousSelectedTabIndexIfHasOne = this.previousSelectedTabIndex == -1
                ? this.baseTabControl.SelectedIndex
                : this.previousSelectedTabIndex;
            var previousActiveTabRect = this.tabRects[previousSelectedTabIndexIfHasOne];
            var activeTabPageRect = this.tabRects[this.baseTabControl.SelectedIndex];

            var x = activeTabPageRect.Right - 4;
            var y = previousActiveTabRect.Y
                    + (int) ((activeTabPageRect.Y - previousActiveTabRect.Y) * animationProgress);
            var height = previousActiveTabRect.Height
                         + (int) ((activeTabPageRect.Height - previousActiveTabRect.Height) * animationProgress);

            g.FillRectangle(this.SkinManager.ColorScheme.AccentBrush, x, y, TabIndicatorWidth, height);
        }

        private int CalculateTextAlpha(int tabIndex, double animationProgress)
        {
            int primaryA = this.SkinManager.ActionBarText.A;
            int secondaryA = this.SkinManager.ActionBarTextSecondary.A;

            if (tabIndex == this.baseTabControl.SelectedIndex && !this.animationManager.IsAnimating())
            {
                return primaryA;
            }
            if (tabIndex != this.previousSelectedTabIndex && tabIndex != this.baseTabControl.SelectedIndex)
            {
                return secondaryA;
            }
            if (tabIndex == this.previousSelectedTabIndex)
            {
                return primaryA - (int) ((primaryA - secondaryA) * animationProgress);
            }
            return secondaryA + (int) ((primaryA - secondaryA) * animationProgress);
        }

        private void UpdateTabRects()
        {
            this.tabRects = new List<Rectangle>();

            //If there isn't a base tab control, the rects shouldn't be calculated
            //If there aren't tab pages in the base tab control, the list should just be empty which has been set already; exit the void
            if (this.baseTabControl == null || this.baseTabControl.TabCount == 0)
            {
                return;
            }

            //Caluclate the bounds of each tab header specified in the base tab control
            using (var b = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(b))
                {
                    this.tabRects.Add(
                        new Rectangle(
                            0,
                            0, this.Width,
                            TabHeaderPadding * 2
                            + (int)
                                g.MeasureString(this.baseTabControl.TabPages[0].Text, this.SkinManager.RobotoMedium10)
                                 .Height));
                    for (var i = 1; i < this.baseTabControl.TabPages.Count; i++)
                    {
                        this.tabRects.Add(
                            new Rectangle(
                                0, this.tabRects[i - 1].Bottom, this.Width,
                                TabHeaderPadding * 2
                                + (int)
                                    g.MeasureString(this.baseTabControl.TabPages[0].Text, this.SkinManager.RobotoMedium10)
                                     .Height));
                    }
                }
            }
        }
    }
}