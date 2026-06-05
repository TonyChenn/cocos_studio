using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CustomControls.Controls;

namespace OpenDialogs
{
	// Token: 0x02000031 RID: 49
	public class OpenFloderDialog : OpenFileDialogEx
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00007E60 File Offset: 0x00006060
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00007E77 File Offset: 0x00006077
		public DirectoryInfo Info { get; private set; }

		// Token: 0x0600017F RID: 383 RVA: 0x00007E80 File Offset: 0x00006080
		public OpenFloderDialog()
		{
			this.InitializeComponent();
			base.OpenDialog.Multiselect = false;
			base.OpenDialog.Filter = "folders|*.\n";
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00007EB7 File Offset: 0x000060B7
		public override void OnFileNameChanged(string fileName)
		{
			this.SetShowText(fileName);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007EC2 File Offset: 0x000060C2
		public override void OnFolderNameChanged(string folderName)
		{
			this.SetShowText(folderName);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007ED0 File Offset: 0x000060D0
		public override void SetShowText(string directorypath)
		{
			if (!string.IsNullOrWhiteSpace(directorypath))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(directorypath);
				if (directoryInfo.Exists)
				{
					base.SetShowText(directorypath);
					this.textBox1.Text = directoryInfo.Name;
					this.Info = directoryInfo;
					return;
				}
			}
			this.Info = null;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007F2C File Offset: 0x0000612C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007F64 File Offset: 0x00006164
		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Name = "OpenFloderDialog";
			base.ResumeLayout(false);
		}

		// Token: 0x040000DD RID: 221
		private IContainer components = null;
	}
}
