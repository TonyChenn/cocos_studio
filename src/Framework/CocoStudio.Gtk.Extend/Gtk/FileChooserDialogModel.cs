using System;

namespace Gtk
{
	public class FileChooserDialogModel
	{
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
