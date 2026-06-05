using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	// Token: 0x02000039 RID: 57
	[TypeExtensionPoint]
	public interface IGameFileSerializer
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600014F RID: 335
		string ID { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000150 RID: 336
		string Label { get; }

		// Token: 0x06000151 RID: 337
		string Serialize(PublishInfo info, GameFile gameFile);

		// Token: 0x06000152 RID: 338
		void ContextInitialize(PublishInfo publishInfo);

		// Token: 0x06000153 RID: 339
		void ContextFinalize(PublishInfo publishInfo);
	}
}
