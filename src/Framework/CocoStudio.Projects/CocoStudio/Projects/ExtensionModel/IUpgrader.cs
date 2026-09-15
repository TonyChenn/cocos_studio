using System;

namespace CocoStudio.Projects.ExtensionModel
{
	public interface IUpgrader
	{
		Version Version { get; }

		bool Upgrade(string filePath);
	}
}
