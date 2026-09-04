using System;
using System.Collections.Generic;
using Mono.Addins;

namespace CocoStudio.Model.ExtensionModel
{
	// Token: 0x02000081 RID: 129
	[TypeExtensionPoint]
	public interface IModelScanner
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000483 RID: 1155
		string Description { get; }

		// Token: 0x06000484 RID: 1156
		IEnumerable<ModelMetaData> GetModels();
	}
}
