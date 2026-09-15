using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using CustomControls.Controls;

namespace OpenDialogs
{
	public partial class DummyForm : Form
	{
		public DummyForm(OpenFileDialogEx fileDialogEx, string lable_OpenFloder, string lable_SelectTexg, string lable_CancelText)
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			if (currentCulture.ToString().Equals("zh-CN"))
			{
				this.Lable_OpenFloder = lable_OpenFloder;
				this.Lable_SelectTexg = lable_SelectTexg;
				this.Lable_CancelText = lable_CancelText;
			}
			else
			{
				this.Lable_OpenFloder = "Folder:";
				this.Lable_SelectTexg = "Select";
				this.Lable_CancelText = "Cancel";
			}
			this.mFileDialogEx = fileDialogEx;
			this.Text = "";
			base.StartPosition = FormStartPosition.Manual;
			base.Location = new Point(-32000, -32000);
			base.ShowInTaskbar = false;
		}

		public bool WatchForActivate
		{
			get
			{
				return this.mWatchForActivate;
			}
			set
			{
				this.mWatchForActivate = value;
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (this.mNativeDialog != null)
			{
				this.mNativeDialog.Dispose();
			}
			base.OnClosing(e);
		}

		protected override void WndProc(ref Message m)
		{
			if (this.mWatchForActivate && m.Msg == 6)
			{
				this.mWatchForActivate = false;
				this.mOpenDialogHandle = m.LParam;
				this.mNativeDialog = new OpenDialogNative(m.LParam, this.mFileDialogEx, base.Handle, this.Lable_OpenFloder, this.Lable_SelectTexg, this.Lable_CancelText);
			}
			base.WndProc(ref m);
		}

		public string Lable_OpenFloder = "文件夹:";

		public string Lable_SelectTexg = "选择";

		public string Lable_CancelText = "取消";

		private OpenDialogNative mNativeDialog = null;

		private OpenFileDialogEx mFileDialogEx = null;

		private bool mWatchForActivate = false;

		private IntPtr mOpenDialogHandle = IntPtr.Zero;
	}
}
