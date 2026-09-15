using System;
using Mono.Addins;

namespace CocoStudio.Core.ExtensionModel
{
	[TypeExtensionPoint]
	public interface ICommandHandle
	{
		void Initialize();
	}
}
