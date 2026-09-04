using System;
using Microsoft.Build.Framework;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	internal class BuildEvent
	{
		public BuildStatusEventArgs EventArgs;

		public bool StartHandlerHasExecuted;

		public MDConsoleLogger ConsoleLogger;

		public void ExecuteStartedHandler()
		{
			if (!StartHandlerHasExecuted)
			{
				if (EventArgs is ProjectStartedEventArgs)
				{
					ConsoleLogger.ProjectStartedHandler(null, (ProjectStartedEventArgs)EventArgs);
				}
				else if (EventArgs is TargetStartedEventArgs)
				{
					ConsoleLogger.TargetStartedHandler(null, (TargetStartedEventArgs)EventArgs);
				}
				else if (EventArgs is TaskStartedEventArgs)
				{
					ConsoleLogger.TaskStartedHandler(null, (TaskStartedEventArgs)EventArgs);
				}
				else if (!(EventArgs is BuildStartedEventArgs))
				{
					throw new InvalidOperationException("Unexpected event on the stack, type: " + EventArgs.GetType());
				}
				StartHandlerHasExecuted = true;
			}
		}

		public void ExecuteFinishedHandler(BuildStatusEventArgs finished_args)
		{
			if (StartHandlerHasExecuted)
			{
				if (EventArgs is ProjectStartedEventArgs && finished_args is ProjectFinishedEventArgs)
				{
					ConsoleLogger.ProjectFinishedHandler(null, (ProjectFinishedEventArgs)finished_args);
				}
				else if (EventArgs is TargetStartedEventArgs && finished_args is TargetFinishedEventArgs)
				{
					ConsoleLogger.TargetFinishedHandler(null, (TargetFinishedEventArgs)finished_args);
				}
				else if (EventArgs is TaskStartedEventArgs && finished_args is TaskFinishedEventArgs)
				{
					ConsoleLogger.TaskFinishedHandler(null, (TaskFinishedEventArgs)finished_args);
				}
			}
		}
	}
}
