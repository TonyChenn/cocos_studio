using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Gtk;
using Mono.TextEditor;
using Mono.TextEditor.Utils;
using MonoDevelop.Core;

namespace MonoDevelop.SourceEditor
{
	internal static class AutoSave
	{
		private class FileContent
		{
			public string FileName;

			public TextDocument Content;

			public FileContent(string fileName, TextDocument content)
			{
				FileName = fileName;
				Content = content;
			}
		}

		private static string autoSavePath;

		private static bool autoSaveEnabled;

		private static readonly AutoResetEvent resetEvent;

		private static readonly AutoResetEvent saveEvent;

		private static bool autoSaveThreadRunning;

		private static Thread autoSaveThread;

		private static Queue<FileContent> queue;

		private static object contentLock;

		public static bool Running => autoSaveThreadRunning;

		static AutoSave()
		{
			autoSavePath = UserProfile.Current.CacheDir.Combine("AutoSave");
			resetEvent = new AutoResetEvent(initialState: false);
			saveEvent = new AutoResetEvent(initialState: false);
			autoSaveThreadRunning = false;
			queue = new Queue<FileContent>();
			contentLock = new object();
			try
			{
				if (!Directory.Exists(autoSavePath))
				{
					Directory.CreateDirectory(autoSavePath);
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Can't create auto save path:" + autoSavePath + ". Auto save is disabled.", ex);
				autoSaveEnabled = false;
				return;
			}
			autoSaveEnabled = true;
			StartAutoSaveThread();
		}

		private static string GetAutoSaveFileName(string fileName)
		{
			if (fileName == null)
			{
				return null;
			}
			string text = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + Path.GetExtension(fileName) + "~");
			return Path.Combine(autoSavePath, text.Replace(',', '_').Replace(" ", "").Replace(":", "")
				.Replace(Path.DirectorySeparatorChar, '_')
				.Replace(Path.AltDirectorySeparatorChar, '_'));
		}

		public static bool AutoSaveExists(string fileName)
		{
			if (!autoSaveEnabled)
			{
				return false;
			}
			try
			{
				string autoSaveFileName = GetAutoSaveFileName(fileName);
				bool flag = File.Exists(autoSaveFileName);
				if (flag && File.GetLastWriteTimeUtc(autoSaveFileName) < File.GetLastWriteTimeUtc(fileName))
				{
					File.Delete(autoSaveFileName);
					return false;
				}
				return flag;
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error in auto save - disableing.", ex);
				DisableAutoSave();
				return false;
			}
		}

		private static void CreateAutoSave(string fileName, string content)
		{
			if (!autoSaveEnabled)
			{
				return;
			}
			try
			{
				string tempFileName = Path.GetTempFileName();
				File.WriteAllText(tempFileName, content);
				string autoSaveFileName = GetAutoSaveFileName(fileName);
				if (File.Exists(autoSaveFileName))
				{
					File.Delete(autoSaveFileName);
				}
				File.Move(tempFileName, autoSaveFileName);
				++Counters.AutoSavedFiles;
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error in auto save while creating: " + fileName + ". Disableing auto save.", ex);
				DisableAutoSave();
			}
		}

		private static void StartAutoSaveThread()
		{
			autoSaveThreadRunning = true;
			if (autoSaveThread == null)
			{
				autoSaveThread = new Thread(AutoSaveThread);
				autoSaveThread.Name = "Autosave";
				autoSaveThread.IsBackground = true;
				autoSaveThread.Start();
			}
		}

		private static void AutoSaveThread()
		{
			while (autoSaveThreadRunning)
			{
				resetEvent.WaitOne();
				while (queue.Count > 0)
				{
					FileContent content = queue.Dequeue();
					if (string.IsNullOrEmpty(content.FileName))
					{
						continue;
					}
					string text = null;
					bool set = false;
					Application.Invoke(delegate
					{
						try
						{
							text = content.Content.Text;
							set = true;
						}
						catch (Exception ex)
						{
							LoggingService.LogError("Exception in auto save thread.", ex);
						}
						finally
						{
							saveEvent.Set();
						}
					});
					saveEvent.WaitOne();
					if (set)
					{
						CreateAutoSave(content.FileName, text);
					}
				}
			}
		}

		public static string LoadAutoSave(string fileName)
		{
			string autoSaveFileName = GetAutoSaveFileName(fileName);
			return TextFileUtility.ReadAllText(autoSaveFileName);
		}

		public static void RemoveAutoSaveFile(string fileName)
		{
			if (!autoSaveEnabled || !AutoSaveExists(fileName))
			{
				return;
			}
			string autoSaveFileName = GetAutoSaveFileName(fileName);
			try
			{
				lock (contentLock)
				{
					File.Delete(autoSaveFileName);
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Can't delete auto save file: " + autoSaveFileName + ". Disableing auto save.", ex);
				DisableAutoSave();
			}
		}

		public static void InformAutoSaveThread(TextDocument content)
		{
			if (content != null && autoSaveEnabled)
			{
				if (content.IsDirty)
				{
					queue.Enqueue(new FileContent(content.FileName, content));
					resetEvent.Set();
				}
				else
				{
					RemoveAutoSaveFile(content.FileName);
				}
			}
		}

		public static void DisableAutoSave()
		{
			autoSaveThreadRunning = false;
			if (autoSaveThread != null)
			{
				resetEvent.Set();
				autoSaveThread.Join();
				autoSaveThread = null;
			}
			autoSaveEnabled = false;
		}
	}
}
