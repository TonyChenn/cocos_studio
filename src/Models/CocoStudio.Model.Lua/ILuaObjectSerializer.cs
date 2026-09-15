using System;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace CocoStudio.Model.Lua
{
	[TypeExtensionPoint]
	public interface ILuaObjectSerializer
	{
		bool CanSerialize(BaseObjectData objectData);

		void CreateObject(BaseObjectData objectData);

		void InitializeObject(BaseObjectData objectData);

		void AddChild(BaseObjectData parent, BaseObjectData child);
	}
}
