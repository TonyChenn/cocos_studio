using System;
using System.Collections.Generic;

namespace CocoStudio.Projects
{
	public interface ISerializeManager
	{
		IGameFileSerializer CurrentSerializer { get; set; }

		IGameFileSerializer DefaultSerializer { get; }

		IEnumerable<IGameFileSerializer> GetSerializerList();
	}
}
