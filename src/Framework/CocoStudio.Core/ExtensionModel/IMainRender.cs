using System;
using Mono.Addins;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core.ExtensionModel
{
	[TypeExtensionPoint("/CocoStudio/Ide/Render")]
	public interface IMainRender : IViewContentExtend, IViewContent, IBaseViewContent, IDisposable
	{
		void SwitchView();
	}
}
