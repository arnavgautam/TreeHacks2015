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

namespace FictionalRetail.Crm.Client
{
    partial class Main
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
            this.customersListView = new System.Windows.Forms.ListView();
            this.id = new System.Windows.Forms.ColumnHeader();
            this.name = new System.Windows.Forms.ColumnHeader();
            this.city = new System.Windows.Forms.ColumnHeader();
            this.bankingEntity = new System.Windows.Forms.ColumnHeader();
            this.moveToBankingEntityButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // customersListView
            // 
            this.customersListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customersListView.CheckBoxes = true;
            this.customersListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.name,
            this.city,
            this.bankingEntity,
            this.id});
            this.customersListView.Location = new System.Drawing.Point(13, 12);
            this.customersListView.Name = "customersListView";
            this.customersListView.Size = new System.Drawing.Size(304, 166);
            this.customersListView.TabIndex = 2;
            this.customersListView.UseCompatibleStateImageBehavior = false;
            this.customersListView.View = System.Windows.Forms.View.Details;
            // 
            // id
            // 
            this.id.Text = "Id";
            this.id.Width = 0;
            // 
            // name
            // 
            this.name.Text = "Name";
            this.name.Width = 85;
            // 
            // city
            // 
            this.city.Text = "City";
            this.city.Width = 70;
            // 
            // bankingEntity
            // 
            this.bankingEntity.Text = "Banking Entity";
            this.bankingEntity.Width = 110;
            // 
            // moveToBankingEntityButton
            // 
            this.moveToBankingEntityButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.moveToBankingEntityButton.Location = new System.Drawing.Point(147, 189);
            this.moveToBankingEntityButton.Name = "moveToBankingEntityButton";
            this.moveToBankingEntityButton.Size = new System.Drawing.Size(170, 23);
            this.moveToBankingEntityButton.TabIndex = 3;
            this.moveToBankingEntityButton.Text = "Move to Fictional Retail Bank";
            this.moveToBankingEntityButton.UseVisualStyleBackColor = true;
            this.moveToBankingEntityButton.Click += new System.EventHandler(this.MoveToBankingEntityButton_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 221);
            this.Controls.Add(this.moveToBankingEntityButton);
            this.Controls.Add(this.customersListView);
            this.Name = "Main";
            this.Text = "Fictional Retail CRM Client";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView customersListView;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader city;
        private System.Windows.Forms.ColumnHeader bankingEntity;
        private System.Windows.Forms.Button moveToBankingEntityButton;
    }
}

