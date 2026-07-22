using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	// Token: 0x02000015 RID: 21
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ComSceneSurrogate : BaseComSurrogate
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00004948 File Offset: 0x00002B48
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00004950 File Offset: 0x00002B50
		[DataMember]
		public string scenename { get; set; }

		// Token: 0x060000A5 RID: 165 RVA: 0x00004959 File Offset: 0x00002B59
		protected ComSceneSurrogate()
		{
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004961 File Offset: 0x00002B61
		public ComSceneSurrogate(string scenename)
		{
			this.scenename = scenename;
			this.classname = "CCScene";
			this.name = this.classname;
		}
	}
}
