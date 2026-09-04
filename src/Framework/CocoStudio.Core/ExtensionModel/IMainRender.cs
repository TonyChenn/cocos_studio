using System;
using Mono.Addins;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core.ExtensionModel
{
	// Token: 0x0200003F RID: 63
	[TypeExtensionPoint("/CocoStudio/Ide/Render")]
	public interface IMainRender : IViewContentExtend, IViewContent, IBaseViewContent, IDisposable
	{
		// Token: 0x06000232 RID: 562
		void SwitchView();
	}
}
