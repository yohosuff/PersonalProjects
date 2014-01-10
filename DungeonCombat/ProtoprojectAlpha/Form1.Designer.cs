using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace DungeonCombat
{
    partial class MainForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.canvas = new OpenTK.GLControl();
            this.exitButton = new System.Windows.Forms.Button();
            this.combatantNameLabel = new System.Windows.Forms.Label();
            this.MoveButton = new System.Windows.Forms.Button();
            this.AttackButton = new System.Windows.Forms.Button();
            this.EndTurnButton = new System.Windows.Forms.Button();
            this.combatantStatisticsLabel = new System.Windows.Forms.Label();
            this.combatantStateLabel = new System.Windows.Forms.Label();
            this.combatantWeaponLabel = new System.Windows.Forms.Label();
            this.WeaponsTab = new System.Windows.Forms.TabPage();
            this.EquipWeaponButton = new System.Windows.Forms.Button();
            this.combatantWeaponListBox = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.WeaponsTab.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.Black;
            this.canvas.Location = new System.Drawing.Point(12, 13);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(576, 655);
            this.canvas.TabIndex = 0;
            this.canvas.VSync = false;
            this.canvas.Load += new System.EventHandler(this.canvas_Load);
            this.canvas.Click += new System.EventHandler(this.canvas_Click);
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            this.canvas.DoubleClick += new System.EventHandler(this.canvas_DoubleClick);
            this.canvas.KeyDown += new System.Windows.Forms.KeyEventHandler(this.canvas_KeyDown);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseMove);
            this.canvas.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.canvas_PreviewKeyDown);
            this.canvas.Resize += new System.EventHandler(this.canvas_Resize);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(839, 81);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 10;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // combatantNameLabel
            // 
            this.combatantNameLabel.AutoSize = true;
            this.combatantNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combatantNameLabel.Location = new System.Drawing.Point(594, 13);
            this.combatantNameLabel.Name = "combatantNameLabel";
            this.combatantNameLabel.Size = new System.Drawing.Size(226, 26);
            this.combatantNameLabel.TabIndex = 12;
            this.combatantNameLabel.Text = "combatantNameLabel";
            // 
            // MoveButton
            // 
            this.MoveButton.Location = new System.Drawing.Point(599, 81);
            this.MoveButton.Name = "MoveButton";
            this.MoveButton.Size = new System.Drawing.Size(75, 23);
            this.MoveButton.TabIndex = 13;
            this.MoveButton.Text = "Move";
            this.MoveButton.UseVisualStyleBackColor = true;
            this.MoveButton.Click += new System.EventHandler(this.MoveButton_Click);
            // 
            // AttackButton
            // 
            this.AttackButton.Location = new System.Drawing.Point(679, 81);
            this.AttackButton.Name = "AttackButton";
            this.AttackButton.Size = new System.Drawing.Size(75, 23);
            this.AttackButton.TabIndex = 14;
            this.AttackButton.Text = "Attack";
            this.AttackButton.UseVisualStyleBackColor = true;
            this.AttackButton.Click += new System.EventHandler(this.AttackButton_Click);
            // 
            // EndTurnButton
            // 
            this.EndTurnButton.Location = new System.Drawing.Point(760, 81);
            this.EndTurnButton.Name = "EndTurnButton";
            this.EndTurnButton.Size = new System.Drawing.Size(75, 23);
            this.EndTurnButton.TabIndex = 15;
            this.EndTurnButton.Text = "End Turn";
            this.EndTurnButton.UseVisualStyleBackColor = true;
            this.EndTurnButton.Click += new System.EventHandler(this.EndTurnButton_Click);
            // 
            // combatantStatisticsLabel
            // 
            this.combatantStatisticsLabel.AutoSize = true;
            this.combatantStatisticsLabel.Location = new System.Drawing.Point(596, 52);
            this.combatantStatisticsLabel.Name = "combatantStatisticsLabel";
            this.combatantStatisticsLabel.Size = new System.Drawing.Size(135, 13);
            this.combatantStatisticsLabel.TabIndex = 16;
            this.combatantStatisticsLabel.Text = "currentCombatantInfoLabel";
            // 
            // combatantStateLabel
            // 
            this.combatantStateLabel.AutoSize = true;
            this.combatantStateLabel.Location = new System.Drawing.Point(596, 39);
            this.combatantStateLabel.Name = "combatantStateLabel";
            this.combatantStateLabel.Size = new System.Drawing.Size(108, 13);
            this.combatantStateLabel.TabIndex = 17;
            this.combatantStateLabel.Text = "combatantStateLabel";
            // 
            // combatantWeaponLabel
            // 
            this.combatantWeaponLabel.AutoSize = true;
            this.combatantWeaponLabel.Location = new System.Drawing.Point(596, 65);
            this.combatantWeaponLabel.Name = "combatantWeaponLabel";
            this.combatantWeaponLabel.Size = new System.Drawing.Size(158, 13);
            this.combatantWeaponLabel.TabIndex = 18;
            this.combatantWeaponLabel.Text = "currentCombatantWeaponLabel";
            // 
            // WeaponsTab
            // 
            this.WeaponsTab.Controls.Add(this.EquipWeaponButton);
            this.WeaponsTab.Controls.Add(this.combatantWeaponListBox);
            this.WeaponsTab.Location = new System.Drawing.Point(4, 22);
            this.WeaponsTab.Name = "WeaponsTab";
            this.WeaponsTab.Padding = new System.Windows.Forms.Padding(3);
            this.WeaponsTab.Size = new System.Drawing.Size(311, 452);
            this.WeaponsTab.TabIndex = 1;
            this.WeaponsTab.Text = "Weapons";
            this.WeaponsTab.UseVisualStyleBackColor = true;
            // 
            // EquipWeaponButton
            // 
            this.EquipWeaponButton.Location = new System.Drawing.Point(230, 7);
            this.EquipWeaponButton.Name = "EquipWeaponButton";
            this.EquipWeaponButton.Size = new System.Drawing.Size(75, 23);
            this.EquipWeaponButton.TabIndex = 1;
            this.EquipWeaponButton.Text = "Equip";
            this.EquipWeaponButton.UseVisualStyleBackColor = true;
            this.EquipWeaponButton.Click += new System.EventHandler(this.EquipWeaponButton_Click);
            // 
            // combatantWeaponListBox
            // 
            this.combatantWeaponListBox.FormattingEnabled = true;
            this.combatantWeaponListBox.Location = new System.Drawing.Point(7, 7);
            this.combatantWeaponListBox.Name = "combatantWeaponListBox";
            this.combatantWeaponListBox.Size = new System.Drawing.Size(217, 95);
            this.combatantWeaponListBox.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.WeaponsTab);
            this.tabControl1.Location = new System.Drawing.Point(599, 110);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(319, 478);
            this.tabControl1.TabIndex = 20;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 669);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.combatantWeaponLabel);
            this.Controls.Add(this.combatantStateLabel);
            this.Controls.Add(this.combatantStatisticsLabel);
            this.Controls.Add(this.EndTurnButton);
            this.Controls.Add(this.AttackButton);
            this.Controls.Add(this.MoveButton);
            this.Controls.Add(this.combatantNameLabel);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.canvas);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.WeaponsTab.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl canvas;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label combatantNameLabel;

        List<System.Windows.Forms.Button> movementButtons = new List<System.Windows.Forms.Button>();
        private System.Windows.Forms.Button MoveButton;
        private System.Windows.Forms.Button AttackButton;
        private System.Windows.Forms.Button EndTurnButton;
        private System.Windows.Forms.Label combatantStatisticsLabel;
        private System.Windows.Forms.Label combatantStateLabel;
        private System.Windows.Forms.Label combatantWeaponLabel;
        private System.Windows.Forms.TabPage WeaponsTab;
        private System.Windows.Forms.Button EquipWeaponButton;
        private System.Windows.Forms.ListBox combatantWeaponListBox;
        private System.Windows.Forms.TabControl tabControl1;
        
    }
}

