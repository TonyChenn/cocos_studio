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
	// Token: 0x02000027 RID: 39
	[Extension(typeof(ICustomMenu))]
	public class TextFieldObjectMenu : NodeObjectMenu
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00007354 File Offset: 0x00005554
		// (set) Token: 0x06000124 RID: 292 RVA: 0x0000736C File Offset: 0x0000556C
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

		// Token: 0x06000126 RID: 294 RVA: 0x000073B0 File Offset: 0x000055B0
		public override List<MenuItem> GetCustomMenu()
		{
			this.UpdateCheckItemState();
			return this.MenuItemList;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000073D0 File Offset: 0x000055D0
		private void UpdateCheckItemState()
		{
			PropertyInfo property = this.TriggerButton.GetType().GetProperty("Password");
			this.passwordValue = (PasswordValue)property.GetValue(this.triggerbutton, null);
			this.menuItemShowCiphertext.Active = this.passwordValue.PasswordEnable;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007424 File Offset: 0x00005624
		protected override void InitMenu()
		{
			base.InitMenu();
			this.menuItemEditOccupyingText = new TextEditMenuItem(LanguageInfo.ContexMenu_EditOccupyingText);
			this.MenuItemList.Add(this.menuItemEditOccupyingText);
			this.menuItemShowCiphertext = new CheckMenuItem(LanguageInfo.ContexMenu_ShowCiphertext);
			this.menuItemShowCiphertext.Toggled += this.menuItemShowCiphertext_Click;
			this.MenuItemList.Add(this.menuItemShowCiphertext);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00007498 File Offset: 0x00005698
		private void menuItemShowCiphertext_Click(object sender, EventArgs e)
		{
			using (CompositeTask.Run("show ciphertext", null))
			{
				this.passwordValue.PasswordEnable = this.menuItemShowCiphertext.Active;
				PropertyInfo property = this.TriggerButton.GetType().GetProperty("Password");
				property.SetValue(this.TriggerButton, this.passwordValue, null);
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00007518 File Offset: 0x00005718
		public override Type GetObjectType()
		{
			return typeof(TextFieldObject);
		}

		// Token: 0x04000092 RID: 146
		private TextEditMenuItem menuItemEditOccupyingText;

		// Token: 0x04000093 RID: 147
		private CheckMenuItem menuItemShowCiphertext;

		// Token: 0x04000094 RID: 148
		private PasswordValue passwordValue;

		// Token: 0x04000095 RID: 149
		private VisualObject triggerbutton;
	}
}
