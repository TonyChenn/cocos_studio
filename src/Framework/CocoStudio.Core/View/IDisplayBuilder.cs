using System;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Core.View
{
	[TypeExtensionPoint(Path = "CocoStudio/Ide/DisplayBuilder")]
	public interface IDisplayBuilder
	{
		bool CanHandle(FilePath fileName, string mimeType, CocosItem ownerProject);

		bool CanUseAsDefault { get; }
	}
}
