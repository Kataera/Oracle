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

            this.ForeColor = this.SkinManager.GetPrimaryTextColor();
            this.Font = this.SkinManager.RobotoRegular11;

            this.BackColorChanged += (sender, args) => this.ForeColor = this.SkinManager.GetPrimaryTextColor();
        }
    }
}