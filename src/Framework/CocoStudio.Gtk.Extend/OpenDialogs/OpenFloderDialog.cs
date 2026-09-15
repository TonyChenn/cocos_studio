using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CustomControls.Controls;

namespace OpenDialogs
{
	public class OpenFloderDialog : OpenFileDialogEx
	{
		public DirectoryInfo Info { get; private set; }

		public OpenFloderDialog()
		{
			this.InitializeComponent();
			base.OpenDialog.Multiselect = false;
			base.OpenDialog.Filter = "folders|*.\n";
		}

		public override void OnFileNameChanged(string fileName)
		{
			this.SetShowText(fileName);
		}

		public override void OnFolderNameChanged(string folderName)
		{
			this.SetShowText(folderName);
		}

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
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Name = "OpenFloderDialog";
			base.ResumeLayout(false);
		}

		private IContainer components = null;
	}
}
