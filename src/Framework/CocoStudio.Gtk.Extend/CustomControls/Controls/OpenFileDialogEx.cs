using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using OpenDialogs;

namespace CustomControls.Controls
{
	// Token: 0x0200002D RID: 45
	public class OpenFileDialogEx : UserControl
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00006E00 File Offset: 0x00005000
		public virtual string Lable_OpenFloder
		{
			get
			{
				return this.lable_OpenFloder;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00006E18 File Offset: 0x00005018
		public virtual string Lable_SelectTexg
		{
			get
			{
				return this.lable_SelectTexg;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00006E30 File Offset: 0x00005030
		public virtual string Lable_CancelText
		{
			get
			{
				return this.lable_CancelText;
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x0600014B RID: 331 RVA: 0x00006E48 File Offset: 0x00005048
		// (remove) Token: 0x0600014C RID: 332 RVA: 0x00006E84 File Offset: 0x00005084
		public virtual event EventHandler SelectedNameChanged;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600014D RID: 333 RVA: 0x00006EC0 File Offset: 0x000050C0
		// (remove) Token: 0x0600014E RID: 334 RVA: 0x00006EFC File Offset: 0x000050FC
		public event OpenFileDialogEx.FileNameChangedHandler FileNameChanged;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600014F RID: 335 RVA: 0x00006F38 File Offset: 0x00005138
		// (remove) Token: 0x06000150 RID: 336 RVA: 0x00006F74 File Offset: 0x00005174
		public event OpenFileDialogEx.FileNameChangedHandler FolderNameChanged;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000151 RID: 337 RVA: 0x00006FB0 File Offset: 0x000051B0
		// (remove) Token: 0x06000152 RID: 338 RVA: 0x00006FEC File Offset: 0x000051EC
		public event EventHandler ClosingDialog;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000153 RID: 339 RVA: 0x00007028 File Offset: 0x00005228
		// (remove) Token: 0x06000154 RID: 340 RVA: 0x00007064 File Offset: 0x00005264
		protected event OpenFileDialogEx.FilesChangedHandler FilesSelected;

		// Token: 0x06000155 RID: 341 RVA: 0x000070A0 File Offset: 0x000052A0
		public OpenFileDialogEx()
		{
			this.InitializeComponent();
			this.DefaultViewMode = FolderViewMode.Default;
			this.OpenDialog.AddExtension = false;
			this.OpenDialog.CheckFileExists = false;
			this.OpenDialog.ValidateNames = true;
			this.OpenDialog.ShowHelp = true;
			base.SizeChanged += this.OpenFileDialogEx_SizeChanged;
			this.OpenDialog.HelpRequest += this.OpenDialog_HelpRequest;
			base.Disposed += this.OpenFileDialogEx_Disposed;
			this.OpenDialog.DereferenceLinks = true;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00007186 File Offset: 0x00005386
		private void OpenFileDialogEx_Disposed(object sender, EventArgs e)
		{
			WindowHelper.ShowCurrentWindowHandle();
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000718F File Offset: 0x0000538F
		private void OpenDialog_HelpRequest(object sender, EventArgs e)
		{
			this.Close();
			this.OnSelectedNameChanged();
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000071A0 File Offset: 0x000053A0
		private void OpenFileDialogEx_SizeChanged(object sender, EventArgs e)
		{
			this.textBox1.Width = base.Width + 1;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000071B8 File Offset: 0x000053B8
		public OpenFileDialog OpenDialog
		{
			get
			{
				return this.dlgOpen;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600015A RID: 346 RVA: 0x000071D0 File Offset: 0x000053D0
		// (set) Token: 0x0600015B RID: 347 RVA: 0x000071E8 File Offset: 0x000053E8
		[DefaultValue(FolderViewMode.Default)]
		public FolderViewMode DefaultViewMode
		{
			get
			{
				return this.mDefaultViewMode;
			}
			set
			{
				this.mDefaultViewMode = value;
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000071F4 File Offset: 0x000053F4
		public virtual void OnFileNameChanged(string fileName)
		{
			if (this.FileNameChanged != null)
			{
				this.FileNameChanged(this, fileName);
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007220 File Offset: 0x00005420
		public virtual void OnFolderNameChanged(string folderName)
		{
			if (this.FolderNameChanged != null)
			{
				this.FolderNameChanged(this, folderName);
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000724C File Offset: 0x0000544C
		public virtual void OnFilesSelectedHandle(IntPtr handle)
		{
			if (this.FilesSelected != null)
			{
				this.FilesSelected(this, handle);
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007278 File Offset: 0x00005478
		public virtual void OnClosingDialog()
		{
			if (this.ClosingDialog != null)
			{
				this.ClosingDialog(this, new EventArgs());
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000072A8 File Offset: 0x000054A8
		public virtual void OnSelectedNameChanged()
		{
			if (this.SelectedNameChanged != null)
			{
				this.SelectedNameChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000072D8 File Offset: 0x000054D8
		public virtual void SetShowText(string directorypath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(directorypath);
			if (directoryInfo.Exists)
			{
				this.OpenDialog.InitialDirectory = directorypath;
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007309 File Offset: 0x00005509
		public void ShowDialog()
		{
			this.ShowDialog(null);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007314 File Offset: 0x00005514
		public void ShowDialog(IWin32Window owner)
		{
			this.form = new DummyForm(this, this.Lable_OpenFloder, this.Lable_SelectTexg, this.Lable_CancelText);
			this.form.Show(owner);
			NativeMethods.SetWindowPos(this.form.Handle, IntPtr.Zero, 0, 0, 0, 0, this.UFLAGSHIDE);
			this.form.WatchForActivate = true;
			try
			{
				this.dlgOpen.ShowDialog(this.form);
			}
			catch (Exception)
			{
			}
			this.form.Dispose();
			this.form.Close();
			base.Dispose();
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000073C8 File Offset: 0x000055C8
		public void Close()
		{
			if (this.form != null)
			{
				this.form.Dispose();
				this.form.Close();
				base.Dispose();
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00007404 File Offset: 0x00005604
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000743C File Offset: 0x0000563C
		private void InitializeComponent()
		{
			this.dlgOpen = new OpenFileDialog();
			this.textBox1 = new TextBox();
			base.SuspendLayout();
			this.textBox1.Name = "textBox1";
			this.textBox1.TabIndex = 0;
			base.Controls.Add(this.textBox1);
			base.Name = "OpenFileDialogEx";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040000C7 RID: 199
		private string lable_OpenFloder = "文件夹:";

		// Token: 0x040000C8 RID: 200
		private string lable_SelectTexg = "选择";

		// Token: 0x040000C9 RID: 201
		private string lable_CancelText = "取消";

		// Token: 0x040000CF RID: 207
		private SetWindowPosFlags UFLAGSHIDE = (SetWindowPosFlags)659;

		// Token: 0x040000D0 RID: 208
		private FolderViewMode mDefaultViewMode = FolderViewMode.Default;

		// Token: 0x040000D1 RID: 209
		private DummyForm form;

		// Token: 0x040000D2 RID: 210
		private IContainer components = null;

		// Token: 0x040000D3 RID: 211
		protected OpenFileDialog dlgOpen;

		// Token: 0x040000D4 RID: 212
		protected TextBox textBox1;

		// Token: 0x0200002E RID: 46
		// (Invoke) Token: 0x06000168 RID: 360
		public delegate void FileNameChangedHandler(OpenFileDialogEx sender, string filePath);

		// Token: 0x0200002F RID: 47
		// (Invoke) Token: 0x0600016C RID: 364
		protected delegate void FilesChangedHandler(OpenFileDialogEx sender, IntPtr handle);
	}
}
