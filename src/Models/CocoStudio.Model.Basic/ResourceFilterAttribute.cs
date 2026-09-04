using System;

namespace CocoStudio.Model
{
	// Token: 0x02000014 RID: 20
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class ResourceFilterAttribute : Attribute
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00003044 File Offset: 0x00001244
		// (set) Token: 0x06000089 RID: 137 RVA: 0x0000305B File Offset: 0x0000125B
		public string[] FileFilter { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003064 File Offset: 0x00001264
		// (set) Token: 0x0600008B RID: 139 RVA: 0x0000307B File Offset: 0x0000127B
		public EnumResourceType[] ResourceTypeFilter { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00003084 File Offset: 0x00001284
		// (set) Token: 0x0600008D RID: 141 RVA: 0x0000309B File Offset: 0x0000129B
		public bool DefaultFileMarker { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000030A4 File Offset: 0x000012A4
		// (set) Token: 0x0600008F RID: 143 RVA: 0x000030BB File Offset: 0x000012BB
		public bool CanReset { get; set; }

		// Token: 0x06000090 RID: 144 RVA: 0x000030C4 File Offset: 0x000012C4
		public ResourceFilterAttribute(params string[] fileFilter)
		{
			this.FileFilter = fileFilter;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000030D7 File Offset: 0x000012D7
		public ResourceFilterAttribute(bool defaultFileMarker = false, bool canReset = false, params string[] fileFilter)
		{
			this.FileFilter = fileFilter;
			this.DefaultFileMarker = defaultFileMarker;
			this.CanReset = canReset;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000030FC File Offset: 0x000012FC
		public ResourceFilterAttribute(EnumResourceType resouceType, params string[] fileFilter) : this(fileFilter)
		{
			this.ResourceTypeFilter = new EnumResourceType[]
			{
				resouceType
			};
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003126 File Offset: 0x00001326
		public ResourceFilterAttribute(EnumResourceType[] resoureTypeFilter, params string[] fileFilter) : this(fileFilter)
		{
			this.ResourceTypeFilter = resoureTypeFilter;
		}
	}
}
