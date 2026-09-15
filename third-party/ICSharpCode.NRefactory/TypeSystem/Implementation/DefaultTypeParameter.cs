using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	public class DefaultTypeParameter : AbstractTypeParameter
	{
		public DefaultTypeParameter(IEntity owner, int index, string name = null, VarianceModifier variance = VarianceModifier.Invariant, IList<IAttribute> attributes = null, DomRegion region = default(DomRegion), bool hasValueTypeConstraint = false, bool hasReferenceTypeConstraint = false, bool hasDefaultConstructorConstraint = false, IList<IType> constraints = null) : base(owner, index, name, variance, attributes, region)
		{
			this.hasValueTypeConstraint = hasValueTypeConstraint;
			this.hasReferenceTypeConstraint = hasReferenceTypeConstraint;
			this.hasDefaultConstructorConstraint = hasDefaultConstructorConstraint;
			this.constraints = (constraints ?? EmptyList<IType>.Instance);
		}

		public DefaultTypeParameter(ICompilation compilation, SymbolKind ownerType, int index, string name = null, VarianceModifier variance = VarianceModifier.Invariant, IList<IAttribute> attributes = null, DomRegion region = default(DomRegion), bool hasValueTypeConstraint = false, bool hasReferenceTypeConstraint = false, bool hasDefaultConstructorConstraint = false, IList<IType> constraints = null) : base(compilation, ownerType, index, name, variance, attributes, region)
		{
			this.hasValueTypeConstraint = hasValueTypeConstraint;
			this.hasReferenceTypeConstraint = hasReferenceTypeConstraint;
			this.hasDefaultConstructorConstraint = hasDefaultConstructorConstraint;
			this.constraints = (constraints ?? EmptyList<IType>.Instance);
		}

		public override bool HasValueTypeConstraint
		{
			get
			{
				return this.hasValueTypeConstraint;
			}
		}

		public override bool HasReferenceTypeConstraint
		{
			get
			{
				return this.hasReferenceTypeConstraint;
			}
		}

		public override bool HasDefaultConstructorConstraint
		{
			get
			{
				return this.hasDefaultConstructorConstraint;
			}
		}

		public override IEnumerable<IType> DirectBaseTypes
		{
			get
			{
				bool hasNonInterfaceConstraint = false;
				foreach (IType c in this.constraints)
				{
					yield return c;
					if (c.Kind != TypeKind.Interface)
					{
						hasNonInterfaceConstraint = true;
					}
				}
				if (this.HasValueTypeConstraint || !hasNonInterfaceConstraint)
				{
					yield return base.Compilation.FindType(this.HasValueTypeConstraint ? KnownTypeCode.ValueType : KnownTypeCode.Object);
				}
				yield break;
			}
		}

		private readonly bool hasValueTypeConstraint;

		private readonly bool hasReferenceTypeConstraint;

		private readonly bool hasDefaultConstructorConstraint;

		private readonly IList<IType> constraints;
	}
}
