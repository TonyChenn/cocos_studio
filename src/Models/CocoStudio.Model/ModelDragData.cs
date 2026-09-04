using System;

namespace CocoStudio.Model
{
	// Token: 0x020000BF RID: 191
	[Serializable]
	public class ModelDragData
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x00019558 File Offset: 0x00017758
		// (set) Token: 0x0600060E RID: 1550 RVA: 0x0001956F File Offset: 0x0001776F
		public ModelMetaData MetaData { get; private set; }

		// Token: 0x0600060F RID: 1551 RVA: 0x00019578 File Offset: 0x00017778
		public ModelDragData(ModelMetaData MetaData)
		{
			this.MetaData = MetaData;
		}
	}
}
