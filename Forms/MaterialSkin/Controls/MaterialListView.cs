using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Oracle.Forms.MaterialSkin.Controls
{

    #region Using Directives

    #endregion

    public class MaterialListView : ListView, IMaterialControl
    {
        private const int ItemPadding = 12;

        public MaterialListView()
        {
            this.GridLines = false;
            this.FullRowSelect = true;
            this.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.View = View.Details;
            this.OwnerDraw = true;
            this.ResizeRedraw = true;
            this.BorderStyle = BorderStyle.None;
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            //Fix for hovers, by default it doesn't redraw
            //TODO: should only redraw when the hovered line changed, this to reduce unnecessary redraws
            this.MouseLocation = new Point(-1, -1);
            this.MouseState = MouseState.Out;
            this.MouseEnter += delegate { this.MouseState = MouseState.Hover; };
            this.MouseLeave += delegate
            {
                this.MouseState = MouseState.Out;
                this.MouseLocation = new Point(-1, -1);
                this.Invalidate();
            };
            this.MouseDown += delegate { this.MouseState = MouseState.Down; };
            this.MouseUp += delegate { this.MouseState = MouseState.Hover; };
            this.MouseMove += delegate(object sender, MouseEventArgs args)
            {
                this.MouseLocation = args.Location;
                this.Invalidate();
            };
        }

        [Browsable(false)]
        public int Depth { get; set; }

        [Browsable(false)]
        public Point MouseLocation { get; set; }

        [Browsable(false)]
        public MouseState MouseState { get; set; }

        [Browsable(false)]
        public MaterialSkinManager SkinManager
        {
            get { return MaterialSkinManager.Instance; }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            //This is a hax for the needed padding.
            //Another way would be intercepting all ListViewItems and changing the sizes, but really, that will be a lot of work
            //This will do for now.
            this.Font = new Font(this.SkinManager.RobotoMedium12.FontFamily, 24);
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(
                new SolidBrush(this.SkinManager.GetApplicationBackgroundColor()),
                new Rectangle(e.Bounds.X, e.Bounds.Y, this.Width, e.Bounds.Height));
            e.Graphics.DrawString(
                e.Header.Text, this.SkinManager.RobotoMedium10, this.SkinManager.GetSecondaryTextBrush(),
                new Rectangle(
                    e.Bounds.X + ItemPadding,
                    e.Bounds.Y + ItemPadding,
                    e.Bounds.Width - ItemPadding * 2,
                    e.Bounds.Height - ItemPadding * 2), this.GetStringFormat());
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            //We draw the current line of items (= item with subitems) on a temp bitmap, then draw the bitmap at once. This is to reduce flickering.
            var b = new Bitmap(e.Item.Bounds.Width, e.Item.Bounds.Height);
            var g = Graphics.FromImage(b);

            //always draw default background
            g.FillRectangle(
                new SolidBrush(this.SkinManager.GetApplicationBackgroundColor()),
                new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));

            if (e.State.HasFlag(ListViewItemStates.Selected))
            {
                //selected background
                g.FillRectangle(this.SkinManager.GetFlatButtonPressedBackgroundBrush(),
                    new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
            }
            else if (e.Bounds.Contains(this.MouseLocation) && this.MouseState == MouseState.Hover)
            {
                //hover background
                g.FillRectangle(this.SkinManager.GetFlatButtonHoverBackgroundBrush(),
                    new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
            }

            //Draw seperator
            g.DrawLine(new Pen(this.SkinManager.GetDividersColor()), e.Bounds.Left, 0, e.Bounds.Right, 0);

            foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
            {
                //Draw text
                g.DrawString(
                    subItem.Text, this.SkinManager.RobotoMedium10, this.SkinManager.GetPrimaryTextBrush(),
                    new Rectangle(
                        subItem.Bounds.Location.X + ItemPadding,
                        ItemPadding,
                        subItem.Bounds.Width - 2 * ItemPadding,
                        subItem.Bounds.Height - 2 * ItemPadding), this.GetStringFormat());
            }

            e.Graphics.DrawImage((Image) b.Clone(), e.Item.Bounds.Location);
            g.Dispose();
            b.Dispose();
        }

        private StringFormat GetStringFormat()
        {
            return new StringFormat
            {
                FormatFlags = StringFormatFlags.LineLimit,
                Trimming = StringTrimming.EllipsisCharacter,
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
        }
    }
}