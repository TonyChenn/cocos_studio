using System;

namespace ICSharpCode.NRefactory.Completion
{
	public abstract class CompletionCategory : IComparable<CompletionCategory>
	{
		public string DisplayText { get; set; }

		public string Icon { get; set; }

		protected CompletionCategory()
		{
		}

		protected CompletionCategory(string displayText, string icon)
		{
			this.DisplayText = displayText;
			this.Icon = icon;
		}

		public abstract int CompareTo(CompletionCategory other);
	}
}
