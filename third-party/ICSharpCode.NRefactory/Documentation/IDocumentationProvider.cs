using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Documentation
{
	/// <summary>
	/// Provides XML documentation for entities.
	/// </summary>
	// Token: 0x0200000A RID: 10
	public interface IDocumentationProvider
	{
		/// <summary>
		/// Gets the XML documentation for the specified entity.
		/// </summary>
		// Token: 0x06000023 RID: 35
		DocumentationComment GetDocumentation(IEntity entity);
	}
}
