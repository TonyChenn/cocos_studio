using System;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace CocoStudio.Model.Lua
{
	// Token: 0x02000002 RID: 2
	[TypeExtensionPoint]
	public interface ILuaObjectSerializer
	{
		// Token: 0x06000001 RID: 1
		bool CanSerialize(BaseObjectData objectData);

		// Token: 0x06000002 RID: 2
		void CreateObject(BaseObjectData objectData);

		// Token: 0x06000003 RID: 3
		void InitializeObject(BaseObjectData objectData);

		// Token: 0x06000004 RID: 4
		void AddChild(BaseObjectData parent, BaseObjectData child);
	}
}
