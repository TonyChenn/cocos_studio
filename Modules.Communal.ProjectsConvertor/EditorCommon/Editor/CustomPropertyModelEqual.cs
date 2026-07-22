using System;
using System.Collections.Generic;

namespace EditorCommon.Editor
{
	// Token: 0x02000019 RID: 25
	public class CustomPropertyModelEqual : IEqualityComparer<CustomPropertyModel>
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00004B76 File Offset: 0x00002D76
		public bool Equals(CustomPropertyModel x, CustomPropertyModel y)
		{
			return x.Name == y.Name;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004B89 File Offset: 0x00002D89
		public int GetHashCode(CustomPropertyModel obj)
		{
			return obj.Name.GetHashCode();
		}
	}
}
