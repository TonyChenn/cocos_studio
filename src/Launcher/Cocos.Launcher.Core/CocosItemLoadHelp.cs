using System;
using System.IO;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	public class CocosItemLoadHelp
	{
		public static CocosItemLoadHelp Instance
		{
			get
			{
				if (CocosItemLoadHelp.instance == null)
				{
					CocosItemLoadHelp.instance = new CocosItemLoadHelp();
				}
				return CocosItemLoadHelp.instance;
			}
		}

		private CocosItemLoadHelp()
		{
		}

		public bool CheckCocosItem(CocosItemModel cocosItem)
		{
			return this.CheckCocosItemExistsToRomve(cocosItem);
		}

		public bool CheckCocosItemExistsToRomve(CocosItemModel cocosItem)
		{
			if (!File.Exists(cocosItem.LocalPath))
			{
				this.AskToRemoveRecord(LanguageInfo.Launcher_InvalidProj, cocosItem);
				return false;
			}
			return true;
		}

		private void AskToRemoveRecord(string info, CocosItemModel cocosItem)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show(info, MessageBoxButton.YesNo, MessageBoxImage.Question, Services.MainWindow, EnumMainButton.Yes, null);
			if (messageBoxResult == MessageBoxResult.Yes)
			{
				Services.RecentFileService.RemoveCocosItem(cocosItem);
			}
		}

		private static CocosItemLoadHelp instance;
	}
}
