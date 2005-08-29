using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NAntGui.Core
{
	public class PersistWindowState
	{
		private const bool ALLOW_SAVE_MINIMIZED = false;
		private readonly RegistryKey _currentUser = Registry.CurrentUser;

		private NAntForm _nantForm;

		public static void Attach(NAntForm form)
		{
			new PersistWindowState(form);
		}

		private PersistWindowState(NAntForm form)
		{
			_nantForm = form;
			_nantForm.Load += new EventHandler(this.OnLoad);
		}

		private void OnLoad(object sender, EventArgs e)
		{
			RegistryKey key = _currentUser.CreateSubKey(Settings.WINDOW_KEY_PATH);

			if (KeyExists(key))
			{
				this.LoadStateFromReg(key);
			}
			else
			{
				this.LoadStateFromOldRegKey();
			}
		}

		private void LoadStateFromReg(RegistryKey key)
		{
			int	left	= Convert.ToInt32(key.GetValue("Left", _nantForm.Left));
			int	top		= Convert.ToInt32(key.GetValue("Top", _nantForm.Top));
			int	width	= Convert.ToInt32(key.GetValue("Width", _nantForm.Width));
			int	height	= Convert.ToInt32(key.GetValue("Height", _nantForm.Height));

			int	horzSplitter = (int)key.GetValue("HorzSplitter", _nantForm.BuildTreeView.Height);
			int	vertSplitter = (int)key.GetValue("VertSplitter", _nantForm.LeftPanel.Width);

			FormWindowState	windowState	= (FormWindowState)key.GetValue("WindowState", (int)_nantForm.WindowState);
			PropertySort propertySort	= (PropertySort)key.GetValue("PropertySort", (int)_nantForm.ProjectPropertyGrid.PropertySort);
			
			_nantForm.Location							= new Point(left, top);
			_nantForm.Size								= new Size(width, height);
			_nantForm.LeftPanel.Width					= vertSplitter;
			_nantForm.BuildTreeView.Height				= horzSplitter;
			_nantForm.WindowState						= windowState;
			_nantForm.ProjectPropertyGrid.PropertySort	= propertySort;

			_nantForm.Closing				+= new CancelEventHandler(this.OnClosing);
		}

		private void LoadStateFromOldRegKey()
		{
			RegistryKey key = _currentUser.OpenSubKey(Settings.OLD_KEY_PATH);

			if (KeyExists(key))
			{
				this.LoadStateFromReg(key);
			}
		}

		private static bool KeyExists(RegistryKey key)
		{
			return key != null;
		}

		private void OnClosing(object sender, CancelEventArgs e)
		{
			// save position, size and state
			RegistryKey lKey = _currentUser.CreateSubKey(Settings.WINDOW_KEY_PATH);

			lKey.SetValue("Left",			_nantForm.Left);
			lKey.SetValue("Top",			_nantForm.Top);
			lKey.SetValue("Width",			_nantForm.Width);
			lKey.SetValue("Height",			_nantForm.Height);
			lKey.SetValue("VertSplitter",	_nantForm.LeftPanel.Width);
			lKey.SetValue("HorzSplitter",	_nantForm.BuildTreeView.Height);
			lKey.SetValue("WindowState",	this.AdjustWindowState());
			lKey.SetValue("PropertySort",	(int)_nantForm.ProjectPropertyGrid.PropertySort);
		}

		private int AdjustWindowState()
		{
			// check if we are allowed to save the state as minimized (not normally)
			if (_nantForm.WindowState == FormWindowState.Minimized && !ALLOW_SAVE_MINIMIZED)
			{
				return (int)FormWindowState.Normal;
			}
			else
			{
				return (int)_nantForm.WindowState;
			}
		}
	}
}