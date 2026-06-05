using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Represents an unresolved type definition.
	/// </summary>
	// Token: 0x020000CA RID: 202
	[Serializable]
	public class DefaultUnresolvedTypeDefinition : AbstractUnresolvedEntity, IUnresolvedTypeDefinition, ITypeReference, IUnresolvedEntity, INamedElement, IHasAccessibility
	{
		// Token: 0x06000754 RID: 1876 RVA: 0x00012BF1 File Offset: 0x00011BF1
		public DefaultUnresolvedTypeDefinition()
		{
			base.SymbolKind = SymbolKind.TypeDefinition;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00012C08 File Offset: 0x00011C08
		public DefaultUnresolvedTypeDefinition(string fullName)
		{
			int num = fullName.LastIndexOf('.');
			string text;
			string name;
			if (num > 0)
			{
				text = fullName.Substring(0, num);
				name = fullName.Substring(num + 1);
			}
			else
			{
				text = "";
				name = fullName;
			}
			base.SymbolKind = SymbolKind.TypeDefinition;
			this.namespaceName = text;
			base.Name = name;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00012C61 File Offset: 0x00011C61
		public DefaultUnresolvedTypeDefinition(string namespaceName, string name)
		{
			base.SymbolKind = SymbolKind.TypeDefinition;
			this.namespaceName = namespaceName;
			base.Name = name;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00012C85 File Offset: 0x00011C85
		public DefaultUnresolvedTypeDefinition(IUnresolvedTypeDefinition declaringTypeDefinition, string name)
		{
			base.SymbolKind = SymbolKind.TypeDefinition;
			base.DeclaringTypeDefinition = declaringTypeDefinition;
			this.namespaceName = declaringTypeDefinition.Namespace;
			base.Name = name;
			base.UnresolvedFile = declaringTypeDefinition.UnresolvedFile;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00012CC4 File Offset: 0x00011CC4
		protected override void FreezeInternal()
		{
			base.FreezeInternal();
			this.baseTypes = FreezableHelper.FreezeList<ITypeReference>(this.baseTypes);
			this.typeParameters = FreezableHelper.FreezeListAndElements<IUnresolvedTypeParameter>(this.typeParameters);
			this.nestedTypes = FreezableHelper.FreezeListAndElements<IUnresolvedTypeDefinition>(this.nestedTypes);
			this.members = FreezableHelper.FreezeListAndElements<IUnresolvedMember>(this.members);
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00012D1C File Offset: 0x00011D1C
		public override object Clone()
		{
			DefaultUnresolvedTypeDefinition defaultUnresolvedTypeDefinition = (DefaultUnresolvedTypeDefinition)base.Clone();
			if (this.baseTypes != null)
			{
				defaultUnresolvedTypeDefinition.baseTypes = new List<ITypeReference>(this.baseTypes);
			}
			if (this.typeParameters != null)
			{
				defaultUnresolvedTypeDefinition.typeParameters = new List<IUnresolvedTypeParameter>(this.typeParameters);
			}
			if (this.nestedTypes != null)
			{
				defaultUnresolvedTypeDefinition.nestedTypes = new List<IUnresolvedTypeDefinition>(this.nestedTypes);
			}
			if (this.members != null)
			{
				defaultUnresolvedTypeDefinition.members = new List<IUnresolvedMember>(this.members);
			}
			return defaultUnresolvedTypeDefinition;
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x00012D9A File Offset: 0x00011D9A
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x00012DA2 File Offset: 0x00011DA2
		public TypeKind Kind
		{
			get
			{
				return this.kind;
			}
			set
			{
				base.ThrowIfFrozen();
				this.kind = value;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x00012DB1 File Offset: 0x00011DB1
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x00012DC0 File Offset: 0x00011DC0
		public bool AddDefaultConstructorIfRequired
		{
			get
			{
				return this.flags[64];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[64] = value;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x00012DD8 File Offset: 0x00011DD8
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x00012E20 File Offset: 0x00011E20
		public bool? HasExtensionMethods
		{
			get
			{
				if (this.flags[128])
				{
					return new bool?(true);
				}
				if (this.flags[256])
				{
					return new bool?(false);
				}
				return null;
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[128] = (value == true);
				this.flags[256] = (value == false);
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x00012E7D File Offset: 0x00011E7D
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x00012E8F File Offset: 0x00011E8F
		public bool IsPartial
		{
			get
			{
				return this.flags[512];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[512] = value;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x00012EA8 File Offset: 0x00011EA8
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x00012EB0 File Offset: 0x00011EB0
		public override string Namespace
		{
			get
			{
				return this.namespaceName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.ThrowIfFrozen();
				this.namespaceName = value;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x00012ED0 File Offset: 0x00011ED0
		public override string ReflectionName
		{
			get
			{
				return this.FullTypeName.ReflectionName;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x00012EEC File Offset: 0x00011EEC
		public FullTypeName FullTypeName
		{
			get
			{
				IUnresolvedTypeDefinition declaringTypeDefinition = base.DeclaringTypeDefinition;
				if (declaringTypeDefinition != null)
				{
					return declaringTypeDefinition.FullTypeName.NestedType(base.Name, this.TypeParameters.Count - declaringTypeDefinition.TypeParameters.Count);
				}
				return new TopLevelTypeName(this.namespaceName, base.Name, this.TypeParameters.Count);
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x00012F50 File Offset: 0x00011F50
		public IList<ITypeReference> BaseTypes
		{
			get
			{
				if (this.baseTypes == null)
				{
					this.baseTypes = new List<ITypeReference>();
				}
				return this.baseTypes;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x00012F6B File Offset: 0x00011F6B
		public IList<IUnresolvedTypeParameter> TypeParameters
		{
			get
			{
				if (this.typeParameters == null)
				{
					this.typeParameters = new List<IUnresolvedTypeParameter>();
				}
				return this.typeParameters;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x00012F86 File Offset: 0x00011F86
		public IList<IUnresolvedTypeDefinition> NestedTypes
		{
			get
			{
				if (this.nestedTypes == null)
				{
					this.nestedTypes = new List<IUnresolvedTypeDefinition>();
				}
				return this.nestedTypes;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x00012FA1 File Offset: 0x00011FA1
		public IList<IUnresolvedMember> Members
		{
			get
			{
				if (this.members == null)
				{
					this.members = new List<IUnresolvedMember>();
				}
				return this.members;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x00012FBC File Offset: 0x00011FBC
		public IEnumerable<IUnresolvedMethod> Methods
		{
			get
			{
				return this.Members.OfType<IUnresolvedMethod>();
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x00012FC9 File Offset: 0x00011FC9
		public IEnumerable<IUnresolvedProperty> Properties
		{
			get
			{
				return this.Members.OfType<IUnresolvedProperty>();
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x00012FD6 File Offset: 0x00011FD6
		public IEnumerable<IUnresolvedField> Fields
		{
			get
			{
				return this.Members.OfType<IUnresolvedField>();
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x00012FE3 File Offset: 0x00011FE3
		public IEnumerable<IUnresolvedEvent> Events
		{
			get
			{
				return this.Members.OfType<IUnresolvedEvent>();
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00012FF0 File Offset: 0x00011FF0
		public IType Resolve(ITypeResolveContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.CurrentAssembly == null)
			{
				throw new ArgumentException("An ITypeDefinition cannot be resolved in a context without a current assembly.");
			}
			IType type = context.CurrentAssembly.GetTypeDefinition(this.FullTypeName);
			if (type != null)
			{
				return type;
			}
			return new UnknownType(this.Namespace, base.Name, this.TypeParameters.Count);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0001304F File Offset: 0x0001204F
		public virtual ITypeResolveContext CreateResolveContext(ITypeResolveContext parentContext)
		{
			return parentContext;
		}

		// Token: 0x0400021E RID: 542
		private TypeKind kind = TypeKind.Class;

		// Token: 0x0400021F RID: 543
		private string namespaceName;

		// Token: 0x04000220 RID: 544
		private IList<ITypeReference> baseTypes;

		// Token: 0x04000221 RID: 545
		private IList<IUnresolvedTypeParameter> typeParameters;

		// Token: 0x04000222 RID: 546
		private IList<IUnresolvedTypeDefinition> nestedTypes;

		// Token: 0x04000223 RID: 547
		private IList<IUnresolvedMember> members;
	}
}
