using System;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	public class DefaultResolvedField : AbstractResolvedMember, IField, IMember, IEntity, ICompilationProvider, INamedElement, IHasAccessibility, IVariable, ISymbol
	{
		public DefaultResolvedField(IUnresolvedField unresolved, ITypeResolveContext parentContext) : base(unresolved, parentContext)
		{
		}

		public bool IsReadOnly
		{
			get
			{
				return ((IUnresolvedField)this.unresolved).IsReadOnly;
			}
		}

		public bool IsVolatile
		{
			get
			{
				return ((IUnresolvedField)this.unresolved).IsVolatile;
			}
		}

		IType IVariable.Type
		{
			get
			{
				return base.ReturnType;
			}
		}

		public bool IsConst
		{
			get
			{
				return ((IUnresolvedField)this.unresolved).IsConst;
			}
		}

		public bool IsFixed
		{
			get
			{
				return ((IUnresolvedField)this.unresolved).IsFixed;
			}
		}

		public object ConstantValue
		{
			get
			{
				ResolveResult resolveResult = this.constantValue;
				if (resolveResult == null)
				{
					using (BusyManager.BusyLock busyLock = BusyManager.Enter(this))
					{
						if (!busyLock.Success)
						{
							return null;
						}
						IConstantValue constantValue = ((IUnresolvedField)this.unresolved).ConstantValue;
						if (constantValue != null)
						{
							resolveResult = constantValue.Resolve(this.context);
						}
						else
						{
							resolveResult = ErrorResolveResult.UnknownError;
						}
						this.constantValue = resolveResult;
					}
				}
				return resolveResult.ConstantValue;
			}
		}

		public override IMember Specialize(TypeParameterSubstitution substitution)
		{
			if (TypeParameterSubstitution.Identity.Equals(substitution))
			{
				return this;
			}
			return new SpecializedField(this, substitution);
		}

		IMemberReference IField.ToReference()
		{
			return (IMemberReference)this.ToReference();
		}

		private volatile ResolveResult constantValue;
	}
}
