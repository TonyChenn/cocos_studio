using System;
using System.Collections.Generic;
using System.Linq;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Base class for <see cref="T:ICSharpCode.NRefactory.TypeSystem.IUnresolvedMember" /> implementations.
	/// </summary>
	[Serializable]
	public abstract class AbstractUnresolvedMember : AbstractUnresolvedEntity, IUnresolvedMember, IUnresolvedEntity, INamedElement, IHasAccessibility, IMemberReference, ISymbolReference
	{
		public override void ApplyInterningProvider(InterningProvider provider)
		{
			base.ApplyInterningProvider(provider);
			this.interfaceImplementations = provider.InternList<IMemberReference>(this.interfaceImplementations);
		}

		protected override void FreezeInternal()
		{
			base.FreezeInternal();
			this.interfaceImplementations = FreezableHelper.FreezeList<IMemberReference>(this.interfaceImplementations);
		}

		public override object Clone()
		{
			AbstractUnresolvedMember abstractUnresolvedMember = (AbstractUnresolvedMember)base.Clone();
			if (this.interfaceImplementations != null)
			{
				abstractUnresolvedMember.interfaceImplementations = new List<IMemberReference>(this.interfaceImplementations);
			}
			return abstractUnresolvedMember;
		}

		public ITypeReference ReturnType
		{
			get
			{
				return this.returnType;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.ThrowIfFrozen();
				this.returnType = value;
			}
		}

		public bool IsExplicitInterfaceImplementation
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

		public IList<IMemberReference> ExplicitInterfaceImplementations
		{
			get
			{
				if (this.interfaceImplementations == null)
				{
					this.interfaceImplementations = new List<IMemberReference>();
				}
				return this.interfaceImplementations;
			}
		}

		public bool IsVirtual
		{
			get
			{
				return this.flags[128];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[128] = value;
			}
		}

		public bool IsOverride
		{
			get
			{
				return this.flags[256];
			}
			set
			{
				base.ThrowIfFrozen();
				this.flags[256] = value;
			}
		}

		public bool IsOverridable
		{
			get
			{
				return (this.flags.Data & 388) != 0 && !base.IsSealed;
			}
		}

		ITypeReference IMemberReference.DeclaringTypeReference
		{
			get
			{
				return base.DeclaringTypeDefinition;
			}
		}

		public abstract IMember CreateResolved(ITypeResolveContext context);

		public virtual IMember Resolve(ITypeResolveContext context)
		{
			ITypeReference explicitInterfaceTypeReference = null;
			if (this.IsExplicitInterfaceImplementation && this.ExplicitInterfaceImplementations.Count == 1)
			{
				explicitInterfaceTypeReference = this.ExplicitInterfaceImplementations[0].DeclaringTypeReference;
			}
			return AbstractUnresolvedMember.Resolve(AbstractUnresolvedMember.ExtendContextForType(context, base.DeclaringTypeDefinition), base.SymbolKind, base.Name, explicitInterfaceTypeReference, null, null);
		}

		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			return ((IUnresolvedMember)this).Resolve(context);
		}

		protected static ITypeResolveContext ExtendContextForType(ITypeResolveContext assemblyContext, IUnresolvedTypeDefinition typeDef)
		{
			if (typeDef == null)
			{
				return assemblyContext;
			}
			ITypeResolveContext parentContext;
			if (typeDef.DeclaringTypeDefinition != null)
			{
				parentContext = AbstractUnresolvedMember.ExtendContextForType(assemblyContext, typeDef.DeclaringTypeDefinition);
			}
			else
			{
				parentContext = assemblyContext;
			}
			ITypeDefinition definition = typeDef.Resolve(assemblyContext).GetDefinition();
			return typeDef.CreateResolveContext(parentContext).WithCurrentTypeDefinition(definition);
		}

		public static IMember Resolve(ITypeResolveContext context, SymbolKind symbolKind, string name, ITypeReference explicitInterfaceTypeReference = null, IList<string> typeParameterNames = null, IList<ITypeReference> parameterTypeReferences = null)
		{
			if (context.CurrentTypeDefinition == null)
			{
				return null;
			}
			if (parameterTypeReferences == null)
			{
				parameterTypeReferences = EmptyList<ITypeReference>.Instance;
			}
			if (typeParameterNames == null || typeParameterNames.Count == 0)
			{
				IList<IType> parameterTypes = parameterTypeReferences.Resolve(context);
				if (explicitInterfaceTypeReference == null)
				{
					using (IEnumerator<IMember> enumerator = context.CurrentTypeDefinition.Members.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IMember member = enumerator.Current;
							if (!member.IsExplicitInterfaceImplementation && AbstractUnresolvedMember.IsNonGenericMatch(member, symbolKind, name, parameterTypes))
							{
								return member;
							}
						}
						goto IL_21A;
					}
				}
				IType type = explicitInterfaceTypeReference.Resolve(context);
				using (IEnumerator<IMember> enumerator2 = context.CurrentTypeDefinition.Members.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						IMember member2 = enumerator2.Current;
						if (member2.IsExplicitInterfaceImplementation && member2.ImplementedInterfaceMembers.Count == 1 && AbstractUnresolvedMember.IsNonGenericMatch(member2, symbolKind, name, parameterTypes) && type.Equals(member2.ImplementedInterfaceMembers[0].DeclaringType))
						{
							return member2;
						}
					}
					goto IL_21A;
				}
			}
			foreach (IMethod method in context.CurrentTypeDefinition.Methods)
			{
				if (method.SymbolKind == symbolKind && !(method.Name != name) && method.Parameters.Count == parameterTypeReferences.Count)
				{
					if (typeParameterNames.SequenceEqual(from tp in method.TypeParameters
					select tp.Name))
					{
						ITypeResolveContext context2 = context.WithCurrentMember(method);
						IList<IType> parameterTypes2 = parameterTypeReferences.Resolve(context2);
						if (AbstractUnresolvedMember.IsParameterTypeMatch(method, parameterTypes2))
						{
							if (explicitInterfaceTypeReference == null)
							{
								if (!method.IsExplicitInterfaceImplementation)
								{
									return method;
								}
							}
							else if (method.IsExplicitInterfaceImplementation && method.ImplementedInterfaceMembers.Count == 1)
							{
								IType type2 = explicitInterfaceTypeReference.Resolve(context2);
								if (type2.Equals(method.ImplementedInterfaceMembers[0].DeclaringType))
								{
									return method;
								}
							}
						}
					}
				}
			}
			IL_21A:
			return null;
		}

		private static bool IsNonGenericMatch(IMember member, SymbolKind symbolKind, string name, IList<IType> parameterTypes)
		{
			if (member.SymbolKind != symbolKind)
			{
				return false;
			}
			if (member.Name != name)
			{
				return false;
			}
			IMethod method = member as IMethod;
			return (method == null || method.TypeParameters.Count <= 0) && AbstractUnresolvedMember.IsParameterTypeMatch(member, parameterTypes);
		}

		private static bool IsParameterTypeMatch(IMember member, IList<IType> parameterTypes)
		{
			IParameterizedMember parameterizedMember = member as IParameterizedMember;
			if (parameterizedMember == null)
			{
				return parameterTypes.Count == 0;
			}
			if (parameterTypes.Count == parameterizedMember.Parameters.Count)
			{
				for (int i = 0; i < parameterTypes.Count; i++)
				{
					IType type = parameterTypes[i];
					IType type2 = parameterizedMember.Parameters[i].Type;
					if (!type.Equals(type2))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		private ITypeReference returnType = SpecialType.UnknownType;

		private IList<IMemberReference> interfaceImplementations;
	}
}
