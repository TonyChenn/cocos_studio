using System;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000092 RID: 146
	[JsonObject(MemberSerialization.OptIn)]
	[DataModelExtension]
	public sealed class SizeValue
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x00015BC4 File Offset: 0x00013DC4
		// (set) Token: 0x06000505 RID: 1285 RVA: 0x00015BDB File Offset: 0x00013DDB
		[ItemProperty]
		[JsonProperty]
		public int Width { get; set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x00015BE4 File Offset: 0x00013DE4
		// (set) Token: 0x06000507 RID: 1287 RVA: 0x00015BFB File Offset: 0x00013DFB
		[ItemProperty]
		[JsonProperty]
		public int Height { get; set; }

		// Token: 0x06000508 RID: 1288 RVA: 0x00015C04 File Offset: 0x00013E04
		public SizeValue()
		{
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00015C0F File Offset: 0x00013E0F
		public SizeValue(int width, int height)
		{
			this.Width = width;
			this.Height = height;
		}
	}
}
