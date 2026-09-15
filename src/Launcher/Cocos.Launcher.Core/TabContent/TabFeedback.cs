using System;
using Cocos.Launcher.Control;
using Cocos.Launcher.Core.ExtensionModel;
using CocoStudio.Core;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Cocos.Launcher.Core.TabContent
{
	[Extension(Type = typeof(ITabContent))]
	internal class TabFeedback : BaseTabContent
	{
		public override int Order
		{
			get
			{
				return 4;
			}
		}

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

		protected override void OnInitialize(ITabHead tabHead)
		{
			tabHead.HeadName = LanguageInfo.Launcher_Feedback;
			this.netWorkErrorView = new NetWorkErrorView(ConstantConfig.Constant.Feedback404);
			this.AddChildContent();
			CocoStudio.Core.Services.NetworkService.NetworkChanged += this.NetworkService_NetworkChanged;
		}

		private void NetworkService_NetworkChanged(object sender, NetworkChangedEventArgs e)
		{
			this.box.RemoveAll();
			this.AddChildContent();
		}

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

		private VBox box;

		protected NetWorkErrorView netWorkErrorView;
	}
}
