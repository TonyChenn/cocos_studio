using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AppKit;
using Foundation;
using Microsoft.WindowsAPICodePack.Dialogs;
using MonoDevelop.Core;
using OpenDialogs;

namespace Gtk
{
	// Token: 0x0200007F RID: 127
	public class FileChooserAdapter
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000AE64 File Offset: 0x00009064
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x0000AE7B File Offset: 0x0000907B
		private bool IsSelectFiles { get; set; }

		// Token: 0x060002CA RID: 714 RVA: 0x0000AE84 File Offset: 0x00009084
		public FileChooserAdapter(FileAction fileChooserAction, string title = "", bool selectMultiple = false, string initialDirectory = "")
		{
			this.Title = title;
			this.FileChooserAction = fileChooserAction;
			this.AllowMultipleSelect = selectMultiple;
			this.InitialDirectory = initialDirectory;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000AEAC File Offset: 0x000090AC
		public bool Run(bool IsWin7Style = false)
		{
			bool result;
			if (Platform.IsMac)
			{
				result = this.MacOpenFile();
			}
			else if (Platform.IsWindows)
			{
				if (this.UseModernWindowsDialog && (this.FileChooserAction == FileAction.Open || this.FileChooserAction == FileAction.SelectFolder))
				{
					result = this.WindowsCommonOpenDialog();
				}
				else
				{
					result = this.WinOpenFile(IsWin7Style);
				}
			}
			else
			{
				result = this.GtkOpenFile();
			}
			return result;
		}

		private bool WindowsCommonOpenDialog()
		{
			using (CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog())
			{
				commonOpenFileDialog.Title = this.Title;
				commonOpenFileDialog.Multiselect = this.AllowMultipleSelect;
				commonOpenFileDialog.IsFolderPicker = this.FileChooserAction == FileAction.SelectFolder;
				commonOpenFileDialog.EnsurePathExists = true;
				if (!string.IsNullOrWhiteSpace(this.InitialDirectory) && Directory.Exists(this.InitialDirectory))
				{
					commonOpenFileDialog.InitialDirectory = this.InitialDirectory;
				}
				if (commonOpenFileDialog.ShowDialog() != CommonFileDialogResult.Ok)
				{
					return false;
				}
				this.Paths = commonOpenFileDialog.FileNames.ToArray<string>();
				this.Path = this.Paths.FirstOrDefault<string>();
				return this.Paths.Length > 0;
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000AEFC File Offset: 0x000090FC
		private bool WinOpenFile(bool IsWin7Style)
		{
			switch (this.FileChooserAction)
			{
			case FileAction.Open:
				return this.WinOpenFile_OpenFile();
			case FileAction.Save:
				return this.WindowsSaveFiles();
			case FileAction.SelectFolder:
				return this.WinOpenFile_OpenFolder(IsWin7Style);
			case FileAction.SelectFiles:
				return this.WindowsOpenFiles();
			}
			return false;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000AF5C File Offset: 0x0000915C
		private bool WindowsSaveFiles()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Title = this.Title;
			if (this.InitialDirectory != "")
			{
				string fileName = System.IO.Path.GetFileName(this.InitialDirectory);
				saveFileDialog.FileName = fileName;
				saveFileDialog.InitialDirectory = this.InitialDirectory;
			}
			if (this.AllowedFileTypes != null)
			{
				string[] array = new string[this.AllowedFileTypes.Length];
				for (int i = 0; i < this.AllowedFileTypes.Length; i++)
				{
					array[i] = "*." + this.AllowedFileTypes[i];
				}
				string arg = string.Join(",", array);
				string arg2 = string.Join(";", array);
				saveFileDialog.Filter = string.Format("({0})|{1}", arg, arg2);
			}
			else
			{
				saveFileDialog.Filter = "(*.*)|*.*";
			}
			bool result;
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.Paths = saveFileDialog.FileNames;
				this.Path = saveFileDialog.FileName;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000B080 File Offset: 0x00009280
		private bool GtkOpenFile()
		{
			Window mainWindow = ApplicationCurrent.MainWindow;
			FileChooserDialog fileChooserDialog = new FileChooserDialog(this.Title, mainWindow, (FileChooserAction)this.FileChooserAction, new object[0]);
			switch (this.FileChooserAction)
			{
			case FileAction.Open:
			{
				fileChooserDialog.AddButton("Open", ResponseType.Ok);
				FileFilter fileFilter = new FileFilter();
				if (this.AllowedFileTypes != null)
				{
					foreach (string str in this.AllowedFileTypes)
					{
						fileFilter.AddPattern("*." + str);
					}
				}
				break;
			}
			case FileAction.SelectFolder:
				fileChooserDialog.AddButton("Select Folder", ResponseType.Ok);
				break;
			}
			fileChooserDialog.AddButton("Cancel", ResponseType.Cancel);
			fileChooserDialog.SelectMultiple = this.AllowMultipleSelect;
			MessageFileDiaolg messageFileDiaolg = (MessageFileDiaolg)fileChooserDialog.Run();
			bool result;
			if (messageFileDiaolg == MessageFileDiaolg.OK)
			{
				this.Paths = fileChooserDialog.Filenames;
				this.Path = this.Paths[0];
				fileChooserDialog.Destroy();
				result = true;
			}
			else
			{
				fileChooserDialog.Destroy();
				result = false;
			}
			return result;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000B1A8 File Offset: 0x000093A8
		private bool WinOpenFile_OpenFile()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = this.AllowMultipleSelect;
			openFileDialog.Title = this.Title;
			if (this.AllowedFileTypes != null)
			{
				string[] array = new string[this.AllowedFileTypes.Length];
				for (int i = 0; i < this.AllowedFileTypes.Length; i++)
				{
					array[i] = "*." + this.AllowedFileTypes[i];
				}
				string arg = string.Join(",", array);
				string arg2 = string.Join(";", array);
				openFileDialog.Filter = string.Format("({0})|{1}", arg, arg2);
			}
			else
			{
				openFileDialog.Filter = "(*.*)|*.*";
			}
			if (this.InitialDirectory != "")
			{
				openFileDialog.InitialDirectory = this.InitialDirectory;
			}
			bool result;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.Paths = openFileDialog.FileNames;
				this.Path = openFileDialog.FileName;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000B32C File Offset: 0x0000952C
		private bool WinOpenFile_OpenFolder(bool IsWin7Style)
		{
			bool result;
			if (IsWin7Style)
			{
				FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
				folderBrowserDialog.Description = this.Title;
				folderBrowserDialog.ShowNewFolderButton = true;
				if (this.InitialDirectory != "")
				{
					folderBrowserDialog.SelectedPath = this.InitialDirectory;
				}
				DialogResult dialogResult = folderBrowserDialog.ShowDialog();
				if (dialogResult == DialogResult.OK)
				{
					this.Path = folderBrowserDialog.SelectedPath;
					this.Paths = new string[]
					{
						folderBrowserDialog.SelectedPath
					};
					result = true;
				}
				else
				{
					result = false;
				}
			}
			else
			{
				OpenFloderDialog folderBrowser = new OpenFloderDialog();
				folderBrowser.OpenDialog.Title = this.Title;
				if (this.InitialDirectory != "")
				{
					folderBrowser.SetShowText(this.InitialDirectory);
				}
				folderBrowser.SelectedNameChanged += delegate(object s, EventArgs e)
				{
					if (folderBrowser.Info != null)
					{
						this.Path = folderBrowser.Info.FullName;
						this.Paths = new string[]
						{
							folderBrowser.Info.FullName
						};
					}
				};
				folderBrowser.ShowDialog();
				result = (folderBrowser.Info != null);
			}
			return result;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000B46C File Offset: 0x0000966C
		private bool MacOpenFile()
		{
			NSOpenPanel nsopenPanel = new NSOpenPanel();
			nsopenPanel.Title = this.Title;
			switch (this.FileChooserAction)
			{
			case FileAction.Open:
				nsopenPanel.CanChooseFiles = true;
				nsopenPanel.CanChooseDirectories = false;
				if (this.AllowedFileTypes != null)
				{
					nsopenPanel.AllowedFileTypes = this.AllowedFileTypes;
				}
				break;
			case FileAction.Save:
			{
				NSSavePanel savePanel = NSSavePanel.SavePanel;
				savePanel.AllowedFileTypes = this.AllowedFileTypes;
				savePanel.CanCreateDirectories = true;
				savePanel.DirectoryUrl = NSUrl.CreateFileUrl(new string[]
				{
					this.InitialDirectory
				});
				if (savePanel.RunModal() == 1)
				{
					this.Path = savePanel.Url.Path;
					savePanel.Dispose();
					return true;
				}
				savePanel.Dispose();
				return false;
			}
			case FileAction.SelectFolder:
				nsopenPanel.CanChooseDirectories = true;
				nsopenPanel.CanChooseFiles = false;
				break;
			case FileAction.SelectFiles:
				nsopenPanel.CanChooseDirectories = true;
				nsopenPanel.CanChooseFiles = true;
				break;
			}
			nsopenPanel.CanCreateDirectories = false;
			nsopenPanel.AllowsMultipleSelection = this.AllowMultipleSelect;
			nsopenPanel.DirectoryUrl = NSUrl.CreateFileUrl(new string[]
			{
				this.InitialDirectory
			});
			bool result;
			if (nsopenPanel.RunModal() == 1)
			{
				int num = nsopenPanel.Urls.Length;
				string[] array = new string[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = nsopenPanel.Urls[i].Path.ToString();
				}
				this.Paths = array;
				this.Path = this.Paths[0];
				nsopenPanel.Dispose();
				result = true;
			}
			else
			{
				nsopenPanel.Dispose();
				result = false;
			}
			return result;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000B6A8 File Offset: 0x000098A8
		private bool WindowsOpenFiles()
		{
			OpenFilesDialog filesdialog = new OpenFilesDialog();
			filesdialog.OpenDialog.Title = this.Title;
			if (this.InitialDirectory != "")
			{
				filesdialog.SetShowText(this.InitialDirectory);
			}
			filesdialog.SelectedNameChanged += delegate(object s, EventArgs e)
			{
				if (!string.IsNullOrWhiteSpace(filesdialog.FolderParent))
				{
					this.Paths = filesdialog.FilesPath.ToArray();
				}
			};
			filesdialog.ShowDialog();
			return !string.IsNullOrWhiteSpace(filesdialog.FolderParent);
		}

		// Token: 0x0400034F RID: 847
		public static bool ISMac;

		// Token: 0x04000350 RID: 848
		public string Path;

		// Token: 0x04000351 RID: 849
		public string[] Paths;

		// Token: 0x04000352 RID: 850
		public string[] AllowedFileTypes;

		// Token: 0x04000353 RID: 851
		public bool AllowMultipleSelect;

		public bool UseModernWindowsDialog;

		// Token: 0x04000354 RID: 852
		public string Title;

		// Token: 0x04000355 RID: 853
		public FileAction FileChooserAction;

		// Token: 0x04000356 RID: 854
		public string InitialDirectory;
	}
}
