using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Represents a specialized IField (field after type substitution).
	/// </summary>
	// Token: 0x020000DF RID: 223
	public class SpecializedField : SpecializedMember, IField, IMember, IEntity, ICompilationProvider, INamedElement, IHasAccessibility, IVariable, ISymbol
	{
		// Token: 0x06000855 RID: 2133 RVA: 0x00015DEA File Offset: 0x00014DEA
		public SpecializedField(IField fieldDefinition, TypeParameterSubstitution substitution) : base(fieldDefinition)
		{
			this.fieldDefinition = fieldDefinition;
			base.AddSubstitution(substitution);
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x00015E01 File Offset: 0x00014E01
		public bool IsReadOnly
		{
			get
			{
				return this.fieldDefinition.IsReadOnly;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x00015E0E File Offset: 0x00014E0E
		public bool IsVolatile
		{
			get
			{
				return this.fieldDefinition.IsVolatile;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x00015E1B File Offset: 0x00014E1B
		IType IVariable.Type
		{
			get
			{
				return base.ReturnType;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x00015E23 File Offset: 0x00014E23
		public bool IsConst
		{
			get
			{
				return this.fieldDefinition.IsConst;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x00015E30 File Offset: 0x00014E30
		public bool IsFixed
		{
			get
			{
				return this.fieldDefinition.IsFixed;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x00015E3D File Offset: 0x00014E3D
		public object ConstantValue
		{
			get
			{
				return this.fieldDefinition.ConstantValue;
			}
		}

		// Token: 0x04000263 RID: 611
		private readonly IField fieldDefinition;
	}
}
