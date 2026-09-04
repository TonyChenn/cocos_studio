using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Framework;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	public class ProjectBuilder : MarshalByRefObject, IProjectBuilder, IDisposable
	{
		private readonly string file;

		private ILogWriter currentLogWriter;

		private readonly MDConsoleLogger consoleLogger;

		private readonly BuildEngine buildEngine;

		private bool? hasXbuildFileBug;

		public ProjectBuilder(BuildEngine buildEngine, string file)
		{
			this.file = file;
			this.buildEngine = buildEngine;
			consoleLogger = new MDConsoleLogger(LoggerVerbosity.Normal, LogWriteLine, null, null);
		}

		public MSBuildResult Run(ProjectConfigurationInfo[] configurations, ILogWriter logWriter, MSBuildVerbosity verbosity, string[] runTargets, string[] evaluateItems, string[] evaluateProperties)
		{
			MSBuildResult result = null;
			BuildEngine.RunSTA(delegate
			{
				try
				{
					Project project = SetupProject(configurations);
					currentLogWriter = logWriter;
					buildEngine.Engine.UnregisterAllLoggers();
					LocalLogger localLogger = new LocalLogger(file);
					buildEngine.Engine.RegisterLogger(localLogger);
					if (logWriter != null)
					{
						buildEngine.Engine.RegisterLogger(consoleLogger);
						consoleLogger.Verbosity = GetVerbosity(verbosity);
					}
					if (runTargets != null && runTargets.Length > 0)
					{
						buildEngine.Engine.BuildProject(project, runTargets, new Hashtable(), BuildSettings.None);
					}
					result = new MSBuildResult(localLogger.BuildResult.ToArray());
					if (evaluateProperties != null)
					{
						string[] array = evaluateProperties;
						foreach (string text in array)
						{
							result.Properties[text] = project.GetEvaluatedProperty(text);
						}
					}
					if (evaluateItems != null)
					{
						string[] array = evaluateItems;
						foreach (string text in array)
						{
							BuildItemGroup evaluatedItemsByName = project.GetEvaluatedItemsByName(text);
							List<MSBuildEvaluatedItem> list = new List<MSBuildEvaluatedItem>();
							foreach (BuildItem item in evaluatedItemsByName)
							{
								MSBuildEvaluatedItem mSBuildEvaluatedItem = new MSBuildEvaluatedItem(text, UnescapeString(item.FinalItemSpec));
								// Use the public API: the private metadata field differs between Mono and Windows MSBuild.
								foreach (string metadataName in item.CustomMetadataNames)
								{
									// GetEvaluatedMetadata already unescapes its result; do not unescape literal percent sequences twice.
									mSBuildEvaluatedItem.Metadata[metadataName] = item.GetEvaluatedMetadata(metadataName);
								}
								list.Add(mSBuildEvaluatedItem);
							}
							result.Items[text] = list;
						}
					}
				}
				catch (InvalidProjectFileException ex)
				{
					MSBuildTargetResult mSBuildTargetResult = new MSBuildTargetResult(file, isWarning: false, ex.ErrorSubcategory, ex.ErrorCode, ex.ProjectFile, ex.LineNumber, ex.ColumnNumber, ex.EndLineNumber, ex.EndColumnNumber, ex.BaseMessage, ex.HelpKeyword);
					if (logWriter != null)
					{
						logWriter.WriteLine(mSBuildTargetResult.ToString());
					}
					result = new MSBuildResult(new MSBuildTargetResult[1] { mSBuildTargetResult });
				}
				finally
				{
					currentLogWriter = null;
				}
			});
			return result;
		}

		private Project SetupProject(ProjectConfigurationInfo[] configurations)
		{
			Project result = null;
			string propertyValue = GenerateSolutionConfigurationContents(configurations);
			foreach (ProjectConfigurationInfo projectConfigurationInfo in configurations)
			{
				Project project = buildEngine.Engine.GetLoadedProject(projectConfigurationInfo.ProjectFile);
				if (project != null && projectConfigurationInfo.ProjectFile == file)
				{
					buildEngine.Engine.UnloadProject(project);
					project = null;
				}
				Environment.CurrentDirectory = Path.GetDirectoryName(file);
				if (project == null)
				{
					project = new Project(buildEngine.Engine);
					string unsavedProjectContent = buildEngine.GetUnsavedProjectContent(projectConfigurationInfo.ProjectFile);
					if (unsavedProjectContent == null)
					{
						project.Load(projectConfigurationInfo.ProjectFile);
					}
					else
					{
						project.FullFileName = projectConfigurationInfo.ProjectFile;
						if (HasXbuildFileBug())
						{
							Type type = project.GetType();
							type.InvokeMember("PushThisFileProperty", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, project, new object[1] { project.FullFileName });
							type.InvokeMember("DoLoad", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, project, new object[1]
							{
								new StringReader(unsavedProjectContent)
							});
						}
						else
						{
							project.Load(new StringReader(unsavedProjectContent));
						}
					}
				}
				project.GlobalProperties.SetProperty("CurrentSolutionConfigurationContents", propertyValue);
				project.GlobalProperties.SetProperty("Configuration", projectConfigurationInfo.Configuration);
				if (!string.IsNullOrEmpty(projectConfigurationInfo.Platform))
				{
					project.GlobalProperties.SetProperty("Platform", projectConfigurationInfo.Platform);
				}
				else
				{
					project.GlobalProperties.RemoveProperty("Platform");
				}
				if (projectConfigurationInfo.ProjectFile == file)
				{
					result = project;
				}
			}
			Environment.CurrentDirectory = Path.GetDirectoryName(file);
			return result;
		}

		private bool HasXbuildFileBug()
		{
			if (!hasXbuildFileBug.HasValue)
			{
				Project project = new Project();
				project.FullFileName = "foo";
				project.LoadXml("<Project xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"/>");
				hasXbuildFileBug = project.FullFileName.Length == 0;
			}
			return hasXbuildFileBug.Value;
		}

		public void Dispose()
		{
			buildEngine.UnloadProject(file);
		}

		public void Refresh()
		{
			buildEngine.UnloadProject(file);
		}

		public void RefreshWithContent(string projectContent)
		{
			buildEngine.UnloadProject(file);
			buildEngine.SetUnsavedProjectContent(file, projectContent);
		}

		private void LogWriteLine(string txt)
		{
			if (currentLogWriter != null)
			{
				currentLogWriter.WriteLine(txt);
			}
		}

		private LoggerVerbosity GetVerbosity(MSBuildVerbosity verbosity)
		{
			switch (verbosity)
			{
			case MSBuildVerbosity.Quiet:
				return LoggerVerbosity.Quiet;
			case MSBuildVerbosity.Minimal:
				return LoggerVerbosity.Minimal;
			default:
				return LoggerVerbosity.Normal;
			case MSBuildVerbosity.Detailed:
				return LoggerVerbosity.Detailed;
			case MSBuildVerbosity.Diagnostic:
				return LoggerVerbosity.Diagnostic;
			}
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}

		private static string UnescapeString(string str)
		{
			int num = str.IndexOf('%');
			while (num != -1 && num < str.Length - 2)
			{
				if (int.TryParse(str.Substring(num + 1, 2), NumberStyles.HexNumber, null, out var result))
				{
					str = str.Substring(0, num) + (char)result + str.Substring(num + 3);
				}
				num = str.IndexOf('%', num + 1);
			}
			return str;
		}

		private string GenerateSolutionConfigurationContents(ProjectConfigurationInfo[] configurations)
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = xmlDocument.CreateElement("SolutionConfiguration");
			xmlDocument.AppendChild(xmlElement);
			foreach (ProjectConfigurationInfo projectConfigurationInfo in configurations)
			{
				XmlElement xmlElement2 = xmlDocument.CreateElement("ProjectConfiguration");
				xmlElement.AppendChild(xmlElement2);
				xmlElement2.SetAttribute("Project", projectConfigurationInfo.ProjectGuid);
				xmlElement2.SetAttribute("AbsolutePath", projectConfigurationInfo.ProjectFile);
				xmlElement2.InnerText = string.Format(projectConfigurationInfo.Configuration + "|" + projectConfigurationInfo.Platform);
			}
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;
			xmlWriterSettings.IndentChars = "";
			xmlWriterSettings.OmitXmlDeclaration = true;
			XmlWriterSettings settings = xmlWriterSettings;
			using (StringWriter stringWriter = new StringWriter())
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
				{
					xmlDocument.WriteTo(xmlWriter);
					xmlWriter.Flush();
					return stringWriter.ToString();
				}
			}
		}
	}
}
