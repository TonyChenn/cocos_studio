using MonoDevelop.Core;
using MonoDevelop.Projects;

namespace CocoStudio.LuaBinding
{
	internal class LuaLanguageBinding : ILanguageBinding
	{
		public string Language => "Lua";

		public string ProjectStockIcon => "md-project";

		public string SingleLineCommentTag => "--";

		public string BlockCommentStartTag => "--[[";

		public string BlockCommentEndTag => "]]";

		public bool IsSourceCodeFile(FilePath file_name)
		{
			return file_name.ToString().ToLower().EndsWith(".lua");
		}

		public FilePath GetFileName(FilePath base_name)
		{
			return string.Concat(base_name, ".lua");
		}
	}
}
