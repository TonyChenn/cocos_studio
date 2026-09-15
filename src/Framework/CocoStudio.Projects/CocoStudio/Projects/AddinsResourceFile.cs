using System;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	internal class AddinsResourceFile : ResourceFile
	{
		internal override string PreviewImagePath
		{
			get
			{
				return this.FileName;
			}
		}

		protected AddinsResourceFile() : base(true)
		{
		}

		public AddinsResourceFile(ResourceData resourceData) : this()
		{
			this.resourceData = resourceData;
			if (Path.IsPathRooted(resourceData.Path))
			{
				this.FileName = resourceData.Path;
				return;
			}
			this.FileName = Path.Combine(Option.LuaScriptFolder, resourceData.Path);
		}

		protected override ResourceData CreateDefaultResourceData(FilePath filePath)
		{
			return this.resourceData;
		}

		private ResourceData resourceData;
	}
}
