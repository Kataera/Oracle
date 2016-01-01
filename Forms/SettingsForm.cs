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
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
        private bool UpdatingRows;

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
            dataTable.PrimaryKey = new[] {dataTable.Columns["Id"]};

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

        // Add to MouseDown of a component to allow dragging of the form.
        private void MoveWindow(object sender, MouseEventArgs e)
        {
            if (this.Maximized)
            {
                return;
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            ReleaseCapture();
            SendMessage(this.Handle, WmNclbuttondown, HtCaption, 0);
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

        private void OnButtonResetZoneLevelsToDefaultClick(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you wish to set the zone levels to their default setting?", "Warning",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                OracleSettings.Instance.ZoneLevels = new Dictionary<uint, uint>();
                OracleSettings.Instance.PopulateZoneLevels();

                this.dataGridViewZoneChangeSettings.CellValueChanged -= this.OnDataGridViewCellValueChanged;
                this.dataGridViewZoneChangeSettings.Rows.Clear();

                foreach (var item in OracleSettings.Instance.ZoneLevels)
                {
                    this.dataGridViewZoneChangeSettings.Rows.Add(item.Key, item.Value.ToString());
                }

                OracleSettings.Instance.Save();
                this.dataGridViewZoneChangeSettings.CellValueChanged += this.OnDataGridViewCellValueChanged;
            }
        }

        private void OnChangingEnabledChanged(object sender, EventArgs e)
        {
            OracleSettings.Instance.ChangeZonesEnabled = this.checkBoxZoneChangingEnabledSetting.Checked;
        }

        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnDataGridViewCellClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Make sure the clicked row isn't the header.
            var validRow = e.RowIndex != -1;
            var dataGridView = sender as DataGridView;

            if (dataGridView == null)
            {
                return;
            }

            if (!(dataGridView.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn) || !validRow)
            {
                return;
            }

            // Band-aid fix for dropping selection of current cell.
            if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Control)
            {
                dataGridView.Rows[e.RowIndex].Selected = true;
            }

            dataGridView.BeginEdit(false);
            ((ComboBox) dataGridView.EditingControl).KeyDown += this.OnEnterKeyDownDropFocus;
            ((ComboBox) dataGridView.EditingControl).DroppedDown = true;
        }

        private void OnDataGridViewCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var aetheryteId =
                Convert.ToUInt32(this.dataGridViewZoneChangeSettings.Rows[e.RowIndex].Cells[this.ColumnAetheryte.Index].Value.ToString());

            if (this.UpdatingRows)
            {
                return;
            }

            this.UpdatingRows = true;
            foreach (DataGridViewRow row in this.dataGridViewZoneChangeSettings.SelectedRows)
            {
                var level = Convert.ToUInt32(row.Cells[this.ColumnLevel.Index].Value);
                if (!OracleSettings.Instance.ZoneLevels.ContainsKey(level))
                {
                    continue;
                }

                OracleSettings.Instance.ZoneLevels.Remove(level);
                OracleSettings.Instance.ZoneLevels.Add(level, aetheryteId);
                row.Cells[this.ColumnAetheryte.Index].Value = aetheryteId.ToString();
            }

            OracleSettings.Instance.Save();
            this.UpdatingRows = false;
        }

        private void OnDataGridViewPaint(object sender, PaintEventArgs e)
        {
            // Another dirty hack, WinForms is fun!
            var colour = this.dataGridViewZoneChangeSettings.ColumnHeadersDefaultCellStyle.BackColor;
            var headerRect = this.dataGridViewZoneChangeSettings.GetCellDisplayRectangle(this.ColumnAetheryte.Index, -1, true);

            headerRect.X += headerRect.Width - 2;
            headerRect.Y += 1;
            headerRect.Width = 4;
            headerRect.Height -= 2;
            e.Graphics.FillRectangle(new SolidBrush(colour), headerRect);

            headerRect = this.dataGridViewZoneChangeSettings.GetCellDisplayRectangle(this.ColumnLevel.Index, -1, true);

            headerRect.X += headerRect.Width - 2;
            headerRect.Y += 1;
            headerRect.Width = 4;
            headerRect.Height -= 2;
            e.Graphics.FillRectangle(new SolidBrush(colour), headerRect);

            headerRect = this.dataGridViewZoneChangeSettings.GetCellDisplayRectangle(this.EmptyColumn.Index, -1, true);

            headerRect.X += headerRect.Width - 2;
            headerRect.Y += 1;
            headerRect.Width = 4;
            headerRect.Height -= 2;
            e.Graphics.FillRectangle(new SolidBrush(colour), headerRect);
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
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            this.ActiveControl = this.labelDefaultFocus;
            e.SuppressKeyPress = true;
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

        private void SetComponentValues()
        {
            this.comboBoxOracleModeSetting.SelectedIndex = (int) OracleSettings.Instance.OracleOperationMode;
            this.tabControllerOracleMode.SelectedIndex = (int) OracleSettings.Instance.OracleOperationMode;
            this.textBoxSpecificFateNameSetting.Text = OracleSettings.Instance.SpecificFate;

            this.comboBoxFateSelectStrategySetting.SelectedIndex = (int) OracleSettings.Instance.FateSelectMode;

            this.comboBoxDowntimeBehaviourSetting.SelectedIndex = (int) OracleSettings.Instance.FateWaitMode;

            this.numericUpDownMaxLevelAboveSetting.Value = OracleSettings.Instance.MobMaximumLevelAbove;
            this.numericUpDownMinLevelBelowSetting.Value = OracleSettings.Instance.MobMaximumLevelBelow;

            this.checkBoxZoneChangingEnabledSetting.Checked = OracleSettings.Instance.ChangeZonesEnabled;
            this.ColumnAetheryte.DataSource = GenerateAetheryteNameTable();
            this.ColumnAetheryte.DisplayMember = "Name";
            this.ColumnAetheryte.ValueMember = "Id";

            foreach (var item in OracleSettings.Instance.ZoneLevels)
            {
                this.dataGridViewZoneChangeSettings.Rows.Add(item.Key, item.Value.ToString());
            }

            this.dataGridViewZoneChangeSettings.CellValueChanged += this.OnDataGridViewCellValueChanged;

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