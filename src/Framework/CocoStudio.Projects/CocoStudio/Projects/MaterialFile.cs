using System;
using CocoStudio.Model;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	// Token: 0x02000054 RID: 84
	[Obsolete("This class is just to read old solution, do not use this class.")]
	[Extension(Type = typeof(IResource))]
	[DataItem(Name = "Material")]
	internal class MaterialFile : ResourceFile
	{
		// Token: 0x0600024E RID: 590 RVA: 0x000091F1 File Offset: 0x000073F1
		private MaterialFile()
		{
		}

		// Token: 0x0600024F RID: 591 RVA: 0x000091F9 File Offset: 0x000073F9
		public MaterialFile(FilePath fileName) : base(fileName)
		{
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00009202 File Offset: 0x00007402
		public MaterialFile(ResourceData resourceData) : base(resourceData)
		{
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000920B File Offset: 0x0000740B
		internal override string PreviewImagePath
		{
			get
			{
				return this.FullPath;
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00009213 File Offset: 0x00007413
		protected override ResourceData CreateResourceData(FilePath filePath)
		{
			return base.CreateResourceData(filePath);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000921C File Offset: 0x0000741C
		protected override void OnRefresh()
		{
			base.OnRefresh();
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00009224 File Offset: 0x00007424
		protected override void OnDelete(IProgressMonitor monitor)
		{
			base.OnDelete(monitor);
		}
	}
}
