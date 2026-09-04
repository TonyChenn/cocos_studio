using ICSharpCode.NRefactory.Completion;

namespace CocoStudio.LuaBinding
{
	public class LocalCompletionCategory : CompletionCategory
	{
		public LocalCompletionCategory()
		{
			base.DisplayText = "Local";
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
