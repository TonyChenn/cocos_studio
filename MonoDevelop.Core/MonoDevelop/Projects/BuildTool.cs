using System;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.ProgressMonitoring;

namespace MonoDevelop.Projects
{
	// Token: 0x0200014C RID: 332
	internal class BuildTool : IApplication
	{
		// Token: 0x06000C54 RID: 3156 RVA: 0x0002D80C File Offset: 0x0002BA0C
		public int Run(string[] arguments)
		{
			Console.WriteLine(BrandingService.BrandApplicationName("MonoDevelop Build Tool"));
			foreach (string argument in arguments)
			{
				this.ReadArgument(argument);
			}
			if (this.help)
			{
				Console.WriteLine("build [options] [build-file]");
				Console.WriteLine("-p --project:PROJECT  Name of the project to build.");
				Console.WriteLine("-t --target:TARGET    Name of the target: Build or Clean.");
				Console.WriteLine("-c --configuration:CONFIGURATION  Name of the solution configuration to build.");
				Console.WriteLine("-r --runtime:PREFIX   Prefix of the Mono runtime to build against.");
				Console.WriteLine();
				Console.WriteLine("Supported targets:");
				Console.WriteLine("  {0}: build the project (the default target).", "Build");
				Console.WriteLine("  {0}: clean the project.", "Clean");
				Console.WriteLine();
				return 0;
			}
			string text = null;
			string text2 = null;
			if (this.file == null)
			{
				string[] files = Directory.GetFiles(".");
				foreach (string text3 in files)
				{
					if (Services.ProjectService.IsWorkspaceItemFile(text3))
					{
						text = text3;
						break;
					}
					if (text2 == null && Services.ProjectService.IsSolutionItemFile(text3))
					{
						text2 = text3;
					}
				}
				if (text == null && text2 == null)
				{
					Console.WriteLine("Project file not found.");
					return 1;
				}
			}
			else if (Services.ProjectService.IsWorkspaceItemFile(this.file))
			{
				text = this.file;
			}
			else
			{
				if (!Services.ProjectService.IsSolutionItemFile(this.file))
				{
					Console.WriteLine("File '{0}' is not a project or solution.", this.file);
					return 1;
				}
				text2 = this.file;
			}
			IProgressMonitor monitor = new ConsoleProjectLoadProgressMonitor(new ConsoleProgressMonitor());
			TargetRuntime targetRuntime = null;
			TargetRuntime defaultRuntime = Runtime.SystemAssemblyService.DefaultRuntime;
			if (this.runtime != null)
			{
				targetRuntime = MonoTargetRuntimeFactory.RegisterRuntime(new MonoRuntimeInfo(this.runtime));
				if (targetRuntime != null)
				{
					Runtime.SystemAssemblyService.DefaultRuntime = targetRuntime;
				}
			}
			IBuildTarget buildTarget;
			if (text != null)
			{
				buildTarget = Services.ProjectService.ReadWorkspaceItem(monitor, text);
			}
			else
			{
				buildTarget = Services.ProjectService.ReadSolutionItem(monitor, text2);
			}
			int result;
			using (buildTarget)
			{
				if (this.project != null)
				{
					Solution solution = buildTarget as Solution;
					buildTarget = null;
					if (solution != null)
					{
						buildTarget = solution.FindProjectByName(this.project);
					}
					if (buildTarget == null)
					{
						Console.WriteLine("The project '" + this.project + "' could not be found in " + this.file);
						return 1;
					}
				}
				IConfigurationTarget configurationTarget = buildTarget as IConfigurationTarget;
				if (this.config == null && configurationTarget != null)
				{
					this.config = configurationTarget.DefaultConfigurationId;
				}
				monitor = new ConsoleProgressMonitor();
				BuildResult buildResult = null;
				if (buildTarget is SolutionEntityItem && ((SolutionEntityItem)buildTarget).ParentSolution == null)
				{
					ConfigurationSelector configuration = new ItemConfigurationSelector(this.config);
					buildResult = buildTarget.RunTarget(monitor, this.command, configuration);
				}
				else
				{
					ConfigurationSelector configurationSelector = new SolutionConfigurationSelector(this.config);
					SolutionEntityItem solutionEntityItem = buildTarget as SolutionEntityItem;
					if (solutionEntityItem != null)
					{
						if (this.command == "Build")
						{
							buildResult = solutionEntityItem.Build(monitor, configurationSelector, true);
						}
						else if (this.command == "Clean")
						{
							solutionEntityItem.Clean(monitor, configurationSelector);
						}
						else
						{
							buildResult = buildTarget.RunTarget(monitor, this.command, configurationSelector);
						}
					}
					else
					{
						buildResult = buildTarget.RunTarget(monitor, this.command, configurationSelector);
					}
				}
				if (targetRuntime != null)
				{
					Runtime.SystemAssemblyService.DefaultRuntime = defaultRuntime;
					MonoTargetRuntimeFactory.UnregisterRuntime((MonoTargetRuntime)targetRuntime);
				}
				if (buildResult != null)
				{
					foreach (BuildError value in buildResult.Errors)
					{
						Console.Error.WriteLine(value);
					}
				}
				result = ((buildResult == null || buildResult.ErrorCount == 0) ? 0 : 1);
			}
			return result;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0002DBCC File Offset: 0x0002BDCC
		private void ReadArgument(string argument)
		{
			string text;
			if (argument.StartsWith("--"))
			{
				text = argument.Substring(2);
			}
			else
			{
				if ((!argument.StartsWith("/") && !argument.StartsWith("-")) || File.Exists(argument))
				{
					this.file = argument;
					return;
				}
				text = argument.Substring(1);
			}
			int num = text.IndexOf(':');
			string text2;
			string value;
			if (num > 0)
			{
				text2 = text.Substring(0, num);
				value = text.Substring(num + 1);
			}
			else
			{
				text2 = text;
				value = null;
			}
			string key;
			switch (key = text2)
			{
			case "f":
			case "buildfile":
				this.file = value;
				return;
			case "help":
			case "?":
				this.help = true;
				return;
			case "p":
			case "project":
				if (string.IsNullOrEmpty(value))
				{
					throw new Exception("Project name not specified (syntax is: -p:PROJECT)");
				}
				this.project = value;
				return;
			case "c":
			case "configuration":
				if (string.IsNullOrEmpty(value))
				{
					throw new Exception("Configuration name not specified (syntax is: -c:CONFIGURATION)");
				}
				this.config = value;
				return;
			case "t":
			case "target":
				if (string.IsNullOrEmpty(value))
				{
					throw new Exception("Target name not specified (syntax is: -t:TARGET)");
				}
				this.command = value;
				return;
			case "r":
			case "runtime":
				if (string.IsNullOrEmpty(value))
				{
					throw new Exception("Runtime prefix not specified (syntax is: -r:PREFIX)");
				}
				this.runtime = value;
				return;
			}
			throw new Exception("Unknown option '" + text2 + "'");
		}

		// Token: 0x040003AA RID: 938
		private bool help;

		// Token: 0x040003AB RID: 939
		private string file;

		// Token: 0x040003AC RID: 940
		private string project;

		// Token: 0x040003AD RID: 941
		private string config;

		// Token: 0x040003AE RID: 942
		private string command = "Build";

		// Token: 0x040003AF RID: 943
		private string runtime;
	}
}
