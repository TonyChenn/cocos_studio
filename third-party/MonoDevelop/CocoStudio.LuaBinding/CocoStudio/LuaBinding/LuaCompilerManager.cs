using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Projects;

namespace CocoStudio.LuaBinding
{
	internal static class LuaCompilerManager
	{
		private static Regex regex_error = new Regex("\\s*[\\w-.]+:\\s*(?<file>.+):(?<line>\\d+):\\s*(?<message>.*)", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

		private static void AppendQuoted(StringBuilder sb, string option, string val)
		{
			sb.Append('"');
			sb.Append(option);
			sb.Append(val);
			sb.Append("\" ");
		}

		public static BuildResult Compile(ProjectItemCollection project_items, LuaConfiguration configuration, ConfigurationSelector config_selector, IProgressMonitor monitor)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("-p ");
			List<string> gac_roots = new List<string>();
			_ = configuration.DebugMode;
			foreach (ProjectFile item in project_items.GetAll<ProjectFile>())
			{
				string buildAction;
				if (item.Subtype != Subtype.Directory && (buildAction = item.BuildAction) != null && buildAction == "Compile")
				{
					AppendQuoted(stringBuilder, "", item.Name);
				}
			}
			string output = "";
			string error = "";
			string text = stringBuilder.ToString();
			monitor.Log.WriteLine(text);
			string text2 = ".";
			if (configuration.ParentItem != null)
			{
				text2 = configuration.ParentItem.BaseDirectory;
				if (text2 == null)
				{
					text2 = ".";
				}
			}
			LoggingService.LogInfo("luac " + stringBuilder.ToString());
			Dictionary<string, string> environmentVariables = configuration.EnvironmentVariables;
			int num = DoCompilation(text, text2, environmentVariables, gac_roots, ref output, ref error, monitor);
			BuildResult buildResult = ParseOutput(output, error, project_items);
			if (buildResult.CompilerOutput.Trim().Length != 0)
			{
				monitor.Log.WriteLine(buildResult.CompilerOutput);
			}
			if (buildResult.ErrorCount == 0 && num != 0)
			{
				if (!string.IsNullOrEmpty(error))
				{
					buildResult.AddError(error);
				}
				else
				{
					buildResult.AddError("The compiler appears to have crashed without any error output.");
				}
			}
			FileService.DeleteFile(output);
			FileService.DeleteFile(error);
			return buildResult;
		}

		private static BuildResult ParseOutput(string stdout, string stderr, ProjectItemCollection project_items)
		{
			BuildResult buildResult = new BuildResult();
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = new string[2] { stdout, stderr };
			foreach (string path in array)
			{
				StreamReader streamReader = File.OpenText(path);
				while (true)
				{
					string text = streamReader.ReadLine();
					stringBuilder.AppendLine(text);
					if (text == null)
					{
						break;
					}
					text = text.Trim();
					if (text.Length == 0)
					{
						continue;
					}
					BuildError buildError = CreateErrorFromString(text);
					if (buildError == null)
					{
						continue;
					}
					string fileName = buildError.FileName;
					if (fileName.StartsWith("..."))
					{
						string value = fileName.Substring(3);
						ProjectFile projectFile = null;
						foreach (ProjectFile item in project_items.GetAll<ProjectFile>())
						{
							string buildAction;
							if (item.Subtype != Subtype.Directory && (buildAction = item.BuildAction) != null && buildAction == "Compile")
							{
								if (item.Name.EndsWith(value))
								{
									projectFile = item;
								}
								if (projectFile != null)
								{
									break;
								}
							}
						}
						if (projectFile != null)
						{
							buildError.FileName = projectFile.Project.GetAbsoluteChildPath(projectFile.FilePath);
						}
					}
					buildResult.Append(buildError);
				}
				streamReader.Close();
			}
			buildResult.CompilerOutput = stringBuilder.ToString();
			return buildResult;
		}

		private static int DoCompilation(string outstr, string working_dir, Dictionary<string, string> env_vars, List<string> gac_roots, ref string output, ref string error, IProgressMonitor monitor)
		{
			output = Path.GetTempFileName();
			error = Path.GetTempFileName();
			StreamWriter streamWriter = new StreamWriter(output);
			StreamWriter streamWriter2 = new StreamWriter(error);
			string text = PropertyService.Get<string>("Lua.DefaultInterpreterPath");
			if (string.IsNullOrEmpty(text))
			{
				monitor.ReportError("Can't find Lua compiler (please set the default interpreter path)", new Exception());
				return 1;
			}
			text += "c";
			ProcessStartInfo processStartInfo = new ProcessStartInfo(text, outstr);
			processStartInfo.WorkingDirectory = working_dir;
			if (gac_roots.Count > 0)
			{
				string text2 = string.Join(string.Concat(Path.PathSeparator), gac_roots.ToArray());
				string environmentVariable = Environment.GetEnvironmentVariable("MONO_GAC_PREFIX");
				if (!string.IsNullOrEmpty(environmentVariable))
				{
					text2 = text2 + Path.PathSeparator + environmentVariable;
				}
				processStartInfo.EnvironmentVariables["MONO_GAC_PREFIX"] = text2;
			}
			foreach (KeyValuePair<string, string> env_var in env_vars)
			{
				processStartInfo.EnvironmentVariables.Add(env_var.Key, env_var.Value);
			}
			processStartInfo.UseShellExecute = false;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardError = true;
			ProcessWrapper processWrapper = Runtime.ProcessService.StartProcess(processStartInfo, streamWriter, streamWriter2, null);
			processWrapper.WaitForOutput();
			int exitCode = processWrapper.ExitCode;
			streamWriter.Close();
			streamWriter2.Close();
			processWrapper.Dispose();
			return exitCode;
		}

		private static BuildError CreateErrorFromString(string error_string)
		{
			if (error_string.StartsWith("Lua ", StringComparison.InvariantCulture))
			{
				return null;
			}
			Match match = regex_error.Match(error_string);
			if (!match.Success)
			{
				return null;
			}
			BuildError buildError = new BuildError();
			string fileName = match.Result("${file}") ?? "";
			buildError.FileName = fileName;
			string text = match.Result("${line}");
			buildError.Line = ((!string.IsNullOrEmpty(text)) ? int.Parse(text) : 0);
			buildError.IsWarning = false;
			buildError.ErrorText = match.Result("${message}");
			return buildError;
		}
	}
}
