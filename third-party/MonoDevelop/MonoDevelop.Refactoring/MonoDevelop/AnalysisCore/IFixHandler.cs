using System.Collections.Generic;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.AnalysisCore
{
	public interface IFixHandler
	{
		IEnumerable<IAnalysisFixAction> GetFixes(Document doc, object fix);
	}
}
