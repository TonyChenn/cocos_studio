using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x020000FB RID: 251
	[Serializable]
	public sealed class TopLevelTypeNameComparer : IEqualityComparer<TopLevelTypeName>
	{
		// Token: 0x06000943 RID: 2371 RVA: 0x00018C60 File Offset: 0x00017C60
		public TopLevelTypeNameComparer(StringComparer nameComparer)
		{
			this.NameComparer = nameComparer;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00018C70 File Offset: 0x00017C70
		public bool Equals(TopLevelTypeName x, TopLevelTypeName y)
		{
			return x.TypeParameterCount == y.TypeParameterCount && this.NameComparer.Equals(x.Name, y.Name) && this.NameComparer.Equals(x.Namespace, y.Namespace);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00018CC3 File Offset: 0x00017CC3
		public int GetHashCode(TopLevelTypeName obj)
		{
			return this.NameComparer.GetHashCode(obj.Name) ^ this.NameComparer.GetHashCode(obj.Namespace) ^ obj.TypeParameterCount;
		}

		// Token: 0x040002F4 RID: 756
		public static readonly TopLevelTypeNameComparer Ordinal = new TopLevelTypeNameComparer(StringComparer.Ordinal);

		// Token: 0x040002F5 RID: 757
		public static readonly TopLevelTypeNameComparer OrdinalIgnoreCase = new TopLevelTypeNameComparer(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040002F6 RID: 758
		public readonly StringComparer NameComparer;
	}
}
