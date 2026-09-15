using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using OpenDialogs;

namespace CustomControls.Controls
{
	public class OpenFileDialogEx : UserControl
	{
		public virtual string Lable_OpenFloder
		{
			get
			{
				return this.lable_OpenFloder;
			}
		}

		public virtual string Lable_SelectTexg
		{
			get
			{
				return this.lable_SelectTexg;
			}
		}

		public virtual string Lable_CancelText
		{
			get
			{
				return this.lable_CancelText;
			}
		}

		public virtual event EventHandler SelectedNameChanged;

		public event OpenFileDialogEx.FileNameChangedHandler FileNameChanged;

		public event OpenFileDialogEx.FileNameChangedHandler FolderNameChanged;

		public event EventHandler ClosingDialog;

		protected event OpenFileDialogEx.FilesChangedHandler FilesSelected;

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

		private void OpenFileDialogEx_Disposed(object sender, EventArgs e)
		{
			WindowHelper.ShowCurrentWindowHandle();
		}

		private void OpenDialog_HelpRequest(object sender, EventArgs e)
		{
			this.Close();
			this.OnSelectedNameChanged();
		}

		private void OpenFileDialogEx_SizeChanged(object sender, EventArgs e)
		{
			this.textBox1.Width = base.Width + 1;
		}

		public OpenFileDialog OpenDialog
		{
			get
			{
				return this.dlgOpen;
			}
		}

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

		public virtual void OnFileNameChanged(string fileName)
		{
			if (this.FileNameChanged != null)
			{
				this.FileNameChanged(this, fileName);
			}
		}

		public virtual void OnFolderNameChanged(string folderName)
		{
			if (this.FolderNameChanged != null)
			{
				this.FolderNameChanged(this, folderName);
			}
		}

		public virtual void OnFilesSelectedHandle(IntPtr handle)
		{
			if (this.FilesSelected != null)
			{
				this.FilesSelected(this, handle);
			}
		}

		public virtual void OnClosingDialog()
		{
			if (this.ClosingDialog != null)
			{
				this.ClosingDialog(this, new EventArgs());
			}
		}

		public virtual void OnSelectedNameChanged()
		{
			if (this.SelectedNameChanged != null)
			{
				this.SelectedNameChanged(this, EventArgs.Empty);
			}
		}

		public virtual void SetShowText(string directorypath)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(directorypath);
			if (directoryInfo.Exists)
			{
				this.OpenDialog.InitialDirectory = directorypath;
			}
		}

		public void ShowDialog()
		{
			this.ShowDialog(null);
		}

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

		public void Close()
		{
			if (this.form != null)
			{
				this.form.Dispose();
				this.form.Close();
				base.Dispose();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

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

		private string lable_OpenFloder = "文件夹:";

		private string lable_SelectTexg = "选择";

		private string lable_CancelText = "取消";

		private SetWindowPosFlags UFLAGSHIDE = (SetWindowPosFlags)659;

		private FolderViewMode mDefaultViewMode = FolderViewMode.Default;

		private DummyForm form;

		private IContainer components = null;

		protected OpenFileDialog dlgOpen;

		protected TextBox textBox1;

		public delegate void FileNameChangedHandler(OpenFileDialogEx sender, string filePath);

		protected delegate void FilesChangedHandler(OpenFileDialogEx sender, IntPtr handle);
	}
}
