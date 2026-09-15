using System;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	public class GameFile : CocosFile
	{
		protected GameFile()
		{
		}

		public GameFile(FilePath file) : base(file)
		{
			this.Type = "GameProject";
		}

		public GameFile(CocosItemCreateInfo info) : base(info)
		{
			this.Type = info.ContentType;
		}
	}
}
