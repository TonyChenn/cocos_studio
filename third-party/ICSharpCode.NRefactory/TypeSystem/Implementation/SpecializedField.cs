using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Represents a specialized IField (field after type substitution).
	/// </summary>
	public class SpecializedField : SpecializedMember, IField, IMember, IEntity, ICompilationProvider, INamedElement, IHasAccessibility, IVariable, ISymbol
	{
		public SpecializedField(IField fieldDefinition, TypeParameterSubstitution substitution) : base(fieldDefinition)
		{
			this.fieldDefinition = fieldDefinition;
			base.AddSubstitution(substitution);
		}

		public bool IsReadOnly
		{
			get
			{
				return this.fieldDefinition.IsReadOnly;
			}
		}

		public bool IsVolatile
		{
			get
			{
				return this.fieldDefinition.IsVolatile;
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
				return this.fieldDefinition.IsConst;
			}
		}

		public bool IsFixed
		{
			get
			{
				return this.fieldDefinition.IsFixed;
			}
		}

		public object ConstantValue
		{
			get
			{
				return this.fieldDefinition.ConstantValue;
			}
		}

		private readonly IField fieldDefinition;
	}
}
