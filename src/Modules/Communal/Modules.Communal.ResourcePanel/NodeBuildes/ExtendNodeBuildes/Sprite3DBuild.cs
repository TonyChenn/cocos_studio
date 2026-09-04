using System;
using System.Collections.Generic;
using CocoStudio.Projects;

namespace Modules.Communal.ResourcePanel.NodeBuildes.ExtendNodeBuildes
{
	// Token: 0x02000015 RID: 21
	[ResourcePanelExtension(typeof(Sprite3DBuild))]
	public class Sprite3DBuild : ResourceFileBuild
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00003887 File Offset: 0x00001A87
		public override Type NodeDataType
		{
			get
			{
				return typeof(Sprite3DFile);
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003894 File Offset: 0x00001A94
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

		// Token: 0x0600009A RID: 154 RVA: 0x00003928 File Offset: 0x00001B28
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

		// Token: 0x0600009B RID: 155 RVA: 0x000039BC File Offset: 0x00001BBC
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

		// Token: 0x0600009C RID: 156 RVA: 0x00003A04 File Offset: 0x00001C04
		public override void Dispose()
		{
			this.dic.Clear();
			base.Dispose();
		}

		// Token: 0x04000032 RID: 50
		private Dictionary<ImageFile, Sprite3DFile> dic = new Dictionary<ImageFile, Sprite3DFile>();
	}
}
