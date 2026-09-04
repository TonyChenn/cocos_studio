using System;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.TypeSystem;

namespace MonoDevelop.SourceEditor
{
	internal static class FileRegistry
	{
		private static readonly List<SourceEditorView> openFiles;

		private static readonly FileSystemWatcher fileSystemWatcher;

		private static readonly StringComparison fileNameComparer;

		public static bool SuspendFileWatch { get; set; }

		public static bool HasMultipleIncorretEolMarkers
		{
			get
			{
				int num = 0;
				foreach (SourceEditorView openFile in openFiles)
				{
					if (!SkipView(openFile) && openFile.SourceEditorWidget.HasIncorrectEolMarker)
					{
						num++;
						if (num > 1)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		static FileRegistry()
		{
			openFiles = new List<SourceEditorView>();
			fileNameComparer = (Platform.IsWindows ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
			fileSystemWatcher = new FileSystemWatcher();
			fileSystemWatcher.Created += DispatchService.GuiDispatch<FileSystemEventHandler>(OnFileChanged);
			fileSystemWatcher.Changed += DispatchService.GuiDispatch<FileSystemEventHandler>(OnFileChanged);
			EventHandler<FileEventArgs> value = DispatchService.GuiDispatch<EventHandler<FileEventArgs>>(HandleFileServiceChange);
			FileService.FileCreated += value;
			FileService.FileChanged += value;
		}

		public static void Add(SourceEditorView sourceEditorView)
		{
			openFiles.Add(sourceEditorView);
		}

		public static void Remove(SourceEditorView sourceEditorView)
		{
			openFiles.Remove(sourceEditorView);
			UpdateEolMessages();
		}

		private static bool SkipView(SourceEditorView view)
		{
			if (view.Document != null && view.IsFile)
			{
				return view.IsUntitled;
			}
			return true;
		}

		private static void HandleFileServiceChange(object sender, FileEventArgs e)
		{
			if (!TypeSystemService.TrackFileChanges)
			{
				return;
			}
			bool flag = false;
			foreach (FileEventInfo item in e)
			{
				foreach (SourceEditorView openFile in openFiles)
				{
					if (!SkipView(openFile) && string.Equals(openFile.ContentName, item.FileName, fileNameComparer))
					{
						if (!openFile.IsDirty)
						{
							openFile.SourceEditorWidget.Reload();
						}
						else
						{
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				CommitViewChange(GetAllChangedFiles());
			}
		}

		private static List<SourceEditorView> GetAllChangedFiles()
		{
			List<SourceEditorView> list = new List<SourceEditorView>();
			foreach (SourceEditorView openFile in openFiles)
			{
				if (!SkipView(openFile) && !(openFile.LastSaveTimeUtc == File.GetLastWriteTimeUtc(openFile.ContentName)))
				{
					if (!openFile.IsDirty)
					{
						openFile.SourceEditorWidget.Reload();
					}
					else
					{
						list.Add(openFile);
					}
				}
			}
			return list;
		}

		private static void OnFileChanged(object sender, FileSystemEventArgs e)
		{
			if (e.ChangeType == WatcherChangeTypes.Changed || e.ChangeType == WatcherChangeTypes.Created)
			{
				CheckFileChange(e.FullPath);
			}
		}

		private static void CheckFileChange(string fileName)
		{
			if (SuspendFileWatch)
			{
				return;
			}
			List<SourceEditorView> list = new List<SourceEditorView>();
			foreach (SourceEditorView openFile in openFiles)
			{
				if (!SkipView(openFile) && string.Equals(openFile.ContentName, fileName, fileNameComparer) && !(openFile.LastSaveTimeUtc == File.GetLastWriteTimeUtc(fileName)))
				{
					if (!openFile.IsDirty)
					{
						openFile.SourceEditorWidget.Reload();
					}
					else
					{
						list.Add(openFile);
					}
				}
			}
			CommitViewChange(list);
		}

		private static void CommitViewChange(List<SourceEditorView> changedViews)
		{
			if (changedViews.Count == 0)
			{
				return;
			}
			if (changedViews.Count == 1)
			{
				changedViews[0].SourceEditorWidget.ShowFileChangedWarning(multiple: false);
				return;
			}
			foreach (SourceEditorView changedView in changedViews)
			{
				changedView.SourceEditorWidget.ShowFileChangedWarning(multiple: true);
			}
			if (!changedViews.Contains(IdeApp.Workbench.ActiveDocument.PrimaryView.GetContent<SourceEditorView>()))
			{
				changedViews[0].WorkbenchWindow.SelectWindow();
			}
		}

		public static void IgnoreAllChangedFiles()
		{
			foreach (SourceEditorView allChangedFile in GetAllChangedFiles())
			{
				allChangedFile.LastSaveTimeUtc = File.GetLastWriteTime(allChangedFile.ContentName);
				allChangedFile.SourceEditorWidget.RemoveMessageBar();
				allChangedFile.WorkbenchWindow.ShowNotification = false;
			}
		}

		public static void ReloadAllChangedFiles()
		{
			foreach (SourceEditorView allChangedFile in GetAllChangedFiles())
			{
				allChangedFile.SourceEditorWidget.RemoveMessageBar();
				allChangedFile.SourceEditorWidget.Reload();
				allChangedFile.WorkbenchWindow.ShowNotification = false;
			}
		}

		public static void ConvertLineEndingsInAllFiles()
		{
			DefaultSourceEditorOptions.Instance.LineEndingConversion = LineEndingConversion.ConvertAlways;
			foreach (SourceEditorView openFile in openFiles)
			{
				if (!SkipView(openFile) && openFile.SourceEditorWidget.HasIncorrectEolMarker)
				{
					openFile.SourceEditorWidget.ConvertLineEndings();
					openFile.SourceEditorWidget.RemoveMessageBar();
					openFile.WorkbenchWindow.ShowNotification = false;
					openFile.Save();
				}
			}
		}

		public static void IgnoreLineEndingsInAllFiles()
		{
			DefaultSourceEditorOptions.Instance.LineEndingConversion = LineEndingConversion.LeaveAsIs;
			foreach (SourceEditorView openFile in openFiles)
			{
				if (!SkipView(openFile) && openFile.SourceEditorWidget.HasIncorrectEolMarker)
				{
					openFile.SourceEditorWidget.UseIncorrectMarkers = true;
					openFile.SourceEditorWidget.RemoveMessageBar();
					openFile.WorkbenchWindow.ShowNotification = false;
					openFile.Save();
				}
			}
		}

		public static void UpdateEolMessages()
		{
			bool hasMultipleIncorretEolMarkers = HasMultipleIncorretEolMarkers;
			foreach (SourceEditorView openFile in openFiles)
			{
				if (!SkipView(openFile) && openFile.SourceEditorWidget.HasIncorrectEolMarker)
				{
					openFile.SourceEditorWidget.UpdateEolMarkerMessage(hasMultipleIncorretEolMarkers);
				}
			}
		}
	}
}
