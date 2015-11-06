namespace MaterialSkin.Controls
{
    #region Using Directives

    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    #endregion

    public class MaterialTabControl : TabControl, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }

        [Browsable(false)]
        public MaterialSkinManager SkinManager
        {
            get
            {
                return MaterialSkinManager.Instance;
            }
        }

        [Browsable(false)]
        public MouseState MouseState { get; set; }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x1328 && !this.DesignMode)
            {
                m.Result = (IntPtr) 1;
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}