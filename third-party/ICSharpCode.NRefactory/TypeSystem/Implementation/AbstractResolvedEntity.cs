using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.Documentation;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IEntity" /> that resolves an unresolved entity.
	/// </summary>
	// Token: 0x0200005D RID: 93
	public abstract class AbstractResolvedEntity : IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x000075B0 File Offset: 0x000065B0
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

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x000075FF File Offset: 0x000065FF
		public SymbolKind SymbolKind
		{
			get
			{
				return this.unresolved.SymbolKind;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000760C File Offset: 0x0000660C
		[Obsolete("Use the SymbolKind property instead.")]
		public EntityType EntityType
		{
			get
			{
				return (EntityType)this.unresolved.SymbolKind;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00007619 File Offset: 0x00006619
		public DomRegion Region
		{
			get
			{
				return this.unresolved.Region;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060002CB RID: 715 RVA: 0x00007626 File Offset: 0x00006626
		public DomRegion BodyRegion
		{
			get
			{
				return this.unresolved.BodyRegion;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00007633 File Offset: 0x00006633
		public ITypeDefinition DeclaringTypeDefinition
		{
			get
			{
				return this.parentContext.CurrentTypeDefinition;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060002CD RID: 717 RVA: 0x00007640 File Offset: 0x00006640
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

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060002CE RID: 718 RVA: 0x00007656 File Offset: 0x00006656
		public IAssembly ParentAssembly
		{
			get
			{
				return this.parentContext.CurrentAssembly;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060002CF RID: 719 RVA: 0x00007663 File Offset: 0x00006663
		// (set) Token: 0x060002D0 RID: 720 RVA: 0x0000766B File Offset: 0x0000666B
		public IList<IAttribute> Attributes { get; protected set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x00007674 File Offset: 0x00006674
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

		// Token: 0x060002D2 RID: 722 RVA: 0x0000769C File Offset: 0x0000669C
		internal static IDocumentationProvider FindDocumentation(ITypeResolveContext context)
		{
			IAssembly currentAssembly = context.CurrentAssembly;
			if (currentAssembly != null)
			{
				return currentAssembly.UnresolvedAssembly as IDocumentationProvider;
			}
			return null;
		}

		// Token: 0x060002D3 RID: 723
		public abstract ISymbolReference ToReference();

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x000076C0 File Offset: 0x000066C0
		public bool IsStatic
		{
			get
			{
				return this.unresolved.IsStatic;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x000076CD File Offset: 0x000066CD
		public bool IsAbstract
		{
			get
			{
				return this.unresolved.IsAbstract;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x000076DA File Offset: 0x000066DA
		public bool IsSealed
		{
			get
			{
				return this.unresolved.IsSealed;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x000076E7 File Offset: 0x000066E7
		public bool IsShadowing
		{
			get
			{
				return this.unresolved.IsShadowing;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x000076F4 File Offset: 0x000066F4
		public bool IsSynthetic
		{
			get
			{
				return this.unresolved.IsSynthetic;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060002D9 RID: 729 RVA: 0x00007701 File Offset: 0x00006701
		public ICompilation Compilation
		{
			get
			{
				return this.parentContext.Compilation;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000770E File Offset: 0x0000670E
		public string FullName
		{
			get
			{
				return this.unresolved.FullName;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000771B File Offset: 0x0000671B
		public string Name
		{
			get
			{
				return this.unresolved.Name;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00007728 File Offset: 0x00006728
		public string ReflectionName
		{
			get
			{
				return this.unresolved.ReflectionName;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00007735 File Offset: 0x00006735
		public string Namespace
		{
			get
			{
				return this.unresolved.Namespace;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00007742 File Offset: 0x00006742
		public Accessibility Accessibility
		{
			get
			{
				return this.unresolved.Accessibility;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000774F File Offset: 0x0000674F
		public bool IsPrivate
		{
			get
			{
				return this.unresolved.IsPrivate;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000775C File Offset: 0x0000675C
		public bool IsPublic
		{
			get
			{
				return this.unresolved.IsPublic;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00007769 File Offset: 0x00006769
		public bool IsProtected
		{
			get
			{
				return this.unresolved.IsProtected;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00007776 File Offset: 0x00006776
		public bool IsInternal
		{
			get
			{
				return this.unresolved.IsInternal;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00007783 File Offset: 0x00006783
		public bool IsProtectedOrInternal
		{
			get
			{
				return this.unresolved.IsProtectedOrInternal;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x00007790 File Offset: 0x00006790
		public bool IsProtectedAndInternal
		{
			get
			{
				return this.unresolved.IsProtectedAndInternal;
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000077A0 File Offset: 0x000067A0
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

		// Token: 0x040000C0 RID: 192
		protected readonly IUnresolvedEntity unresolved;

		// Token: 0x040000C1 RID: 193
		protected readonly ITypeResolveContext parentContext;
	}
}
