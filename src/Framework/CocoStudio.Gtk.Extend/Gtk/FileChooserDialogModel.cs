using System;

namespace Gtk
{
	// Token: 0x02000080 RID: 128
	public class FileChooserDialogModel
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x0000B74C File Offset: 0x0000994C
		public static SelectFolderDialogResult GetBrowseDialogPath(string title = "Select Folder", bool selectMultiple = false, string initialDirectory = "", bool IsWin7Style = false, bool useModernWindowsDialog = false)
		{
			SelectFolderDialogResult selectFolderDialogResult = default(SelectFolderDialogResult);
			FileChooserAdapter fileChooserAdapter = new FileChooserAdapter(FileAction.SelectFolder, title, selectMultiple, initialDirectory);
			fileChooserAdapter.UseModernWindowsDialog = useModernWindowsDialog;
			SelectFolderDialogResult result;
			if (fileChooserAdapter.Run(IsWin7Style))
			{
				selectFolderDialogResult.Folder = fileChooserAdapter.Path;
				selectFolderDialogResult.Folders = fileChooserAdapter.Paths;
				result = selectFolderDialogResult;
			}
			else
			{
				result = selectFolderDialogResult;
			}
			return result;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000B7A0 File Offset: 0x000099A0
		public static SelectFileDialogResult GetOpenFilePath(string[] fileTypes = null, string title = "Open File", bool selectMultiple = false, string initialDirectory = "", bool useModernWindowsDialog = false)
		{
			SelectFileDialogResult selectFileDialogResult = default(SelectFileDialogResult);
			if (fileTypes != null)
			{
				for (int i = 0; i < fileTypes.Length; i++)
				{
					fileTypes[i] = fileTypes[i].Replace("*.", "");
				}
			}
			FileChooserAdapter fileChooserAdapter = new FileChooserAdapter(FileAction.Open, title, selectMultiple, initialDirectory);
			fileChooserAdapter.AllowedFileTypes = fileTypes;
			fileChooserAdapter.UseModernWindowsDialog = useModernWindowsDialog;
			SelectFileDialogResult result;
			if (fileChooserAdapter.Run(false))
			{
				selectFileDialogResult.FileName = fileChooserAdapter.Path;
				selectFileDialogResult.FileNames = fileChooserAdapter.Paths;
				result = selectFileDialogResult;
			}
			else
			{
				result = selectFileDialogResult;
			}
			return result;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000B834 File Offset: 0x00009A34
		public static SelectFileDialogResult GetOpenFilesPath(string[] fileTypes = null, string title = "Select Files", bool selectMultiple = false, string initialDirectory = "")
		{
			SelectFileDialogResult selectFileDialogResult = default(SelectFileDialogResult);
			if (fileTypes != null)
			{
				for (int i = 0; i < fileTypes.Length; i++)
				{
					fileTypes[i] = fileTypes[i].Replace("*.", "");
				}
			}
			FileChooserAdapter fileChooserAdapter = new FileChooserAdapter(FileAction.SelectFiles, title, selectMultiple, initialDirectory);
			fileChooserAdapter.AllowedFileTypes = fileTypes;
			SelectFileDialogResult result;
			if (fileChooserAdapter.Run(false))
			{
				selectFileDialogResult.FileName = fileChooserAdapter.Path;
				selectFileDialogResult.FileNames = fileChooserAdapter.Paths;
				result = selectFileDialogResult;
			}
			else
			{
				result = selectFileDialogResult;
			}
			return result;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000B8C8 File Offset: 0x00009AC8
		public static SelectFileDialogResult GetSaveFilesPath(string[] fileTypes = null, string title = "Save Files", bool selectMultiple = false, string initialDirectory = "")
		{
			SelectFileDialogResult selectFileDialogResult = default(SelectFileDialogResult);
			if (fileTypes != null)
			{
				for (int i = 0; i < fileTypes.Length; i++)
				{
					fileTypes[i] = fileTypes[i].Replace("*.", "");
				}
			}
			FileChooserAdapter fileChooserAdapter = new FileChooserAdapter(FileAction.Save, title, selectMultiple, initialDirectory);
			fileChooserAdapter.AllowedFileTypes = fileTypes;
			SelectFileDialogResult result;
			if (fileChooserAdapter.Run(false))
			{
				selectFileDialogResult.FileName = fileChooserAdapter.Path;
				selectFileDialogResult.FileNames = fileChooserAdapter.Paths;
				result = selectFileDialogResult;
			}
			else
			{
				result = selectFileDialogResult;
			}
			return result;
		}
	}
}
