using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Implementation of <see cref="T:ICSharpCode.NRefactory.TypeSystem.IMember" /> that resolves an unresolved member.
	/// </summary>
	public abstract class AbstractResolvedMember : AbstractResolvedEntity, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		protected AbstractResolvedMember(IUnresolvedMember unresolved, ITypeResolveContext parentContext) : base(unresolved, parentContext)
		{
			this.unresolved = unresolved;
			this.context = parentContext.WithCurrentMember(this);
		}

		IMember IMember.MemberDefinition
		{
			get
			{
				return this;
			}
		}

		public IType ReturnType
		{
			get
			{
				IType result;
				if ((result = this.returnType) == null)
				{
					result = (this.returnType = this.unresolved.ReturnType.Resolve(this.context));
				}
				return result;
			}
		}

		public IUnresolvedMember UnresolvedMember
		{
			get
			{
				return this.unresolved;
			}
		}

		public IList<IMember> ImplementedInterfaceMembers
		{
			get
			{
				IList<IMember> list = LazyInit.VolatileRead<IList<IMember>>(ref this.implementedInterfaceMembers);
				if (list != null)
				{
					return list;
				}
				return LazyInit.GetOrSet<IList<IMember>>(ref this.implementedInterfaceMembers, this.FindImplementedInterfaceMembers());
			}
		}

		private IList<IMember> FindImplementedInterfaceMembers()
		{
			if (this.unresolved.IsExplicitInterfaceImplementation)
			{
				List<IMember> list = new List<IMember>();
				foreach (IMemberReference memberReference in this.unresolved.ExplicitInterfaceImplementations)
				{
					IMember member = memberReference.Resolve(this.context);
					if (member != null)
					{
						list.Add(member);
					}
				}
				return list.ToArray();
			}
			if (this.unresolved.IsStatic || !this.unresolved.IsPublic || base.DeclaringTypeDefinition == null || base.DeclaringTypeDefinition.Kind == TypeKind.Interface)
			{
				return EmptyList<IMember>.Instance;
			}
			IMember[] source = (from m in InheritanceHelper.GetBaseMembers(this, true)
			where m.DeclaringTypeDefinition != null && m.DeclaringTypeDefinition.Kind == TypeKind.Interface
			select m).ToArray<IMember>();
			IEnumerable<IMember> otherMembers = base.DeclaringTypeDefinition.Members;
			if (base.SymbolKind == SymbolKind.Accessor)
			{
				otherMembers = base.DeclaringTypeDefinition.GetAccessors(null, GetMemberOptions.IgnoreInheritedMembers);
			}
			return (from item in source
			where !otherMembers.Any((IMember m) => m.IsExplicitInterfaceImplementation && m.ImplementedInterfaceMembers.Contains(item))
			select item).ToArray<IMember>();
		}

		public override DocumentationComment Documentation
		{
			get
			{
				IUnresolvedDocumentationProvider unresolvedDocumentationProvider = this.unresolved.UnresolvedFile as IUnresolvedDocumentationProvider;
				if (unresolvedDocumentationProvider != null)
				{
					DocumentationComment documentation = unresolvedDocumentationProvider.GetDocumentation(this.unresolved, this);
					if (documentation != null)
					{
						return documentation;
					}
				}
				return base.Documentation;
			}
		}

		public bool IsExplicitInterfaceImplementation
		{
			get
			{
				return this.unresolved.IsExplicitInterfaceImplementation;
			}
		}

		public bool IsVirtual
		{
			get
			{
				return this.unresolved.IsVirtual;
			}
		}

		public bool IsOverride
		{
			get
			{
				return this.unresolved.IsOverride;
			}
		}

		public bool IsOverridable
		{
			get
			{
				return this.unresolved.IsOverridable;
			}
		}

		public TypeParameterSubstitution Substitution
		{
			get
			{
				return TypeParameterSubstitution.Identity;
			}
		}

		public abstract IMember Specialize(TypeParameterSubstitution substitution);

		IMemberReference IMember.ToReference()
		{
			return (IMemberReference)this.ToReference();
		}

		public override ISymbolReference ToReference()
		{
			ITypeReference typeReference = this.DeclaringType.ToTypeReference();
			if (this.IsExplicitInterfaceImplementation && this.ImplementedInterfaceMembers.Count == 1)
			{
				return new ExplicitInterfaceImplementationMemberReference(typeReference, this.ImplementedInterfaceMembers[0].ToReference());
			}
			return new DefaultMemberReference(base.SymbolKind, typeReference, base.Name, 0, null);
		}

		public virtual IMemberReference ToMemberReference()
		{
			return (IMemberReference)this.ToReference();
		}

		internal IMethod GetAccessor(ref IMethod accessorField, IUnresolvedMethod unresolvedAccessor)
		{
			if (unresolvedAccessor == null)
			{
				return null;
			}
			IMethod method = LazyInit.VolatileRead<IMethod>(ref accessorField);
			if (method != null)
			{
				return method;
			}
			return LazyInit.GetOrSet<IMethod>(ref accessorField, this.CreateResolvedAccessor(unresolvedAccessor));
		}

		protected virtual IMethod CreateResolvedAccessor(IUnresolvedMethod unresolvedAccessor)
		{
			return (IMethod)unresolvedAccessor.CreateResolved(this.context);
		}

		protected new readonly IUnresolvedMember unresolved;

		protected readonly ITypeResolveContext context;

		private volatile IType returnType;

		private IList<IMember> implementedInterfaceMembers;
	}
}
