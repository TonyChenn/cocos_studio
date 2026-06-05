using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Mdb;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.ProgressMonitoring;

namespace MonoDevelop.Projects
{
	// Token: 0x0200018A RID: 394
	public class CompiledAssemblyProject : Project, IAssemblyProject
	{
		// Token: 0x06000F56 RID: 3926 RVA: 0x00039A4F File Offset: 0x00037C4F
		public CompiledAssemblyProject()
		{
			base.AddNewConfiguration("Default");
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00039B2C File Offset: 0x00037D2C
		public override IEnumerable<string> GetProjectTypes()
		{
			yield return "CompiledAssembly";
			yield break;
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000F58 RID: 3928 RVA: 0x00039B49 File Offset: 0x00037D49
		public override IconId StockIcon
		{
			get
			{
				return "md-assembly-project";
			}
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x00039B55 File Offset: 0x00037D55
		public override SolutionItemConfiguration CreateConfiguration(string name)
		{
			return new ProjectConfiguration(name);
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000F5A RID: 3930 RVA: 0x00039B5D File Offset: 0x00037D5D
		public TargetFramework TargetFramework
		{
			get
			{
				return this.targetFramework;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x00039B65 File Offset: 0x00037D65
		public MonoDevelop.Core.Assemblies.TargetRuntime TargetRuntime
		{
			get
			{
				return Runtime.SystemAssemblyService.DefaultRuntime;
			}
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00039B74 File Offset: 0x00037D74
		public void LoadFrom(FilePath assemblyPath)
		{
			this.FileName = assemblyPath;
			TargetFrameworkMoniker targetFrameworkForAssembly = Runtime.SystemAssemblyService.GetTargetFrameworkForAssembly(Runtime.SystemAssemblyService.DefaultRuntime, assemblyPath);
			if (targetFrameworkForAssembly != null)
			{
				this.targetFramework = Runtime.SystemAssemblyService.GetTargetFramework(targetFrameworkForAssembly);
			}
			AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyPath);
			MdbReaderProvider mdbReaderProvider = new MdbReaderProvider();
			try
			{
				ISymbolReader symbolReader = mdbReaderProvider.GetSymbolReader(assemblyDefinition.MainModule, assemblyPath);
				assemblyDefinition.MainModule.ReadSymbols(symbolReader);
			}
			catch
			{
			}
			HashSet<FilePath> hashSet = new HashSet<FilePath>();
			foreach (TypeDefinition typeDefinition in assemblyDefinition.MainModule.Types)
			{
				foreach (MethodDefinition methodDefinition in typeDefinition.Methods)
				{
					if (methodDefinition.HasBody && methodDefinition.Body.Instructions != null && methodDefinition.Body.Instructions.Count > 0)
					{
						SequencePoint sequencePoint = methodDefinition.Body.Instructions[0].SequencePoint;
						if (sequencePoint != null)
						{
							hashSet.Add(sequencePoint.Document.Url);
						}
					}
				}
			}
			FilePath filePath = FilePath.Empty;
			foreach (FilePath filePath2 in hashSet)
			{
				base.AddFile(filePath2, "Compile");
				if (filePath.IsNullOrEmpty)
				{
					filePath = filePath2.ParentDirectory;
				}
				else if (!filePath2.IsChildPathOf(filePath))
				{
					filePath = this.FindCommonRoot(filePath, filePath2);
				}
			}
			if (!filePath.IsNullOrEmpty)
			{
				base.BaseDirectory = filePath;
			}
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00039D80 File Offset: 0x00037F80
		private FilePath FindCommonRoot(FilePath p1, FilePath p2)
		{
			string[] array = p1.ToString().Split(new char[]
			{
				Path.DirectorySeparatorChar
			});
			string[] array2 = p2.ToString().Split(new char[]
			{
				Path.DirectorySeparatorChar
			});
			int num = 0;
			while (num < array.Length && num < array2.Length && array[num] == array2[num])
			{
				num++;
			}
			return string.Join(Path.DirectorySeparatorChar.ToString(), array, 0, num);
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x00039E12 File Offset: 0x00038012
		protected override BuildResult OnBuild(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			return new BuildResult();
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00039E19 File Offset: 0x00038019
		protected internal override bool OnGetNeedsBuilding(ConfigurationSelector configuration)
		{
			return false;
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00039E1C File Offset: 0x0003801C
		protected internal override void OnExecute(IProgressMonitor monitor, ExecutionContext context, ConfigurationSelector configuration)
		{
			ProjectConfiguration projectConfiguration = (ProjectConfiguration)this.GetConfiguration(configuration);
			monitor.Log.WriteLine(GettextCatalog.GetString("Running {0} ...", this.FileName));
			IConsole console = projectConfiguration.ExternalConsole ? context.ExternalConsoleFactory.CreateConsole(!projectConfiguration.PauseConsoleOutput) : context.ConsoleFactory.CreateConsole(!projectConfiguration.PauseConsoleOutput);
			AggregatedOperationMonitor aggregatedOperationMonitor = new AggregatedOperationMonitor(monitor, new IAsyncOperation[0]);
			try
			{
				try
				{
					ExecutionCommand command = this.CreateExecutionCommand(configuration, projectConfiguration);
					if (!context.ExecutionHandler.CanExecute(command))
					{
						monitor.ReportError(GettextCatalog.GetString("Can not execute \"{0}\". The selected execution mode is not supported for .NET projects.", this.FileName), null);
					}
					else
					{
						IProcessAsyncOperation processAsyncOperation = context.ExecutionHandler.Execute(command, console);
						aggregatedOperationMonitor.AddOperation(processAsyncOperation);
						processAsyncOperation.WaitForCompleted();
						monitor.Log.WriteLine(GettextCatalog.GetString("The application exited with code: {0}", processAsyncOperation.ExitCode));
					}
				}
				finally
				{
					console.Dispose();
					aggregatedOperationMonitor.Dispose();
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError(string.Format("Cannot execute \"{0}\"", this.FileName), ex);
				monitor.ReportError(GettextCatalog.GetString("Cannot execute \"{0}\"", this.FileName), ex);
			}
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x00039F78 File Offset: 0x00038178
		protected internal override bool OnGetCanExecute(ExecutionContext context, ConfigurationSelector configuration)
		{
			ProjectConfiguration projectConfiguration = (ProjectConfiguration)this.GetConfiguration(configuration);
			if (projectConfiguration == null)
			{
				return false;
			}
			if (this.FileName.Extension.ToLower() != ".exe")
			{
				return false;
			}
			ExecutionCommand command = this.CreateExecutionCommand(configuration, projectConfiguration);
			return context.ExecutionHandler.CanExecute(command);
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00039FD0 File Offset: 0x000381D0
		protected virtual ExecutionCommand CreateExecutionCommand(ConfigurationSelector configSel, ProjectConfiguration configuration)
		{
			return new DotNetExecutionCommand(this.FileName)
			{
				Arguments = configuration.CommandLineParameters,
				WorkingDirectory = Path.GetDirectoryName(this.FileName),
				EnvironmentVariables = new Dictionary<string, string>(configuration.EnvironmentVariables)
			};
		}

		// Token: 0x0400046F RID: 1135
		private TargetFramework targetFramework;
	}
}
