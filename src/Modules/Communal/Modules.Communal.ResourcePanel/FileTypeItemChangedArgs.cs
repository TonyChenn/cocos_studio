using System;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200002E RID: 46
	internal class FileTypeItemChangedArgs : EventArgs
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00009E2C File Offset: 0x0000802C
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00009E34 File Offset: 0x00008034
		public FileTypeItem Item { get; private set; }

		// Token: 0x060001C4 RID: 452 RVA: 0x00009E3D File Offset: 0x0000803D
		public FileTypeItemChangedArgs(FileTypeItem item)
		{
			this.Item = item;
		}
	}
}
