//=======================================================================================
//
//  This source code is only intended as a supplement to existing Microsoft documentation. 
//
//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE.
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//=======================================================================================
namespace Figlut.Server.Toolkit.Mmc.Forms
{
    /// <summary>
    /// Form container for a ListView control 
    /// </summary>
    partial class SettingsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstvSettings = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lstvSettings
            // 
            this.lstvSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstvSettings.FullRowSelect = true;
            this.lstvSettings.GridLines = true;
            this.lstvSettings.Location = new System.Drawing.Point(15, 17);
            this.lstvSettings.Name = "lstvSettings";
            this.lstvSettings.Size = new System.Drawing.Size(121, 115);
            this.lstvSettings.TabIndex = 0;
            this.lstvSettings.UseCompatibleStateImageBehavior = false;
            this.lstvSettings.View = System.Windows.Forms.View.Details;
            this.lstvSettings.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstvSettings_MouseClick);
            this.lstvSettings.SelectedIndexChanged += new System.EventHandler(this.lstvSettings_SelectedIndexChanged);
            // 
            // SelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstvSettings);
            this.Name = "SelectionControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstvSettings;      
    }
}
