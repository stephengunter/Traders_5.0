﻿
namespace OrderMakerWinApp.Test
{
    partial class BasicTestForm
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
            this.tpTop = new System.Windows.Forms.TableLayoutPanel();
            this.btnEditConfig = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLogs = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.tpTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpTop
            // 
            this.tpTop.ColumnCount = 4;
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.tpTop.Controls.Add(this.btnEditConfig, 1, 0);
            this.tpTop.Controls.Add(this.btnLogs, 3, 0);
            this.tpTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.tpTop.Location = new System.Drawing.Point(0, 0);
            this.tpTop.Name = "tpTop";
            this.tpTop.Padding = new System.Windows.Forms.Padding(10, 10, 0, 0);
            this.tpTop.RowCount = 1;
            this.tpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tpTop.Size = new System.Drawing.Size(800, 45);
            this.tpTop.TabIndex = 1;
            // 
            // btnEditConfig
            // 
            this.btnEditConfig.Location = new System.Drawing.Point(210, 13);
            this.btnEditConfig.Name = "btnEditConfig";
            this.btnEditConfig.Size = new System.Drawing.Size(75, 23);
            this.btnEditConfig.TabIndex = 2;
            this.btnEditConfig.Text = "設定";
            this.btnEditConfig.Click += new System.EventHandler(this.OnEditConfig);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Location = new System.Drawing.Point(0, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 100);
            this.panel1.TabIndex = 0;
            // 
            // btnLogs
            // 
            this.btnLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogs.Location = new System.Drawing.Point(722, 13);
            this.btnLogs.Name = "btnLogs";
            this.btnLogs.Size = new System.Drawing.Size(75, 23);
            this.btnLogs.TabIndex = 0;
            this.btnLogs.Text = "查看Log";
            this.btnLogs.UseVisualStyleBackColor = true;
            this.btnLogs.Click += new System.EventHandler(this.btnLogs_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(0, 155);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 23);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "登出";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // 
            // BasicTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.tpTop);
            this.Controls.Add(this.panel1);
            this.Name = "BasicTestForm";
            this.Text = "Trader 2.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BasicTestForm_FormClosing);
            this.Load += new System.EventHandler(this.BasicTestForm_Load);
            this.tpTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tpTop;
        private System.Windows.Forms.Panel panel1;

        private OrderMakerWinApp.UI.EditConfig editConfig;
        private System.Windows.Forms.Button btnEditConfig;
        private System.Windows.Forms.Button btnLogs;
        private System.Windows.Forms.Button btnLogout;
    }
}