using System;
using System.IO;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000045 RID: 69
	public class CocosItemLoadHelp
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00009DFB File Offset: 0x00007FFB
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

		// Token: 0x0600025B RID: 603 RVA: 0x00009E13 File Offset: 0x00008013
		private CocosItemLoadHelp()
		{
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00009E1B File Offset: 0x0000801B
		public bool CheckCocosItem(CocosItemModel cocosItem)
		{
			return this.CheckCocosItemExistsToRomve(cocosItem);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00009E29 File Offset: 0x00008029
		public bool CheckCocosItemExistsToRomve(CocosItemModel cocosItem)
		{
			if (!File.Exists(cocosItem.LocalPath))
			{
				this.AskToRemoveRecord(LanguageInfo.Launcher_InvalidProj, cocosItem);
				return false;
			}
			return true;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00009E48 File Offset: 0x00008048
		private void AskToRemoveRecord(string info, CocosItemModel cocosItem)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show(info, MessageBoxButton.YesNo, MessageBoxImage.Question, Services.MainWindow, EnumMainButton.Yes, null);
			if (messageBoxResult == MessageBoxResult.Yes)
			{
				Services.RecentFileService.RemoveCocosItem(cocosItem);
			}
		}

		// Token: 0x040000EC RID: 236
		private static CocosItemLoadHelp instance;
	}
}
