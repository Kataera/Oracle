using System.ComponentModel;
using System.Windows.Forms;

namespace Tarot.Forms.MaterialSkin.Controls
{

    #region Using Directives

    #endregion

    public class MaterialLabel : Label, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }

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

            ForeColor = SkinManager.GetPrimaryTextColor();
            Font = SkinManager.RobotoRegular11;

            BackColorChanged += (sender, args) => ForeColor = SkinManager.GetPrimaryTextColor();
        }
    }
}