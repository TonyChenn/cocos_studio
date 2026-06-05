using System;
using System.CodeDom.Compiler;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000157 RID: 343
	internal class CustomCommandExtension : ProjectServiceExtension
	{
		// Token: 0x06000CA9 RID: 3241 RVA: 0x0002E838 File Offset: 0x0002CA38
		protected override BuildResult Build(IProgressMonitor monitor, SolutionEntityItem entry, ConfigurationSelector configuration)
		{
			SolutionItemConfiguration configuration2 = entry.GetConfiguration(configuration);
			if (configuration2 != null)
			{
				if (configuration2.CustomCommands.CanExecute(entry, CustomCommandType.BeforeBuild, null, configuration))
				{
					configuration2.CustomCommands.ExecuteCommand(monitor, entry, CustomCommandType.BeforeBuild, configuration);
				}
				if (monitor.IsCancelRequested)
				{
					return new BuildResult(new CompilerResults(null), "");
				}
			}
			BuildResult buildResult = base.Build(monitor, entry, configuration);
			if (configuration2 != null && !monitor.IsCancelRequested && !buildResult.Failed && configuration2.CustomCommands.CanExecute(entry, CustomCommandType.AfterBuild, null, configuration))
			{
				configuration2.CustomCommands.ExecuteCommand(monitor, entry, CustomCommandType.AfterBuild, configuration);
			}
			return buildResult;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0002E8C8 File Offset: 0x0002CAC8
		protected override void Clean(IProgressMonitor monitor, SolutionEntityItem entry, ConfigurationSelector configuration)
		{
			SolutionItemConfiguration configuration2 = entry.GetConfiguration(configuration);
			if (configuration2 != null)
			{
				if (configuration2.CustomCommands.CanExecute(entry, CustomCommandType.BeforeClean, null, configuration))
				{
					configuration2.CustomCommands.ExecuteCommand(monitor, entry, CustomCommandType.BeforeClean, configuration);
				}
				if (monitor.IsCancelRequested)
				{
					return;
				}
			}
			base.Clean(monitor, entry, configuration);
			if (configuration2 != null && !monitor.IsCancelRequested && configuration2.CustomCommands.CanExecute(entry, CustomCommandType.AfterClean, null, configuration))
			{
				configuration2.CustomCommands.ExecuteCommand(monitor, entry, CustomCommandType.AfterClean, configuration);
			}
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0002E940 File Offset: 0x0002CB40
		protected override void Execute(IProgressMonitor monitor, SolutionEntityItem entry, ExecutionContext context, ConfigurationSelector configuration)
		{
			SolutionItemConfiguration configuration2 = entry.GetConfiguration(configuration);
			if (configuration2 != null)
			{
				ExecutionContext context2 = new ExecutionContext(Runtime.ProcessService.DefaultExecutionHandler, context.ConsoleFactory, context.ExecutionTarget);
				if (configuration2.CustomCommands.CanExecute(entry, CustomCommandType.BeforeExecute, context2, configuration))
				{
					configuration2.CustomCommands.ExecuteCommand(monitor, entry, CustomCommandType.BeforeExecute, context2, configuration);
				}
				if (monitor.IsCancelRequested)
				{
					return;
				}
			}
			base.Execute(monitor, entry, context, configuration);
			if (configuration2 != null && !monitor.IsCancelRequested)
			{
				ExecutionContext context3 = new ExecutionContext(Runtime.ProcessService.DefaultExecutionHandler, context.ConsoleFactory, context.ExecutionTarget);
				if (configuration2.CustomCommands.CanExecute(entry, CustomCommandType.AfterExecute, context3, configuration))
				{
					configuration2.CustomCommands.ExecuteCommand(monitor, entry, CustomCommandType.AfterExecute, context3, configuration);
				}
			}
		}
	}
}
