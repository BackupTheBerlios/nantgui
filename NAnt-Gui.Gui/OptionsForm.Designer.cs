#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2007 Colin Svingen
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General internal License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General internal License for more details.
//
// You should have received a copy of the GNU General internal License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Colin Svingen (swoogan@gmail.com)

#endregion

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace NAntGui.Gui
{
	/// <summary>
	/// Description of OptionsForm
	/// </summary>
	internal partial class OptionsForm : Form
	{
		private Button OKButton;
		private Button CloseButton;
		private Label label1;
		private NumericUpDown _maxRecentItemsUpDown;
		private CheckBox _hideTargetsCheckBox;
		private IContainer components = null;		
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this._hideTargetsCheckBox = new System.Windows.Forms.CheckBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this._maxRecentItemsUpDown = new System.Windows.Forms.NumericUpDown();
            this._openPreviousCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this._maxRecentItemsUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // HideTargetsCheckBox
            // 
            this._hideTargetsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._hideTargetsCheckBox.Location = new System.Drawing.Point(8, 8);
            this._hideTargetsCheckBox.Name = "HideTargetsCheckBox";
            this._hideTargetsCheckBox.Size = new System.Drawing.Size(351, 22);
            this._hideTargetsCheckBox.TabIndex = 1;
            this._hideTargetsCheckBox.Text = "Hide targets that do not contain a description?";
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(209, 91);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 4;
            this.OKButton.Text = "&OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(289, 91);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 5;
            this.CloseButton.Text = "&Cancel";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(272, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Maximum number of items in the Recent Items menu:";
            // 
            // MaxRecentItemsUpDown
            // 
            this._maxRecentItemsUpDown.Location = new System.Drawing.Point(319, 34);
            this._maxRecentItemsUpDown.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this._maxRecentItemsUpDown.Name = "MaxRecentItemsUpDown";
            this._maxRecentItemsUpDown.Size = new System.Drawing.Size(40, 20);
            this._maxRecentItemsUpDown.TabIndex = 2;
            // 
            // _openPreviousCheckBox
            // 
            this._openPreviousCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._openPreviousCheckBox.Location = new System.Drawing.Point(8, 60);
            this._openPreviousCheckBox.Name = "_openPreviousCheckBox";
            this._openPreviousCheckBox.Size = new System.Drawing.Size(351, 24);
            this._openPreviousCheckBox.TabIndex = 3;
            this._openPreviousCheckBox.Text = "Open previously opened documents when application starts";
            this._openPreviousCheckBox.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(371, 122);
            this.Controls.Add(this._openPreviousCheckBox);
            this.Controls.Add(this._maxRecentItemsUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this._hideTargetsCheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this._maxRecentItemsUpDown)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion		

        private CheckBox _openPreviousCheckBox;

    }
}
