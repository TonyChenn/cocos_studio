using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Represents an unresolved type definition.
	/// </summary>
	[Serializable]
	public class DefaultUnresolvedTypeDefinition : AbstractUnresolvedEntity, IUnresolvedTypeDefinition, ITypeReference, IUnresolvedEntity, INamedElement, IHasAccessibility
	{
		public DefaultUnresolvedTypeDefinition()
		{
			base.SymbolKind = SymbolKind.TypeDefinition;
		}

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

		public DefaultUnresolvedTypeDefinition(string namespaceName, string name)
		{
			base.SymbolKind = SymbolKind.TypeDefinition;
			this.namespaceName = namespaceName;
			base.Name = name;
		}

		public DefaultUnresolvedTypeDefinition(IUnresolvedTypeDefinition declaringTypeDefinition, string name)
		{
			base.SymbolKind = SymbolKind.TypeDefinition;
			base.DeclaringTypeDefinition = declaringTypeDefinition;
			this.namespaceName = declaringTypeDefinition.Namespace;
			base.Name = name;
			base.UnresolvedFile = declaringTypeDefinition.UnresolvedFile;
		}

		protected override void FreezeInternal()
		{
			base.FreezeInternal();
			this.baseTypes = FreezableHelper.FreezeList<ITypeReference>(this.baseTypes);
			this.typeParameters = FreezableHelper.FreezeListAndElements<IUnresolvedTypeParameter>(this.typeParameters);
			this.nestedTypes = FreezableHelper.FreezeListAndElements<IUnresolvedTypeDefinition>(this.nestedTypes);
			this.members = FreezableHelper.FreezeListAndElements<IUnresolvedMember>(this.members);
		}

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

		public override string ReflectionName
		{
			get
			{
				return this.FullTypeName.ReflectionName;
			}
		}

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

		public IEnumerable<IUnresolvedMethod> Methods
		{
			get
			{
				return this.Members.OfType<IUnresolvedMethod>();
			}
		}

		public IEnumerable<IUnresolvedProperty> Properties
		{
			get
			{
				return this.Members.OfType<IUnresolvedProperty>();
			}
		}

		public IEnumerable<IUnresolvedField> Fields
		{
			get
			{
				return this.Members.OfType<IUnresolvedField>();
			}
		}

		public IEnumerable<IUnresolvedEvent> Events
		{
			get
			{
				return this.Members.OfType<IUnresolvedEvent>();
			}
		}

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

		public virtual ITypeResolveContext CreateResolveContext(ITypeResolveContext parentContext)
		{
			return parentContext;
		}

		private TypeKind kind = TypeKind.Class;

		private string namespaceName;

		private IList<ITypeReference> baseTypes;

		private IList<IUnresolvedTypeParameter> typeParameters;

		private IList<IUnresolvedTypeDefinition> nestedTypes;

		private IList<IUnresolvedMember> members;
	}
}
