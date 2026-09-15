using System;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Documentation
{
	/// <summary>
	/// Represents a documentation comment.
	/// </summary>
	public class DocumentationComment
	{
		/// <summary>
		/// Gets the XML code for this documentation comment.
		/// </summary>
		public ITextSource Xml
		{
			get
			{
				return this.xml;
			}
		}

		/// <summary>
		/// Creates a new DocumentationComment.
		/// </summary>
		/// <param name="xml">The XML text.</param>
		/// <param name="context">Context for resolving cref attributes.</param>
		public DocumentationComment(ITextSource xml, ITypeResolveContext context)
		{
			if (xml == null)
			{
				throw new ArgumentNullException("xml");
			}
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			this.xml = xml;
			this.context = context;
		}

		/// <summary>
		/// Creates a new DocumentationComment.
		/// </summary>
		/// <param name="xml">The XML text.</param>
		/// <param name="context">Context for resolving cref attributes.</param>
		public DocumentationComment(string xml, ITypeResolveContext context)
		{
			if (xml == null)
			{
				throw new ArgumentNullException("xml");
			}
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			this.xml = new StringTextSource(xml);
			this.context = context;
		}

		/// <summary>
		/// Resolves the given cref value to an entity.
		/// Returns null if the entity is not found, or if the cref attribute is syntactically invalid.
		/// </summary>
		public virtual IEntity ResolveCref(string cref)
		{
			IEntity result;
			try
			{
				result = IdStringProvider.FindEntity(cref, this.context);
			}
			catch (ReflectionNameParseException)
			{
				result = null;
			}
			return result;
		}

		public override string ToString()
		{
			return this.Xml.Text;
		}

		public static implicit operator string(DocumentationComment documentationComment)
		{
			if (documentationComment != null)
			{
				return documentationComment.ToString();
			}
			return null;
		}

		private ITextSource xml;

		protected readonly ITypeResolveContext context;
	}
}
