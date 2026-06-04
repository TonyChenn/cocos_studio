using System;
using System.IO;
using CocoStudio.Core;
using CocoStudio.Core.ExtensionModel;
using Gtk;

namespace Modules.Communal.ResourcePanel
{
	internal class ImportFromPsdHandler : MenuHandler
	{
		protected override void Run()
		{
			string fileName = FileChooserDialogModel.GetOpenFilePath(new string[]
			{
				"psd"
			}, "选择PSD文件", false, Services.RecentFileService.LastImportLocation).FileName;
			if (string.IsNullOrEmpty(fileName))
			{
				return;
			}
			if (!File.Exists(fileName))
			{
				return;
			}
			if (!string.Equals(Path.GetExtension(fileName), ".psd", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			string directoryName = Path.GetDirectoryName(fileName);
			if (!string.IsNullOrEmpty(directoryName))
			{
				Services.RecentFileService.LastImportLocation = directoryName;
			}
		}

		protected override void Update(MenuInfo info)
		{
			info.Enabled = true;
			info.Visible = true;
		}
	}

	internal class GenerateLuaHandler : MenuHandler
	{
		protected override void Run()
		{
		}

		protected override void Update(MenuInfo info)
		{
			info.Enabled = true;
			info.Visible = true;
		}
	}
}
