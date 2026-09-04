using System;
using System.Runtime.Serialization;
using Gdk;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	// Token: 0x02000034 RID: 52
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class RectSurrogate : BaseEntitySurrogate
	{
		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00009150 File Offset: 0x00007350
		// (set) Token: 0x06000370 RID: 880 RVA: 0x00009158 File Offset: 0x00007358
		[DataMember]
		public double x { get; set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00009161 File Offset: 0x00007361
		// (set) Token: 0x06000372 RID: 882 RVA: 0x00009169 File Offset: 0x00007369
		[DataMember]
		public double y { get; set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00009172 File Offset: 0x00007372
		// (set) Token: 0x06000374 RID: 884 RVA: 0x0000917A File Offset: 0x0000737A
		[DataMember]
		public double w { get; set; }

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00009183 File Offset: 0x00007383
		// (set) Token: 0x06000376 RID: 886 RVA: 0x0000918B File Offset: 0x0000738B
		[DataMember]
		public double h { get; set; }

		// Token: 0x06000377 RID: 887 RVA: 0x00009194 File Offset: 0x00007394
		protected RectSurrogate()
		{
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000919C File Offset: 0x0000739C
		public RectSurrogate(Rectangle rect)
		{
			this.x = (double)rect.X;
			this.y = (double)rect.Y;
			this.w = (double)rect.Width;
			this.h = (double)rect.Height;
		}
	}
}
