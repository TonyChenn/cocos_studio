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
	public class FileChooserAdapter
	{
		private bool IsSelectFiles { get; set; }

		public FileChooserAdapter(FileAction fileChooserAction, string title = "", bool selectMultiple = false, string initialDirectory = "")
		{
			this.Title = title;
			this.FileChooserAction = fileChooserAction;
			this.AllowMultipleSelect = selectMultiple;
			this.InitialDirectory = initialDirectory;
		}

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

		public static bool ISMac;

		public string Path;

		public string[] Paths;

		public string[] AllowedFileTypes;

		public bool AllowMultipleSelect;

		public bool UseModernWindowsDialog;

		public string Title;

		public FileAction FileChooserAction;

		public string InitialDirectory;
	}
}
