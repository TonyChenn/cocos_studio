using System;
using System.IO;
using System.Reflection;
using CocoStudio.Basic;
using Microsoft.VisualBasic.FileIO;
using MonoDevelop.Platform;
using Xwt;
using Xwt.Backends;
using Xwt.WPFBackend;

namespace CocoStudio.WindowsPlatform
{
	public class CSWindowsPlatformService : MonoDevelop.Platform.WindowsPlatform
	{
		public override Toolkit LoadNativeToolkit()
		{
			string directoryName = Path.GetDirectoryName(((object)this).GetType().Assembly.Location);
			Assembly.LoadFrom(Path.Combine(directoryName, "Xwt.Wpf.dll"));
			Toolkit result = Toolkit.Load(ToolkitType.Wpf);
			Toolkit.CurrentEngine.RegisterBackend<IOpenFileDialogBackend, OpenFileDialogBackend>();
			Toolkit.CurrentEngine.RegisterBackend<ISelectFolderDialogBackend, SelectFolderDialogBackend>();
			Toolkit.CurrentEngine.RegisterBackend<IWebViewBackend, CSWindowWebViewBackend>();
			Toolkit.CurrentEngine.RegisterBackend<IStatusIconBackend, CSWinStatusIconBackend>();
			GlobalSetup();
			return result;
		}

		private void GlobalSetup()
		{
		}

		public override void DeleteToTrash(string directory)
		{
			MakeWritable(recurse: true, directory);
			MakeWritable(recurse: false, Path.GetDirectoryName(directory));
			if (Directory.Exists(directory))
			{
				FileSystem.DeleteDirectory(directory, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
			}
			else if (File.Exists(directory))
			{
				FileSystem.DeleteFile(directory, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
			}
		}

		private void MakeWritable(bool recurse, string filePath)
		{
			if (Directory.Exists(filePath))
			{
				try
				{
					new DirectoryInfo(filePath).Attributes &= ~FileAttributes.ReadOnly;
				}
				catch (Exception ex)
				{
					LogConfig.Logger.Error(ex.ToString());
				}
				if (recurse)
				{
					string[] fileSystemEntries = Directory.GetFileSystemEntries(filePath);
					foreach (string filePath2 in fileSystemEntries)
					{
						MakeWritable(recurse, filePath2);
					}
				}
			}
			else if (File.Exists(filePath))
			{
				try
				{
					new FileInfo(filePath).Attributes &= ~FileAttributes.ReadOnly;
				}
				catch (Exception ex2)
				{
					LogConfig.Logger.Error(ex2.ToString());
				}
			}
		}
	}
}
