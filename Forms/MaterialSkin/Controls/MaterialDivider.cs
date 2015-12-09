using System.ComponentModel;
using System.Windows.Forms;

namespace Tarot.Forms.MaterialSkin.Controls
{
    #region Using Directives

    

    #endregion

    public sealed class MaterialDivider : Control, IMaterialControl
    {
        public MaterialDivider()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Height = 1;
            BackColor = SkinManager.GetDividersColor();
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