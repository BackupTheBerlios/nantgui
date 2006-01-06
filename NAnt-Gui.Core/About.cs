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
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for About.
	/// </summary>
	public class About : Form
	{
		private Label label1;
		private Label label2;
		private Label label3;
		private Label label4;
		private Button OKButton;
		private Label VersionLabel;
		private Label label5;
		private LinkLabel EmailLinkLabel;
		private LinkLabel WebLinkLabel;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public About()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(About));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.OKButton = new System.Windows.Forms.Button();
			this.VersionLabel = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.EmailLinkLabel = new System.Windows.Forms.LinkLabel();
			this.WebLinkLabel = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(216, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "NAnt-Gui: It\'s like NAnt, but with a GUI.";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(240, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.TabIndex = 1;
			this.label2.Text = "Version:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(240, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "PropertyBag Copyright © 2002  Tony Allowatt";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(256, 16);
			this.label4.TabIndex = 3;
			this.label4.Text = "NAnt 0.8.5 Copyright © 2001-2004 Gerry Shaw";
			// 
			// OKButton
			// 
			this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKButton.Location = new System.Drawing.Point(264, 137);
			this.OKButton.Name = "OKButton";
			this.OKButton.TabIndex = 4;
			this.OKButton.Text = "&OK";
			// 
			// VersionLabel
			// 
			this.VersionLabel.Location = new System.Drawing.Point(240, 24);
			this.VersionLabel.Name = "VersionLabel";
			this.VersionLabel.Size = new System.Drawing.Size(88, 16);
			this.VersionLabel.TabIndex = 5;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(8, 40);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(320, 16);
			this.label5.TabIndex = 6;
			this.label5.Text = "Copyright © 2005 Colin Svingen";
			// 
			// EmailLinkLabel
			// 
			this.EmailLinkLabel.Location = new System.Drawing.Point(200, 40);
			this.EmailLinkLabel.Name = "EmailLinkLabel";
			this.EmailLinkLabel.Size = new System.Drawing.Size(128, 16);
			this.EmailLinkLabel.TabIndex = 7;
			this.EmailLinkLabel.TabStop = true;
			this.EmailLinkLabel.Text = "nantgui@swoogan.com";
			this.EmailLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// WebLinkLabel
			// 
			this.WebLinkLabel.Location = new System.Drawing.Point(8, 64);
			this.WebLinkLabel.Name = "WebLinkLabel";
			this.WebLinkLabel.Size = new System.Drawing.Size(200, 16);
			this.WebLinkLabel.TabIndex = 8;
			this.WebLinkLabel.TabStop = true;
			this.WebLinkLabel.Text = "http://www.swoogan.com/nantgui.html";
			this.WebLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WebLinkLabel_LinkClicked);
			// 
			// About
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(346, 168);
			this.Controls.Add(this.WebLinkLabel);
			this.Controls.Add(this.EmailLinkLabel);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.VersionLabel);
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "About";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About NAnt-Gui";
			this.Load += new System.EventHandler(this.About_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private void About_Load(object sender, EventArgs e)
		{
			this.VersionLabel.Text = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("mailto://nantgui@swoogan.com");
		}

		private void WebLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://www.swoogan.com/nantgui.html");
		}
	}
}