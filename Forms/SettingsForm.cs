﻿/*
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
using System.Data;
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

        private static DataTable GenerateAetheryteNameTable()
        {
            var dataTable = new DataTable("Aetherytes");
            dataTable.Columns.Add("Id");
            dataTable.Columns.Add("Name");

            dataTable.Rows.Add(14, "Aleport");
            dataTable.Rows.Add(77, "Anyx Trine");
            dataTable.Rows.Add(3, "Bentbranch Meadows");
            dataTable.Rows.Add(53, "Black Brush Station");
            dataTable.Rows.Add(21, "Camp Bluefog");
            dataTable.Rows.Add(15, "Camp Bronze Lake");
            dataTable.Rows.Add(72, "Camp Cloudtop");
            dataTable.Rows.Add(23, "Camp Dragonhead");
            dataTable.Rows.Add(18, "Camp Drybone");
            dataTable.Rows.Add(16, "Camp Overlook");
            dataTable.Rows.Add(6, "Camp Tranquil");
            dataTable.Rows.Add(22, "Ceruleum Processing Plant");
            dataTable.Rows.Add(11, "Costa del Sol");
            dataTable.Rows.Add(71, "Falcon's Nest");
            dataTable.Rows.Add(7, "Fallgourd Float");
            dataTable.Rows.Add(20, "Forgotten Springs");
            dataTable.Rows.Add(74, "Helix");
            dataTable.Rows.Add(17, "Horizon");
            dataTable.Rows.Add(75, "Idyllshire");
            dataTable.Rows.Add(19, "Little Ala Mhigo");
            dataTable.Rows.Add(78, "Moghome");
            dataTable.Rows.Add(10, "Moraby Drydocks");
            dataTable.Rows.Add(73, "Ok' Zundu");
            dataTable.Rows.Add(5, "Quarrymill");
            dataTable.Rows.Add(24, "Revenant's Toll");
            dataTable.Rows.Add(52, "Summerford Farms");
            dataTable.Rows.Add(13, "Swiftperch");
            dataTable.Rows.Add(76, "Tailfeather");
            dataTable.Rows.Add(4, "The Hawthorne Hut");
            dataTable.Rows.Add(12, "Wineport");
            dataTable.Rows.Add(79, "Zenith");

            return dataTable;
        }

        private static void OnDataGridViewControlEnter(object sender, EventArgs e)
        {
            ((ComboBox) sender).DroppedDown = true;
        }

        // Add to MouseDown of a component to allow dragging of the form.
        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (!this.Maximized)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WmNclbuttondown, HtCaption, 0);
                }
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

        private void OnDataGridViewEditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Dirty hack that lets us open the combo box in one click. :>
            var ctrl = e.Control;
            ctrl.Enter -= OnDataGridViewControlEnter;
            ctrl.Enter += OnDataGridViewControlEnter;
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
            this.ActiveControl = this.labelDefaultFocus;
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
            this.ActiveControl = this.labelDefaultFocus;
        }

        private void OnTabPageClick(object sender, EventArgs e)
        {
            this.ActiveControl = this.labelDefaultFocus;
        }

        private void OnTextBoxSpecificFateNameTextChanged(object sender, EventArgs e)
        {
            OracleSettings.Instance.SpecificFate = this.textBoxSpecificFateNameSetting.Text;
        }

        private void OnZoneChangeSettingsCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var level = Convert.ToUInt32(this.dataGridViewZoneChangeSettings.Rows[e.RowIndex].Cells[this.ColumnLevel.Index].Value.ToString());
            var value =
                Convert.ToUInt32(this.dataGridViewZoneChangeSettings.Rows[e.RowIndex].Cells[this.ColumnAetheryte.Index].Value.ToString());

            if (OracleSettings.Instance.ZoneLevels.ContainsKey(level))
            {
                OracleSettings.Instance.ZoneLevels.Remove(level);
                OracleSettings.Instance.ZoneLevels.Add(level, value);
            }
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

            this.ColumnAetheryte.DataSource = GenerateAetheryteNameTable();
            this.ColumnAetheryte.DisplayMember = "Name";
            this.ColumnAetheryte.ValueMember = "Id";

            foreach (var item in OracleSettings.Instance.ZoneLevels)
            {
                this.dataGridViewZoneChangeSettings.Rows.Add(item.Key, item.Value.ToString());
            }

            this.dataGridViewZoneChangeSettings.CellValueChanged += this.OnZoneChangeSettingsCellValueChanged;

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