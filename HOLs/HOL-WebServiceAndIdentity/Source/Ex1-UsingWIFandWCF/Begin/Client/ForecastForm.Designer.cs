// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Client
{
    public partial class ForecastForm
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
            this.forecastPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.getThreeDaysButton = new System.Windows.Forms.Button();
            this.getTenDaysButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.zipCodeTextBox = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.sourceLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // forecastPanel
            // 
            this.forecastPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.forecastPanel.AutoScroll = true;
            this.forecastPanel.Location = new System.Drawing.Point(0, 41);
            this.forecastPanel.Name = "forecastPanel";
            this.forecastPanel.Size = new System.Drawing.Size(497, 203);
            this.forecastPanel.TabIndex = 0;
            // 
            // getThreeDaysButton
            // 
            this.getThreeDaysButton.Location = new System.Drawing.Point(178, 11);
            this.getThreeDaysButton.Name = "getThreeDaysButton";
            this.getThreeDaysButton.Size = new System.Drawing.Size(90, 23);
            this.getThreeDaysButton.TabIndex = 1;
            this.getThreeDaysButton.Text = "Get 3 days forecast";
            this.getThreeDaysButton.UseVisualStyleBackColor = true;
            this.getThreeDaysButton.Click += new System.EventHandler(this.GetThreeDaysButton_Click);
            // 
            // getTenDaysButton
            // 
            this.getTenDaysButton.Location = new System.Drawing.Point(274, 11);
            this.getTenDaysButton.Name = "getTenDaysButton";
            this.getTenDaysButton.Size = new System.Drawing.Size(90, 23);
            this.getTenDaysButton.TabIndex = 2;
            this.getTenDaysButton.Text = "Get 10 days forecast";
            this.getTenDaysButton.UseVisualStyleBackColor = true;
            this.getTenDaysButton.Click += new System.EventHandler(this.GetTenDaysButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "ZIP code:";
            // 
            // zipCodeTextBox
            // 
            this.zipCodeTextBox.Location = new System.Drawing.Point(72, 13);
            this.zipCodeTextBox.Name = "zipCodeTextBox";
            this.zipCodeTextBox.Size = new System.Drawing.Size(100, 20);
            this.zipCodeTextBox.TabIndex = 0;
            this.zipCodeTextBox.Text = "0";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 222);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(497, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // sourceLabel
            // 
            this.sourceLabel.Name = "sourceLabel";
            this.sourceLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // ForecastForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 244);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.zipCodeTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.getTenDaysButton);
            this.Controls.Add(this.getThreeDaysButton);
            this.Controls.Add(this.forecastPanel);
            this.Name = "ForecastForm";
            this.Text = "Weather Station client";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel forecastPanel;
        private System.Windows.Forms.Button getThreeDaysButton;
        private System.Windows.Forms.Button getTenDaysButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox zipCodeTextBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel sourceLabel;
    }
}

