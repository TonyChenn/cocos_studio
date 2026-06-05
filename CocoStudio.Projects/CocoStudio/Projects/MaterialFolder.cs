using System;
using CocoStudio.Model;
using CocoStudio.Projects.Formates;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	// Token: 0x02000049 RID: 73
	internal class MaterialFolder : ResourceFolder
	{
		// Token: 0x060001FF RID: 511 RVA: 0x00007E34 File Offset: 0x00006034
		private MaterialFolder()
		{
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00007E3C File Offset: 0x0000603C
		public MaterialFolder(FilePath filePath) : base(filePath)
		{
			this.SavedPath = filePath;
			base.BaseDirectory = filePath;
			MaterialManager.GetInstance().UpdateContainer(filePath);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00007E63 File Offset: 0x00006063
		protected override void OnRefresh()
		{
			base.OnRefresh();
			MaterialManager.GetInstance().UpdateContainer(base.BaseDirectory);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00007E80 File Offset: 0x00006080
		public override ResourceData GetResourceData()
		{
			return this.CreateResourceData(base.BaseDirectory);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00007E8E File Offset: 0x0000608E
		protected override void OnDelete(IProgressMonitor monitor)
		{
			base.OnDelete(monitor);
			MaterialManager.GetInstance().RemoveContainer(this.SavedPath);
			this.SavedPath = "";
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00007EBC File Offset: 0x000060BC
		protected override void OnSetLocation(FilePath newFilePath, bool isRename)
		{
			base.OnSetLocation(newFilePath, isRename);
			if (newFilePath != this.SavedPath)
			{
				MaterialManager.GetInstance().MoveContainer(this.SavedPath, newFilePath);
				this.SavedPath = newFilePath;
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00007EF6 File Offset: 0x000060F6
		protected override void OnMove(FilePath newMovePath)
		{
			base.OnMove(newMovePath);
			if (newMovePath != this.SavedPath)
			{
				MaterialManager.GetInstance().MoveContainer(this.SavedPath, newMovePath);
				this.SavedPath = newMovePath;
			}
		}

		// Token: 0x0400007F RID: 127
		public const string FileSuffix = ".materials";

		// Token: 0x04000080 RID: 128
		private FilePath SavedPath;
	}
}
