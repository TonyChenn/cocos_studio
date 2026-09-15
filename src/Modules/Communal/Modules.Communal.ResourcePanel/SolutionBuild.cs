using System;
using System.IO;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	[ResourcePanelExtension(typeof(SolutionBuild))]
	public class SolutionBuild : NodeBuilder
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(Solution);
			}
		}

		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = SolutionBuild.expandIcon
			};
		}

		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
			Solution solution = dataObject as Solution;
			nodeInfo.DataItem = solution;
			nodeInfo.Name = ((solution.Name == null) ? "Solution is Null" : solution.Name);
		}

		protected override void OnBuildNodeAfter(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			Solution solution = dataObject as Solution;
			if (!Directory.Exists(solution.BaseDirectory) || !File.Exists(solution.FileName))
			{
				nodeInfo.Name = base.GetRedNameString(nodeInfo.Name);
			}
			base.OnBuildNodeAfter(treeBuilder, dataObject, nodeInfo);
		}

		public override void BuildChildNodes(ITreeBuild treeBuilder, object dataObject)
		{
			Solution solution = dataObject as Solution;
			if (solution.RootFolder != null && solution.RootFolder.Items.Count != 0)
			{
				ResourceGroup resourceGroup = solution.RootFolder.Items[0] as ResourceGroup;
				treeBuilder.AddChildren(resourceGroup.RootFolder.Items);
			}
		}

		protected override bool OnCanDrag(object dataObject)
		{
			return false;
		}

		public override bool CanRename()
		{
			return false;
		}

		internal override bool CanDelete()
		{
			return false;
		}

		public override ResourceFolder GetTargetFolder(object dataObject)
		{
			Solution solution = dataObject as Solution;
			return solution.GetRootFolder();
		}

		public override string CanMove(object moveSource, object moveTarget, TreeViewDropPosition pos)
		{
			return LanguageInfo.FileMove_Solution;
		}

		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("ccs.png"));
	}
}
