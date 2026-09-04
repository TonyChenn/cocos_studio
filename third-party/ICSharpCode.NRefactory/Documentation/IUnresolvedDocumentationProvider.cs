using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Documentation
{
	/// <summary>
	/// Provides XML documentation for entities.
	/// </summary>
	// Token: 0x0200000B RID: 11
	public interface IUnresolvedDocumentationProvider
	{
		/// <summary>
		/// Gets the XML documentation for the specified entity.
		/// </summary>
		// Token: 0x06000024 RID: 36
		string GetDocumentation(IUnresolvedEntity entity);

		/// <summary>
		/// Gets the XML documentation for the specified entity.
		/// </summary>
		// Token: 0x06000025 RID: 37
		DocumentationComment GetDocumentation(IUnresolvedEntity entity, IEntity resolvedEntity);
	}
}
