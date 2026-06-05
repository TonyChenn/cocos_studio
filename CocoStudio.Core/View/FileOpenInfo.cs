using System;
using System.Text;
using CocoStudio.Projects;
using MonoDevelop.Core;

namespace CocoStudio.Core.View
{
	// Token: 0x02000044 RID: 68
	public class FileOpenInfo
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000A424 File Offset: 0x00008624
		// (set) Token: 0x0600023D RID: 573 RVA: 0x0000A43B File Offset: 0x0000863B
		public int Line { get; set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000A444 File Offset: 0x00008644
		// (set) Token: 0x0600023F RID: 575 RVA: 0x0000A45B File Offset: 0x0000865B
		public int Column { get; set; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000A464 File Offset: 0x00008664
		// (set) Token: 0x06000241 RID: 577 RVA: 0x0000A47B File Offset: 0x0000867B
		public ResourceFile Project { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000A484 File Offset: 0x00008684
		// (set) Token: 0x06000243 RID: 579 RVA: 0x0000A49B File Offset: 0x0000869B
		public IViewContentExtend NewContent { get; set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000A4A4 File Offset: 0x000086A4
		// (set) Token: 0x06000245 RID: 581 RVA: 0x0000A4BB File Offset: 0x000086BB
		public FilePath FileName { get; set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000A4C4 File Offset: 0x000086C4
		// (set) Token: 0x06000247 RID: 583 RVA: 0x0000A4DB File Offset: 0x000086DB
		public bool BringToFront { get; set; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000A4E4 File Offset: 0x000086E4
		// (set) Token: 0x06000249 RID: 585 RVA: 0x0000A4FB File Offset: 0x000086FB
		public IViewDisplayBuilder DisplayBuilder { get; set; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000A504 File Offset: 0x00008704
		// (set) Token: 0x0600024B RID: 587 RVA: 0x0000A51B File Offset: 0x0000871B
		public Encoding Encoding { get; set; }

		// Token: 0x0600024C RID: 588 RVA: 0x0000A524 File Offset: 0x00008724
		public FileOpenInfo(FilePath file, ResourceFile project, bool bringToFront)
		{
			this.FileName = file;
			this.Project = project;
			this.BringToFront = bringToFront;
		}
	}
}
