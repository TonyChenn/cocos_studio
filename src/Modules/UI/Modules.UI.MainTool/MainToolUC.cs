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
	[ToolboxItem(true)]
	[Extension(Path = "/CocoStudio/Ide/MainToolbar")]
	public class MainToolUC : Bin
	{
		public MainToolUC()
		{
			this.Build();
			this.InitControls();
			this.InitEvent();
		}

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

		private void InitEvent()
		{
			Services.ProjectOperations.CurrentProjectChanged += this.CurrentProjectChangedHandler;
			Services.ProjectOperations.CurrentSelectedSolutionChanged += this.CurrentSolutionChangedHandler;
			Services.ProjectOperations.CurrentSelectedSolutionClosed += this.CurrentSolutionClosedHandler;
		}

		private void CurrentProjectChangedHandler(object sender, ProjectsOperations.ProjectEventArgs e)
		{
			this.toolbarExtend.OnProjectChanged(e.Project);
			foreach (IToolbarWidget toolbarWidget in this.toolbarWidgetList)
			{
				toolbarWidget.OnProjectChanged(e);
			}
		}

		private void CurrentSolutionClosedHandler(object sender, SolutionEventArgs e)
		{
			foreach (IToolbarWidget toolbarWidget in this.toolbarWidgetList)
			{
				toolbarWidget.OnSolutionClosed(e);
			}
		}

		private void CurrentSolutionChangedHandler(object sender, SolutionEventArgs e)
		{
			foreach (IToolbarWidget toolbarWidget in this.toolbarWidgetList)
			{
				toolbarWidget.OnSolutionChanged(e);
			}
		}

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

		private List<IToolbarWidget> toolbarWidgetList = new List<IToolbarWidget>();

		private ToolbarExtend toolbarExtend;

		private EventBox evtbx_bg;

		private Alignment alignment_main;

		private HBox hbox_main;
	}
}
