using System;
using System.Collections;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Completion;
using Mono.Debugging.Client;
using MonoDevelop.Ide.CodeCompletion;

namespace MonoDevelop.Debugger
{
	internal class DebugCompletionDataList : List<ICompletionData>, ICompletionDataList, IList<ICompletionData>, ICollection<ICompletionData>, IEnumerable<ICompletionData>, IEnumerable
	{
		private static readonly List<ICompletionKeyHandler> keyHandler = new List<ICompletionKeyHandler>();

		public bool IsSorted { get; set; }

		public bool AutoSelect { get; set; }

		public string DefaultCompletionString => string.Empty;

		public bool AutoCompleteUniqueMatch => false;

		public bool AutoCompleteEmptyMatch => false;

		public bool AutoCompleteEmptyMatchOnCurlyBrace => false;

		public bool CloseOnSquareBrackets => false;

		public CompletionSelectionMode CompletionSelectionMode { get; set; }

		public IEnumerable<ICompletionKeyHandler> KeyHandler => keyHandler;

		public event EventHandler CompletionListClosed;

		public DebugCompletionDataList(Mono.Debugging.Client.CompletionData data)
		{
			IsSorted = false;
			foreach (CompletionItem item in data.Items)
			{
				Add(new DebugCompletionData(item));
			}
			AutoSelect = true;
		}

		public void OnCompletionListClosed(EventArgs e)
		{
			CompletionListClosed?.Invoke(this, e);
		}

		void ICompletionDataList.Sort(Comparison<ICompletionData> P_0)
		{
			Sort(P_0);
		}

		void ICompletionDataList.Sort(IComparer<ICompletionData> P_0)
		{
			Sort(P_0);
		}
	}
}
