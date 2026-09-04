using System;
using System.Collections.Generic;

namespace MonoDevelop.SourceEditor.QuickTasks
{
	public interface IUsageProvider
	{
		IEnumerable<Usage> Usages { get; }

		event EventHandler UsagesUpdated;
	}
}
