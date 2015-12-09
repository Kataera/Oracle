/*
    #################
    ##   License   ##
    #################

    Tarot - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Tarot.

    Tarot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Tarot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Tarot. If not, see http://www.gnu.org/licenses/.
*/

namespace Tarot.Forms
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    using global::Tarot.Forms.MaterialSkin;
    using global::Tarot.Forms.MaterialSkin.Controls;

    public partial class SettingsForm : MaterialForm
    {
        public SettingsForm()
        {
            this.InitializeComponent();

            // Initialize MaterialSkinManager
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.Light;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Indigo500,
                Primary.Indigo500,
                Primary.Indigo300,
                Accent.Indigo100,
                TextShade.White);
        }

        // Add to MouseDown of a component to allow dragging of the form.
        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnDonatePictureBoxClick(object sender, EventArgs e)
        {
            var startInfo =
                new ProcessStartInfo(
                    "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=CK2TD9J572T34");
            Process.Start(startInfo);
        }

        private void OnFullLicenseLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var startInfo = new ProcessStartInfo("http://www.gnu.org/licenses/gpl-3.0.en.html");
            Process.Start(startInfo);
        }
    }
}