using System;
using System.Collections.Generic;
using Mono.Addins;

namespace CocoStudio.Model.ExtensionModel
{
	[TypeExtensionPoint]
	public interface IModelScanner
	{
		string Description { get; }

		IEnumerable<ModelMetaData> GetModels();
	}
}
