using System;

namespace CocoStudio.Model
{
	// Token: 0x02000006 RID: 6
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	internal sealed class FramePropertyAttribute : Attribute
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000216C File Offset: 0x0000036C
		// (set) Token: 0x0600000E RID: 14 RVA: 0x00002183 File Offset: 0x00000383
		public Type FrameType { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000218C File Offset: 0x0000038C
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000021A3 File Offset: 0x000003A3
		public bool IsAutoCreate { get; private set; }

		// Token: 0x06000011 RID: 17 RVA: 0x000021AC File Offset: 0x000003AC
		public FramePropertyAttribute()
		{
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000021B7 File Offset: 0x000003B7
		public FramePropertyAttribute(bool isAutoCreate)
		{
			this.IsAutoCreate = isAutoCreate;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000021CA File Offset: 0x000003CA
		public FramePropertyAttribute(Type frameType) : this()
		{
			this.FrameType = frameType;
		}
	}
}
