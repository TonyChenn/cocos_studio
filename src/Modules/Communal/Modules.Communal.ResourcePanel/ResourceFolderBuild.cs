using System;
using System.IO;
using CocoStudio.Projects;
using Gtk;
using MonoDevelop.Core;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	[ResourcePanelExtension(typeof(ResourceFolderBuild))]
	public class ResourceFolderBuild : ResourceItemBuild
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(ResourceFolder);
			}
		}

		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = ResourceFolderBuild.expandIcon,
				UnExpandIcon = ResourceFolderBuild.UnexpandIocn
			};
		}

		public override void BuildChildNodes(ITreeBuild treeBuilder, object dataObject)
		{
			ResourceFolder resourceFolder = dataObject as ResourceFolder;
			treeBuilder.AddChildren(resourceFolder.Items);
		}

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

		protected override bool OnCanDrag(object dataObject)
		{
			ResourceFolder resourceFolder = dataObject as ResourceFolder;
			return resourceFolder != null && Directory.Exists(resourceFolder.FullPath);
		}

		public override ResourceFolder GetTargetFolder(object dataObject)
		{
			return dataObject as ResourceFolder;
		}

		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("folder.png"));

		private static readonly Xwt.Drawing.Image UnexpandIocn = ImageIcon.GetIcon(StaticVariable.GetResourceID("folderOpen.png"));
	}
}
