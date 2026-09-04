using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu.Model
{
	// Token: 0x02000014 RID: 20
	[Extension(typeof(ICustomMenu))]
	public class SimpleAudioObjectMenu : NodeObjectMenu
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003458 File Offset: 0x00001658
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00003470 File Offset: 0x00001670
		public override VisualObject TriggerButton
		{
			get
			{
				return this.triggerbutton;
			}
			set
			{
				if (this.triggerbutton != value)
				{
					this.triggerbutton = value;
					this.menuItemAddFile.TriggerObject = value;
				}
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000034AC File Offset: 0x000016AC
		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000034C4 File Offset: 0x000016C4
		protected override void InitMenu()
		{
			base.InitMenu();
			string[] filetype = new string[]
			{
				"mp3",
				"wav"
			};
			this.menuItemAddFile = new SetStyleMenuItem(filetype, "FileData", LanguageInfo.ContexMenu_SetAudioFile);
			this.MenuItemList.Add(this.menuItemAddFile);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000351C File Offset: 0x0000171C
		public override Type GetObjectType()
		{
			return typeof(SimpleAudioObject);
		}

		// Token: 0x0400002B RID: 43
		private SetStyleMenuItem menuItemAddFile;

		// Token: 0x0400002C RID: 44
		private VisualObject triggerbutton;
	}
}
