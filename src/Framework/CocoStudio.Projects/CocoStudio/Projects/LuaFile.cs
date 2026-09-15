using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	[DataItem(Name = "Lua")]
	public class LuaFile : CodeFile
	{
		protected LuaFile()
		{
		}

		public LuaFile(FilePath file) : base(file)
		{
		}

		public const string FileSuffix = ".lua";
	}
}
