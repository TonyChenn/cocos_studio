using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001AE RID: 430
	public abstract class MSBuildFileFormat : IFileFormat
	{
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x0600101C RID: 4124 RVA: 0x0003C370 File Offset: 0x0003A570
		public string Name
		{
			get
			{
				return "MSBuild";
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x0600101D RID: 4125 RVA: 0x0003C377 File Offset: 0x0003A577
		public SlnFileFormat SlnFileFormat
		{
			get
			{
				return this.slnFileFormat;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x0600101E RID: 4126 RVA: 0x0003C37F File Offset: 0x0003A57F
		public bool SupportsMonikers
		{
			get
			{
				return this.SupportedFrameworks == null;
			}
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0003C38A File Offset: 0x0003A58A
		public bool SupportsFramework(TargetFramework fx)
		{
			return this.SupportsMonikers || ((IList<TargetFrameworkMoniker>)this.SupportedFrameworks).Contains(fx.Id);
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0003C3AC File Offset: 0x0003A5AC
		internal virtual bool SupportsSlnVersion(string version)
		{
			return version == this.SlnVersion;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0003C3BA File Offset: 0x0003A5BA
		protected virtual bool SupportsToolsVersion(string version)
		{
			return version == this.DefaultToolsVersion;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0003C3C8 File Offset: 0x0003A5C8
		public FilePath GetValidFormatName(object obj, FilePath fileName)
		{
			if (this.slnFileFormat.CanWriteFile(obj, this))
			{
				return this.slnFileFormat.GetValidFormatName(obj, fileName, this);
			}
			string extensionForItem = MSBuildProjectService.GetExtensionForItem((SolutionEntityItem)obj);
			if (!string.IsNullOrEmpty(extensionForItem))
			{
				return fileName.ChangeExtension("." + extensionForItem);
			}
			return fileName;
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0003C428 File Offset: 0x0003A628
		public bool CanReadFile(FilePath file, Type expectedType)
		{
			return (expectedType.IsAssignableFrom(typeof(Solution)) && this.slnFileFormat.CanReadFile(file, this)) || (expectedType.IsAssignableFrom(typeof(SolutionEntityItem)) && MSBuildProjectService.CanReadFile(file) && this.SupportsToolsVersion(MSBuildFileFormat.ReadToolsVersion(file)));
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0003C488 File Offset: 0x0003A688
		public bool CanWriteFile(object obj)
		{
			if (this.slnFileFormat.CanWriteFile(obj, this))
			{
				Solution solution = (Solution)obj;
				foreach (SolutionEntityItem obj2 in solution.GetAllSolutionItems<SolutionEntityItem>())
				{
					if (!this.CanWriteFile(obj2))
					{
						return false;
					}
				}
				return true;
			}
			if (obj is SolutionEntityItem)
			{
				DotNetProject dotNetProject = obj as DotNetProject;
				return dotNetProject == null || dotNetProject.Loading || this.SupportsFramework(dotNetProject.TargetFramework);
			}
			return false;
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0003C528 File Offset: 0x0003A728
		public virtual IEnumerable<string> GetCompatibilityWarnings(object obj)
		{
			if (obj is Solution)
			{
				List<string> list = new List<string>();
				foreach (SolutionEntityItem obj2 in ((Solution)obj).GetAllSolutionItems<SolutionEntityItem>())
				{
					IEnumerable<string> compatibilityWarnings = this.GetCompatibilityWarnings(obj2);
					if (compatibilityWarnings != null)
					{
						list.AddRange(compatibilityWarnings);
					}
				}
				return list;
			}
			DotNetProject dotNetProject = obj as DotNetProject;
			if (dotNetProject != null && !this.SupportsMonikers && !((IList)this.SupportedFrameworks).Contains(dotNetProject.TargetFramework.Id))
			{
				return new string[]
				{
					GettextCatalog.GetString("The project '{0}' is being saved using the file format '{1}', but this version of Visual Studio does not support the framework that the project is targetting ({2})", dotNetProject.Name, this.ProductDescription, dotNetProject.TargetFramework.Name)
				};
			}
			return null;
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0003C5F8 File Offset: 0x0003A7F8
		public void WriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
			if (this.slnFileFormat.CanWriteFile(obj, this))
			{
				this.slnFileFormat.WriteFile(file, obj, this, true, monitor);
				return;
			}
			SolutionEntityItem solutionEntityItem = (SolutionEntityItem)obj;
			if (!(solutionEntityItem.ItemHandler is MSBuildProjectHandler))
			{
				MSBuildProjectService.InitializeItemHandler(solutionEntityItem);
			}
			MSBuildProjectHandler msbuildProjectHandler = (MSBuildProjectHandler)solutionEntityItem.ItemHandler;
			msbuildProjectHandler.SetSolutionFormat(this, false);
			msbuildProjectHandler.Save(monitor);
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0003C65F File Offset: 0x0003A85F
		public object ReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			if (this.slnFileFormat.CanReadFile(file, this))
			{
				return this.slnFileFormat.ReadFile(file, this, monitor);
			}
			return MSBuildProjectService.LoadItem(monitor, file, null, null, null);
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0003C698 File Offset: 0x0003A898
		public List<FilePath> GetItemFiles(object obj)
		{
			return new List<FilePath>();
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0003C6A0 File Offset: 0x0003A8A0
		public void ConvertToFormat(object obj)
		{
			if (obj == null)
			{
				return;
			}
			SolutionItem solutionItem = obj as SolutionItem;
			MSBuildHandler msbuildHandler;
			if (solutionItem != null)
			{
				msbuildHandler = (solutionItem.GetItemHandler() as MSBuildHandler);
				if (msbuildHandler != null)
				{
					msbuildHandler.SetSolutionFormat(this, true);
					return;
				}
			}
			MSBuildProjectService.InitializeItemHandler(solutionItem);
			msbuildHandler = (MSBuildHandler)solutionItem.ItemHandler;
			msbuildHandler.SetSolutionFormat(this, true);
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600102A RID: 4138 RVA: 0x0003C6ED File Offset: 0x0003A8ED
		public bool SupportsMixedFormats
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600102B RID: 4139
		public abstract string DefaultToolsVersion { get; }

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x0600102C RID: 4140
		public abstract string SlnVersion { get; }

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x0003C6F0 File Offset: 0x0003A8F0
		public virtual string DefaultProductVersion
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x0600102E RID: 4142 RVA: 0x0003C6F3 File Offset: 0x0003A8F3
		public virtual string DefaultSchemaVersion
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x0600102F RID: 4143
		public abstract string ProductDescription { get; }

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001030 RID: 4144 RVA: 0x0003C6F6 File Offset: 0x0003A8F6
		public virtual TargetFrameworkMoniker[] SupportedFrameworks
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0003C6FC File Offset: 0x0003A8FC
		private static string ReadToolsVersion(FilePath file)
		{
			try
			{
				using (XmlTextReader xmlTextReader = new XmlTextReader(new StreamReader(file)))
				{
					if (xmlTextReader.MoveToContent() == XmlNodeType.Element)
					{
						if (xmlTextReader.LocalName != "Project" || xmlTextReader.NamespaceURI != "http://schemas.microsoft.com/developer/msbuild/2003")
						{
							return string.Empty;
						}
						string attribute = xmlTextReader.GetAttribute("ToolsVersion");
						if (string.IsNullOrEmpty(attribute))
						{
							return "2.0";
						}
						return attribute;
					}
				}
			}
			catch
			{
			}
			return string.Empty;
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001032 RID: 4146
		public abstract string Id { get; }

		// Token: 0x040004AB RID: 1195
		private readonly SlnFileFormat slnFileFormat = new SlnFileFormat();
	}
}
