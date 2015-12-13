using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Tarot.Forms.MaterialSkin.Controls;
using Tarot.Properties;

namespace Tarot.Forms.MaterialSkin
{

    #region Using Directives

    #endregion

    public class MaterialSkinManager
    {
        public static Color SecondaryTextBlack = Color.FromArgb(138, 0, 0, 0);

        public static Brush SecondaryTextBlackBrush = new SolidBrush(SecondaryTextBlack);

        public static Color SecondaryTextWhite = Color.FromArgb(179, 255, 255, 255);

        public static Brush SecondaryTextWhiteBrush = new SolidBrush(SecondaryTextWhite);

        private static readonly Color BackgroundDark = Color.FromArgb(255, 51, 51, 51);

        //Application background
        private static readonly Color BackgroundLight = Color.FromArgb(255, 255, 255, 255);

        private static readonly Color CheckboxOffDark = Color.FromArgb(179, 255, 255, 255);

        private static readonly Brush CheckboxOffDarkBrush = new SolidBrush(CheckboxOffDark);

        private static readonly Color CheckboxOffDisabledDark = Color.FromArgb(77, 255, 255, 255);

        private static readonly Brush CheckboxOffDisabledDarkBrush = new SolidBrush(CheckboxOffDisabledDark);

        private static readonly Color CheckboxOffDisabledLight = Color.FromArgb(66, 0, 0, 0);

        private static readonly Brush CheckboxOffDisabledLightBrush = new SolidBrush(CheckboxOffDisabledLight);

        // Checkbox colors
        private static readonly Color CheckboxOffLight = Color.FromArgb(138, 0, 0, 0);

        private static readonly Brush CheckboxOffLightBrush = new SolidBrush(CheckboxOffLight);

        private static readonly Color CmsBackgroundDarkHover = Color.FromArgb(38, 204, 204, 204);

        private static readonly Brush CmsBackgroundHoverDarkBrush = new SolidBrush(CmsBackgroundDarkHover);
        private static readonly Color CmsBackgroundLightHover = Color.FromArgb(255, 238, 238, 238);
        private static readonly Brush CmsBackgroundHoverLightBrush = new SolidBrush(CmsBackgroundLightHover);

        //ContextMenuStrip

        private static readonly Color DisabledOrHintTextBlack = Color.FromArgb(66, 0, 0, 0);

        private static readonly Brush DisabledOrHintTextBlackBrush = new SolidBrush(DisabledOrHintTextBlack);

        private static readonly Color DisabledOrHintTextWhite = Color.FromArgb(77, 255, 255, 255);

        private static readonly Brush DisabledOrHintTextWhiteBrush = new SolidBrush(DisabledOrHintTextWhite);

        private static readonly Color DividersBlack = Color.FromArgb(31, 0, 0, 0);

        private static readonly Brush DividersBlackBrush = new SolidBrush(DividersBlack);

        private static readonly Color DividersWhite = Color.FromArgb(31, 255, 255, 255);

        private static readonly Brush DividersWhiteBrush = new SolidBrush(DividersWhite);

        private static readonly Color FlatButtonBackgroundHoverDark = Color.FromArgb(
            15.PercentageToColorComponent(),
            0xCCCCCC.ToColor());

        private static readonly Brush FlatButtonBackgroundHoverDarkBrush = new SolidBrush(FlatButtonBackgroundHoverDark);

        //Flat button
        private static readonly Color FlatButtonBackgroundHoverLight = Color.FromArgb(
            20.PercentageToColorComponent(),
            0x999999.ToColor());

        private static readonly Brush FlatButtonBackgroundHoverLightBrush =
            new SolidBrush(FlatButtonBackgroundHoverLight);

        private static readonly Color FlatButtonBackgroundPressedDark = Color.FromArgb(
            25.PercentageToColorComponent(),
            0xCCCCCC.ToColor());

        private static readonly Brush FlatButtonBackgroundPressedDarkBrush =
            new SolidBrush(FlatButtonBackgroundPressedDark);

        private static readonly Color FlatButtonBackgroundPressedLight = Color.FromArgb(
            40.PercentageToColorComponent(),
            0x999999.ToColor());

        private static readonly Brush FlatButtonBackgroundPressedLightBrush =
            new SolidBrush(FlatButtonBackgroundPressedLight);

        private static readonly Color FlatButtonDisabledtextDark = Color.FromArgb(
            30.PercentageToColorComponent(),
            0xFFFFFF.ToColor());

        private static readonly Brush FlatButtonDisabledtextDarkBrush = new SolidBrush(FlatButtonDisabledtextDark);

        private static readonly Color FlatButtonDisabledtextLight = Color.FromArgb(
            26.PercentageToColorComponent(),
            0x000000.ToColor());

        private static readonly Brush FlatButtonDisabledtextLightBrush = new SolidBrush(FlatButtonDisabledtextLight);

