using System;
using Cocos.Launcher.Control;
using Cocos.Launcher.Core.ExtensionModel;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Cocos.Launcher.Core.TabContent
{
	// Token: 0x02000044 RID: 68
	[Extension(Type = typeof(ITabContent))]
	internal class TabFeedback : BaseTabContent
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00009D16 File Offset: 0x00007F16
		public override int Order
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000255 RID: 597 RVA: 0x00009D19 File Offset: 0x00007F19
		public override Widget Content
		{
			get
			{
				if (this.box == null)
				{
					this.box = new VBox();
				}
				return this.box;
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00009D34 File Offset: 0x00007F34
		protected override void OnInitialize(ITabHead tabHead)
		{
			tabHead.HeadName = LanguageInfo.Launcher_Feedback;
			this.netWorkErrorView = new NetWorkErrorView(ConstantConfig.Constant.Feedback404);
			this.AddChildContent();
			CocoStudio.Core.Services.NetworkService.NetworkChanged += this.NetworkService_NetworkChanged;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00009D72 File Offset: 0x00007F72
		private void NetworkService_NetworkChanged(object sender, NetworkChangedEventArgs e)
		{
			this.box.RemoveAll();
			this.AddChildContent();
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00009D88 File Offset: 0x00007F88
		private void AddChildContent()
		{
			if (this.box == null)
			{
				this.box = new VBox();
			}
			if (this.box.Children.Length > 0)
			{
				return;
			}
			if (CocoStudio.Core.Services.NetworkService.IsOK)
			{
				this.box.Add(new FeedbackView());
			}
			else
			{
				this.box.Add(this.netWorkErrorView);
			}
			this.box.ShowAll();
		}

		// Token: 0x040000EA RID: 234
		private VBox box;

		// Token: 0x040000EB RID: 235
		protected NetWorkErrorView netWorkErrorView;
	}
}
