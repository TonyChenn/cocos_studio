using System;
using System.Globalization;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	[Serializable]
	public sealed class TypeParameterReference : ITypeReference, ISymbolReference
	{
		/// <summary>
		/// Creates a type parameter reference.
		/// For common type parameter references, this method may return a shared instance.
		/// </summary>
		public static TypeParameterReference Create(SymbolKind ownerType, int index)
		{
			if (index >= 0 && index < 8 && (ownerType == SymbolKind.TypeDefinition || ownerType == SymbolKind.Method))
			{
				TypeParameterReference[] array = (ownerType == SymbolKind.TypeDefinition) ? TypeParameterReference.classTypeParameterReferences : TypeParameterReference.methodTypeParameterReferences;
				TypeParameterReference typeParameterReference = LazyInit.VolatileRead<TypeParameterReference>(ref array[index]);
				if (typeParameterReference == null)
				{
					typeParameterReference = LazyInit.GetOrSet<TypeParameterReference>(ref array[index], new TypeParameterReference(ownerType, index));
				}
				return typeParameterReference;
			}
			return new TypeParameterReference(ownerType, index);
		}

		public int Index
		{
			get
			{
				return this.index;
			}
		}

		public TypeParameterReference(SymbolKind ownerType, int index)
		{
			this.ownerType = ownerType;
			this.index = index;
		}

		public IType Resolve(ITypeResolveContext context)
		{
			if (this.ownerType == SymbolKind.Method)
			{
				IMethod method = context.CurrentMember as IMethod;
				if (method != null && this.index < method.TypeParameters.Count)
				{
					return method.TypeParameters[this.index];
				}
				return DummyTypeParameter.GetMethodTypeParameter(this.index);
			}
			else
			{
				if (this.ownerType != SymbolKind.TypeDefinition)
				{
					return SpecialType.UnknownType;
				}
				ITypeDefinition currentTypeDefinition = context.CurrentTypeDefinition;
				if (currentTypeDefinition != null && this.index < currentTypeDefinition.TypeParameters.Count)
				{
					return currentTypeDefinition.TypeParameters[this.index];
				}
				return DummyTypeParameter.GetClassTypeParameter(this.index);
			}
		}

		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			return this.Resolve(context) as ISymbol;
		}

		public override string ToString()
		{
			if (this.ownerType == SymbolKind.Method)
			{
				return "!!" + this.index.ToString(CultureInfo.InvariantCulture);
			}
			return "!" + this.index.ToString(CultureInfo.InvariantCulture);
		}

		private static readonly TypeParameterReference[] classTypeParameterReferences = new TypeParameterReference[8];

		private static readonly TypeParameterReference[] methodTypeParameterReferences = new TypeParameterReference[8];

		private readonly SymbolKind ownerType;

		private readonly int index;
	}
}