        //Constant color values
        private static readonly Color PrimaryTextBlack = Color.FromArgb(222, 0, 0, 0);

        private static readonly Brush PrimaryTextBlackBrush = new SolidBrush(PrimaryTextBlack);

        private static readonly Color PrimaryTextWhite = Color.FromArgb(255, 255, 255, 255);

        private static readonly Brush PrimaryTextWhiteBrush = new SolidBrush(PrimaryTextWhite);

        //Raised button
        private static readonly Color RaisedButtonBackground = Color.FromArgb(255, 255, 255, 255);

        private static readonly Brush RaisedButtonBackgroundBrush = new SolidBrush(RaisedButtonBackground);

        private static readonly Color RaisedButtonTextDark = PrimaryTextBlack;

        private static readonly Brush RaisedButtonTextDarkBrush = new SolidBrush(RaisedButtonTextDark);

        private static readonly Color RaisedButtonTextLight = PrimaryTextWhite;

        private static readonly Brush RaisedButtonTextLightBrush = new SolidBrush(RaisedButtonTextLight);

        private static Brush backgroundDarkBrush = new SolidBrush(BackgroundDark);

        private static Brush backgroundLightBrush = new SolidBrush(BackgroundLight);

        //Singleton instance
        private static MaterialSkinManager instance;

        //Application action bar
        public readonly Color ActionBarText = Color.FromArgb(255, 255, 255, 255);

        public readonly Brush ActionBarTextBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

        public readonly Color ActionBarTextSecondary = Color.FromArgb(153, 255, 255, 255);

        public readonly Brush ActionBarTextSecondaryBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

        //Other constants
        public int FormPadding = 14;

        public Font RobotoMedium10;

        public Font RobotoMedium11;

        //Roboto font
        public Font RobotoMedium12;

        public Font RobotoRegular11;

        //Forms to control
        private readonly List<MaterialForm> formsToManage = new List<MaterialForm>();

        private readonly PrivateFontCollection privateFontCollection = new PrivateFontCollection();

        private ColorScheme colorScheme;

        //Theme
        private Themes theme;

        private MaterialSkinManager()
        {
            this.RobotoMedium12 = new Font(this.LoadFont(Resources.Roboto_Medium), 12f);
            this.RobotoMedium10 = new Font(this.LoadFont(Resources.Roboto_Medium), 10f);
            this.RobotoRegular11 = new Font(this.LoadFont(Resources.Roboto_Regular), 11f);
            this.RobotoMedium11 = new Font(this.LoadFont(Resources.Roboto_Medium), 11f);
            this.Theme = Themes.Light;
            this.ColorScheme = new ColorScheme(
                Primary.BlueGrey800,
                Primary.BlueGrey900,
                Primary.BlueGrey500,
                Accent.LightBlue200,
                TextShade.White);
        }

        public enum Themes : byte
        {
            Light,

            Dark
        }

        public static MaterialSkinManager Instance
        {
            get { return instance ?? (instance = new MaterialSkinManager()); }
        }

        public ColorScheme ColorScheme
        {
            get { return this.colorScheme; }
            set
            {
                this.colorScheme = value;
                this.UpdateBackgrounds();
            }
        }

        public Themes Theme
        {
            get { return this.theme; }
            set
            {
                this.theme = value;
                this.UpdateBackgrounds();
            }
        }

        public void AddFormToManage(MaterialForm materialForm)
        {
            this.formsToManage.Add(materialForm);
            this.UpdateBackgrounds();
        }

        public Color GetApplicationBackgroundColor()
        {
            return this.Theme == Themes.Light ? BackgroundLight : BackgroundDark;
        }

        public Brush GetCheckboxOffBrush()
        {
            return this.Theme == Themes.Light ? CheckboxOffLightBrush : CheckboxOffDarkBrush;
        }

        public Color GetCheckboxOffColor()
        {
            return this.Theme == Themes.Light ? CheckboxOffLight : CheckboxOffDark;
        }

        public Brush GetCheckBoxOffDisabledBrush()
        {
            return this.Theme == Themes.Light ? CheckboxOffDisabledLightBrush : CheckboxOffDisabledDarkBrush;
        }

        public Color GetCheckBoxOffDisabledColor()
        {
            return this.Theme == Themes.Light ? CheckboxOffDisabledLight : CheckboxOffDisabledDark;
        }

        public Brush GetCmsSelectedItemBrush()
        {
            return this.Theme == Themes.Light ? CmsBackgroundHoverLightBrush : CmsBackgroundHoverDarkBrush;
        }

        public Brush GetDisabledOrHintBrush()
        {
            return this.Theme == Themes.Light ? DisabledOrHintTextBlackBrush : DisabledOrHintTextWhiteBrush;
        }

        public Color GetDisabledOrHintColor()
        {
            return this.Theme == Themes.Light ? DisabledOrHintTextBlack : DisabledOrHintTextWhite;
        }

