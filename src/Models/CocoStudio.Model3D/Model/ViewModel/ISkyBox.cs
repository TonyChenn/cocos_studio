using System;
using System.Collections.Generic;
using CocoStudio.Projects;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000022 RID: 34
	public interface ISkyBox
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000128 RID: 296
		// (set) Token: 0x06000129 RID: 297
		bool SkyBoxEnabled { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600012A RID: 298
		// (set) Token: 0x0600012B RID: 299
		string SkyboxResourceError { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600012C RID: 300
		// (set) Token: 0x0600012D RID: 301
		ResourceFile UpImage { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600012E RID: 302
		// (set) Token: 0x0600012F RID: 303
		ResourceFile DownImage { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000130 RID: 304
		// (set) Token: 0x06000131 RID: 305
		ResourceFile BackImage { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000132 RID: 306
		// (set) Token: 0x06000133 RID: 307
		ResourceFile ForwardImage { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000134 RID: 308
		// (set) Token: 0x06000135 RID: 309
		ResourceFile LeftImage { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000136 RID: 310
		// (set) Token: 0x06000137 RID: 311
		ResourceFile RightImage { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000138 RID: 312
		// (set) Token: 0x06000139 RID: 313
		List<string> ResourceValue { get; set; }

		// Token: 0x0600013A RID: 314
		void ResetSkyBox();

		// Token: 0x0600013B RID: 315
		void RefreshSkyBox();
	}
}
