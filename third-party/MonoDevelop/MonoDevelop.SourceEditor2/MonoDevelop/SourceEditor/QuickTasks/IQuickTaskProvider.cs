using System;
using System.Collections.Generic;

namespace MonoDevelop.SourceEditor.QuickTasks
{
	public interface IQuickTaskProvider
	{
		IEnumerable<QuickTask> QuickTasks { get; }

		event EventHandler TasksUpdated;
	}
}
