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
            this.tabPageFateSelection = new System.Windows.Forms.TabPage();
            this.labelFateSelectionTitle = new System.Windows.Forms.Label();
            this.tabPageBotMode = new System.Windows.Forms.TabPage();
            this.labelBotModeTitle = new System.Windows.Forms.Label();
            this.tabPageMovement = new System.Windows.Forms.TabPage();
            this.labelMovementTitle = new System.Windows.Forms.Label();
            this.tabPageScheduler = new System.Windows.Forms.TabPage();
            this.labelSchedulerTitle = new System.Windows.Forms.Label();
            this.tabPagePatrol = new System.Windows.Forms.TabPage();
            this.labelPatrolTitle = new System.Windows.Forms.Label();
            this.tabPageMiscellaneous = new System.Windows.Forms.TabPage();
            this.labelMiscellaneousTitle = new System.Windows.Forms.Label();
            this.tabFateSettings = new System.Windows.Forms.TabPage();
            this.tabControllerFate = new MaterialSkin.Controls.MaterialTabControl();
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
            this.tabCustomBehaviour = new System.Windows.Forms.TabPage();
            this.tabSelectorCustom = new MaterialSkin.Controls.MaterialTabSelectorVertical();
            this.tabControllerCustom = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPageAvoidance = new System.Windows.Forms.TabPage();
            this.labelAvoidanceTitle = new System.Windows.Forms.Label();
            this.tabPageNavigation = new System.Windows.Forms.TabPage();
            this.labelNavigationTitle = new System.Windows.Forms.Label();
            this.tabPageSpecialFates = new System.Windows.Forms.TabPage();
            this.labelSpecialFatesTitle = new System.Windows.Forms.Label();
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
            this.pictureBoxDonate = new System.Windows.Forms.PictureBox();
            this.labelDonateText = new System.Windows.Forms.Label();
            this.labelDonateTitle = new System.Windows.Forms.Label();
            this.tabSelectorAbout = new MaterialSkin.Controls.MaterialTabSelectorVertical();
            this.labelTooltip = new System.Windows.Forms.Label();
            this.panelControl = new System.Windows.Forms.Panel();
            this.buttonClose = new MaterialSkin.Controls.MaterialFlatButton();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.tabControllerMain.SuspendLayout();
            this.tabGeneralSettings.SuspendLayout();
            this.tabControllerGeneral.SuspendLayout();
            this.tabPageFateSelection.SuspendLayout();
            this.tabPageBotMode.SuspendLayout();
            this.tabPageMovement.SuspendLayout();
            this.tabPageScheduler.SuspendLayout();
            this.tabPagePatrol.SuspendLayout();
            this.tabPageMiscellaneous.SuspendLayout();
            this.tabFateSettings.SuspendLayout();
            this.tabControllerFate.SuspendLayout();
            this.tabPageKillFates.SuspendLayout();
            this.tabPageCollectFates.SuspendLayout();
            this.tabPageEscortFates.SuspendLayout();
            this.tabPageDefenceFates.SuspendLayout();
            this.tabPageBossFates.SuspendLayout();
            this.tabPageMegaBossFates.SuspendLayout();
            this.tabCustomBehaviour.SuspendLayout();
            this.tabControllerCustom.SuspendLayout();
            this.tabPageAvoidance.SuspendLayout();
            this.tabPageNavigation.SuspendLayout();
            this.tabPageSpecialFates.SuspendLayout();
            this.tabBlacklist.SuspendLayout();
            this.tabControllerBlacklist.SuspendLayout();
            this.tabPageFateBlacklist.SuspendLayout();
            this.tabPageMobBlacklist.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.tabControllerAbout.SuspendLayout();
            this.tabPageLicense.SuspendLayout();
            this.tabPageDonate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDonate)).BeginInit();
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
            this.tabSelectorMain.Location = new System.Drawing.Point(172, 24);
            this.tabSelectorMain.Margin = new System.Windows.Forms.Padding(0);
            this.tabSelectorMain.MouseState = MaterialSkin.MouseState.Hover;
            this.tabSelectorMain.Name = "tabSelectorMain";
            this.tabSelectorMain.Size = new System.Drawing.Size(678, 40);
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
            this.tabControllerMain.Controls.Add(this.tabCustomBehaviour);
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
            this.tabGeneralSettings.UseVisualStyleBackColor = true;
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
            this.tabControllerGeneral.Controls.Add(this.tabPageFateSelection);
            this.tabControllerGeneral.Controls.Add(this.tabPageBotMode);
            this.tabControllerGeneral.Controls.Add(this.tabPageMovement);
            this.tabControllerGeneral.Controls.Add(this.tabPageScheduler);
            this.tabControllerGeneral.Controls.Add(this.tabPagePatrol);
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
            // tabPageFateSelection
            // 
            this.tabPageFateSelection.AutoScroll = true;
            this.tabPageFateSelection.Controls.Add(this.labelFateSelectionTitle);
            this.tabPageFateSelection.Location = new System.Drawing.Point(4, 29);
            this.tabPageFateSelection.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageFateSelection.Name = "tabPageFateSelection";
            this.tabPageFateSelection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFateSelection.Size = new System.Drawing.Size(666, 416);
            this.tabPageFateSelection.TabIndex = 0;
            this.tabPageFateSelection.Text = "FATE Selection";
            this.tabPageFateSelection.UseVisualStyleBackColor = true;
            // 
            // labelFateSelectionTitle
            // 
            this.labelFateSelectionTitle.AutoSize = true;
            this.labelFateSelectionTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFateSelectionTitle.Location = new System.Drawing.Point(10, 10);
            this.labelFateSelectionTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelFateSelectionTitle.Name = "labelFateSelectionTitle";
            this.labelFateSelectionTitle.Size = new System.Drawing.Size(187, 38);
            this.labelFateSelectionTitle.TabIndex = 1;
            this.labelFateSelectionTitle.Text = "FATE Selection";
            // 
            // tabPageBotMode
            // 
            this.tabPageBotMode.Controls.Add(this.labelBotModeTitle);
            this.tabPageBotMode.Location = new System.Drawing.Point(4, 22);
            this.tabPageBotMode.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageBotMode.Name = "tabPageBotMode";
            this.tabPageBotMode.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBotMode.Size = new System.Drawing.Size(666, 423);
            this.tabPageBotMode.TabIndex = 1;
            this.tabPageBotMode.Text = "Bot Mode";
            this.tabPageBotMode.UseVisualStyleBackColor = true;
            // 
            // labelBotModeTitle
            // 
            this.labelBotModeTitle.AutoSize = true;
            this.labelBotModeTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBotModeTitle.Location = new System.Drawing.Point(10, 10);
            this.labelBotModeTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelBotModeTitle.Name = "labelBotModeTitle";
            this.labelBotModeTitle.Size = new System.Drawing.Size(124, 38);
            this.labelBotModeTitle.TabIndex = 4;
            this.labelBotModeTitle.Text = "Bot Mode";
            // 
            // tabPageMovement
            // 
            this.tabPageMovement.Controls.Add(this.labelMovementTitle);
            this.tabPageMovement.Location = new System.Drawing.Point(4, 22);
            this.tabPageMovement.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMovement.Name = "tabPageMovement";
            this.tabPageMovement.Size = new System.Drawing.Size(666, 423);
            this.tabPageMovement.TabIndex = 2;
            this.tabPageMovement.Text = "Movement";
            this.tabPageMovement.UseVisualStyleBackColor = true;
            // 
            // labelMovementTitle
            // 
            this.labelMovementTitle.AutoSize = true;
            this.labelMovementTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMovementTitle.Location = new System.Drawing.Point(10, 10);
            this.labelMovementTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelMovementTitle.Name = "labelMovementTitle";
            this.labelMovementTitle.Size = new System.Drawing.Size(135, 38);
            this.labelMovementTitle.TabIndex = 5;
            this.labelMovementTitle.Text = "Movement";
            // 
            // tabPageScheduler
            // 
            this.tabPageScheduler.Controls.Add(this.labelSchedulerTitle);
            this.tabPageScheduler.Location = new System.Drawing.Point(4, 22);
            this.tabPageScheduler.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageScheduler.Name = "tabPageScheduler";
            this.tabPageScheduler.Size = new System.Drawing.Size(666, 423);
            this.tabPageScheduler.TabIndex = 3;
            this.tabPageScheduler.Text = "Scheduler";
            this.tabPageScheduler.UseVisualStyleBackColor = true;
            // 
            // labelSchedulerTitle
            // 
            this.labelSchedulerTitle.AutoSize = true;
            this.labelSchedulerTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSchedulerTitle.Location = new System.Drawing.Point(10, 10);
            this.labelSchedulerTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelSchedulerTitle.Name = "labelSchedulerTitle";
            this.labelSchedulerTitle.Size = new System.Drawing.Size(130, 38);
            this.labelSchedulerTitle.TabIndex = 5;
            this.labelSchedulerTitle.Text = "Scheduler";
            // 
            // tabPagePatrol
            // 
            this.tabPagePatrol.Controls.Add(this.labelPatrolTitle);
            this.tabPagePatrol.Location = new System.Drawing.Point(4, 22);
            this.tabPagePatrol.Margin = new System.Windows.Forms.Padding(0);
            this.tabPagePatrol.Name = "tabPagePatrol";
            this.tabPagePatrol.Size = new System.Drawing.Size(666, 423);
            this.tabPagePatrol.TabIndex = 4;
            this.tabPagePatrol.Text = "Patrol";
            this.tabPagePatrol.UseVisualStyleBackColor = true;
            // 
            // labelPatrolTitle
            // 
            this.labelPatrolTitle.AutoSize = true;
            this.labelPatrolTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPatrolTitle.Location = new System.Drawing.Point(10, 10);
            this.labelPatrolTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelPatrolTitle.Name = "labelPatrolTitle";
            this.labelPatrolTitle.Size = new System.Drawing.Size(85, 38);
            this.labelPatrolTitle.TabIndex = 5;
            this.labelPatrolTitle.Text = "Patrol";
            // 
            // tabPageMiscellaneous
            // 
            this.tabPageMiscellaneous.Controls.Add(this.labelMiscellaneousTitle);
            this.tabPageMiscellaneous.Location = new System.Drawing.Point(4, 22);
            this.tabPageMiscellaneous.Name = "tabPageMiscellaneous";
            this.tabPageMiscellaneous.Size = new System.Drawing.Size(666, 423);
            this.tabPageMiscellaneous.TabIndex = 5;
            this.tabPageMiscellaneous.Text = "Miscellaneous";
            this.tabPageMiscellaneous.UseVisualStyleBackColor = true;
            // 
            // labelMiscellaneousTitle
            // 
            this.labelMiscellaneousTitle.AutoSize = true;
            this.labelMiscellaneousTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMiscellaneousTitle.Location = new System.Drawing.Point(10, 10);
            this.labelMiscellaneousTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelMiscellaneousTitle.Name = "labelMiscellaneousTitle";
            this.labelMiscellaneousTitle.Size = new System.Drawing.Size(179, 38);
            this.labelMiscellaneousTitle.TabIndex = 6;
            this.labelMiscellaneousTitle.Text = "Miscellaneous";
            // 
            // tabFateSettings
            // 
            this.tabFateSettings.Controls.Add(this.tabControllerFate);
            this.tabFateSettings.Controls.Add(this.tabSelectorFate);
            this.tabFateSettings.Location = new System.Drawing.Point(4, 29);
            this.tabFateSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tabFateSettings.Name = "tabFateSettings";
            this.tabFateSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabFateSettings.Size = new System.Drawing.Size(842, 488);
            this.tabFateSettings.TabIndex = 1;
            this.tabFateSettings.Text = "Fate Settings";
            this.tabFateSettings.UseVisualStyleBackColor = true;
            // 
            // tabControllerFate
            // 
            this.tabControllerFate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.tabControllerFate.Name = "tabControllerFate";
            this.tabControllerFate.SelectedIndex = 0;
            this.tabControllerFate.Size = new System.Drawing.Size(674, 449);
            this.tabControllerFate.TabIndex = 9;
            // 
            // tabPageKillFates
            // 
            this.tabPageKillFates.Controls.Add(this.labelKillFatesTitle);
            this.tabPageKillFates.Location = new System.Drawing.Point(4, 29);
            this.tabPageKillFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageKillFates.Name = "tabPageKillFates";
            this.tabPageKillFates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageKillFates.Size = new System.Drawing.Size(666, 416);
            this.tabPageKillFates.TabIndex = 0;
            this.tabPageKillFates.Text = "Kill FATEs";
            this.tabPageKillFates.UseVisualStyleBackColor = true;
            // 
            // labelKillFatesTitle
            // 
            this.labelKillFatesTitle.AutoSize = true;
            this.labelKillFatesTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKillFatesTitle.Location = new System.Drawing.Point(10, 10);
            this.labelKillFatesTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelKillFatesTitle.Name = "labelKillFatesTitle";
            this.labelKillFatesTitle.Size = new System.Drawing.Size(131, 38);
            this.labelKillFatesTitle.TabIndex = 1;
            this.labelKillFatesTitle.Text = "Kill FATEs";
            // 
            // tabPageCollectFates
            // 
            this.tabPageCollectFates.Controls.Add(this.labelCollectFatesTitle);
            this.tabPageCollectFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageCollectFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageCollectFates.Name = "tabPageCollectFates";
            this.tabPageCollectFates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCollectFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageCollectFates.TabIndex = 1;
            this.tabPageCollectFates.Text = "Collect FATEs";
            this.tabPageCollectFates.UseVisualStyleBackColor = true;
            // 
            // labelCollectFatesTitle
            // 
            this.labelCollectFatesTitle.AutoSize = true;
            this.labelCollectFatesTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCollectFatesTitle.Location = new System.Drawing.Point(10, 10);
            this.labelCollectFatesTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelCollectFatesTitle.Name = "labelCollectFatesTitle";
            this.labelCollectFatesTitle.Size = new System.Drawing.Size(172, 38);
            this.labelCollectFatesTitle.TabIndex = 4;
            this.labelCollectFatesTitle.Text = "Collect FATEs";
            // 
            // tabPageEscortFates
            // 
            this.tabPageEscortFates.Controls.Add(this.labelEscortFatesTitle);
            this.tabPageEscortFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageEscortFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageEscortFates.Name = "tabPageEscortFates";
            this.tabPageEscortFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageEscortFates.TabIndex = 2;
            this.tabPageEscortFates.Text = "Escort FATEs";
            this.tabPageEscortFates.UseVisualStyleBackColor = true;
            // 
            // labelEscortFatesTitle
            // 
            this.labelEscortFatesTitle.AutoSize = true;
            this.labelEscortFatesTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEscortFatesTitle.Location = new System.Drawing.Point(10, 10);
            this.labelEscortFatesTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelEscortFatesTitle.Name = "labelEscortFatesTitle";
            this.labelEscortFatesTitle.Size = new System.Drawing.Size(166, 38);
            this.labelEscortFatesTitle.TabIndex = 5;
            this.labelEscortFatesTitle.Text = "Escort FATEs";
            // 
            // tabPageDefenceFates
            // 
            this.tabPageDefenceFates.Controls.Add(this.labelDefenceFatesTitle);
            this.tabPageDefenceFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageDefenceFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageDefenceFates.Name = "tabPageDefenceFates";
            this.tabPageDefenceFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageDefenceFates.TabIndex = 3;
            this.tabPageDefenceFates.Text = "Defence FATEs";
            this.tabPageDefenceFates.UseVisualStyleBackColor = true;
            // 
            // labelDefenceFatesTitle
            // 
            this.labelDefenceFatesTitle.AutoSize = true;
            this.labelDefenceFatesTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefenceFatesTitle.Location = new System.Drawing.Point(10, 10);
            this.labelDefenceFatesTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelDefenceFatesTitle.Name = "labelDefenceFatesTitle";
            this.labelDefenceFatesTitle.Size = new System.Drawing.Size(185, 38);
            this.labelDefenceFatesTitle.TabIndex = 6;
            this.labelDefenceFatesTitle.Text = "Defence FATEs";
            // 
            // tabPageBossFates
            // 
            this.tabPageBossFates.Controls.Add(this.labelBossFatesTitle);
            this.tabPageBossFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageBossFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageBossFates.Name = "tabPageBossFates";
            this.tabPageBossFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageBossFates.TabIndex = 4;
            this.tabPageBossFates.Text = "Boss FATEs";
            this.tabPageBossFates.UseVisualStyleBackColor = true;
            // 
            // labelBossFatesTitle
            // 
            this.labelBossFatesTitle.AutoSize = true;
            this.labelBossFatesTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBossFatesTitle.Location = new System.Drawing.Point(10, 10);
            this.labelBossFatesTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelBossFatesTitle.Name = "labelBossFatesTitle";
            this.labelBossFatesTitle.Size = new System.Drawing.Size(150, 38);
            this.labelBossFatesTitle.TabIndex = 6;
            this.labelBossFatesTitle.Text = "Boss FATEs";
            // 
            // tabPageMegaBossFates
            // 
            this.tabPageMegaBossFates.Controls.Add(this.labelMegaBossFatesTitle);
            this.tabPageMegaBossFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageMegaBossFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMegaBossFates.Name = "tabPageMegaBossFates";
            this.tabPageMegaBossFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageMegaBossFates.TabIndex = 5;
            this.tabPageMegaBossFates.Text = "Mega-Boss FATEs";
            this.tabPageMegaBossFates.UseVisualStyleBackColor = true;
            // 
            // labelMegaBossFatesTitle
            // 
            this.labelMegaBossFatesTitle.AutoSize = true;
            this.labelMegaBossFatesTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMegaBossFatesTitle.Location = new System.Drawing.Point(10, 10);
            this.labelMegaBossFatesTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelMegaBossFatesTitle.Name = "labelMegaBossFatesTitle";
            this.labelMegaBossFatesTitle.Size = new System.Drawing.Size(222, 38);
            this.labelMegaBossFatesTitle.TabIndex = 6;
            this.labelMegaBossFatesTitle.Text = "Mega-Boss FATEs";
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
            // tabCustomBehaviour
            // 
            this.tabCustomBehaviour.Controls.Add(this.tabSelectorCustom);
            this.tabCustomBehaviour.Controls.Add(this.tabControllerCustom);
            this.tabCustomBehaviour.Location = new System.Drawing.Point(4, 29);
            this.tabCustomBehaviour.Margin = new System.Windows.Forms.Padding(0);
            this.tabCustomBehaviour.Name = "tabCustomBehaviour";
            this.tabCustomBehaviour.Padding = new System.Windows.Forms.Padding(3);
            this.tabCustomBehaviour.Size = new System.Drawing.Size(842, 488);
            this.tabCustomBehaviour.TabIndex = 2;
            this.tabCustomBehaviour.Text = "Custom Behaviour";
            this.tabCustomBehaviour.UseVisualStyleBackColor = true;
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
            this.tabControllerCustom.Controls.Add(this.tabPageAvoidance);
            this.tabControllerCustom.Controls.Add(this.tabPageNavigation);
            this.tabControllerCustom.Controls.Add(this.tabPageSpecialFates);
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
            // tabPageAvoidance
            // 
            this.tabPageAvoidance.Controls.Add(this.labelAvoidanceTitle);
            this.tabPageAvoidance.Location = new System.Drawing.Point(4, 29);
            this.tabPageAvoidance.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageAvoidance.Name = "tabPageAvoidance";
            this.tabPageAvoidance.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAvoidance.Size = new System.Drawing.Size(666, 416);
            this.tabPageAvoidance.TabIndex = 0;
            this.tabPageAvoidance.Text = "Avoidance";
            this.tabPageAvoidance.UseVisualStyleBackColor = true;
            // 
            // labelAvoidanceTitle
            // 
            this.labelAvoidanceTitle.AutoSize = true;
            this.labelAvoidanceTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAvoidanceTitle.Location = new System.Drawing.Point(10, 10);
            this.labelAvoidanceTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelAvoidanceTitle.Name = "labelAvoidanceTitle";
            this.labelAvoidanceTitle.Size = new System.Drawing.Size(134, 38);
            this.labelAvoidanceTitle.TabIndex = 1;
            this.labelAvoidanceTitle.Text = "Avoidance";
            // 
            // tabPageNavigation
            // 
            this.tabPageNavigation.Controls.Add(this.labelNavigationTitle);
            this.tabPageNavigation.Location = new System.Drawing.Point(4, 22);
            this.tabPageNavigation.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageNavigation.Name = "tabPageNavigation";
            this.tabPageNavigation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNavigation.Size = new System.Drawing.Size(666, 423);
            this.tabPageNavigation.TabIndex = 1;
            this.tabPageNavigation.Text = "Navigation";
            this.tabPageNavigation.UseVisualStyleBackColor = true;
            // 
            // labelNavigationTitle
            // 
            this.labelNavigationTitle.AutoSize = true;
            this.labelNavigationTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNavigationTitle.Location = new System.Drawing.Point(10, 10);
            this.labelNavigationTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelNavigationTitle.Name = "labelNavigationTitle";
            this.labelNavigationTitle.Size = new System.Drawing.Size(139, 38);
            this.labelNavigationTitle.TabIndex = 4;
            this.labelNavigationTitle.Text = "Navigation";
            // 
            // tabPageSpecialFates
            // 
            this.tabPageSpecialFates.Controls.Add(this.labelSpecialFatesTitle);
            this.tabPageSpecialFates.Location = new System.Drawing.Point(4, 22);
            this.tabPageSpecialFates.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageSpecialFates.Name = "tabPageSpecialFates";
            this.tabPageSpecialFates.Size = new System.Drawing.Size(666, 423);
            this.tabPageSpecialFates.TabIndex = 2;
            this.tabPageSpecialFates.Text = "Special FATEs";
            this.tabPageSpecialFates.UseVisualStyleBackColor = true;
            // 
            // labelSpecialFatesTitle
            // 
            this.labelSpecialFatesTitle.AutoSize = true;
            this.labelSpecialFatesTitle.Font = new System.Drawing.Font("Roboto Condensed", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSpecialFatesTitle.Location = new System.Drawing.Point(10, 10);
            this.labelSpecialFatesTitle.Margin = new System.Windows.Forms.Padding(3);
            this.labelSpecialFatesTitle.Name = "labelSpecialFatesTitle";
            this.labelSpecialFatesTitle.Size = new System.Drawing.Size(177, 38);
            this.labelSpecialFatesTitle.TabIndex = 5;
            this.labelSpecialFatesTitle.Text = "Special FATEs";
            // 
            // tabBlacklist
            // 
            this.tabBlacklist.Controls.Add(this.tabSelectorBlacklist);
            this.tabBlacklist.Controls.Add(this.tabControllerBlacklist);
            this.tabBlacklist.Location = new System.Drawing.Point(4, 29);
            this.tabBlacklist.Margin = new System.Windows.Forms.Padding(0);
            this.tabBlacklist.Name = "tabBlacklist";
            this.tabBlacklist.Padding = new System.Windows.Forms.Padding(3);
            this.tabBlacklist.Size = new System.Drawing.Size(842, 488);
            this.tabBlacklist.TabIndex = 3;
            this.tabBlacklist.Text = "Blacklist";
            this.tabBlacklist.UseVisualStyleBackColor = true;
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
            this.tabPageFateBlacklist.Controls.Add(this.labelFateBlacklistTitle);
            this.tabPageFateBlacklist.Location = new System.Drawing.Point(4, 29);
            this.tabPageFateBlacklist.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageFateBlacklist.Name = "tabPageFateBlacklist";
            this.tabPageFateBlacklist.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFateBlacklist.Size = new System.Drawing.Size(666, 416);
            this.tabPageFateBlacklist.TabIndex = 0;
            this.tabPageFateBlacklist.Text = "FATE Blacklist";
            this.tabPageFateBlacklist.UseVisualStyleBackColor = true;
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
            this.tabPageMobBlacklist.Controls.Add(this.labelMobBlacklistTitle);
            this.tabPageMobBlacklist.Location = new System.Drawing.Point(4, 22);
            this.tabPageMobBlacklist.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMobBlacklist.Name = "tabPageMobBlacklist";
            this.tabPageMobBlacklist.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMobBlacklist.Size = new System.Drawing.Size(666, 423);
            this.tabPageMobBlacklist.TabIndex = 1;
            this.tabPageMobBlacklist.Text = "Mob Blacklist";
            this.tabPageMobBlacklist.UseVisualStyleBackColor = true;
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
            this.tabAbout.Controls.Add(this.tabControllerAbout);
            this.tabAbout.Controls.Add(this.tabSelectorAbout);
            this.tabAbout.Location = new System.Drawing.Point(4, 29);
            this.tabAbout.Margin = new System.Windows.Forms.Padding(0);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabAbout.Size = new System.Drawing.Size(842, 488);
            this.tabAbout.TabIndex = 4;
            this.tabAbout.Text = "About";
            this.tabAbout.UseVisualStyleBackColor = true;
            // 
            // tabControllerAbout
            // 
            this.tabControllerAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControllerAbout.Controls.Add(this.tabPageLicense);
            this.tabControllerAbout.Controls.Add(this.tabPageDonate);
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
            this.tabPageLicense.UseVisualStyleBackColor = true;
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
            this.labelLicenseText.Size = new System.Drawing.Size(641, 263);
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
            this.tabPageDonate.UseVisualStyleBackColor = true;
            // 
            // pictureBoxDonate
            // 
            this.pictureBoxDonate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBoxDonate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxDonate.Image = global::Tarot.Properties.Resources.PayPal;
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
            this.labelTooltip.Location = new System.Drawing.Point(4, 5);
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
            this.panelControl.Location = new System.Drawing.Point(0, 541);
            this.panelControl.Margin = new System.Windows.Forms.Padding(0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(850, 44);
            this.panelControl.TabIndex = 5;
            this.panelControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindow);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonClose.Depth = 0;
            this.buttonClose.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonClose.Location = new System.Drawing.Point(746, 7);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.buttonClose.MouseState = MaterialSkin.MouseState.Hover;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Primary = true;
            this.buttonClose.Size = new System.Drawing.Size(100, 33);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.OnCloseButtonClick);
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLogo.Image = global::Tarot.Properties.Resources.Logo;
            this.pictureBoxLogo.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(173, 61);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            this.pictureBoxLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindow);
            // 
            // SettingsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(850, 585);
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
            this.Text = "Tarot";
            this.tabControllerMain.ResumeLayout(false);
            this.tabGeneralSettings.ResumeLayout(false);
            this.tabControllerGeneral.ResumeLayout(false);
            this.tabPageFateSelection.ResumeLayout(false);
            this.tabPageFateSelection.PerformLayout();
            this.tabPageBotMode.ResumeLayout(false);
            this.tabPageBotMode.PerformLayout();
            this.tabPageMovement.ResumeLayout(false);
            this.tabPageMovement.PerformLayout();
            this.tabPageScheduler.ResumeLayout(false);
            this.tabPageScheduler.PerformLayout();
            this.tabPagePatrol.ResumeLayout(false);
            this.tabPagePatrol.PerformLayout();
            this.tabPageMiscellaneous.ResumeLayout(false);
            this.tabPageMiscellaneous.PerformLayout();
            this.tabFateSettings.ResumeLayout(false);
            this.tabControllerFate.ResumeLayout(false);
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
            this.tabCustomBehaviour.ResumeLayout(false);
            this.tabControllerCustom.ResumeLayout(false);
            this.tabPageAvoidance.ResumeLayout(false);
            this.tabPageAvoidance.PerformLayout();
            this.tabPageNavigation.ResumeLayout(false);
            this.tabPageNavigation.PerformLayout();
            this.tabPageSpecialFates.ResumeLayout(false);
            this.tabPageSpecialFates.PerformLayout();
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
            this.panelControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabSelector tabSelectorMain;
        private MaterialSkin.Controls.MaterialTabControl tabControllerMain;
        private System.Windows.Forms.TabPage tabGeneralSettings;
        private System.Windows.Forms.TabPage tabFateSettings;
        private System.Windows.Forms.TabPage tabCustomBehaviour;
        private System.Windows.Forms.TabPage tabBlacklist;
        private System.Windows.Forms.Label labelTooltip;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
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
        private System.Windows.Forms.PictureBox pictureBoxDonate;
        private MaterialSkin.Controls.MaterialTabSelectorVertical tabSelectorCustom;
        private MaterialSkin.Controls.MaterialTabControl tabControllerCustom;
        private System.Windows.Forms.TabPage tabPageAvoidance;
        private System.Windows.Forms.Label labelAvoidanceTitle;
        private System.Windows.Forms.TabPage tabPageNavigation;
        private System.Windows.Forms.Label labelNavigationTitle;
        private System.Windows.Forms.TabPage tabPageSpecialFates;
        private System.Windows.Forms.Label labelSpecialFatesTitle;
        private MaterialSkin.Controls.MaterialTabSelectorVertical tabSelectorFate;
        private MaterialSkin.Controls.MaterialTabControl tabControllerFate;
        private System.Windows.Forms.TabPage tabPageKillFates;
        private System.Windows.Forms.Label labelKillFatesTitle;
        private System.Windows.Forms.TabPage tabPageCollectFates;
        private System.Windows.Forms.Label labelCollectFatesTitle;
        private System.Windows.Forms.TabPage tabPageEscortFates;
        private System.Windows.Forms.Label labelEscortFatesTitle;
        private System.Windows.Forms.TabPage tabPageDefenceFates;
        private System.Windows.Forms.TabPage tabPageBossFates;
        private System.Windows.Forms.TabPage tabPageMegaBossFates;
        private System.Windows.Forms.Label labelDefenceFatesTitle;
        private System.Windows.Forms.Label labelBossFatesTitle;
        private System.Windows.Forms.Label labelMegaBossFatesTitle;
        private MaterialSkin.Controls.MaterialFlatButton buttonClose;
        private MaterialSkin.Controls.MaterialTabControl tabControllerGeneral;
        private System.Windows.Forms.TabPage tabPageFateSelection;
        private System.Windows.Forms.Label labelFateSelectionTitle;
        private System.Windows.Forms.TabPage tabPageBotMode;
        private System.Windows.Forms.Label labelBotModeTitle;
        private MaterialSkin.Controls.MaterialTabSelectorVertical tabSelectorGeneral;
        private System.Windows.Forms.TabPage tabPageMovement;
        private System.Windows.Forms.TabPage tabPageScheduler;
        private System.Windows.Forms.TabPage tabPagePatrol;
        private System.Windows.Forms.Label labelMovementTitle;
        private System.Windows.Forms.Label labelSchedulerTitle;
        private System.Windows.Forms.Label labelPatrolTitle;
        private System.Windows.Forms.TabPage tabPageMiscellaneous;
        private System.Windows.Forms.Label labelMiscellaneousTitle;
    }
}