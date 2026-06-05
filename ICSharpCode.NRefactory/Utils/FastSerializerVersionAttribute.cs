using System;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Specifies the version of the class.
	/// The <see cref="T:ICSharpCode.NRefactory.Utils.FastSerializer" /> will refuse to deserialize an instance that was stored by
	/// a different version of the class than the current one.
	/// </summary>
	// Token: 0x02000118 RID: 280
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
	public class FastSerializerVersionAttribute : Attribute
	{
		// Token: 0x060009FD RID: 2557 RVA: 0x0001DB8D File Offset: 0x0001CB8D
		public FastSerializerVersionAttribute(int versionNumber)
		{
			this.versionNumber = versionNumber;
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x0001DB9C File Offset: 0x0001CB9C
		public int VersionNumber
		{
			get
			{
				return this.versionNumber;
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0001DBA4 File Offset: 0x0001CBA4
		internal static int GetVersionNumber(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(FastSerializerVersionAttribute), false);
			if (customAttributes.Length == 0)
			{
				return 0;
			}
			return ((FastSerializerVersionAttribute)customAttributes[0]).VersionNumber;
		}

		// Token: 0x0400035E RID: 862
		private readonly int versionNumber;
	}
}
