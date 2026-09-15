using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Represents a specialized IProperty (property after type substitution).
	/// </summary>
	public class SpecializedProperty : SpecializedParameterizedMember, IProperty, IParameterizedMember, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		public SpecializedProperty(IProperty propertyDefinition, TypeParameterSubstitution substitution) : base(propertyDefinition)
		{
			this.propertyDefinition = propertyDefinition;
			base.AddSubstitution(substitution);
		}

		public bool CanGet
		{
			get
			{
				return this.propertyDefinition.CanGet;
			}
		}

		public bool CanSet
		{
			get
			{
				return this.propertyDefinition.CanSet;
			}
		}

		public IMethod Getter
		{
			get
			{
				return base.WrapAccessor(ref this.getter, this.propertyDefinition.Getter);
			}
		}

		public IMethod Setter
		{
			get
			{
				return base.WrapAccessor(ref this.setter, this.propertyDefinition.Setter);
			}
		}

		public bool IsIndexer
		{
			get
			{
				return this.propertyDefinition.IsIndexer;
			}
		}

		private readonly IProperty propertyDefinition;

		private IMethod getter;

		private IMethod setter;
	}
}
