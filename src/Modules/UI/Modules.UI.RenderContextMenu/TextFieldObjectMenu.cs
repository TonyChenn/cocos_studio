using System;
using System.Collections.Generic;
using System.Reflection;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.UI.RenderContextMenu
{
	[Extension(typeof(ICustomMenu))]
	public class TextFieldObjectMenu : NodeObjectMenu
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
					this.menuItemEditOccupyingText.TriggerObject = this.triggerbutton;
				}
			}
		}

		public override List<MenuItem> GetCustomMenu()
		{
			this.UpdateCheckItemState();
			return this.MenuItemList;
		}

		private void UpdateCheckItemState()
		{
			PropertyInfo property = this.TriggerButton.GetType().GetProperty("Password");
			this.passwordValue = (PasswordValue)property.GetValue(this.triggerbutton, null);
			this.menuItemShowCiphertext.Active = this.passwordValue.PasswordEnable;
		}

		protected override void InitMenu()
		{
			base.InitMenu();
			this.menuItemEditOccupyingText = new TextEditMenuItem(LanguageInfo.ContexMenu_EditOccupyingText);
			this.MenuItemList.Add(this.menuItemEditOccupyingText);
			this.menuItemShowCiphertext = new CheckMenuItem(LanguageInfo.ContexMenu_ShowCiphertext);
			this.menuItemShowCiphertext.Toggled += this.menuItemShowCiphertext_Click;
			this.MenuItemList.Add(this.menuItemShowCiphertext);
		}

		private void menuItemShowCiphertext_Click(object sender, EventArgs e)
		{
			using (CompositeTask.Run("show ciphertext", null))
			{
				this.passwordValue.PasswordEnable = this.menuItemShowCiphertext.Active;
				PropertyInfo property = this.TriggerButton.GetType().GetProperty("Password");
				property.SetValue(this.TriggerButton, this.passwordValue, null);
			}
		}

		public override Type GetObjectType()
		{
			return typeof(TextFieldObject);
		}

		private TextEditMenuItem menuItemEditOccupyingText;

		private CheckMenuItem menuItemShowCiphertext;

		private PasswordValue passwordValue;

		private VisualObject triggerbutton;
	}
}
