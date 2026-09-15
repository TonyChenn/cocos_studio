using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Represents a SpecializedMember (a member on which type substitution has been performed).
	/// </summary>
	public abstract class SpecializedMember : IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		protected SpecializedMember(IMember memberDefinition)
		{
			if (memberDefinition == null)
			{
				throw new ArgumentNullException("memberDefinition");
			}
			if (memberDefinition is SpecializedMember)
			{
				throw new ArgumentException("Member definition cannot be specialized. Please use IMember.Specialize() instead of directly constructing SpecializedMember instances.");
			}
			this.baseMember = memberDefinition;
			this.substitution = TypeParameterSubstitution.Identity;
		}

		/// <summary>
		/// Performs a substitution. This method may only be called by constructors in derived classes.
		/// </summary>
		protected void AddSubstitution(TypeParameterSubstitution newSubstitution)
		{
			this.substitution = TypeParameterSubstitution.Compose(newSubstitution, this.substitution);
		}

		[Obsolete("Use IMember.Specialize() instead")]
		public static IMember Create(IMember memberDefinition, TypeParameterSubstitution substitution)
		{
			if (memberDefinition == null)
			{
				return null;
			}
			return memberDefinition.Specialize(substitution);
		}

		public virtual IMemberReference ToMemberReference()
		{
			return this.ToReference();
		}

		public virtual IMemberReference ToReference()
		{
			return new SpecializingMemberReference(this.baseMember.ToReference(), SpecializedMember.ToTypeReference(this.substitution.ClassTypeArguments), null);
		}

		ISymbolReference ISymbol.ToReference()
		{
			return this.ToReference();
		}

		internal static IList<ITypeReference> ToTypeReference(IList<IType> typeArguments)
		{
			if (typeArguments == null)
			{
				return null;
			}
			return (from t in typeArguments
			select t.ToTypeReference()).ToArray<ITypeReference>();
		}

		internal IMethod WrapAccessor(ref IMethod cachingField, IMethod accessorDefinition)
		{
			if (accessorDefinition == null)
			{
				return null;
			}
			IMethod method = LazyInit.VolatileRead<IMethod>(ref cachingField);
			if (method != null)
			{
				return method;
			}
			IMethod newValue = accessorDefinition.Specialize(this.substitution);
			return LazyInit.GetOrSet<IMethod>(ref cachingField, newValue);
		}

		/// <summary>
		/// Gets the substitution belonging to this specialized member.
		/// </summary>
		public TypeParameterSubstitution Substitution
		{
			get
			{
				return this.substitution;
			}
		}

		public IType DeclaringType
		{
			get
			{
				IType type = LazyInit.VolatileRead<IType>(ref this.declaringType);
				if (type != null)
				{
					return type;
				}
				IType type2 = this.baseMember.DeclaringType;
				ITypeDefinition typeDefinition = type2 as ITypeDefinition;
				if (typeDefinition != null && type2.TypeParameterCount > 0)
				{
					if (this.substitution.ClassTypeArguments != null && this.substitution.ClassTypeArguments.Count == type2.TypeParameterCount)
					{
						type = new ParameterizedType(typeDefinition, this.substitution.ClassTypeArguments);
					}
					else
					{
						type = new ParameterizedType(typeDefinition, typeDefinition.TypeParameters).AcceptVisitor(this.substitution);
					}
				}
				else
				{
					type = type2.AcceptVisitor(this.substitution);
				}
				return LazyInit.GetOrSet<IType>(ref this.declaringType, type);
			}
			internal set
			{
				this.declaringType = value;
			}
		}

		public IMember MemberDefinition
		{
			get
			{
				return this.baseMember.MemberDefinition;
			}
		}

		public IUnresolvedMember UnresolvedMember
		{
			get
			{
				return this.baseMember.UnresolvedMember;
			}
		}

		public IType ReturnType
		{
			get
			{
				IType type = LazyInit.VolatileRead<IType>(ref this.returnType);
				if (type != null)
				{
					return type;
				}
				return LazyInit.GetOrSet<IType>(ref this.returnType, this.baseMember.ReturnType.AcceptVisitor(this.substitution));
			}
			protected set
			{
				this.returnType = value;
			}
		}

		public bool IsVirtual
		{
			get
			{
				return this.baseMember.IsVirtual;
			}
		}

		public bool IsOverride
		{
			get
			{
				return this.baseMember.IsOverride;
			}
		}

		public bool IsOverridable
		{
			get
			{
				return this.baseMember.IsOverridable;
			}
		}

		public SymbolKind SymbolKind
		{
			get
			{
				return this.baseMember.SymbolKind;
			}
		}

		[Obsolete("Use the SymbolKind property instead.")]
		public EntityType EntityType
		{
			get
			{
				return this.baseMember.EntityType;
			}
		}

		public DomRegion Region
		{
			get
			{
				return this.baseMember.Region;
			}
		}

		public DomRegion BodyRegion
		{
			get
			{
				return this.baseMember.BodyRegion;
			}
		}

		public ITypeDefinition DeclaringTypeDefinition
		{
			get
			{
				return this.baseMember.DeclaringTypeDefinition;
			}
		}

		public IList<IAttribute> Attributes
		{
			get
			{
				return this.baseMember.Attributes;
			}
		}

		public IList<IMember> ImplementedInterfaceMembers
		{
			get
			{
				return LazyInitializer.EnsureInitialized<IList<IMember>>(ref this.implementedInterfaceMembers, new Func<IList<IMember>>(this.FindImplementedInterfaceMembers));
			}
		}

		private IList<IMember> FindImplementedInterfaceMembers()
		{
			IList<IMember> list = this.baseMember.ImplementedInterfaceMembers;
			IMember[] array = new IMember[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = list[i].Specialize(this.substitution);
			}
			return array;
		}

		public bool IsExplicitInterfaceImplementation
		{
			get
			{
				return this.baseMember.IsExplicitInterfaceImplementation;
			}
		}

		public DocumentationComment Documentation
		{
			get
			{
				return this.baseMember.Documentation;
			}
		}

		public Accessibility Accessibility
		{
			get
			{
				return this.baseMember.Accessibility;
			}
		}

		public bool IsStatic
		{
			get
			{
				return this.baseMember.IsStatic;
			}
		}

		public bool IsAbstract
		{
			get
			{
				return this.baseMember.IsAbstract;
			}
		}

		public bool IsSealed
		{
			get
			{
				return this.baseMember.IsSealed;
			}
		}

		public bool IsShadowing
		{
			get
			{
				return this.baseMember.IsShadowing;
			}
		}

		public bool IsSynthetic
		{
			get
			{
				return this.baseMember.IsSynthetic;
			}
		}

		public bool IsPrivate
		{
			get
			{
				return this.baseMember.IsPrivate;
			}
		}

		public bool IsPublic
		{
			get
			{
				return this.baseMember.IsPublic;
			}
		}

		public bool IsProtected
		{
			get
			{
				return this.baseMember.IsProtected;
			}
		}

		public bool IsInternal
		{
			get
			{
				return this.baseMember.IsInternal;
			}
		}

		public bool IsProtectedOrInternal
		{
			get
			{
				return this.baseMember.IsProtectedOrInternal;
			}
		}

		public bool IsProtectedAndInternal
		{
			get
			{
				return this.baseMember.IsProtectedAndInternal;
			}
		}

		public string FullName
		{
			get
			{
				return this.baseMember.FullName;
			}
		}

		public string Name
		{
			get
			{
				return this.baseMember.Name;
			}
		}

		public string Namespace
		{
			get
			{
				return this.baseMember.Namespace;
			}
		}

		public string ReflectionName
		{
			get
			{
				return this.baseMember.ReflectionName;
			}
		}

		public ICompilation Compilation
		{
			get
			{
				return this.baseMember.Compilation;
			}
		}

		public IAssembly ParentAssembly
		{
			get
			{
				return this.baseMember.ParentAssembly;
			}
		}

		public virtual IMember Specialize(TypeParameterSubstitution newSubstitution)
		{
			return this.baseMember.Specialize(TypeParameterSubstitution.Compose(newSubstitution, this.substitution));
		}

		public override bool Equals(object obj)
		{
			SpecializedMember specializedMember = obj as SpecializedMember;
			return specializedMember != null && this.baseMember.Equals(specializedMember.baseMember) && this.substitution.Equals(specializedMember.substitution);
		}

		public override int GetHashCode()
		{
			return 1000000007 * this.baseMember.GetHashCode() + 1000000009 * this.substitution.GetHashCode();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("[");
			stringBuilder.Append(base.GetType().Name);
			stringBuilder.Append(' ');
			stringBuilder.Append(this.DeclaringType.ToString());
			stringBuilder.Append('.');
			stringBuilder.Append(this.Name);
			stringBuilder.Append(':');
			stringBuilder.Append(this.ReturnType.ToString());
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		protected readonly IMember baseMember;

		private TypeParameterSubstitution substitution;

		private IType declaringType;

		private IType returnType;

		private IList<IMember> implementedInterfaceMembers;
	}
}
