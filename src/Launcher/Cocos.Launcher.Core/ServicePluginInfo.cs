using System;
using System.Runtime.Serialization;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000033 RID: 51
	[DataContract]
	public class ServicePluginInfo
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00008B3D File Offset: 0x00006D3D
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x00008B34 File Offset: 0x00006D34
		[DataMember]
		public string Name { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00008B4E File Offset: 0x00006D4E
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x00008B45 File Offset: 0x00006D45
		[DataMember]
		public string Type { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00008B5F File Offset: 0x00006D5F
		// (set) Token: 0x060001CB RID: 459 RVA: 0x00008B56 File Offset: 0x00006D56
		[DataMember]
		public string Version { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00008B70 File Offset: 0x00006D70
		// (set) Token: 0x060001CD RID: 461 RVA: 0x00008B67 File Offset: 0x00006D67
		[DataMember]
		public string DownloadUrl { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x00008B81 File Offset: 0x00006D81
		// (set) Token: 0x060001CF RID: 463 RVA: 0x00008B78 File Offset: 0x00006D78
		[DataMember]
		public bool IsInstalled { get; set; }

		// Token: 0x060001D1 RID: 465 RVA: 0x00008B89 File Offset: 0x00006D89
		public ServicePluginInfo(string name = null, string type = null, string version = null, bool isInstalled = false)
		{
			this.Name = name;
			this.Type = type;
			this.Version = version;
			this.IsInstalled = isInstalled;
		}
	}
}
