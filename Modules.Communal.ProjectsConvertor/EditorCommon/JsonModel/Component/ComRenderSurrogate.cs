using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	// Token: 0x0200000E RID: 14
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ComRenderSurrogate : BaseComSurrogate
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000045AC File Offset: 0x000027AC
		// (set) Token: 0x06000086 RID: 134 RVA: 0x000045B4 File Offset: 0x000027B4
		[DataMember]
		public string file { get; protected set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000087 RID: 135 RVA: 0x000045BD File Offset: 0x000027BD
		// (set) Token: 0x06000088 RID: 136 RVA: 0x000045C5 File Offset: 0x000027C5
		[DataMember]
		public ResourceDataSurrogate fileData { get; protected set; }

		// Token: 0x0600008A RID: 138 RVA: 0x000045D6 File Offset: 0x000027D6
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
		}
	}
}
