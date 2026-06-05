using System;
using ICSharpCode.NRefactory.Editor;
using ICSharpCode.NRefactory.TypeSystem;

namespace ICSharpCode.NRefactory.Documentation
{
	/// <summary>
	/// Represents a documentation comment.
	/// </summary>
	// Token: 0x02000007 RID: 7
	public class DocumentationComment
	{
		/// <summary>
		/// Gets the XML code for this documentation comment.
		/// </summary>
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002A6D File Offset: 0x00001A6D
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
		// Token: 0x0600001B RID: 27 RVA: 0x00002A75 File Offset: 0x00001A75
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
		// Token: 0x0600001C RID: 28 RVA: 0x00002AA7 File Offset: 0x00001AA7
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
		// Token: 0x0600001D RID: 29 RVA: 0x00002AE0 File Offset: 0x00001AE0
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

		// Token: 0x0600001E RID: 30 RVA: 0x00002B14 File Offset: 0x00001B14
		public override string ToString()
		{
			return this.Xml.Text;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002B21 File Offset: 0x00001B21
		public static implicit operator string(DocumentationComment documentationComment)
		{
			if (documentationComment != null)
			{
				return documentationComment.ToString();
			}
			return null;
		}

		// Token: 0x0400000E RID: 14
		private ITextSource xml;

		// Token: 0x0400000F RID: 15
		protected readonly ITypeResolveContext context;
	}
}
