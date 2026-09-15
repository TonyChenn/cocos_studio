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
	[ResourcePanelExtension(typeof(ResourceItemBuild))]
	public class ResourceItemBuild : NodeBuilder
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(ResourceItem);
			}
		}

		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = ResourceItemBuild.expandIcon
			};
		}

		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
			ResourceItem resourceItem = dataObject as ResourceItem;
			nodeInfo.Name = resourceItem.Name;
		}

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

		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("file.png"));
	}
}
