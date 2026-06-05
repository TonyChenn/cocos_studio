using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.BuildEngine;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects.Formats.MD1
{
	// Token: 0x020001AA RID: 426
	internal class MD1DotNetProjectHandler : MD1SolutionEntityItemHandler
	{
		// Token: 0x06001000 RID: 4096 RVA: 0x0003B139 File Offset: 0x00039339
		public MD1DotNetProjectHandler(DotNetProject entry) : base(entry)
		{
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06001001 RID: 4097 RVA: 0x0003B142 File Offset: 0x00039342
		private DotNetProject Project
		{
			get
			{
				return (DotNetProject)base.Item;
			}
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0003B1E0 File Offset: 0x000393E0
		protected override BuildResult OnBuild(IProgressMonitor monitor, ConfigurationSelector configuration)
		{
			if (!this.Project.InternalCheckNeedsBuild(configuration))
			{
				monitor.Log.WriteLine(GettextCatalog.GetString("Skipping project since output files are up to date"));
				return new BuildResult();
			}
			DotNetProject project = this.Project;
			if (!project.TargetRuntime.IsInstalled(project.TargetFramework))
			{
				BuildResult buildResult = new BuildResult();
				buildResult.AddError(GettextCatalog.GetString("Framework '{0}' not installed.", project.TargetFramework.Name));
				return buildResult;
			}
			bool flag = false;
			foreach (ProjectFile projectFile in project.Files)
			{
				if (projectFile.BuildAction == "Compile" || projectFile.BuildAction == "EmbeddedResource")
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return new BuildResult();
			}
			if (project.LanguageBinding == null)
			{
				BuildResult buildResult2 = new BuildResult();
				string @string = GettextCatalog.GetString("Unknown language '{0}'. You may need to install an additional add-in to support this language.", project.LanguageName);
				buildResult2.AddError(@string);
				monitor.ReportError(@string, null);
				return buildResult2;
			}
			BuildResult refres = null;
			HashSet<ProjectItem> hashSet = new HashSet<ProjectItem>();
			foreach (ProjectReference projectReference in project.References)
			{
				if (projectReference.ReferenceType == ReferenceType.Project)
				{
					Project project2 = (project.ParentSolution != null) ? project.ParentSolution.FindProjectByName(projectReference.Reference) : null;
					if (project2 != null && !(project2 is DotNetProject))
					{
						continue;
					}
					if (project2 == null || projectReference.GetReferencedFileNames(configuration).Length == 0)
					{
						if (refres == null)
						{
							refres = new BuildResult();
						}
						string string2 = GettextCatalog.GetString("Referenced project '{0}' not found in the solution.", projectReference.Reference);
						monitor.ReportWarning(string2);
						refres.AddWarning(string2);
					}
				}
				if (!projectReference.IsValid)
				{
					if (refres == null)
					{
						refres = new BuildResult();
					}
					if (!projectReference.IsExactVersion && projectReference.SpecificVersion)
					{
						string string3 = GettextCatalog.GetString("Reference '{0}' not found on system. Using '{1}' instead.", projectReference.StoredReference, projectReference.Reference);
						monitor.ReportWarning(string3);
						refres.AddWarning(string3);
					}
					else
					{
						bool flag2 = false;
						foreach (string path in projectReference.GetReferencedFileNames(configuration))
						{
							if (!File.Exists(path))
							{
								string string3 = GettextCatalog.GetString("Assembly '{0}' not found. Make sure that the assembly exists in disk. If the reference is required to build the project you may get compilation errors.", Path.GetFileName(path));
								refres.AddWarning(string3);
								monitor.ReportWarning(string3);
								flag2 = true;
								hashSet.Add(projectReference);
							}
						}
						if (!flag2)
						{
							string string3 = GettextCatalog.GetString("The reference '{0}' is not valid for the target framework of the project.", projectReference.StoredReference, projectReference.Reference);
							monitor.ReportWarning(string3);
							refres.AddWarning(string3);
							hashSet.Add(projectReference);
						}
					}
				}
			}
			DotNetProjectConfiguration dotNetProjectConfiguration = (DotNetProjectConfiguration)project.GetConfiguration(configuration);
			BuildData buildData = new BuildData();
			ProjectParserContext context = new ProjectParserContext(project, dotNetProjectConfiguration);
			buildData.Items = new ProjectItemCollection();
			foreach (ProjectItem projectItem in project.Items)
			{
				if (!hashSet.Contains(projectItem) && (string.IsNullOrEmpty(projectItem.Condition) || ConditionParser.ParseAndEvaluate(projectItem.Condition, context)))
				{
					buildData.Items.Add(projectItem);
				}
			}
			buildData.Configuration = (DotNetProjectConfiguration)dotNetProjectConfiguration.Clone();
			buildData.Configuration.SetParentItem(project);
			buildData.ConfigurationSelector = configuration;
			return ProjectExtensionUtil.Compile(monitor, project, buildData, delegate(IProgressMonitor param0, SolutionEntityItem param1, BuildData param2)
			{
				ProjectItemCollection items = buildData.Items;
				BuildResult buildResult3 = this.BuildResources(buildData.Configuration, ref items, monitor);
				if (buildResult3 != null)
				{
					return buildResult3;
				}
				buildResult3 = project.LanguageBinding.Compile(items, buildData.Configuration, buildData.ConfigurationSelector, monitor);
				if (refres != null)
				{
					refres.Append(buildResult3);
					return refres;
				}
				return buildResult3;
			});
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0003B6A0 File Offset: 0x000398A0
		private BuildResult BuildResources(DotNetProjectConfiguration configuration, ref ProjectItemCollection projectItems, IProgressMonitor monitor)
		{
			string toolPath = configuration.TargetRuntime.GetToolPath(configuration.TargetFramework, "resgen");
			ExecutionEnvironment toolsExecutionEnvironment = configuration.TargetRuntime.GetToolsExecutionEnvironment(configuration.TargetFramework);
			bool flag = false;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (ProjectFile projectFile in projectItems.GetAll<ProjectFile>())
			{
				if (projectFile.Subtype != Subtype.Directory && !(projectFile.BuildAction != "EmbeddedResource"))
				{
					string name = projectFile.Name;
					string arg;
					CompilerError resourceId = this.GetResourceId(configuration.IntermediateOutputDirectory.Combine(new string[]
					{
						projectFile.ResourceId
					}), toolsExecutionEnvironment, projectFile, ref name, toolPath, out arg, monitor);
					if (resourceId != null)
					{
						return new BuildResult(new CompilerResults(new TempFileCollection())
						{
							Errors = 
							{
								resourceId
							}
						}, string.Empty);
					}
					string resourceCulture = DotNetProject.GetResourceCulture(projectFile.Name);
					if (resourceCulture != null)
					{
						string text = string.Empty;
						if (dictionary.ContainsKey(resourceCulture))
						{
							text = dictionary[resourceCulture];
						}
						text = string.Format("{0} \"/embed:{1},{2}\"", text, name, arg);
						dictionary[resourceCulture] = text;
						if (!flag)
						{
							ProjectItemCollection projectItemCollection = new ProjectItemCollection();
							projectItemCollection.AddRange(projectItems);
							projectItems = projectItemCollection;
							flag = true;
						}
						projectItems.Remove(projectFile);
					}
				}
			}
			string toolPath2 = configuration.TargetRuntime.GetToolPath(configuration.TargetFramework, "al");
			CompilerError compilerError = this.GenerateSatelliteAssemblies(dictionary, configuration.OutputDirectory, toolPath2, Path.GetFileName(configuration.OutputAssembly), monitor);
			if (compilerError != null)
			{
				return new BuildResult(new CompilerResults(new TempFileCollection())
				{
					Errors = 
					{
						compilerError
					}
				}, string.Empty);
			}
			return null;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0003B89C File Offset: 0x00039A9C
		private CompilerError GetResourceId(FilePath outputFile, ExecutionEnvironment env, ProjectFile finfo, ref string fname, string resgen, out string resourceId, IProgressMonitor monitor)
		{
			resourceId = finfo.ResourceId;
			if (resourceId == null)
			{
				LoggingService.LogDebug(GettextCatalog.GetString("Error: Unable to build ResourceId for {0}.", fname));
				monitor.Log.WriteLine(GettextCatalog.GetString("Error: Unable to build ResourceId for {0}.", fname));
				return new CompilerError(fname, 0, 0, string.Empty, GettextCatalog.GetString("Unable to build ResourceId for {0}.", fname));
			}
			if (string.Compare(Path.GetExtension(fname), ".resx", true) != 0)
			{
				return null;
			}
			if (!MD1DotNetProjectHandler.IsResgenRequired(fname, outputFile))
			{
				fname = (File.Exists(outputFile) ? outputFile : Path.ChangeExtension(fname, ".resources"));
				return null;
			}
			if (resgen == null)
			{
				string @string = GettextCatalog.GetString("Unable to find 'resgen' tool.");
				monitor.ReportError(@string, null);
				return new CompilerError(fname, 0, 0, string.Empty, @string);
			}
			using (StringWriter stringWriter = new StringWriter())
			{
				LoggingService.LogDebug("Compiling resources\n{0}$ {1} /compile {2}", new object[]
				{
					Path.GetDirectoryName(fname),
					resgen,
					fname
				});
				monitor.Log.WriteLine(GettextCatalog.GetString("Compiling resource {0} with {1}", fname, resgen));
				ProcessWrapper processWrapper = null;
				try
				{
					ProcessStartInfo processStartInfo = Runtime.ProcessService.CreateProcessStartInfo(resgen, string.Format("/compile \"{0}\"", fname), Path.GetDirectoryName(fname), false);
					env.MergeTo(processStartInfo);
					if (PlatformID.Unix == Environment.OSVersion.Platform)
					{
						processStartInfo.EnvironmentVariables["MONO_IOMAP"] = "drive";
					}
					processWrapper = Runtime.ProcessService.StartProcess(processStartInfo, stringWriter, stringWriter, null);
				}
				catch (Win32Exception ex)
				{
					LoggingService.LogDebug(GettextCatalog.GetString("Error while trying to invoke '{0}' to compile resource '{1}' :\n {2}", resgen, fname, ex.ToString()));
					monitor.Log.WriteLine(GettextCatalog.GetString("Error while trying to invoke '{0}' to compile resource '{1}' :\n {2}", resgen, fname, ex.Message));
					return new CompilerError(fname, 0, 0, string.Empty, ex.Message);
				}
				processWrapper.WaitForOutput();
				if (processWrapper.ExitCode != 0)
				{
					string text = stringWriter.ToString();
					LoggingService.LogDebug(GettextCatalog.GetString("Unable to compile ({0}) {1} to .resources. \nReason: \n{2}\n", resgen, fname, text));
					monitor.Log.WriteLine(GettextCatalog.GetString("Unable to compile ({0}) {1} to .resources. \nReason: \n{2}\n", resgen, fname, text));
					int line = 0;
					int column = 0;
					Match match = MD1DotNetProjectHandler.RegexErrorLinePos.Match(text);
					if (match.Success && match.Groups.Count == 3)
					{
						try
						{
							line = int.Parse(match.Groups[1].Value);
						}
						catch (FormatException)
						{
						}
						try
						{
							column = int.Parse(match.Groups[2].Value);
						}
						catch (FormatException)
						{
						}
					}
					return new CompilerError(fname, line, column, string.Empty, text);
				}
				fname = Path.ChangeExtension(fname, ".resources");
			}
			return null;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0003BBD0 File Offset: 0x00039DD0
		public static bool IsResgenRequired(string resx_filename, string output_filename)
		{
			if (string.Compare(Path.GetExtension(resx_filename), ".resx", true) != 0)
			{
				throw new ArgumentException(".resx file expected", "resx_filename");
			}
			if (File.Exists(output_filename))
			{
				return MD1DotNetProjectHandler.IsFileNewerThan(resx_filename, output_filename);
			}
			return MD1DotNetProjectHandler.IsFileNewerThan(resx_filename, Path.ChangeExtension(resx_filename, ".resources"));
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0003BC24 File Offset: 0x00039E24
		private static bool IsFileNewerThan(string first, string second)
		{
			FileInfo fileInfo = new FileInfo(first);
			FileInfo fileInfo2 = new FileInfo(second);
			return fileInfo.LastWriteTime > fileInfo2.LastWriteTime;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0003BC50 File Offset: 0x00039E50
		private CompilerError GenerateSatelliteAssemblies(Dictionary<string, string> resourcesByCulture, string outputDir, string al, string defaultns, IProgressMonitor monitor)
		{
			foreach (KeyValuePair<string, string> keyValuePair in resourcesByCulture)
			{
				string key = keyValuePair.Key;
				string text = Path.Combine(outputDir, key);
				string arg = defaultns + ".resources.dll";
				Directory.CreateDirectory(text);
				using (StringWriter stringWriter = new StringWriter())
				{
					string text2 = string.Format("/t:lib {0} \"/out:{1}\" /culture:{2}", keyValuePair.Value, arg, key);
					LoggingService.LogDebug("Generating satellite assembly for '{0}' culture.\n{1}$ {2} {3}", new object[]
					{
						key,
						text,
						al,
						text2
					});
					monitor.Log.WriteLine(GettextCatalog.GetString("Generating satellite assembly for '{0}' culture with {1}", key, al));
					ProcessWrapper processWrapper = null;
					try
					{
						ProcessStartInfo startInfo = Runtime.ProcessService.CreateProcessStartInfo(al, text2, text, false);
						processWrapper = Runtime.ProcessService.StartProcess(startInfo, stringWriter, stringWriter, null);
					}
					catch (Win32Exception ex)
					{
						LoggingService.LogDebug(GettextCatalog.GetString("Error while trying to invoke '{0}' to generate satellite assembly for '{1}' culture:\n {2}", al, key, ex.ToString()));
						monitor.Log.WriteLine(GettextCatalog.GetString("Error while trying to invoke '{0}' to generate satellite assembly for '{1}' culture:\n {2}", al, key, ex.Message));
						return new CompilerError("", 0, 0, string.Empty, ex.Message);
					}
					processWrapper.WaitForOutput();
					if (processWrapper.ExitCode != 0)
					{
						string text3 = stringWriter.ToString();
						LoggingService.LogDebug(GettextCatalog.GetString("Unable to generate satellite assemblies for '{0}' culture with {1}.\nReason: \n{2}\n", key, al, text3));
						monitor.Log.WriteLine(GettextCatalog.GetString("Unable to generate satellite assemblies for '{0}' culture with {1}.\nReason: \n{2}\n", key, al, text3));
						return new CompilerError(string.Empty, 0, 0, string.Empty, text3);
					}
				}
			}
			return null;
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06001008 RID: 4104 RVA: 0x0003BE4C File Offset: 0x0003A04C
		private static Regex RegexErrorLinePos
		{
			get
			{
				if (MD1DotNetProjectHandler.regexErrorLinePos == null)
				{
					MD1DotNetProjectHandler.regexErrorLinePos = new Regex("Line (\\d*), position (\\d*)");
				}
				return MD1DotNetProjectHandler.regexErrorLinePos;
			}
		}

		// Token: 0x040004A8 RID: 1192
		private static Regex regexErrorLinePos;
	}
}
