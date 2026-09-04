using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.Projects;
using Gtk;
using MonoDevelop.Core;
using Newtonsoft.Json;

namespace Modules.Communal.ResourcePanel
{
	internal class ImportFromPsdHandler : MenuHandler
	{
		protected override void Run()
		{
			string fileName = FileChooserDialogModel.GetOpenFilePath(new string[]
			{
				"psd"
			}, "Select PSD File", false, Services.RecentFileService.LastImportLocation).FileName;
			if (string.IsNullOrEmpty(fileName))
			{
				return;
			}
			if (!File.Exists(fileName))
			{
				return;
			}
			if (!string.Equals(Path.GetExtension(fileName), ".psd", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			string directoryName = Path.GetDirectoryName(fileName);
			if (!string.IsNullOrEmpty(directoryName))
			{
				Services.RecentFileService.LastImportLocation = directoryName;
			}
			ResourceFolder rootFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
			ResourceFolder targetFolder = GetTargetFolder(rootFolder);
			string outputName = Path.GetFileNameWithoutExtension(fileName);
			string outputCsd = Path.Combine(targetFolder.FullPath, outputName + ".csd");
			string resDir = GetResourceDirectory(rootFolder, targetFolder, outputName);
			try
			{
				string exePath = Psd2CsdRunner.GetExePath();
				Directory.CreateDirectory(targetFolder.FullPath);
				Psd2CsdResult psd2CsdResult = Psd2CsdRunner.Run(exePath, fileName, outputCsd, resDir);
				if (!psd2CsdResult.Success)
				{
					string text = psd2CsdResult.Error;
					if (string.IsNullOrWhiteSpace(text))
					{
						text = "psd2csd.exe failed.";
					}
					LogConfig.Output.Error(text);
					MessageBox.Show(text, MessageBoxImage.Other, null, null);
					return;
				}
				CleanupIgnoredOutputs(psd2CsdResult);
				List<string> importPaths = CollectImportPaths(rootFolder, outputCsd, psd2CsdResult);
				if (importPaths.Count == 0)
				{
					throw new InvalidOperationException("psd2csd.exe succeeded, but no importable files were generated.");
				}
				RegisterGeneratedResources(rootFolder, importPaths);
				MessageBox.Show(BuildSuccessMessage(psd2CsdResult, outputCsd), MessageBoxImage.Other, null, null);
			}
			catch (FileNotFoundException ex)
			{
				LogConfig.Output.Error("psd2csd.exe not found.", ex);
				MessageBox.Show(ex.Message, MessageBoxImage.Other, null, null);
			}
			catch (Exception ex2)
			{
				LogConfig.Output.Error("Import from PSD failed.", ex2);
				MessageBox.Show(ex2.Message, MessageBoxImage.Other, null, null);
			}
		}

		protected override void Update(MenuInfo info)
		{
			info.Enabled = true;
			info.Visible = true;
		}

		private static ResourceFolder GetTargetFolder(ResourceFolder rootFolder)
		{
			ResourceFolder resourceFolder = null;
			if (Services.ProjectsService.CurrentResourceItems != null && Services.ProjectsService.CurrentResourceItems.Count > 0)
			{
				ResourceItem resourceItem = Services.ProjectsService.CurrentResourceItems[0];
				resourceFolder = resourceItem as ResourceFolder;
				if (resourceFolder == null)
				{
					resourceFolder = resourceItem.Parent as ResourceFolder;
				}
			}
			if (resourceFolder == null)
			{
				resourceFolder = rootFolder;
			}
			return resourceFolder;
		}

		private static string GetResourceDirectory(ResourceFolder rootFolder, ResourceFolder targetFolder, string outputName)
		{
			if (targetFolder == null || rootFolder == null)
			{
				return "res/" + outputName;
			}
			FilePath relativePath = new FilePath(targetFolder.FullPath).ToRelative(rootFolder.FullPath);
			string text = relativePath.ToString();
			if (string.IsNullOrWhiteSpace(text) || text == ".")
			{
				return "res/" + outputName;
			}
			return text.Replace('\\', '/') + "/res/" + outputName;
		}

		private static string BuildSuccessMessage(Psd2CsdResult result, string fallbackOutputCsd)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("PSD import succeeded.");
			stringBuilder.AppendLine("CSD: " + GetDisplayPath(result.CsdPath, fallbackOutputCsd));
			if (!string.IsNullOrWhiteSpace(result.ResourceDir))
			{
				stringBuilder.AppendLine("Resource Dir: " + result.ResourceDir);
			}
			if (!string.IsNullOrWhiteSpace(result.ResourcePrefix))
			{
				stringBuilder.AppendLine("Resource Prefix: " + result.ResourcePrefix);
			}
			stringBuilder.AppendLine("Export Count: " + result.ExportCount);
			stringBuilder.AppendLine("Warning Count: " + result.WarningCount);
			if (result.Warnings != null && result.Warnings.Count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Warnings:");
				foreach (string text in result.Warnings)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						stringBuilder.AppendLine("- " + text);
					}
				}
			}
			return stringBuilder.ToString().TrimEnd();
		}

