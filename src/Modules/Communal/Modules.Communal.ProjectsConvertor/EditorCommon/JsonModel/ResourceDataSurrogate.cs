using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	// Token: 0x02000035 RID: 53
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ResourceDataSurrogate : ObjectSurrogate
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000379 RID: 889 RVA: 0x000091DC File Offset: 0x000073DC
		// (set) Token: 0x0600037A RID: 890 RVA: 0x000091E4 File Offset: 0x000073E4
		[DefaultValue(-1)]
		[DataMember]
		public int resourceType { get; private set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600037B RID: 891 RVA: 0x000091ED File Offset: 0x000073ED
		// (set) Token: 0x0600037C RID: 892 RVA: 0x000091F5 File Offset: 0x000073F5
		[DataMember]
		public string path { get; private set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600037D RID: 893 RVA: 0x000091FE File Offset: 0x000073FE
		// (set) Token: 0x0600037E RID: 894 RVA: 0x00009206 File Offset: 0x00007406
		[DataMember]
		public string plistFile { get; private set; }

		// Token: 0x0600037F RID: 895 RVA: 0x0000920F File Offset: 0x0000740F
		protected ResourceDataSurrogate()
		{
		}
	}
}
