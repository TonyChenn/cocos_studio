using System;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Components.Commands;
using MonoDevelop.Components.Commands.ExtensionNodes;
using MonoDevelop.Core;

namespace CocoStudio.Core.ExtensionModel
{
	// Token: 0x02000005 RID: 5
	[ExtensionNode(Description = "修改自Mono中的ItemSetCodon，修改了一个属性的名称，并添加多语言支持")]
	internal class CmdEntrySetCodon : InstanceExtensionNode
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00002EC0 File Offset: 0x000010C0
		public override object CreateInstance()
		{
			if (Platform.IsMac && this.macLabel != null)
			{
				this.label = LanguageOption.GetValueBykey(this.macLabel);
			}
			if (this.label == null)
			{
				this.label = base.Id;
			}
			this.label = LanguageOption.GetValueBykey(this.label);
			this.label = StringParserService.Parse(this.label);
			if (this.icon != null)
			{
				this.icon = CommandCodon.GetStockId(base.Addin, this.icon);
			}
			CommandEntrySet commandEntrySet = new CommandEntrySet(this.label, this.icon);
			commandEntrySet.CommandId = base.Id;
			commandEntrySet.AutoHide = this.autohide;
			foreach (object obj in base.ChildNodes)
			{
				InstanceExtensionNode instanceExtensionNode = (InstanceExtensionNode)obj;
				CommandEntry commandEntry = instanceExtensionNode.CreateInstance() as CommandEntry;
				if (commandEntry == null)
				{
					throw new InvalidOperationException("Invalid ItemSet child: " + instanceExtensionNode);
				}
				commandEntrySet.Add(commandEntry);
			}
			return commandEntrySet;
		}

		// Token: 0x0400002B RID: 43
		[NodeAttribute("label", "Label of the submenu", Localizable = true)]
		private string label;

		// Token: 0x0400002C RID: 44
		[NodeAttribute("macLabel", "Mac平台下的显示文本")]
		private string macLabel = null;

		// Token: 0x0400002D RID: 45
		[NodeAttribute("icon", "Icon of the submenu. The provided value must be a registered stock icon. A resource icon can also be specified using 'res:' as prefix for the name, for example: 'res:customIcon.png'")]
		private string icon;

		// Token: 0x0400002E RID: 46
		[NodeAttribute("autohide", "Whether the submenu should be hidden when it contains no items.")]
		private bool autohide;
	}
}
