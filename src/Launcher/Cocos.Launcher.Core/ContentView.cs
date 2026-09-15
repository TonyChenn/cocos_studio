using System;
using System.ComponentModel;
using Cocos.Launcher.Core.View;
using Gtk;

namespace Cocos.Launcher.Core
{
	[ToolboxItem(true)]
	public class ContentView : EventBox, ITabMain
	{
		public Widget ContainerWidget
		{
			get
			{
				return this.containerEventBox;
			}
		}

		public ContentView()
		{
			this.Initialize();
		}

		private void Initialize()
		{
			HBox hbox = new HBox();
			this.tabGroup = new TabGroup(this);
			Services.TabGroupService = this.tabGroup;
			VBox vbox = new VBox();
			vbox.PackStart(this.tabGroup.Content, true, true, 0U);
			hbox.PackStart(vbox, false, false, 0U);
			this.containerEventBox = new EventBox();
			this.rightBox = new HBox();
			this.rightBox.PackStart(this.containerEventBox, true, true, 0U);
			hbox.PackStart(this.rightBox, true, true, 0U);
			this.outputView = new OutputView();
			this.outputView.SetParentWidget(this.rightBox);
			base.Add(hbox);
			base.ShowAll();
		}

		public void ChangeTabContent(ITabContent tabContent)
		{
			this.containerEventBox.RemoveAll();
			this.containerEventBox.Add(tabContent.Content);
			this.containerEventBox.ShowAll();
		}

		private ITabGroup tabGroup;

		private HBox rightBox;

		private EventBox containerEventBox;

		private OutputView outputView;
	}
}
