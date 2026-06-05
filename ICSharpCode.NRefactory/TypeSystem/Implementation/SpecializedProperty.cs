using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Represents a specialized IProperty (property after type substitution).
	/// </summary>
	// Token: 0x020000E3 RID: 227
	public class SpecializedProperty : SpecializedParameterizedMember, IProperty, IParameterizedMember, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x06000881 RID: 2177 RVA: 0x00016574 File Offset: 0x00015574
		public SpecializedProperty(IProperty propertyDefinition, TypeParameterSubstitution substitution) : base(propertyDefinition)
		{
			this.propertyDefinition = propertyDefinition;
			base.AddSubstitution(substitution);
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x0001658B File Offset: 0x0001558B
		public bool CanGet
		{
			get
			{
				return this.propertyDefinition.CanGet;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x00016598 File Offset: 0x00015598
		public bool CanSet
		{
			get
			{
				return this.propertyDefinition.CanSet;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x000165A5 File Offset: 0x000155A5
		public IMethod Getter
		{
			get
			{
				return base.WrapAccessor(ref this.getter, this.propertyDefinition.Getter);
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x000165BE File Offset: 0x000155BE
		public IMethod Setter
		{
			get
			{
				return base.WrapAccessor(ref this.setter, this.propertyDefinition.Setter);
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x000165D7 File Offset: 0x000155D7
		public bool IsIndexer
		{
			get
			{
				return this.propertyDefinition.IsIndexer;
			}
		}

		// Token: 0x0400026C RID: 620
		private readonly IProperty propertyDefinition;

		// Token: 0x0400026D RID: 621
		private IMethod getter;

		// Token: 0x0400026E RID: 622
		private IMethod setter;
	}
}
