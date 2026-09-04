using Mono.Debugging.Client;
using MonoDevelop.Core;
using MonoDevelop.Ide.CodeCompletion;

namespace MonoDevelop.Debugger
{
	internal class DebugCompletionData : MonoDevelop.Ide.CodeCompletion.CompletionData
	{
		private readonly CompletionItem item;

		public override IconId Icon => ObjectValueTreeView.GetIcon(item.Flags);

		public override string DisplayText => item.Name;

		public override string CompletionText => item.Name;

		public DebugCompletionData(CompletionItem item)
		{
			this.item = item;
		}
	}
}
