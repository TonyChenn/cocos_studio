using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000003 RID: 3
	internal abstract class BaseCocosSupplyment : ICocosSupplyment
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4
		public abstract int Order { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5
		protected abstract EnumSolutionCodeType supplymentedType { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000006 RID: 6
		protected abstract bool needBackup { get; }

		// Token: 0x06000007 RID: 7 RVA: 0x00002050 File Offset: 0x00000250
		public bool CanSupplyment(string frameworkVersion)
		{
			return this.OnCanSupplyment(frameworkVersion);
		}

		// Token: 0x06000008 RID: 8
		protected abstract bool OnCanSupplyment(string frameworkVersion);

		// Token: 0x06000009 RID: 9 RVA: 0x0000205C File Offset: 0x0000025C
		public bool RunSupplyment(string frameworkVersion, EnumProgramLanguage language, CocosMonitor monitor)
		{
			monitor.Start();
			string sourceDir = Services.ProjectsService.CurrentSolution.BaseDirectory;
			if (this.needBackup && !this.BackupSolution(sourceDir, monitor))
			{
				monitor.SendInfo("Failed to backup project");
				return false;
			}
			string tempDir = this.GetTempDir(sourceDir, "temp");
			if (!this.OnCreateTempSolution(tempDir, frameworkVersion, language, monitor))
			{
				monitor.SendInfo("Failed to create temp project");
				return false;
			}
			bool flag = this.OnRunSupplyment(frameworkVersion, language, tempDir, monitor);
			if (Directory.Exists(tempDir))
			{
				try
				{
					Directory.Delete(tempDir, true);
				}
				catch (Exception exception)
				{
					monitor.SendInfo("Failed to delete temp project");
					LogConfig.Logger.Error("删除临时解决方案时出错", exception);
				}
			}
			if (flag)
			{
				this.SetSolutionProperties(language, frameworkVersion);
			}
			monitor.Finish(flag);
			return flag;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002128 File Offset: 0x00000328
		protected virtual bool OnCreateTempSolution(string dir, string frameworkVersion, EnumProgramLanguage language, CocosMonitor monitor)
		{
			return true;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000212C File Offset: 0x0000032C
		protected bool ToCreateSolution(string dir, Cocos2dxInfo cocosInfo, EnumProgramLanguage language, CocosMonitor monitor)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string android_PackageName = PackageServices.Instance.PackageParams.Android_PackageName;
			string name = Services.ProjectsService.CurrentSolution.Name;
			stringBuilder.Append(string.Format(" new -p {0} -l {1} -d {2} {3}", new object[]
			{
				android_PackageName,
				language,
				dir,
				name
			}));
			if (language != EnumProgramLanguage.cpp)
			{
				stringBuilder.Append(" -t runtime");
			}
			string cmd = stringBuilder.ToString();
			CocosPythonTool cocosPythonTool = new CocosPythonTool(monitor);
			return cocosPythonTool.RunPython(cocosInfo, cmd, false);
		}

		// Token: 0x0600000C RID: 12
		protected abstract bool OnRunSupplyment(string frameworkVersion, EnumProgramLanguage language, string tempDir, CocosMonitor monitor);

		// Token: 0x0600000D RID: 13 RVA: 0x000021BC File Offset: 0x000003BC
		private void SetSolutionProperties(EnumProgramLanguage language, string frameworkVersion)
		{
			Cocos2dxServices.CocosProperties.SolutionCodeType = this.supplymentedType;
			Cocos2dxServices.CocosProperties.ProgramLanguage = language;
			Cocos2dxServices.CocosProperties.CurrentFrameworkVersion = frameworkVersion;
			Solution currentSolution = Services.ProjectsService.CurrentSolution;
			if (Cocos2dxServices.SupplymentServices.CheckNeedResetPublishDir(language))
			{
				currentSolution.Config.PublishDirectory = Cocos2dxServices.SupplymentServices.GetDefaultPublishDir(language);
				currentSolution.SetPublishDirectory();
			}
			if (language == EnumProgramLanguage.js)
			{
				currentSolution.Config.DefaultSerializer = "Serializer_Json";
				currentSolution.Config.CustomSerializer = "Serializer_Json";
			}
			else
			{
				currentSolution.Config.DefaultSerializer = "Serializer_FlatBuffers";
				currentSolution.Config.CustomSerializer = "Serializer_FlatBuffers";
			}
			currentSolution.Config.Save();
			PackageServices.Instance.InitPackageParams();
			PackageServices.Instance.PackageParams.FrameworkVersion = frameworkVersion;
			Version v = FrameworkHelper.TryParseVersion(frameworkVersion);
			if (v != null && v > new Version("3.6"))
			{
				string value = currentSolution.Name + " iOS";
				if (PackageServices.Instance.PackageParams.iOS_Target.Equals(value))
				{
					PackageServices.Instance.PackageParams.iOS_Target = currentSolution.Name + "-mobile";
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022F8 File Offset: 0x000004F8
		private string GetTempDir(string sourceDir, string suffix)
		{
			string fileName = Path.GetFileName(sourceDir);
			string directoryName = Path.GetDirectoryName(sourceDir);
			string text = Path.Combine(directoryName, fileName + "-" + suffix);
			int num = 1;
			while (Directory.Exists(text))
			{
				text = Path.Combine(directoryName, string.Concat(new object[]
				{
					fileName,
					"-",
					suffix,
					num
				}));
				num++;
			}
			return text;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000236C File Offset: 0x0000056C
		protected bool CopySourceCode(string originSlnDir, string tempDir, CocosMonitor monitor)
		{
			string name = Services.ProjectsService.CurrentSolution.Name;
			string path = Path.Combine(tempDir, name);
			string[] files = Directory.GetFiles(path);
			List<string> excludeFiles = this.GetExcludeFiles();
			foreach (string text in files)
			{
				string fileName = Path.GetFileName(text);
				if (!excludeFiles.Contains(fileName))
				{
					string text2 = Path.Combine(originSlnDir, fileName);
					try
					{
						File.Copy(text, text2, true);
					}
					catch (Exception exception)
					{
						monitor.SendInfo(string.Format("Failed to copy file {0} to {1}", text, text2));
						LogConfig.Logger.Error(string.Format("将文件{0}复制到{1}时出错", text, text2), exception);
						return false;
					}
				}
			}
			string[] directories = Directory.GetDirectories(path);
			List<string> excludeFolders = this.GetExcludeFolders();
			foreach (string text3 in directories)
			{
				string fileName2 = Path.GetFileName(text3);
				if (!excludeFolders.Contains(fileName2))
				{
					string dstPath = Path.Combine(originSlnDir, fileName2);
					if (!Cocos2dxServices.CopyFolder(text3, dstPath, false, monitor))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002490 File Offset: 0x00000690
		private bool BackupSolution(string sourceDir, CocosMonitor monitor)
		{
			string tempDir = this.GetTempDir(sourceDir, "ccs");
			monitor.SendInfo(string.Format("Backup project to {0}", tempDir));
			return Cocos2dxServices.CopyFolder(sourceDir, tempDir, false, monitor);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000024C4 File Offset: 0x000006C4
		private List<string> GetExcludeFiles()
		{
			List<string> list = new List<string>();
			string name = Services.ProjectsService.CurrentSolution.Name;
			list.Add(name + ".ccs");
			list.Add(name + ".cfg");
			list.Add(name + ".udf");
			return list;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000251C File Offset: 0x0000071C
		private List<string> GetExcludeFolders()
		{
			return new List<string>
			{
				"cocosstudio",
				"res",
				"Resources"
			};
		}
	}
}
