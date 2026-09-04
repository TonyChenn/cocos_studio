using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x0200008A RID: 138
	[Serializable]
	public sealed class FullTypeNameComparer : IEqualityComparer<FullTypeName>
	{
		// Token: 0x0600047D RID: 1149 RVA: 0x0000BD25 File Offset: 0x0000AD25
		public FullTypeNameComparer(StringComparer nameComparer)
		{
			this.NameComparer = nameComparer;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000BD34 File Offset: 0x0000AD34
		public bool Equals(FullTypeName x, FullTypeName y)
		{
			if (x.NestingLevel != y.NestingLevel)
			{
				return false;
			}
			TopLevelTypeName topLevelTypeName = x.TopLevelTypeName;
			TopLevelTypeName topLevelTypeName2 = y.TopLevelTypeName;
			if (topLevelTypeName.TypeParameterCount == topLevelTypeName2.TypeParameterCount && this.NameComparer.Equals(topLevelTypeName.Name, topLevelTypeName2.Name) && this.NameComparer.Equals(topLevelTypeName.Namespace, topLevelTypeName2.Namespace))
			{
				for (int i = 0; i < x.NestingLevel; i++)
				{
					if (x.GetNestedTypeAdditionalTypeParameterCount(i) != y.GetNestedTypeAdditionalTypeParameterCount(i))
					{
						return false;
					}
					if (!this.NameComparer.Equals(x.GetNestedTypeName(i), y.GetNestedTypeName(i)))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000BDF4 File Offset: 0x0000ADF4
		public int GetHashCode(FullTypeName obj)
		{
			TopLevelTypeName topLevelTypeName = obj.TopLevelTypeName;
			int num = this.NameComparer.GetHashCode(topLevelTypeName.Name) ^ this.NameComparer.GetHashCode(topLevelTypeName.Namespace) ^ topLevelTypeName.TypeParameterCount;
			for (int i = 0; i < obj.NestingLevel; i++)
			{
				num *= 31;
				num += (this.NameComparer.GetHashCode(obj.Name) ^ obj.TypeParameterCount);
			}
			return num;
		}

		// Token: 0x0400013D RID: 317
		public static readonly FullTypeNameComparer Ordinal = new FullTypeNameComparer(StringComparer.Ordinal);

		// Token: 0x0400013E RID: 318
		public static readonly FullTypeNameComparer OrdinalIgnoreCase = new FullTypeNameComparer(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400013F RID: 319
		public readonly StringComparer NameComparer;
	}
}
