using System;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Refactoring;

namespace MonoDevelop.SourceEditor.QuickTasks
{
	public class QuickTask
	{
		private Lazy<string> description;

		public string Description => description.Value;

		public TextLocation Location { get; private set; }

		public Severity Severity { get; private set; }

		public QuickTask(Func<string> descriptionFunc, TextLocation location, Severity severity)
		{
			description = new Lazy<string>(descriptionFunc);
			Location = location;
			Severity = severity;
		}

		public QuickTask(string description, TextLocation location, Severity severity)
		{
			Func<string> valueFactory = () => description;
			this.description = new Lazy<string>(valueFactory);
			Location = location;
			Severity = severity;
		}

		public override string ToString()
		{
			return $"[QuickTask: Description={Description}, Location={Location}, Severity={Severity}]";
		}
	}
}
