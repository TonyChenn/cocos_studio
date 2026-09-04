using System.Collections.Generic;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.AnalysisCore.Fixes
{
	public class GenericFixHandler : IFixHandler
	{
		public IEnumerable<IAnalysisFixAction> GetFixes(Document doc, object fix)
		{
			yield return (GenericFix)fix;
		}
	}
}
