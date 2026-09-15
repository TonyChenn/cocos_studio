using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu.Model
{
	[Extension(typeof(ICustomMenu))]
	public class SimpleAudioObjectMenu : NodeObjectMenu
	{
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

		public override List<MenuItem> GetCustomMenu()
		{
			return this.MenuItemList;
		}

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

		public override Type GetObjectType()
		{
			return typeof(SimpleAudioObject);
		}

		private SetStyleMenuItem menuItemAddFile;

		private VisualObject triggerbutton;
	}
}
