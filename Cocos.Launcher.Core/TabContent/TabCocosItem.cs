using System;
using Cocos.Launcher.Core.ExtensionModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Cocos.Launcher.Core.TabContent
{
	// Token: 0x02000046 RID: 70
	[Extension(Type = typeof(ITabContent))]
	internal class TabCocosItem : BaseTabContent
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600025F RID: 607 RVA: 0x00009E76 File Offset: 0x00008076
		public override int Order
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00009E79 File Offset: 0x00008079
		public override Widget Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00009E81 File Offset: 0x00008081
		protected override void OnInitialize(ITabHead tabHead)
		{
			tabHead.HeadName = LanguageInfo.Launcher_Projects;
			this.content = new CocosItemView();
		}

		// Token: 0x040000ED RID: 237
		private Widget content;
	}
}
