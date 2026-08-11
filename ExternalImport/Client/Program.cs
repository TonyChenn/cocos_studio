using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using CocosStudio.ExternalImport.Protocol;
using Newtonsoft.Json;

namespace CocosStudio.ExternalImport
{
	internal static class Program
	{
		private static int Main(string[] args)
		{
			try
			{
				if (args == null || args.Length == 0)
				{
					return WriteFailure("INVALID_ARGUMENTS", GetUsage(), 2);
				}
				string command = args[0].Trim().ToLowerInvariant();
				if (command == "instances")
				{
					Console.WriteLine(JsonConvert.SerializeObject(LoadInstances(), Formatting.Indented));
					return 0;
				}
				if (command != "import")
				{
					return WriteFailure("INVALID_ARGUMENTS", GetUsage(), 2);
				}
				return RunImport(args.Skip(1).ToArray());
			}
			catch (Exception ex)
			{
				return WriteFailure("CLIENT_ERROR", ex.Message, 5);
			}
		}

		private static int RunImport(string[] args)
		{
			Dictionary<string, string> values;
			HashSet<string> flags;
			string parseError;
			if (!TryParseArguments(args, out values, out flags, out parseError))
			{
				return WriteFailure("INVALID_ARGUMENTS", parseError + Environment.NewLine + GetUsage(), 2);
			}
			string csdFile;
			string resourceFolder;
			string targetDirectory;
			if (!values.TryGetValue("--csd", out csdFile) || !values.TryGetValue("--resources", out resourceFolder) || !values.TryGetValue("--target", out targetDirectory))
			{
				return WriteFailure("INVALID_ARGUMENTS", "--csd, --resources and --target are required." + Environment.NewLine + GetUsage(), 2);
			}

			string projectFile;
			values.TryGetValue("--project", out projectFile);
			List<ExternalImportInstance> instances = LoadInstances();
			ExternalImportInstance instance;
			string routeError;
			if (!TrySelectInstance(instances, projectFile, out instance, out routeError))
			{
				string routeCode = instances.Count == 0 ? "STUDIO_NOT_FOUND" : (string.IsNullOrWhiteSpace(projectFile) ? "MULTIPLE_STUDIOS" : "PROJECT_NOT_OPEN");
				return WriteFailure(routeCode, routeError, 3);
			}

			ExternalImportRequest request = new ExternalImportRequest
			{
				ProjectFile = string.IsNullOrWhiteSpace(projectFile) ? instance.ProjectFile : projectFile,
				CsdFile = csdFile,
				ResourceFolder = resourceFolder,
				TargetDirectory = targetDirectory,
				Overwrite = flags.Contains("--overwrite"),
				OpenAfterImport = !flags.Contains("--no-open")
			};

			ExternalImportResponse response;
			using (NamedPipeClientStream pipe = new NamedPipeClientStream(".", instance.PipeName, PipeDirection.InOut, PipeOptions.None))
			{
				pipe.Connect(5000);
				ExternalImportProtocol.WriteMessage(pipe, request);
				response = ExternalImportProtocol.ReadMessage<ExternalImportResponse>(pipe);
			}
			Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
			return response != null && response.Success ? 0 : 4;
		}

		private static List<ExternalImportInstance> LoadInstances()
		{
			List<ExternalImportInstance> result = new List<ExternalImportInstance>();
			string directory = ExternalImportProtocol.InstanceDirectory;
			if (!Directory.Exists(directory))
			{
				return result;
			}
			foreach (string file in Directory.GetFiles(directory, "studio-*.json", SearchOption.TopDirectoryOnly))
			{
				try
				{
					ExternalImportInstance instance = JsonConvert.DeserializeObject<ExternalImportInstance>(File.ReadAllText(file));
					if (instance == null || instance.ProtocolVersion != ExternalImportProtocol.CurrentVersion || string.IsNullOrWhiteSpace(instance.PipeName))
					{
						continue;
					}
					Process process = Process.GetProcessById(instance.ProcessId);
					if (!process.HasExited)
					{
						result.Add(instance);
					}
				}
				catch
				{
				}
			}
			return result.OrderBy((ExternalImportInstance instance) => instance.ProcessId).ToList();
		}

		private static bool TrySelectInstance(IList<ExternalImportInstance> instances, string projectFile, out ExternalImportInstance instance, out string error)
		{
			instance = null;
			error = string.Empty;
			if (!string.IsNullOrWhiteSpace(projectFile))
			{
				string fullProject;
				try
				{
					fullProject = Path.GetFullPath(projectFile);
				}
				catch (Exception ex)
				{
					error = ex.Message;
					return false;
				}
				List<ExternalImportInstance> matches = instances.Where((ExternalImportInstance candidate) => PathEquals(candidate.ProjectFile, fullProject)).ToList();
				if (matches.Count == 1)
				{
					instance = matches[0];
					return true;
				}
				error = matches.Count == 0 ? "No running Cocos Studio instance has the requested project open." : "More than one Cocos Studio instance matches the requested project.";
				return false;
			}
			if (instances.Count == 1)
			{
				instance = instances[0];
				return true;
			}
			error = instances.Count == 0 ? "Cocos Studio is not running or its import service is unavailable." : "Multiple Cocos Studio instances are running. Specify --project.";
			return false;
		}

		private static bool TryParseArguments(string[] args, out Dictionary<string, string> values, out HashSet<string> flags, out string error)
		{
			values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			error = string.Empty;
			for (int index = 0; index < args.Length; index++)
			{
				string argument = args[index];
				if (argument == "--overwrite" || argument == "--no-open")
				{
					flags.Add(argument);
					continue;
				}
				if (argument != "--csd" && argument != "--resources" && argument != "--target" && argument != "--project")
				{
					error = "Unknown argument: " + argument;
					return false;
				}
				if (index + 1 >= args.Length)
				{
					error = "Missing value for " + argument;
					return false;
				}
				values[argument] = args[++index];
			}
			return true;
		}

		private static bool PathEquals(string left, string right)
		{
			if (string.IsNullOrWhiteSpace(left) || string.IsNullOrWhiteSpace(right))
			{
				return false;
			}
			try
			{
				return string.Equals(Path.GetFullPath(left).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), Path.GetFullPath(right).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), StringComparison.OrdinalIgnoreCase);
			}
			catch
			{
				return false;
			}
		}

		private static int WriteFailure(string code, string message, int exitCode)
		{
			ExternalImportResponse response = new ExternalImportResponse
			{
				Success = false,
				Code = code,
				Message = message
			};
			Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
			return exitCode;
		}

		private static string GetUsage()
		{
			return "Usage: CocosStudio.Import.exe import --csd <file.csd> --resources <folder> --target <project-relative-dir> [--project <file.ccs>] [--overwrite] [--no-open]" + Environment.NewLine + "       CocosStudio.Import.exe instances";
		}
	}
}
