using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	[TypeExtensionPoint]
	public interface IGameFileSerializer
	{
		string ID { get; }

		string Label { get; }

		string Serialize(PublishInfo info, GameFile gameFile);

		void ContextInitialize(PublishInfo publishInfo);

		void ContextFinalize(PublishInfo publishInfo);
	}
}
