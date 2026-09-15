using System;
using System.Collections.Generic;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	[Extension(typeof(ICustomMenu))]
	public class TextBMFontObjectMenu : NodeObjectMenu
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
					this.menuItemSetStyle.TriggerObject = this.triggerbutton;
					this.menuItemEditText.TriggerObject = this.triggerbutton;
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
				"fnt"
			};
			this.menuItemEditText = new TextEditMenuItem(LanguageInfo.ContexMenu_editText);
			this.MenuItemList.Add(this.menuItemEditText);
			this.menuItemSetStyle = new SetStyleMenuItem(filetype, "LabelBMFontFile_CNB", LanguageInfo.ContexMenu_SetCustomFontRec);
			this.MenuItemList.Add(this.menuItemSetStyle);
		}

		public override Type GetObjectType()
		{
			return typeof(TextBMFontObject);
		}

		private SetStyleMenuItem menuItemSetStyle;

		private TextEditMenuItem menuItemEditText;

		private VisualObject triggerbutton;
	}
}
