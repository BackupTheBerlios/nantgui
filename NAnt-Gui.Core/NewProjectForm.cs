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
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using NAntGui.NAnt.NAnt;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for NewProjectForm.
	/// </summary>
	public class NewProjectForm : Form
	{
		private Label label1;
		private Label label2;
		private Label label3;
		private Button OKButton;
		private Button CloseButton;
		private Label label4;
		private TextBox NameTextBox;
		private TextBox DefaultTextBox;
		private TextBox DescriptionTextBox;
		private TextBox BasedirTextBox;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public NewProjectForm()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NewProjectForm));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.NameTextBox = new System.Windows.Forms.TextBox();
			this.DefaultTextBox = new System.Windows.Forms.TextBox();
			this.BasedirTextBox = new System.Windows.Forms.TextBox();
			this.DescriptionTextBox = new System.Windows.Forms.TextBox();
			this.OKButton = new System.Windows.Forms.Button();
			this.CloseButton = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(136, 8);
			this.label2.Name = "label2";
			this.label2.TabIndex = 1;
			this.label2.Text = "Default Target";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(264, 8);
			this.label3.Name = "label3";
			this.label3.TabIndex = 2;
			this.label3.Text = "Basedir";
			// 
			// NameTextBox
			// 
			this.NameTextBox.Location = new System.Drawing.Point(8, 24);
			this.NameTextBox.Name = "NameTextBox";
			this.NameTextBox.Size = new System.Drawing.Size(120, 20);
			this.NameTextBox.TabIndex = 1;
			this.NameTextBox.Text = "";
			// 
			// DefaultTextBox
			// 
			this.DefaultTextBox.Location = new System.Drawing.Point(136, 24);
			this.DefaultTextBox.Name = "DefaultTextBox";
			this.DefaultTextBox.Size = new System.Drawing.Size(120, 20);
			this.DefaultTextBox.TabIndex = 5;
			this.DefaultTextBox.Text = "";
			// 
			// BasedirTextBox
			// 
			this.BasedirTextBox.Location = new System.Drawing.Point(264, 24);
			this.BasedirTextBox.Name = "BasedirTextBox";
			this.BasedirTextBox.Size = new System.Drawing.Size(120, 20);
			this.BasedirTextBox.TabIndex = 10;
			this.BasedirTextBox.Text = "";
			// 
			// DescriptionTextBox
			// 
			this.DescriptionTextBox.Location = new System.Drawing.Point(8, 72);
			this.DescriptionTextBox.Multiline = true;
			this.DescriptionTextBox.Name = "DescriptionTextBox";
			this.DescriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.DescriptionTextBox.Size = new System.Drawing.Size(376, 88);
			this.DescriptionTextBox.TabIndex = 15;
			this.DescriptionTextBox.Text = "";
			// 
			// OKButton
			// 
			this.OKButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.OKButton.Location = new System.Drawing.Point(232, 168);
			this.OKButton.Name = "OKButton";
			this.OKButton.TabIndex = 20;
			this.OKButton.Text = "&OK";
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// CloseButton
			// 
			this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.CloseButton.Location = new System.Drawing.Point(312, 168);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.TabIndex = 25;
			this.CloseButton.Text = "&Cancel";
			this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 56);
			this.label4.Name = "label4";
			this.label4.TabIndex = 9;
			this.label4.Text = "Description";
			// 
			// NewProjectForm
			// 
			this.AcceptButton = this.OKButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.CloseButton;
			this.ClientSize = new System.Drawing.Size(394, 199);
			this.Controls.Add(this.DescriptionTextBox);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.BasedirTextBox);
			this.Controls.Add(this.DefaultTextBox);
			this.Controls.Add(this.NameTextBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(400, 224);
			this.Name = "NewProjectForm";
			this.Text = "New Project";
			this.ResumeLayout(false);

		}

		#endregion

		private void CloseButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void OKButton_Click(object sender, EventArgs e)
		{
			if (this.NameEntered)
			{
				project proj = new project();
				proj.name = this.NameTextBox.Text;
				proj.@default = this.DefaultTextBox.Text;
				proj.basedir = this.BasedirTextBox.Text;

				XmlSerializer serial = new XmlSerializer(typeof (project));
				MemoryStream stream = new MemoryStream();

				serial.Serialize(stream, proj);

				byte[] buffer = new byte[stream.Length];

				stream.Read(buffer, 0, (int) stream.Length - 1);

				string xml = Encoding.Unicode.GetString(buffer);

				MessageBox.Show(xml);
			}
			else
			{
				MessageBox.Show("A name is required.", "Missing Name");
			}
		}

		private bool NameEntered
		{
			get { return this.NameTextBox.Text.Length == 0; }
		}
	}
}