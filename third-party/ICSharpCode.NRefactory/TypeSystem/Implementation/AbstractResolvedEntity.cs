using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Documentation;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IEntity" /> that resolves an unresolved entity.
	/// </summary>
	public abstract class AbstractResolvedEntity : IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		protected AbstractResolvedEntity(IUnresolvedEntity unresolved, ITypeResolveContext parentContext)
		{
			if (unresolved == null)
			{
				throw new ArgumentNullException("unresolved");
			}
			if (parentContext == null)
			{
				throw new ArgumentNullException("parentContext");
			}
			this.unresolved = unresolved;
			this.parentContext = parentContext;
			this.Attributes = unresolved.Attributes.CreateResolvedAttributes(parentContext);
		}

		public SymbolKind SymbolKind
		{
			get
			{
				return this.unresolved.SymbolKind;
			}
		}

		[Obsolete("Use the SymbolKind property instead.")]
		public EntityType EntityType
		{
			get
			{
				return (EntityType)this.unresolved.SymbolKind;
			}
		}

		public DomRegion Region
		{
			get
			{
				return this.unresolved.Region;
			}
		}

		public DomRegion BodyRegion
		{
			get
			{
				return this.unresolved.BodyRegion;
			}
		}

		public ITypeDefinition DeclaringTypeDefinition
		{
			get
			{
				return this.parentContext.CurrentTypeDefinition;
			}
		}

		public virtual IType DeclaringType
		{
			get
			{
				IType type = this.parentContext.CurrentTypeDefinition;
				if (type != null)
				{
					return type;
				}
				return SpecialType.UnknownType;
			}
		}

		public IAssembly ParentAssembly
		{
			get
			{
				return this.parentContext.CurrentAssembly;
			}
		}

		public IList<IAttribute> Attributes { get; protected set; }

		public virtual DocumentationComment Documentation
		{
			get
			{
				IDocumentationProvider documentationProvider = AbstractResolvedEntity.FindDocumentation(this.parentContext);
				if (documentationProvider != null)
				{
					return documentationProvider.GetDocumentation(this);
				}
				return null;
			}
		}

		internal static IDocumentationProvider FindDocumentation(ITypeResolveContext context)
		{
			IAssembly currentAssembly = context.CurrentAssembly;
			if (currentAssembly != null)
			{
				return currentAssembly.UnresolvedAssembly as IDocumentationProvider;
			}
			return null;
		}

		public abstract ISymbolReference ToReference();

		public bool IsStatic
		{
			get
			{
				return this.unresolved.IsStatic;
			}
		}

		public bool IsAbstract
		{
			get
			{
				return this.unresolved.IsAbstract;
			}
		}

		public bool IsSealed
		{
			get
			{
				return this.unresolved.IsSealed;
			}
		}

		public bool IsShadowing
		{
			get
			{
				return this.unresolved.IsShadowing;
			}
		}

		public bool IsSynthetic
		{
			get
			{
				return this.unresolved.IsSynthetic;
			}
		}

		public ICompilation Compilation
		{
			get
			{
				return this.parentContext.Compilation;
			}
		}

		public string FullName
		{
			get
			{
				return this.unresolved.FullName;
			}
		}

		public string Name
		{
			get
			{
				return this.unresolved.Name;
			}
		}

		public string ReflectionName
		{
			get
			{
				return this.unresolved.ReflectionName;
			}
		}

		public string Namespace
		{
			get
			{
				return this.unresolved.Namespace;
			}
		}

		public Accessibility Accessibility
		{
			get
			{
				return this.unresolved.Accessibility;
			}
		}

		public bool IsPrivate
		{
			get
			{
				return this.unresolved.IsPrivate;
			}
		}

		public bool IsPublic
		{
			get
			{
				return this.unresolved.IsPublic;
			}
		}

		public bool IsProtected
		{
			get
			{
				return this.unresolved.IsProtected;
			}
		}

		public bool IsInternal
		{
			get
			{
				return this.unresolved.IsInternal;
			}
		}

		public bool IsProtectedOrInternal
		{
			get
			{
				return this.unresolved.IsProtectedOrInternal;
			}
		}

		public bool IsProtectedAndInternal
		{
			get
			{
				return this.unresolved.IsProtectedAndInternal;
			}
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.SymbolKind.ToString(),
				" ",
				this.ReflectionName,
				"]"
			});
		}

		protected readonly IUnresolvedEntity unresolved;

		protected readonly ITypeResolveContext parentContext;
	}
}
