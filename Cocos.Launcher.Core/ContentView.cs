using System;
using System.ComponentModel;
using Cocos.Launcher.Core.View;
using Gtk;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200004E RID: 78
	[ToolboxItem(true)]
	public class ContentView : EventBox, ITabMain
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000A691 File Offset: 0x00008891
		public Widget ContainerWidget
		{
			get
			{
				return this.containerEventBox;
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000A699 File Offset: 0x00008899
		public ContentView()
		{
			this.Initialize();
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000A6A8 File Offset: 0x000088A8
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

		// Token: 0x0600029C RID: 668 RVA: 0x0000A758 File Offset: 0x00008958
		public void ChangeTabContent(ITabContent tabContent)
		{
			this.containerEventBox.RemoveAll();
			this.containerEventBox.Add(tabContent.Content);
			this.containerEventBox.ShowAll();
		}

		// Token: 0x040000FC RID: 252
		private ITabGroup tabGroup;

		// Token: 0x040000FD RID: 253
		private HBox rightBox;

		// Token: 0x040000FE RID: 254
		private EventBox containerEventBox;

		// Token: 0x040000FF RID: 255
		private OutputView outputView;
	}
}
