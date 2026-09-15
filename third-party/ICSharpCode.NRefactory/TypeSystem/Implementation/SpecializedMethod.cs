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
	public class SpecializedMethod : SpecializedParameterizedMember, IMethod, IParameterizedMember, IMember, IEntity, ISymbol, ICompilationProvider, INamedElement, IHasAccessibility
	{
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

		public IList<IType> TypeArguments
		{
			get
			{
				return base.Substitution.MethodTypeArguments ?? EmptyList<IType>.Instance;
			}
		}

		public bool IsParameterized
		{
			get
			{
				return this.isParameterized;
			}
		}

		public IList<IUnresolvedMethod> Parts
		{
			get
			{
				return this.methodDefinition.Parts;
			}
		}

		public IList<IAttribute> ReturnTypeAttributes
		{
			get
			{
				return this.methodDefinition.ReturnTypeAttributes;
			}
		}

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

		public bool IsExtensionMethod
		{
			get
			{
				return this.methodDefinition.IsExtensionMethod;
			}
		}

		public bool IsConstructor
		{
			get
			{
				return this.methodDefinition.IsConstructor;
			}
		}

		public bool IsDestructor
		{
			get
			{
				return this.methodDefinition.IsDestructor;
			}
		}

		public bool IsOperator
		{
			get
			{
				return this.methodDefinition.IsOperator;
			}
		}

		public bool IsPartial
		{
			get
			{
				return this.methodDefinition.IsPartial;
			}
		}

		public bool IsAsync
		{
			get
			{
				return this.methodDefinition.IsAsync;
			}
		}

		public bool HasBody
		{
			get
			{
				return this.methodDefinition.HasBody;
			}
		}

		public bool IsAccessor
		{
			get
			{
				return this.methodDefinition.IsAccessor;
			}
		}

		public IMethod ReducedFrom
		{
			get
			{
				return null;
			}
		}

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

		public override IMemberReference ToReference()
		{
			if (this.isParameterized)
			{
				return new SpecializingMemberReference(this.baseMember.ToReference(), SpecializedMember.ToTypeReference(base.Substitution.ClassTypeArguments), SpecializedMember.ToTypeReference(base.Substitution.MethodTypeArguments));
			}
			return base.ToReference();
		}

		public override IMemberReference ToMemberReference()
		{
			return this.ToReference();
		}

		public override bool Equals(object obj)
		{
			SpecializedMethod specializedMethod = obj as SpecializedMethod;
			return specializedMethod != null && this.baseMember.Equals(specializedMethod.baseMember) && this.substitutionWithoutSpecializedTypeParameters.Equals(specializedMethod.substitutionWithoutSpecializedTypeParameters);
		}

		public override int GetHashCode()
		{
			return 1000000013 * this.baseMember.GetHashCode() + 1000000009 * this.substitutionWithoutSpecializedTypeParameters.GetHashCode();
		}

		public override IMember Specialize(TypeParameterSubstitution newSubstitution)
		{
			return this.methodDefinition.Specialize(TypeParameterSubstitution.Compose(newSubstitution, this.substitutionWithoutSpecializedTypeParameters));
		}

		IMethod IMethod.Specialize(TypeParameterSubstitution newSubstitution)
		{
			return this.methodDefinition.Specialize(TypeParameterSubstitution.Compose(newSubstitution, this.substitutionWithoutSpecializedTypeParameters));
		}

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

		private readonly IMethod methodDefinition;

		private readonly ITypeParameter[] specializedTypeParameters;

		private readonly bool isParameterized;

		private readonly TypeParameterSubstitution substitutionWithoutSpecializedTypeParameters;

		private IMember accessorOwner;

		private sealed class SpecializedTypeParameter : AbstractTypeParameter
		{
			public SpecializedTypeParameter(ITypeParameter baseTp, IMethod specializedOwner) : base(specializedOwner, baseTp.Index, baseTp.Name, baseTp.Variance, baseTp.Attributes, baseTp.Region)
			{
				this.baseTp = baseTp;
			}

			public override int GetHashCode()
			{
				return this.baseTp.GetHashCode() ^ base.Owner.GetHashCode();
			}

			public override bool Equals(IType other)
			{
				SpecializedMethod.SpecializedTypeParameter specializedTypeParameter = other as SpecializedMethod.SpecializedTypeParameter;
				return specializedTypeParameter != null && this.baseTp.Equals(specializedTypeParameter.baseTp) && base.Owner.Equals(specializedTypeParameter.Owner);
			}

			public override bool HasValueTypeConstraint
			{
				get
				{
					return this.baseTp.HasValueTypeConstraint;
				}
			}

			public override bool HasReferenceTypeConstraint
			{
				get
				{
					return this.baseTp.HasReferenceTypeConstraint;
				}
			}

			public override bool HasDefaultConstructorConstraint
			{
				get
				{
					return this.baseTp.HasDefaultConstructorConstraint;
				}
			}

			public override IEnumerable<IType> DirectBaseTypes
			{
				get
				{
					return from t in this.baseTp.DirectBaseTypes
					select t.AcceptVisitor(this.substitution);
				}
			}

			private readonly ITypeParameter baseTp;

			internal TypeVisitor substitution;
		}
	}
}
