using System;
using Mono.Addins;

namespace CocoStudio.Core.ModulesInterface
{
	[TypeExtensionPoint]
	public interface IAutoSaveManager
	{
		int SaveTimeInterval { get; set; }

		void Initialize();

		void StartAutoSave();

		void StopAutoSave();
	}
}
