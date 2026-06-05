using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Core.StringParsing;

namespace MonoDevelop.Projects
{
	// Token: 0x02000152 RID: 338
	public class CustomCommand
	{
		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x0002E0CC File Offset: 0x0002C2CC
		public Dictionary<string, string> EnvironmentVariables
		{
			get
			{
				return this.environmentVariables;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x0002E0D4 File Offset: 0x0002C2D4
		// (set) Token: 0x06000C88 RID: 3208 RVA: 0x0002E0DC File Offset: 0x0002C2DC
		public CustomCommandType Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x0002E0E5 File Offset: 0x0002C2E5
		// (set) Token: 0x06000C8A RID: 3210 RVA: 0x0002E0ED File Offset: 0x0002C2ED
		public string Command
		{
			get
			{
				return this.command;
			}
			set
			{
				this.command = value;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x0002E0F6 File Offset: 0x0002C2F6
		// (set) Token: 0x06000C8C RID: 3212 RVA: 0x0002E0FE File Offset: 0x0002C2FE
		public string WorkingDir
		{
			get
			{
				return this.workingdir;
			}
			set
			{
				this.workingdir = value;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0002E107 File Offset: 0x0002C307
		// (set) Token: 0x06000C8E RID: 3214 RVA: 0x0002E10F File Offset: 0x0002C30F
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x0002E118 File Offset: 0x0002C318
		// (set) Token: 0x06000C90 RID: 3216 RVA: 0x0002E120 File Offset: 0x0002C320
		public bool ExternalConsole
		{
			get
			{
				return this.externalConsole;
			}
			set
			{
				this.externalConsole = value;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x0002E129 File Offset: 0x0002C329
		// (set) Token: 0x06000C92 RID: 3218 RVA: 0x0002E131 File Offset: 0x0002C331
		public bool PauseExternalConsole
		{
			get
			{
				return this.pauseExternalConsole;
			}
			set
			{
				this.pauseExternalConsole = value;
			}
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0002E13C File Offset: 0x0002C33C
		public string GetCommandFile(IWorkspaceObject entry, ConfigurationSelector configuration)
		{
			StringTagModel tagModel = this.GetTagModel(entry, configuration);
			string result;
			string text;
			this.ParseCommand(tagModel, out result, out text);
			return result;
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0002E160 File Offset: 0x0002C360
		public string GetCommandArgs(IWorkspaceObject entry, ConfigurationSelector configuration)
		{
			StringTagModel tagModel = this.GetTagModel(entry, configuration);
			string text;
			string result;
			this.ParseCommand(tagModel, out text, out result);
			return result;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0002E184 File Offset: 0x0002C384
		public FilePath GetCommandWorkingDir(IWorkspaceObject entry, ConfigurationSelector configuration)
		{
			StringTagModel tagModel = this.GetTagModel(entry, configuration);
			if (string.IsNullOrEmpty(this.workingdir))
			{
				return entry.BaseDirectory;
			}
			return StringParserService.Parse(this.workingdir, tagModel).ToAbsolute(entry.BaseDirectory);
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0002E1D0 File Offset: 0x0002C3D0
		public CustomCommand Clone()
		{
			return new CustomCommand
			{
				command = this.command,
				workingdir = this.workingdir,
				name = this.name,
				externalConsole = this.externalConsole,
				pauseExternalConsole = this.pauseExternalConsole,
				type = this.type
			};
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0002E22C File Offset: 0x0002C42C
		private StringTagModel GetTagModel(IWorkspaceObject entry, ConfigurationSelector configuration)
		{
			if (entry is SolutionItem)
			{
				return ((SolutionItem)entry).GetStringTagModel(configuration);
			}
			if (entry is WorkspaceItem)
			{
				return ((WorkspaceItem)entry).GetStringTagModel();
			}
			return new StringTagModel();
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0002E25C File Offset: 0x0002C45C
		private void ParseCommand(StringTagModel tagSource, out string cmd, out string args)
		{
			if (this.command.Length > 0 && this.command[0] == '"')
			{
				int num = this.command.IndexOf('"', 1);
				if (num != -1)
				{
					cmd = this.command.Substring(1, num - 1);
					args = this.command.Substring(num + 1).Trim();
				}
				else
				{
					cmd = this.command;
					args = string.Empty;
				}
			}
			else
			{
				int num2 = this.command.IndexOf(' ');
				if (num2 != -1)
				{
					cmd = this.command.Substring(0, num2);
					args = this.command.Substring(num2 + 1).Trim();
				}
				else
				{
					cmd = this.command;
					args = string.Empty;
				}
			}
			cmd = StringParserService.Parse(cmd, tagSource);
			args = StringParserService.Parse(args, tagSource);
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0002E330 File Offset: 0x0002C530
		public ProcessExecutionCommand CreateExecutionCommand(IWorkspaceObject entry, ConfigurationSelector configuration)
		{
			StringTagModel tagModel = this.GetTagModel(entry, configuration);
			string text;
			string arguments;
			this.ParseCommand(tagModel, out text, out arguments);
			if (!Path.IsPathRooted(text))
			{
				string text2 = text.ToAbsolute(entry.BaseDirectory).FullPath;
				if (File.Exists(text2))
				{
					text = text2;
				}
			}
			ProcessExecutionCommand processExecutionCommand = Runtime.ProcessService.CreateCommand(text);
			processExecutionCommand.Arguments = arguments;
			FilePath filePath = this.workingdir;
			if (!filePath.IsNullOrEmpty)
			{
				filePath = StringParserService.Parse(filePath, tagModel);
			}
			processExecutionCommand.WorkingDirectory = (filePath.IsNullOrEmpty ? entry.BaseDirectory : filePath.ToAbsolute(entry.BaseDirectory));
			if (this.environmentVariables != null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				foreach (KeyValuePair<string, string> keyValuePair in this.environmentVariables)
				{
					dictionary[keyValuePair.Key] = StringParserService.Parse(keyValuePair.Value, tagModel);
				}
				processExecutionCommand.EnvironmentVariables = dictionary;
			}
			return processExecutionCommand;
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0002E468 File Offset: 0x0002C668
		public void Execute(IProgressMonitor monitor, IWorkspaceObject entry, ConfigurationSelector configuration)
		{
			this.Execute(monitor, entry, null, configuration);
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x0002E474 File Offset: 0x0002C674
		public bool CanExecute(IWorkspaceObject entry, ExecutionContext context, ConfigurationSelector configuration)
		{
			if (string.IsNullOrEmpty(this.command))
			{
				return false;
			}
			if (context == null)
			{
				return true;
			}
			ProcessExecutionCommand processExecutionCommand = this.CreateExecutionCommand(entry, configuration);
			return context.ExecutionHandler.CanExecute(processExecutionCommand);
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0002E4AC File Offset: 0x0002C6AC
		public void Execute(IProgressMonitor monitor, IWorkspaceObject entry, ExecutionContext context, ConfigurationSelector configuration)
		{
			ProcessExecutionCommand processExecutionCommand = this.CreateExecutionCommand(entry, configuration);
			monitor.Log.WriteLine(GettextCatalog.GetString("Executing: {0} {1}", processExecutionCommand.Command, processExecutionCommand.Arguments));
			if (!Directory.Exists(processExecutionCommand.WorkingDirectory))
			{
				monitor.ReportError(GettextCatalog.GetString("Custom command working directory does not exist"), null);
				return;
			}
			AggregatedOperationMonitor aggregatedOperationMonitor = null;
			IProcessAsyncOperation processAsyncOperation = null;
			IConsole console = null;
			try
			{
				if (context != null)
				{
					if (this.externalConsole)
					{
						console = context.ExternalConsoleFactory.CreateConsole(!this.pauseExternalConsole);
					}
					else
					{
						console = context.ConsoleFactory.CreateConsole(!this.pauseExternalConsole);
					}
					processAsyncOperation = context.ExecutionHandler.Execute(processExecutionCommand, console);
				}
				else if (this.externalConsole)
				{
					console = ExternalConsoleFactory.Instance.CreateConsole(!this.pauseExternalConsole);
					processAsyncOperation = Runtime.ProcessService.StartConsoleProcess(processExecutionCommand.Command, processExecutionCommand.Arguments, processExecutionCommand.WorkingDirectory, console, null);
				}
				else
				{
					processAsyncOperation = Runtime.ProcessService.StartProcess(processExecutionCommand.Command, processExecutionCommand.Arguments, processExecutionCommand.WorkingDirectory, monitor.Log, monitor.Log, null, false);
				}
				aggregatedOperationMonitor = new AggregatedOperationMonitor(monitor, new IAsyncOperation[]
				{
					processAsyncOperation
				});
				processAsyncOperation.WaitForCompleted();
				if (!processAsyncOperation.Success)
				{
					monitor.ReportError("Custom command failed (exit code: " + processAsyncOperation.ExitCode + ")", null);
				}
			}
			catch (Win32Exception ex)
			{
				monitor.ReportError(GettextCatalog.GetString("Failed to execute custom command '{0}': {1}", processExecutionCommand.Command, ex.Message), null);
			}
			catch (Exception ex2)
			{
				LoggingService.LogError("Command execution failed", ex2);
				throw new UserException(GettextCatalog.GetString("Command execution failed: {0}", ex2.Message));
			}
			finally
			{
				if (processAsyncOperation == null || !processAsyncOperation.Success)
				{
					monitor.AsyncOperation.Cancel();
				}
				if (processAsyncOperation != null)
				{
					processAsyncOperation.Dispose();
				}
				if (console != null)
				{
					console.Dispose();
				}
				if (aggregatedOperationMonitor != null)
				{
					aggregatedOperationMonitor.Dispose();
				}
			}
		}

		// Token: 0x040003BB RID: 955
		[ItemProperty]
		private CustomCommandType type;

		// Token: 0x040003BC RID: 956
		[ItemProperty(DefaultValue = "")]
		private string name = "";

		// Token: 0x040003BD RID: 957
		[ItemProperty]
		private string command;

		// Token: 0x040003BE RID: 958
		[ItemProperty]
		private string workingdir;

		// Token: 0x040003BF RID: 959
		[ItemProperty(DefaultValue = false)]
		private bool externalConsole;

		// Token: 0x040003C0 RID: 960
		[ItemProperty(DefaultValue = false)]
		private bool pauseExternalConsole;

		// Token: 0x040003C1 RID: 961
		[ItemProperty("name", Scope = "key")]
		[ItemProperty("EnvironmentVariables", SkipEmpty = true)]
		[ItemProperty("Variable", Scope = "item")]
		[ItemProperty("value", Scope = "value")]
		private Dictionary<string, string> environmentVariables = new Dictionary<string, string>();
	}
}
