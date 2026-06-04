using System;
using CocoStudio.Projects;
using MonoDevelop.Core;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000010 RID: 16
	[ResourcePanelExtension(typeof(ResourceFileBuild))]
	public class ResourceFileBuild : ResourceItemBuild
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000035DE File Offset: 0x000017DE
		public override Type NodeDataType
		{
			get
			{
				return typeof(ResourceFile);
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000035EA File Offset: 0x000017EA
		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000035F5 File Offset: 0x000017F5
		public bool IsRedNameState(string text)
		{
			return !string.IsNullOrWhiteSpace(text) && text.Substring(0, 1) == "<";
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003618 File Offset: 0x00001818
		public override void AddChild(ITreeBuild treeBuilder, object dateObject)
		{
			base.AddChild(treeBuilder, dateObject);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003624 File Offset: 0x00001824
		protected override void OnRename(ITreeBuild treeBuilder, object dataObject, string newName)
		{
			ResourceFile resourceFile = dataObject as ResourceFile;
			FilePath fileName = resourceFile.FileName;
			string extension = resourceFile.FileName.Extension;
			newName += extension;
			string text = fileName.ParentDirectory.Combine(new string[]
			{
				newName
			});
			if (text != null && text != fileName.ToString())
			{
				FileService.RenameFile(fileName, text);
				resourceFile.SetLocation(text, true);
			}
			base.OnRename(treeBuilder, dataObject, newName);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000036B8 File Offset: 0x000018B8
		protected override bool OnCanDrag(object dataObject)
		{
			ResourceFile resourceFile = dataObject as ResourceFile;
			return resourceFile != null && resourceFile.DataError == null;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000036DC File Offset: 0x000018DC
		public override ResourceFolder GetTargetFolder(object dataObject)
		{
			ResourceFile resourceFile = dataObject as ResourceFile;
			return resourceFile.Parent as ResourceFolder;
		}
	}
}
