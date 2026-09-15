using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Core;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Packer;

namespace Addins.Sample
{
	internal class ExportPlistMenuHandler : MenuHandler
	{
		protected override void Run()
		{
			List<ResourceItem> currentResourceItems = Services.ProjectsService.CurrentResourceItems;
			PlistImageFolder plistImageFolder = currentResourceItems[0] as PlistImageFolder;
			OptionDialog optionDialog = new OptionDialog(plistImageFolder.Name);
			ResponseType responseType = (ResponseType)optionDialog.Run();
			string exportDirectory = optionDialog.ExportDirectory;
			bool isRetainEdge = optionDialog.IsRetainEdge;
			optionDialog.Destroy();
			if (responseType == ResponseType.Ok)
			{
				string[] files = Directory.GetFiles(plistImageFolder.BaseDirectory);
				bool flag = false;
				foreach (string path in files)
				{
					string fileName = Path.GetFileName(path);
					string path2 = Path.Combine(exportDirectory, fileName);
					if (File.Exists(path2))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					MessageBoxResult messageBoxResult = MessageBox.Show(LanguageInfo.Dialog_ExportFileExist, MessageBoxButton.YesNo, MessageBoxImage.Warning, null, EnumMainButton.Yes, null);
					if (messageBoxResult == MessageBoxResult.No)
					{
						return;
					}
				}
				PListImageReader plistImageReader = new PListImageReader(plistImageFolder.FullPath);
				plistImageReader.SaveAllSubImage(exportDirectory, isRetainEdge, false);
			}
		}

		protected override void Update(MenuInfo info)
		{
			List<ResourceItem> currentResourceItems = Services.ProjectsService.CurrentResourceItems;
			if (currentResourceItems == null || currentResourceItems.Count != 1)
			{
				info.Enabled = false;
				return;
			}
			PlistImageFolder plistImageFolder = currentResourceItems[0] as PlistImageFolder;
			if (plistImageFolder == null)
			{
				info.Enabled = false;
				return;
			}
			if (plistImageFolder.DataError != null)
			{
				info.Enabled = false;
			}
		}
	}
}
