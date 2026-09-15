using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	public class DefaultResolvedProperty : AbstractResolvedMember, IProperty, IParameterizedMember, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		public DefaultResolvedProperty(IUnresolvedProperty unresolved, ITypeResolveContext parentContext) : base(unresolved, parentContext)
		{
			this.unresolved = unresolved;
			this.parameters = unresolved.Parameters.CreateResolvedParameters(this.context);
		}

		public IList<IParameter> Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		public bool CanGet
		{
			get
			{
				return this.unresolved.CanGet;
			}
		}

		public bool CanSet
		{
			get
			{
				return this.unresolved.CanSet;
			}
		}

		public IMethod Getter
		{
			get
			{
				return base.GetAccessor(ref this.getter, this.unresolved.Getter);
			}
		}

		public IMethod Setter
		{
			get
			{
				return base.GetAccessor(ref this.setter, this.unresolved.Setter);
			}
		}

		public bool IsIndexer
		{
			get
			{
				return this.unresolved.IsIndexer;
			}
		}

		public override ISymbolReference ToReference()
		{
			ITypeReference typeReference = this.DeclaringType.ToTypeReference();
			if (base.IsExplicitInterfaceImplementation && base.ImplementedInterfaceMembers.Count == 1)
			{
				return new ExplicitInterfaceImplementationMemberReference(typeReference, base.ImplementedInterfaceMembers[0].ToReference());
			}
			return new DefaultMemberReference(base.SymbolKind, typeReference, base.Name, 0, (from p in this.Parameters
			select p.Type.ToTypeReference()).ToList<ITypeReference>());
		}

		public override IMember Specialize(TypeParameterSubstitution substitution)
		{
			if (TypeParameterSubstitution.Identity.Equals(substitution))
			{
				return this;
			}
			return new SpecializedProperty(this, substitution);
		}

		protected new readonly IUnresolvedProperty unresolved;

		private readonly IList<IParameter> parameters;

		private IMethod getter;

		private IMethod setter;
	}
}
