using System;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using Gtk;

namespace Modules.UI.MainTool
{
	internal interface IToolbarWidget
	{
		Widget GtkWidget { get; }

		void OnProjectChanged(ProjectsOperations.ProjectEventArgs args);

		void OnSolutionChanged(SolutionEventArgs args);

		void OnSolutionClosed(SolutionEventArgs args);
	}
}
