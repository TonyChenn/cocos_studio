using System;

namespace Cocos.Launcher.Core
{
	// Token: 0x0200003B RID: 59
	public class UpdateIdentify
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00009158 File Offset: 0x00007358
		// (set) Token: 0x06000205 RID: 517 RVA: 0x00009160 File Offset: 0x00007360
		public string TutorialTime { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00009169 File Offset: 0x00007369
		// (set) Token: 0x06000207 RID: 519 RVA: 0x00009171 File Offset: 0x00007371
		public string ToolsTime { get; set; }

		// Token: 0x06000208 RID: 520 RVA: 0x0000917A File Offset: 0x0000737A
		public UpdateIdentify()
		{
			this.InitDefaulValue();
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00009188 File Offset: 0x00007388
		private void InitDefaulValue()
		{
			this.TutorialTime = (this.ToolsTime = string.Empty);
		}
	}
}
