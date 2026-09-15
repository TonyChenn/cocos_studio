using System;
using System.Collections.Generic;
using CocoStudio.Projects;

namespace Modules.Communal.ResourcePanel.NodeBuildes.ExtendNodeBuildes
{
	[ResourcePanelExtension(typeof(Sprite3DBuild))]
	public class Sprite3DBuild : ResourceFileBuild
	{
		public override Type NodeDataType
		{
			get
			{
				return typeof(Sprite3DFile);
			}
		}

		public override void OnNodeAdded(object dateObject)
		{
			Sprite3DFile sprite3DFile = dateObject as Sprite3DFile;
			if (sprite3DFile != null && sprite3DFile.CompositeFiles != null)
			{
				foreach (ImageFile imageFile in sprite3DFile.CompositeFiles)
				{
					if (!this.dic.ContainsKey(imageFile))
					{
						this.dic.Add(imageFile, sprite3DFile);
					}
					imageFile.Deleted += this.item_Deleted;
				}
			}
			base.OnNodeAdded(dateObject);
		}

		public override void OnNodeRemoved(object dateObject)
		{
			Sprite3DFile sprite3DFile = dateObject as Sprite3DFile;
			if (sprite3DFile != null && sprite3DFile.CompositeFiles != null)
			{
				foreach (ImageFile imageFile in sprite3DFile.CompositeFiles)
				{
					if (!this.dic.ContainsKey(imageFile))
					{
						this.dic.Remove(imageFile);
					}
					imageFile.Deleted -= this.item_Deleted;
				}
			}
			base.OnNodeRemoved(dateObject);
		}

		private void item_Deleted(object sender, EventArgs e)
		{
			ITreeBuild treeBuilder = base.Context.GetTreeBuilder();
			treeBuilder.Update(sender);
			if (this.dic.Count != 0)
			{
				ImageFile key = sender as ImageFile;
				Sprite3DFile objecData = this.dic[key];
				treeBuilder.Update(objecData);
			}
		}

		public override void Dispose()
		{
			this.dic.Clear();
			base.Dispose();
		}

		private Dictionary<ImageFile, Sprite3DFile> dic = new Dictionary<ImageFile, Sprite3DFile>();
	}
}
