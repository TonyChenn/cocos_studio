using System;
using System.Collections;
using System.Collections.Specialized;
using CocoStudio.Projects;
using Gtk;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200001D RID: 29
	[ResourcePanelExtension(typeof(PlistImageFloderBuild))]
	public class PlistImageFloderBuild : ResourceFolderBuild
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00003DD5 File Offset: 0x00001FD5
		public override Type NodeDataType
		{
			get
			{
				return typeof(PlistImageFolder);
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003DE4 File Offset: 0x00001FE4
		protected override IconInfo GetIcon(object dataObject)
		{
			return new IconInfo
			{
				ExpandIcon = PlistImageFloderBuild.expandIcon
			};
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003E04 File Offset: 0x00002004
		protected override void OnBuildNode(ITreeBuild treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			base.OnBuildNode(treeBuilder, dataObject, nodeInfo);
			PlistImageFolder plistImageFolder = dataObject as PlistImageFolder;
			nodeInfo.Name = plistImageFolder.Name;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003E2D File Offset: 0x0000202D
		protected override bool OnCanDrag(object dataObject)
		{
			return true;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00003E30 File Offset: 0x00002030
		public override bool CanRename()
		{
			return false;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003E34 File Offset: 0x00002034
		public override ResourceFolder GetTargetFolder(object dataObject)
		{
			PlistImageFolder plistImageFolder = dataObject as PlistImageFolder;
			return plistImageFolder.Parent as ResourceFolder;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00003E54 File Offset: 0x00002054
		public override void OnNodeAdded(object dateObject)
		{
			ResourceFolder resourceFolder = dateObject as ResourceFolder;
			if (resourceFolder != null)
			{
				resourceFolder.Items.CollectionChanged += this.Items_CollectionChanged;
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003E84 File Offset: 0x00002084
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

		// Token: 0x060000D8 RID: 216 RVA: 0x00003F64 File Offset: 0x00002164
		public override void OnNodeRemoved(object dateObject)
		{
			ResourceFolder resourceFolder = dateObject as ResourceFolder;
			if (resourceFolder != null)
			{
				resourceFolder.Items.CollectionChanged -= this.Items_CollectionChanged;
			}
		}

		// Token: 0x0400003B RID: 59
		private static readonly Xwt.Drawing.Image expandIcon = ImageIcon.GetIcon(StaticVariable.GetResourceID("plistFolder.png"));
	}
}
