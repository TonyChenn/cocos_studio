using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Compares member signatures.
	/// </summary>
	/// <remarks>
	/// This comparer checks for equal short name, equal type parameter count, and equal parameter types (using ParameterListComparer).
	/// </remarks>
	public sealed class SignatureComparer : IEqualityComparer<IMember>
	{
		public SignatureComparer(StringComparer nameComparer)
		{
			if (nameComparer == null)
			{
				throw new ArgumentNullException("nameComparer");
			}
			this.nameComparer = nameComparer;
		}

		public bool Equals(IMember x, IMember y)
		{
			if (x == y)
			{
				return true;
			}
			if (x == null || y == null || x.SymbolKind != y.SymbolKind || !this.nameComparer.Equals(x.Name, y.Name))
			{
				return false;
			}
			IParameterizedMember parameterizedMember = x as IParameterizedMember;
			IParameterizedMember parameterizedMember2 = y as IParameterizedMember;
			if (parameterizedMember != null && parameterizedMember2 != null)
			{
				IMethod method = x as IMethod;
				IMethod method2 = y as IMethod;
				return (method == null || method2 == null || method.TypeParameters.Count == method2.TypeParameters.Count) && ParameterListComparer.Instance.Equals(parameterizedMember.Parameters, parameterizedMember2.Parameters);
			}
			return true;
		}

		public int GetHashCode(IMember obj)
		{
			int num = (int)obj.SymbolKind * 33 + this.nameComparer.GetHashCode(obj.Name);
			IParameterizedMember parameterizedMember = obj as IParameterizedMember;
			if (parameterizedMember != null)
			{
				num *= 27;
				num += ParameterListComparer.Instance.GetHashCode(parameterizedMember.Parameters);
				IMethod method = parameterizedMember as IMethod;
				if (method != null)
				{
					num += method.TypeParameters.Count;
				}
			}
			return num;
		}

		private StringComparer nameComparer;

		/// <summary>
		/// Gets a signature comparer that uses an ordinal comparison for the member name.
		/// </summary>
		public static readonly SignatureComparer Ordinal = new SignatureComparer(StringComparer.Ordinal);
	}
}
