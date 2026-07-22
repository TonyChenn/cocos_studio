using System;
using CocoStudio.Projects;
using CocoStudio.Projects.ExtensionModel;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200000B RID: 11
	public abstract class GameFileUpgrader : IFileUpgrader, IUpgrader
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000041 RID: 65
		public abstract Version Version { get; }

		// Token: 0x06000042 RID: 66 RVA: 0x00002734 File Offset: 0x00000934
		public virtual bool Upgrade(string filePath)
		{
			return false;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002748 File Offset: 0x00000948
		public virtual bool Upgrade(CocosFile file)
		{
			GameFileContent gameFileContent = file.Content as GameFileContent;
			return gameFileContent != null && this.OnUpgrade(gameFileContent.Content);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002784 File Offset: 0x00000984
		protected virtual bool OnUpgrade(GameFileData projectData)
		{
			return false;
		}
	}
}
