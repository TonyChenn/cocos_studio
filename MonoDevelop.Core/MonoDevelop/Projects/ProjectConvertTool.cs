using System;
using System.Collections.Generic;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.ProgressMonitoring;

namespace MonoDevelop.Projects
{
	// Token: 0x02000158 RID: 344
	internal class ProjectConvertTool : IApplication
	{
		// Token: 0x06000CAD RID: 3245 RVA: 0x0002EA00 File Offset: 0x0002CC00
		public int Run(string[] arguments)
		{
			if (arguments.Length == 0 || arguments[0] == "--help")
			{
				Console.WriteLine("");
				Console.WriteLine("Project Export Tool");
				Console.WriteLine("Usage: mdtool project-export <source-project-file> [-d:dest-path] [-f:format-name]");
				Console.WriteLine("");
				Console.WriteLine("Options");
				Console.WriteLine("  -d:<dest-path>      Directory where the project will be exported.");
				Console.WriteLine("  -f:\"<format-name>\"  Format to which export the project or solution.");
				Console.WriteLine("  -l                  Show a list of all allowed target formats.");
				Console.WriteLine("  -p:<project-name>   When exporting a solution, name of a project to be");
				Console.WriteLine("                      included in the export. It can be specified multiple");
				Console.WriteLine("                      times.");
				Console.WriteLine("");
				Console.WriteLine("  The format name is optional. A list of allowed file formats will be");
				Console.WriteLine("  shown if none is provided.");
				Console.WriteLine("");
				return 0;
			}
			string text = null;
			string text2 = null;
			string text3 = null;
			bool flag = false;
			List<string> list = new List<string>();
			string[] includedChildIds = null;
			foreach (string text4 in arguments)
			{
				if (text4.StartsWith("-d:"))
				{
					text2 = text4.Substring(3);
				}
				else if (text4.StartsWith("-f:"))
				{
					text3 = text4.Substring(3);
				}
				else if (text4.StartsWith("-p:"))
				{
					list.Add(text4.Substring(3));
				}
				else if (text4 == "-l")
				{
					flag = true;
				}
				else
				{
					if (text != null)
					{
						Console.WriteLine("Only one project can be converted at a time.");
						return 1;
					}
					text = text4;
				}
			}
			if (text == null)
			{
				Console.WriteLine("Project or solution file name not provided.");
				return 1;
			}
			text = FileService.GetFullPath(text);
			if (!File.Exists(text))
			{
				Console.WriteLine("File {0} not found.", text);
				return 1;
			}
			ConsoleProgressMonitor consoleProgressMonitor = new ConsoleProgressMonitor();
			consoleProgressMonitor.IgnoreLogMessages = true;
			object obj;
			if (Services.ProjectService.IsWorkspaceItemFile(text))
			{
				obj = Services.ProjectService.ReadWorkspaceItem(consoleProgressMonitor, text);
				if (list.Count > 0)
				{
					Solution solution = obj as Solution;
					if (solution == null)
					{
						Console.WriteLine("The -p option can only be used when exporting a solution.");
						return 1;
					}
					for (int j = 0; j < list.Count; j++)
					{
						string text5 = list[j];
						if (text5.Length == 0)
						{
							Console.WriteLine("Project name not specified in -p option.");
							return 1;
						}
						Project project = solution.FindProjectByName(text5);
						if (project == null)
						{
							Console.WriteLine("Project '" + text5 + "' not found in solution.");
							return 1;
						}
						list[j] = project.ItemId;
					}
					includedChildIds = list.ToArray();
				}
			}
			else
			{
				if (list.Count > 0)
				{
					Console.WriteLine("The -p option can't be used when exporting a single project");
					return 1;
				}
				obj = Services.ProjectService.ReadSolutionItem(consoleProgressMonitor, text);
			}
			FileFormat[] fileFormatsForObject = Services.ProjectService.FileFormats.GetFileFormatsForObject(obj);
			if (fileFormatsForObject.Length == 0)
			{
				Console.WriteLine("Can't convert file to any format: " + text);
				return 1;
			}
			FileFormat fileFormat = null;
			if (text3 == null || flag)
			{
				Console.WriteLine();
				Console.WriteLine("Target formats:");
				for (int k = 0; k < fileFormatsForObject.Length; k++)
				{
					Console.WriteLine("  {0}. {1}", k + 1, fileFormatsForObject[k].Name);
				}
				Console.WriteLine();
				if (flag)
				{
					return 0;
				}
				int num = 0;
				for (;;)
				{
					Console.Write("Convert to format: ");
					string text6 = Console.ReadLine();
					if (text6.Length == 0)
					{
						break;
					}
					if (int.TryParse(text6, out num) && num > 0 && num <= fileFormatsForObject.Length)
					{
						goto Block_24;
					}
				}
				return 1;
				Block_24:
				fileFormat = fileFormatsForObject[num - 1];
			}
			else
			{
				foreach (FileFormat fileFormat2 in fileFormatsForObject)
				{
					if (fileFormat2.Name == text3)
					{
						fileFormat = fileFormat2;
					}
				}
				if (fileFormat == null)
				{
					Console.WriteLine("Unknown file format: " + text3);
					return 1;
				}
			}
			if (text2 == null)
			{
				text2 = Path.GetDirectoryName(text);
			}
			text2 = FileService.GetFullPath(text2);
			string text7 = Services.ProjectService.Export(consoleProgressMonitor, text, includedChildIds, text2, fileFormat);
			if (text7 != null)
			{
				Console.WriteLine("Saved file: " + text7);
				return 0;
			}
			Console.WriteLine("Project export failed.");
			return 1;
		}
	}
}
