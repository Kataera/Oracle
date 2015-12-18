using System.ComponentModel;
using System.Windows.Forms;

namespace Oracle.Forms.MaterialSkin.Controls
{

    #region Using Directives

    #endregion

    public sealed class MaterialDivider : Control, IMaterialControl
    {
        public MaterialDivider()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Height = 1;
            this.BackColor = this.SkinManager.GetDividersColor();
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
    }
}