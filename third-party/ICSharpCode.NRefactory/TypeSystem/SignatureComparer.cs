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
	// Token: 0x020000F6 RID: 246
	public sealed class SignatureComparer : IEqualityComparer<IMember>
	{
		// Token: 0x06000920 RID: 2336 RVA: 0x00018794 File Offset: 0x00017794
		public SignatureComparer(StringComparer nameComparer)
		{
			if (nameComparer == null)
			{
				throw new ArgumentNullException("nameComparer");
			}
			this.nameComparer = nameComparer;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x000187B4 File Offset: 0x000177B4
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

		// Token: 0x06000922 RID: 2338 RVA: 0x00018850 File Offset: 0x00017850
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

		// Token: 0x040002E9 RID: 745
		private StringComparer nameComparer;

		/// <summary>
		/// Gets a signature comparer that uses an ordinal comparison for the member name.
		/// </summary>
		// Token: 0x040002EA RID: 746
		public static readonly SignatureComparer Ordinal = new SignatureComparer(StringComparer.Ordinal);
	}
}
