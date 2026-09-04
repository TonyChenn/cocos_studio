using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Represents a specialized IMethod (e.g. after type substitution).
	/// </summary>
	// Token: 0x020000E1 RID: 225
	public class SpecializedMethod : SpecializedParameterizedMember, IMethod, IParameterizedMember, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
		// Token: 0x06000861 RID: 2145 RVA: 0x00016004 File Offset: 0x00015004
		public SpecializedMethod(IMethod methodDefinition, TypeParameterSubstitution substitution) : base(methodDefinition)
		{
			if (substitution == null)
			{
				throw new ArgumentNullException("substitution");
			}
			this.methodDefinition = methodDefinition;
			this.isParameterized = (substitution.MethodTypeArguments != null);
			if (methodDefinition.TypeParameters.Count > 0)
			{
				this.specializedTypeParameters = new ITypeParameter[methodDefinition.TypeParameters.Count];
				for (int i = 0; i < this.specializedTypeParameters.Length; i++)
				{
					this.specializedTypeParameters[i] = new SpecializedMethod.SpecializedTypeParameter(methodDefinition.TypeParameters[i], this);
				}
				if (!this.isParameterized)
				{
					this.substitutionWithoutSpecializedTypeParameters = base.Substitution;
					base.AddSubstitution(new TypeParameterSubstitution(null, this.specializedTypeParameters));
				}
			}
			base.AddSubstitution(substitution);
			if (this.substitutionWithoutSpecializedTypeParameters != null)
			{
				this.substitutionWithoutSpecializedTypeParameters = TypeParameterSubstitution.Compose(substitution, this.substitutionWithoutSpecializedTypeParameters);
			}
			else
			{
				this.substitutionWithoutSpecializedTypeParameters = base.Substitution;
			}
			if (this.specializedTypeParameters != null)
			{
				foreach (SpecializedMethod.SpecializedTypeParameter specializedTypeParameter in this.specializedTypeParameters.OfType<SpecializedMethod.SpecializedTypeParameter>())
				{
					if (specializedTypeParameter.Owner == this)
					{
						specializedTypeParameter.substitution = base.Substitution;
					}
				}
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x00016140 File Offset: 0x00015140
		public IList<IType> TypeArguments
		{
			get
			{
				return base.Substitution.MethodTypeArguments ?? EmptyList<IType>.Instance;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x00016156 File Offset: 0x00015156
		public bool IsParameterized
		{
			get
			{
				return this.isParameterized;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x0001615E File Offset: 0x0001515E
		public IList<IUnresolvedMethod> Parts
		{
			get
			{
				return this.methodDefinition.Parts;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x0001616B File Offset: 0x0001516B
		public IList<IAttribute> ReturnTypeAttributes
		{
			get
			{
				return this.methodDefinition.ReturnTypeAttributes;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x00016178 File Offset: 0x00015178
		public IList<ITypeParameter> TypeParameters
		{
			get
			{
				ITypeParameter[] array = this.specializedTypeParameters;
				if (array == null)
				{
					return this.methodDefinition.TypeParameters;
				}
				return (IList<ITypeParameter>)array;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x00016195 File Offset: 0x00015195
		public bool IsExtensionMethod
		{
			get
			{
				return this.methodDefinition.IsExtensionMethod;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x000161A2 File Offset: 0x000151A2
		public bool IsConstructor
		{
			get
			{
				return this.methodDefinition.IsConstructor;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x000161AF File Offset: 0x000151AF
		public bool IsDestructor
		{
			get
			{
				return this.methodDefinition.IsDestructor;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x000161BC File Offset: 0x000151BC
		public bool IsOperator
		{
			get
			{
				return this.methodDefinition.IsOperator;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x000161C9 File Offset: 0x000151C9
		public bool IsPartial
		{
			get
			{
				return this.methodDefinition.IsPartial;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x000161D6 File Offset: 0x000151D6
		public bool IsAsync
		{
			get
			{
				return this.methodDefinition.IsAsync;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x0600086D RID: 2157 RVA: 0x000161E3 File Offset: 0x000151E3
		public bool HasBody
		{
			get
			{
				return this.methodDefinition.HasBody;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x000161F0 File Offset: 0x000151F0
		public bool IsAccessor
		{
			get
			{
				return this.methodDefinition.IsAccessor;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x000161FD File Offset: 0x000151FD
		public IMethod ReducedFrom
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x00016200 File Offset: 0x00015200
		// (set) Token: 0x06000871 RID: 2161 RVA: 0x00016248 File Offset: 0x00015248
		public IMember AccessorOwner
		{
			get
			{
				IMember member = LazyInit.VolatileRead<IMember>(ref this.accessorOwner);
				if (member != null)
				{
					return member;
				}
				IMember member2 = this.methodDefinition.AccessorOwner;
				if (member2 == null)
				{
					return null;
				}
				member = member2.Specialize(base.Substitution);
				return LazyInit.GetOrSet<IMember>(ref this.accessorOwner, member);
			}
			internal set
			{
				this.accessorOwner = value;
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00016254 File Offset: 0x00015254
		public override IMemberReference ToReference()
		{
			if (this.isParameterized)
			{
				return new SpecializingMemberReference(this.baseMember.ToReference(), SpecializedMember.ToTypeReference(base.Substitution.ClassTypeArguments), SpecializedMember.ToTypeReference(base.Substitution.MethodTypeArguments));
			}
			return base.ToReference();
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x000162A0 File Offset: 0x000152A0
		public override IMemberReference ToMemberReference()
		{
			return this.ToReference();
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x000162A8 File Offset: 0x000152A8
		public override bool Equals(object obj)
		{
			SpecializedMethod specializedMethod = obj as SpecializedMethod;
			return specializedMethod != null && this.baseMember.Equals(specializedMethod.baseMember) && this.substitutionWithoutSpecializedTypeParameters.Equals(specializedMethod.substitutionWithoutSpecializedTypeParameters);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x000162E7 File Offset: 0x000152E7
		public override int GetHashCode()
		{
			return 1000000013 * this.baseMember.GetHashCode() + 1000000009 * this.substitutionWithoutSpecializedTypeParameters.GetHashCode();
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0001630C File Offset: 0x0001530C
		public override IMember Specialize(TypeParameterSubstitution newSubstitution)
		{
			return this.methodDefinition.Specialize(TypeParameterSubstitution.Compose(newSubstitution, this.substitutionWithoutSpecializedTypeParameters));
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00016325 File Offset: 0x00015325
		IMethod IMethod.Specialize(TypeParameterSubstitution newSubstitution)
		{
			return this.methodDefinition.Specialize(TypeParameterSubstitution.Compose(newSubstitution, this.substitutionWithoutSpecializedTypeParameters));
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00016340 File Offset: 0x00015340
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("[");
			stringBuilder.Append(base.GetType().Name);
			stringBuilder.Append(' ');
			stringBuilder.Append(base.DeclaringType.ReflectionName);
			stringBuilder.Append('.');
			stringBuilder.Append(base.Name);
			if (this.TypeArguments.Count > 0)
			{
				stringBuilder.Append('[');
				for (int i = 0; i < this.TypeArguments.Count; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.TypeArguments[i].ReflectionName);
				}
				stringBuilder.Append(']');
			}
			else if (this.TypeParameters.Count > 0)
			{
				stringBuilder.Append("``");
				stringBuilder.Append(this.TypeParameters.Count);
			}
			stringBuilder.Append('(');
			for (int j = 0; j < base.Parameters.Count; j++)
			{
				if (j > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(base.Parameters[j].ToString());
			}
			stringBuilder.Append("):");
			stringBuilder.Append(base.ReturnType.ReflectionName);
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x04000265 RID: 613
		private readonly IMethod methodDefinition;

		// Token: 0x04000266 RID: 614
		private readonly ITypeParameter[] specializedTypeParameters;

		// Token: 0x04000267 RID: 615
		private readonly bool isParameterized;

		// Token: 0x04000268 RID: 616
		private readonly TypeParameterSubstitution substitutionWithoutSpecializedTypeParameters;

		// Token: 0x04000269 RID: 617
		private IMember accessorOwner;

		// Token: 0x020000E2 RID: 226
		private sealed class SpecializedTypeParameter : AbstractTypeParameter
		{
			// Token: 0x06000879 RID: 2169 RVA: 0x0001649B File Offset: 0x0001549B
			public SpecializedTypeParameter(ITypeParameter baseTp, IMethod specializedOwner) : base(specializedOwner, baseTp.Index, baseTp.Name, baseTp.Variance, baseTp.Attributes, baseTp.Region)
			{
				this.baseTp = baseTp;
			}

			// Token: 0x0600087A RID: 2170 RVA: 0x000164C9 File Offset: 0x000154C9
			public override int GetHashCode()
			{
				return this.baseTp.GetHashCode() ^ base.Owner.GetHashCode();
			}

			// Token: 0x0600087B RID: 2171 RVA: 0x000164E4 File Offset: 0x000154E4
			public override bool Equals(IType other)
			{
				SpecializedMethod.SpecializedTypeParameter specializedTypeParameter = other as SpecializedMethod.SpecializedTypeParameter;
				return specializedTypeParameter != null && this.baseTp.Equals(specializedTypeParameter.baseTp) && base.Owner.Equals(specializedTypeParameter.Owner);
			}

			// Token: 0x17000393 RID: 915
			// (get) Token: 0x0600087C RID: 2172 RVA: 0x00016521 File Offset: 0x00015521
			public override bool HasValueTypeConstraint
			{
				get
				{
					return this.baseTp.HasValueTypeConstraint;
				}
			}

			// Token: 0x17000394 RID: 916
			// (get) Token: 0x0600087D RID: 2173 RVA: 0x0001652E File Offset: 0x0001552E
			public override bool HasReferenceTypeConstraint
			{
				get
				{
					return this.baseTp.HasReferenceTypeConstraint;
				}
			}

			// Token: 0x17000395 RID: 917
			// (get) Token: 0x0600087E RID: 2174 RVA: 0x0001653B File Offset: 0x0001553B
			public override bool HasDefaultConstructorConstraint
			{
				get
				{
					return this.baseTp.HasDefaultConstructorConstraint;
				}
			}

			// Token: 0x17000396 RID: 918
			// (get) Token: 0x0600087F RID: 2175 RVA: 0x00016556 File Offset: 0x00015556
			public override IEnumerable<IType> DirectBaseTypes
			{
				get
				{
					return from t in this.baseTp.DirectBaseTypes
					select t.AcceptVisitor(this.substitution);
				}
			}

			// Token: 0x0400026A RID: 618
			private readonly ITypeParameter baseTp;

			// Token: 0x0400026B RID: 619
			internal TypeVisitor substitution;
		}
	}
}
