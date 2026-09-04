using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using Gtk;
using Modules.UI.MainTool.View;
using Mono.Addins;
using Stetic;

namespace Modules.UI.MainTool
{
	// Token: 0x02000016 RID: 22
	[ToolboxItem(true)]
	[Extension(Path = "/CocoStudio/Ide/MainToolbar")]
	public class MainToolUC : Bin
	{
		// Token: 0x06000079 RID: 121 RVA: 0x0000429F File Offset: 0x0000249F
		public MainToolUC()
		{
			this.Build();
			this.InitControls();
			this.InitEvent();
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000042CC File Offset: 0x000024CC
		private void InitControls()
		{
			this.evtbx_bg.ModifyBg(StateType.Normal, WindowStyle.WindowBgColor);
			IToolbarWidget toolbarWidget = new NewButtonWidget();
			this.toolbarWidgetList.Add(toolbarWidget);
			IToolbarWidget toolbarWidget2 = new CanvasWidget();
			this.toolbarWidgetList.Add(toolbarWidget2);
			IToolbarWidget toolbarWidget3 = new SimulatorButtonWidget();
			this.toolbarWidgetList.Add(toolbarWidget3);
			IToolbarWidget toolbarWidget4 = new PublishPackageWidget();
			this.toolbarWidgetList.Add(toolbarWidget4);
			IToolbarWidget toolbarWidget5 = new RunComboBoxWidget();
			this.toolbarWidgetList.Add(toolbarWidget5);
			this.toolbarExtend = new ToolbarExtend();
			this.hbox_main.PackStart(toolbarWidget.GtkWidget, false, false, 0U);
			this.hbox_main.PackStart(new VSeparator(), false, false, 0U);
			this.hbox_main.PackStart(toolbarWidget2.GtkWidget, false, false, 0U);
			this.hbox_main.PackStart(new VSeparator(), false, false, 0U);
			this.hbox_main.PackStart(toolbarWidget3.GtkWidget, false, false, 0U);
			this.hbox_main.PackStart(toolbarWidget4.GtkWidget, false, false, 0U);
			this.hbox_main.PackStart(toolbarWidget5.GtkWidget, false, false, 0U);
			this.hbox_main.PackStart(this.toolbarExtend, false, false, 0U);
			base.ShowAll();
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004408 File Offset: 0x00002608
		private void InitEvent()
		{
			Services.ProjectOperations.CurrentProjectChanged += this.CurrentProjectChangedHandler;
			Services.ProjectOperations.CurrentSelectedSolutionChanged += this.CurrentSolutionChangedHandler;
			Services.ProjectOperations.CurrentSelectedSolutionClosed += this.CurrentSolutionClosedHandler;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000445C File Offset: 0x0000265C
		private void CurrentProjectChangedHandler(object sender, ProjectsOperations.ProjectEventArgs e)
		{
			this.toolbarExtend.OnProjectChanged(e.Project);
			foreach (IToolbarWidget toolbarWidget in this.toolbarWidgetList)
			{
				toolbarWidget.OnProjectChanged(e);
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000044CC File Offset: 0x000026CC
		private void CurrentSolutionClosedHandler(object sender, SolutionEventArgs e)
		{
			foreach (IToolbarWidget toolbarWidget in this.toolbarWidgetList)
			{
				toolbarWidget.OnSolutionClosed(e);
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004528 File Offset: 0x00002728
		private void CurrentSolutionChangedHandler(object sender, SolutionEventArgs e)
		{
			foreach (IToolbarWidget toolbarWidget in this.toolbarWidgetList)
			{
				toolbarWidget.OnSolutionChanged(e);
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004584 File Offset: 0x00002784
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.UI.MainTool.MainToolUC";
			this.evtbx_bg = new EventBox();
			this.evtbx_bg.Name = "evtbx_bg";
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.LeftPadding = 8U;
			this.alignment_main.TopPadding = 4U;
			this.alignment_main.RightPadding = 8U;
			this.hbox_main = new HBox();
			this.hbox_main.HeightRequest = 24;
			this.hbox_main.Name = "hbox_main";
			this.hbox_main.Spacing = 6;
			this.alignment_main.Add(this.hbox_main);
			this.evtbx_bg.Add(this.alignment_main);
			base.Add(this.evtbx_bg);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		// Token: 0x04000033 RID: 51
		private List<IToolbarWidget> toolbarWidgetList = new List<IToolbarWidget>();

		// Token: 0x04000034 RID: 52
		private ToolbarExtend toolbarExtend;

		// Token: 0x04000035 RID: 53
		private EventBox evtbx_bg;

		// Token: 0x04000036 RID: 54
		private Alignment alignment_main;

		// Token: 0x04000037 RID: 55
		private HBox hbox_main;
	}
}
