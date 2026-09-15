using System;
using CocoStudio.Projects;

namespace Modules.Communal.ResourcePanel.NodeBuildes
{
	[ResourcePanelExtension(typeof(ResourceFileBuild))]
	public class CodeFileBuild : ResourceFileBuild
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(CodeFile);
			}
		}

		public override bool CanRename()
		{
			return false;
		}

		protected override bool OnCanDrag(object dataObject)
		{
			CodeFile codeFile = dataObject as CodeFile;
			return !(codeFile.Parent is CocosItem) && base.OnCanDrag(dataObject);
		}

		public override ResourceFolder GetTargetFolder(object dataObject)
		{
			CodeFile codeFile = dataObject as CodeFile;
			if (codeFile.Parent is CocosItem)
			{
				return codeFile.Parent.Parent as ResourceFolder;
			}
			return base.GetTargetFolder(dataObject);
		}
	}
}
