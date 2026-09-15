using System;
using System.Collections;
using System.Collections.Specialized;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	[ResourcePanelExtension(typeof(PlistImageFloderBuild))]
	public class PlistImageFloderBuild : ResourceFolderBuild
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(PlistImageFolder);
			}
		}

		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = PlistImageFloderBuild.expandIcon
			};
		}

		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
			PlistImageFolder plistImageFolder = dataObject as PlistImageFolder;
			nodeInfo.Name = plistImageFolder.Name;
		}

		protected override bool OnCanDrag(object dataObject)
		{
			return true;
		}

		public override bool CanRename()
		{
			return false;
		}

		public override ResourceFolder GetTargetFolder(object dataObject)
		{
			PlistImageFolder plistImageFolder = dataObject as PlistImageFolder;
			return plistImageFolder.Parent as ResourceFolder;
		}

		public override void OnNodeAdded(object dateObject)
		{
			ResourceFolder resourceFolder = dateObject as ResourceFolder;
			if (resourceFolder != null)
			{
				resourceFolder.Items.CollectionChanged += this.Items_CollectionChanged;
			}
		}

		private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			ITreeBuild treeBuilder = base.Context.GetTreeBuilder();
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (treeBuilder == null)
				{
					return;
				}
				IEnumerator enumerator = e.NewItems.GetEnumerator();
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					ResourceItem parent = ((ResourceItem)obj).Parent;
					treeBuilder.AddChild(parent, obj, false);
				}
				return;
				break;
			case NotifyCollectionChangedAction.Remove:
				break;
			default:
				return;
			}
			if (treeBuilder != null)
			{
				foreach (object dataObject in e.OldItems)
				{
					treeBuilder.Remove(dataObject);
				}
			}
		}

		public override void OnNodeRemoved(object dateObject)
		{
			ResourceFolder resourceFolder = dateObject as ResourceFolder;
			if (resourceFolder != null)
			{
				resourceFolder.Items.CollectionChanged -= this.Items_CollectionChanged;
			}
		}

		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("plistFolder.png"));
	}
}
