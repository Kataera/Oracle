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
            this.tabSelectorMain = new MaterialSkin.Controls.MaterialTabSelector();
            this.tabControllerMain = new MaterialSkin.Controls.MaterialTabControl();
            this.tabGeneralSettings = new System.Windows.Forms.TabPage();
            this.tabSelectorGeneral = new MaterialSkin.Controls.MaterialTabSelectorVertical();
            this.tabControllerGeneral = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPageOracleMode = new System.Windows.Forms.TabPage();
            this.tabControlOracleMode = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPageOracleModeFateGrind = new System.Windows.Forms.TabPage();
            this.labelOracleModeFateGrind = new System.Windows.Forms.Label();
            this.tabPageOracleModeSpecificFate = new System.Windows.Forms.TabPage();
            this.labelSpecificFateName = new System.Windows.Forms.Label();
            this.textBoxSpecificFateName = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.labelOracleModeSpecificFate = new System.Windows.Forms.Label();
            this.tabPageOracleModeAtmaGrind = new System.Windows.Forms.TabPage();
            this.labelAtmaGrindNYI = new System.Windows.Forms.Label();
            this.tabPageOracleModeZetaGrind = new System.Windows.Forms.TabPage();
            this.labelZetaGrindModeNYI = new System.Windows.Forms.Label();
            this.tabPageOracleModeAnimaGrind = new System.Windows.Forms.TabPage();
            this.labelAnimaGrindMode = new System.Windows.Forms.Label();
            this.materialLabelOracleMode = new System.Windows.Forms.Label();
            this.comboBoxOracleMode = new System.Windows.Forms.ComboBox();
            this.labelOracleModeTitle = new System.Windows.Forms.Label();
            this.tabPageFateSelection = new System.Windows.Forms.TabPage();
            this.labelFateSelectionTitle = new System.Windows.Forms.Label();
            this.tabPageDowntime = new System.Windows.Forms.TabPage();
            this.labelDowntimeTitle = new System.Windows.Forms.Label();
            this.tabPageZoneChange = new System.Windows.Forms.TabPage();
            this.labelZoneChangeTitle = new System.Windows.Forms.Label();
            this.tabPageMiscellaneous = new System.Windows.Forms.TabPage();
            this.labelMiscellaneousTitle = new System.Windows.Forms.Label();
            this.tabFateSettings = new System.Windows.Forms.TabPage();
            this.tabControllerFate = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.labelGeneralFateSettingsTitle = new System.Windows.Forms.Label();
            this.tabPageKillFates = new System.Windows.Forms.TabPage();
            this.labelKillFatesTitle = new System.Windows.Forms.Label();
            this.tabPageCollectFates = new System.Windows.Forms.TabPage();
            this.labelCollectFatesTitle = new System.Windows.Forms.Label();
            this.tabPageEscortFates = new System.Windows.Forms.TabPage();
            this.labelEscortFatesTitle = new System.Windows.Forms.Label();
            this.tabPageDefenceFates = new System.Windows.Forms.TabPage();
            this.labelDefenceFatesTitle = new System.Windows.Forms.Label();
            this.tabPageBossFates = new System.Windows.Forms.TabPage();
            this.labelBossFatesTitle = new System.Windows.Forms.Label();
            this.tabPageMegaBossFates = new System.Windows.Forms.TabPage();
            this.labelMegaBossFatesTitle = new System.Windows.Forms.Label();
            this.tabSelectorFate = new MaterialSkin.Controls.MaterialTabSelectorVertical();
            this.tabNavigation = new System.Windows.Forms.TabPage();
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
            this.labelTooltip = new System.Windows.Forms.Label();
            this.panelControl = new System.Windows.Forms.Panel();
            this.buttonClose = new MaterialSkin.Controls.MaterialFlatButton();
            this.pictureBoxLogo = new MaterialSkin.Controls.MaterialPictureBox();
            this.labelDefaultFocus = new System.Windows.Forms.Label();
            this.tabControllerMain.SuspendLayout();
            this.tabGeneralSettings.SuspendLayout();
            this.tabControllerGeneral.SuspendLayout();
            this.tabPageOracleMode.SuspendLayout();
            this.tabControlOracleMode.SuspendLayout();
            this.tabPageOracleModeFateGrind.SuspendLayout();
            this.tabPageOracleModeSpecificFate.SuspendLayout();
            this.tabPageOracleModeAtmaGrind.SuspendLayout();
            this.tabPageOracleModeZetaGrind.SuspendLayout();
            this.tabPageOracleModeAnimaGrind.SuspendLayout();
            this.tabPageFateSelection.SuspendLayout();
            this.tabPageDowntime.SuspendLayout();
            this.tabPageZoneChange.SuspendLayout();
            this.tabPageMiscellaneous.SuspendLayout();
            this.tabFateSettings.SuspendLayout();
            this.tabControllerFate.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageKillFates.SuspendLayout();
            this.tabPageCollectFates.SuspendLayout();
            this.tabPageEscortFates.SuspendLayout();
            this.tabPageDefenceFates.SuspendLayout();
            this.tabPageBossFates.SuspendLayout();
            this.tabPageMegaBossFates.SuspendLayout();
            this.tabNavigation.SuspendLayout();
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
            this.tabSelectorMain.Text = "Settings Tabs";
            // 
            // tabControllerMain
            // 
            this.tabControllerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControllerMain.Controls.Add(this.tabGeneralSettings);
            this.tabControllerMain.Controls.Add(this.tabFateSettings);
            this.tabControllerMain.Controls.Add(this.tabNavigation);
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
            this.tabSelectorGeneral.Text = "materialTabSelectorVertical1";
            // 
            // tabControllerGeneral
            // 
            this.tabControllerGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControllerGeneral.Controls.Add(this.tabPageOracleMode);
            this.tabControllerGeneral.Controls.Add(this.tabPageFateSelection);
            this.tabControllerGeneral.Controls.Add(this.tabPageDowntime);
            this.tabControllerGeneral.Controls.Add(this.tabPageZoneChange);
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
            // 
            // tabPageOracleMode
            // 
            this.tabPageOracleMode.AutoScroll = true;
            this.tabPageOracleMode.BackColor = System.Drawing.Color.White;
            this.tabPageOracleMode.Controls.Add(this.tabControlOracleMode);
            this.tabPageOracleMode.Controls.Add(this.materialLabelOracleMode);
            this.tabPageOracleMode.Controls.Add(this.comboBoxOracleMode);
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
            // tabControlOracleMode
            // 
            this.tabControlOracleMode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlOracleMode.Controls.Add(this.tabPageOracleModeFateGrind);
            this.tabControlOracleMode.Controls.Add(this.tabPageOracleModeSpecificFate);
            this.tabControlOracleMode.Controls.Add(this.tabPageOracleModeAtmaGrind);
            this.tabControlOracleMode.Controls.Add(this.tabPageOracleModeZetaGrind);
            this.tabControlOracleMode.Controls.Add(this.tabPageOracleModeAnimaGrind);
            this.tabControlOracleMode.Depth = 0;
            this.tabControlOracleMode.Location = new System.Drawing.Point(3, 117);
            this.tabControlOracleMode.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlOracleMode.MouseState = MaterialSkin.MouseState.Hover;
            this.tabControlOracleMode.Name = "tabControlOracleMode";
            this.tabControlOracleMode.SelectedIndex = 0;
            this.tabControlOracleMode.Size = new System.Drawing.Size(655, 283);
            this.tabControlOracleMode.TabIndex = 4;
            // 
            // tabPageOracleModeFateGrind
            // 
            this.tabPageOracleModeFateGrind.BackColor = System.Drawing.Color.White;
            this.tabPageOracleModeFateGrind.Controls.Add(this.labelOracleModeFateGrind);
            this.tabPageOracleModeFateGrind.Location = new System.Drawing.Point(4, 29);
            this.tabPageOracleModeFateGrind.Name = "tabPageOracleModeFateGrind";
            this.tabPageOracleModeFateGrind.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOracleModeFateGrind.Size = new System.Drawing.Size(647, 250);
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
            this.labelOracleModeFateGrind.Size = new System.Drawing.Size(631, 240);
            this.labelOracleModeFateGrind.TabIndex = 1;
            this.labelOracleModeFateGrind.Text = resources.GetString("labelOracleModeFateGrind.Text");
            // 
            // tabPageOracleModeSpecificFate
            // 
            this.tabPageOracleModeSpecificFate.BackColor = System.Drawing.Color.White;
            this.tabPageOracleModeSpecificFate.Controls.Add(this.labelSpecificFateName);
            this.tabPageOracleModeSpecificFate.Controls.Add(this.textBoxSpecificFateName);
            this.tabPageOracleModeSpecificFate.Controls.Add(this.labelOracleModeSpecificFate);
            this.tabPageOracleModeSpecificFate.Location = new System.Drawing.Point(4, 22);
            this.tabPageOracleModeSpecificFate.Name = "tabPageOracleModeSpecificFate";
            this.tabPageOracleModeSpecificFate.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOracleModeSpecificFate.Size = new System.Drawing.Size(647, 257);
            this.tabPageOracleModeSpecificFate.TabIndex = 1;
            this.tabPageOracleModeSpecificFate.Text = "Specific FATE";
            this.tabPageOracleModeSpecificFate.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelSpecificFateName
            // 
            this.labelSpecificFateName.AutoSize = true;
            this.labelSpecificFateName.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSpecificFateName.Location = new System.Drawing.Point(18, 45);
            this.labelSpecificFateName.Name = "labelSpecificFateName";
            this.labelSpecificFateName.Size = new System.Drawing.Size(89, 18);
            this.labelSpecificFateName.TabIndex = 4;
            this.labelSpecificFateName.Text = "FATE Name:";
            // 
            // textBoxSpecificFateName
            // 
            this.textBoxSpecificFateName.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxSpecificFateName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBoxSpecificFateName.Depth = 0;
            this.textBoxSpecificFateName.Hint = "";
            this.textBoxSpecificFateName.Location = new System.Drawing.Point(113, 45);
            this.textBoxSpecificFateName.MaxLength = 100;
            this.textBoxSpecificFateName.MouseState = MaterialSkin.MouseState.Hover;
            this.textBoxSpecificFateName.Name = "textBoxSpecificFateName";
            this.textBoxSpecificFateName.PasswordChar = '\0';
            this.textBoxSpecificFateName.SelectedText = "";
            this.textBoxSpecificFateName.SelectionLength = 0;
            this.textBoxSpecificFateName.SelectionStart = 0;
            this.textBoxSpecificFateName.Size = new System.Drawing.Size(257, 25);
            this.textBoxSpecificFateName.TabIndex = 3;
            this.textBoxSpecificFateName.TabStop = false;
            this.textBoxSpecificFateName.UseSystemPasswordChar = false;
            this.textBoxSpecificFateName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTextBoxSpecificFateNameKeyDown);
            this.textBoxSpecificFateName.TextChanged += new System.EventHandler(this.OnTextBoxSpecificFateNameTextChanged);
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
            this.labelOracleModeSpecificFate.Text = "Specific FATE mode will only run the FATE you specify below:";
            // 
            // tabPageOracleModeAtmaGrind
            // 
            this.tabPageOracleModeAtmaGrind.BackColor = System.Drawing.Color.White;
            this.tabPageOracleModeAtmaGrind.Controls.Add(this.labelAtmaGrindNYI);
            this.tabPageOracleModeAtmaGrind.Location = new System.Drawing.Point(4, 22);
            this.tabPageOracleModeAtmaGrind.Name = "tabPageOracleModeAtmaGrind";
            this.tabPageOracleModeAtmaGrind.Size = new System.Drawing.Size(647, 257);
            this.tabPageOracleModeAtmaGrind.TabIndex = 2;
            this.tabPageOracleModeAtmaGrind.Text = "Atma Grind";
            this.tabPageOracleModeAtmaGrind.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelAtmaGrindNYI
            // 
            this.labelAtmaGrindNYI.AutoSize = true;
            this.labelAtmaGrindNYI.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAtmaGrindNYI.ForeColor = System.Drawing.Color.Red;
            this.labelAtmaGrindNYI.Location = new System.Drawing.Point(10, 0);
            this.labelAtmaGrindNYI.Name = "labelAtmaGrindNYI";
            this.labelAtmaGrindNYI.Size = new System.Drawing.Size(285, 18);
            this.labelAtmaGrindNYI.TabIndex = 0;
            this.labelAtmaGrindNYI.Text = "Atma Grind mode is not yet implemented.";
            // 
            // tabPageOracleModeZetaGrind
            // 
            this.tabPageOracleModeZetaGrind.BackColor = System.Drawing.Color.White;
            this.tabPageOracleModeZetaGrind.Controls.Add(this.labelZetaGrindModeNYI);
            this.tabPageOracleModeZetaGrind.Location = new System.Drawing.Point(4, 29);
            this.tabPageOracleModeZetaGrind.Name = "tabPageOracleModeZetaGrind";
            this.tabPageOracleModeZetaGrind.Size = new System.Drawing.Size(647, 250);
            this.tabPageOracleModeZetaGrind.TabIndex = 3;
            this.tabPageOracleModeZetaGrind.Text = "Zeta Grind";
            this.tabPageOracleModeZetaGrind.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelZetaGrindModeNYI
            // 
            this.labelZetaGrindModeNYI.AutoSize = true;
            this.labelZetaGrindModeNYI.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZetaGrindModeNYI.ForeColor = System.Drawing.Color.Red;
            this.labelZetaGrindModeNYI.Location = new System.Drawing.Point(10, 0);
            this.labelZetaGrindModeNYI.Name = "labelZetaGrindModeNYI";
            this.labelZetaGrindModeNYI.Size = new System.Drawing.Size(279, 18);
            this.labelZetaGrindModeNYI.TabIndex = 1;
            this.labelZetaGrindModeNYI.Text = "Zeta Grind mode is not yet implemented.";
            // 
            // tabPageOracleModeAnimaGrind
            // 
            this.tabPageOracleModeAnimaGrind.BackColor = System.Drawing.Color.White;
            this.tabPageOracleModeAnimaGrind.Controls.Add(this.labelAnimaGrindMode);
            this.tabPageOracleModeAnimaGrind.Location = new System.Drawing.Point(4, 29);
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
            // materialLabelOracleMode
            // 
            this.materialLabelOracleMode.AutoSize = true;
            this.materialLabelOracleMode.Font = new System.Drawing.Font("Roboto", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.materialLabelOracleMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabelOracleMode.Location = new System.Drawing.Point(12, 68);
            this.materialLabelOracleMode.Name = "materialLabelOracleMode";
            this.materialLabelOracleMode.Size = new System.Drawing.Size(99, 20);
            this.materialLabelOracleMode.TabIndex = 3;
            this.materialLabelOracleMode.Text = "Oracle Mode:";
            // 
            // comboBoxOracleMode
            // 
            this.comboBoxOracleMode.BackColor = System.Drawing.Color.Azure;
            this.comboBoxOracleMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOracleMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxOracleMode.FormattingEnabled = true;
            this.comboBoxOracleMode.Items.AddRange(new object[] {
            "FATE Grind",
            "Specific FATE",
            "Atma Grind",
            "Zeta Grind",
            "Anima Grind"});
            this.comboBoxOracleMode.Location = new System.Drawing.Point(117, 65);
            this.comboBoxOracleMode.Name = "comboBoxOracleMode";
            this.comboBoxOracleMode.Size = new System.Drawing.Size(121, 28);
            this.comboBoxOracleMode.TabIndex = 2;
            this.comboBoxOracleMode.SelectedIndexChanged += new System.EventHandler(this.OnComboBoxOracleModeSelectedIndexChanged);
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
            this.tabPageDowntime.Controls.Add(this.labelDowntimeTitle);
            this.tabPageDowntime.Location = new System.Drawing.Point(4, 29);
            this.tabPageDowntime.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageDowntime.Name = "tabPageDowntime";
            this.tabPageDowntime.Size = new System.Drawing.Size(666, 416);
            this.tabPageDowntime.TabIndex = 2;
            this.tabPageDowntime.Text = "Downtime";
            this.tabPageDowntime.Click += new System.EventHandler(this.OnTabPageClick);
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
            // tabPageZoneChange
            // 
            this.tabPageZoneChange.BackColor = System.Drawing.Color.White;
            this.tabPageZoneChange.Controls.Add(this.labelZoneChangeTitle);
            this.tabPageZoneChange.Location = new System.Drawing.Point(4, 29);
            this.tabPageZoneChange.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageZoneChange.Name = "tabPageZoneChange";
            this.tabPageZoneChange.Size = new System.Drawing.Size(666, 416);
            this.tabPageZoneChange.TabIndex = 3;
            this.tabPageZoneChange.Text = "Zone Change";
            this.tabPageZoneChange.Click += new System.EventHandler(this.OnTabPageClick);
            // 
            // labelZoneChangeTitle
            // 
            this.labelZoneChangeTitle.AutoSize = true;
            this.labelZoneChangeTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelZoneChangeTitle.Location = new System.Drawing.Point(10, 10);
            this.labelZoneChangeTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelZoneChangeTitle.Name = "labelZoneChangeTitle";
            this.labelZoneChangeTitle.Size = new System.Drawing.Size(262, 38);
            this.labelZoneChangeTitle.TabIndex = 5;
            this.labelZoneChangeTitle.Text = "Zone Change Settings";
            // 
            // tabPageMiscellaneous
            // 
            this.tabPageMiscellaneous.BackColor = System.Drawing.Color.White;
            this.tabPageMiscellaneous.Controls.Add(this.labelMiscellaneousTitle);
            this.tabPageMiscellaneous.Location = new System.Drawing.Point(4, 29);
            this.tabPageMiscellaneous.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMiscellaneous.Name = "tabPageMiscellaneous";
            this.tabPageMiscellaneous.Size = new System.Drawing.Size(666, 416);
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
            this.tabFateSettings.Location = new System.Drawing.Point(4, 29);
            this.tabFateSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tabFateSettings.Name = "tabFateSettings";
            this.tabFateSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabFateSettings.Size = new System.Drawing.Size(842, 488);
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
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.BackColor = System.Drawing.Color.White;
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
            this.tabPageCollectFates.Controls.Add(this.labelCollectFatesTitle);
            this.tabPageCollectFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageCollectFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageCollectFates.Name = "tabPageCollectFates";
            this.tabPageCollectFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageCollectFates.TabIndex = 2;
            this.tabPageCollectFates.Text = "Collect FATEs";
            this.tabPageCollectFates.Click += new System.EventHandler(this.OnTabPageClick);
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
            this.tabPageEscortFates.Controls.Add(this.labelEscortFatesTitle);
            this.tabPageEscortFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageEscortFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageEscortFates.Name = "tabPageEscortFates";
            this.tabPageEscortFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageEscortFates.TabIndex = 3;
            this.tabPageEscortFates.Text = "Escort FATEs";
            this.tabPageEscortFates.Click += new System.EventHandler(this.OnTabPageClick);
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
            this.tabPageDefenceFates.Controls.Add(this.labelDefenceFatesTitle);
            this.tabPageDefenceFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageDefenceFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageDefenceFates.Name = "tabPageDefenceFates";
            this.tabPageDefenceFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageDefenceFates.TabIndex = 4;
            this.tabPageDefenceFates.Text = "Defence FATEs";
            this.tabPageDefenceFates.Click += new System.EventHandler(this.OnTabPageClick);
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
            this.tabPageBossFates.Controls.Add(this.labelBossFatesTitle);
            this.tabPageBossFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageBossFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageBossFates.Name = "tabPageBossFates";
            this.tabPageBossFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageBossFates.TabIndex = 5;
            this.tabPageBossFates.Text = "Boss FATEs";
            this.tabPageBossFates.Click += new System.EventHandler(this.OnTabPageClick);
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
            this.tabPageMegaBossFates.Controls.Add(this.labelMegaBossFatesTitle);
            this.tabPageMegaBossFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageMegaBossFates.Name = "tabPageMegaBossFates";
            this.tabPageMegaBossFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageMegaBossFates.TabIndex = 6;
            this.tabPageMegaBossFates.Text = "Mega-Boss FATEs";
            this.tabPageMegaBossFates.Click += new System.EventHandler(this.OnTabPageClick);
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
            this.tabSelectorFate.Text = "materialTabSelectorVertical1";
            // 
            // tabNavigation
            // 
            this.tabNavigation.BackColor = System.Drawing.Color.White;
            this.tabNavigation.Controls.Add(this.tabSelectorCustom);
            this.tabNavigation.Controls.Add(this.tabControllerCustom);
            this.tabNavigation.Location = new System.Drawing.Point(4, 29);
            this.tabNavigation.Margin = new System.Windows.Forms.Padding(0);
            this.tabNavigation.Name = "tabNavigation";
            this.tabNavigation.Padding = new System.Windows.Forms.Padding(3);
            this.tabNavigation.Size = new System.Drawing.Size(842, 488);
            this.tabNavigation.TabIndex = 2;
            this.tabNavigation.Text = "Navigation";
            this.tabNavigation.Click += new System.EventHandler(this.OnTabPageClick);
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
            this.tabSelectorCustom.Text = "materialTabSelectorVertical1";
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
            this.tabBlacklist.Location = new System.Drawing.Point(4, 29);
            this.tabBlacklist.Margin = new System.Windows.Forms.Padding(0);
            this.tabBlacklist.Name = "tabBlacklist";
            this.tabBlacklist.Padding = new System.Windows.Forms.Padding(3);
            this.tabBlacklist.Size = new System.Drawing.Size(842, 488);
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
            this.tabSelectorBlacklist.Text = "materialTabSelectorVertical1";
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
            this.tabAbout.Location = new System.Drawing.Point(4, 29);
            this.tabAbout.Margin = new System.Windows.Forms.Padding(0);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabAbout.Size = new System.Drawing.Size(842, 488);
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
            this.labelLicenseText.Size = new System.Drawing.Size(641, 256);
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
            this.tabSelectorAbout.Text = "materialTabSelectorVertical1";
            // 
            // labelTooltip
            // 
            this.labelTooltip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTooltip.BackColor = System.Drawing.Color.Transparent;
            this.labelTooltip.Cursor = System.Windows.Forms.Cursors.Help;
            this.labelTooltip.Font = new System.Drawing.Font("Roboto Condensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTooltip.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelTooltip.Location = new System.Drawing.Point(9, 9);
            this.labelTooltip.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelTooltip.Name = "labelTooltip";
            this.labelTooltip.Size = new System.Drawing.Size(735, 39);
            this.labelTooltip.TabIndex = 4;
            this.labelTooltip.Text = resources.GetString("labelTooltip.Text");
            this.labelTooltip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindow);
            // 
            // panelControl
            // 
            this.panelControl.BackColor = System.Drawing.Color.Gainsboro;
            this.panelControl.Controls.Add(this.labelTooltip);
            this.panelControl.Controls.Add(this.buttonClose);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl.Location = new System.Drawing.Point(0, 533);
            this.panelControl.Margin = new System.Windows.Forms.Padding(0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(850, 52);
            this.panelControl.TabIndex = 5;
            this.panelControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindow);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonClose.Depth = 0;
            this.buttonClose.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonClose.Location = new System.Drawing.Point(742, 9);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonClose.MouseState = MaterialSkin.MouseState.Hover;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Primary = true;
            this.buttonClose.Size = new System.Drawing.Size(100, 37);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.OnCloseButtonClick);
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
            // labelDefaultFocus
            // 
            this.labelDefaultFocus.AutoSize = true;
            this.labelDefaultFocus.Location = new System.Drawing.Point(209, 8);
            this.labelDefaultFocus.Name = "labelDefaultFocus";
            this.labelDefaultFocus.Size = new System.Drawing.Size(146, 20);
            this.labelDefaultFocus.TabIndex = 0;
            this.labelDefaultFocus.Text = "Default Focus Label";
            this.labelDefaultFocus.Visible = false;
            // 
            // SettingsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(850, 585);
            this.Controls.Add(this.labelDefaultFocus);
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
            this.tabControlOracleMode.ResumeLayout(false);
            this.tabPageOracleModeFateGrind.ResumeLayout(false);
            this.tabPageOracleModeSpecificFate.ResumeLayout(false);
            this.tabPageOracleModeSpecificFate.PerformLayout();
            this.tabPageOracleModeAtmaGrind.ResumeLayout(false);
            this.tabPageOracleModeAtmaGrind.PerformLayout();
            this.tabPageOracleModeZetaGrind.ResumeLayout(false);
            this.tabPageOracleModeZetaGrind.PerformLayout();
            this.tabPageOracleModeAnimaGrind.ResumeLayout(false);
            this.tabPageFateSelection.ResumeLayout(false);
            this.tabPageFateSelection.PerformLayout();
            this.tabPageDowntime.ResumeLayout(false);
            this.tabPageDowntime.PerformLayout();
            this.tabPageZoneChange.ResumeLayout(false);
            this.tabPageZoneChange.PerformLayout();
            this.tabPageMiscellaneous.ResumeLayout(false);
            this.tabPageMiscellaneous.PerformLayout();
            this.tabFateSettings.ResumeLayout(false);
            this.tabControllerFate.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
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
            this.tabNavigation.ResumeLayout(false);
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabSelector tabSelectorMain;
        private MaterialSkin.Controls.MaterialTabControl tabControllerMain;
        private System.Windows.Forms.TabPage tabGeneralSettings;
        private System.Windows.Forms.TabPage tabFateSettings;
        private System.Windows.Forms.TabPage tabNavigation;
        private System.Windows.Forms.TabPage tabBlacklist;
        private System.Windows.Forms.Label labelTooltip;
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
        private System.Windows.Forms.TabPage tabPageZoneChange;
        private System.Windows.Forms.TabPage tabPageMiscellaneous;
        private System.Windows.Forms.Label labelDowntimeTitle;
        private System.Windows.Forms.Label labelZoneChangeTitle;
        private System.Windows.Forms.Label labelMiscellaneousTitle;
        private MaterialSkin.Controls.MaterialPictureBox pictureBoxLogo;
        private MaterialSkin.Controls.MaterialPictureBox pictureBoxDonate;
        private System.Windows.Forms.TabPage tabPageMegaBossFates;
        private System.Windows.Forms.Label labelMegaBossFatesTitle;
        private System.Windows.Forms.TabPage tabPageTeleport;
        private System.Windows.Forms.Label labelTeleportTitle;
        private System.Windows.Forms.TabPage tabPageDevelopment;
        private System.Windows.Forms.Label labelDevelopmentTitle;
        private System.Windows.Forms.ComboBox comboBoxOracleMode;
        private System.Windows.Forms.TabPage tabPageOracleModeFateGrind;
        private System.Windows.Forms.TabPage tabPageOracleModeSpecificFate;
        private MaterialSkin.Controls.MaterialTabControl tabControlOracleMode;
        private System.Windows.Forms.TabPage tabPageOracleModeAtmaGrind;
        private System.Windows.Forms.TabPage tabPageOracleModeZetaGrind;
        private System.Windows.Forms.TabPage tabPageOracleModeAnimaGrind;
        private System.Windows.Forms.Label labelAtmaGrindNYI;
        private System.Windows.Forms.Label materialLabelOracleMode;
        private System.Windows.Forms.Label labelZetaGrindModeNYI;
        private System.Windows.Forms.Label labelOracleModeFateGrind;
        private System.Windows.Forms.Label labelOracleModeSpecificFate;
        private MaterialSkin.Controls.MaterialSingleLineTextField textBoxSpecificFateName;
        private System.Windows.Forms.Label labelSpecificFateName;
        private System.Windows.Forms.Label labelDefaultFocus;
        private System.Windows.Forms.Label labelAnimaGrindMode;
    }
}