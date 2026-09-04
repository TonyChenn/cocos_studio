using System;
using Mono.Addins;

namespace CocoStudio.Core.ExtensionModel
{
	// Token: 0x02000040 RID: 64
	[TypeExtensionPoint(Path = "/CocoStudio/Ide/MainMenuBar")]
	[TypeExtensionPoint(Path = "/CocoStudio/Ide/MainToolbar")]
	[TypeExtensionPoint(Path = "/CocoStudio/Ide/MainStatus")]
	public interface IMainWindowPart
	{
	}
}
