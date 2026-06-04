using System;
using System.IO;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Components;
using MonoDevelop.Core;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200000F RID: 15
	[ResourcePanelExtension(typeof(ResourceItemBuild))]
	public class ResourceItemBuild : NodeBuilder
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600006F RID: 111 RVA: 0x0000326B File Offset: 0x0000146B
		public override Type NodeDataType
		{
			get
			{
				return typeof(ResourceItem);
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003278 File Offset: 0x00001478
		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = ResourceItemBuild.expandIcon
			};
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003298 File Offset: 0x00001498
		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
			ResourceItem resourceItem = dataObject as ResourceItem;
			nodeInfo.Name = resourceItem.Name;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000032C4 File Offset: 0x000014C4
		protected override void OnBuildNodeBefore(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			ResourceItem resourceItem = dataObject as ResourceItem;
			nodeInfo.DataItem = resourceItem;
			if (!string.IsNullOrWhiteSpace(nodeInfo.Name))
			{
				resourceItem.Refresh();
			}
			base.OnBuildNodeBefore(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000032FC File Offset: 0x000014FC
		protected override void OnBuildNodeAfter(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			ResourceItem resourceItem = dataObject as ResourceItem;
			DataError dataError = resourceItem.DataError;
			if (dataError != null)
			{
				nodeInfo.Name = base.GetRedNameString(nodeInfo.Name);
				string resourceID = StaticVariable.GetResourceID("project-status-warning-16.png");
				nodeInfo.IconInfo.StatusIconInternal = ImageIcon.GetIcon(resourceID);
				nodeInfo.StatusMessage = dataError.Message;
			}
			else
			{
				nodeInfo.IconInfo.StatusIconInternal = CellRendererImage.NullImage;
				nodeInfo.StatusMessage = string.Empty;
			}
			base.OnBuildNodeAfter(treeBuilder, dataObject, nodeInfo);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000337C File Offset: 0x0000157C
		protected override void OnDelete(ITreeBuild treeBuilder, object dataObject, IProgressMonitor monitor)
		{
			ResourceItem resourceItem = dataObject as ResourceItem;
			if (resourceItem == null)
			{
				return;
			}
			ResourceFolder resourceFolder = resourceItem.Parent as ResourceFolder;
			if (Platform.IsMac)
			{
				string cmd = string.Format("chflags -R nouchg \"{0}\"", resourceItem.FullPath);
				StaticVariable.ExecuteCommand(cmd);
			}
			if (resourceFolder != null)
			{
				resourceItem.Delete(monitor);
				if (monitor.AsyncOperation.Success)
				{
					resourceFolder.Items.Remove(resourceItem);
				}
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000033F0 File Offset: 0x000015F0
		protected override void OnRemove(ITreeBuild treeBuilder, object dataObject, IProgressMonitor monitor)
		{
			ResourceItem resourceItem = dataObject as ResourceItem;
			if (resourceItem == null)
			{
				return;
			}
			ResourceFolder resourceFolder = resourceItem.Parent as ResourceFolder;
			if (resourceFolder != null)
			{
				resourceItem.Remove(monitor);
				if (monitor.AsyncOperation.Success)
				{
					resourceFolder.Items.Remove(resourceItem);
				}
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003444 File Offset: 0x00001644
		public override string CanMove(object moveSource, object moveTarget, TreeViewDropPosition pos)
		{
			if (moveTarget == null)
			{
				return LanguageInfo.FileMove_TargetIsNull;
			}
			if (moveTarget is Solution)
			{
				return LanguageInfo.FileMove_Solution;
			}
			if (moveTarget is PlistImageFolder)
			{
				return LanguageInfo.FileMove_TagerIsPlist;
			}
			if (moveSource == moveTarget)
			{
				return LanguageInfo.FileMove_SourceAndTagetSame;
			}
			ResourceItem resourceItem = moveSource as ResourceItem;
			ResourceItem resourceItem2 = moveTarget as ResourceItem;
			if (resourceItem.DataError != null)
			{
				return resourceItem.DataError.Message;
			}
			if (resourceItem2.DataError != null)
			{
				return resourceItem2.DataError.Message;
			}
			ResourceFolder resourceFolder = moveTarget as ResourceFolder;
			if (resourceFolder == null)
			{
				resourceFolder = (resourceItem2.Parent as ResourceFolder);
				if (resourceFolder is PlistImageFolder)
				{
					return LanguageInfo.FileMove_TagerIsPlist;
				}
			}
			if (resourceItem.Parent == resourceFolder)
			{
				return LanguageInfo.FileMove_SameLeve;
			}
			if (new FilePath(resourceItem2.FullPath).IsChildPathOf(resourceItem.FullPath))
			{
				return LanguageInfo.FileMove_MoveInChild;
			}
			string path = resourceFolder.BaseDirectory.Combine(new string[]
			{
				resourceItem.Name
			});
			if (File.Exists(path) || Directory.Exists(path) || this.IsChild(resourceItem.Name, resourceFolder))
			{
				return LanguageInfo.FileMove_Exists;
			}
			return null;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003564 File Offset: 0x00001764
		private bool IsChild(string fileName, ResourceFolder folder)
		{
			foreach (ResourceItem resourceItem in folder.Items)
			{
				if (resourceItem.Name.Equals(fileName, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400002E RID: 46
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("file.png"));
	}
}
