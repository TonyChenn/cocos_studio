using System;
using System.Collections.Generic;
using CocoStudio.Projects;
using Mono.Addins;

namespace CocoStudio.Model.DataModel
{
	[Extension(typeof(ICocosItemBinding))]
	internal class GameFileBinding : CocosItemBinding
	{
		public GameFileBinding()
		{
			string[] names = Enum.GetNames(typeof(NodeType));
			this.typeSet = new HashSet<string>(names);
		}

		protected override CocosItem OnCreateItem(CocosItemCreateInfo info)
		{
			GameFile gameFile = new GameFile(info);
			gameFile.Content = this.CreateFileContent(info);
			return new CocosItem(info.FileName, gameFile);
		}

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

		protected override bool OnCanCreateItem(string fileType)
		{
			return !(fileType == NodeType.Plist.ToString()) && this.typeSet.Contains(fileType);
		}

		private HashSet<string> typeSet;
	}
}