		private static void CleanupIgnoredOutputs(Psd2CsdResult result)
		{
			DeleteIfIgnored(result.DebugJsonPath);
			result.DebugJsonPath = string.Empty;
			if (result.ExportedResources == null || result.ExportedResources.Count == 0)
			{
				return;
			}
			List<string> list = new List<string>();
			foreach (string text in result.ExportedResources)
			{
				if (IsIgnoredOutput(text))
				{
					DeleteIfIgnored(text);
				}
				else
				{
					list.Add(text);
				}
			}
			result.ExportedResources = list;
		}

		private static List<string> CollectImportPaths(ResourceFolder rootFolder, string outputCsd, Psd2CsdResult result)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			string outputDirectory = Path.GetDirectoryName(outputCsd);
			AddImportPathRecursive(hashSet, NormalizeImportPath(rootFolder, outputDirectory, outputCsd));
			if (result.ExportedResources != null)
			{
				foreach (string text in result.ExportedResources)
				{
					AddImportPathRecursive(hashSet, NormalizeImportPath(rootFolder, outputDirectory, text));
				}
			}
			if (!string.IsNullOrWhiteSpace(result.ResourceDir))
			{
				AddImportPathRecursive(hashSet, NormalizeImportPath(rootFolder, outputDirectory, result.ResourceDir));
			}
			return hashSet.ToList<string>();
		}

		private static void RegisterGeneratedResources(ResourceFolder rootFolder, IEnumerable<string> importPaths)
		{
			List<ResourceItem> list = new List<ResourceItem>();
			IProgressMonitor consoleProgressMonitor = Services.ProgressMonitors.GetConsoleProgressMonitor(false, true);
			foreach (string text in importPaths.OrderBy(new Func<string, int>(GetPathDepth)).ThenBy(new Func<string, string>(Path.GetFullPath), StringComparer.OrdinalIgnoreCase))
			{
				AddResourceItemIfMissing(rootFolder, text, consoleProgressMonitor, list);
			}
			Services.ProjectOperations.CurrentSelectedSolution.Save(Services.ProgressMonitors.Default);
			if (list.Count > 0)
			{
				Services.EventsService.GetEvent<AddResourcesEvent>().Publish(new AddResourcesArgs(rootFolder, list, false));
			}
			else
			{
				rootFolder.Refresh();
			}
		}

		private static string GetDisplayPath(string path, string fallbackPath)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				return path;
			}
			return fallbackPath;
		}

		private static string NormalizeImportPath(ResourceFolder rootFolder, string outputDirectory, string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return null;
			}
			string text = path;
			if (!Path.IsPathRooted(text))
			{
				string path2 = Path.Combine(rootFolder.FullPath, path.Replace('/', Path.DirectorySeparatorChar));
				if (File.Exists(path2) || Directory.Exists(path2))
				{
					text = path2;
				}
				else
				{
					text = Path.Combine(outputDirectory, path.Replace('/', Path.DirectorySeparatorChar));
				}
			}
			if (IsIgnoredOutput(text))
			{
				return null;
			}
			if (!File.Exists(text) && !Directory.Exists(text))
			{
				return null;
			}
			return Path.GetFullPath(text);
		}

		private static ResourceItem AddResourceItemIfMissing(ResourceFolder rootFolder, string fullPath, IProgressMonitor monitor, IList<ResourceItem> addedItems)
		{
			ResourceItem resourceItem = Services.ProjectOperations.CurrentResourceGroup.FindResourceItem(rootFolder, fullPath);
			if (resourceItem != null)
			{
				return resourceItem;
			}
			Stack<string> stack = CreateParentStack(fullPath, rootFolder.BaseDirectory);
			ResourceFolder resourceFolder = rootFolder;
			ResourceItem result = rootFolder;
			while (stack.Count > 0)
			{
				string text = stack.Pop();
				string filePath = Path.Combine(resourceFolder.FullPath, text);
				ResourceItem resourceItem2 = resourceFolder.Items.FirstOrDefault((ResourceItem n) => string.Equals(n.Name, text, StringComparison.OrdinalIgnoreCase));
				if (resourceItem2 == null)
				{
					resourceItem2 = Services.ProjectsService.ReadResourceItem(monitor, filePath);
					if (resourceItem2 is ICocosFile)
					{
						((ICocosFile)resourceItem2).Initialize(monitor);
					}
					resourceFolder.Items.Add(resourceItem2);
					addedItems.Add(resourceItem2);
				}
				result = resourceItem2;
				resourceFolder = (resourceItem2 as ResourceFolder);
				if (stack.Count > 0 && resourceFolder == null)
				{
					throw new InvalidOperationException("PSD import generated an invalid resource path: " + fullPath);
				}
			}
			return result;
		}

		private static Stack<string> CreateParentStack(string importPath, FilePath parent)
		{
			Stack<string> stack = new Stack<string>();
			FilePath filePath = importPath;
			while (filePath != parent)
			{
				stack.Push(filePath.FileName);
				filePath = filePath.ParentDirectory;
			}
			return stack;
		}

		private static bool IsIgnoredOutput(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}
			string extension = Path.GetExtension(path);
			return string.Equals(extension, ".json", StringComparison.OrdinalIgnoreCase) || string.Equals(extension, ".psd", StringComparison.OrdinalIgnoreCase);
		}

		private static void AddImportPathRecursive(HashSet<string> paths, string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}
			paths.Add(path);
			if (!Directory.Exists(path))
			{
				return;
			}
			foreach (string text in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
			{
				if (!IsIgnoredOutput(text))
				{
					paths.Add(text);
				}
			}
			foreach (string text2 in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
			{
				if (!IsIgnoredOutput(text2))
				{
					paths.Add(text2);
				}
			}
		}

		private static int GetPathDepth(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return int.MaxValue;
			}
			string fullPath = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			return fullPath.Count((char c) => c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar);
		}

		private static void DeleteIfIgnored(string path)
		{
			if (!IsIgnoredOutput(path) || !File.Exists(path))
			{
				return;
			}
			try
			{
				File.Delete(path);
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("Failed to delete ignored PSD import output: " + path, ex);
			}
		}
	}

	internal class GenerateLuaHandler : MenuHandler
	{
		protected override void Run()
		{
		}

		protected override void Update(MenuInfo info)
		{
			info.Enabled = true;
			info.Visible = true;
		}
	}

	internal sealed class Psd2CsdResult
	{
		public Psd2CsdResult()
		{
			this.CsdPath = string.Empty;
			this.DebugJsonPath = string.Empty;
			this.ResourceDir = string.Empty;
			this.ResourcePrefix = string.Empty;
			this.Warnings = new List<string>();
			this.ExportedResources = new List<string>();
			this.Error = string.Empty;
		}

		public bool Success { get; set; }

		public string CsdPath { get; set; }

		public string DebugJsonPath { get; set; }

		public string ResourceDir { get; set; }

		public string ResourcePrefix { get; set; }

		public int ExportCount { get; set; }

		public int WarningCount { get; set; }

		public List<string> Warnings { get; set; }

		public List<string> ExportedResources { get; set; }

		public string Error { get; set; }
	}

	internal static class Psd2CsdRunner
	{
		internal static string GetExePath()
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			string exePath = Path.Combine(baseDirectory, "inject", "psd2csd.exe");
			if (!File.Exists(exePath))
			{
				throw new FileNotFoundException("Cannot find psd2csd.exe: " + exePath, exePath);
			}
			return exePath;
		}

		internal static Psd2CsdResult Run(string exePath, string inputPsd, string outputCsd, string resDir)
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = exePath;
			processStartInfo.Arguments = BuildArguments(inputPsd, outputCsd, resDir);
			processStartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardError = true;
			processStartInfo.UseShellExecute = false;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.StandardOutputEncoding = Encoding.UTF8;
			processStartInfo.StandardErrorEncoding = Encoding.UTF8;
			Psd2CsdResult psd2CsdResult = null;
			string text = string.Empty;
			string text2 = string.Empty;
			using (Process process = new Process())
			{
				process.StartInfo = processStartInfo;
				process.Start();
				text = process.StandardOutput.ReadToEnd();
				text2 = process.StandardError.ReadToEnd();
				process.WaitForExit();
				if (!string.IsNullOrWhiteSpace(text))
				{
					try
					{
						psd2CsdResult = JsonConvert.DeserializeObject<Psd2CsdResult>(text);
					}
					catch (Exception ex)
					{
						psd2CsdResult = new Psd2CsdResult();
						psd2CsdResult.Success = false;
						psd2CsdResult.Error = string.Format("Failed to parse psd2csd.exe JSON output.{0}Stdout:{0}{1}{0}{0}Stderr:{0}{2}{0}{0}{3}", Environment.NewLine, text, text2, ex.Message);
					}
				}
				if (psd2CsdResult == null)
				{
					psd2CsdResult = new Psd2CsdResult();
					psd2CsdResult.Success = false;
					psd2CsdResult.Error = BuildNoJsonError(text2, text);
				}
				if (process.ExitCode != 0)
				{
					psd2CsdResult.Success = false;
					if (string.IsNullOrWhiteSpace(psd2CsdResult.Error))
					{
						psd2CsdResult.Error = string.Format("psd2csd.exe exited with code {0}.{1}{2}", process.ExitCode, Environment.NewLine, BuildNoJsonError(text2, text));
					}
				}
			}
			if (string.IsNullOrWhiteSpace(psd2CsdResult.CsdPath))
			{
				psd2CsdResult.CsdPath = outputCsd;
			}
			if (string.IsNullOrWhiteSpace(psd2CsdResult.ResourceDir))
			{
				psd2CsdResult.ResourceDir = resDir ?? string.Empty;
			}
			return psd2CsdResult;
		}

		private static string BuildArguments(string inputPsd, string outputCsd, string resDir)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(QuoteArgument(inputPsd));
			stringBuilder.Append(" --out ");
			stringBuilder.Append(QuoteArgument(outputCsd));
			if (!string.IsNullOrWhiteSpace(resDir))
			{
				stringBuilder.Append(" --res-dir ");
				stringBuilder.Append(QuoteArgument(resDir));
			}
			stringBuilder.Append(" --json");
			return stringBuilder.ToString();
		}

		private static string BuildNoJsonError(string stderr, string stdout)
		{
			if (!string.IsNullOrWhiteSpace(stderr))
			{
				return stderr.Trim();
			}
			if (!string.IsNullOrWhiteSpace(stdout))
			{
				return "No JSON output from psd2csd.exe." + Environment.NewLine + stdout.Trim();
			}
			return "No JSON output from psd2csd.exe.";
		}

		private static string QuoteArgument(string value)
		{
			if (value == null)
			{
				return "\"\"";
			}
			return "\"" + value.Replace("\"", "\\\"") + "\"";
		}
	}
}
