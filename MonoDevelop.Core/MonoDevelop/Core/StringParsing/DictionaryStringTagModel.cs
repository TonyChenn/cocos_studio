using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.StringParsing
{
	// Token: 0x02000207 RID: 519
	internal class DictionaryStringTagModel<T> : IStringTagModel
	{
		// Token: 0x060013B0 RID: 5040 RVA: 0x00051832 File Offset: 0x0004FA32
		public DictionaryStringTagModel(Dictionary<string, T> dict)
		{
			this.dict = new Dictionary<string, T>(dict, StringComparer.InvariantCultureIgnoreCase);
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x0005184C File Offset: 0x0004FA4C
		public object GetValue(string name)
		{
			T t;
			if (this.dict.TryGetValue(name, out t))
			{
				return t;
			}
			return null;
		}

		// Token: 0x040005D2 RID: 1490
		private Dictionary<string, T> dict;
	}
}
