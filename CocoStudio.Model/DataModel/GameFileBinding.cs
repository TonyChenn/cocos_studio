using System;
using System.Collections.Generic;
using CocoStudio.Projects;
using Mono.Addins;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000019 RID: 25
	[Extension(typeof(ICocosItemBinding))]
	internal class GameFileBinding : CocosItemBinding
	{
		// Token: 0x0600010F RID: 271 RVA: 0x00003D1C File Offset: 0x00001F1C
		public GameFileBinding()
		{
			string[] names = Enum.GetNames(typeof(NodeType));
			this.typeSet = new HashSet<string>(names);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00003D50 File Offset: 0x00001F50
		protected override CocosItem OnCreateItem(CocosItemCreateInfo info)
		{
			GameFile gameFile = new GameFile(info);
			gameFile.Content = this.CreateFileContent(info);
			return new CocosItem(info.FileName, gameFile);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00003D88 File Offset: 0x00001F88
		protected virtual ICocosFileContent CreateFileContent(CocosItemCreateInfo info)
		{
			NodeType nodeType;
			if (!Enum.TryParse<NodeType>(info.ContentType, out nodeType))
			{
				throw new InvalidOperationException("Unsupport content type" + info.ContentType);
			}
			GameFileContent gameFileContent = new GameFileContent(nodeType);
			if (nodeType == NodeType.Scene || NodeType.Layer == nodeType)
			{
				gameFileContent.Content.ObjectData.Size = new SizeF(info.Width, info.Height);
			}
			return gameFileContent;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00003E08 File Offset: 0x00002008
		protected override bool OnCanCreateItem(string fileType)
		{
			return !(fileType == NodeType.Plist.ToString()) && this.typeSet.Contains(fileType);
		}

		// Token: 0x04000073 RID: 115
		private HashSet<string> typeSet;
	}
}
