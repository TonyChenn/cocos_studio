using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	// Token: 0x0200000D RID: 13
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class BaseComSurrogate : BaseEntitySurrogate
	{
		// Token: 0x06000084 RID: 132 RVA: 0x000045A4 File Offset: 0x000027A4
		protected BaseComSurrogate()
		{
		}
	}
}
