using System;
using CocoStudio.Projects;
using MonoDevelop.Core;

namespace Modules.Communal.ResourcePanel
{
	[ResourcePanelExtension(typeof(ResourceFileBuild))]
	public class ResourceFileBuild : ResourceItemBuild
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(ResourceFile);
			}
		}

		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
		}

		public bool IsRedNameState(string text)
		{
			return !string.IsNullOrWhiteSpace(text) && text.Substring(0, 1) == "<";
		}

		public override void AddChild(ITreeBuild treeBuilder, object dateObject)
		{
			base.AddChild(treeBuilder, dateObject);
		}

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

		protected override bool OnCanDrag(object dataObject)
		{
			ResourceFile resourceFile = dataObject as ResourceFile;
			return resourceFile != null && resourceFile.DataError == null;
		}

		public override ResourceFolder GetTargetFolder(object dataObject)
		{
			ResourceFile resourceFile = dataObject as ResourceFile;
			return resourceFile.Parent as ResourceFolder;
		}
	}
}
