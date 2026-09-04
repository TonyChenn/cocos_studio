using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	// Token: 0x02000013 RID: 19
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ComPrimitiveSurrogate : BaseComSurrogate
	{
		// Token: 0x0600009B RID: 155 RVA: 0x000048E0 File Offset: 0x00002AE0
		public ComPrimitiveSurrogate()
		{
			this.classname = "PrimitiveComponent";
		}
	}
}
