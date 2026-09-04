using System;
using System.Collections.Generic;
using System.Threading;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x020000CD RID: 205
	public sealed class DummyTypeParameter : AbstractType, ITypeParameter, IType, INamedElement, IEquatable<IType>, ISymbol
	{
		// Token: 0x0600078F RID: 1935 RVA: 0x00013334 File Offset: 0x00012334
		public static ITypeParameter GetMethodTypeParameter(int index)
		{
			return DummyTypeParameter.GetTypeParameter(ref DummyTypeParameter.methodTypeParameters, SymbolKind.Method, index);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00013342 File Offset: 0x00012342
		public static ITypeParameter GetClassTypeParameter(int index)
		{
			return DummyTypeParameter.GetTypeParameter(ref DummyTypeParameter.classTypeParameters, SymbolKind.TypeDefinition, index);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00013350 File Offset: 0x00012350
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
		// Token: 0x06000792 RID: 1938 RVA: 0x000133A9 File Offset: 0x000123A9
		public static IType NormalizeMethodTypeParameters(IType type)
		{
			return type.AcceptVisitor(DummyTypeParameter.normalizeMethodTypeParameters);
		}

		/// <summary>
		/// Replaces all occurrences of class type parameters in the given type
		/// by normalized type parameters. This allows comparing parameter types from different
		/// generic methods.
		/// </summary>
		// Token: 0x06000793 RID: 1939 RVA: 0x000133B6 File Offset: 0x000123B6
		public static IType NormalizeClassTypeParameters(IType type)
		{
			return type.AcceptVisitor(DummyTypeParameter.normalizeClassTypeParameters);
		}

		/// <summary>
		/// Replaces all occurrences of class and method type parameters in the given type
		/// by normalized type parameters. This allows comparing parameter types from different
		/// generic methods.
		/// </summary>
		// Token: 0x06000794 RID: 1940 RVA: 0x000133C3 File Offset: 0x000123C3
		public static IType NormalizeAllTypeParameters(IType type)
		{
			return type.AcceptVisitor(DummyTypeParameter.normalizeClassTypeParameters).AcceptVisitor(DummyTypeParameter.normalizeMethodTypeParameters);
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x000133DA File Offset: 0x000123DA
		private DummyTypeParameter(SymbolKind ownerType, int index)
		{
			this.ownerType = ownerType;
			this.index = index;
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x000133F0 File Offset: 0x000123F0
		SymbolKind ISymbol.SymbolKind
		{
			get
			{
				return SymbolKind.TypeParameter;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x000133F4 File Offset: 0x000123F4
		public override string Name
		{
			get
			{
				return ((this.ownerType == SymbolKind.Method) ? "!!" : "!") + this.index;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x0001341B File Offset: 0x0001241B
		public override string ReflectionName
		{
			get
			{
				return ((this.ownerType == SymbolKind.Method) ? "``" : "`") + this.index;
			}
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x00013442 File Offset: 0x00012442
		public override string ToString()
		{
			return this.ReflectionName + " (dummy)";
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x00013454 File Offset: 0x00012454
		public override bool? IsReferenceType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x0001346A File Offset: 0x0001246A
		public override TypeKind Kind
		{
			get
			{
				return TypeKind.TypeParameter;
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001346E File Offset: 0x0001246E
		public override ITypeReference ToTypeReference()
		{
			return TypeParameterReference.Create(this.ownerType, this.index);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00013481 File Offset: 0x00012481
		public override IType AcceptVisitor(TypeVisitor visitor)
		{
			return visitor.VisitTypeParameter(this);
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x0001348A File Offset: 0x0001248A
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x00013492 File Offset: 0x00012492
		IList<IAttribute> ITypeParameter.Attributes
		{
			get
			{
				return EmptyList<IAttribute>.Instance;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00013499 File Offset: 0x00012499
		SymbolKind ITypeParameter.OwnerType
		{
			get
			{
				return this.ownerType;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x000134A1 File Offset: 0x000124A1
		VarianceModifier ITypeParameter.Variance
		{
			get
			{
				return VarianceModifier.Invariant;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x000134A4 File Offset: 0x000124A4
		DomRegion ITypeParameter.Region
		{
			get
			{
				return DomRegion.Empty;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x000134AB File Offset: 0x000124AB
		IEntity ITypeParameter.Owner
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x000134AE File Offset: 0x000124AE
		IType ITypeParameter.EffectiveBaseClass
		{
			get
			{
				return SpecialType.UnknownType;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x000134B5 File Offset: 0x000124B5
		ICollection<IType> ITypeParameter.EffectiveInterfaceSet
		{
			get
			{
				return EmptyList<IType>.Instance;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x000134BC File Offset: 0x000124BC
		bool ITypeParameter.HasDefaultConstructorConstraint
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x000134BF File Offset: 0x000124BF
		bool ITypeParameter.HasReferenceTypeConstraint
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x000134C2 File Offset: 0x000124C2
		bool ITypeParameter.HasValueTypeConstraint
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x000134C5 File Offset: 0x000124C5
		public ISymbolReference ToReference()
		{
			return new TypeParameterReference(this.ownerType, this.index);
		}

		// Token: 0x04000230 RID: 560
		private static ITypeParameter[] methodTypeParameters = new ITypeParameter[]
		{
			new DummyTypeParameter(SymbolKind.Method, 0)
		};

		// Token: 0x04000231 RID: 561
		private static ITypeParameter[] classTypeParameters = new ITypeParameter[]
		{
			new DummyTypeParameter(SymbolKind.TypeDefinition, 0)
		};

		// Token: 0x04000232 RID: 562
		private static readonly DummyTypeParameter.NormalizeMethodTypeParametersVisitor normalizeMethodTypeParameters = new DummyTypeParameter.NormalizeMethodTypeParametersVisitor();

		// Token: 0x04000233 RID: 563
		private static readonly DummyTypeParameter.NormalizeClassTypeParametersVisitor normalizeClassTypeParameters = new DummyTypeParameter.NormalizeClassTypeParametersVisitor();

		// Token: 0x04000234 RID: 564
		private readonly SymbolKind ownerType;

		// Token: 0x04000235 RID: 565
		private readonly int index;

		// Token: 0x020000CE RID: 206
		private sealed class NormalizeMethodTypeParametersVisitor : TypeVisitor
		{
			// Token: 0x060007AB RID: 1963 RVA: 0x00013527 File Offset: 0x00012527
			public override IType VisitTypeParameter(ITypeParameter type)
			{
				if (type.OwnerType == SymbolKind.Method)
				{
					return DummyTypeParameter.GetMethodTypeParameter(type.Index);
				}
				return base.VisitTypeParameter(type);
			}
		}

		// Token: 0x020000CF RID: 207
		private sealed class NormalizeClassTypeParametersVisitor : TypeVisitor
		{
			// Token: 0x060007AD RID: 1965 RVA: 0x0001354D File Offset: 0x0001254D
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
