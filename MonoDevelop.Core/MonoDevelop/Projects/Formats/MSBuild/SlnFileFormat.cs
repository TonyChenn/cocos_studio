using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001BC RID: 444
	public class SlnFileFormat
	{
		// Token: 0x060010DC RID: 4316 RVA: 0x00041EB7 File Offset: 0x000400B7
		public string GetValidFormatName(object obj, string fileName, MSBuildFileFormat format)
		{
			return Path.ChangeExtension(fileName, ".sln");
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00041EC4 File Offset: 0x000400C4
		public bool CanReadFile(string file, MSBuildFileFormat format)
		{
			if (string.Compare(Path.GetExtension(file), ".sln", StringComparison.OrdinalIgnoreCase) == 0)
			{
				string text;
				string slnFileVersion = this.GetSlnFileVersion(file, out text);
				return format.SupportsSlnVersion(slnFileVersion);
			}
			return false;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00041EF7 File Offset: 0x000400F7
		public bool CanWriteFile(object obj, MSBuildFileFormat format)
		{
			return obj is Solution;
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00041F02 File Offset: 0x00040102
		public List<string> GetItemFiles(object obj)
		{
			return null;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00041F08 File Offset: 0x00040108
		public void WriteFile(string file, object obj, MSBuildFileFormat format, bool saveProjects, IProgressMonitor monitor)
		{
			Solution solution = (Solution)obj;
			string text = string.Empty;
			try
			{
				monitor.BeginTask(GettextCatalog.GetString("Saving solution: {0}", file), 1);
				try
				{
					if (File.Exists(file))
					{
						text = Path.GetTempFileName();
					}
				}
				catch (IOException)
				{
				}
				string directoryName = Path.GetDirectoryName(file);
				if (text == string.Empty)
				{
					this.WriteFileInternal(file, solution, directoryName, format, saveProjects, monitor);
				}
				else
				{
					this.WriteFileInternal(text, solution, directoryName, format, saveProjects, monitor);
					File.Delete(file);
					File.Move(text, file);
				}
			}
			catch (Exception ex)
			{
				monitor.ReportError(GettextCatalog.GetString("Could not save solution: {0}", file), ex);
				LoggingService.LogError(GettextCatalog.GetString("Could not save solution: {0}", file), ex);
				if (!string.IsNullOrEmpty(text))
				{
					File.Delete(text);
				}
				throw;
			}
			finally
			{
				monitor.EndTask();
			}
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00041FF0 File Offset: 0x000401F0
		private void WriteFileInternal(string file, Solution solution, string baseDir, MSBuildFileFormat format, bool saveProjects, IProgressMonitor monitor)
		{
			SolutionFolder rootFolder = solution.RootFolder;
			using (StreamWriter streamWriter = new StreamWriter(file, false, Encoding.UTF8))
			{
				streamWriter.NewLine = "\r\n";
				SlnData slnData = SlnFileFormat.GetSlnData(rootFolder);
				if (slnData == null)
				{
					slnData = new SlnData();
					rootFolder.ExtendedProperties[typeof(SlnFileFormat)] = slnData;
				}
				slnData.UpdateVersion(format);
				streamWriter.WriteLine();
				streamWriter.WriteLine("Microsoft Visual Studio Solution File, Format Version " + slnData.VersionString);
				streamWriter.WriteLine(slnData.HeaderComment);
				if (slnData.VisualStudioVersion != null)
				{
					streamWriter.WriteLine("VisualStudioVersion = {0}", slnData.VisualStudioVersion);
				}
				if (slnData.MinimumVisualStudioVersion != null)
				{
					streamWriter.WriteLine("MinimumVisualStudioVersion = {0}", slnData.MinimumVisualStudioVersion);
				}
				monitor.BeginTask(GettextCatalog.GetString("Saving projects"), 1);
				this.WriteProjects(rootFolder, baseDir, streamWriter, saveProjects, monitor);
				monitor.EndTask();
				foreach (string value in slnData.UnknownProjects)
				{
					streamWriter.WriteLine(value);
				}
				streamWriter.WriteLine("Global");
				streamWriter.WriteLine("\tGlobalSection(SolutionConfigurationPlatforms) = preSolution");
				foreach (SolutionConfiguration configuration in solution.Configurations)
				{
					streamWriter.WriteLine("\t\t{0} = {0}", this.ToSlnConfigurationId(configuration));
				}
				streamWriter.WriteLine("\tEndGlobalSection");
				streamWriter.WriteLine("\tGlobalSection(ProjectConfigurationPlatforms) = postSolution");
				List<string> list = new List<string>();
				this.WriteProjectConfigurations(solution, list);
				list.Sort(StringComparer.Create(CultureInfo.InvariantCulture, true));
				foreach (string value2 in list)
				{
					streamWriter.WriteLine(value2);
				}
				if (slnData.SectionExtras.ContainsKey("ProjectConfigurationPlatforms"))
				{
					foreach (string arg in slnData.SectionExtras["ProjectConfigurationPlatforms"])
					{
						streamWriter.WriteLine("\t\t{0}", arg);
					}
				}
				streamWriter.WriteLine("\tEndGlobalSection");
				ICollection<SolutionFolder> allItems = solution.RootFolder.GetAllItems<SolutionFolder>();
				if (allItems.Count > 1)
				{
					streamWriter.WriteLine("\tGlobalSection(NestedProjects) = preSolution");
					foreach (SolutionFolder solutionFolder in allItems)
					{
						if (!solutionFolder.IsRoot)
						{
							this.WriteNestedProjects(solutionFolder, solution.RootFolder, streamWriter);
						}
					}
					streamWriter.WriteLine("\tEndGlobalSection");
				}
				MSBuildSerializer msbuildSerializer = new MSBuildSerializer(solution.FileName);
				DataItem dataItem = (DataItem)msbuildSerializer.Serialize(solution, typeof(Solution));
				if (dataItem.HasItemData)
				{
					streamWriter.WriteLine("\tGlobalSection(MonoDevelopProperties) = preSolution");
					this.WriteDataItem(streamWriter, dataItem);
					streamWriter.WriteLine("\tEndGlobalSection");
				}
				foreach (SolutionConfiguration solutionConfiguration in solution.Configurations)
				{
					dataItem = (DataItem)msbuildSerializer.Serialize(solutionConfiguration);
					if (dataItem.HasItemData)
					{
						streamWriter.WriteLine("\tGlobalSection(MonoDevelopProperties." + solutionConfiguration.Id + ") = preSolution");
						this.WriteDataItem(streamWriter, dataItem);
						streamWriter.WriteLine("\tEndGlobalSection");
					}
				}
				if (slnData.GlobalExtra != null)
				{
					foreach (string value3 in slnData.GlobalExtra)
					{
						streamWriter.WriteLine(value3);
					}
				}
				streamWriter.WriteLine("EndGlobal");
			}
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x00042490 File Offset: 0x00040690
		private void WriteProjects(SolutionFolder folder, string baseDirectory, StreamWriter writer, bool saveProjects, IProgressMonitor monitor)
		{
			monitor.BeginStepTask(GettextCatalog.GetString("Saving projects"), folder.Items.Count, 1);
			foreach (SolutionItem solutionItem in folder.Items)
			{
				string[] array = null;
				if (solutionItem is SolutionEntityItem)
				{
					SolutionEntityItem solutionEntityItem = (SolutionEntityItem)solutionItem;
					MSBuildHandler itemHandler = MSBuildProjectService.GetItemHandler(solutionEntityItem);
					if (saveProjects)
					{
						try
						{
							itemHandler.SavingSolution = true;
							solutionEntityItem.Save(monitor);
						}
						finally
						{
							itemHandler.SavingSolution = false;
						}
					}
					array = itemHandler.SlnProjectContent;
					writer.WriteLine("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"", new object[]
					{
						itemHandler.TypeGuid,
						solutionEntityItem.Name,
						FileService.NormalizeRelativePath(FileService.AbsoluteToRelativePath(baseDirectory, solutionEntityItem.FileName)).Replace('/', '\\'),
						solutionItem.ItemId
					});
					DataItem dataItem = itemHandler.WriteSlnData();
					if (dataItem != null && dataItem.HasItemData)
					{
						writer.WriteLine("\tProjectSection(MonoDevelopProperties) = preProject");
						this.WriteDataItem(writer, dataItem);
						writer.WriteLine("\tEndProjectSection");
					}
					if (solutionEntityItem.ItemDependencies.Count > 0 || itemHandler.UnresolvedProjectDependencies != null)
					{
						writer.WriteLine("\tProjectSection(ProjectDependencies) = postProject");
						foreach (SolutionEntityItem solutionEntityItem2 in solutionEntityItem.ItemDependencies)
						{
							writer.WriteLine("\t\t{0} = {0}", solutionEntityItem2.ItemId);
						}
						if (itemHandler.UnresolvedProjectDependencies != null)
						{
							foreach (string arg in itemHandler.UnresolvedProjectDependencies)
							{
								writer.WriteLine("\t\t{0} = {0}", arg);
							}
						}
						writer.WriteLine("\tEndProjectSection");
					}
				}
				else if (solutionItem is SolutionFolder)
				{
					SlnData slnData = SlnFileFormat.GetSlnData(solutionItem);
					if (slnData == null)
					{
						slnData = new SlnData();
						solutionItem.ExtendedProperties[typeof(SlnFileFormat)] = slnData;
					}
					array = slnData.Extra;
					writer.WriteLine("Project(\"{0}\") = \"{1}\", \"{2}\", \"{3}\"", new object[]
					{
						"{2150E333-8FDC-42A3-9474-1A3956D46DE8}",
						solutionItem.Name,
						solutionItem.Name,
						solutionItem.ItemId
					});
					this.WriteFolderFiles(writer, (SolutionFolder)solutionItem);
					MSBuildSerializer msbuildSerializer = new MSBuildSerializer(folder.ParentSolution.FileName);
					DataItem dataItem2 = (DataItem)msbuildSerializer.Serialize(solutionItem, typeof(SolutionFolder));
					if (dataItem2.HasItemData)
					{
						writer.WriteLine("\tProjectSection(MonoDevelopProperties) = preProject");
						this.WriteDataItem(writer, dataItem2);
						writer.WriteLine("\tEndProjectSection");
					}
				}
				if (array != null)
				{
					foreach (string value in array)
					{
						writer.WriteLine(value);
					}
				}
				writer.WriteLine("EndProject");
				if (solutionItem is SolutionFolder)
				{
					this.WriteProjects(solutionItem as SolutionFolder, baseDirectory, writer, saveProjects, monitor);
				}
				monitor.Step(1);
			}
			monitor.EndTask();
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x00042810 File Offset: 0x00040A10
		private void WriteFolderFiles(StreamWriter writer, SolutionFolder folder)
		{
			if (folder.Files.Count > 0)
			{
				writer.WriteLine("\tProjectSection(SolutionItems) = preProject");
				foreach (FilePath filePath in folder.Files)
				{
					string text = MSBuildProjectService.ToMSBuildPathRelative(folder.ParentSolution.ItemDirectory, filePath);
					writer.WriteLine("\t\t" + text + " = " + text);
				}
				writer.WriteLine("\tEndProjectSection");
			}
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x000428B0 File Offset: 0x00040AB0
		private void WriteProjectConfigurations(Solution sol, List<string> list)
		{
			foreach (SolutionConfiguration solutionConfiguration in sol.Configurations)
			{
				foreach (SolutionConfigurationEntry solutionConfigurationEntry in solutionConfiguration.Configurations)
				{
					SolutionEntityItem item = solutionConfigurationEntry.Item;
					if (item.SupportsConfigurations())
					{
						string text = item.ItemId;
						if (!text.StartsWith("{") && !text.EndsWith("}"))
						{
							text = "{" + text + "}";
						}
						list.Add(string.Format("\t\t{0}.{1}.ActiveCfg = {2}", text, this.ToSlnConfigurationId(solutionConfiguration), this.ToSlnConfigurationId(solutionConfigurationEntry.ItemConfiguration)));
						if (solutionConfigurationEntry.Build)
						{
							list.Add(string.Format("\t\t{0}.{1}.Build.0 = {2}", text, this.ToSlnConfigurationId(solutionConfiguration), this.ToSlnConfigurationId(solutionConfigurationEntry.ItemConfiguration)));
						}
						if (solutionConfigurationEntry.Deploy)
						{
							list.Add(string.Format("\t\t{0}.{1}.Deploy.0 = {2}", text, this.ToSlnConfigurationId(solutionConfiguration), this.ToSlnConfigurationId(solutionConfigurationEntry.ItemConfiguration)));
						}
					}
				}
			}
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x00042A1C File Offset: 0x00040C1C
		private void WriteNestedProjects(SolutionFolder folder, SolutionFolder root, StreamWriter writer)
		{
			foreach (SolutionItem solutionItem in folder.Items)
			{
				writer.WriteLine("{0}{1} = {2}", "\t\t", solutionItem.ItemId, folder.ItemId);
			}
		}

		// Token: 0x060010E6 RID: 4326 RVA: 0x00042A80 File Offset: 0x00040C80
		private DataItem GetSolutionItemData(List<string> lines)
		{
			int num;
			int num2;
			if (!this.FindSection(lines, "MonoDevelopProperties", true, out num, out num2))
			{
				return null;
			}
			DataItem result = this.ReadDataItem(num, num2 - num + 1, lines);
			lines.RemoveRange(num, num2 - num + 1);
			return result;
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00042AC0 File Offset: 0x00040CC0
		private List<string> ReadSolutionItemDependencies(List<string> lines)
		{
			int num;
			int num2;
			if (!this.FindSection(lines, "ProjectDependencies", false, out num, out num2))
			{
				return null;
			}
			List<string> list = new List<string>();
			for (int i = num + 1; i < num2; i++)
			{
				string text = lines[i];
				int num3 = text.IndexOf('=');
				if (num3 != -1)
				{
					list.Add(text.Substring(0, num3).Trim());
				}
			}
			lines.RemoveRange(num, num2 - num + 1);
			return list;
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00042B34 File Offset: 0x00040D34
		private List<string> ReadFolderFiles(List<string> lines)
		{
			List<string> list = new List<string>();
			int num;
			int num2;
			if (!this.FindSection(lines, "SolutionItems", true, out num, out num2))
			{
				return list;
			}
			for (int i = num + 1; i < num2; i++)
			{
				string text = lines[i];
				int num3 = text.IndexOf('=');
				if (num3 != -1)
				{
					text = text.Substring(0, num3).Trim(new char[]
					{
						' ',
						'\t'
					});
					if (text.Length > 0)
					{
						list.Add(text);
					}
				}
			}
			lines.RemoveRange(num, num2 - num + 1);
			return list;
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00042BCC File Offset: 0x00040DCC
		private bool FindSection(List<string> lines, string name, bool preProject, out int start, out int end)
		{
			start = -1;
			end = -1;
			string str = preProject ? "preProject" : "postProject";
			string b = "ProjectSection(" + name + ")=" + str;
			int num = 0;
			while (num < lines.Count && start == -1)
			{
				string a = lines[num].Replace("\t", "").Replace(" ", "");
				if (a == b)
				{
					start = num;
				}
				num++;
			}
			if (start == -1)
			{
				return false;
			}
			int num2 = start + 1;
			while (num2 < lines.Count && end == -1)
			{
				string a2 = lines[num2].Replace("\t", "").Replace(" ", "");
				if (a2 == "EndProjectSection")
				{
					end = num2;
				}
				num2++;
			}
			return end != -1;
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00042CB8 File Offset: 0x00040EB8
		private void DeserializeSolutionItem(Solution sln, SolutionItem item, List<string> lines)
		{
			DataItem solutionItemData = this.GetSolutionItemData(lines);
			if (solutionItemData == null)
			{
				return;
			}
			new MSBuildSerializer(sln.FileName)
			{
				SerializationContext = 
				{
					BaseFile = sln.FileName
				}
			}.Deserialize(item, solutionItemData);
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x00042D00 File Offset: 0x00040F00
		private void WriteDataItem(StreamWriter sw, DataItem item)
		{
			int num = 0;
			foreach (object obj in item.ItemData)
			{
				DataNode node = (DataNode)obj;
				this.WriteDataNode(sw, "", node, ref num);
			}
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x00042D64 File Offset: 0x00040F64
		private void WriteDataNode(StreamWriter sw, string prefix, DataNode node, ref int id)
		{
			string name = node.Name;
			string text = (prefix.Length > 0) ? (prefix + "." + name) : name;
			if (node is DataValue)
			{
				DataValue dataValue = (DataValue)node;
				string str = this.EncodeString(dataValue.Value);
				sw.WriteLine("\t\t" + text + " = " + str);
				return;
			}
			DataItem dataItem = (DataItem)node;
			sw.WriteLine(string.Concat(new object[]
			{
				"\t\t",
				text,
				" = $",
				id
			}));
			text = "$" + id;
			id++;
			foreach (object obj in dataItem.ItemData)
			{
				DataNode node2 = (DataNode)obj;
				this.WriteDataNode(sw, text, node2, ref id);
			}
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x00042E88 File Offset: 0x00041088
		private string EncodeString(string val)
		{
			if (val.Length == 0)
			{
				return val;
			}
			int num = val.IndexOfAny(new char[]
			{
				'\n',
				'\r',
				'\t'
			});
			if (num != -1 || val[0] == '@')
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (num != -1)
				{
					int num2 = val.IndexOf('\\');
					if (num2 != -1 && num2 < num)
					{
						num = num2;
					}
					stringBuilder.Append(val.Substring(0, num));
				}
				else
				{
					num = 0;
				}
				for (int i = num; i < val.Length; i++)
				{
					char c = val[i];
					if (c == '\r')
					{
						stringBuilder.Append("\\r");
					}
					else if (c == '\n')
					{
						stringBuilder.Append("\\n");
					}
					else if (c == '\t')
					{
						stringBuilder.Append("\\t");
					}
					else if (c == '\\')
					{
						stringBuilder.Append("\\\\");
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
				val = "@" + stringBuilder.ToString();
			}
			char c2 = val[0];
			char c3 = val[val.Length - 1];
			if (c2 == ' ' || c2 == '"' || c2 == '$' || c3 == ' ')
			{
				val = "\"" + val + "\"";
			}
			return val;
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00042FC4 File Offset: 0x000411C4
		private string DecodeString(string val)
		{
			val = val.Trim(new char[]
			{
				' ',
				'\t'
			});
			if (val.Length == 0)
			{
				return val;
			}
			if (val[0] == '"')
			{
				val = val.Substring(1, val.Length - 2);
			}
			if (val[0] == '@')
			{
				StringBuilder stringBuilder = new StringBuilder(val.Length);
				for (int i = 1; i < val.Length; i++)
				{
					char c = val[i];
					if (c == '\\')
					{
						c = val[++i];
						if (c == 'r')
						{
							c = '\r';
						}
						else if (c == 'n')
						{
							c = '\n';
						}
						else if (c == 't')
						{
							c = '\t';
						}
					}
					stringBuilder.Append(c);
				}
				return stringBuilder.ToString();
			}
			return val;
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0004307E File Offset: 0x0004127E
		private DataItem ReadDataItem(Section sec, List<string> lines)
		{
			return this.ReadDataItem(sec.Start, sec.Count, lines);
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00043094 File Offset: 0x00041294
		private DataItem ReadDataItem(int start, int count, List<string> lines)
		{
			DataItem dataItem = new DataItem();
			int i = start + 1;
			int num = start + count - 2;
			while (i <= num)
			{
				if (!this.ReadDataNode(dataItem, lines, num, "", ref i))
				{
					i++;
				}
			}
			return dataItem;
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x000430D0 File Offset: 0x000412D0
		private bool ReadDataNode(DataItem item, List<string> lines, int lastLine, string prefix, ref int lineNum)
		{
			string text = lines[lineNum].Trim(new char[]
			{
				' ',
				'\t'
			});
			if (text.Length == 0)
			{
				lineNum++;
				return true;
			}
			if (prefix.Length > 0)
			{
				if (!text.StartsWith(prefix + "."))
				{
					return false;
				}
				text = text.Substring(prefix.Length + 1);
			}
			else if (text.StartsWith("$"))
			{
				return false;
			}
			int num = text.IndexOf('=');
			if (num == -1)
			{
				lineNum++;
				return true;
			}
			string text2 = text.Substring(0, num).Trim(new char[]
			{
				' ',
				'\t'
			});
			if (text2.Length == 0)
			{
				lineNum++;
				return true;
			}
			string text3 = text.Substring(num + 1).Trim(new char[]
			{
				' ',
				'\t'
			});
			if (text3.StartsWith("$"))
			{
				DataItem dataItem = new DataItem();
				dataItem.Name = text2;
				lineNum++;
				while (lineNum <= lastLine && this.ReadDataNode(dataItem, lines, lastLine, text3, ref lineNum))
				{
				}
				item.ItemData.Add(dataItem);
			}
			else
			{
				text3 = this.DecodeString(text3);
				DataValue entry = new DataValue(text2, text3);
				item.ItemData.Add(entry);
				lineNum++;
			}
			return true;
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x00043233 File Offset: 0x00041433
		private string ToSlnConfigurationId(ItemConfiguration configuration)
		{
			if (configuration.Platform.Length == 0)
			{
				return configuration.Name + "|Any CPU";
			}
			return configuration.Name + "|" + configuration.Platform;
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0004326C File Offset: 0x0004146C
		private string FromSlnConfigurationId(string configId)
		{
			int num = configId.IndexOf('|');
			if (num != -1 && configId.Substring(num + 1) == "Any CPU")
			{
				return configId.Substring(0, num);
			}
			return configId;
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x000432A5 File Offset: 0x000414A5
		private string ToSlnConfigurationId(string configId)
		{
			if (configId.IndexOf('|') == -1)
			{
				return configId + "|Any CPU";
			}
			return configId;
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x000432C0 File Offset: 0x000414C0
		public object ReadFile(string fileName, MSBuildFileFormat format, IProgressMonitor monitor)
		{
			if (fileName == null || monitor == null)
			{
				return null;
			}
			Solution solution;
			try
			{
				ProjectExtensionUtil.BeginLoadOperation();
				solution = new Solution();
				monitor.BeginTask(string.Format(GettextCatalog.GetString("Loading solution: {0}"), fileName), 1);
				IProjectLoadProgressMonitor projectLoadProgressMonitor = monitor as IProjectLoadProgressMonitor;
				if (projectLoadProgressMonitor != null)
				{
					projectLoadProgressMonitor.CurrentSolution = solution;
				}
				this.LoadSolution(solution, fileName, format, monitor);
			}
			catch (Exception exception)
			{
				monitor.ReportError(GettextCatalog.GetString("Could not load solution: {0}", fileName), exception);
				throw;
			}
			finally
			{
				ProjectExtensionUtil.EndLoadOperation();
				monitor.EndTask();
			}
			return solution;
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x000433E4 File Offset: 0x000415E4
		private SolutionFolder LoadSolution(Solution sol, string fileName, MSBuildFileFormat format, IProgressMonitor monitor)
		{
			string headerComment;
			string slnFileVersion = this.GetSlnFileVersion(fileName, out headerComment);
			ListDictionary listDictionary = null;
			SolutionFolder solutionFolder = null;
			SlnData slnData = null;
			List<Section> list = null;
			List<string> list2 = null;
			FileFormat fileFormat = Services.ProjectService.FileFormats.GetFileFormat(format);
			monitor.BeginTask(GettextCatalog.GetString("Loading solution: {0}", fileName), 1);
			using (StreamReader streamReader = new StreamReader(fileName))
			{
				sol.FileName = fileName;
				sol.ConvertToFormat(fileFormat, false);
				solutionFolder = sol.RootFolder;
				sol.Version = "0.1";
				slnData = new SlnData();
				solutionFolder.ExtendedProperties[typeof(SlnFileFormat)] = slnData;
				slnData.VersionString = slnFileVersion;
				slnData.HeaderComment = headerComment;
				list = new List<Section>();
				list2 = new List<string>();
				listDictionary = new ListDictionary();
				while (streamReader.Peek() >= 0)
				{
					string text = this.GetNextLine(streamReader, list2).Trim();
					if (string.Compare(text, "Global", StringComparison.OrdinalIgnoreCase) == 0)
					{
						this.ParseGlobal(streamReader, list2, listDictionary);
					}
					else if (text.StartsWith("Project", StringComparison.Ordinal))
					{
						Section section = new Section();
						list.Add(section);
						section.Start = list2.Count - 1;
						int num = this.ReadUntil("EndProject", streamReader, list2);
						section.Count = ((num < 0) ? 1 : (num - section.Start + 1));
					}
					else
					{
						if (text.StartsWith("VisualStudioVersion = ", StringComparison.Ordinal))
						{
							Version visualStudioVersion;
							if (Version.TryParse(text.Substring("VisualStudioVersion = ".Length), out visualStudioVersion))
							{
								slnData.VisualStudioVersion = visualStudioVersion;
							}
							else
							{
								monitor.Log.WriteLine("Ignoring unparseable VisualStudioVersion value in sln file");
							}
						}
						if (text.StartsWith("MinimumVisualStudioVersion = ", StringComparison.Ordinal))
						{
							Version minimumVisualStudioVersion;
							if (Version.TryParse(text.Substring("MinimumVisualStudioVersion = ".Length), out minimumVisualStudioVersion))
							{
								slnData.MinimumVisualStudioVersion = minimumVisualStudioVersion;
							}
							else
							{
								monitor.Log.WriteLine("Ignoring unparseable MinimumVisualStudioVersion value in sln file");
							}
						}
					}
				}
			}
			monitor.BeginTask("Loading projects ..", list.Count + 1);
			Dictionary<string, SolutionItem> dictionary = new Dictionary<string, SolutionItem>();
			List<SolutionItem> list3 = new List<SolutionItem>();
			foreach (Section section2 in list)
			{
				monitor.Step(1);
				Match match = SlnFileFormat.ProjectRegex.Match(list2[section2.Start]);
				if (!match.Success)
				{
					LoggingService.LogDebug(GettextCatalog.GetString("Invalid Project definition on line number #{0} in file '{1}'. Ignoring.", section2.Start + 1, fileName));
				}
				else
				{
					try
					{
						new Guid(match.Groups[1].Value);
					}
					catch (FormatException)
					{
						LoggingService.LogDebug(GettextCatalog.GetString("Invalid Project type guid '{0}' on line #{1}. Ignoring.", match.Groups[1].Value, section2.Start + 1));
						continue;
					}
					string projTypeGuid = match.Groups[1].Value.ToUpper();
					string value = match.Groups[2].Value;
					string projectPath = match.Groups[3].Value;
					string projectGuid = match.Groups[4].Value.ToUpper();
					if (projTypeGuid == "{2150E333-8FDC-42A3-9474-1A3956D46DE8}")
					{
						SolutionFolder solutionFolder2 = new SolutionFolder();
						solutionFolder2.Name = value;
						MSBuildProjectService.InitializeItemHandler(solutionFolder2);
						MSBuildProjectService.SetId(solutionFolder2, projectGuid);
						List<string> range = list2.GetRange(section2.Start + 1, section2.Count - 2);
						this.DeserializeSolutionItem(sol, solutionFolder2, range);
						foreach (string relPath in this.ReadFolderFiles(range))
						{
							solutionFolder2.Files.Add(MSBuildProjectService.FromMSBuildPath(Path.GetDirectoryName(fileName), relPath));
						}
						SlnData slnData2 = new SlnData();
						slnData2.Extra = range.ToArray();
						solutionFolder2.ExtendedProperties[typeof(SlnFileFormat)] = slnData2;
						dictionary.Add(projectGuid, solutionFolder2);
						list3.Add(solutionFolder2);
					}
					else if (projectPath.StartsWith("http://"))
					{
						monitor.ReportWarning(GettextCatalog.GetString("{0}({1}): Projects with non-local source (http://...) not supported. '{2}'.", fileName, section2.Start + 1, projectPath));
						slnData.UnknownProjects.AddRange(list2.GetRange(section2.Start, section2.Count));
					}
					else
					{
						string text2 = MSBuildProjectService.FromMSBuildPath(Path.GetDirectoryName(fileName), projectPath);
						if (string.IsNullOrEmpty(text2))
						{
							monitor.ReportWarning(GettextCatalog.GetString("Invalid project path found in {0} : {1}", fileName, projectPath));
							LoggingService.LogWarning(GettextCatalog.GetString("Invalid project path found in {0} : {1}", fileName, projectPath));
						}
						else
						{
							projectPath = Path.GetFullPath(text2);
							SolutionEntityItem solutionEntityItem = null;
							try
							{
								if (sol.IsSolutionItemEnabled(projectPath))
								{
									solutionEntityItem = ProjectExtensionUtil.LoadSolutionItem(monitor, projectPath, (IProgressMonitor param0, string param1) => MSBuildProjectService.LoadItem(monitor, projectPath, format, projTypeGuid, projectGuid));
									if (solutionEntityItem == null)
									{
										throw new UnknownSolutionItemTypeException(projTypeGuid);
									}
								}
								else
								{
									UnloadedSolutionItem unloadedSolutionItem = new UnloadedSolutionItem
									{
										FileName = projectPath
									};
									MSBuildHandler itemHandler = new MSBuildHandler(projTypeGuid, projectGuid)
									{
										Item = unloadedSolutionItem
									};
									unloadedSolutionItem.SetItemHandler(itemHandler);
									solutionEntityItem = unloadedSolutionItem;
								}
							}
							catch (Exception innerException)
							{
								while (innerException is TargetInvocationException)
								{
									innerException = ((TargetInvocationException)innerException).InnerException;
								}
								bool flag = false;
								if (innerException is UnknownSolutionItemTypeException)
								{
									string typeName = ((UnknownSolutionItemTypeException)innerException).TypeName;
									FilePath filePath = new FilePath(text2).ToRelative(sol.BaseDirectory);
									if (!string.IsNullOrEmpty(typeName))
									{
										string[] guids = typeName.Split(new char[]
										{
											';'
										});
										UnknownProjectTypeNode unknownProjectTypeInfo = MSBuildProjectService.GetUnknownProjectTypeInfo(guids, fileName);
										if (unknownProjectTypeInfo != null)
										{
											flag = unknownProjectTypeInfo.LoadFiles;
											LoggingService.LogWarning(string.Format("Could not load {0} project '{1}'. {2}", unknownProjectTypeInfo.Name, filePath, unknownProjectTypeInfo.GetInstructions()));
											monitor.ReportWarning(GettextCatalog.GetString("Could not load {0} project '{1}'. {2}", unknownProjectTypeInfo.Name, filePath, unknownProjectTypeInfo.GetInstructions()));
										}
										else
										{
											LoggingService.LogWarning(string.Format("Could not load project '{0}' with unknown item type '{1}'", filePath, typeName));
											monitor.ReportWarning(GettextCatalog.GetString("Could not load project '{0}' with unknown item type '{1}'", filePath, typeName));
										}
									}
									else
									{
										LoggingService.LogWarning(string.Format("Could not load project '{0}' with unknown item type", filePath));
										monitor.ReportWarning(GettextCatalog.GetString("Could not load project '{0}' with unknown item type", filePath));
									}
								}
								else if (innerException is UserException)
								{
									UserException ex = (UserException)innerException;
									LoggingService.LogError("{0}: {1}", new object[]
									{
										ex.Message,
										ex.Details
									});
									monitor.ReportError(string.Format("{0}{1}{1}{2}", ex.Message, Environment.NewLine, ex.Details), null);
								}
								else
								{
									LoggingService.LogError(string.Format("Error while trying to load the project {0}", projectPath), innerException);
									monitor.ReportWarning(GettextCatalog.GetString("Error while trying to load the project '{0}': {1}", projectPath, innerException.Message));
								}
								SolutionEntityItem solutionEntityItem2;
								if (flag)
								{
									solutionEntityItem2 = new UnknownProject
									{
										FileName = projectPath,
										LoadError = innerException.Message
									};
								}
								else
								{
									solutionEntityItem2 = new UnknownSolutionItem
									{
										FileName = projectPath,
										LoadError = innerException.Message
									};
								}
								MSBuildHandler itemHandler2 = new MSBuildHandler(projTypeGuid, projectGuid)
								{
									Item = solutionEntityItem2
								};
								solutionEntityItem2.SetItemHandler(itemHandler2);
								solutionEntityItem = solutionEntityItem2;
							}
							MSBuildHandler msbuildHandler = (MSBuildHandler)solutionEntityItem.ItemHandler;
							List<string> range = list2.GetRange(section2.Start + 1, section2.Count - 2);
							DataItem solutionItemData = this.GetSolutionItemData(range);
							msbuildHandler.UnresolvedProjectDependencies = this.ReadSolutionItemDependencies(range);
							msbuildHandler.SlnProjectContent = range.ToArray();
							msbuildHandler.ReadSlnData(solutionItemData);
							if (!dictionary.ContainsKey(projectGuid))
							{
								dictionary.Add(projectGuid, solutionEntityItem);
								list3.Add(solutionEntityItem);
								slnData.ItemsByGuid[projectGuid] = solutionEntityItem;
							}
							else
							{
								monitor.ReportError(GettextCatalog.GetString("Invalid solution file. There are two projects with the same GUID. The project {0} will be ignored.", projectPath), null);
							}
						}
					}
				}
			}
			monitor.EndTask();
			if (listDictionary != null && listDictionary.Contains("NestedProjects"))
			{
				this.LoadNestedProjects(listDictionary["NestedProjects"] as Section, list2, dictionary, monitor);
				listDictionary.Remove("NestedProjects");
			}
			foreach (SolutionEntityItem solutionEntityItem3 in dictionary.Values.OfType<SolutionEntityItem>())
			{
				MSBuildHandler msbuildHandler2 = (MSBuildHandler)solutionEntityItem3.ItemHandler;
				if (msbuildHandler2.UnresolvedProjectDependencies != null)
				{
					foreach (string text3 in msbuildHandler2.UnresolvedProjectDependencies.ToArray())
					{
						SolutionItem solutionItem;
						if (dictionary.TryGetValue(text3, out solutionItem) && solutionItem is SolutionEntityItem)
						{
							msbuildHandler2.UnresolvedProjectDependencies.Remove(text3);
							solutionEntityItem3.ItemDependencies.Add((SolutionEntityItem)solutionItem);
						}
					}
					if (msbuildHandler2.UnresolvedProjectDependencies.Count == 0)
					{
						msbuildHandler2.UnresolvedProjectDependencies = null;
					}
				}
			}
			foreach (SolutionItem solutionItem2 in list3)
			{
				if (solutionItem2.ParentFolder == null)
				{
					solutionFolder.Items.Add(solutionItem2);
				}
			}
			if (listDictionary != null)
			{
				if (listDictionary.Contains("SolutionConfigurationPlatforms"))
				{
					this.LoadSolutionConfigurations(listDictionary["SolutionConfigurationPlatforms"] as Section, list2, sol, monitor);
					listDictionary.Remove("SolutionConfigurationPlatforms");
				}
				if (listDictionary.Contains("ProjectConfigurationPlatforms"))
				{
					this.LoadProjectConfigurationMappings(listDictionary["ProjectConfigurationPlatforms"] as Section, list2, sol, monitor);
					listDictionary.Remove("ProjectConfigurationPlatforms");
				}
				if (listDictionary.Contains("MonoDevelopProperties"))
				{
					this.LoadMonoDevelopProperties(listDictionary["MonoDevelopProperties"] as Section, list2, sol, monitor);
					listDictionary.Remove("MonoDevelopProperties");
				}
				ArrayList arrayList = new ArrayList();
				foreach (object obj in listDictionary)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text4 = (string)dictionaryEntry.Key;
					if (text4.StartsWith("MonoDevelopProperties."))
					{
						int num2 = text4.IndexOf('.');
						this.LoadMonoDevelopConfigurationProperties(text4.Substring(num2 + 1), (Section)dictionaryEntry.Value, list2, sol, monitor);
						arrayList.Add(dictionaryEntry.Key);
					}
				}
				foreach (object key in arrayList)
				{
					listDictionary.Remove(key);
				}
			}
			List<string> list4 = new List<string>();
			foreach (object obj2 in listDictionary.Values)
			{
				Section section3 = (Section)obj2;
				list4.InsertRange(list4.Count, list2.GetRange(section3.Start, section3.Count));
			}
			slnData.GlobalExtra = list4;
			monitor.EndTask();
			sol.SolutionItemAdded += delegate(object sender, SolutionItemChangeEventArgs e)
			{
				if (e.Reloading)
				{
					ItemSlnData.TransferData(e.ReplacedItem, e.SolutionItem);
					MSBuildHandler msbuildHandler3 = e.SolutionItem.ItemHandler as MSBuildHandler;
					if (msbuildHandler3 != null)
					{
						msbuildHandler3.ItemId = e.ReplacedItem.ItemId;
					}
				}
			};
			return solutionFolder;
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00044194 File Offset: 0x00042394
		private void ParseGlobal(StreamReader reader, List<string> lines, ListDictionary dict)
		{
			while (reader.Peek() >= 0)
			{
				string text = this.GetNextLine(reader, lines).Trim();
				if (text.Length != 0)
				{
					Match match = SlnFileFormat.GlobalSectionRegex.Match(text);
					if (!match.Success)
					{
						if (string.Compare(text, "EndGlobal", true) == 0)
						{
							return;
						}
					}
					else
					{
						Section section = new Section(match.Groups[1].Value, match.Groups[2].Value, lines.Count - 1, 1);
						dict[section.Key] = section;
						section.Count = this.ReadUntil("EndGlobalSection", reader, lines) - section.Start + 1;
					}
				}
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00044248 File Offset: 0x00042448
		private void LoadProjectConfigurationMappings(Section sec, List<string> lines, Solution sln, IProgressMonitor monitor)
		{
			if (sec == null || string.Compare(sec.Val, "postSolution", true) != 0)
			{
				return;
			}
			Dictionary<string, SolutionConfigurationEntry> dictionary = new Dictionary<string, SolutionConfigurationEntry>();
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			SlnData slnData = SlnFileFormat.GetSlnData(sln.RootFolder);
			List<string> list = new List<string>();
			for (int i = 0; i < sec.Count - 2; i++)
			{
				int num = i + sec.Start + 1;
				string text = lines[num].Trim();
				list.Add(text);
				string[] array = text.Split(new char[]
				{
					'='
				}, 2);
				if (array.Length < 2)
				{
					LoggingService.LogDebug("{0} ({1}) : Invalid format. Ignoring", new object[]
					{
						sln.FileName,
						num + 1
					});
				}
				else
				{
					string configId = array[1].Trim();
					string text2 = array[0].Trim();
					string a;
					if (text2.EndsWith(".ActiveCfg"))
					{
						a = "ActiveCfg";
						text2 = text2.Substring(0, text2.Length - 10);
					}
					else if (text2.EndsWith(".Build.0"))
					{
						a = "Build.0";
						text2 = text2.Substring(0, text2.Length - 8);
					}
					else
					{
						if (!text2.EndsWith(".Deploy.0"))
						{
							LoggingService.LogWarning(GettextCatalog.GetString("{0} ({1}) : Unknown action. Only ActiveCfg, Build.0 and Deploy.0 supported.", sln.FileName, num + 1));
							goto IL_2EF;
						}
						a = "Deploy.0";
						text2 = text2.Substring(0, text2.Length - 9);
					}
					string[] array2 = text2.Split(new char[]
					{
						'.'
					}, 2);
					if (array2.Length < 2)
					{
						LoggingService.LogDebug("{0} ({1}) : Invalid format of the left side. Ignoring", new object[]
						{
							sln.FileName,
							num + 1
						});
					}
					else
					{
						string text3 = array2[0].ToUpper();
						string text4 = array2[1];
						if (!slnData.ItemsByGuid.ContainsKey(text3))
						{
							if (!dictionary2.ContainsKey(text3))
							{
								LoggingService.LogWarning(GettextCatalog.GetString("{0} ({1}) : Project with guid = '{2}' not found or not loaded. Ignoring", sln.FileName, num + 1, text3));
								dictionary2[text3] = text3;
							}
						}
						else
						{
							SolutionEntityItem solutionEntityItem;
							if (slnData.ItemsByGuid.TryGetValue(text3, out solutionEntityItem) && solutionEntityItem.SupportsConfigurations())
							{
								string key = text3 + "." + text4;
								SolutionConfigurationEntry solutionConfigurationEntry;
								if (dictionary.ContainsKey(key))
								{
									solutionConfigurationEntry = dictionary[key];
								}
								else
								{
									solutionConfigurationEntry = this.GetConfigEntry(sln, solutionEntityItem, text4);
									solutionConfigurationEntry.Build = false;
									dictionary[key] = solutionConfigurationEntry;
								}
								if (a == "ActiveCfg")
								{
									solutionConfigurationEntry.ItemConfiguration = this.FromSlnConfigurationId(configId);
								}
								else if (a == "Build.0")
								{
									solutionConfigurationEntry.Build = true;
								}
								else if (a == "Deploy.0")
								{
									solutionConfigurationEntry.Deploy = true;
								}
							}
							list.RemoveAt(list.Count - 1);
						}
					}
				}
				IL_2EF:;
			}
			slnData.SectionExtras["ProjectConfigurationPlatforms"] = list;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0004456C File Offset: 0x0004276C
		private SolutionConfigurationEntry GetConfigEntry(Solution sol, SolutionEntityItem item, string configName)
		{
			configName = this.FromSlnConfigurationId(configName);
			SolutionConfiguration solutionConfiguration = sol.Configurations[configName];
			if (solutionConfiguration == null)
			{
				solutionConfiguration = this.CreateSolutionConfigurationFromId(configName);
				sol.Configurations.Add(solutionConfiguration);
			}
			SolutionConfigurationEntry entryForItem = solutionConfiguration.GetEntryForItem(item);
			if (entryForItem != null)
			{
				return entryForItem;
			}
			return solutionConfiguration.AddItem(item);
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x000445BC File Offset: 0x000427BC
		private void LoadSolutionConfigurations(Section sec, List<string> lines, Solution solution, IProgressMonitor monitor)
		{
			if (sec == null || string.Compare(sec.Val, "preSolution", true) != 0)
			{
				return;
			}
			for (int i = 0; i < sec.Count - 2; i++)
			{
				int index = i + sec.Start + 1;
				string text = lines[index].Trim();
				if (text.Length != 0)
				{
					string text2 = this.FromSlnConfigurationId(this.SplitKeyValue(text).Key);
					if (solution.Configurations[text2] == null)
					{
						SolutionConfiguration item = this.CreateSolutionConfigurationFromId(text2);
						solution.Configurations.Add(item);
					}
				}
			}
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x00044654 File Offset: 0x00042854
		private SolutionConfiguration CreateSolutionConfigurationFromId(string fullId)
		{
			return new SolutionConfiguration(fullId);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0004465C File Offset: 0x0004285C
		private void LoadMonoDevelopProperties(Section sec, List<string> lines, Solution sln, IProgressMonitor monitor)
		{
			DataItem data = this.ReadDataItem(sec, lines);
			new MSBuildSerializer(sln.FileName)
			{
				SerializationContext = 
				{
					BaseFile = sln.FileName
				}
			}.Deserialize(sln, data);
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x000446A4 File Offset: 0x000428A4
		private void LoadMonoDevelopConfigurationProperties(string configName, Section sec, List<string> lines, Solution sln, IProgressMonitor monitor)
		{
			SolutionConfiguration solutionConfiguration = sln.Configurations[configName];
			if (solutionConfiguration == null)
			{
				return;
			}
			DataItem data = this.ReadDataItem(sec, lines);
			MSBuildSerializer msbuildSerializer = new MSBuildSerializer(sln.FileName);
			msbuildSerializer.Deserialize(solutionConfiguration, data);
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x000446E8 File Offset: 0x000428E8
		private void LoadNestedProjects(Section sec, List<string> lines, IDictionary<string, SolutionItem> entries, IProgressMonitor monitor)
		{
			if (sec == null || string.Compare(sec.Val, "preSolution", true) != 0)
			{
				return;
			}
			for (int i = 0; i < sec.Count - 2; i++)
			{
				KeyValuePair<string, string> keyValuePair = this.SplitKeyValue(lines[i + sec.Start + 1].Trim());
				keyValuePair = new KeyValuePair<string, string>(keyValuePair.Key.ToUpper(), keyValuePair.Value.ToUpper());
				SolutionItem solutionItem;
				if (!entries.TryGetValue(keyValuePair.Value, out solutionItem))
				{
					LoggingService.LogWarning(GettextCatalog.GetString("Project with guid '{0}' not found.", keyValuePair.Value));
				}
				else
				{
					SolutionFolder solutionFolder = solutionItem as SolutionFolder;
					SolutionItem item;
					if (solutionFolder == null)
					{
						LoggingService.LogWarning(GettextCatalog.GetString("Item with guid '{0}' is not a folder.", keyValuePair.Value));
					}
					else if (!entries.TryGetValue(keyValuePair.Key, out item))
					{
						LoggingService.LogWarning(GettextCatalog.GetString("Project with guid '{0}' not found.", keyValuePair.Key));
					}
					else
					{
						solutionFolder.Items.Add(item);
					}
				}
			}
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x000447E4 File Offset: 0x000429E4
		private string GetNextLine(StreamReader reader, List<string> list)
		{
			if (reader.Peek() < 0)
			{
				return null;
			}
			string text = reader.ReadLine();
			list.Add(text);
			return text;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0004480C File Offset: 0x00042A0C
		private int ReadUntil(string end, StreamReader reader, List<string> lines)
		{
			int result = -1;
			while (reader.Peek() >= 0)
			{
				string nextLine = this.GetNextLine(reader, lines);
				if (string.Compare(nextLine.Trim(), end, true) == 0)
				{
					return lines.Count - 1;
				}
			}
			return result;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00044848 File Offset: 0x00042A48
		private KeyValuePair<string, string> SplitKeyValue(string s)
		{
			string[] array = s.Split(new char[]
			{
				'='
			}, 2);
			string key = array[0].Trim();
			string value = string.Empty;
			if (array.Length == 2)
			{
				value = array[1].Trim();
			}
			return new KeyValuePair<string, string>(key, value);
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00044890 File Offset: 0x00042A90
		private string GetSlnFileVersion(string strInSlnFile, out string headerComment)
		{
			string result = null;
			headerComment = null;
			StreamReader streamReader = new StreamReader(strInSlnFile);
			string text = streamReader.ReadLine();
			if (text == null)
			{
				return null;
			}
			Match match = SlnFileFormat.SlnVersionRegex.Match(text);
			if (!match.Success)
			{
				text = streamReader.ReadLine();
				if (text == null)
				{
					return null;
				}
				match = SlnFileFormat.SlnVersionRegex.Match(text);
			}
			if (match.Success)
			{
				result = match.Groups[1].Value;
				headerComment = streamReader.ReadLine();
			}
			streamReader.Close();
			return result;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0004490C File Offset: 0x00042B0C
		private static SlnData GetSlnData(SolutionItem c)
		{
			if (c.ExtendedProperties.Contains(typeof(SlnFileFormat)))
			{
				return c.ExtendedProperties[typeof(SlnFileFormat)] as SlnData;
			}
			return null;
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001104 RID: 4356 RVA: 0x00044941 File Offset: 0x00042B41
		internal static Regex ProjectRegex
		{
			get
			{
				if (SlnFileFormat.projectRegex == null)
				{
					SlnFileFormat.projectRegex = new Regex("Project\\(\"(\\{[^}]*\\})\"\\) = \"(.*)\", \"(.*)\", \"(\\{[^{]*\\})\"");
				}
				return SlnFileFormat.projectRegex;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x0004495E File Offset: 0x00042B5E
		private static Regex GlobalSectionRegex
		{
			get
			{
				if (SlnFileFormat.globalSectionRegex == null)
				{
					SlnFileFormat.globalSectionRegex = new Regex("GlobalSection\\s*\\(([^)]*)\\)\\s*=\\s*(\\w*)");
				}
				return SlnFileFormat.globalSectionRegex;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001106 RID: 4358 RVA: 0x0004497B File Offset: 0x00042B7B
		internal static Regex SlnVersionRegex
		{
			get
			{
				if (SlnFileFormat.slnVersionRegex == null)
				{
					SlnFileFormat.slnVersionRegex = new Regex("Microsoft Visual Studio Solution File, Format Version (\\d?\\d.\\d\\d)");
				}
				return SlnFileFormat.slnVersionRegex;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x00044998 File Offset: 0x00042B98
		public string Name
		{
			get
			{
				return "MSBuild";
			}
		}

		// Token: 0x040004E3 RID: 1251
		private static Regex projectRegex;

		// Token: 0x040004E4 RID: 1252
		private static Regex globalSectionRegex;

		// Token: 0x040004E5 RID: 1253
		private static Regex slnVersionRegex;
	}
}
