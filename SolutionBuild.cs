using System;
using System.IO;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000020 RID: 32
	[ResourcePanelExtension(typeof(SolutionBuild))]
	public class SolutionBuild : NodeBuilder
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000040C4 File Offset: 0x000022C4
		public override Type NodeDataType
		{
			get
			{
				return typeof(Solution);
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000040D0 File Offset: 0x000022D0
		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = SolutionBuild.expandIcon
			};
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000040F0 File Offset: 0x000022F0
		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
			Solution solution = dataObject as Solution;
			nodeInfo.DataItem = solution;
			nodeInfo.Name = ((solution.Name == null) ? "Solution is Null" : solution.Name);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004130 File Offset: 0x00002330
		protected override void OnBuildNodeAfter(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			Solution solution = dataObject as Solution;
			if (!Directory.Exists(solution.BaseDirectory) || !File.Exists(solution.FileName))
			{
				nodeInfo.Name = base.GetRedNameString(nodeInfo.Name);
			}
			base.OnBuildNodeAfter(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004184 File Offset: 0x00002384
		public override void BuildChildNodes(ITreeBuild treeBuilder, object dataObject)
		{
			Solution solution = dataObject as Solution;
			if (solution.RootFolder != null && solution.RootFolder.Items.Count != 0)
			{
				ResourceGroup resourceGroup = solution.RootFolder.Items[0] as ResourceGroup;
				treeBuilder.AddChildren(resourceGroup.RootFolder.Items);
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000041DA File Offset: 0x000023DA
		protected override bool OnCanDrag(object dataObject)
		{
			return false;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000041DD File Offset: 0x000023DD
		public override bool CanRename()
		{
			return false;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000041E0 File Offset: 0x000023E0
		internal override bool CanDelete()
		{
			return false;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000041E4 File Offset: 0x000023E4
		public override ResourceFolder GetTargetFolder(object dataObject)
		{
			Solution solution = dataObject as Solution;
			return solution.GetRootFolder();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000041FE File Offset: 0x000023FE
		public override string CanMove(object moveSource, object moveTarget, TreeViewDropPosition pos)
		{
			return LanguageInfo.FileMove_Solution;
		}

		// Token: 0x0400003E RID: 62
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("ccs.png"));
	}
}
