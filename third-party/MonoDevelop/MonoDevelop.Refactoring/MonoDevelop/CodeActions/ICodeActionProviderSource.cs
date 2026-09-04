using System.Collections.Generic;

namespace MonoDevelop.CodeActions
{
	public interface ICodeActionProviderSource
	{
		IEnumerable<CodeActionProvider> GetProviders();
	}
}
