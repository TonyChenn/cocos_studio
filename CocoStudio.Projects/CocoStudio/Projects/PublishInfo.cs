using System;

namespace CocoStudio.Projects
{
	// Token: 0x0200005F RID: 95
	public class PublishInfo
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060002CC RID: 716 RVA: 0x0000B014 File Offset: 0x00009214
		// (set) Token: 0x060002CD RID: 717 RVA: 0x0000B01C File Offset: 0x0000921C
		public virtual string PublishDirectory { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0000B025 File Offset: 0x00009225
		// (set) Token: 0x060002CF RID: 719 RVA: 0x0000B02D File Offset: 0x0000922D
		public string SourceFilePath { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000B036 File Offset: 0x00009236
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x0000B03E File Offset: 0x0000923E
		public string DestinationFilePath { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000B047 File Offset: 0x00009247
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x0000B04F File Offset: 0x0000924F
		public PublishType PublishType { get; set; }

		// Token: 0x060002D4 RID: 724 RVA: 0x0000B058 File Offset: 0x00009258
		public PublishInfo()
		{
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000B060 File Offset: 0x00009260
		public PublishInfo(string publishDirectory, PublishType publishType = PublishType.Reference)
		{
			this.PublishDirectory = publishDirectory;
			this.PublishType = publishType;
		}
	}
}
