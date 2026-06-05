using System;
using System.IO;
using System.Linq;
using System.Xml;
using MonoDevelop.Core;
using MonoDevelop.Projects.Formats.MSBuild;

namespace MonoDevelop.Projects.SharedAssetsProjects
{
	// Token: 0x02000252 RID: 594
	internal class SharedAssetsProjectMSBuildHandler : MSBuildProjectHandler
	{
		// Token: 0x060015E7 RID: 5607 RVA: 0x00058B9C File Offset: 0x00056D9C
		public SharedAssetsProjectMSBuildHandler()
		{
			base.UseMSBuildEngineByDefault = true;
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x00058BC0 File Offset: 0x00056DC0
		protected override void LoadProject(IProgressMonitor monitor, MSBuildProject msproject)
		{
			XmlDocument document = msproject.Document;
			this.projitemsFile = null;
			foreach (object obj in document.DocumentElement.ChildNodes)
			{
				XmlElement xmlElement = obj as XmlElement;
				if (xmlElement != null && xmlElement.LocalName == "Import" && xmlElement.GetAttribute("Label") == "Shared")
				{
					this.projitemsFile = xmlElement.GetAttribute("Project");
					break;
				}
			}
			if (this.projitemsFile == null)
			{
				return;
			}
			((SharedAssetsProject)base.EntityItem).LanguageName = "C#";
			this.projitemsFile = Path.Combine(Path.GetDirectoryName(msproject.FileName), this.projitemsFile);
			MSBuildProject msbuildProject = new MSBuildProject();
			msbuildProject.Load(this.projitemsFile);
			MSBuildSerializer msbuildSerializer = this.CreateSerializer();
			msbuildSerializer.SerializationContext.BaseFile = base.EntityItem.FileName;
			msbuildSerializer.SerializationContext.ProgressMonitor = monitor;
			((SharedAssetsProject)base.Item).ProjItemsPath = this.projitemsFile;
			base.Item.SetItemHandler(this);
			MSBuildPropertyGroup msbuildPropertyGroup = msbuildProject.PropertyGroups.FirstOrDefault((MSBuildPropertyGroup g) => g.Label == "Configuration");
			if (msbuildPropertyGroup != null)
			{
				((SharedAssetsProject)base.EntityItem).DefaultNamespace = msbuildPropertyGroup.GetPropertyValue("Import_RootNamespace", false);
			}
			base.LoadProjectItems(msbuildProject, msbuildSerializer, ProjectItemFlags.None);
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x00058D7C File Offset: 0x00056F7C
		protected override MSBuildProject SaveProject(IProgressMonitor monitor)
		{
			MSBuildSerializer msbuildSerializer = this.CreateSerializer();
			msbuildSerializer.SerializationContext.BaseFile = base.EntityItem.FileName;
			msbuildSerializer.SerializationContext.ProgressMonitor = monitor;
			MSBuildProject msbuildProject = new MSBuildProject();
			MSBuildProject msbuildProject2 = new MSBuildProject();
			bool flag = base.EntityItem.FileName == null || !File.Exists(base.EntityItem.FileName);
			if (flag)
			{
				MSBuildPropertySet msbuildPropertySet = msbuildProject2.GetGlobalPropertyGroup();
				if (msbuildPropertySet == null)
				{
					msbuildPropertySet = msbuildProject2.AddNewPropertyGroup(false);
				}
				msbuildPropertySet.SetPropertyValue("ProjectGuid", base.EntityItem.ItemId, false, false);
				MSBuildImport msbuildImport = msbuildProject2.AddNewImport("$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props", null);
				msbuildImport.Condition = "Exists('$(MSBuildExtensionsPath)\\$(MSBuildToolsVersion)\\Microsoft.Common.props')";
				msbuildProject2.AddNewImport("$(MSBuildExtensionsPath32)\\Microsoft\\VisualStudio\\v$(VisualStudioVersion)\\CodeSharing\\Microsoft.CodeSharing.Common.Default.props", null);
				msbuildProject2.AddNewImport("$(MSBuildExtensionsPath32)\\Microsoft\\VisualStudio\\v$(VisualStudioVersion)\\CodeSharing\\Microsoft.CodeSharing.Common.props", null);
				msbuildImport = msbuildProject2.AddNewImport(Path.ChangeExtension(base.EntityItem.FileName.FileName, ".projitems"), null);
				msbuildImport.Label = "Shared";
				msbuildProject2.AddNewImport("$(MSBuildExtensionsPath32)\\Microsoft\\VisualStudio\\v$(VisualStudioVersion)\\CodeSharing\\Microsoft.CodeSharing.CSharp.targets", null);
			}
			else
			{
				msbuildProject2.Load(base.EntityItem.FileName);
			}
			if (base.ToolsVersion != "2.0")
			{
				msbuildProject2.ToolsVersion = base.ToolsVersion;
			}
			else if (string.IsNullOrEmpty(msbuildProject2.ToolsVersion))
			{
				msbuildProject2.ToolsVersion = null;
			}
			else
			{
				msbuildProject2.ToolsVersion = "2.0";
			}
			if (this.projitemsFile == null)
			{
				this.projitemsFile = ((SharedAssetsProject)base.Item).ProjItemsPath;
			}
			if (File.Exists(this.projitemsFile))
			{
				msbuildProject.Load(this.projitemsFile);
			}
			else
			{
				MSBuildPropertyGroup msbuildPropertyGroup = msbuildProject.AddNewPropertyGroup(true);
				msbuildPropertyGroup.SetPropertyValue("MSBuildAllProjects", "$(MSBuildAllProjects);$(MSBuildThisFileFullPath)", false, false);
				msbuildPropertyGroup.SetPropertyValue("HasSharedItems", "true", false, false);
				msbuildPropertyGroup.SetPropertyValue("SharedGUID", base.EntityItem.ItemId, false, false);
			}
			MSBuildPropertyGroup msbuildPropertyGroup2 = msbuildProject.PropertyGroups.FirstOrDefault((MSBuildPropertyGroup g) => g.Label == "Configuration");
			if (msbuildPropertyGroup2 == null)
			{
				msbuildPropertyGroup2 = msbuildProject.AddNewPropertyGroup(true);
				msbuildPropertyGroup2.Label = "Configuration";
			}
			msbuildPropertyGroup2.SetPropertyValue("Import_RootNamespace", ((SharedAssetsProject)base.EntityItem).DefaultNamespace, false, false);
			base.SaveProjectItems(monitor, new MSBuildFileFormatVS12(), msbuildSerializer, msbuildProject, "$(MSBuildThisFileDirectory)");
			msbuildProject.Save(this.projitemsFile);
			return msbuildProject2;
		}

		// Token: 0x04000696 RID: 1686
		private string projitemsFile;
	}
}
