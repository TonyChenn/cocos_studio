using System;
using System.Collections.Generic;
using System.Threading;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	public sealed class DummyTypeParameter : AbstractType, ITypeParameter, IType, INamedElement, IEquatable<IType>, ISymbol
	{
		public static ITypeParameter GetMethodTypeParameter(int index)
		{
			return DummyTypeParameter.GetTypeParameter(ref DummyTypeParameter.methodTypeParameters, SymbolKind.Method, index);
		}

		public static ITypeParameter GetClassTypeParameter(int index)
		{
			return DummyTypeParameter.GetTypeParameter(ref DummyTypeParameter.classTypeParameters, SymbolKind.TypeDefinition, index);
		}

		private static ITypeParameter GetTypeParameter(ref ITypeParameter[] typeParameters, SymbolKind symbolKind, int index)
		{
			ITypeParameter[] array = typeParameters;
			while (index >= array.Length)
			{
				ITypeParameter[] array2 = new ITypeParameter[index + 1];
				array.CopyTo(array2, 0);
				for (int i = array.Length; i < array2.Length; i++)
				{
					array2[i] = new DummyTypeParameter(symbolKind, i);
				}
				ITypeParameter[] array3 = Interlocked.CompareExchange<ITypeParameter[]>(ref typeParameters, array2, array);
				if (array3 == array)
				{
					array = array2;
				}
				else
				{
					array = array3;
				}
			}
			return array[index];
		}

		/// <summary>
		/// Replaces all occurrences of method type parameters in the given type
		/// by normalized type parameters. This allows comparing parameter types from different
		/// generic methods.
		/// </summary>
		public static IType NormalizeMethodTypeParameters(IType type)
		{
			return type.AcceptVisitor(DummyTypeParameter.normalizeMethodTypeParameters);
		}

		/// <summary>
		/// Replaces all occurrences of class type parameters in the given type
		/// by normalized type parameters. This allows comparing parameter types from different
		/// generic methods.
		/// </summary>
		public static IType NormalizeClassTypeParameters(IType type)
		{
			return type.AcceptVisitor(DummyTypeParameter.normalizeClassTypeParameters);
		}

		/// <summary>
		/// Replaces all occurrences of class and method type parameters in the given type
		/// by normalized type parameters. This allows comparing parameter types from different
		/// generic methods.
		/// </summary>
		public static IType NormalizeAllTypeParameters(IType type)
		{
			return type.AcceptVisitor(DummyTypeParameter.normalizeClassTypeParameters).AcceptVisitor(DummyTypeParameter.normalizeMethodTypeParameters);
		}

		private DummyTypeParameter(SymbolKind ownerType, int index)
		{
			this.ownerType = ownerType;
			this.index = index;
		}

		SymbolKind ISymbol.SymbolKind
		{
			get
			{
				return SymbolKind.TypeParameter;
			}
		}

		public override string Name
		{
			get
			{
				return ((this.ownerType == SymbolKind.Method) ? "!!" : "!") + this.index;
			}
		}

		public override string ReflectionName
		{
			get
			{
				return ((this.ownerType == SymbolKind.Method) ? "``" : "`") + this.index;
			}
		}

		public override string ToString()
		{
			return this.ReflectionName + " (dummy)";
		}

		public override bool? IsReferenceType
		{
			get
			{
				return null;
			}
		}

		public override TypeKind Kind
		{
			get
			{
				return TypeKind.TypeParameter;
			}
		}

		public override ITypeReference ToTypeReference()
		{
			return TypeParameterReference.Create(this.ownerType, this.index);
		}

		public override IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitTypeParameter(this);
		}

		public int Index
		{
			get
			{
				return this.index;
			}
		}

		IList<IAttribute> ITypeParameter.Attributes
		{
			get
			{
				return EmptyList<IAttribute>.Instance;
			}
		}

		SymbolKind ITypeParameter.OwnerType
		{
			get
			{
				return this.ownerType;
			}
		}

		VarianceModifier ITypeParameter.Variance
		{
			get
			{
				return VarianceModifier.Invariant;
			}
		}

		DomRegion ITypeParameter.Region
		{
			get
			{
				return DomRegion.Empty;
			}
		}

		IEntity ITypeParameter.Owner
		{
			get
			{
				return null;
			}
		}

		IType ITypeParameter.EffectiveBaseClass
		{
			get
			{
				return SpecialType.UnknownType;
			}
		}

		ICollection<IType> ITypeParameter.EffectiveInterfaceSet
		{
			get
			{
				return EmptyList<IType>.Instance;
			}
		}

		bool ITypeParameter.HasDefaultConstructorConstraint
		{
			get
			{
				return false;
			}
		}

		bool ITypeParameter.HasReferenceTypeConstraint
		{
			get
			{
				return false;
			}
		}

		bool ITypeParameter.HasValueTypeConstraint
		{
			get
			{
				return false;
			}
		}

		public ISymbolReference ToReference()
		{
			return new TypeParameterReference(this.ownerType, this.index);
		}

		private static ITypeParameter[] methodTypeParameters = new ITypeParameter[]
		{
			new DummyTypeParameter(SymbolKind.Method, 0)
		};

		private static ITypeParameter[] classTypeParameters = new ITypeParameter[]
		{
			new DummyTypeParameter(SymbolKind.TypeDefinition, 0)
		};

		private static readonly DummyTypeParameter.NormalizeMethodTypeParametersVisitor normalizeMethodTypeParameters = new DummyTypeParameter.NormalizeMethodTypeParametersVisitor();

		private static readonly DummyTypeParameter.NormalizeClassTypeParametersVisitor normalizeClassTypeParameters = new DummyTypeParameter.NormalizeClassTypeParametersVisitor();

		private readonly SymbolKind ownerType;

		private readonly int index;

		private sealed class NormalizeMethodTypeParametersVisitor : TypeVisitor
		{
			public override IType VisitTypeParameter(ITypeParameter type)
			{
				if (type.OwnerType == SymbolKind.Method)
				{
					return DummyTypeParameter.GetMethodTypeParameter(type.Index);
				}
				return base.VisitTypeParameter(type);
			}
		}

		private sealed class NormalizeClassTypeParametersVisitor : TypeVisitor
		{
			public override IType VisitTypeParameter(ITypeParameter type)
			{
				if (type.OwnerType == SymbolKind.TypeDefinition)
				{
					return DummyTypeParameter.GetClassTypeParameter(type.Index);
				}
				return base.VisitTypeParameter(type);
			}
		}
	}
}
