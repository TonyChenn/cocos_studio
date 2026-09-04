using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Ide;
using MonoDevelop.Projects;

namespace CocoStudio.LuaBinding
{
	public class LuaProject : Project
	{
		private const string ProjectTypeName = "Lua";

		private List<string> projectTypes = new List<string> { "Lua" };

		public LuaProject()
		{
		}

		public LuaProject(string language_name, ProjectCreateInformation info, XmlElement project_options)
		{
			if (!string.Equals(language_name, "Lua"))
			{
				throw new ArgumentException("Not a Lua project: " + language_name);
			}
			if (info != null)
			{
				Name = info.ProjectName;
			}
			CreateDefaultConfigurations();
		}

		public static LuaProject FromSingleFile(string language_name, string file_name)
		{
			ProjectCreateInformation projectCreateInformation = new ProjectCreateInformation();
			projectCreateInformation.ProjectName = Path.GetFileNameWithoutExtension(file_name);
			projectCreateInformation.SolutionPath = Path.GetDirectoryName(file_name);
			projectCreateInformation.ProjectBasePath = Path.GetDirectoryName(file_name);
			ProjectCreateInformation info = projectCreateInformation;
			LuaProject luaProject = new LuaProject(language_name, info, null);
			luaProject.AddFile(new ProjectFile(file_name));
			return luaProject;
		}

		public override SolutionItemConfiguration CreateConfiguration(string name)
		{
			return new LuaConfiguration(name);
		}

		public override bool IsCompileable(string file_name)
		{
			return file_name.ToLower().EndsWith(".lua");
		}

		protected override bool OnGetCanExecute(ExecutionContext context, ConfigurationSelector configuration)
		{
			LuaConfiguration luaConfiguration = base.DefaultConfiguration as LuaConfiguration;
			return !string.IsNullOrWhiteSpace(luaConfiguration.MainFile);
		}

		protected override BuildResult DoBuild(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			LuaConfiguration luaConfiguration = base.DefaultConfiguration as LuaConfiguration;
			if (luaConfiguration != null && luaConfiguration.LangVersion == LangVersion.GarrysMod)
			{
				monitor.ReportWarning("Can't build a with Garry's Mod Lua syntax!");
				return new BuildResult("Can't build a with Garry's Mod Lua syntax!", 0, 0);
			}
			return LuaCompilerManager.Compile(base.Items, luaConfiguration, configuration, monitor);
		}

		protected override void DoExecute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
			if (!CheckCanExecute(configuration))
			{
				return;
			}
			LuaConfiguration luaConfiguration = (LuaConfiguration)GetConfiguration(configuration);
			IConsole console = (luaConfiguration.ExternalConsole ? context.ExternalConsoleFactory.CreateConsole(!luaConfiguration.PauseConsoleOutput) : context.ConsoleFactory.CreateConsole(!luaConfiguration.PauseConsoleOutput));
			AggregatedOperationMonitor aggregatedOperationMonitor = new AggregatedOperationMonitor(monitor);
			try
			{
				string arguments = $"\"{luaConfiguration.MainFile}\" {luaConfiguration.CommandLineParameters}";
				IProcessAsyncOperation op = Runtime.ProcessService.StartConsoleProcess(GetLuaPath(luaConfiguration.LangVersion), arguments, base.BaseDirectory, luaConfiguration.EnvironmentVariables, console, null);
				monitor.CancelRequested += delegate
				{
					op.Cancel();
				};
				aggregatedOperationMonitor.AddOperation(op);
				op.WaitForCompleted();
				monitor.Log.WriteLine("The application exited with code: " + op.ExitCode);
			}
			catch (Exception exception)
			{
				monitor.ReportError(GettextCatalog.GetString("Cannot execute \"{0}\"", luaConfiguration.MainFile), exception);
			}
			finally
			{
				console.Dispose();
				aggregatedOperationMonitor.Dispose();
			}
		}

		private bool CheckCanExecute(ConfigurationSelector configuration)
		{
			LuaConfiguration luaConfiguration = (LuaConfiguration)GetConfiguration(configuration);
			FilePath luaPath = GetLuaPath(luaConfiguration.LangVersion);
			if (string.IsNullOrWhiteSpace(luaPath))
			{
				return false;
			}
			if (string.IsNullOrEmpty(luaConfiguration.MainFile))
			{
				MessageService.ShowError("Main file not set", "Main file has not been set.");
				return false;
			}
			if (!File.Exists(string.Concat(base.BaseDirectory, "/", luaConfiguration.MainFile)))
			{
				MessageService.ShowError("Main file is missing", $"The file `{luaConfiguration.MainFile}` does not exist!");
				return false;
			}
			return true;
		}

		private static FilePath GetLuaPath(LangVersion ver)
		{
			switch (ver)
			{
			case LangVersion.Lua:
				return PropertyService.Get<string>("Lua.DefaultInterpreterPath");
			case LangVersion.Lua51:
				return PropertyService.Get<string>("Lua.51InterpreterPath");
			case LangVersion.Lua52:
				return PropertyService.Get<string>("Lua.52InterpreterPath");
			case LangVersion.LuaJIT:
				return PropertyService.Get<string>("Lua.JITInterpreterPath");
			default:
				return null;
			}
		}

		protected virtual LuaExecutionCommand CreateExecutionCommand(ConfigurationSelector config_sel, LuaConfiguration configuration)
		{
			LangVersion langVersion = configuration.LangVersion;
			FilePath luaPath = GetLuaPath(langVersion);
			string arguments = "\"" + configuration.MainFile + "\" " + configuration.CommandLineParameters;
			LuaExecutionCommand luaExecutionCommand = new LuaExecutionCommand(luaPath);
			luaExecutionCommand.Arguments = arguments;
			luaExecutionCommand.WorkingDirectory = base.BaseDirectory;
			luaExecutionCommand.EnvironmentVariables = configuration.GetParsedEnvironmentVariables();
			luaExecutionCommand.Configuration = configuration;
			return luaExecutionCommand;
		}

		private void CreateDefaultConfigurations()
		{
			LuaConfiguration item = CreateConfiguration("Release") as LuaConfiguration;
			base.Configurations.Add(item);
		}

		public override IEnumerable<string> GetProjectTypes()
		{
			return projectTypes;
		}
	}
}
