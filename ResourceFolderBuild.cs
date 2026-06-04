using System;
using System.IO;
using CocoStudio.Projects;
using Gtk;
using MonoDevelop.Core;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200001C RID: 28
	[ResourcePanelExtension(typeof(ResourceFolderBuild))]
	public class ResourceFolderBuild : ResourceItemBuild
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00003CAA File Offset: 0x00001EAA
		public override Type NodeDataType
		{
			get
			{
				return typeof(ResourceFolder);
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003CB8 File Offset: 0x00001EB8
		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = ResourceFolderBuild.expandIcon,
				UnExpandIcon = ResourceFolderBuild.UnexpandIocn
			};
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003CE4 File Offset: 0x00001EE4
		public override void BuildChildNodes(ITreeBuild treeBuilder, object dataObject)
		{
			ResourceFolder resourceFolder = dataObject as ResourceFolder;
			treeBuilder.AddChildren(resourceFolder.Items);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003D04 File Offset: 0x00001F04
		protected override void OnRename(ITreeBuild treeBuilder, object dataObject, string newName)
		{
			ResourceFolder resourceFolder = dataObject as ResourceFolder;
			FilePath baseDirectory = resourceFolder.BaseDirectory;
			string text = baseDirectory.ParentDirectory.Combine(new string[]
			{
				newName
			});
			if (text != null && text != baseDirectory.ToString())
			{
				FileService.RenameDirectory(baseDirectory, newName);
				resourceFolder.SetLocation(text, true);
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003D74 File Offset: 0x00001F74
		protected override bool OnCanDrag(object dataObject)
		{
			ResourceFolder resourceFolder = dataObject as ResourceFolder;
			return resourceFolder != null && Directory.Exists(resourceFolder.FullPath);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003D9B File Offset: 0x00001F9B
		public override ResourceFolder GetTargetFolder(object dataObject)
		{
			return dataObject as ResourceFolder;
		}

		// Token: 0x04000039 RID: 57
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("folder.png"));

		// Token: 0x0400003A RID: 58
		private static readonly Xwt.Drawing.Image UnexpandIocn = ImageIcon.GetIcon(StaticVariable.GetResourceID("folderOpen.png"));
	}
}
