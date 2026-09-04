using System;
using CocoStudio.Projects;

namespace Modules.Communal.ResourcePanel.NodeBuildes
{
	// Token: 0x02000012 RID: 18
	[ResourcePanelExtension(typeof(ResourceFileBuild))]
	public class CodeFileBuild : ResourceFileBuild
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00003758 File Offset: 0x00001958
		public override Type NodeDataType
		{
			get
			{
				return typeof(CodeFile);
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003764 File Offset: 0x00001964
		public override bool CanRename()
		{
			return false;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003768 File Offset: 0x00001968
		protected override bool OnCanDrag(object dataObject)
		{
			CodeFile codeFile = dataObject as CodeFile;
			return !(codeFile.Parent is CocosItem) && base.OnCanDrag(dataObject);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003794 File Offset: 0x00001994
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
