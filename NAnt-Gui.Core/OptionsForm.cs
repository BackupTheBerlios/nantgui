#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2005 Colin Svingen
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Colin Svingen (nantgui@swoogan.com)

#endregion

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Options.
	/// </summary>
	public class OptionsForm : Form
	{
		private Button OKButton;
		private Button CloseButton;
		private Label label1;
		private NumericUpDown MaxRecentItemsUpDown;
		private CheckBox HideTargetsCheckBox;
		private IContainer components = null;

		public OptionsForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(OptionsForm));
			this.HideTargetsCheckBox = new System.Windows.Forms.CheckBox();
			this.OKButton = new System.Windows.Forms.Button();
			this.CloseButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.MaxRecentItemsUpDown = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.MaxRecentItemsUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// HideTargetsCheckBox
			// 
			this.HideTargetsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.HideTargetsCheckBox.Location = new System.Drawing.Point(16, 16);
			this.HideTargetsCheckBox.Name = "HideTargetsCheckBox";
			this.HideTargetsCheckBox.Size = new System.Drawing.Size(320, 24);
			this.HideTargetsCheckBox.TabIndex = 1;
			this.HideTargetsCheckBox.Text = "Hide targets that do not contain a description?";
			// 
			// OKButton
			// 
			this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKButton.Location = new System.Drawing.Point(192, 88);
			this.OKButton.Name = "OKButton";
			this.OKButton.TabIndex = 2;
			this.OKButton.Text = "&OK";
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// CloseButton
			// 
			this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CloseButton.Location = new System.Drawing.Point(272, 88);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.TabIndex = 3;
			this.CloseButton.Text = "&Cancel";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(272, 24);
			this.label1.TabIndex = 4;
			this.label1.Text = "Maximum number of items in the Recent Items menu:";
			// 
			// MaxRecentItemsUpDown
			// 
			this.MaxRecentItemsUpDown.Location = new System.Drawing.Point(296, 48);
			this.MaxRecentItemsUpDown.Maximum = new System.Decimal(new int[] {
																				 15,
																				 0,
																				 0,
																				 0});
			this.MaxRecentItemsUpDown.Name = "MaxRecentItemsUpDown";
			this.MaxRecentItemsUpDown.Size = new System.Drawing.Size(40, 20);
			this.MaxRecentItemsUpDown.TabIndex = 5;
			// 
			// OptionsForm
			// 
			this.AcceptButton = this.OKButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.CloseButton;
			this.ClientSize = new System.Drawing.Size(354, 119);
			this.Controls.Add(this.MaxRecentItemsUpDown);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.HideTargetsCheckBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(360, 160);
			this.Name = "OptionsForm";
			this.Text = "Options";
			this.Load += new System.EventHandler(this.OptionsForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.MaxRecentItemsUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private void OptionsForm_Load(object sender, EventArgs e)
		{
			this.HideTargetsCheckBox.Checked = Settings.HideTargetsWithoutDescription;
			this.MaxRecentItemsUpDown.Value = Settings.MaxRecentItems;
		}

		private void OKButton_Click(object sender, EventArgs e)
		{
			Settings.HideTargetsWithoutDescription = this.HideTargetsCheckBox.Checked;
			Settings.MaxRecentItems = (int) this.MaxRecentItemsUpDown.Value;
		}

	}
}