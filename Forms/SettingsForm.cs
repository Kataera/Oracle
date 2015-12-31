/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015-2016 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Oracle.

    Oracle is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Oracle is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Oracle. If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using ff14bot;
using ff14bot.Managers;

using MaterialSkin;
using MaterialSkin.Controls;

using Oracle.Enumerations;
using Oracle.Settings;

namespace Oracle.Forms
{
    public partial class SettingsForm : MaterialForm
    {
        public SettingsForm()
        {
            this.InitializeComponent();

            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);
            skinManager.Theme = MaterialSkinManager.Themes.Light;
            skinManager.ColorScheme = new ColorScheme(
                Primary.Indigo600,
                Primary.Indigo600,
                Primary.Indigo300,
                Accent.Indigo100,
                TextShade.White);

            this.ActiveControl = this.labelDefaultFocus;
            this.SetComponentValues();
        }

        // Add to MouseDown of a component to allow dragging of the form.
        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WmNclbuttondown, HtCaption, 0);
            }
        }

        private void OnButtonDowntimeSetLocationClick(object sender, EventArgs e)
        {
            try
            {
                var zoneId = WorldManager.ZoneId;
                var location = Core.Player.Location;

                if (OracleSettings.Instance.FateWaitLocations.ContainsKey(zoneId))
                {
                    OracleSettings.Instance.FateWaitLocations.Remove(zoneId);
                }

                OracleSettings.Instance.FateWaitLocations.Add(zoneId, location);
            }
            catch (NullReferenceException)
            {
                // This will only occur if the form is created outside of RebornBuddy.
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

        private void OnDowntimeBehaviourSelectedIndexChanged(object sender, EventArgs e)
        {
            OracleSettings.Instance.FateWaitMode = (FateWaitMode) this.comboBoxDowntimeBehaviourSetting.SelectedIndex;
            this.tabControllerDowntime.SelectedIndex = (int) OracleSettings.Instance.FateWaitMode;
        }

        private void OnEnterKeyDownDropFocus(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ActiveControl = this.labelDefaultFocus;
                e.SuppressKeyPress = true;
            }
        }

        private void OnEnterSelectAllText(object sender, EventArgs e)
        {
            this.textBoxSpecificFateNameSetting.SelectAll();
        }

        private void OnFullLicenseLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var startInfo = new ProcessStartInfo("http://www.gnu.org/licenses/gpl-3.0.en.html");
            Process.Start(startInfo);
        }

        private void OnOracleModeSelectedIndexChanged(object sender, EventArgs e)
        {
            OracleSettings.Instance.OracleOperationMode = (OracleOperationMode) this.comboBoxOracleModeSetting.SelectedIndex;
            this.tabControllerOracleMode.SelectedIndex = (int) OracleSettings.Instance.OracleOperationMode;
        }

        private void OnTabPageClick(object sender, EventArgs e)
        {
            this.ActiveControl = this.labelDefaultFocus;
        }

        private void OnTextBoxSpecificFateNameTextChanged(object sender, EventArgs e)
        {
            OracleSettings.Instance.SpecificFate = this.textBoxSpecificFateNameSetting.Text;
        }

        private void SetComponentValues()
        {
            this.comboBoxOracleModeSetting.SelectedIndex = (int) OracleSettings.Instance.OracleOperationMode;
            this.tabControllerOracleMode.SelectedIndex = (int) OracleSettings.Instance.OracleOperationMode;
            this.textBoxSpecificFateNameSetting.Text = OracleSettings.Instance.SpecificFate;

            this.comboBoxFateSelectStrategySetting.SelectedIndex = (int) OracleSettings.Instance.FateSelectMode;

            this.comboBoxDowntimeBehaviourSetting.SelectedIndex = (int) OracleSettings.Instance.FateWaitMode;

            this.numericUpDownMaxLevelAboveSetting.Value = OracleSettings.Instance.MobMaximumLevelAbove;
            this.numericUpDownMinLevelBelowSetting.Value = OracleSettings.Instance.MobMaximumLevelBelow;

            try
            {
                this.labelDowntimeCurrentZoneValue.Text = WorldManager.ZoneId.ToString();
                if (OracleSettings.Instance.FateWaitLocations.ContainsKey(WorldManager.ZoneId))
                {
                    var waitLocation = OracleSettings.Instance.FateWaitLocations.FirstOrDefault(loc => loc.Key == WorldManager.ZoneId).Value;
                    this.labelDowntimeWaitLocationValue.Text = waitLocation.ToString();
                }
            }
            catch (NullReferenceException)
            {
                // This will only occur if the form is created outside of RebornBuddy.
            }
        }
    }
}