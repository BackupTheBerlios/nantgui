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
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NAntGui.Gui
{
	/// <summary>
	/// Summary description for NewProjectForm.
	/// </summary>
	internal partial class NewProjectForm
	{
		internal event EventHandler<NewProjectEventArgs> NewClicked;

		internal NewProjectForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void OKButton_Click(object sender, EventArgs e)
		{
			if (NameEntered)
			{
                ProjectInfo project = new ProjectInfo();
                project.Name = NameTextBox.Text;
                project.Default = DefaultTextBox.Text;
                project.Basedir = BasedirTextBox.Text;
                project.Description = DescriptionTextBox.Text;

                OnNewProject(project);

                //XmlSerializer serial = new XmlSerializer(typeof(project));
                //MemoryStream stream = new MemoryStream();

                //serial.Serialize(stream, proj);

                //byte[] buffer = new byte[stream.Length];

                //stream.Read(buffer, 0, (int)stream.Length - 1);

                //string xml = Encoding.Unicode.GetString(buffer);

                //MessageBox.Show(xml);
                Close();
			}
			else
			{
				MessageBox.Show("A name is required.", "Missing Name");
			}
		}

		private bool NameEntered
		{
			get { return NameTextBox.Text.Length != 0; }
		}

        private void OnNewProject(ProjectInfo info)
        {
            if (NewClicked != null)
                NewClicked(this, new NewProjectEventArgs(info));
        }
	}
}