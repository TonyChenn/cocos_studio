using System;
using Mono.Addins;

namespace CocoStudio.Core.ExtensionModel
{
	[TypeExtensionPoint(Path = "/CocoStudio/Ide/MainMenuBar")]
	[TypeExtensionPoint(Path = "/CocoStudio/Ide/MainToolbar")]
	[TypeExtensionPoint(Path = "/CocoStudio/Ide/MainStatus")]
	public interface IMainWindowPart
	{
	}
}
