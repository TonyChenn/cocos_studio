using System;
using CocoStudio.Model.DataModel;
using Xwt.Drawing;

namespace CocoStudio.Model
{
	public interface IProjectFileCreator
	{
		string Name { get; }

		NodeType FileType { get; }

		string FileExtension { get; }

		int Order { get; }

		string LabelName { get; }

		string Description { get; }

		Image Icon { get; }

		int MaxSize { get; }

		bool CanEditSize { get; }

		bool IsShowTrackPoint { get; }

		GameFileData CreateGameProjectData();
	}
}
