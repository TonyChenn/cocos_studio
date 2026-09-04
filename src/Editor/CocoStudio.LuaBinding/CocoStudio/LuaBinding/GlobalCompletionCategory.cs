using ICSharpCode.NRefactory.Completion;

namespace CocoStudio.LuaBinding
{
	public class GlobalCompletionCategory : CompletionCategory
	{
		public GlobalCompletionCategory()
		{
			base.DisplayText = "Global";
		}

		public override int CompareTo(CompletionCategory other)
		{
			if (!(other.DisplayText == base.DisplayText))
			{
				return 0;
			}
			return 1;
		}
	}
}
