using System;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Documentation
{
	/// <summary>
	/// Provides XML documentation for entities.
	/// </summary>
	public interface IDocumentationProvider
	{
		/// <summary>
		/// Gets the XML documentation for the specified entity.
		/// </summary>
		DocumentationComment GetDocumentation(IEntity entity);
	}
}
