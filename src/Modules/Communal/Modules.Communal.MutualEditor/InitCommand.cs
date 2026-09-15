using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.ExtensionModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Components;

namespace Modules.Communal.MutualEditor
{
	[Extension(Type = typeof(ICommandHandle))]
	public class InitCommand : ICommandHandle
	{
		public void Initialize()
		{
			GlobalCommand.OpenCmd.Execute += InitCommand.OpenCmd_Execute;
		}

		private static void OpenCmd_Execute(object sender, CommandRunArgs e)
		{
			string text = null;
			if (e.DataItem == null)
			{
				string lastBrowserLocation = Services.RecentFileService.LastBrowserLocation;
				if (Option.IsXP)
				{
					string[] fileTypes = new string[]
					{
						"*.ccs"
					};
					text = FileChooserDialogModel.GetOpenFilePath(fileTypes, "Open File", false, lastBrowserLocation).FileName;
					Services.RecentFileService.LastBrowserLocation = Path.GetDirectoryName(text);
				}
				else
				{
					SelectFileDialog selectFileDialog = new SelectFileDialog();
					selectFileDialog.Title = LanguageInfo.Menu_File_OpenProject;
					selectFileDialog.Action = FileChooserAction.Open;
					selectFileDialog.SelectMultiple = false;
					selectFileDialog.CurrentFolder = lastBrowserLocation;
					selectFileDialog.AddFilter("Solution Files", new string[]
					{
						"*.ccs"
					});
					if (selectFileDialog.Run())
					{
						text = selectFileDialog.SelectedFile;
						Services.RecentFileService.LastBrowserLocation = Path.GetDirectoryName(text);
					}
				}
			}
			else
			{
				text = e.DataItem.ToString();
			}
			StartInfoService.Instance.HandleOpenSolution(text);
		}
	}
}
