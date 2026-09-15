using System;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using Gtk;

namespace Modules.UI.MainTool
{
	internal abstract class BaseToolbarWidget : IToolbarWidget
	{
		public abstract Widget GtkWidget { get; }

		public virtual void OnProjectChanged(ProjectsOperations.ProjectEventArgs args)
		{
		}

		public virtual void OnSolutionChanged(SolutionEventArgs args)
		{
		}

		public virtual void OnSolutionClosed(SolutionEventArgs args)
		{
		}
	}
}