        public Brush GetDividersBrush()
        {
            return this.Theme == Themes.Light ? DividersBlackBrush : DividersWhiteBrush;
        }

        public Color GetDividersColor()
        {
            return this.Theme == Themes.Light ? DividersBlack : DividersWhite;
        }

        public Brush GetFlatButtonDisabledTextBrush()
        {
            return this.Theme == Themes.Light ? FlatButtonDisabledtextLightBrush : FlatButtonDisabledtextDarkBrush;
        }

        public Brush GetFlatButtonHoverBackgroundBrush()
        {
            return this.Theme == Themes.Light ? FlatButtonBackgroundHoverLightBrush : FlatButtonBackgroundHoverDarkBrush;
        }

        public Color GetFlatButtonHoverBackgroundColor()
        {
            return this.Theme == Themes.Light ? FlatButtonBackgroundHoverLight : FlatButtonBackgroundHoverDark;
        }

        public Brush GetFlatButtonPressedBackgroundBrush()
        {
            return this.Theme == Themes.Light
                ? FlatButtonBackgroundPressedLightBrush
                : FlatButtonBackgroundPressedDarkBrush;
        }

        public Color GetFlatButtonPressedBackgroundColor()
        {
            return this.Theme == Themes.Light ? FlatButtonBackgroundPressedLight : FlatButtonBackgroundPressedDark;
        }

        public Brush GetPrimaryTextBrush()
        {
            return this.Theme == Themes.Light ? PrimaryTextBlackBrush : PrimaryTextWhiteBrush;
        }

        public Color GetPrimaryTextColor()
        {
            return this.Theme == Themes.Light ? PrimaryTextBlack : PrimaryTextWhite;
        }

        public Brush GetRaisedButtonBackgroundBrush()
        {
            return RaisedButtonBackgroundBrush;
        }

        public Brush GetRaisedButtonTextBrush(bool primary)
        {
            return primary ? RaisedButtonTextLightBrush : RaisedButtonTextDarkBrush;
        }

        public Brush GetSecondaryTextBrush()
        {
            return this.Theme == Themes.Light ? SecondaryTextBlackBrush : SecondaryTextWhiteBrush;
        }

        public Color GetSecondaryTextColor()
        {
            return this.Theme == Themes.Light ? SecondaryTextBlack : SecondaryTextWhite;
        }

        public void RemoveFormToManage(MaterialForm materialForm)
        {
            this.formsToManage.Remove(materialForm);
        }

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pvd, [In] ref uint pcFonts);

        private FontFamily LoadFont(byte[] fontResource)
        {
            var dataLength = fontResource.Length;
            var fontPtr = Marshal.AllocCoTaskMem(dataLength);
            Marshal.Copy(fontResource, 0, fontPtr, dataLength);

            uint cFonts = 0;
            AddFontMemResourceEx(fontPtr, (uint) fontResource.Length, IntPtr.Zero, ref cFonts);
            this.privateFontCollection.AddMemoryFont(fontPtr, dataLength);

            return this.privateFontCollection.Families.Last();
        }

        private void UpdateBackgrounds()
        {
            var newBackColor = this.GetApplicationBackgroundColor();
            foreach (var materialForm in this.formsToManage)
            {
                materialForm.BackColor = newBackColor;
                this.UpdateControl(materialForm, newBackColor);
            }
        }

        private void UpdateControl(Control controlToUpdate, Color newBackColor)
        {
            if (controlToUpdate == null)
            {
                return;
            }

            if (controlToUpdate.ContextMenuStrip != null)
            {
                this.UpdateToolStrip(controlToUpdate.ContextMenuStrip, newBackColor);
            }
            var tabControl = controlToUpdate as MaterialTabControl;
            if (tabControl != null)
            {
                foreach (TabPage tabPage in tabControl.TabPages)
                {
                    tabPage.BackColor = newBackColor;
                }
            }

            if (controlToUpdate is MaterialDivider)
            {
                controlToUpdate.BackColor = this.GetDividersColor();
            }

            if (controlToUpdate is MaterialListView)
            {
                controlToUpdate.BackColor = newBackColor;
            }

            //recursive call
            foreach (Control control in controlToUpdate.Controls)
            {
                this.UpdateControl(control, newBackColor);
            }

            controlToUpdate.Invalidate();
        }

        private void UpdateToolStrip(ToolStrip toolStrip, Color newBackColor)
        {
            if (toolStrip == null)
            {
                return;
            }

            toolStrip.BackColor = newBackColor;
            foreach (ToolStripItem control in toolStrip.Items)
            {
                control.BackColor = newBackColor;
                if (control is MaterialToolStripMenuItem && (control as MaterialToolStripMenuItem).HasDropDownItems)
                {
                    //recursive call
                    this.UpdateToolStrip((control as MaterialToolStripMenuItem).DropDown, newBackColor);
                }
            }
        }
    }
}