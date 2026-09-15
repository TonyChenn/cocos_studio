using System;
using CocoStudio.Model;
using CocoStudio.Projects.Formates;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	internal class MaterialFolder : ResourceFolder
	{
		private MaterialFolder()
		{
		}

		public MaterialFolder(FilePath filePath) : base(filePath)
		{
			this.SavedPath = filePath;
			base.BaseDirectory = filePath;
			MaterialManager.GetInstance().UpdateContainer(filePath);
		}

		protected override void OnRefresh()
		{
			base.OnRefresh();
			MaterialManager.GetInstance().UpdateContainer(base.BaseDirectory);
		}

		public override ResourceData GetResourceData()
		{
			return this.CreateResourceData(base.BaseDirectory);
		}

		protected override void OnDelete(IProgressMonitor monitor)
		{
			base.OnDelete(monitor);
			MaterialManager.GetInstance().RemoveContainer(this.SavedPath);
			this.SavedPath = "";
		}

		protected override void OnSetLocation(FilePath newFilePath, bool isRename)
		{
			base.OnSetLocation(newFilePath, isRename);
			if (newFilePath != this.SavedPath)
			{
				MaterialManager.GetInstance().MoveContainer(this.SavedPath, newFilePath);
				this.SavedPath = newFilePath;
			}
		}

		protected override void OnMove(FilePath newMovePath)
		{
			base.OnMove(newMovePath);
			if (newMovePath != this.SavedPath)
			{
				MaterialManager.GetInstance().MoveContainer(this.SavedPath, newMovePath);
				this.SavedPath = newMovePath;
			}
		}

		public const string FileSuffix = ".materials";

		private FilePath SavedPath;
	}
}
