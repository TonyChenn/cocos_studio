using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Execution;
using MonoDevelop.Core.ProgressMonitoring;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001BE RID: 446
	public static class MSBuildProjectService
	{
		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x0600110D RID: 4365 RVA: 0x000449E4 File Offset: 0x00042BE4
		// (set) Token: 0x0600110E RID: 4366 RVA: 0x000449EB File Offset: 0x00042BEB
		internal static bool ShutDown { get; private set; }

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x000449F4 File Offset: 0x00042BF4
		public static DataContext DataContext
		{
			get
			{
				if (MSBuildProjectService.dataContext == null)
				{
					MSBuildProjectService.dataContext = new MSBuildDataContext();
					Services.ProjectService.InitializeDataContext(MSBuildProjectService.dataContext);
					foreach (ItemMember itemMember in MSBuildProjectHandler.ExtendedMSBuildProperties)
					{
						ItemProperty itemProperty = new ItemProperty(itemMember.Name, itemMember.Type);
						itemProperty.IsExternal = itemMember.IsExternal;
						if (itemMember.CustomAttributes != null)
						{
							itemProperty.CustomAttributes = itemMember.CustomAttributes;
						}
						MSBuildProjectService.dataContext.RegisterProperty(itemMember.DeclaringType, itemProperty);
					}
				}
				return MSBuildProjectService.dataContext;
			}
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00044AA8 File Offset: 0x00042CA8
		static MSBuildProjectService()
		{
			Services.ProjectService.DataContextChanged += delegate(object param0, EventArgs param1)
			{
				MSBuildProjectService.dataContext = null;
			};
			PropertyService.PropertyChanged += MSBuildProjectService.HandlePropertyChanged;
			MSBuildProjectService.DefaultMSBuildVerbosity = PropertyService.Get<MSBuildVerbosity>("MonoDevelop.Ide.MSBuildVerbosity", MSBuildVerbosity.Normal);
			Runtime.ShuttingDown += delegate(object sender, EventArgs e)
			{
				MSBuildProjectService.ShutDown = true;
			};
			MSBuildProjectService.globalPropertyProviders = AddinManager.GetExtensionObjects<IMSBuildGlobalPropertyProvider>("/MonoDevelop/ProjectModel/MSBuildGlobalPropertyProviders");
			foreach (IMSBuildGlobalPropertyProvider imsbuildGlobalPropertyProvider in MSBuildProjectService.globalPropertyProviders)
			{
				imsbuildGlobalPropertyProvider.GlobalPropertiesChanged += MSBuildProjectService.HandleGlobalPropertyProviderChanged;
			}
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00044B88 File Offset: 0x00042D88
		private static void HandleGlobalPropertyProviderChanged(object sender, EventArgs e)
		{
			lock (MSBuildProjectService.builders)
			{
				IMSBuildGlobalPropertyProvider imsbuildGlobalPropertyProvider = (IMSBuildGlobalPropertyProvider)sender;
				foreach (RemoteBuildEngine remoteBuildEngine in MSBuildProjectService.builders.Values)
				{
					remoteBuildEngine.SetGlobalProperties(imsbuildGlobalPropertyProvider.GetGlobalProperties());
				}
			}
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00044C14 File Offset: 0x00042E14
		private static void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.Key == "MonoDevelop.Ide.MSBuildVerbosity")
			{
				MSBuildProjectService.DefaultMSBuildVerbosity = (MSBuildVerbosity)e.NewValue;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x00044C38 File Offset: 0x00042E38
		// (set) Token: 0x06001114 RID: 4372 RVA: 0x00044C3F File Offset: 0x00042E3F
		internal static MSBuildVerbosity DefaultMSBuildVerbosity { get; private set; }

		// Token: 0x06001115 RID: 4373 RVA: 0x00044C48 File Offset: 0x00042E48
		public static SolutionEntityItem LoadItem(IProgressMonitor monitor, string fileName, MSBuildFileFormat expectedFormat, string typeGuid, string itemGuid)
		{
			foreach (ItemTypeNode itemTypeNode in MSBuildProjectService.GetItemTypeNodes())
			{
				if (itemTypeNode.CanHandleFile(fileName, typeGuid))
				{
					return itemTypeNode.LoadSolutionItem(monitor, fileName, expectedFormat, itemGuid);
				}
			}
			if (string.IsNullOrEmpty(typeGuid) && MSBuildProjectService.IsProjectSubtypeFile(fileName))
			{
				typeGuid = MSBuildProjectService.LoadProjectTypeGuids(fileName);
				foreach (ItemTypeNode itemTypeNode2 in MSBuildProjectService.GetItemTypeNodes())
				{
					if (itemTypeNode2.CanHandleFile(fileName, typeGuid))
					{
						return itemTypeNode2.LoadSolutionItem(monitor, fileName, expectedFormat, itemGuid);
					}
				}
			}
			UnknownProjectTypeNode unknownProjectTypeInfo = MSBuildProjectService.GetUnknownProjectTypeInfo((typeGuid != null) ? new string[]
			{
				typeGuid
			} : new string[0], fileName);
			if (unknownProjectTypeInfo != null && unknownProjectTypeInfo.LoadFiles)
			{
				if (typeGuid == null)
				{
					typeGuid = unknownProjectTypeInfo.Guid;
				}
				MSBuildProjectHandler msbuildProjectHandler = new MSBuildProjectHandler(typeGuid, "", itemGuid);
				msbuildProjectHandler.SetUnsupportedType(unknownProjectTypeInfo);
				return msbuildProjectHandler.Load(monitor, fileName, expectedFormat, "", null);
			}
			return null;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00044D80 File Offset: 0x00042F80
		internal static IResourceHandler GetResourceHandlerForItem(DotNetProject project)
		{
			foreach (ItemTypeNode itemTypeNode in MSBuildProjectService.GetItemTypeNodes())
			{
				DotNetProjectNode dotNetProjectNode = itemTypeNode as DotNetProjectNode;
				if (dotNetProjectNode != null && dotNetProjectNode.CanHandleItem(project))
				{
					return dotNetProjectNode.GetResourceHandler();
				}
			}
			return new MSBuildResourceHandler();
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00044DE8 File Offset: 0x00042FE8
		internal static MSBuildHandler GetItemHandler(SolutionEntityItem item)
		{
			MSBuildHandler msbuildHandler = item.ItemHandler as MSBuildHandler;
			if (msbuildHandler != null)
			{
				return msbuildHandler;
			}
			throw new InvalidOperationException("Not an MSBuild project");
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00044E10 File Offset: 0x00043010
		internal static void SetId(SolutionItem item, string id)
		{
			MSBuildHandler msbuildHandler = item.ItemHandler as MSBuildHandler;
			if (msbuildHandler != null)
			{
				msbuildHandler.ItemId = id;
				return;
			}
			throw new InvalidOperationException("Not an MSBuild project");
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00044E40 File Offset: 0x00043040
		internal static void InitializeItemHandler(SolutionItem item)
		{
			SolutionEntityItem solutionEntityItem = item as SolutionEntityItem;
			if (solutionEntityItem != null)
			{
				using (IEnumerator<ItemTypeNode> enumerator = MSBuildProjectService.GetItemTypeNodes().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ItemTypeNode itemTypeNode = enumerator.Current;
						if (itemTypeNode.CanHandleItem(solutionEntityItem))
						{
							itemTypeNode.InitializeHandler(solutionEntityItem);
							foreach (DotNetProjectSubtypeNode dotNetProjectSubtypeNode in MSBuildProjectService.GetItemSubtypeNodes())
							{
								if (dotNetProjectSubtypeNode.CanHandleItem(solutionEntityItem))
								{
									dotNetProjectSubtypeNode.InitializeHandler(solutionEntityItem);
								}
							}
							break;
						}
					}
					return;
				}
			}
			if (item is SolutionFolder)
			{
				item.SetItemHandler(new MSBuildHandler("{2150E333-8FDC-42A3-9474-1A3956D46DE8}", null)
				{
					Item = item
				});
			}
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00044F14 File Offset: 0x00043114
		public static bool SupportsProjectType(string projectFile)
		{
			if (!string.IsNullOrWhiteSpace(projectFile))
			{
				try
				{
					using (ConsoleProgressMonitor consoleProgressMonitor = new ConsoleProgressMonitor())
					{
						return MSBuildProjectService.LoadItem(consoleProgressMonitor, projectFile, null, null, null) != null;
					}
				}
				catch
				{
					return false;
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00044F70 File Offset: 0x00043170
		public static void CheckHandlerUsesMSBuildEngine(SolutionItem item, out bool useByDefault, out bool require)
		{
			MSBuildProjectHandler msbuildProjectHandler = item.ItemHandler as MSBuildProjectHandler;
			if (msbuildProjectHandler == null)
			{
				useByDefault = (require = false);
				return;
			}
			useByDefault = msbuildProjectHandler.UseMSBuildEngineByDefault;
			require = msbuildProjectHandler.RequireMSBuildEngine;
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00044FB0 File Offset: 0x000431B0
		internal static DotNetProjectSubtypeNode GetDotNetProjectSubtype(string typeGuids)
		{
			if (!string.IsNullOrEmpty(typeGuids))
			{
				return MSBuildProjectService.GetDotNetProjectSubtype(from t in typeGuids.Split(new char[]
				{
					';'
				})
				select t.Trim());
			}
			return null;
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00045004 File Offset: 0x00043204
		internal static DotNetProjectSubtypeNode GetDotNetProjectSubtype(IEnumerable<string> typeGuids)
		{
			Type type = null;
			DotNetProjectSubtypeNode result = null;
			foreach (string guid in typeGuids)
			{
				foreach (DotNetProjectSubtypeNode dotNetProjectSubtypeNode in MSBuildProjectService.GetItemSubtypeNodes())
				{
					if (dotNetProjectSubtypeNode.SupportsType(guid) && (type == null || type.IsAssignableFrom(dotNetProjectSubtypeNode.Type)))
					{
						type = dotNetProjectSubtypeNode.Type;
						result = dotNetProjectSubtypeNode;
					}
				}
			}
			return result;
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0004527C File Offset: 0x0004347C
		private static IEnumerable<ItemTypeNode> GetItemTypeNodes()
		{
			foreach (object obj in AddinManager.GetExtensionNodes("/MonoDevelop/ProjectModel/MSBuildItemTypes"))
			{
				ExtensionNode node = (ExtensionNode)obj;
				if (node is ItemTypeNode)
				{
					yield return (ItemTypeNode)node;
				}
			}
			yield return MSBuildProjectService.genericItemTypeNode;
			yield break;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00045438 File Offset: 0x00043638
		internal static IEnumerable<DotNetProjectSubtypeNode> GetItemSubtypeNodes()
		{
			foreach (object obj in AddinManager.GetExtensionNodes("/MonoDevelop/ProjectModel/MSBuildItemTypes"))
			{
				ExtensionNode node = (ExtensionNode)obj;
				if (node is DotNetProjectSubtypeNode)
				{
					yield return (DotNetProjectSubtypeNode)node;
				}
			}
			yield break;
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00045450 File Offset: 0x00043650
		internal static bool CanReadFile(FilePath file)
		{
			foreach (ItemTypeNode itemTypeNode in MSBuildProjectService.GetItemTypeNodes())
			{
				if (itemTypeNode.CanHandleFile(file, null))
				{
					return true;
				}
			}
			if (MSBuildProjectService.IsProjectSubtypeFile(file))
			{
				string typeGuid = MSBuildProjectService.LoadProjectTypeGuids(file);
				foreach (ItemTypeNode itemTypeNode2 in MSBuildProjectService.GetItemTypeNodes())
				{
					if (itemTypeNode2.CanHandleFile(file, typeGuid))
					{
						return true;
					}
				}
			}
			return MSBuildProjectService.GetUnknownProjectTypeInfo(new string[0], file) != null;
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x0004552C File Offset: 0x0004372C
		internal static string GetExtensionForItem(SolutionEntityItem item)
		{
			foreach (DotNetProjectSubtypeNode dotNetProjectSubtypeNode in MSBuildProjectService.GetItemSubtypeNodes())
			{
				if (!string.IsNullOrEmpty(dotNetProjectSubtypeNode.Extension) && dotNetProjectSubtypeNode.CanHandleItem(item))
				{
					return dotNetProjectSubtypeNode.Extension;
				}
			}
			foreach (ItemTypeNode itemTypeNode in MSBuildProjectService.GetItemTypeNodes())
			{
				if (itemTypeNode.CanHandleItem(item))
				{
					return itemTypeNode.Extension;
				}
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x000455E4 File Offset: 0x000437E4
		private static bool IsProjectSubtypeFile(FilePath file)
		{
			foreach (DotNetProjectSubtypeNode dotNetProjectSubtypeNode in MSBuildProjectService.GetItemSubtypeNodes())
			{
				if (!string.IsNullOrEmpty(dotNetProjectSubtypeNode.Extension) && dotNetProjectSubtypeNode.CanHandleFile(file, null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x0004564C File Offset: 0x0004384C
		public static string EscapeString(string str)
		{
			for (int num = str.IndexOfAny(MSBuildProjectService.specialCharacters); num != -1; num = str.IndexOfAny(MSBuildProjectService.specialCharacters, num + 3))
			{
				str = string.Concat(new object[]
				{
					str.Substring(0, num),
					'%',
					((int)str[num]).ToString("X"),
					str.Substring(num + 1)
				});
			}
			return str;
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x000456C2 File Offset: 0x000438C2
		public static string UnescapePath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return path;
			}
			if (!Platform.IsWindows)
			{
				path = path.Replace("\\", "/");
			}
			return MSBuildProjectService.UnscapeString(path);
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x000456F0 File Offset: 0x000438F0
		public static string UnscapeString(string str)
		{
			int num = str.IndexOf('%');
			while (num != -1 && num < str.Length - 2)
			{
				int num2;
				if (int.TryParse(str.Substring(num + 1, 2), NumberStyles.HexNumber, null, out num2))
				{
					str = str.Substring(0, num) + (char)num2 + str.Substring(num + 3);
				}
				num = str.IndexOf('%', num + 1);
			}
			return str;
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x0004575C File Offset: 0x0004395C
		public static string ToMSBuildPath(string baseDirectory, string absPath)
		{
			if (baseDirectory != null)
			{
				absPath = FileService.NormalizeRelativePath(FileService.AbsoluteToRelativePath(baseDirectory, absPath));
			}
			return MSBuildProjectService.EscapeString(absPath).Replace('/', '\\');
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00045780 File Offset: 0x00043980
		internal static string ToMSBuildPathRelative(string baseDirectory, string absPath)
		{
			return MSBuildProjectService.ToMSBuildPath(baseDirectory, absPath).ToRelative(baseDirectory);
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x000457AC File Offset: 0x000439AC
		internal static string FromMSBuildPathRelative(string basePath, string relPath)
		{
			return MSBuildProjectService.FromMSBuildPath(basePath, relPath).ToRelative(basePath);
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x000457D8 File Offset: 0x000439D8
		public static string FromMSBuildPath(string basePath, string relPath)
		{
			string result;
			MSBuildProjectService.FromMSBuildPath(basePath, relPath, out result);
			return result;
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x000457F0 File Offset: 0x000439F0
		internal static bool IsAbsoluteMSBuildPath(string path)
		{
			return (path.Length > 1 && char.IsLetter(path[0]) && path[1] == ':') || (path.Length > 0 && path[0] == '\\');
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00045830 File Offset: 0x00043A30
		internal static bool FromMSBuildPath(string basePath, string relPath, out string resultPath)
		{
			resultPath = relPath;
			if (string.IsNullOrEmpty(relPath))
			{
				return false;
			}
			string text = MSBuildProjectService.UnescapePath(relPath);
			if (char.IsLetter(text[0]) && text.Length > 1 && text[1] == ':')
			{
				if (Platform.IsWindows)
				{
					resultPath = text;
					return true;
				}
				return false;
			}
			else
			{
				bool flag = Path.IsPathRooted(text);
				if (!flag && basePath != null)
				{
					text = Path.Combine(basePath, text);
					flag = Path.IsPathRooted(text);
				}
				if (!flag)
				{
					resultPath = FileService.NormalizeRelativePath(text);
					return true;
				}
				if (Platform.IsWindows)
				{
					resultPath = FileService.GetFullPath(text);
					return true;
				}
				if (File.Exists(text) || Directory.Exists(text))
				{
					resultPath = Path.GetFullPath(text);
					return true;
				}
				string[] array = text.Substring(1).Split(new char[]
				{
					'/'
				});
				string text2 = "/";
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == "..")
					{
						if (text2 == "/")
						{
							return false;
						}
						text2 = Path.GetFullPath(text2 + "/..");
					}
					else
					{
						string[] fileSystemEntries = Directory.GetFileSystemEntries(text2);
						string text3 = null;
						foreach (string text4 in fileSystemEntries)
						{
							if (string.Compare(Path.GetFileName(text4), array[i], StringComparison.OrdinalIgnoreCase) == 0)
							{
								text3 = text4;
								break;
							}
						}
						if (text3 == null)
						{
							text2 = Path.GetFullPath(text2);
							while (i < array.Length)
							{
								text2 = text2 + "/" + array[i];
								i++;
							}
							resultPath = text2;
							return true;
						}
						text2 = text3;
					}
				}
				resultPath = Path.GetFullPath(text2);
				return true;
			}
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x000459C0 File Offset: 0x00043BC0
		public static bool TrySplitResourceName(string fname, out string only_filename, out string culture, out string extn)
		{
			string text;
			extn = (text = null);
			string text2;
			culture = (text2 = text);
			only_filename = text2;
			int num = -1;
			int num2 = -1;
			int i;
			for (i = fname.Length - 1; i >= 0; i--)
			{
				if (fname[i] == '.')
				{
					num = i;
					break;
				}
			}
			if (i < 0)
			{
				return false;
			}
			for (i--; i >= 0; i--)
			{
				if (fname[i] == '.')
				{
					num2 = i;
					break;
				}
			}
			if (num2 < 0)
			{
				return false;
			}
			culture = fname.Substring(num2 + 1, num - num2 - 1);
			if (!MSBuildProjectService.CultureNamesTable.ContainsKey(culture))
			{
				return false;
			}
			only_filename = fname.Substring(0, num2);
			extn = fname.Substring(num + 1);
			return true;
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x00045B20 File Offset: 0x00043D20
		internal static RemoteProjectBuilder GetProjectBuilder(TargetRuntime runtime, string minToolsVersion, string file, string solutionFile)
		{
			bool flag = false;
			RemoteProjectBuilder result;
			try
			{
				Dictionary<string, RemoteBuildEngine> obj;
				Monitor.Enter(obj = MSBuildProjectService.builders, ref flag);
				string text = "12.0";
				if (runtime.GetMSBuildBinPath("12.0") == null)
				{
					text = "4.0";
				}
				Version v;
				Version v2;
				if (Version.TryParse(text, out v) && Version.TryParse(minToolsVersion, out v2) && v < v2)
				{
					string text2 = null;
					if (runtime is MsNetTargetRuntime && minToolsVersion == "12.0")
					{
						text2 = "MSBuild 2013 is not installed. Please download and install it from http://www.microsoft.com/en-us/download/details.aspx?id=40760";
					}
					throw new InvalidOperationException(text2 ?? string.Format("Runtime '{0}' does not have MSBuild '{1}' ToolsVersion installed", runtime.Id, text));
				}
				string builderKey = runtime.Id + " # " + solutionFile;
				RemoteBuildEngine remoteBuildEngine;
				if (MSBuildProjectService.builders.TryGetValue(builderKey, out remoteBuildEngine))
				{
					remoteBuildEngine.ReferenceCount++;
					result = new RemoteProjectBuilder(file, remoteBuildEngine);
				}
				else
				{
					string exeLocation = MSBuildProjectService.GetExeLocation(runtime, text);
					RemotingService.RegisterRemotingChannel();
					ProcessStartInfo pinfo = new ProcessStartInfo(exeLocation)
					{
						UseShellExecute = false,
						CreateNoWindow = true,
						RedirectStandardError = true,
						RedirectStandardInput = true
					};
					runtime.GetToolsExecutionEnvironment().MergeTo(pinfo);
					Process process = null;
					try
					{
						IBuildEngine buildEngine;
						if (!MSBuildProjectService.runLocal)
						{
							process = runtime.ExecuteAssembly(pinfo);
							ManualResetEvent ev = new ManualResetEvent(false);
							string responseKey = "[MonoDevelop]";
							string sref = null;
							process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
							{
								if (e.Data == null)
								{
									return;
								}
								if (e.Data.StartsWith(responseKey, StringComparison.Ordinal))
								{
									sref = e.Data.Substring(responseKey.Length);
									ev.Set();
									return;
								}
								Console.WriteLine(e.Data);
							};
							process.BeginErrorReadLine();
							process.StandardInput.WriteLine(Process.GetCurrentProcess().Id.ToString());
							if (!ev.WaitOne(TimeSpan.FromSeconds(5.0)))
							{
								throw new Exception("MSBuild process could not be started");
							}
							byte[] buffer = Convert.FromBase64String(sref);
							MemoryStream serializationStream = new MemoryStream(buffer);
							BinaryFormatter binaryFormatter = new BinaryFormatter();
							buildEngine = (IBuildEngine)binaryFormatter.Deserialize(serializationStream);
						}
						else
						{
							Assembly assembly = Assembly.LoadFrom(exeLocation);
							Type type = assembly.GetType("MonoDevelop.Projects.Formats.MSBuild.BuildEngine");
							buildEngine = (IBuildEngine)Activator.CreateInstance(type);
						}
						buildEngine.SetCulture(GettextCatalog.UICulture);
						buildEngine.SetGlobalProperties(MSBuildProjectService.GetCoreGlobalProperties(solutionFile));
						foreach (IMSBuildGlobalPropertyProvider imsbuildGlobalPropertyProvider in MSBuildProjectService.globalPropertyProviders)
						{
							remoteBuildEngine.SetGlobalProperties(imsbuildGlobalPropertyProvider.GetGlobalProperties());
						}
						remoteBuildEngine = new RemoteBuildEngine(process, buildEngine);
					}
					catch
					{
						if (process != null)
						{
							try
							{
								process.Kill();
							}
							catch
							{
							}
						}
						throw;
					}
					MSBuildProjectService.builders[builderKey] = remoteBuildEngine;
					remoteBuildEngine.ReferenceCount = 1;
					remoteBuildEngine.Disconnected += delegate(object param0, EventArgs param1)
					{
						lock (MSBuildProjectService.builders)
						{
							MSBuildProjectService.builders.Remove(builderKey);
						}
					};
					result = new RemoteProjectBuilder(file, remoteBuildEngine);
				}
			}
			finally
			{
				if (flag)
				{
					Dictionary<string, RemoteBuildEngine> obj;
					Monitor.Exit(obj);
				}
			}
			return result;
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00045E48 File Offset: 0x00044048
		private static IDictionary<string, string> GetCoreGlobalProperties(string slnFile)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("BuildingInsideVisualStudio", "true");
			dictionary.Add("UseHostCompilerIfAvailable", "false");
			if (string.IsNullOrEmpty(slnFile))
			{
				return dictionary;
			}
			dictionary.Add("SolutionPath", Path.GetFullPath(slnFile));
			dictionary.Add("SolutionName", Path.GetFileNameWithoutExtension(slnFile));
			dictionary.Add("SolutionFilename", Path.GetFileName(slnFile));
			dictionary.Add("SolutionDir", Path.GetDirectoryName(slnFile) + Path.DirectorySeparatorChar);
			return dictionary;
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x00045EDC File Offset: 0x000440DC
		private static string GetExeLocation(TargetRuntime runtime, string toolsVersion)
		{
			FilePath filePath = typeof(MSBuildProjectService).Assembly.Location;
			if (runtime is MsNetTargetRuntime && int.Parse(toolsVersion.Split(new char[]
			{
				'.'
			})[0]) >= 4)
			{
				toolsVersion = "dotnet." + toolsVersion;
			}
			FilePath filePath2 = filePath.ParentDirectory.Combine(new string[]
			{
				"MSBuild",
				toolsVersion,
				"MonoDevelop.Projects.Formats.MSBuild.exe"
			});
			if (File.Exists(filePath2))
			{
				return filePath2;
			}
			throw new InvalidOperationException("Unsupported MSBuild ToolsVersion '" + toolsVersion + "'");
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00045FA8 File Offset: 0x000441A8
		internal static void ReleaseProjectBuilder(RemoteBuildEngine engine)
		{
			lock (MSBuildProjectService.builders)
			{
				if (--engine.ReferenceCount != 0)
				{
					return;
				}
				MSBuildProjectService.builders.Remove(MSBuildProjectService.builders.First((KeyValuePair<string, RemoteBuildEngine> kvp) => kvp.Value == engine).Key);
			}
			engine.Dispose();
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001131 RID: 4401 RVA: 0x00046048 File Offset: 0x00044248
		private static Dictionary<string, string> CultureNamesTable
		{
			get
			{
				if (MSBuildProjectService.cultureNamesTable == null)
				{
					MSBuildProjectService.cultureNamesTable = new Dictionary<string, string>();
					foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
					{
						MSBuildProjectService.cultureNamesTable[cultureInfo.Name] = cultureInfo.Name;
					}
				}
				return MSBuildProjectService.cultureNamesTable;
			}
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x0004609C File Offset: 0x0004429C
		private static string LoadProjectTypeGuids(string fileName)
		{
			MSBuildProject msbuildProject = new MSBuildProject();
			msbuildProject.Load(fileName);
			MSBuildPropertySet globalPropertyGroup = msbuildProject.GetGlobalPropertyGroup();
			if (globalPropertyGroup == null)
			{
				return null;
			}
			return globalPropertyGroup.GetPropertyValue("ProjectTypeGuids", false);
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00046124 File Offset: 0x00044324
		internal static UnknownProjectTypeNode GetUnknownProjectTypeInfo(string[] guids, string fileName = null)
		{
			string ext = (fileName != null) ? Path.GetExtension(fileName).TrimStart(new char[]
			{
				'.'
			}) : null;
			List<UnknownProjectTypeNode> source = (from p in AddinManager.GetExtensionNodes<UnknownProjectTypeNode>("/MonoDevelop/ProjectModel/UnknownMSBuildProjectTypes")
			where guids.Any(new Func<string, bool>(p.MatchesGuid)) || (ext != null && p.Extension == ext)
			select p).ToList<UnknownProjectTypeNode>();
			UnknownProjectTypeNode result;
			if ((result = source.FirstOrDefault((UnknownProjectTypeNode n) => !n.IsSolvable)) == null)
			{
				result = source.FirstOrDefault((UnknownProjectTypeNode n) => n.IsSolvable);
			}
			return result;
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x000461CD File Offset: 0x000443CD
		public static MSBuildProjectHandler GetHandler(Project project)
		{
			return (MSBuildProjectHandler)project.GetItemHandler();
		}

		// Token: 0x040004EB RID: 1259
		private const string ItemTypesExtensionPath = "/MonoDevelop/ProjectModel/MSBuildItemTypes";

		// Token: 0x040004EC RID: 1260
		public const string GenericItemGuid = "{9344BDBB-3E7F-41FC-A0DD-8665D75EE146}";

		// Token: 0x040004ED RID: 1261
		public const string FolderTypeGuid = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";

		// Token: 0x040004EE RID: 1262
		public const string DefaultFormat = "MSBuild12";

		// Token: 0x040004EF RID: 1263
		private static DataContext dataContext;

		// Token: 0x040004F0 RID: 1264
		private static IMSBuildGlobalPropertyProvider[] globalPropertyProviders;

		// Token: 0x040004F1 RID: 1265
		private static Dictionary<string, RemoteBuildEngine> builders = new Dictionary<string, RemoteBuildEngine>();

		// Token: 0x040004F2 RID: 1266
		private static GenericItemTypeNode genericItemTypeNode = new GenericItemTypeNode();

		// Token: 0x040004F3 RID: 1267
		private static char[] specialCharacters = new char[]
		{
			'%',
			'$',
			'@',
			'(',
			')',
			'\'',
			';',
			'?'
		};

		// Token: 0x040004F4 RID: 1268
		private static bool runLocal = false;

		// Token: 0x040004F5 RID: 1269
		private static Dictionary<string, string> cultureNamesTable;
	}
}
