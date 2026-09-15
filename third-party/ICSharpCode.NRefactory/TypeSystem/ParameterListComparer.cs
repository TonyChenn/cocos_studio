using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Compares parameter lists by comparing the types of all parameters.
	/// </summary>
	/// <remarks>
	/// 'ref int' and 'out int' are considered to be equal.
	/// 'object' and 'dynamic' are also equal.
	/// For generic methods, "Method{T}(T a)" and "Method{S}(S b)" are considered equal.
	/// However, "Method(T a)" and "Method(S b)" are not considered equal when the type parameters T and S belong to classes.
	/// </remarks>
	public sealed class ParameterListComparer : IEqualityComparer<IList<IParameter>>
	{
		/// <summary>
		/// Replaces all occurrences of method type parameters in the given type
		/// by normalized type parameters. This allows comparing parameter types from different
		/// generic methods.
		/// </summary>
		[Obsolete("Use DummyTypeParameter.NormalizeMethodTypeParameters instead if you only need to normalize type parameters. Also, consider if you need to normalize object vs. dynamic as well.")]
		public IType NormalizeMethodTypeParameters(IType type)
		{
			return DummyTypeParameter.NormalizeMethodTypeParameters(type);
		}

		public bool Equals(IList<IParameter> x, IList<IParameter> y)
		{
			if (x == y)
			{
				return true;
			}
			if (x == null || y == null || x.Count != y.Count)
			{
				return false;
			}
			for (int i = 0; i < x.Count; i++)
			{
				IParameter parameter = x[i];
				IParameter parameter2 = y[i];
				if (parameter != null || parameter2 != null)
				{
					if (parameter == null || parameter2 == null)
					{
						return false;
					}
					IType type = parameter.Type.AcceptVisitor(ParameterListComparer.normalizationVisitor);
					IType other = parameter2.Type.AcceptVisitor(ParameterListComparer.normalizationVisitor);
					if (!type.Equals(other))
					{
						return false;
					}
				}
			}
			return true;
		}

		public int GetHashCode(IList<IParameter> obj)
		{
			int num = obj.Count;
			foreach (IParameter parameter in obj)
			{
				num *= 27;
				IType type = parameter.Type.AcceptVisitor(ParameterListComparer.normalizationVisitor);
				num += type.GetHashCode();
			}
			return num;
		}

		public static readonly ParameterListComparer Instance = new ParameterListComparer();

		private static readonly ParameterListComparer.NormalizeTypeVisitor normalizationVisitor = new ParameterListComparer.NormalizeTypeVisitor();

		private sealed class NormalizeTypeVisitor : TypeVisitor
		{
			public override IType VisitTypeParameter(ITypeParameter type)
			{
				if (type.OwnerType == SymbolKind.Method)
				{
					return DummyTypeParameter.GetMethodTypeParameter(type.Index);
				}
				return base.VisitTypeParameter(type);
			}

			public override IType VisitTypeDefinition(ITypeDefinition type)
			{
				if (type.KnownTypeCode == KnownTypeCode.Object)
				{
					return SpecialType.Dynamic;
				}
				return base.VisitTypeDefinition(type);
			}
		}
	}
}
