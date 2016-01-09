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

using System.Windows.Forms;

using Oracle.Enumerations;
using Oracle.Settings;

namespace Oracle.Forms
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabSelectorMain = new MaterialSkin.Controls.MaterialTabSelector();
            this.tabControllerMain = new MaterialSkin.Controls.MaterialTabControl();
            this.tabGeneralSettings = new System.Windows.Forms.TabPage();
            this.tabSelectorGeneral = new MaterialSkin.Controls.MaterialTabSelectorVertical();
            this.tabControllerGeneral = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPageOracleMode = new System.Windows.Forms.TabPage();
            this.tabControllerOracleMode = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPageOracleModeFateGrind = new System.Windows.Forms.TabPage();
            this.labelOracleModeFateGrind = new System.Windows.Forms.Label();
            this.tabPageOracleModeSpecificFate = new System.Windows.Forms.TabPage();
            this.labelSpecificFateNameSetting = new System.Windows.Forms.Label();
            this.textBoxSpecificFateNameSetting = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.labelOracleModeSpecificFate = new System.Windows.Forms.Label();
            this.tabPageOracleModeAtmaGrind = new System.Windows.Forms.TabPage();
            this.labelAtmaGrindNYI = new System.Windows.Forms.Label();
            this.tabPageOracleModeAnimusGrind = new System.Windows.Forms.TabPage();
            this.labelAnimusGrindModeNYI = new System.Windows.Forms.Label();
            this.tabPageOracleModeAnimaGrind = new System.Windows.Forms.TabPage();
            this.labelAnimaGrindMode = new System.Windows.Forms.Label();
            this.labelOracleModeSetting = new System.Windows.Forms.Label();
            this.comboBoxOracleModeSetting = new System.Windows.Forms.ComboBox();
            this.labelOracleModeTitle = new System.Windows.Forms.Label();
            this.tabPageFateSelection = new System.Windows.Forms.TabPage();
            this.labelFateSelectionDescription = new System.Windows.Forms.Label();
            this.comboBoxFateSelectStrategySetting = new System.Windows.Forms.ComboBox();
            this.labelFateSelectStrategySetting = new System.Windows.Forms.Label();
            this.labelFateSelectionTitle = new System.Windows.Forms.Label();
            this.tabPageDowntime = new System.Windows.Forms.TabPage();
            this.tabControllerDowntime = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPageDowntimeReturnToAetheryte = new System.Windows.Forms.TabPage();
            this.labelDowntimeReturnToAetheryte = new System.Windows.Forms.Label();
            this.tabPageDowntimeMoveToLocation = new System.Windows.Forms.TabPage();
            this.buttonDowntimeSetLocation = new MaterialSkin.Controls.MaterialRaisedButton();
            this.buttonDowntimeRefreshZone = new MaterialSkin.Controls.MaterialRaisedButton();
            this.labelDowntimeWaitLocationValue = new System.Windows.Forms.Label();
            this.labelDowntimeCurrentZoneValue = new System.Windows.Forms.Label();
            this.labelDowntimeWaitLocation = new System.Windows.Forms.Label();
            this.labelDowntimeCurrentZone = new System.Windows.Forms.Label();
            this.labelDowntimeMoveToLocation = new System.Windows.Forms.Label();
            this.tabPageDowntimeGrindMobs = new System.Windows.Forms.TabPage();
            this.numericUpDownMobMaximumLevelAboveSetting = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMobMinimumLevelBelowSetting = new System.Windows.Forms.NumericUpDown();
            this.labelMobMaxLevelAboveSetting = new System.Windows.Forms.Label();
            this.labelMobMinLevelBelowSetting = new System.Windows.Forms.Label();
            this.labelDowntimeGrindMobs = new System.Windows.Forms.Label();
            this.tabPageDowntimeDoNothing = new System.Windows.Forms.TabPage();
            this.labelDowntimeDoNothing = new System.Windows.Forms.Label();
            this.comboBoxFateWaitModeSetting = new System.Windows.Forms.ComboBox();
            this.labelDowntimeBehaviourSetting = new System.Windows.Forms.Label();
            this.labelDowntimeTitle = new System.Windows.Forms.Label();
            this.tabPageZoneChanging = new System.Windows.Forms.TabPage();
            this.checkBoxBindHomePointSetting = new MaterialSkin.Controls.MaterialCheckBox();
            this.labelBindHomePointSetting = new System.Windows.Forms.Label();
            this.buttonResetZoneLevelsToDefault = new MaterialSkin.Controls.MaterialRaisedButton();
            this.labelZoneChangeTip = new System.Windows.Forms.Label();
            this.dataGridViewZoneChangeSettings = new System.Windows.Forms.DataGridView();
            this.ColumnLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAetheryte = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.EmptyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelZoneChangeEnabledSetting = new System.Windows.Forms.Label();
            this.checkBoxZoneChangingEnabledSetting = new MaterialSkin.Controls.MaterialCheckBox();
            this.labelZoneChangingTitle = new System.Windows.Forms.Label();
            this.tabPageMiscellaneous = new System.Windows.Forms.TabPage();
            this.labelMiscellaneousTitle = new System.Windows.Forms.Label();
            this.tabFateSettings = new System.Windows.Forms.TabPage();
            this.tabControllerFate = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.labelChainFateWaitTimeoutSettingSuffix = new System.Windows.Forms.Label();
            this.textBoxChainFateWaitTimeoutSetting = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.labelChainFateWaitTimeoutSetting = new System.Windows.Forms.Label();
            this.checkBoxWaitForChainFateSetting = new MaterialSkin.Controls.MaterialCheckBox();
            this.labelWaitForChainFateSetting = new System.Windows.Forms.Label();
            this.labelLowDurationTimeSettingSuffix = new System.Windows.Forms.Label();
            this.textBoxLowRemainingFateDurationSetting = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.labelLowDurationTimeSetting = new System.Windows.Forms.Label();
            this.checkBoxIgnoreLowDurationUnstartedFateSetting = new MaterialSkin.Controls.MaterialCheckBox();
            this.labelIgnoreLowDurationFatesSetting = new System.Windows.Forms.Label();
            this.checkBoxRunProblematicFatesSetting = new MaterialSkin.Controls.MaterialCheckBox();
            this.labelRunProblematicFatesWarning = new System.Windows.Forms.Label();
            this.labelRunProblematicFatesSetting = new System.Windows.Forms.Label();
            this.numericUpDownFateMaximumLevelAboveSetting = new System.Windows.Forms.NumericUpDown();
            this.labelFateMaximumLevelAboveSetting = new System.Windows.Forms.Label();
            this.numericUpDownFateMinimumLevelBelowSetting = new System.Windows.Forms.NumericUpDown();
            this.labelFateMinimumLevelBelowSetting = new System.Windows.Forms.Label();
            this.labelGeneralFateSettingsTitle = new System.Windows.Forms.Label();
            this.tabPageKillFates = new System.Windows.Forms.TabPage();
            this.checkBoxKillFatesEnabledSetting = new MaterialSkin.Controls.MaterialCheckBox();
            this.labelKillFatesEnabledSetting = new System.Windows.Forms.Label();
            this.labelKillFatesTitle = new System.Windows.Forms.Label();
            this.tabPageCollectFates = new System.Windows.Forms.TabPage();
            this.checkBoxCollectFatesEnabledSetting = new MaterialSkin.Controls.MaterialCheckBox();
            this.labelCollectFatesEnabledSetting = new System.Windows.Forms.Label();
            this.labelCollectFatesTitle = new System.Windows.Forms.Label();
            this.tabPageEscortFates = new System.Windows.Forms.TabPage();
            this.checkBoxEscortFatesEnabledSetting = new MaterialSkin.Controls.MaterialCheckBox();
            this.labelEscortFatesEnabledSetting = new System.Windows.Forms.Label();
            this.labelEscortFatesTitle = new System.Windows.Forms.Label();
            this.tabPageDefenceFates = new System.Windows.Forms.TabPage();
            this.checkBoxDefenceFatesEnabledSetting = new MaterialSkin.Controls.MaterialCheckBox();
            this.labelDefenceFatesEnabledSetting = new System.Windows.Forms.Label();
            this.labelDefenceFatesTitle = new System.Windows.Forms.Label();
            this.tabPageBossFates = new System.Windows.Forms.TabPage();
            this.checkBoxBossFatesEnabledSetting = new MaterialSkin.Controls.MaterialCheckBox();
            this.labelBossFatesEnabledSetting = new System.Windows.Forms.Label();
            this.labelBossFatesTitle = new System.Windows.Forms.Label();
            this.tabPageMegaBossFates = new System.Windows.Forms.TabPage();
            this.checkBoxMegaBossFatesEnabledSetting = new MaterialSkin.Controls.MaterialCheckBox();
            this.labelMegaBossFatesEnabledSetting = new System.Windows.Forms.Label();
            this.labelMegaBossFatesTitle = new System.Windows.Forms.Label();
            this.tabSelectorFate = new MaterialSkin.Controls.MaterialTabSelectorVertical();
            this.tabNavigationSettings = new System.Windows.Forms.TabPage();
            this.tabSelectorCustom = new MaterialSkin.Controls.MaterialTabSelectorVertical();
            this.tabControllerCustom = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPageMovement = new System.Windows.Forms.TabPage();
            this.labelMovementTitle = new System.Windows.Forms.Label();
            this.tabPageFlight = new System.Windows.Forms.TabPage();
            this.labelFlightTitle = new System.Windows.Forms.Label();
            this.tabPageTeleport = new System.Windows.Forms.TabPage();
            this.labelTeleportTitle = new System.Windows.Forms.Label();
            this.tabBlacklist = new System.Windows.Forms.TabPage();
            this.tabSelectorBlacklist = new MaterialSkin.Controls.MaterialTabSelectorVertical();
            this.tabControllerBlacklist = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPageFateBlacklist = new System.Windows.Forms.TabPage();
            this.labelFateBlacklistTitle = new System.Windows.Forms.Label();
            this.tabPageMobBlacklist = new System.Windows.Forms.TabPage();
            this.labelMobBlacklistTitle = new System.Windows.Forms.Label();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.tabControllerAbout = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPageLicense = new System.Windows.Forms.TabPage();
            this.labelFullLicenseLink = new System.Windows.Forms.LinkLabel();
            this.labelLicenseText = new System.Windows.Forms.Label();
            this.labelLicenseTitle = new System.Windows.Forms.Label();
            this.tabPageDonate = new System.Windows.Forms.TabPage();
            this.pictureBoxDonate = new MaterialSkin.Controls.MaterialPictureBox();
            this.labelDonateText = new System.Windows.Forms.Label();
            this.labelDonateTitle = new System.Windows.Forms.Label();
            this.tabPageDevelopment = new System.Windows.Forms.TabPage();
            this.labelDevelopmentTitle = new System.Windows.Forms.Label();
            this.tabSelectorAbout = new MaterialSkin.Controls.MaterialTabSelectorVertical();
            this.labelVersionInformation = new System.Windows.Forms.Label();
            this.panelControl = new System.Windows.Forms.Panel();
            this.buttonClose = new MaterialSkin.Controls.MaterialFlatButton();
            this.labelDefaultFocus = new System.Windows.Forms.Label();
            this.pictureBoxLogo = new MaterialSkin.Controls.MaterialPictureBox();
            this.tabControllerMain.SuspendLayout();
            this.tabGeneralSettings.SuspendLayout();
            this.tabControllerGeneral.SuspendLayout();
            this.tabPageOracleMode.SuspendLayout();
            this.tabControllerOracleMode.SuspendLayout();
            this.tabPageOracleModeFateGrind.SuspendLayout();
            this.tabPageOracleModeSpecificFate.SuspendLayout();
            this.tabPageOracleModeAtmaGrind.SuspendLayout();
            this.tabPageOracleModeAnimusGrind.SuspendLayout();
            this.tabPageOracleModeAnimaGrind.SuspendLayout();
            this.tabPageFateSelection.SuspendLayout();
            this.tabPageDowntime.SuspendLayout();
            this.tabControllerDowntime.SuspendLayout();
            this.tabPageDowntimeReturnToAetheryte.SuspendLayout();
            this.tabPageDowntimeMoveToLocation.SuspendLayout();
            this.tabPageDowntimeGrindMobs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMobMaximumLevelAboveSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMobMinimumLevelBelowSetting)).BeginInit();
            this.tabPageDowntimeDoNothing.SuspendLayout();
            this.tabPageZoneChanging.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewZoneChangeSettings)).BeginInit();
            this.tabPageMiscellaneous.SuspendLayout();
            this.tabFateSettings.SuspendLayout();
            this.tabControllerFate.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFateMaximumLevelAboveSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFateMinimumLevelBelowSetting)).BeginInit();
            this.tabPageKillFates.SuspendLayout();
            this.tabPageCollectFates.SuspendLayout();
            this.tabPageEscortFates.SuspendLayout();
            this.tabPageDefenceFates.SuspendLayout();
            this.tabPageBossFates.SuspendLayout();
            this.tabPageMegaBossFates.SuspendLayout();
            this.tabNavigationSettings.SuspendLayout();
            this.tabControllerCustom.SuspendLayout();
            this.tabPageMovement.SuspendLayout();
            this.tabPageFlight.SuspendLayout();
            this.tabPageTeleport.SuspendLayout();
            this.tabBlacklist.SuspendLayout();
            this.tabControllerBlacklist.SuspendLayout();
            this.tabPageFateBlacklist.SuspendLayout();
            this.tabPageMobBlacklist.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.tabControllerAbout.SuspendLayout();
            this.tabPageLicense.SuspendLayout();
            this.tabPageDonate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDonate)).BeginInit();
            this.tabPageDevelopment.SuspendLayout();
            this.panelControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // tabSelectorMain
            // 
            this.tabSelectorMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSelectorMain.BaseTabControl = this.tabControllerMain;
            this.tabSelectorMain.Depth = 0;
            this.tabSelectorMain.Font = new System.Drawing.Font("Roboto Condensed", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabSelectorMain.Location = new System.Drawing.Point(155, 24);
            this.tabSelectorMain.Margin = new System.Windows.Forms.Padding(0);
            this.tabSelectorMain.MouseState = MaterialSkin.MouseState.Hover;
            this.tabSelectorMain.Name = "tabSelectorMain";
            this.tabSelectorMain.Size = new System.Drawing.Size(695, 40);
            this.tabSelectorMain.TabIndex = 0;
            this.tabSelectorMain.TabStop = false;
            this.tabSelectorMain.Text = "Settings Tabs";
            this.tabSelectorMain.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // tabControllerMain
            // 
            this.tabControllerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControllerMain.Controls.Add(this.tabGeneralSettings);
            this.tabControllerMain.Controls.Add(this.tabFateSettings);
            this.tabControllerMain.Controls.Add(this.tabNavigationSettings);
            this.tabControllerMain.Controls.Add(this.tabBlacklist);
            this.tabControllerMain.Controls.Add(this.tabAbout);
            this.tabControllerMain.Depth = 0;
            this.tabControllerMain.Location = new System.Drawing.Point(0, 64);
            this.tabControllerMain.Margin = new System.Windows.Forms.Padding(0);
            this.tabControllerMain.MouseState = MaterialSkin.MouseState.Hover;
            this.tabControllerMain.Name = "tabControllerMain";
            this.tabControllerMain.Padding = new System.Drawing.Point(0, 0);
            this.tabControllerMain.SelectedIndex = 0;
            this.tabControllerMain.Size = new System.Drawing.Size(850, 521);
            this.tabControllerMain.TabIndex = 1;
            // 
            // tabGeneralSettings
            // 
            this.tabGeneralSettings.BackColor = System.Drawing.Color.White;
            this.tabGeneralSettings.Controls.Add(this.tabSelectorGeneral);
            this.tabGeneralSettings.Controls.Add(this.tabControllerGeneral);
            this.tabGeneralSettings.Location = new System.Drawing.Point(4, 29);
            this.tabGeneralSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tabGeneralSettings.Name = "tabGeneralSettings";
            this.tabGeneralSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneralSettings.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabGeneralSettings.Size = new System.Drawing.Size(842, 488);
            this.tabGeneralSettings.TabIndex = 0;
            this.tabGeneralSettings.Text = "General Settings";
            this.tabGeneralSettings.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // tabSelectorGeneral
            // 
            this.tabSelectorGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabSelectorGeneral.BaseTabControl = this.tabControllerGeneral;
            this.tabSelectorGeneral.Depth = 0;
            this.tabSelectorGeneral.Location = new System.Drawing.Point(-4, 0);
            this.tabSelectorGeneral.Margin = new System.Windows.Forms.Padding(0);
            this.tabSelectorGeneral.MinimumSize = new System.Drawing.Size(173, 449);
            this.tabSelectorGeneral.MouseState = MaterialSkin.MouseState.Hover;
            this.tabSelectorGeneral.Name = "tabSelectorGeneral";
            this.tabSelectorGeneral.Size = new System.Drawing.Size(173, 449);
            this.tabSelectorGeneral.TabIndex = 8;
            this.tabSelectorGeneral.TabStop = false;
            this.tabSelectorGeneral.Text = "materialTabSelectorVertical1";
            this.tabSelectorGeneral.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // tabControllerGeneral
            // 
            this.tabControllerGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControllerGeneral.Controls.Add(this.tabPageOracleMode);
            this.tabControllerGeneral.Controls.Add(this.tabPageFateSelection);
            this.tabControllerGeneral.Controls.Add(this.tabPageDowntime);
            this.tabControllerGeneral.Controls.Add(this.tabPageZoneChanging);
            this.tabControllerGeneral.Controls.Add(this.tabPageMiscellaneous);
            this.tabControllerGeneral.Depth = 0;
            this.tabControllerGeneral.Location = new System.Drawing.Point(172, 0);
            this.tabControllerGeneral.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.tabControllerGeneral.MinimumSize = new System.Drawing.Size(674, 449);
            this.tabControllerGeneral.MouseState = MaterialSkin.MouseState.Hover;
            this.tabControllerGeneral.Name = "tabControllerGeneral";
            this.tabControllerGeneral.SelectedIndex = 0;
            this.tabControllerGeneral.Size = new System.Drawing.Size(674, 449);
            this.tabControllerGeneral.TabIndex = 2;
            this.tabControllerGeneral.TabStop = false;
            // 
            // tabPageOracleMode
            // 
            this.tabPageOracleMode.AutoScroll = true;
            this.tabPageOracleMode.BackColor = System.Drawing.Color.White;
            this.tabPageOracleMode.Controls.Add(this.tabControllerOracleMode);
            this.tabPageOracleMode.Controls.Add(this.labelOracleModeSetting);
            this.tabPageOracleMode.Controls.Add(this.comboBoxOracleModeSetting);
            this.tabPageOracleMode.Controls.Add(this.labelOracleModeTitle);
            this.tabPageOracleMode.Location = new System.Drawing.Point(4, 29);
            this.tabPageOracleMode.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageOracleMode.Name = "tabPageOracleMode";
            this.tabPageOracleMode.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOracleMode.Size = new System.Drawing.Size(666, 416);
            this.tabPageOracleMode.TabIndex = 0;
            this.tabPageOracleMode.Text = "Oracle Mode";
            this.tabPageOracleMode.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // tabControllerOracleMode
            // 
            this.tabControllerOracleMode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControllerOracleMode.Controls.Add(this.tabPageOracleModeFateGrind);
            this.tabControllerOracleMode.Controls.Add(this.tabPageOracleModeSpecificFate);
            this.tabControllerOracleMode.Controls.Add(this.tabPageOracleModeAtmaGrind);
            this.tabControllerOracleMode.Controls.Add(this.tabPageOracleModeAnimusGrind);
            this.tabControllerOracleMode.Controls.Add(this.tabPageOracleModeAnimaGrind);
            this.tabControllerOracleMode.Depth = 0;
            this.tabControllerOracleMode.Location = new System.Drawing.Point(3, 117);
            this.tabControllerOracleMode.Margin = new System.Windows.Forms.Padding(0);
            this.tabControllerOracleMode.MouseState = MaterialSkin.MouseState.Hover;
            this.tabControllerOracleMode.Name = "tabControllerOracleMode";
            this.tabControllerOracleMode.SelectedIndex = 0;
            this.tabControllerOracleMode.Size = new System.Drawing.Size(655, 276);
            this.tabControllerOracleMode.TabIndex = 4;
            this.tabControllerOracleMode.TabStop = false;
            // 
            // tabPageOracleModeFateGrind
            // 
            this.tabPageOracleModeFateGrind.BackColor = System.Drawing.Color.White;
            this.tabPageOracleModeFateGrind.Controls.Add(this.labelOracleModeFateGrind);
            this.tabPageOracleModeFateGrind.Location = new System.Drawing.Point(4, 29);
            this.tabPageOracleModeFateGrind.Name = "tabPageOracleModeFateGrind";
            this.tabPageOracleModeFateGrind.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOracleModeFateGrind.Size = new System.Drawing.Size(647, 243);
            this.tabPageOracleModeFateGrind.TabIndex = 0;
            this.tabPageOracleModeFateGrind.Text = "FATE Grind";
            this.tabPageOracleModeFateGrind.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelOracleModeFateGrind
            // 
            this.labelOracleModeFateGrind.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelOracleModeFateGrind.ForeColor = System.Drawing.Color.Black;
            this.labelOracleModeFateGrind.Location = new System.Drawing.Point(10, 0);
            this.labelOracleModeFateGrind.Name = "labelOracleModeFateGrind";
            this.labelOracleModeFateGrind.Size = new System.Drawing.Size(631, 226);
            this.labelOracleModeFateGrind.TabIndex = 1;
            this.labelOracleModeFateGrind.Text = resources.GetString("labelOracleModeFateGrind.Text");
            // 
            // tabPageOracleModeSpecificFate
            // 
            this.tabPageOracleModeSpecificFate.BackColor = System.Drawing.Color.White;
            this.tabPageOracleModeSpecificFate.Controls.Add(this.labelSpecificFateNameSetting);
            this.tabPageOracleModeSpecificFate.Controls.Add(this.textBoxSpecificFateNameSetting);
            this.tabPageOracleModeSpecificFate.Controls.Add(this.labelOracleModeSpecificFate);
            this.tabPageOracleModeSpecificFate.Location = new System.Drawing.Point(4, 22);
            this.tabPageOracleModeSpecificFate.Name = "tabPageOracleModeSpecificFate";
            this.tabPageOracleModeSpecificFate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOracleModeSpecificFate.Size = new System.Drawing.Size(647, 250);
            this.tabPageOracleModeSpecificFate.TabIndex = 1;
            this.tabPageOracleModeSpecificFate.Text = "Specific FATE";
            this.tabPageOracleModeSpecificFate.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelSpecificFateNameSetting
            // 
            this.labelSpecificFateNameSetting.AutoSize = true;
            this.labelSpecificFateNameSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSpecificFateNameSetting.Location = new System.Drawing.Point(28, 45);
            this.labelSpecificFateNameSetting.Name = "labelSpecificFateNameSetting";
            this.labelSpecificFateNameSetting.Size = new System.Drawing.Size(89, 18);
            this.labelSpecificFateNameSetting.TabIndex = 4;
            this.labelSpecificFateNameSetting.Text = "FATE Name:";
            // 
            // textBoxSpecificFateNameSetting
            // 
            this.textBoxSpecificFateNameSetting.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxSpecificFateNameSetting.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxSpecificFateNameSetting.Depth = 0;
            this.textBoxSpecificFateNameSetting.Hint = "";
            this.textBoxSpecificFateNameSetting.Location = new System.Drawing.Point(123, 45);
            this.textBoxSpecificFateNameSetting.MaxLength = 100;
            this.textBoxSpecificFateNameSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.textBoxSpecificFateNameSetting.Name = "textBoxSpecificFateNameSetting";
            this.textBoxSpecificFateNameSetting.PasswordChar = '\0';
            this.textBoxSpecificFateNameSetting.SelectedText = "";
            this.textBoxSpecificFateNameSetting.SelectionLength = 0;
            this.textBoxSpecificFateNameSetting.SelectionStart = 0;
            this.textBoxSpecificFateNameSetting.Size = new System.Drawing.Size(317, 25);
            this.textBoxSpecificFateNameSetting.TabIndex = 3;
            this.textBoxSpecificFateNameSetting.TabStop = false;
            this.textBoxSpecificFateNameSetting.UseSystemPasswordChar = false;
            this.textBoxSpecificFateNameSetting.Enter += new System.EventHandler(this.OnEnterSpecificFateName);
            this.textBoxSpecificFateNameSetting.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnEnterKeyDownDropFocus);
            this.textBoxSpecificFateNameSetting.TextChanged += new System.EventHandler(this.OnSpecificFateNameChanged);
            // 
            // labelOracleModeSpecificFate
            // 
            this.labelOracleModeSpecificFate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelOracleModeSpecificFate.AutoSize = true;
            this.labelOracleModeSpecificFate.BackColor = System.Drawing.Color.White;
            this.labelOracleModeSpecificFate.ForeColor = System.Drawing.Color.Black;
            this.labelOracleModeSpecificFate.Location = new System.Drawing.Point(10, 0);
            this.labelOracleModeSpecificFate.Name = "labelOracleModeSpecificFate";
            this.labelOracleModeSpecificFate.Size = new System.Drawing.Size(435, 20);
            this.labelOracleModeSpecificFate.TabIndex = 2;
            this.labelOracleModeSpecificFate.Text = "Specific FATE mode will only run the FATE you specify below.";
            // 
            // tabPageOracleModeAtmaGrind
            // 
            this.tabPageOracleModeAtmaGrind.BackColor = System.Drawing.Color.White;
            this.tabPageOracleModeAtmaGrind.Controls.Add(this.labelAtmaGrindNYI);
            this.tabPageOracleModeAtmaGrind.Location = new System.Drawing.Point(4, 22);
            this.tabPageOracleModeAtmaGrind.Name = "tabPageOracleModeAtmaGrind";
            this.tabPageOracleModeAtmaGrind.Size = new System.Drawing.Size(647, 250);
            this.tabPageOracleModeAtmaGrind.TabIndex = 2;
            this.tabPageOracleModeAtmaGrind.Text = "Atma Grind";
            this.tabPageOracleModeAtmaGrind.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelAtmaGrindNYI
            // 
            this.labelAtmaGrindNYI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAtmaGrindNYI.AutoSize = true;
            this.labelAtmaGrindNYI.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAtmaGrindNYI.ForeColor = System.Drawing.Color.Red;
            this.labelAtmaGrindNYI.Location = new System.Drawing.Point(10, 0);
            this.labelAtmaGrindNYI.Name = "labelAtmaGrindNYI";
            this.labelAtmaGrindNYI.Size = new System.Drawing.Size(285, 18);
            this.labelAtmaGrindNYI.TabIndex = 0;
            this.labelAtmaGrindNYI.Text = "Atma Grind mode is not yet implemented.";
            // 
            // tabPageOracleModeAnimusGrind
            // 
            this.tabPageOracleModeAnimusGrind.BackColor = System.Drawing.Color.White;
            this.tabPageOracleModeAnimusGrind.Controls.Add(this.labelAnimusGrindModeNYI);
            this.tabPageOracleModeAnimusGrind.Location = new System.Drawing.Point(4, 22);
            this.tabPageOracleModeAnimusGrind.Name = "tabPageOracleModeAnimusGrind";
            this.tabPageOracleModeAnimusGrind.Size = new System.Drawing.Size(647, 250);
            this.tabPageOracleModeAnimusGrind.TabIndex = 3;
            this.tabPageOracleModeAnimusGrind.Text = "Animus Grind";
            this.tabPageOracleModeAnimusGrind.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelAnimusGrindModeNYI
            // 
            this.labelAnimusGrindModeNYI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAnimusGrindModeNYI.AutoSize = true;
            this.labelAnimusGrindModeNYI.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAnimusGrindModeNYI.ForeColor = System.Drawing.Color.Red;
            this.labelAnimusGrindModeNYI.Location = new System.Drawing.Point(10, 0);
            this.labelAnimusGrindModeNYI.Name = "labelAnimusGrindModeNYI";
            this.labelAnimusGrindModeNYI.Size = new System.Drawing.Size(300, 18);
            this.labelAnimusGrindModeNYI.TabIndex = 1;
            this.labelAnimusGrindModeNYI.Text = "Animus Grind mode is not yet implemented.";
            // 
            // tabPageOracleModeAnimaGrind
            // 
            this.tabPageOracleModeAnimaGrind.BackColor = System.Drawing.Color.White;
            this.tabPageOracleModeAnimaGrind.Controls.Add(this.labelAnimaGrindMode);
            this.tabPageOracleModeAnimaGrind.Location = new System.Drawing.Point(4, 22);
            this.tabPageOracleModeAnimaGrind.Name = "tabPageOracleModeAnimaGrind";
            this.tabPageOracleModeAnimaGrind.Size = new System.Drawing.Size(647, 250);
            this.tabPageOracleModeAnimaGrind.TabIndex = 4;
            this.tabPageOracleModeAnimaGrind.Text = "Anima Grind";
            this.tabPageOracleModeAnimaGrind.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelAnimaGrindMode
            // 
            this.labelAnimaGrindMode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAnimaGrindMode.ForeColor = System.Drawing.Color.Black;
            this.labelAnimaGrindMode.Location = new System.Drawing.Point(10, 0);
            this.labelAnimaGrindMode.Name = "labelAnimaGrindMode";
            this.labelAnimaGrindMode.Size = new System.Drawing.Size(631, 240);
            this.labelAnimaGrindMode.TabIndex = 2;
            this.labelAnimaGrindMode.Text = resources.GetString("labelAnimaGrindMode.Text");
            // 
            // labelOracleModeSetting
            // 
            this.labelOracleModeSetting.AutoSize = true;
            this.labelOracleModeSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOracleModeSetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelOracleModeSetting.Location = new System.Drawing.Point(15, 68);
            this.labelOracleModeSetting.Name = "labelOracleModeSetting";
            this.labelOracleModeSetting.Size = new System.Drawing.Size(96, 18);
            this.labelOracleModeSetting.TabIndex = 3;
            this.labelOracleModeSetting.Text = "Oracle Mode:";
            // 
            // comboBoxOracleModeSetting
            // 
            this.comboBoxOracleModeSetting.BackColor = System.Drawing.Color.White;
            this.comboBoxOracleModeSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOracleModeSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxOracleModeSetting.ForeColor = System.Drawing.Color.Black;
            this.comboBoxOracleModeSetting.FormattingEnabled = true;
            this.comboBoxOracleModeSetting.Items.AddRange(new object[] {
            "FATE Grind",
            "Specific FATE",
            "Atma Grind",
            "Animus Grind",
            "Anima Grind"});
            this.comboBoxOracleModeSetting.Location = new System.Drawing.Point(117, 63);
            this.comboBoxOracleModeSetting.Name = "comboBoxOracleModeSetting";
            this.comboBoxOracleModeSetting.Size = new System.Drawing.Size(121, 28);
            this.comboBoxOracleModeSetting.TabIndex = 2;
            this.comboBoxOracleModeSetting.TabStop = false;
            this.comboBoxOracleModeSetting.SelectedIndexChanged += new System.EventHandler(this.OnOracleModeChanged);
            // 
            // labelOracleModeTitle
            // 
            this.labelOracleModeTitle.AutoSize = true;
            this.labelOracleModeTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOracleModeTitle.Location = new System.Drawing.Point(10, 10);
            this.labelOracleModeTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelOracleModeTitle.Name = "labelOracleModeTitle";
            this.labelOracleModeTitle.Size = new System.Drawing.Size(258, 38);
            this.labelOracleModeTitle.TabIndex = 1;
            this.labelOracleModeTitle.Text = "Oracle Mode Settings";
            // 
            // tabPageFateSelection
            // 
            this.tabPageFateSelection.BackColor = System.Drawing.Color.White;
            this.tabPageFateSelection.Controls.Add(this.labelFateSelectionDescription);
            this.tabPageFateSelection.Controls.Add(this.comboBoxFateSelectStrategySetting);
            this.tabPageFateSelection.Controls.Add(this.labelFateSelectStrategySetting);
            this.tabPageFateSelection.Controls.Add(this.labelFateSelectionTitle);
            this.tabPageFateSelection.Location = new System.Drawing.Point(4, 29);
            this.tabPageFateSelection.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageFateSelection.Name = "tabPageFateSelection";
            this.tabPageFateSelection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFateSelection.Size = new System.Drawing.Size(666, 416);
            this.tabPageFateSelection.TabIndex = 1;
            this.tabPageFateSelection.Text = "Fate Selection";
            this.tabPageFateSelection.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelFateSelectionDescription
            // 
            this.labelFateSelectionDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFateSelectionDescription.ForeColor = System.Drawing.Color.Black;
            this.labelFateSelectionDescription.Location = new System.Drawing.Point(13, 117);
            this.labelFateSelectionDescription.Name = "labelFateSelectionDescription";
            this.labelFateSelectionDescription.Size = new System.Drawing.Size(631, 203);
            this.labelFateSelectionDescription.TabIndex = 7;
            this.labelFateSelectionDescription.Text = "At the moment, Oracle will only select the closest FATE to you.\r\n\r\nMore strategie" +
    "s are coming soon!";
            // 
            // comboBoxFateSelectStrategySetting
            // 
            this.comboBoxFateSelectStrategySetting.BackColor = System.Drawing.Color.White;
            this.comboBoxFateSelectStrategySetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFateSelectStrategySetting.Enabled = false;
            this.comboBoxFateSelectStrategySetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxFateSelectStrategySetting.ForeColor = System.Drawing.Color.Black;
            this.comboBoxFateSelectStrategySetting.FormattingEnabled = true;
            this.comboBoxFateSelectStrategySetting.Items.AddRange(new object[] {
            "Closest FATE"});
            this.comboBoxFateSelectStrategySetting.Location = new System.Drawing.Point(169, 63);
            this.comboBoxFateSelectStrategySetting.Name = "comboBoxFateSelectStrategySetting";
            this.comboBoxFateSelectStrategySetting.Size = new System.Drawing.Size(121, 28);
            this.comboBoxFateSelectStrategySetting.TabIndex = 6;
            this.comboBoxFateSelectStrategySetting.TabStop = false;
            // 
            // labelFateSelectStrategySetting
            // 
            this.labelFateSelectStrategySetting.AutoSize = true;
            this.labelFateSelectStrategySetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFateSelectStrategySetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelFateSelectStrategySetting.Location = new System.Drawing.Point(15, 68);
            this.labelFateSelectStrategySetting.Name = "labelFateSelectStrategySetting";
            this.labelFateSelectStrategySetting.Size = new System.Drawing.Size(148, 18);
            this.labelFateSelectStrategySetting.TabIndex = 5;
            this.labelFateSelectStrategySetting.Text = "Fate Select Strategy:";
            // 
            // labelFateSelectionTitle
            // 
            this.labelFateSelectionTitle.AutoSize = true;
            this.labelFateSelectionTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFateSelectionTitle.Location = new System.Drawing.Point(10, 10);
            this.labelFateSelectionTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelFateSelectionTitle.Name = "labelFateSelectionTitle";
            this.labelFateSelectionTitle.Size = new System.Drawing.Size(279, 38);
            this.labelFateSelectionTitle.TabIndex = 4;
            this.labelFateSelectionTitle.Text = "Fate Selection Settings";
            // 
            // tabPageDowntime
            // 
            this.tabPageDowntime.BackColor = System.Drawing.Color.White;
            this.tabPageDowntime.Controls.Add(this.tabControllerDowntime);
            this.tabPageDowntime.Controls.Add(this.comboBoxFateWaitModeSetting);
            this.tabPageDowntime.Controls.Add(this.labelDowntimeBehaviourSetting);
            this.tabPageDowntime.Controls.Add(this.labelDowntimeTitle);
            this.tabPageDowntime.Location = new System.Drawing.Point(4, 29);
            this.tabPageDowntime.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageDowntime.Name = "tabPageDowntime";
            this.tabPageDowntime.Size = new System.Drawing.Size(666, 416);
            this.tabPageDowntime.TabIndex = 2;
            this.tabPageDowntime.Text = "Downtime";
            this.tabPageDowntime.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // tabControllerDowntime
            // 
            this.tabControllerDowntime.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControllerDowntime.Controls.Add(this.tabPageDowntimeReturnToAetheryte);
            this.tabControllerDowntime.Controls.Add(this.tabPageDowntimeMoveToLocation);
            this.tabControllerDowntime.Controls.Add(this.tabPageDowntimeGrindMobs);
            this.tabControllerDowntime.Controls.Add(this.tabPageDowntimeDoNothing);
            this.tabControllerDowntime.Depth = 0;
            this.tabControllerDowntime.Location = new System.Drawing.Point(3, 117);
            this.tabControllerDowntime.Margin = new System.Windows.Forms.Padding(0);
            this.tabControllerDowntime.MouseState = MaterialSkin.MouseState.Hover;
            this.tabControllerDowntime.Name = "tabControllerDowntime";
            this.tabControllerDowntime.SelectedIndex = 0;
            this.tabControllerDowntime.Size = new System.Drawing.Size(655, 283);
            this.tabControllerDowntime.TabIndex = 8;
            this.tabControllerDowntime.TabStop = false;
            // 
            // tabPageDowntimeReturnToAetheryte
            // 
            this.tabPageDowntimeReturnToAetheryte.BackColor = System.Drawing.Color.White;
            this.tabPageDowntimeReturnToAetheryte.Controls.Add(this.labelDowntimeReturnToAetheryte);
            this.tabPageDowntimeReturnToAetheryte.Location = new System.Drawing.Point(4, 29);
            this.tabPageDowntimeReturnToAetheryte.Name = "tabPageDowntimeReturnToAetheryte";
            this.tabPageDowntimeReturnToAetheryte.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDowntimeReturnToAetheryte.Size = new System.Drawing.Size(647, 250);
            this.tabPageDowntimeReturnToAetheryte.TabIndex = 0;
            this.tabPageDowntimeReturnToAetheryte.Text = "Return to Aetheryte";
            // 
            // labelDowntimeReturnToAetheryte
            // 
            this.labelDowntimeReturnToAetheryte.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDowntimeReturnToAetheryte.ForeColor = System.Drawing.Color.Black;
            this.labelDowntimeReturnToAetheryte.Location = new System.Drawing.Point(10, 0);
            this.labelDowntimeReturnToAetheryte.Name = "labelDowntimeReturnToAetheryte";
            this.labelDowntimeReturnToAetheryte.Size = new System.Drawing.Size(631, 233);
            this.labelDowntimeReturnToAetheryte.TabIndex = 1;
            this.labelDowntimeReturnToAetheryte.Text = "The Return to Aetheryte behaviour will move your character to the closest aethery" +
    "te crystal when there are no viable FATEs active.";
            // 
            // tabPageDowntimeMoveToLocation
            // 
            this.tabPageDowntimeMoveToLocation.BackColor = System.Drawing.Color.White;
            this.tabPageDowntimeMoveToLocation.Controls.Add(this.buttonDowntimeSetLocation);
            this.tabPageDowntimeMoveToLocation.Controls.Add(this.buttonDowntimeRefreshZone);
            this.tabPageDowntimeMoveToLocation.Controls.Add(this.labelDowntimeWaitLocationValue);
            this.tabPageDowntimeMoveToLocation.Controls.Add(this.labelDowntimeCurrentZoneValue);
            this.tabPageDowntimeMoveToLocation.Controls.Add(this.labelDowntimeWaitLocation);
            this.tabPageDowntimeMoveToLocation.Controls.Add(this.labelDowntimeCurrentZone);
            this.tabPageDowntimeMoveToLocation.Controls.Add(this.labelDowntimeMoveToLocation);
            this.tabPageDowntimeMoveToLocation.Location = new System.Drawing.Point(4, 29);
            this.tabPageDowntimeMoveToLocation.Name = "tabPageDowntimeMoveToLocation";
            this.tabPageDowntimeMoveToLocation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDowntimeMoveToLocation.Size = new System.Drawing.Size(647, 250);
            this.tabPageDowntimeMoveToLocation.TabIndex = 1;
            this.tabPageDowntimeMoveToLocation.Text = "Move to Location";
            // 
            // buttonDowntimeSetLocation
            // 
            this.buttonDowntimeSetLocation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonDowntimeSetLocation.BackColor = System.Drawing.Color.Lavender;
            this.buttonDowntimeSetLocation.Depth = 0;
            this.buttonDowntimeSetLocation.Location = new System.Drawing.Point(14, 122);
            this.buttonDowntimeSetLocation.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonDowntimeSetLocation.MouseState = MaterialSkin.MouseState.Hover;
            this.buttonDowntimeSetLocation.Name = "buttonDowntimeSetLocation";
            this.buttonDowntimeSetLocation.Primary = true;
            this.buttonDowntimeSetLocation.Size = new System.Drawing.Size(203, 35);
            this.buttonDowntimeSetLocation.TabIndex = 8;
            this.buttonDowntimeSetLocation.Text = "Set Current Location";
            this.buttonDowntimeSetLocation.UseVisualStyleBackColor = false;
            this.buttonDowntimeSetLocation.Click += new System.EventHandler(this.OnSetLocationClick);
            // 
            // buttonDowntimeRefreshZone
            // 
            this.buttonDowntimeRefreshZone.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonDowntimeRefreshZone.Depth = 0;
            this.buttonDowntimeRefreshZone.Location = new System.Drawing.Point(225, 122);
            this.buttonDowntimeRefreshZone.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonDowntimeRefreshZone.MouseState = MaterialSkin.MouseState.Hover;
            this.buttonDowntimeRefreshZone.Name = "buttonDowntimeRefreshZone";
            this.buttonDowntimeRefreshZone.Primary = true;
            this.buttonDowntimeRefreshZone.Size = new System.Drawing.Size(140, 35);
            this.buttonDowntimeRefreshZone.TabIndex = 7;
            this.buttonDowntimeRefreshZone.Text = "Refresh Zone";
            this.buttonDowntimeRefreshZone.UseVisualStyleBackColor = true;
            this.buttonDowntimeRefreshZone.Click += new System.EventHandler(this.OnRefreshZoneClick);
            // 
            // labelDowntimeWaitLocationValue
            // 
            this.labelDowntimeWaitLocationValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDowntimeWaitLocationValue.AutoSize = true;
            this.labelDowntimeWaitLocationValue.BackColor = System.Drawing.Color.White;
            this.labelDowntimeWaitLocationValue.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDowntimeWaitLocationValue.ForeColor = System.Drawing.Color.Black;
            this.labelDowntimeWaitLocationValue.Location = new System.Drawing.Point(164, 85);
            this.labelDowntimeWaitLocationValue.Name = "labelDowntimeWaitLocationValue";
            this.labelDowntimeWaitLocationValue.Size = new System.Drawing.Size(43, 18);
            this.labelDowntimeWaitLocationValue.TabIndex = 6;
            this.labelDowntimeWaitLocationValue.Text = "None";
            // 
            // labelDowntimeCurrentZoneValue
            // 
            this.labelDowntimeCurrentZoneValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDowntimeCurrentZoneValue.AutoSize = true;
            this.labelDowntimeCurrentZoneValue.BackColor = System.Drawing.Color.White;
            this.labelDowntimeCurrentZoneValue.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDowntimeCurrentZoneValue.ForeColor = System.Drawing.Color.Black;
            this.labelDowntimeCurrentZoneValue.Location = new System.Drawing.Point(164, 55);
            this.labelDowntimeCurrentZoneValue.Name = "labelDowntimeCurrentZoneValue";
            this.labelDowntimeCurrentZoneValue.Size = new System.Drawing.Size(17, 18);
            this.labelDowntimeCurrentZoneValue.TabIndex = 5;
            this.labelDowntimeCurrentZoneValue.Text = "0";
            // 
            // labelDowntimeWaitLocation
            // 
            this.labelDowntimeWaitLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDowntimeWaitLocation.BackColor = System.Drawing.Color.White;
            this.labelDowntimeWaitLocation.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDowntimeWaitLocation.ForeColor = System.Drawing.Color.Black;
            this.labelDowntimeWaitLocation.Location = new System.Drawing.Point(28, 85);
            this.labelDowntimeWaitLocation.Name = "labelDowntimeWaitLocation";
            this.labelDowntimeWaitLocation.Size = new System.Drawing.Size(130, 22);
            this.labelDowntimeWaitLocation.TabIndex = 4;
            this.labelDowntimeWaitLocation.Text = "Wait Location: ";
            // 
            // labelDowntimeCurrentZone
            // 
            this.labelDowntimeCurrentZone.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDowntimeCurrentZone.BackColor = System.Drawing.Color.White;
            this.labelDowntimeCurrentZone.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDowntimeCurrentZone.ForeColor = System.Drawing.Color.Black;
            this.labelDowntimeCurrentZone.Location = new System.Drawing.Point(28, 55);
            this.labelDowntimeCurrentZone.Name = "labelDowntimeCurrentZone";
            this.labelDowntimeCurrentZone.Size = new System.Drawing.Size(130, 22);
            this.labelDowntimeCurrentZone.TabIndex = 3;
            this.labelDowntimeCurrentZone.Text = "Current Zone Id: ";
            // 
            // labelDowntimeMoveToLocation
            // 
            this.labelDowntimeMoveToLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDowntimeMoveToLocation.BackColor = System.Drawing.Color.White;
            this.labelDowntimeMoveToLocation.ForeColor = System.Drawing.Color.Black;
            this.labelDowntimeMoveToLocation.Location = new System.Drawing.Point(10, 0);
            this.labelDowntimeMoveToLocation.Name = "labelDowntimeMoveToLocation";
            this.labelDowntimeMoveToLocation.Size = new System.Drawing.Size(631, 41);
            this.labelDowntimeMoveToLocation.TabIndex = 2;
            this.labelDowntimeMoveToLocation.Text = "The Move to Location behaviour will move you to a location of your choosing when " +
    "there are no viable FATEs active. Each zone has its own location.";
            // 
            // tabPageDowntimeGrindMobs
            // 
            this.tabPageDowntimeGrindMobs.BackColor = System.Drawing.Color.White;
            this.tabPageDowntimeGrindMobs.Controls.Add(this.numericUpDownMobMaximumLevelAboveSetting);
            this.tabPageDowntimeGrindMobs.Controls.Add(this.numericUpDownMobMinimumLevelBelowSetting);
            this.tabPageDowntimeGrindMobs.Controls.Add(this.labelMobMaxLevelAboveSetting);
            this.tabPageDowntimeGrindMobs.Controls.Add(this.labelMobMinLevelBelowSetting);
            this.tabPageDowntimeGrindMobs.Controls.Add(this.labelDowntimeGrindMobs);
            this.tabPageDowntimeGrindMobs.Location = new System.Drawing.Point(4, 22);
            this.tabPageDowntimeGrindMobs.Name = "tabPageDowntimeGrindMobs";
            this.tabPageDowntimeGrindMobs.Size = new System.Drawing.Size(647, 257);
            this.tabPageDowntimeGrindMobs.TabIndex = 2;
            this.tabPageDowntimeGrindMobs.Text = "Grind Mobs";
            // 
            // numericUpDownMobMaximumLevelAboveSetting
            // 
            this.numericUpDownMobMaximumLevelAboveSetting.AutoSize = true;
            this.numericUpDownMobMaximumLevelAboveSetting.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownMobMaximumLevelAboveSetting.Location = new System.Drawing.Point(232, 84);
            this.numericUpDownMobMaximumLevelAboveSetting.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDownMobMaximumLevelAboveSetting.Name = "numericUpDownMobMaximumLevelAboveSetting";
            this.numericUpDownMobMaximumLevelAboveSetting.Size = new System.Drawing.Size(46, 23);
            this.numericUpDownMobMaximumLevelAboveSetting.TabIndex = 8;
            this.numericUpDownMobMaximumLevelAboveSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownMobMaximumLevelAboveSetting.ValueChanged += new System.EventHandler(this.OnMobMaximumLevelAboveChanged);
            this.numericUpDownMobMaximumLevelAboveSetting.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnEnterKeyDownDropFocus);
            // 
            // numericUpDownMobMinimumLevelBelowSetting
            // 
            this.numericUpDownMobMinimumLevelBelowSetting.AutoSize = true;
            this.numericUpDownMobMinimumLevelBelowSetting.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownMobMinimumLevelBelowSetting.Location = new System.Drawing.Point(232, 54);
            this.numericUpDownMobMinimumLevelBelowSetting.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDownMobMinimumLevelBelowSetting.Name = "numericUpDownMobMinimumLevelBelowSetting";
            this.numericUpDownMobMinimumLevelBelowSetting.Size = new System.Drawing.Size(46, 23);
            this.numericUpDownMobMinimumLevelBelowSetting.TabIndex = 7;
            this.numericUpDownMobMinimumLevelBelowSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownMobMinimumLevelBelowSetting.ValueChanged += new System.EventHandler(this.OnMobMinimumLevelBelowChanged);
            this.numericUpDownMobMinimumLevelBelowSetting.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnEnterKeyDownDropFocus);
            // 
            // labelMobMaxLevelAboveSetting
            // 
            this.labelMobMaxLevelAboveSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMobMaxLevelAboveSetting.AutoSize = true;
            this.labelMobMaxLevelAboveSetting.BackColor = System.Drawing.Color.White;
            this.labelMobMaxLevelAboveSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMobMaxLevelAboveSetting.ForeColor = System.Drawing.Color.Black;
            this.labelMobMaxLevelAboveSetting.Location = new System.Drawing.Point(28, 85);
            this.labelMobMaxLevelAboveSetting.Name = "labelMobMaxLevelAboveSetting";
            this.labelMobMaxLevelAboveSetting.Size = new System.Drawing.Size(198, 18);
            this.labelMobMaxLevelAboveSetting.TabIndex = 6;
            this.labelMobMaxLevelAboveSetting.Text = "Mob Maximum Level Above:";
            // 
            // labelMobMinLevelBelowSetting
            // 
            this.labelMobMinLevelBelowSetting.AutoSize = true;
            this.labelMobMinLevelBelowSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMobMinLevelBelowSetting.Location = new System.Drawing.Point(28, 55);
            this.labelMobMinLevelBelowSetting.Name = "labelMobMinLevelBelowSetting";
            this.labelMobMinLevelBelowSetting.Size = new System.Drawing.Size(193, 18);
            this.labelMobMinLevelBelowSetting.TabIndex = 5;
            this.labelMobMinLevelBelowSetting.Text = "Mob Minimum Level Below:";
            // 
            // labelDowntimeGrindMobs
            // 
            this.labelDowntimeGrindMobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDowntimeGrindMobs.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDowntimeGrindMobs.ForeColor = System.Drawing.Color.Black;
            this.labelDowntimeGrindMobs.Location = new System.Drawing.Point(10, 0);
            this.labelDowntimeGrindMobs.Name = "labelDowntimeGrindMobs";
            this.labelDowntimeGrindMobs.Size = new System.Drawing.Size(631, 47);
            this.labelDowntimeGrindMobs.TabIndex = 0;
            this.labelDowntimeGrindMobs.Text = "The Grind Mobs behaviour will kill nearby enemy mobs when there are no viable FAT" +
    "Es active. You can blacklist mobs in the Blacklist tab.";
            // 
            // tabPageDowntimeDoNothing
            // 
            this.tabPageDowntimeDoNothing.BackColor = System.Drawing.Color.White;
            this.tabPageDowntimeDoNothing.Controls.Add(this.labelDowntimeDoNothing);
            this.tabPageDowntimeDoNothing.Location = new System.Drawing.Point(4, 22);
            this.tabPageDowntimeDoNothing.Name = "tabPageDowntimeDoNothing";
            this.tabPageDowntimeDoNothing.Size = new System.Drawing.Size(647, 257);
            this.tabPageDowntimeDoNothing.TabIndex = 3;
            this.tabPageDowntimeDoNothing.Text = "Do Nothing";
            // 
            // labelDowntimeDoNothing
            // 
            this.labelDowntimeDoNothing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDowntimeDoNothing.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDowntimeDoNothing.ForeColor = System.Drawing.Color.Black;
            this.labelDowntimeDoNothing.Location = new System.Drawing.Point(10, 0);
            this.labelDowntimeDoNothing.Name = "labelDowntimeDoNothing";
            this.labelDowntimeDoNothing.Size = new System.Drawing.Size(631, 247);
            this.labelDowntimeDoNothing.TabIndex = 1;
            this.labelDowntimeDoNothing.Text = "The Do Nothing behaviour will stand still when there\'s no viable FATEs active. Th" +
    "e bot will defend itself if attacked.";
            // 
            // comboBoxFateWaitModeSetting
            // 
            this.comboBoxFateWaitModeSetting.BackColor = System.Drawing.Color.White;
            this.comboBoxFateWaitModeSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFateWaitModeSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxFateWaitModeSetting.ForeColor = System.Drawing.Color.Black;
            this.comboBoxFateWaitModeSetting.FormattingEnabled = true;
            this.comboBoxFateWaitModeSetting.Items.AddRange(new object[] {
            "Return to Aetheryte",
            "Move to Location",
            "Grind Mobs",
            "Do Nothing"});
            this.comboBoxFateWaitModeSetting.Location = new System.Drawing.Point(171, 63);
            this.comboBoxFateWaitModeSetting.Name = "comboBoxFateWaitModeSetting";
            this.comboBoxFateWaitModeSetting.Size = new System.Drawing.Size(160, 28);
            this.comboBoxFateWaitModeSetting.TabIndex = 7;
            this.comboBoxFateWaitModeSetting.TabStop = false;
            this.comboBoxFateWaitModeSetting.SelectedIndexChanged += new System.EventHandler(this.OnFateWaitModeChanged);
            // 
            // labelDowntimeBehaviourSetting
            // 
            this.labelDowntimeBehaviourSetting.AutoSize = true;
            this.labelDowntimeBehaviourSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDowntimeBehaviourSetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelDowntimeBehaviourSetting.Location = new System.Drawing.Point(15, 68);
            this.labelDowntimeBehaviourSetting.Name = "labelDowntimeBehaviourSetting";
            this.labelDowntimeBehaviourSetting.Size = new System.Drawing.Size(150, 18);
            this.labelDowntimeBehaviourSetting.TabIndex = 6;
            this.labelDowntimeBehaviourSetting.Text = "Downtime Behaviour:";
            // 
            // labelDowntimeTitle
            // 
            this.labelDowntimeTitle.AutoSize = true;
            this.labelDowntimeTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDowntimeTitle.Location = new System.Drawing.Point(10, 10);
            this.labelDowntimeTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelDowntimeTitle.Name = "labelDowntimeTitle";
            this.labelDowntimeTitle.Size = new System.Drawing.Size(230, 38);
            this.labelDowntimeTitle.TabIndex = 5;
            this.labelDowntimeTitle.Text = "Downtime Settings";
            // 
            // tabPageZoneChanging
            // 
            this.tabPageZoneChanging.BackColor = System.Drawing.Color.White;
            this.tabPageZoneChanging.Controls.Add(this.checkBoxBindHomePointSetting);
            this.tabPageZoneChanging.Controls.Add(this.labelBindHomePointSetting);
            this.tabPageZoneChanging.Controls.Add(this.buttonResetZoneLevelsToDefault);
            this.tabPageZoneChanging.Controls.Add(this.labelZoneChangeTip);
            this.tabPageZoneChanging.Controls.Add(this.dataGridViewZoneChangeSettings);
            this.tabPageZoneChanging.Controls.Add(this.labelZoneChangeEnabledSetting);
            this.tabPageZoneChanging.Controls.Add(this.checkBoxZoneChangingEnabledSetting);
            this.tabPageZoneChanging.Controls.Add(this.labelZoneChangingTitle);
            this.tabPageZoneChanging.Location = new System.Drawing.Point(4, 22);
            this.tabPageZoneChanging.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageZoneChanging.Name = "tabPageZoneChanging";
            this.tabPageZoneChanging.Size = new System.Drawing.Size(666, 423);
            this.tabPageZoneChanging.TabIndex = 3;
            this.tabPageZoneChanging.Text = "Zone Changing";
            this.tabPageZoneChanging.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // checkBoxBindHomePointSetting
            // 
            this.checkBoxBindHomePointSetting.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxBindHomePointSetting.AutoSize = true;
            this.checkBoxBindHomePointSetting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxBindHomePointSetting.Depth = 0;
            this.checkBoxBindHomePointSetting.Font = new System.Drawing.Font("Roboto Medium", 11F);
            this.checkBoxBindHomePointSetting.Location = new System.Drawing.Point(393, 62);
            this.checkBoxBindHomePointSetting.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxBindHomePointSetting.MouseLocation = new System.Drawing.Point(-1, -1);
            this.checkBoxBindHomePointSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.checkBoxBindHomePointSetting.Name = "checkBoxBindHomePointSetting";
            this.checkBoxBindHomePointSetting.Ripple = true;
            this.checkBoxBindHomePointSetting.Size = new System.Drawing.Size(30, 30);
            this.checkBoxBindHomePointSetting.TabIndex = 15;
            this.checkBoxBindHomePointSetting.Text = "Bind Home Point";
            this.checkBoxBindHomePointSetting.UseVisualStyleBackColor = true;
            this.checkBoxBindHomePointSetting.CheckedChanged += new System.EventHandler(this.OnBindHomePointChanged);
            // 
            // labelBindHomePointSetting
            // 
            this.labelBindHomePointSetting.AutoSize = true;
            this.labelBindHomePointSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBindHomePointSetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelBindHomePointSetting.Location = new System.Drawing.Point(261, 68);
            this.labelBindHomePointSetting.Name = "labelBindHomePointSetting";
            this.labelBindHomePointSetting.Size = new System.Drawing.Size(129, 18);
            this.labelBindHomePointSetting.TabIndex = 14;
            this.labelBindHomePointSetting.Text = "Bind Home Point: ";
            // 
            // buttonResetZoneLevelsToDefault
            // 
            this.buttonResetZoneLevelsToDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonResetZoneLevelsToDefault.Depth = 0;
            this.buttonResetZoneLevelsToDefault.Location = new System.Drawing.Point(16, 352);
            this.buttonResetZoneLevelsToDefault.MouseState = MaterialSkin.MouseState.Hover;
            this.buttonResetZoneLevelsToDefault.Name = "buttonResetZoneLevelsToDefault";
            this.buttonResetZoneLevelsToDefault.Primary = true;
            this.buttonResetZoneLevelsToDefault.Size = new System.Drawing.Size(163, 33);
            this.buttonResetZoneLevelsToDefault.TabIndex = 13;
            this.buttonResetZoneLevelsToDefault.Text = "Reset to Default";
            this.buttonResetZoneLevelsToDefault.UseVisualStyleBackColor = true;
            this.buttonResetZoneLevelsToDefault.Click += new System.EventHandler(this.OnResetZoneLevelsToDefaultClick);
            // 
            // labelZoneChangeTip
            // 
            this.labelZoneChangeTip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelZoneChangeTip.BackColor = System.Drawing.Color.Transparent;
            this.labelZoneChangeTip.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZoneChangeTip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelZoneChangeTip.Location = new System.Drawing.Point(422, 103);
            this.labelZoneChangeTip.Name = "labelZoneChangeTip";
            this.labelZoneChangeTip.Size = new System.Drawing.Size(220, 297);
            this.labelZoneChangeTip.TabIndex = 11;
            this.labelZoneChangeTip.Text = resources.GetString("labelZoneChangeTip.Text");
            // 
            // dataGridViewZoneChangeSettings
            // 
            this.dataGridViewZoneChangeSettings.AllowUserToAddRows = false;
            this.dataGridViewZoneChangeSettings.AllowUserToDeleteRows = false;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.White;
            this.dataGridViewZoneChangeSettings.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewZoneChangeSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewZoneChangeSettings.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewZoneChangeSettings.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewZoneChangeSettings.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGridViewZoneChangeSettings.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.MediumSlateBlue;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewZoneChangeSettings.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewZoneChangeSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewZoneChangeSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnLevel,
            this.ColumnAetheryte,
            this.EmptyColumn});
            this.dataGridViewZoneChangeSettings.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewZoneChangeSettings.GridColor = System.Drawing.Color.Black;
            this.dataGridViewZoneChangeSettings.Location = new System.Drawing.Point(17, 108);
            this.dataGridViewZoneChangeSettings.Name = "dataGridViewZoneChangeSettings";
            this.dataGridViewZoneChangeSettings.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle17.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewZoneChangeSettings.RowHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dataGridViewZoneChangeSettings.RowHeadersVisible = false;
            this.dataGridViewZoneChangeSettings.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle18.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(73)))), ((int)(((byte)(171)))));
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridViewZoneChangeSettings.RowsDefaultCellStyle = dataGridViewCellStyle18;
            this.dataGridViewZoneChangeSettings.RowTemplate.Height = 26;
            this.dataGridViewZoneChangeSettings.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewZoneChangeSettings.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewZoneChangeSettings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewZoneChangeSettings.Size = new System.Drawing.Size(384, 228);
            this.dataGridViewZoneChangeSettings.TabIndex = 10;
            this.dataGridViewZoneChangeSettings.TabStop = false;
            this.dataGridViewZoneChangeSettings.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.OnDataGridViewCellClick);
            this.dataGridViewZoneChangeSettings.Paint += new System.Windows.Forms.PaintEventHandler(this.OnDataGridViewPaint);
            // 
            // ColumnLevel
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(73)))), ((int)(((byte)(171)))));
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.White;
            this.ColumnLevel.DefaultCellStyle = dataGridViewCellStyle15;
            this.ColumnLevel.HeaderText = "Level";
            this.ColumnLevel.Name = "ColumnLevel";
            this.ColumnLevel.ReadOnly = true;
            this.ColumnLevel.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ColumnAetheryte
            // 
            this.ColumnAetheryte.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(73)))), ((int)(((byte)(171)))));
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.White;
            this.ColumnAetheryte.DefaultCellStyle = dataGridViewCellStyle16;
            this.ColumnAetheryte.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.ColumnAetheryte.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColumnAetheryte.HeaderText = "Aetheryte";
            this.ColumnAetheryte.Items.AddRange(new object[] {
            "New Gridania",
            "Bentbranch Meadows",
            "The Hawthorne Hut",
            "Quarrymill",
            "Camp Tranquil",
            "Fallgourd Float",
            "Limsa Lominsa Lower Decks",
            "Ul\'dah - Steps of Nald",
            "Moraby Drydocks",
            "Costa del Sol",
            "Wineport",
            "Swiftperch",
            "Aleport",
            "Camp Bronze Lake",
            "Camp Overlook",
            "Horizon",
            "Camp Drybone",
            "Little Ala Mhigo",
            "Forgotten Springs",
            "Camp Bluefog",
            "Ceruleum Processing Plant",
            "Camp Dragonhead",
            "Revenant\'s Toll",
            "Summerford Farms",
            "Black Brush Station",
            "Wolves\' Den Pier",
            "Estate Hall (Free Company)",
            "Estate Hall (Free Company)",
            "Estate Hall (Free Company)",
            "Estate Hall (Private)",
            "Estate Hall (Private)",
            "Estate Hall (Private)",
            "The Gold Saucer",
            "Foundation",
            "Falcon\'s Nest",
            "Camp Cloudtop",
            "Ok\' Zundu",
            "Helix",
            "Idyllshire",
            "Tailfeather",
            "Anyx Trine",
            "Moghome",
            "Zenith"});
            this.ColumnAetheryte.Name = "ColumnAetheryte";
            this.ColumnAetheryte.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // EmptyColumn
            // 
            this.EmptyColumn.HeaderText = "";
            this.EmptyColumn.Name = "EmptyColumn";
            this.EmptyColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.EmptyColumn.Width = 5;
            // 
            // labelZoneChangeEnabledSetting
            // 
            this.labelZoneChangeEnabledSetting.AutoSize = true;
            this.labelZoneChangeEnabledSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZoneChangeEnabledSetting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelZoneChangeEnabledSetting.Location = new System.Drawing.Point(15, 68);
            this.labelZoneChangeEnabledSetting.Name = "labelZoneChangeEnabledSetting";
            this.labelZoneChangeEnabledSetting.Size = new System.Drawing.Size(173, 18);
            this.labelZoneChangeEnabledSetting.TabIndex = 9;
            this.labelZoneChangeEnabledSetting.Text = "Zone Changing Enabled: ";
            // 
            // checkBoxZoneChangingEnabledSetting
            // 
            this.checkBoxZoneChangingEnabledSetting.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxZoneChangingEnabledSetting.AutoSize = true;
            this.checkBoxZoneChangingEnabledSetting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxZoneChangingEnabledSetting.Depth = 0;
            this.checkBoxZoneChangingEnabledSetting.Font = new System.Drawing.Font("Roboto Medium", 11F);
            this.checkBoxZoneChangingEnabledSetting.Location = new System.Drawing.Point(191, 62);
            this.checkBoxZoneChangingEnabledSetting.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxZoneChangingEnabledSetting.MouseLocation = new System.Drawing.Point(-1, -1);
            this.checkBoxZoneChangingEnabledSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.checkBoxZoneChangingEnabledSetting.Name = "checkBoxZoneChangingEnabledSetting";
            this.checkBoxZoneChangingEnabledSetting.Ripple = true;
            this.checkBoxZoneChangingEnabledSetting.Size = new System.Drawing.Size(30, 30);
            this.checkBoxZoneChangingEnabledSetting.TabIndex = 8;
            this.checkBoxZoneChangingEnabledSetting.Text = "Zone Change Enabled";
            this.checkBoxZoneChangingEnabledSetting.UseVisualStyleBackColor = true;
            this.checkBoxZoneChangingEnabledSetting.CheckedChanged += new System.EventHandler(this.OnZoneChangingEnabledChanged);
            // 
            // labelZoneChangingTitle
            // 
            this.labelZoneChangingTitle.AutoSize = true;
            this.labelZoneChangingTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZoneChangingTitle.Location = new System.Drawing.Point(10, 10);
            this.labelZoneChangingTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelZoneChangingTitle.Name = "labelZoneChangingTitle";
            this.labelZoneChangingTitle.Size = new System.Drawing.Size(284, 38);
            this.labelZoneChangingTitle.TabIndex = 5;
            this.labelZoneChangingTitle.Text = "Zone Changing Settings";
            // 
            // tabPageMiscellaneous
            // 
            this.tabPageMiscellaneous.BackColor = System.Drawing.Color.White;
            this.tabPageMiscellaneous.Controls.Add(this.labelMiscellaneousTitle);
            this.tabPageMiscellaneous.Location = new System.Drawing.Point(4, 22);
            this.tabPageMiscellaneous.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMiscellaneous.Name = "tabPageMiscellaneous";
            this.tabPageMiscellaneous.Size = new System.Drawing.Size(666, 423);
            this.tabPageMiscellaneous.TabIndex = 4;
            this.tabPageMiscellaneous.Text = "Miscellaneous";
            this.tabPageMiscellaneous.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelMiscellaneousTitle
            // 
            this.labelMiscellaneousTitle.AutoSize = true;
            this.labelMiscellaneousTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMiscellaneousTitle.Location = new System.Drawing.Point(10, 10);
            this.labelMiscellaneousTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelMiscellaneousTitle.Name = "labelMiscellaneousTitle";
            this.labelMiscellaneousTitle.Size = new System.Drawing.Size(280, 38);
            this.labelMiscellaneousTitle.TabIndex = 5;
            this.labelMiscellaneousTitle.Text = "Miscellaneous Settings";
            // 
            // tabFateSettings
            // 
            this.tabFateSettings.BackColor = System.Drawing.Color.White;
            this.tabFateSettings.Controls.Add(this.tabControllerFate);
            this.tabFateSettings.Controls.Add(this.tabSelectorFate);
            this.tabFateSettings.Location = new System.Drawing.Point(4, 22);
            this.tabFateSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tabFateSettings.Name = "tabFateSettings";
            this.tabFateSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabFateSettings.Size = new System.Drawing.Size(842, 495);
            this.tabFateSettings.TabIndex = 1;
            this.tabFateSettings.Text = "Fate Settings";
            this.tabFateSettings.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // tabControllerFate
            // 
            this.tabControllerFate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControllerFate.Controls.Add(this.tabPageGeneral);
            this.tabControllerFate.Controls.Add(this.tabPageKillFates);
            this.tabControllerFate.Controls.Add(this.tabPageCollectFates);
            this.tabControllerFate.Controls.Add(this.tabPageEscortFates);
            this.tabControllerFate.Controls.Add(this.tabPageDefenceFates);
            this.tabControllerFate.Controls.Add(this.tabPageBossFates);
            this.tabControllerFate.Controls.Add(this.tabPageMegaBossFates);
            this.tabControllerFate.Depth = 0;
            this.tabControllerFate.Location = new System.Drawing.Point(172, 0);
            this.tabControllerFate.Margin = new System.Windows.Forms.Padding(0);
            this.tabControllerFate.MinimumSize = new System.Drawing.Size(674, 449);
            this.tabControllerFate.MouseState = MaterialSkin.MouseState.Hover;
            this.tabControllerFate.Multiline = true;
            this.tabControllerFate.Name = "tabControllerFate";
            this.tabControllerFate.SelectedIndex = 0;
            this.tabControllerFate.Size = new System.Drawing.Size(674, 449);
            this.tabControllerFate.TabIndex = 9;
            this.tabControllerFate.TabStop = false;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.BackColor = System.Drawing.Color.White;
            this.tabPageGeneral.Controls.Add(this.labelChainFateWaitTimeoutSettingSuffix);
            this.tabPageGeneral.Controls.Add(this.textBoxChainFateWaitTimeoutSetting);
            this.tabPageGeneral.Controls.Add(this.labelChainFateWaitTimeoutSetting);
            this.tabPageGeneral.Controls.Add(this.checkBoxWaitForChainFateSetting);
            this.tabPageGeneral.Controls.Add(this.labelWaitForChainFateSetting);
            this.tabPageGeneral.Controls.Add(this.labelLowDurationTimeSettingSuffix);
            this.tabPageGeneral.Controls.Add(this.textBoxLowRemainingFateDurationSetting);
            this.tabPageGeneral.Controls.Add(this.labelLowDurationTimeSetting);
            this.tabPageGeneral.Controls.Add(this.checkBoxIgnoreLowDurationUnstartedFateSetting);
            this.tabPageGeneral.Controls.Add(this.labelIgnoreLowDurationFatesSetting);
            this.tabPageGeneral.Controls.Add(this.checkBoxRunProblematicFatesSetting);
            this.tabPageGeneral.Controls.Add(this.labelRunProblematicFatesWarning);
            this.tabPageGeneral.Controls.Add(this.labelRunProblematicFatesSetting);
            this.tabPageGeneral.Controls.Add(this.numericUpDownFateMaximumLevelAboveSetting);
            this.tabPageGeneral.Controls.Add(this.labelFateMaximumLevelAboveSetting);
            this.tabPageGeneral.Controls.Add(this.numericUpDownFateMinimumLevelBelowSetting);
            this.tabPageGeneral.Controls.Add(this.labelFateMinimumLevelBelowSetting);
            this.tabPageGeneral.Controls.Add(this.labelGeneralFateSettingsTitle);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 54);
            this.tabPageGeneral.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(666, 391);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelChainFateWaitTimeoutSettingSuffix
            // 
            this.labelChainFateWaitTimeoutSettingSuffix.AutoSize = true;
            this.labelChainFateWaitTimeoutSettingSuffix.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChainFateWaitTimeoutSettingSuffix.Location = new System.Drawing.Point(268, 186);
            this.labelChainFateWaitTimeoutSettingSuffix.Name = "labelChainFateWaitTimeoutSettingSuffix";
            this.labelChainFateWaitTimeoutSettingSuffix.Size = new System.Drawing.Size(68, 20);
            this.labelChainFateWaitTimeoutSettingSuffix.TabIndex = 23;
            this.labelChainFateWaitTimeoutSettingSuffix.Text = "seconds";
            // 
            // textBoxChainFateWaitTimeoutSetting
            // 
            this.textBoxChainFateWaitTimeoutSetting.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxChainFateWaitTimeoutSetting.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxChainFateWaitTimeoutSetting.Depth = 0;
            this.textBoxChainFateWaitTimeoutSetting.Hint = "";
            this.textBoxChainFateWaitTimeoutSetting.Location = new System.Drawing.Point(219, 188);
            this.textBoxChainFateWaitTimeoutSetting.MaxLength = 4;
            this.textBoxChainFateWaitTimeoutSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.textBoxChainFateWaitTimeoutSetting.Name = "textBoxChainFateWaitTimeoutSetting";
            this.textBoxChainFateWaitTimeoutSetting.PasswordChar = '\0';
            this.textBoxChainFateWaitTimeoutSetting.SelectedText = "";
            this.textBoxChainFateWaitTimeoutSetting.SelectionLength = 0;
            this.textBoxChainFateWaitTimeoutSetting.SelectionStart = 0;
            this.textBoxChainFateWaitTimeoutSetting.Size = new System.Drawing.Size(43, 25);
            this.textBoxChainFateWaitTimeoutSetting.TabIndex = 22;
            this.textBoxChainFateWaitTimeoutSetting.TabStop = false;
            this.textBoxChainFateWaitTimeoutSetting.UseSystemPasswordChar = false;
            this.textBoxChainFateWaitTimeoutSetting.Enter += new System.EventHandler(this.OnEnterChainFateWaitTime);
            this.textBoxChainFateWaitTimeoutSetting.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnEnterKeyDownDropFocus);
            this.textBoxChainFateWaitTimeoutSetting.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnChainFateWaitTimeKeyPress);
            this.textBoxChainFateWaitTimeoutSetting.TextChanged += new System.EventHandler(this.OnChainFateWaitTimeChanged);
            // 
            // labelChainFateWaitTimeoutSetting
            // 
            this.labelChainFateWaitTimeoutSetting.AutoSize = true;
            this.labelChainFateWaitTimeoutSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChainFateWaitTimeoutSetting.Location = new System.Drawing.Point(15, 188);
            this.labelChainFateWaitTimeoutSetting.Name = "labelChainFateWaitTimeoutSetting";
            this.labelChainFateWaitTimeoutSetting.Size = new System.Drawing.Size(173, 18);
            this.labelChainFateWaitTimeoutSetting.TabIndex = 21;
            this.labelChainFateWaitTimeoutSetting.Text = "Wait Time for Follow-Up:";
            // 
            // checkBoxWaitForChainFateSetting
            // 
            this.checkBoxWaitForChainFateSetting.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxWaitForChainFateSetting.AutoSize = true;
            this.checkBoxWaitForChainFateSetting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxWaitForChainFateSetting.Depth = 0;
            this.checkBoxWaitForChainFateSetting.Font = new System.Drawing.Font("Roboto Medium", 11F);
            this.checkBoxWaitForChainFateSetting.Location = new System.Drawing.Point(225, 142);
            this.checkBoxWaitForChainFateSetting.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxWaitForChainFateSetting.MouseLocation = new System.Drawing.Point(-1, -1);
            this.checkBoxWaitForChainFateSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.checkBoxWaitForChainFateSetting.Name = "checkBoxWaitForChainFateSetting";
            this.checkBoxWaitForChainFateSetting.Ripple = true;
            this.checkBoxWaitForChainFateSetting.Size = new System.Drawing.Size(30, 30);
            this.checkBoxWaitForChainFateSetting.TabIndex = 20;
            this.checkBoxWaitForChainFateSetting.Text = "Wait For Follow-Up FATEs";
            this.checkBoxWaitForChainFateSetting.UseVisualStyleBackColor = true;
            this.checkBoxWaitForChainFateSetting.CheckedChanged += new System.EventHandler(this.OnWaitForChainFatesChanged);
            // 
            // labelWaitForChainFateSetting
            // 
            this.labelWaitForChainFateSetting.AutoSize = true;
            this.labelWaitForChainFateSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWaitForChainFateSetting.Location = new System.Drawing.Point(15, 148);
            this.labelWaitForChainFateSetting.Name = "labelWaitForChainFateSetting";
            this.labelWaitForChainFateSetting.Size = new System.Drawing.Size(183, 18);
            this.labelWaitForChainFateSetting.TabIndex = 19;
            this.labelWaitForChainFateSetting.Text = "Wait For Follow-Up FATEs:";
            // 
            // labelLowDurationTimeSettingSuffix
            // 
            this.labelLowDurationTimeSettingSuffix.AutoSize = true;
            this.labelLowDurationTimeSettingSuffix.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLowDurationTimeSettingSuffix.Location = new System.Drawing.Point(268, 266);
            this.labelLowDurationTimeSettingSuffix.Name = "labelLowDurationTimeSettingSuffix";
            this.labelLowDurationTimeSettingSuffix.Size = new System.Drawing.Size(68, 20);
            this.labelLowDurationTimeSettingSuffix.TabIndex = 18;
            this.labelLowDurationTimeSettingSuffix.Text = "seconds";
            // 
            // textBoxLowRemainingFateDurationSetting
            // 
            this.textBoxLowRemainingFateDurationSetting.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxLowRemainingFateDurationSetting.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxLowRemainingFateDurationSetting.Depth = 0;
            this.textBoxLowRemainingFateDurationSetting.Hint = "";
            this.textBoxLowRemainingFateDurationSetting.Location = new System.Drawing.Point(219, 268);
            this.textBoxLowRemainingFateDurationSetting.MaxLength = 4;
            this.textBoxLowRemainingFateDurationSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.textBoxLowRemainingFateDurationSetting.Name = "textBoxLowRemainingFateDurationSetting";
            this.textBoxLowRemainingFateDurationSetting.PasswordChar = '\0';
            this.textBoxLowRemainingFateDurationSetting.SelectedText = "";
            this.textBoxLowRemainingFateDurationSetting.SelectionLength = 0;
            this.textBoxLowRemainingFateDurationSetting.SelectionStart = 0;
            this.textBoxLowRemainingFateDurationSetting.Size = new System.Drawing.Size(43, 25);
            this.textBoxLowRemainingFateDurationSetting.TabIndex = 17;
            this.textBoxLowRemainingFateDurationSetting.TabStop = false;
            this.textBoxLowRemainingFateDurationSetting.UseSystemPasswordChar = false;
            this.textBoxLowRemainingFateDurationSetting.Enter += new System.EventHandler(this.OnEnterLowRemainingFateDuration);
            this.textBoxLowRemainingFateDurationSetting.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnEnterKeyDownDropFocus);
            this.textBoxLowRemainingFateDurationSetting.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnLowDurationFateKeyPress);
            this.textBoxLowRemainingFateDurationSetting.TextChanged += new System.EventHandler(this.OnLowRemainingFateDurationChanged);
            // 
            // labelLowDurationTimeSetting
            // 
            this.labelLowDurationTimeSetting.AutoSize = true;
            this.labelLowDurationTimeSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLowDurationTimeSetting.Location = new System.Drawing.Point(15, 268);
            this.labelLowDurationTimeSetting.Name = "labelLowDurationTimeSetting";
            this.labelLowDurationTimeSetting.Size = new System.Drawing.Size(179, 18);
            this.labelLowDurationTimeSetting.TabIndex = 16;
            this.labelLowDurationTimeSetting.Text = "Low Duration Time Value:";
            // 
            // checkBoxIgnoreLowDurationUnstartedFateSetting
            // 
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.AutoSize = true;
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.Depth = 0;
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.Font = new System.Drawing.Font("Roboto Medium", 11F);
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.Location = new System.Drawing.Point(225, 222);
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.MouseLocation = new System.Drawing.Point(-1, -1);
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.Name = "checkBoxIgnoreLowDurationUnstartedFateSetting";
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.Ripple = true;
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.Size = new System.Drawing.Size(30, 30);
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.TabIndex = 15;
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.Text = "Run Problematic FATEs";
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreLowDurationUnstartedFateSetting.CheckedChanged += new System.EventHandler(this.OnIgnoreLowDurationUnstartedFateChanged);
            // 
            // labelIgnoreLowDurationFatesSetting
            // 
            this.labelIgnoreLowDurationFatesSetting.AutoSize = true;
            this.labelIgnoreLowDurationFatesSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIgnoreLowDurationFatesSetting.Location = new System.Drawing.Point(15, 228);
            this.labelIgnoreLowDurationFatesSetting.Name = "labelIgnoreLowDurationFatesSetting";
            this.labelIgnoreLowDurationFatesSetting.Size = new System.Drawing.Size(190, 18);
            this.labelIgnoreLowDurationFatesSetting.TabIndex = 14;
            this.labelIgnoreLowDurationFatesSetting.Text = "Ignore Low Duration FATEs:";
            // 
            // checkBoxRunProblematicFatesSetting
            // 
            this.checkBoxRunProblematicFatesSetting.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxRunProblematicFatesSetting.AutoSize = true;
            this.checkBoxRunProblematicFatesSetting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxRunProblematicFatesSetting.Depth = 0;
            this.checkBoxRunProblematicFatesSetting.Font = new System.Drawing.Font("Roboto Medium", 11F);
            this.checkBoxRunProblematicFatesSetting.Location = new System.Drawing.Point(225, 302);
            this.checkBoxRunProblematicFatesSetting.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxRunProblematicFatesSetting.MouseLocation = new System.Drawing.Point(-1, -1);
            this.checkBoxRunProblematicFatesSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.checkBoxRunProblematicFatesSetting.Name = "checkBoxRunProblematicFatesSetting";
            this.checkBoxRunProblematicFatesSetting.Ripple = true;
            this.checkBoxRunProblematicFatesSetting.Size = new System.Drawing.Size(30, 30);
            this.checkBoxRunProblematicFatesSetting.TabIndex = 13;
            this.checkBoxRunProblematicFatesSetting.Text = "Run Problematic FATEs";
            this.checkBoxRunProblematicFatesSetting.UseVisualStyleBackColor = true;
            this.checkBoxRunProblematicFatesSetting.CheckedChanged += new System.EventHandler(this.OnRunProblematicFatesChanged);
            // 
            // labelRunProblematicFatesWarning
            // 
            this.labelRunProblematicFatesWarning.AutoSize = true;
            this.labelRunProblematicFatesWarning.Font = new System.Drawing.Font("Roboto", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRunProblematicFatesWarning.ForeColor = System.Drawing.Color.Maroon;
            this.labelRunProblematicFatesWarning.Location = new System.Drawing.Point(15, 326);
            this.labelRunProblematicFatesWarning.Name = "labelRunProblematicFatesWarning";
            this.labelRunProblematicFatesWarning.Size = new System.Drawing.Size(160, 14);
            this.labelRunProblematicFatesWarning.TabIndex = 12;
            this.labelRunProblematicFatesWarning.Text = "Warning: Not Recommended";
            // 
            // labelRunProblematicFatesSetting
            // 
            this.labelRunProblematicFatesSetting.AutoSize = true;
            this.labelRunProblematicFatesSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRunProblematicFatesSetting.Location = new System.Drawing.Point(15, 308);
            this.labelRunProblematicFatesSetting.Name = "labelRunProblematicFatesSetting";
            this.labelRunProblematicFatesSetting.Size = new System.Drawing.Size(168, 18);
            this.labelRunProblematicFatesSetting.TabIndex = 11;
            this.labelRunProblematicFatesSetting.Text = "Run Problematic FATEs:";
            // 
            // numericUpDownFateMaximumLevelAboveSetting
            // 
            this.numericUpDownFateMaximumLevelAboveSetting.AutoSize = true;
            this.numericUpDownFateMaximumLevelAboveSetting.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownFateMaximumLevelAboveSetting.Location = new System.Drawing.Point(223, 107);
            this.numericUpDownFateMaximumLevelAboveSetting.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDownFateMaximumLevelAboveSetting.Name = "numericUpDownFateMaximumLevelAboveSetting";
            this.numericUpDownFateMaximumLevelAboveSetting.Size = new System.Drawing.Size(46, 23);
            this.numericUpDownFateMaximumLevelAboveSetting.TabIndex = 10;
            this.numericUpDownFateMaximumLevelAboveSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownFateMaximumLevelAboveSetting.ValueChanged += new System.EventHandler(this.OnFateMaximumLevelAboveChanged);
            this.numericUpDownFateMaximumLevelAboveSetting.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnEnterKeyDownDropFocus);
            // 
            // labelFateMaximumLevelAboveSetting
            // 
            this.labelFateMaximumLevelAboveSetting.AutoSize = true;
            this.labelFateMaximumLevelAboveSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFateMaximumLevelAboveSetting.Location = new System.Drawing.Point(15, 108);
            this.labelFateMaximumLevelAboveSetting.Name = "labelFateMaximumLevelAboveSetting";
            this.labelFateMaximumLevelAboveSetting.Size = new System.Drawing.Size(202, 18);
            this.labelFateMaximumLevelAboveSetting.TabIndex = 9;
            this.labelFateMaximumLevelAboveSetting.Text = "FATE Maximum Level Above:";
            // 
            // numericUpDownFateMinimumLevelBelowSetting
            // 
            this.numericUpDownFateMinimumLevelBelowSetting.AutoSize = true;
            this.numericUpDownFateMinimumLevelBelowSetting.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownFateMinimumLevelBelowSetting.Location = new System.Drawing.Point(223, 67);
            this.numericUpDownFateMinimumLevelBelowSetting.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDownFateMinimumLevelBelowSetting.Name = "numericUpDownFateMinimumLevelBelowSetting";
            this.numericUpDownFateMinimumLevelBelowSetting.Size = new System.Drawing.Size(46, 23);
            this.numericUpDownFateMinimumLevelBelowSetting.TabIndex = 8;
            this.numericUpDownFateMinimumLevelBelowSetting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownFateMinimumLevelBelowSetting.ValueChanged += new System.EventHandler(this.OnFateMinimumLevelBelowChanged);
            this.numericUpDownFateMinimumLevelBelowSetting.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnEnterKeyDownDropFocus);
            // 
            // labelFateMinimumLevelBelowSetting
            // 
            this.labelFateMinimumLevelBelowSetting.AutoSize = true;
            this.labelFateMinimumLevelBelowSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFateMinimumLevelBelowSetting.Location = new System.Drawing.Point(15, 68);
            this.labelFateMinimumLevelBelowSetting.Name = "labelFateMinimumLevelBelowSetting";
            this.labelFateMinimumLevelBelowSetting.Size = new System.Drawing.Size(197, 18);
            this.labelFateMinimumLevelBelowSetting.TabIndex = 6;
            this.labelFateMinimumLevelBelowSetting.Text = "FATE Minimum Level Below:";
            // 
            // labelGeneralFateSettingsTitle
            // 
            this.labelGeneralFateSettingsTitle.AutoSize = true;
            this.labelGeneralFateSettingsTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGeneralFateSettingsTitle.Location = new System.Drawing.Point(10, 10);
            this.labelGeneralFateSettingsTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelGeneralFateSettingsTitle.Name = "labelGeneralFateSettingsTitle";
            this.labelGeneralFateSettingsTitle.Size = new System.Drawing.Size(268, 38);
            this.labelGeneralFateSettingsTitle.TabIndex = 1;
            this.labelGeneralFateSettingsTitle.Text = "General FATE Settings";
            // 
            // tabPageKillFates
            // 
            this.tabPageKillFates.BackColor = System.Drawing.Color.White;
            this.tabPageKillFates.Controls.Add(this.checkBoxKillFatesEnabledSetting);
            this.tabPageKillFates.Controls.Add(this.labelKillFatesEnabledSetting);
            this.tabPageKillFates.Controls.Add(this.labelKillFatesTitle);
            this.tabPageKillFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageKillFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageKillFates.Name = "tabPageKillFates";
            this.tabPageKillFates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageKillFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageKillFates.TabIndex = 1;
            this.tabPageKillFates.Text = "Kill FATEs";
            this.tabPageKillFates.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // checkBoxKillFatesEnabledSetting
            // 
            this.checkBoxKillFatesEnabledSetting.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxKillFatesEnabledSetting.AutoSize = true;
            this.checkBoxKillFatesEnabledSetting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxKillFatesEnabledSetting.Depth = 0;
            this.checkBoxKillFatesEnabledSetting.Font = new System.Drawing.Font("Roboto Medium", 11F);
            this.checkBoxKillFatesEnabledSetting.Location = new System.Drawing.Point(153, 62);
            this.checkBoxKillFatesEnabledSetting.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxKillFatesEnabledSetting.MouseLocation = new System.Drawing.Point(-1, -1);
            this.checkBoxKillFatesEnabledSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.checkBoxKillFatesEnabledSetting.Name = "checkBoxKillFatesEnabledSetting";
            this.checkBoxKillFatesEnabledSetting.Ripple = true;
            this.checkBoxKillFatesEnabledSetting.Size = new System.Drawing.Size(30, 30);
            this.checkBoxKillFatesEnabledSetting.TabIndex = 21;
            this.checkBoxKillFatesEnabledSetting.Text = "Kill FATEs Enabled";
            this.checkBoxKillFatesEnabledSetting.UseVisualStyleBackColor = true;
            this.checkBoxKillFatesEnabledSetting.CheckedChanged += new System.EventHandler(this.OnKillFatesEnabledChanged);
            // 
            // labelKillFatesEnabledSetting
            // 
            this.labelKillFatesEnabledSetting.AutoSize = true;
            this.labelKillFatesEnabledSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKillFatesEnabledSetting.Location = new System.Drawing.Point(15, 68);
            this.labelKillFatesEnabledSetting.Name = "labelKillFatesEnabledSetting";
            this.labelKillFatesEnabledSetting.Size = new System.Drawing.Size(135, 18);
            this.labelKillFatesEnabledSetting.TabIndex = 7;
            this.labelKillFatesEnabledSetting.Text = "Kill FATEs Enabled:";
            // 
            // labelKillFatesTitle
            // 
            this.labelKillFatesTitle.AutoSize = true;
            this.labelKillFatesTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKillFatesTitle.Location = new System.Drawing.Point(10, 10);
            this.labelKillFatesTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelKillFatesTitle.Name = "labelKillFatesTitle";
            this.labelKillFatesTitle.Size = new System.Drawing.Size(219, 38);
            this.labelKillFatesTitle.TabIndex = 4;
            this.labelKillFatesTitle.Text = "Kill FATE Settings";
            // 
            // tabPageCollectFates
            // 
            this.tabPageCollectFates.BackColor = System.Drawing.Color.White;
            this.tabPageCollectFates.Controls.Add(this.checkBoxCollectFatesEnabledSetting);
            this.tabPageCollectFates.Controls.Add(this.labelCollectFatesEnabledSetting);
            this.tabPageCollectFates.Controls.Add(this.labelCollectFatesTitle);
            this.tabPageCollectFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageCollectFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageCollectFates.Name = "tabPageCollectFates";
            this.tabPageCollectFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageCollectFates.TabIndex = 2;
            this.tabPageCollectFates.Text = "Collect FATEs";
            this.tabPageCollectFates.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // checkBoxCollectFatesEnabledSetting
            // 
            this.checkBoxCollectFatesEnabledSetting.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCollectFatesEnabledSetting.AutoSize = true;
            this.checkBoxCollectFatesEnabledSetting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxCollectFatesEnabledSetting.Depth = 0;
            this.checkBoxCollectFatesEnabledSetting.Font = new System.Drawing.Font("Roboto Medium", 11F);
            this.checkBoxCollectFatesEnabledSetting.Location = new System.Drawing.Point(178, 62);
            this.checkBoxCollectFatesEnabledSetting.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxCollectFatesEnabledSetting.MouseLocation = new System.Drawing.Point(-1, -1);
            this.checkBoxCollectFatesEnabledSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.checkBoxCollectFatesEnabledSetting.Name = "checkBoxCollectFatesEnabledSetting";
            this.checkBoxCollectFatesEnabledSetting.Ripple = true;
            this.checkBoxCollectFatesEnabledSetting.Size = new System.Drawing.Size(30, 30);
            this.checkBoxCollectFatesEnabledSetting.TabIndex = 23;
            this.checkBoxCollectFatesEnabledSetting.Text = "Collect FATEs Enabled";
            this.checkBoxCollectFatesEnabledSetting.UseVisualStyleBackColor = true;
            this.checkBoxCollectFatesEnabledSetting.CheckedChanged += new System.EventHandler(this.OnCollectFatesEnabledChanged);
            // 
            // labelCollectFatesEnabledSetting
            // 
            this.labelCollectFatesEnabledSetting.AutoSize = true;
            this.labelCollectFatesEnabledSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCollectFatesEnabledSetting.Location = new System.Drawing.Point(15, 68);
            this.labelCollectFatesEnabledSetting.Name = "labelCollectFatesEnabledSetting";
            this.labelCollectFatesEnabledSetting.Size = new System.Drawing.Size(160, 18);
            this.labelCollectFatesEnabledSetting.TabIndex = 22;
            this.labelCollectFatesEnabledSetting.Text = "Collect FATEs Enabled:";
            // 
            // labelCollectFatesTitle
            // 
            this.labelCollectFatesTitle.AutoSize = true;
            this.labelCollectFatesTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCollectFatesTitle.Location = new System.Drawing.Point(10, 10);
            this.labelCollectFatesTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelCollectFatesTitle.Name = "labelCollectFatesTitle";
            this.labelCollectFatesTitle.Size = new System.Drawing.Size(260, 38);
            this.labelCollectFatesTitle.TabIndex = 5;
            this.labelCollectFatesTitle.Text = "Collect FATE Settings";
            // 
            // tabPageEscortFates
            // 
            this.tabPageEscortFates.BackColor = System.Drawing.Color.White;
            this.tabPageEscortFates.Controls.Add(this.checkBoxEscortFatesEnabledSetting);
            this.tabPageEscortFates.Controls.Add(this.labelEscortFatesEnabledSetting);
            this.tabPageEscortFates.Controls.Add(this.labelEscortFatesTitle);
            this.tabPageEscortFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageEscortFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageEscortFates.Name = "tabPageEscortFates";
            this.tabPageEscortFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageEscortFates.TabIndex = 3;
            this.tabPageEscortFates.Text = "Escort FATEs";
            this.tabPageEscortFates.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // checkBoxEscortFatesEnabledSetting
            // 
            this.checkBoxEscortFatesEnabledSetting.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEscortFatesEnabledSetting.AutoSize = true;
            this.checkBoxEscortFatesEnabledSetting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxEscortFatesEnabledSetting.Depth = 0;
            this.checkBoxEscortFatesEnabledSetting.Font = new System.Drawing.Font("Roboto Medium", 11F);
            this.checkBoxEscortFatesEnabledSetting.Location = new System.Drawing.Point(173, 62);
            this.checkBoxEscortFatesEnabledSetting.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxEscortFatesEnabledSetting.MouseLocation = new System.Drawing.Point(-1, -1);
            this.checkBoxEscortFatesEnabledSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.checkBoxEscortFatesEnabledSetting.Name = "checkBoxEscortFatesEnabledSetting";
            this.checkBoxEscortFatesEnabledSetting.Ripple = true;
            this.checkBoxEscortFatesEnabledSetting.Size = new System.Drawing.Size(30, 30);
            this.checkBoxEscortFatesEnabledSetting.TabIndex = 23;
            this.checkBoxEscortFatesEnabledSetting.Text = "Escort FATEs Enabled";
            this.checkBoxEscortFatesEnabledSetting.UseVisualStyleBackColor = true;
            this.checkBoxEscortFatesEnabledSetting.CheckedChanged += new System.EventHandler(this.OnEscortFatesEnabledChanged);
            // 
            // labelEscortFatesEnabledSetting
            // 
            this.labelEscortFatesEnabledSetting.AutoSize = true;
            this.labelEscortFatesEnabledSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEscortFatesEnabledSetting.Location = new System.Drawing.Point(15, 68);
            this.labelEscortFatesEnabledSetting.Name = "labelEscortFatesEnabledSetting";
            this.labelEscortFatesEnabledSetting.Size = new System.Drawing.Size(155, 18);
            this.labelEscortFatesEnabledSetting.TabIndex = 22;
            this.labelEscortFatesEnabledSetting.Text = "Escort FATEs Enabled:";
            // 
            // labelEscortFatesTitle
            // 
            this.labelEscortFatesTitle.AutoSize = true;
            this.labelEscortFatesTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEscortFatesTitle.Location = new System.Drawing.Point(10, 10);
            this.labelEscortFatesTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelEscortFatesTitle.Name = "labelEscortFatesTitle";
            this.labelEscortFatesTitle.Size = new System.Drawing.Size(254, 38);
            this.labelEscortFatesTitle.TabIndex = 6;
            this.labelEscortFatesTitle.Text = "Escort FATE Settings";
            // 
            // tabPageDefenceFates
            // 
            this.tabPageDefenceFates.BackColor = System.Drawing.Color.White;
            this.tabPageDefenceFates.Controls.Add(this.checkBoxDefenceFatesEnabledSetting);
            this.tabPageDefenceFates.Controls.Add(this.labelDefenceFatesEnabledSetting);
            this.tabPageDefenceFates.Controls.Add(this.labelDefenceFatesTitle);
            this.tabPageDefenceFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageDefenceFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageDefenceFates.Name = "tabPageDefenceFates";
            this.tabPageDefenceFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageDefenceFates.TabIndex = 4;
            this.tabPageDefenceFates.Text = "Defence FATEs";
            this.tabPageDefenceFates.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // checkBoxDefenceFatesEnabledSetting
            // 
            this.checkBoxDefenceFatesEnabledSetting.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxDefenceFatesEnabledSetting.AutoSize = true;
            this.checkBoxDefenceFatesEnabledSetting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxDefenceFatesEnabledSetting.Depth = 0;
            this.checkBoxDefenceFatesEnabledSetting.Font = new System.Drawing.Font("Roboto Medium", 11F);
            this.checkBoxDefenceFatesEnabledSetting.Location = new System.Drawing.Point(186, 62);
            this.checkBoxDefenceFatesEnabledSetting.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxDefenceFatesEnabledSetting.MouseLocation = new System.Drawing.Point(-1, -1);
            this.checkBoxDefenceFatesEnabledSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.checkBoxDefenceFatesEnabledSetting.Name = "checkBoxDefenceFatesEnabledSetting";
            this.checkBoxDefenceFatesEnabledSetting.Ripple = true;
            this.checkBoxDefenceFatesEnabledSetting.Size = new System.Drawing.Size(30, 30);
            this.checkBoxDefenceFatesEnabledSetting.TabIndex = 23;
            this.checkBoxDefenceFatesEnabledSetting.Text = "Defence FATEs Enabled";
            this.checkBoxDefenceFatesEnabledSetting.UseVisualStyleBackColor = true;
            this.checkBoxDefenceFatesEnabledSetting.CheckedChanged += new System.EventHandler(this.OnDefenceFatesEnabledChanged);
            // 
            // labelDefenceFatesEnabledSetting
            // 
            this.labelDefenceFatesEnabledSetting.AutoSize = true;
            this.labelDefenceFatesEnabledSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefenceFatesEnabledSetting.Location = new System.Drawing.Point(15, 68);
            this.labelDefenceFatesEnabledSetting.Name = "labelDefenceFatesEnabledSetting";
            this.labelDefenceFatesEnabledSetting.Size = new System.Drawing.Size(168, 18);
            this.labelDefenceFatesEnabledSetting.TabIndex = 22;
            this.labelDefenceFatesEnabledSetting.Text = "Defence FATEs Enabled:";
            // 
            // labelDefenceFatesTitle
            // 
            this.labelDefenceFatesTitle.AutoSize = true;
            this.labelDefenceFatesTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefenceFatesTitle.Location = new System.Drawing.Point(10, 10);
            this.labelDefenceFatesTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelDefenceFatesTitle.Name = "labelDefenceFatesTitle";
            this.labelDefenceFatesTitle.Size = new System.Drawing.Size(273, 38);
            this.labelDefenceFatesTitle.TabIndex = 6;
            this.labelDefenceFatesTitle.Text = "Defence FATE Settings";
            // 
            // tabPageBossFates
            // 
            this.tabPageBossFates.BackColor = System.Drawing.Color.White;
            this.tabPageBossFates.Controls.Add(this.checkBoxBossFatesEnabledSetting);
            this.tabPageBossFates.Controls.Add(this.labelBossFatesEnabledSetting);
            this.tabPageBossFates.Controls.Add(this.labelBossFatesTitle);
            this.tabPageBossFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageBossFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageBossFates.Name = "tabPageBossFates";
            this.tabPageBossFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageBossFates.TabIndex = 5;
            this.tabPageBossFates.Text = "Boss FATEs";
            this.tabPageBossFates.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // checkBoxBossFatesEnabledSetting
            // 
            this.checkBoxBossFatesEnabledSetting.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxBossFatesEnabledSetting.AutoSize = true;
            this.checkBoxBossFatesEnabledSetting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxBossFatesEnabledSetting.Depth = 0;
            this.checkBoxBossFatesEnabledSetting.Font = new System.Drawing.Font("Roboto Medium", 11F);
            this.checkBoxBossFatesEnabledSetting.Location = new System.Drawing.Point(165, 62);
            this.checkBoxBossFatesEnabledSetting.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxBossFatesEnabledSetting.MouseLocation = new System.Drawing.Point(-1, -1);
            this.checkBoxBossFatesEnabledSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.checkBoxBossFatesEnabledSetting.Name = "checkBoxBossFatesEnabledSetting";
            this.checkBoxBossFatesEnabledSetting.Ripple = true;
            this.checkBoxBossFatesEnabledSetting.Size = new System.Drawing.Size(30, 30);
            this.checkBoxBossFatesEnabledSetting.TabIndex = 23;
            this.checkBoxBossFatesEnabledSetting.Text = "Boss FATEs Enabled";
            this.checkBoxBossFatesEnabledSetting.UseVisualStyleBackColor = true;
            this.checkBoxBossFatesEnabledSetting.CheckedChanged += new System.EventHandler(this.OnBossFatesEnabledChanged);
            // 
            // labelBossFatesEnabledSetting
            // 
            this.labelBossFatesEnabledSetting.AutoSize = true;
            this.labelBossFatesEnabledSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBossFatesEnabledSetting.Location = new System.Drawing.Point(15, 68);
            this.labelBossFatesEnabledSetting.Name = "labelBossFatesEnabledSetting";
            this.labelBossFatesEnabledSetting.Size = new System.Drawing.Size(147, 18);
            this.labelBossFatesEnabledSetting.TabIndex = 22;
            this.labelBossFatesEnabledSetting.Text = "Boss FATEs Enabled:";
            // 
            // labelBossFatesTitle
            // 
            this.labelBossFatesTitle.AutoSize = true;
            this.labelBossFatesTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBossFatesTitle.Location = new System.Drawing.Point(10, 10);
            this.labelBossFatesTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelBossFatesTitle.Name = "labelBossFatesTitle";
            this.labelBossFatesTitle.Size = new System.Drawing.Size(238, 38);
            this.labelBossFatesTitle.TabIndex = 6;
            this.labelBossFatesTitle.Text = "Boss FATE Settings";
            // 
            // tabPageMegaBossFates
            // 
            this.tabPageMegaBossFates.BackColor = System.Drawing.Color.White;
            this.tabPageMegaBossFates.Controls.Add(this.checkBoxMegaBossFatesEnabledSetting);
            this.tabPageMegaBossFates.Controls.Add(this.labelMegaBossFatesEnabledSetting);
            this.tabPageMegaBossFates.Controls.Add(this.labelMegaBossFatesTitle);
            this.tabPageMegaBossFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageMegaBossFates.Name = "tabPageMegaBossFates";
            this.tabPageMegaBossFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageMegaBossFates.TabIndex = 6;
            this.tabPageMegaBossFates.Text = "Mega-Boss FATEs";
            this.tabPageMegaBossFates.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // checkBoxMegaBossFatesEnabledSetting
            // 
            this.checkBoxMegaBossFatesEnabledSetting.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxMegaBossFatesEnabledSetting.AutoSize = true;
            this.checkBoxMegaBossFatesEnabledSetting.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxMegaBossFatesEnabledSetting.Depth = 0;
            this.checkBoxMegaBossFatesEnabledSetting.Font = new System.Drawing.Font("Roboto Medium", 11F);
            this.checkBoxMegaBossFatesEnabledSetting.Location = new System.Drawing.Point(209, 62);
            this.checkBoxMegaBossFatesEnabledSetting.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxMegaBossFatesEnabledSetting.MouseLocation = new System.Drawing.Point(-1, -1);
            this.checkBoxMegaBossFatesEnabledSetting.MouseState = MaterialSkin.MouseState.Hover;
            this.checkBoxMegaBossFatesEnabledSetting.Name = "checkBoxMegaBossFatesEnabledSetting";
            this.checkBoxMegaBossFatesEnabledSetting.Ripple = true;
            this.checkBoxMegaBossFatesEnabledSetting.Size = new System.Drawing.Size(30, 30);
            this.checkBoxMegaBossFatesEnabledSetting.TabIndex = 23;
            this.checkBoxMegaBossFatesEnabledSetting.Text = "Mega-Boss FATEs Enabled";
            this.checkBoxMegaBossFatesEnabledSetting.UseVisualStyleBackColor = true;
            this.checkBoxMegaBossFatesEnabledSetting.CheckedChanged += new System.EventHandler(this.OnMegaBossFatesEnabledChanged);
            // 
            // labelMegaBossFatesEnabledSetting
            // 
            this.labelMegaBossFatesEnabledSetting.AutoSize = true;
            this.labelMegaBossFatesEnabledSetting.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMegaBossFatesEnabledSetting.Location = new System.Drawing.Point(15, 68);
            this.labelMegaBossFatesEnabledSetting.Name = "labelMegaBossFatesEnabledSetting";
            this.labelMegaBossFatesEnabledSetting.Size = new System.Drawing.Size(191, 18);
            this.labelMegaBossFatesEnabledSetting.TabIndex = 22;
            this.labelMegaBossFatesEnabledSetting.Text = "Mega-Boss FATEs Enabled:";
            // 
            // labelMegaBossFatesTitle
            // 
            this.labelMegaBossFatesTitle.AutoSize = true;
            this.labelMegaBossFatesTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMegaBossFatesTitle.Location = new System.Drawing.Point(10, 10);
            this.labelMegaBossFatesTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelMegaBossFatesTitle.Name = "labelMegaBossFatesTitle";
            this.labelMegaBossFatesTitle.Size = new System.Drawing.Size(310, 38);
            this.labelMegaBossFatesTitle.TabIndex = 7;
            this.labelMegaBossFatesTitle.Text = "Mega-Boss FATE Settings";
            // 
            // tabSelectorFate
            // 
            this.tabSelectorFate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabSelectorFate.BaseTabControl = this.tabControllerFate;
            this.tabSelectorFate.Depth = 0;
            this.tabSelectorFate.Location = new System.Drawing.Point(-4, 0);
            this.tabSelectorFate.Margin = new System.Windows.Forms.Padding(0);
            this.tabSelectorFate.MinimumSize = new System.Drawing.Size(173, 449);
            this.tabSelectorFate.MouseState = MaterialSkin.MouseState.Hover;
            this.tabSelectorFate.Name = "tabSelectorFate";
            this.tabSelectorFate.Size = new System.Drawing.Size(173, 449);
            this.tabSelectorFate.TabIndex = 8;
            this.tabSelectorFate.TabStop = false;
            this.tabSelectorFate.Text = "materialTabSelectorVertical1";
            this.tabSelectorFate.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // tabNavigationSettings
            // 
            this.tabNavigationSettings.BackColor = System.Drawing.Color.White;
            this.tabNavigationSettings.Controls.Add(this.tabSelectorCustom);
            this.tabNavigationSettings.Controls.Add(this.tabControllerCustom);
            this.tabNavigationSettings.Location = new System.Drawing.Point(4, 22);
            this.tabNavigationSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tabNavigationSettings.Name = "tabNavigationSettings";
            this.tabNavigationSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabNavigationSettings.Size = new System.Drawing.Size(842, 495);
            this.tabNavigationSettings.TabIndex = 2;
            this.tabNavigationSettings.Text = "Navigation Settings";
            this.tabNavigationSettings.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // tabSelectorCustom
            // 
            this.tabSelectorCustom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabSelectorCustom.BaseTabControl = this.tabControllerCustom;
            this.tabSelectorCustom.Depth = 0;
            this.tabSelectorCustom.Location = new System.Drawing.Point(-4, 0);
            this.tabSelectorCustom.Margin = new System.Windows.Forms.Padding(0);
            this.tabSelectorCustom.MinimumSize = new System.Drawing.Size(173, 449);
            this.tabSelectorCustom.MouseState = MaterialSkin.MouseState.Hover;
            this.tabSelectorCustom.Name = "tabSelectorCustom";
            this.tabSelectorCustom.Size = new System.Drawing.Size(173, 449);
            this.tabSelectorCustom.TabIndex = 7;
            this.tabSelectorCustom.TabStop = false;
            this.tabSelectorCustom.Text = "materialTabSelectorVertical1";
            this.tabSelectorCustom.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // tabControllerCustom
            // 
            this.tabControllerCustom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControllerCustom.Controls.Add(this.tabPageMovement);
            this.tabControllerCustom.Controls.Add(this.tabPageFlight);
            this.tabControllerCustom.Controls.Add(this.tabPageTeleport);
            this.tabControllerCustom.Depth = 0;
            this.tabControllerCustom.Location = new System.Drawing.Point(172, 0);
            this.tabControllerCustom.Margin = new System.Windows.Forms.Padding(0);
            this.tabControllerCustom.MinimumSize = new System.Drawing.Size(674, 449);
            this.tabControllerCustom.MouseState = MaterialSkin.MouseState.Hover;
            this.tabControllerCustom.Name = "tabControllerCustom";
            this.tabControllerCustom.SelectedIndex = 0;
            this.tabControllerCustom.Size = new System.Drawing.Size(674, 449);
            this.tabControllerCustom.TabIndex = 2;
            this.tabControllerCustom.TabStop = false;
            // 
            // tabPageMovement
            // 
            this.tabPageMovement.BackColor = System.Drawing.Color.White;
            this.tabPageMovement.Controls.Add(this.labelMovementTitle);
            this.tabPageMovement.Location = new System.Drawing.Point(4, 29);
            this.tabPageMovement.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMovement.Name = "tabPageMovement";
            this.tabPageMovement.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMovement.Size = new System.Drawing.Size(666, 416);
            this.tabPageMovement.TabIndex = 0;
            this.tabPageMovement.Text = "Movement";
            this.tabPageMovement.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelMovementTitle
            // 
            this.labelMovementTitle.AutoSize = true;
            this.labelMovementTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMovementTitle.Location = new System.Drawing.Point(10, 10);
            this.labelMovementTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelMovementTitle.Name = "labelMovementTitle";
            this.labelMovementTitle.Size = new System.Drawing.Size(236, 38);
            this.labelMovementTitle.TabIndex = 1;
            this.labelMovementTitle.Text = "Movement Settings";
            // 
            // tabPageFlight
            // 
            this.tabPageFlight.BackColor = System.Drawing.Color.White;
            this.tabPageFlight.Controls.Add(this.labelFlightTitle);
            this.tabPageFlight.Location = new System.Drawing.Point(4, 22);
            this.tabPageFlight.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageFlight.Name = "tabPageFlight";
            this.tabPageFlight.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFlight.Size = new System.Drawing.Size(666, 423);
            this.tabPageFlight.TabIndex = 1;
            this.tabPageFlight.Text = "Flight";
            this.tabPageFlight.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelFlightTitle
            // 
            this.labelFlightTitle.AutoSize = true;
            this.labelFlightTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFlightTitle.Location = new System.Drawing.Point(10, 10);
            this.labelFlightTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelFlightTitle.Name = "labelFlightTitle";
            this.labelFlightTitle.Size = new System.Drawing.Size(183, 38);
            this.labelFlightTitle.TabIndex = 4;
            this.labelFlightTitle.Text = "Flight Settings";
            // 
            // tabPageTeleport
            // 
            this.tabPageTeleport.BackColor = System.Drawing.Color.White;
            this.tabPageTeleport.Controls.Add(this.labelTeleportTitle);
            this.tabPageTeleport.Location = new System.Drawing.Point(4, 22);
            this.tabPageTeleport.Name = "tabPageTeleport";
            this.tabPageTeleport.Size = new System.Drawing.Size(666, 423);
            this.tabPageTeleport.TabIndex = 2;
            this.tabPageTeleport.Text = "Teleport";
            this.tabPageTeleport.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelTeleportTitle
            // 
            this.labelTeleportTitle.AutoSize = true;
            this.labelTeleportTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTeleportTitle.Location = new System.Drawing.Point(10, 10);
            this.labelTeleportTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelTeleportTitle.Name = "labelTeleportTitle";
            this.labelTeleportTitle.Size = new System.Drawing.Size(211, 38);
            this.labelTeleportTitle.TabIndex = 5;
            this.labelTeleportTitle.Text = "Teleport Settings";
            // 
            // tabBlacklist
            // 
            this.tabBlacklist.BackColor = System.Drawing.Color.White;
            this.tabBlacklist.Controls.Add(this.tabSelectorBlacklist);
            this.tabBlacklist.Controls.Add(this.tabControllerBlacklist);
            this.tabBlacklist.Location = new System.Drawing.Point(4, 22);
            this.tabBlacklist.Margin = new System.Windows.Forms.Padding(0);
            this.tabBlacklist.Name = "tabBlacklist";
            this.tabBlacklist.Padding = new System.Windows.Forms.Padding(3);
            this.tabBlacklist.Size = new System.Drawing.Size(842, 495);
            this.tabBlacklist.TabIndex = 3;
            this.tabBlacklist.Text = "Blacklist";
            this.tabBlacklist.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // tabSelectorBlacklist
            // 
            this.tabSelectorBlacklist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabSelectorBlacklist.BaseTabControl = this.tabControllerBlacklist;
            this.tabSelectorBlacklist.Depth = 0;
            this.tabSelectorBlacklist.Location = new System.Drawing.Point(-4, 0);
            this.tabSelectorBlacklist.Margin = new System.Windows.Forms.Padding(0);
            this.tabSelectorBlacklist.MinimumSize = new System.Drawing.Size(173, 449);
            this.tabSelectorBlacklist.MouseState = MaterialSkin.MouseState.Hover;
            this.tabSelectorBlacklist.Name = "tabSelectorBlacklist";
            this.tabSelectorBlacklist.Size = new System.Drawing.Size(173, 449);
            this.tabSelectorBlacklist.TabIndex = 6;
            this.tabSelectorBlacklist.TabStop = false;
            this.tabSelectorBlacklist.Text = "materialTabSelectorVertical1";
            this.tabSelectorBlacklist.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // tabControllerBlacklist
            // 
            this.tabControllerBlacklist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControllerBlacklist.Controls.Add(this.tabPageFateBlacklist);
            this.tabControllerBlacklist.Controls.Add(this.tabPageMobBlacklist);
            this.tabControllerBlacklist.Depth = 0;
            this.tabControllerBlacklist.Location = new System.Drawing.Point(172, 0);
            this.tabControllerBlacklist.Margin = new System.Windows.Forms.Padding(0);
            this.tabControllerBlacklist.MinimumSize = new System.Drawing.Size(674, 449);
            this.tabControllerBlacklist.MouseState = MaterialSkin.MouseState.Hover;
            this.tabControllerBlacklist.Name = "tabControllerBlacklist";
            this.tabControllerBlacklist.SelectedIndex = 0;
            this.tabControllerBlacklist.Size = new System.Drawing.Size(674, 449);
            this.tabControllerBlacklist.TabIndex = 1;
            this.tabControllerBlacklist.TabStop = false;
            // 
            // tabPageFateBlacklist
            // 
            this.tabPageFateBlacklist.BackColor = System.Drawing.Color.White;
            this.tabPageFateBlacklist.Controls.Add(this.labelFateBlacklistTitle);
            this.tabPageFateBlacklist.Location = new System.Drawing.Point(4, 29);
            this.tabPageFateBlacklist.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageFateBlacklist.Name = "tabPageFateBlacklist";
            this.tabPageFateBlacklist.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFateBlacklist.Size = new System.Drawing.Size(666, 416);
            this.tabPageFateBlacklist.TabIndex = 0;
            this.tabPageFateBlacklist.Text = "FATE Blacklist";
            this.tabPageFateBlacklist.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelFateBlacklistTitle
            // 
            this.labelFateBlacklistTitle.AutoSize = true;
            this.labelFateBlacklistTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFateBlacklistTitle.Location = new System.Drawing.Point(10, 10);
            this.labelFateBlacklistTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelFateBlacklistTitle.Name = "labelFateBlacklistTitle";
            this.labelFateBlacklistTitle.Size = new System.Drawing.Size(180, 38);
            this.labelFateBlacklistTitle.TabIndex = 1;
            this.labelFateBlacklistTitle.Text = "FATE Blacklist";
            // 
            // tabPageMobBlacklist
            // 
            this.tabPageMobBlacklist.BackColor = System.Drawing.Color.White;
            this.tabPageMobBlacklist.Controls.Add(this.labelMobBlacklistTitle);
            this.tabPageMobBlacklist.Location = new System.Drawing.Point(4, 22);
            this.tabPageMobBlacklist.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMobBlacklist.Name = "tabPageMobBlacklist";
            this.tabPageMobBlacklist.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMobBlacklist.Size = new System.Drawing.Size(666, 423);
            this.tabPageMobBlacklist.TabIndex = 1;
            this.tabPageMobBlacklist.Text = "Mob Blacklist";
            this.tabPageMobBlacklist.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelMobBlacklistTitle
            // 
            this.labelMobBlacklistTitle.AutoSize = true;
            this.labelMobBlacklistTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMobBlacklistTitle.Location = new System.Drawing.Point(10, 10);
            this.labelMobBlacklistTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelMobBlacklistTitle.Name = "labelMobBlacklistTitle";
            this.labelMobBlacklistTitle.Size = new System.Drawing.Size(171, 38);
            this.labelMobBlacklistTitle.TabIndex = 4;
            this.labelMobBlacklistTitle.Text = "Mob Blacklist";
            // 
            // tabAbout
            // 
            this.tabAbout.BackColor = System.Drawing.Color.White;
            this.tabAbout.Controls.Add(this.tabControllerAbout);
            this.tabAbout.Controls.Add(this.tabSelectorAbout);
            this.tabAbout.Location = new System.Drawing.Point(4, 22);
            this.tabAbout.Margin = new System.Windows.Forms.Padding(0);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabAbout.Size = new System.Drawing.Size(842, 495);
            this.tabAbout.TabIndex = 4;
            this.tabAbout.Text = "About";
            this.tabAbout.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // tabControllerAbout
            // 
            this.tabControllerAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControllerAbout.Controls.Add(this.tabPageLicense);
            this.tabControllerAbout.Controls.Add(this.tabPageDonate);
            this.tabControllerAbout.Controls.Add(this.tabPageDevelopment);
            this.tabControllerAbout.Depth = 0;
            this.tabControllerAbout.Location = new System.Drawing.Point(172, 0);
            this.tabControllerAbout.Margin = new System.Windows.Forms.Padding(0);
            this.tabControllerAbout.MinimumSize = new System.Drawing.Size(674, 449);
            this.tabControllerAbout.MouseState = MaterialSkin.MouseState.Hover;
            this.tabControllerAbout.Name = "tabControllerAbout";
            this.tabControllerAbout.SelectedIndex = 0;
            this.tabControllerAbout.Size = new System.Drawing.Size(674, 449);
            this.tabControllerAbout.TabIndex = 0;
            this.tabControllerAbout.TabStop = false;
            // 
            // tabPageLicense
            // 
            this.tabPageLicense.BackColor = System.Drawing.Color.White;
            this.tabPageLicense.Controls.Add(this.labelFullLicenseLink);
            this.tabPageLicense.Controls.Add(this.labelLicenseText);
            this.tabPageLicense.Controls.Add(this.labelLicenseTitle);
            this.tabPageLicense.Location = new System.Drawing.Point(4, 29);
            this.tabPageLicense.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageLicense.Name = "tabPageLicense";
            this.tabPageLicense.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLicense.Size = new System.Drawing.Size(666, 416);
            this.tabPageLicense.TabIndex = 0;
            this.tabPageLicense.Text = "License";
            this.tabPageLicense.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelFullLicenseLink
            // 
            this.labelFullLicenseLink.AutoSize = true;
            this.labelFullLicenseLink.Font = new System.Drawing.Font("Roboto", 11F);
            this.labelFullLicenseLink.Location = new System.Drawing.Point(13, 374);
            this.labelFullLicenseLink.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.labelFullLicenseLink.Name = "labelFullLicenseLink";
            this.labelFullLicenseLink.Size = new System.Drawing.Size(277, 20);
            this.labelFullLicenseLink.TabIndex = 3;
            this.labelFullLicenseLink.TabStop = true;
            this.labelFullLicenseLink.Text = "GNU General Public License - Version 3";
            this.labelFullLicenseLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnFullLicenseLinkClicked);
            // 
            // labelLicenseText
            // 
            this.labelLicenseText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLicenseText.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLicenseText.Location = new System.Drawing.Point(14, 56);
            this.labelLicenseText.Name = "labelLicenseText";
            this.labelLicenseText.Size = new System.Drawing.Size(641, 249);
            this.labelLicenseText.TabIndex = 4;
            this.labelLicenseText.Text = resources.GetString("labelLicenseText.Text");
            // 
            // labelLicenseTitle
            // 
            this.labelLicenseTitle.AutoSize = true;
            this.labelLicenseTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLicenseTitle.Location = new System.Drawing.Point(10, 10);
            this.labelLicenseTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelLicenseTitle.Name = "labelLicenseTitle";
            this.labelLicenseTitle.Size = new System.Drawing.Size(104, 38);
            this.labelLicenseTitle.TabIndex = 1;
            this.labelLicenseTitle.Text = "License";
            // 
            // tabPageDonate
            // 
            this.tabPageDonate.BackColor = System.Drawing.Color.White;
            this.tabPageDonate.Controls.Add(this.pictureBoxDonate);
            this.tabPageDonate.Controls.Add(this.labelDonateText);
            this.tabPageDonate.Controls.Add(this.labelDonateTitle);
            this.tabPageDonate.Location = new System.Drawing.Point(4, 22);
            this.tabPageDonate.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageDonate.Name = "tabPageDonate";
            this.tabPageDonate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDonate.Size = new System.Drawing.Size(666, 423);
            this.tabPageDonate.TabIndex = 1;
            this.tabPageDonate.Text = "Donate";
            this.tabPageDonate.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // pictureBoxDonate
            // 
            this.pictureBoxDonate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxDonate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxDonate.Image = global::Oracle.Properties.Resources.PayPal;
            this.pictureBoxDonate.Location = new System.Drawing.Point(203, 280);
            this.pictureBoxDonate.Margin = new System.Windows.Forms.Padding(200, 3, 200, 3);
            this.pictureBoxDonate.Name = "pictureBoxDonate";
            this.pictureBoxDonate.Size = new System.Drawing.Size(257, 93);
            this.pictureBoxDonate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDonate.TabIndex = 7;
            this.pictureBoxDonate.TabStop = false;
            this.pictureBoxDonate.Click += new System.EventHandler(this.OnDonatePictureBoxClick);
            // 
            // labelDonateText
            // 
            this.labelDonateText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDonateText.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDonateText.Location = new System.Drawing.Point(14, 56);
            this.labelDonateText.Margin = new System.Windows.Forms.Padding(5);
            this.labelDonateText.Name = "labelDonateText";
            this.labelDonateText.Size = new System.Drawing.Size(641, 171);
            this.labelDonateText.TabIndex = 5;
            this.labelDonateText.Text = resources.GetString("labelDonateText.Text");
            // 
            // labelDonateTitle
            // 
            this.labelDonateTitle.AutoSize = true;
            this.labelDonateTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDonateTitle.Location = new System.Drawing.Point(10, 10);
            this.labelDonateTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelDonateTitle.Name = "labelDonateTitle";
            this.labelDonateTitle.Size = new System.Drawing.Size(96, 38);
            this.labelDonateTitle.TabIndex = 4;
            this.labelDonateTitle.Text = "Donate";
            // 
            // tabPageDevelopment
            // 
            this.tabPageDevelopment.BackColor = System.Drawing.Color.White;
            this.tabPageDevelopment.Controls.Add(this.labelDevelopmentTitle);
            this.tabPageDevelopment.Location = new System.Drawing.Point(4, 22);
            this.tabPageDevelopment.Name = "tabPageDevelopment";
            this.tabPageDevelopment.Size = new System.Drawing.Size(666, 423);
            this.tabPageDevelopment.TabIndex = 2;
            this.tabPageDevelopment.Text = "Development";
            this.tabPageDevelopment.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelDevelopmentTitle
            // 
            this.labelDevelopmentTitle.AutoSize = true;
            this.labelDevelopmentTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDevelopmentTitle.Location = new System.Drawing.Point(10, 10);
            this.labelDevelopmentTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelDevelopmentTitle.Name = "labelDevelopmentTitle";
            this.labelDevelopmentTitle.Size = new System.Drawing.Size(164, 38);
            this.labelDevelopmentTitle.TabIndex = 2;
            this.labelDevelopmentTitle.Text = "Development";
            // 
            // tabSelectorAbout
            // 
            this.tabSelectorAbout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tabSelectorAbout.BaseTabControl = this.tabControllerAbout;
            this.tabSelectorAbout.Depth = 0;
            this.tabSelectorAbout.Location = new System.Drawing.Point(-4, 0);
            this.tabSelectorAbout.Margin = new System.Windows.Forms.Padding(0);
            this.tabSelectorAbout.MinimumSize = new System.Drawing.Size(173, 449);
            this.tabSelectorAbout.MouseState = MaterialSkin.MouseState.Hover;
            this.tabSelectorAbout.Name = "tabSelectorAbout";
            this.tabSelectorAbout.Size = new System.Drawing.Size(173, 449);
            this.tabSelectorAbout.TabIndex = 5;
            this.tabSelectorAbout.TabStop = false;
            this.tabSelectorAbout.Text = "materialTabSelectorVertical1";
            this.tabSelectorAbout.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelVersionInformation
            // 
            this.labelVersionInformation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelVersionInformation.BackColor = System.Drawing.Color.Transparent;
            this.labelVersionInformation.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelVersionInformation.Font = new System.Drawing.Font("Roboto Condensed", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersionInformation.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelVersionInformation.Location = new System.Drawing.Point(0, 9);
            this.labelVersionInformation.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelVersionInformation.Name = "labelVersionInformation";
            this.labelVersionInformation.Size = new System.Drawing.Size(173, 24);
            this.labelVersionInformation.TabIndex = 4;
            this.labelVersionInformation.Text = "Oracle Version: ";
            this.labelVersionInformation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelVersionInformation.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindow);
            // 
            // panelControl
            // 
            this.panelControl.BackColor = System.Drawing.Color.Gainsboro;
            this.panelControl.Controls.Add(this.labelVersionInformation);
            this.panelControl.Controls.Add(this.buttonClose);
            this.panelControl.Controls.Add(this.labelDefaultFocus);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl.Location = new System.Drawing.Point(0, 533);
            this.panelControl.Margin = new System.Windows.Forms.Padding(0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(850, 42);
            this.panelControl.TabIndex = 5;
            this.panelControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindow);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonClose.Depth = 0;
            this.buttonClose.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonClose.Location = new System.Drawing.Point(746, 6);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonClose.MouseState = MaterialSkin.MouseState.Hover;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Primary = true;
            this.buttonClose.Size = new System.Drawing.Size(100, 30);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.OnCloseButtonClick);
            // 
            // labelDefaultFocus
            // 
            this.labelDefaultFocus.AutoSize = true;
            this.labelDefaultFocus.Location = new System.Drawing.Point(348, 22);
            this.labelDefaultFocus.Name = "labelDefaultFocus";
            this.labelDefaultFocus.Size = new System.Drawing.Size(146, 20);
            this.labelDefaultFocus.TabIndex = 0;
            this.labelDefaultFocus.Text = "Default Focus Label";
            this.labelDefaultFocus.Visible = false;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLogo.Image = global::Oracle.Properties.Resources.Logo;
            this.pictureBoxLogo.Location = new System.Drawing.Point(7, 6);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(155, 50);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            this.pictureBoxLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindow);
            // 
            // SettingsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(850, 575);
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.tabControllerMain);
            this.Controls.Add(this.tabSelectorMain);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Sizable = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Oracle";
            this.tabControllerMain.ResumeLayout(false);
            this.tabGeneralSettings.ResumeLayout(false);
            this.tabControllerGeneral.ResumeLayout(false);
            this.tabPageOracleMode.ResumeLayout(false);
            this.tabPageOracleMode.PerformLayout();
            this.tabControllerOracleMode.ResumeLayout(false);
            this.tabPageOracleModeFateGrind.ResumeLayout(false);
            this.tabPageOracleModeSpecificFate.ResumeLayout(false);
            this.tabPageOracleModeSpecificFate.PerformLayout();
            this.tabPageOracleModeAtmaGrind.ResumeLayout(false);
            this.tabPageOracleModeAtmaGrind.PerformLayout();
            this.tabPageOracleModeAnimusGrind.ResumeLayout(false);
            this.tabPageOracleModeAnimusGrind.PerformLayout();
            this.tabPageOracleModeAnimaGrind.ResumeLayout(false);
            this.tabPageFateSelection.ResumeLayout(false);
            this.tabPageFateSelection.PerformLayout();
            this.tabPageDowntime.ResumeLayout(false);
            this.tabPageDowntime.PerformLayout();
            this.tabControllerDowntime.ResumeLayout(false);
            this.tabPageDowntimeReturnToAetheryte.ResumeLayout(false);
            this.tabPageDowntimeMoveToLocation.ResumeLayout(false);
            this.tabPageDowntimeMoveToLocation.PerformLayout();
            this.tabPageDowntimeGrindMobs.ResumeLayout(false);
            this.tabPageDowntimeGrindMobs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMobMaximumLevelAboveSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMobMinimumLevelBelowSetting)).EndInit();
            this.tabPageDowntimeDoNothing.ResumeLayout(false);
            this.tabPageZoneChanging.ResumeLayout(false);
            this.tabPageZoneChanging.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewZoneChangeSettings)).EndInit();
            this.tabPageMiscellaneous.ResumeLayout(false);
            this.tabPageMiscellaneous.PerformLayout();
            this.tabFateSettings.ResumeLayout(false);
            this.tabControllerFate.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFateMaximumLevelAboveSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFateMinimumLevelBelowSetting)).EndInit();
            this.tabPageKillFates.ResumeLayout(false);
            this.tabPageKillFates.PerformLayout();
            this.tabPageCollectFates.ResumeLayout(false);
            this.tabPageCollectFates.PerformLayout();
            this.tabPageEscortFates.ResumeLayout(false);
            this.tabPageEscortFates.PerformLayout();
            this.tabPageDefenceFates.ResumeLayout(false);
            this.tabPageDefenceFates.PerformLayout();
            this.tabPageBossFates.ResumeLayout(false);
            this.tabPageBossFates.PerformLayout();
            this.tabPageMegaBossFates.ResumeLayout(false);
            this.tabPageMegaBossFates.PerformLayout();
            this.tabNavigationSettings.ResumeLayout(false);
            this.tabControllerCustom.ResumeLayout(false);
            this.tabPageMovement.ResumeLayout(false);
            this.tabPageMovement.PerformLayout();
            this.tabPageFlight.ResumeLayout(false);
            this.tabPageFlight.PerformLayout();
            this.tabPageTeleport.ResumeLayout(false);
            this.tabPageTeleport.PerformLayout();
            this.tabBlacklist.ResumeLayout(false);
            this.tabControllerBlacklist.ResumeLayout(false);
            this.tabPageFateBlacklist.ResumeLayout(false);
            this.tabPageFateBlacklist.PerformLayout();
            this.tabPageMobBlacklist.ResumeLayout(false);
            this.tabPageMobBlacklist.PerformLayout();
            this.tabAbout.ResumeLayout(false);
            this.tabControllerAbout.ResumeLayout(false);
            this.tabPageLicense.ResumeLayout(false);
            this.tabPageLicense.PerformLayout();
            this.tabPageDonate.ResumeLayout(false);
            this.tabPageDonate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDonate)).EndInit();
            this.tabPageDevelopment.ResumeLayout(false);
            this.tabPageDevelopment.PerformLayout();
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabSelector tabSelectorMain;
        private MaterialSkin.Controls.MaterialTabControl tabControllerMain;
        private System.Windows.Forms.TabPage tabGeneralSettings;
        private System.Windows.Forms.TabPage tabFateSettings;
        private System.Windows.Forms.TabPage tabNavigationSettings;
        private System.Windows.Forms.TabPage tabBlacklist;
        private System.Windows.Forms.Label labelVersionInformation;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.LinkLabel labelFullLicenseLink;
        private MaterialSkin.Controls.MaterialTabControl tabControllerAbout;
        private System.Windows.Forms.TabPage tabPageLicense;
        private System.Windows.Forms.TabPage tabPageDonate;
        private System.Windows.Forms.Label labelLicenseTitle;
        private MaterialSkin.Controls.MaterialTabSelectorVertical tabSelectorAbout;
        private System.Windows.Forms.Label labelDonateTitle;
        private System.Windows.Forms.Panel panelControl;
        private MaterialSkin.Controls.MaterialTabControl tabControllerBlacklist;
        private System.Windows.Forms.TabPage tabPageFateBlacklist;
        private System.Windows.Forms.Label labelFateBlacklistTitle;
        private System.Windows.Forms.TabPage tabPageMobBlacklist;
        private System.Windows.Forms.Label labelMobBlacklistTitle;
        private MaterialSkin.Controls.MaterialTabSelectorVertical tabSelectorBlacklist;
        private System.Windows.Forms.Label labelLicenseText;
        private System.Windows.Forms.Label labelDonateText;
        private MaterialSkin.Controls.MaterialTabSelectorVertical tabSelectorCustom;
        private MaterialSkin.Controls.MaterialTabControl tabControllerCustom;
        private System.Windows.Forms.TabPage tabPageMovement;
        private System.Windows.Forms.Label labelMovementTitle;
        private System.Windows.Forms.TabPage tabPageFlight;
        private System.Windows.Forms.Label labelFlightTitle;
        private MaterialSkin.Controls.MaterialTabSelectorVertical tabSelectorFate;
        private MaterialSkin.Controls.MaterialTabControl tabControllerFate;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.Label labelGeneralFateSettingsTitle;
        private System.Windows.Forms.TabPage tabPageKillFates;
        private System.Windows.Forms.Label labelKillFatesTitle;
        private System.Windows.Forms.TabPage tabPageCollectFates;
        private System.Windows.Forms.Label labelCollectFatesTitle;
        private System.Windows.Forms.TabPage tabPageEscortFates;
        private System.Windows.Forms.TabPage tabPageDefenceFates;
        private System.Windows.Forms.TabPage tabPageBossFates;
        private System.Windows.Forms.Label labelEscortFatesTitle;
        private System.Windows.Forms.Label labelDefenceFatesTitle;
        private System.Windows.Forms.Label labelBossFatesTitle;
        private MaterialSkin.Controls.MaterialFlatButton buttonClose;
        private MaterialSkin.Controls.MaterialTabControl tabControllerGeneral;
        private System.Windows.Forms.TabPage tabPageOracleMode;
        private System.Windows.Forms.Label labelOracleModeTitle;
        private System.Windows.Forms.TabPage tabPageFateSelection;
        private System.Windows.Forms.Label labelFateSelectionTitle;
        private MaterialSkin.Controls.MaterialTabSelectorVertical tabSelectorGeneral;
        private System.Windows.Forms.TabPage tabPageDowntime;
        private System.Windows.Forms.TabPage tabPageZoneChanging;
        private System.Windows.Forms.TabPage tabPageMiscellaneous;
        private System.Windows.Forms.Label labelDowntimeTitle;
        private System.Windows.Forms.Label labelZoneChangingTitle;
        private System.Windows.Forms.Label labelMiscellaneousTitle;
        private MaterialSkin.Controls.MaterialPictureBox pictureBoxLogo;
        private MaterialSkin.Controls.MaterialPictureBox pictureBoxDonate;
        private System.Windows.Forms.TabPage tabPageMegaBossFates;
        private System.Windows.Forms.Label labelMegaBossFatesTitle;
        private System.Windows.Forms.TabPage tabPageTeleport;
        private System.Windows.Forms.Label labelTeleportTitle;
        private System.Windows.Forms.TabPage tabPageDevelopment;
        private System.Windows.Forms.Label labelDevelopmentTitle;
        private System.Windows.Forms.ComboBox comboBoxOracleModeSetting;
        private System.Windows.Forms.TabPage tabPageOracleModeFateGrind;
        private System.Windows.Forms.TabPage tabPageOracleModeSpecificFate;
        private MaterialSkin.Controls.MaterialTabControl tabControllerOracleMode;
        private System.Windows.Forms.TabPage tabPageOracleModeAtmaGrind;
        private System.Windows.Forms.TabPage tabPageOracleModeAnimusGrind;
        private System.Windows.Forms.TabPage tabPageOracleModeAnimaGrind;
        private System.Windows.Forms.Label labelAtmaGrindNYI;
        private System.Windows.Forms.Label labelOracleModeSetting;
        private System.Windows.Forms.Label labelAnimusGrindModeNYI;
        private System.Windows.Forms.Label labelOracleModeFateGrind;
        private System.Windows.Forms.Label labelOracleModeSpecificFate;
        private MaterialSkin.Controls.MaterialSingleLineTextField textBoxSpecificFateNameSetting;
        private System.Windows.Forms.Label labelSpecificFateNameSetting;
        private System.Windows.Forms.Label labelDefaultFocus;
        private System.Windows.Forms.Label labelAnimaGrindMode;
        private System.Windows.Forms.Label labelFateSelectStrategySetting;
        private System.Windows.Forms.ComboBox comboBoxFateSelectStrategySetting;
        private System.Windows.Forms.Label labelFateSelectionDescription;
        private System.Windows.Forms.ComboBox comboBoxFateWaitModeSetting;
        private System.Windows.Forms.Label labelDowntimeBehaviourSetting;
        private MaterialSkin.Controls.MaterialTabControl tabControllerDowntime;
        private System.Windows.Forms.TabPage tabPageDowntimeReturnToAetheryte;
        private System.Windows.Forms.Label labelDowntimeReturnToAetheryte;
        private System.Windows.Forms.TabPage tabPageDowntimeMoveToLocation;
        private System.Windows.Forms.Label labelDowntimeMoveToLocation;
        private System.Windows.Forms.TabPage tabPageDowntimeGrindMobs;
        private System.Windows.Forms.Label labelDowntimeGrindMobs;
        private System.Windows.Forms.TabPage tabPageDowntimeDoNothing;
        private System.Windows.Forms.Label labelDowntimeDoNothing;
        private System.Windows.Forms.Label labelDowntimeCurrentZone;
        private System.Windows.Forms.Label labelDowntimeWaitLocation;
        private System.Windows.Forms.Label labelDowntimeWaitLocationValue;
        private System.Windows.Forms.Label labelDowntimeCurrentZoneValue;
        private MaterialSkin.Controls.MaterialRaisedButton buttonDowntimeSetLocation;
        private MaterialSkin.Controls.MaterialRaisedButton buttonDowntimeRefreshZone;
        private MaterialSkin.Controls.MaterialCheckBox checkBoxZoneChangingEnabledSetting;
        private System.Windows.Forms.Label labelZoneChangeEnabledSetting;
        private System.Windows.Forms.Label labelMobMaxLevelAboveSetting;
        private System.Windows.Forms.Label labelMobMinLevelBelowSetting;
        private System.Windows.Forms.NumericUpDown numericUpDownMobMinimumLevelBelowSetting;
        private System.Windows.Forms.NumericUpDown numericUpDownMobMaximumLevelAboveSetting;
        private System.Windows.Forms.DataGridView dataGridViewZoneChangeSettings;
        private System.Windows.Forms.Label labelZoneChangeTip;
        private MaterialSkin.Controls.MaterialRaisedButton buttonResetZoneLevelsToDefault;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLevel;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnAetheryte;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmptyColumn;
        private MaterialSkin.Controls.MaterialCheckBox checkBoxBindHomePointSetting;
        private System.Windows.Forms.Label labelBindHomePointSetting;
        private System.Windows.Forms.NumericUpDown numericUpDownFateMinimumLevelBelowSetting;
        private System.Windows.Forms.Label labelFateMinimumLevelBelowSetting;
        private System.Windows.Forms.NumericUpDown numericUpDownFateMaximumLevelAboveSetting;
        private System.Windows.Forms.Label labelFateMaximumLevelAboveSetting;
        private MaterialSkin.Controls.MaterialCheckBox checkBoxRunProblematicFatesSetting;
        private System.Windows.Forms.Label labelRunProblematicFatesWarning;
        private System.Windows.Forms.Label labelRunProblematicFatesSetting;
        private MaterialSkin.Controls.MaterialCheckBox checkBoxIgnoreLowDurationUnstartedFateSetting;
        private System.Windows.Forms.Label labelIgnoreLowDurationFatesSetting;
        private System.Windows.Forms.Label labelLowDurationTimeSetting;
        private MaterialSkin.Controls.MaterialSingleLineTextField textBoxLowRemainingFateDurationSetting;
        private System.Windows.Forms.Label labelLowDurationTimeSettingSuffix;
        private MaterialSkin.Controls.MaterialCheckBox checkBoxWaitForChainFateSetting;
        private Label labelWaitForChainFateSetting;
        private MaterialSkin.Controls.MaterialCheckBox checkBoxKillFatesEnabledSetting;
        private Label labelKillFatesEnabledSetting;
        private MaterialSkin.Controls.MaterialCheckBox checkBoxCollectFatesEnabledSetting;
        private Label labelCollectFatesEnabledSetting;
        private MaterialSkin.Controls.MaterialCheckBox checkBoxEscortFatesEnabledSetting;
        private Label labelEscortFatesEnabledSetting;
        private MaterialSkin.Controls.MaterialCheckBox checkBoxDefenceFatesEnabledSetting;
        private Label labelDefenceFatesEnabledSetting;
        private MaterialSkin.Controls.MaterialCheckBox checkBoxBossFatesEnabledSetting;
        private Label labelBossFatesEnabledSetting;
        private MaterialSkin.Controls.MaterialCheckBox checkBoxMegaBossFatesEnabledSetting;
        private Label labelMegaBossFatesEnabledSetting;
        private Label labelChainFateWaitTimeoutSettingSuffix;
        private MaterialSkin.Controls.MaterialSingleLineTextField textBoxChainFateWaitTimeoutSetting;
        private Label labelChainFateWaitTimeoutSetting;
    }
}