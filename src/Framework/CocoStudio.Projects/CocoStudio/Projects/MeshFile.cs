using System;
using CocoStudio.Model;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000055 RID: 85
	[DataItem(Name = "Mesh")]
	public class MeshFile : CompositeResourceFile
	{
		// Token: 0x06000255 RID: 597 RVA: 0x0000922D File Offset: 0x0000742D
		private MeshFile()
		{
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00009235 File Offset: 0x00007435
		public MeshFile(FilePath fileName) : base(fileName)
		{
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000923E File Offset: 0x0000743E
		public MeshFile(ResourceData resourceData) : base(resourceData)
		{
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00009247 File Offset: 0x00007447
		internal override string PreviewImagePath
		{
			get
			{
				return this.FullPath;
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000924F File Offset: 0x0000744F
		protected override ResourceData CreateResourceData(FilePath filePath)
		{
			return base.CreateResourceData(filePath);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00009258 File Offset: 0x00007458
		protected override void OnRefresh()
		{
			base.OnRefresh();
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00009260 File Offset: 0x00007460
		protected override void OnDelete(IProgressMonitor monitor)
		{
			base.OnDelete(monitor);
		}
	}
}
