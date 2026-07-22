using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Lua.Templates;
using Mono.Addins;

namespace CocoStudio.Model.Lua
{
	// Token: 0x02000006 RID: 6
	internal class LuaObjectManager
	{
		// Token: 0x06000030 RID: 48 RVA: 0x000025D2 File Offset: 0x000007D2
		static LuaObjectManager()
		{
			LuaObjectManager.InitializeObjectSerializer();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000025F0 File Offset: 0x000007F0
		public static ILuaObjectSerializer GetSerializer(BaseObjectData objectData)
		{
			ILuaObjectSerializer luaObjectSerializer = null;
			List<ILuaObjectSerializer> list;
			LuaObjectManager.serializerCache.TryGetValue(objectData.GetType(), out list);
			if (list == null)
			{
				list = new List<ILuaObjectSerializer>(1);
				LuaObjectManager.serializerCache[objectData.GetType()] = list;
			}
			else
			{
				if (!(objectData is FrameData))
				{
					return list.FirstOrDefault<ILuaObjectSerializer>();
				}
				foreach (ILuaObjectSerializer luaObjectSerializer2 in list)
				{
					if (luaObjectSerializer2.CanSerialize(objectData))
					{
						luaObjectSerializer = luaObjectSerializer2;
						break;
					}
				}
			}
			if (luaObjectSerializer == null)
			{
				foreach (ILuaObjectSerializer luaObjectSerializer3 in LuaObjectManager.serializerList)
				{
					if (luaObjectSerializer3.CanSerialize(objectData))
					{
						luaObjectSerializer = luaObjectSerializer3;
						list.Add(luaObjectSerializer3);
						break;
					}
				}
			}
			return luaObjectSerializer;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000026DC File Offset: 0x000008DC
		private static void InitializeObjectSerializer()
		{
			try
			{
				LuaBaseObject[] extensionObjects = AddinManager.GetExtensionObjects<LuaBaseObject>("/CocoStudio/Model/Lua/Templates");
				foreach (LuaBaseObject item in extensionObjects)
				{
					LuaObjectManager.serializerList.Add(item);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Load ILuaObjectSerializer addin failed.", exception);
			}
		}

		// Token: 0x0400000D RID: 13
		private static List<ILuaObjectSerializer> serializerList = new List<ILuaObjectSerializer>();

		// Token: 0x0400000E RID: 14
		private static Dictionary<Type, List<ILuaObjectSerializer>> serializerCache = new Dictionary<Type, List<ILuaObjectSerializer>>();
	}
}
