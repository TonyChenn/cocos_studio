using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Documentation
{
	/// <summary>
	/// Provides XML documentation for entities.
	/// </summary>
	public interface IUnresolvedDocumentationProvider
	{
		/// <summary>
		/// Gets the XML documentation for the specified entity.
		/// </summary>
		string GetDocumentation(IUnresolvedEntity entity);

		/// <summary>
		/// Gets the XML documentation for the specified entity.
		/// </summary>
		DocumentationComment GetDocumentation(IUnresolvedEntity entity, IEntity resolvedEntity);
	}
}
