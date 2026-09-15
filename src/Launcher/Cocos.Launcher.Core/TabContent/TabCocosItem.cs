using System;
using Cocos.Launcher.Core.ExtensionModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Cocos.Launcher.Core.TabContent
{
	[Extension(Type = typeof(ITabContent))]
	internal class TabCocosItem : BaseTabContent
	{
		public override int Order
		{
			get
			{
				return 0;
			}
		}

		public override Widget Content
		{
			get
			{
				return this.content;
			}
		}

		protected override void OnInitialize(ITabHead tabHead)
		{
			tabHead.HeadName = LanguageInfo.Launcher_Projects;
			this.content = new CocosItemView();
		}

		private Widget content;
	}
}
