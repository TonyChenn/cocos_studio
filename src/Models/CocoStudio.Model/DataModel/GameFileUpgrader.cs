using System;
using CocoStudio.Projects;
using CocoStudio.Projects.ExtensionModel;

namespace CocoStudio.Model.DataModel
{
	public abstract class GameFileUpgrader : IFileUpgrader, IUpgrader
	{
		public abstract Version Version { get; }

		public virtual bool Upgrade(string filePath)
		{
			return false;
		}

		public virtual bool Upgrade(CocosFile file)
		{
			GameFileContent gameFileContent = file.Content as GameFileContent;
			return gameFileContent != null && this.OnUpgrade(gameFileContent.Content);
		}

		protected virtual bool OnUpgrade(GameFileData projectData)
		{
			return false;
		}
	}
}
