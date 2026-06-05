using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using CustomControls.Controls;

namespace OpenDialogs
{
	// Token: 0x02000024 RID: 36
	public partial class DummyForm : Form
	{
		// Token: 0x0600011F RID: 287 RVA: 0x000061F0 File Offset: 0x000043F0
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

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000120 RID: 288 RVA: 0x000062DC File Offset: 0x000044DC
		// (set) Token: 0x06000121 RID: 289 RVA: 0x000062F4 File Offset: 0x000044F4
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

		// Token: 0x06000122 RID: 290 RVA: 0x00006300 File Offset: 0x00004500
		protected override void OnClosing(CancelEventArgs e)
		{
			if (this.mNativeDialog != null)
			{
				this.mNativeDialog.Dispose();
			}
			base.OnClosing(e);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006330 File Offset: 0x00004530
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

		// Token: 0x04000080 RID: 128
		public string Lable_OpenFloder = "文件夹:";

		// Token: 0x04000081 RID: 129
		public string Lable_SelectTexg = "选择";

		// Token: 0x04000082 RID: 130
		public string Lable_CancelText = "取消";

		// Token: 0x04000083 RID: 131
		private OpenDialogNative mNativeDialog = null;

		// Token: 0x04000084 RID: 132
		private OpenFileDialogEx mFileDialogEx = null;

		// Token: 0x04000085 RID: 133
		private bool mWatchForActivate = false;

		// Token: 0x04000086 RID: 134
		private IntPtr mOpenDialogHandle = IntPtr.Zero;
	}
}
