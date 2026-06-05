using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x020000B6 RID: 182
	public class DefaultTypeParameter : AbstractTypeParameter
	{
		// Token: 0x06000648 RID: 1608 RVA: 0x000105DD File Offset: 0x0000F5DD
		public DefaultTypeParameter(IEntity owner, int index, string name = null, VarianceModifier variance = VarianceModifier.Invariant, IList<IAttribute> attributes = null, DomRegion region = default(DomRegion), bool hasValueTypeConstraint = false, bool hasReferenceTypeConstraint = false, bool hasDefaultConstructorConstraint = false, IList<IType> constraints = null) : base(owner, index, name, variance, attributes, region)
		{
			this.hasValueTypeConstraint = hasValueTypeConstraint;
			this.hasReferenceTypeConstraint = hasReferenceTypeConstraint;
			this.hasDefaultConstructorConstraint = hasDefaultConstructorConstraint;
			this.constraints = (constraints ?? EmptyList<IType>.Instance);
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00010617 File Offset: 0x0000F617
		public DefaultTypeParameter(ICompilation compilation, SymbolKind ownerType, int index, string name = null, VarianceModifier variance = VarianceModifier.Invariant, IList<IAttribute> attributes = null, DomRegion region = default(DomRegion), bool hasValueTypeConstraint = false, bool hasReferenceTypeConstraint = false, bool hasDefaultConstructorConstraint = false, IList<IType> constraints = null) : base(compilation, ownerType, index, name, variance, attributes, region)
		{
			this.hasValueTypeConstraint = hasValueTypeConstraint;
			this.hasReferenceTypeConstraint = hasReferenceTypeConstraint;
			this.hasDefaultConstructorConstraint = hasDefaultConstructorConstraint;
			this.constraints = (constraints ?? EmptyList<IType>.Instance);
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x00010653 File Offset: 0x0000F653
		public override bool HasValueTypeConstraint
		{
			get
			{
				return this.hasValueTypeConstraint;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x0001065B File Offset: 0x0000F65B
		public override bool HasReferenceTypeConstraint
		{
			get
			{
				return this.hasReferenceTypeConstraint;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x00010663 File Offset: 0x0000F663
		public override bool HasDefaultConstructorConstraint
		{
			get
			{
				return this.hasDefaultConstructorConstraint;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00010874 File Offset: 0x0000F874
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

		// Token: 0x040001CC RID: 460
		private readonly bool hasValueTypeConstraint;

		// Token: 0x040001CD RID: 461
		private readonly bool hasReferenceTypeConstraint;

		// Token: 0x040001CE RID: 462
		private readonly bool hasDefaultConstructorConstraint;

		// Token: 0x040001CF RID: 463
		private readonly IList<IType> constraints;
	}
}
