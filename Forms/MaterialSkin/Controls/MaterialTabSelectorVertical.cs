using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using Tarot.Forms.MaterialSkin.Animations;

namespace Tarot.Forms.MaterialSkin.Controls
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
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            Height = 48;

            animationManager = new AnimationManager {AnimationType = AnimationType.EaseOut, Increment = 0.04};
            animationManager.OnAnimationProgress += sender => Invalidate();
        }

        public MaterialTabControl BaseTabControl
        {
            get { return baseTabControl; }
            set
            {
                baseTabControl = value;
                if (baseTabControl == null)
                {
                    return;
                }
                previousSelectedTabIndex = baseTabControl.SelectedIndex;
                baseTabControl.Deselected +=
                    (sender, args) => { previousSelectedTabIndex = baseTabControl.SelectedIndex; };
                baseTabControl.SelectedIndexChanged += (sender, args) =>
                {
                    animationManager.SetProgress(0);
                    animationManager.StartNewAnimation(AnimationDirection.In);
                };
                baseTabControl.ControlAdded += delegate { Invalidate(); };
                baseTabControl.ControlRemoved += delegate { Invalidate(); };
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

            if (tabRects == null)
            {
                UpdateTabRects();
            }
            for (var i = 0; i < tabRects.Count; i++)
            {
                if (tabRects[i].Contains(e.Location))
                {
                    baseTabControl.SelectedIndex = i;
                }
            }

            animationSource = e.Location;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            g.Clear(SkinManager.ColorScheme.PrimaryColor);

            if (baseTabControl == null)
            {
                return;
            }

            if (!animationManager.IsAnimating() || tabRects == null
                || tabRects.Count != baseTabControl.TabCount)
            {
                UpdateTabRects();
            }

            var animationProgress = animationManager.GetProgress();

            //Click feedback
            if (animationManager.IsAnimating())
            {
                var rippleBrush = new SolidBrush(Color.FromArgb((int) (51 - animationProgress*50), Color.White));
                var rippleSize =
                    (int) (animationProgress*tabRects[baseTabControl.SelectedIndex].Width*1.75);

                g.SetClip(tabRects[baseTabControl.SelectedIndex]);
                g.FillEllipse(
                    rippleBrush,
                    new Rectangle(
                        animationSource.X - rippleSize/2,
                        animationSource.Y - rippleSize/2,
                        rippleSize,
                        rippleSize));
                g.ResetClip();
                rippleBrush.Dispose();
            }

            //Draw tab headers
            foreach (TabPage tabPage in baseTabControl.TabPages)
            {
                var currentTabIndex = tabPage.TabIndex;
                Brush textBrush =
                    new SolidBrush(
                        Color.FromArgb(
                            CalculateTextAlpha(currentTabIndex, animationProgress),
                            SkinManager.ColorScheme.TextColor));

                g.DrawString(
                    tabPage.Text.ToUpper(),
                    SkinManager.RobotoMedium10,
                    textBrush,
                    tabRects[currentTabIndex],
                    new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center});
                textBrush.Dispose();
            }

            //Animate tab indicator
            var previousSelectedTabIndexIfHasOne = previousSelectedTabIndex == -1
                ? baseTabControl.SelectedIndex
                : previousSelectedTabIndex;
            var previousActiveTabRect = tabRects[previousSelectedTabIndexIfHasOne];
            var activeTabPageRect = tabRects[baseTabControl.SelectedIndex];

            var x = activeTabPageRect.Right - 4;
            var y = previousActiveTabRect.Y
                    + (int) ((activeTabPageRect.Y - previousActiveTabRect.Y)*animationProgress);
            var height = previousActiveTabRect.Height
                         + (int) ((activeTabPageRect.Height - previousActiveTabRect.Height)*animationProgress);

            g.FillRectangle(SkinManager.ColorScheme.AccentBrush, x, y, TabIndicatorWidth, height);
        }

        private int CalculateTextAlpha(int tabIndex, double animationProgress)
        {
            int primaryA = SkinManager.ActionBarText.A;
            int secondaryA = SkinManager.ActionBarTextSecondary.A;

            if (tabIndex == baseTabControl.SelectedIndex && !animationManager.IsAnimating())
            {
                return primaryA;
            }
            if (tabIndex != previousSelectedTabIndex && tabIndex != baseTabControl.SelectedIndex)
            {
                return secondaryA;
            }
            if (tabIndex == previousSelectedTabIndex)
            {
                return primaryA - (int) ((primaryA - secondaryA)*animationProgress);
            }
            return secondaryA + (int) ((primaryA - secondaryA)*animationProgress);
        }

        private void UpdateTabRects()
        {
            tabRects = new List<Rectangle>();

            //If there isn't a base tab control, the rects shouldn't be calculated
            //If there aren't tab pages in the base tab control, the list should just be empty which has been set already; exit the void
            if (baseTabControl == null || baseTabControl.TabCount == 0)
            {
                return;
            }

            //Caluclate the bounds of each tab header specified in the base tab control
            using (var b = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(b))
                {
                    tabRects.Add(
                        new Rectangle(
                            0,
                            0,
                            Width,
                            TabHeaderPadding*2
                            + (int)
                                g.MeasureString(baseTabControl.TabPages[0].Text, SkinManager.RobotoMedium10)
                                    .Height));
                    for (var i = 1; i < baseTabControl.TabPages.Count; i++)
                    {
                        tabRects.Add(
                            new Rectangle(
                                0,
                                tabRects[i - 1].Bottom,
                                Width,
                                TabHeaderPadding*2
                                + (int)
                                    g.MeasureString(baseTabControl.TabPages[0].Text, SkinManager.RobotoMedium10)
                                        .Height));
                    }
                }
            }
        }
    }
}