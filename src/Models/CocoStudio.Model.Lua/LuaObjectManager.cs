using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Lua.Templates;
using Mono.Addins;

namespace CocoStudio.Model.Lua
{
	internal class LuaObjectManager
	{
		static LuaObjectManager()
		{
			LuaObjectManager.InitializeObjectSerializer();
		}

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

		private static List<ILuaObjectSerializer> serializerList = new List<ILuaObjectSerializer>();

		private static Dictionary<Type, List<ILuaObjectSerializer>> serializerCache = new Dictionary<Type, List<ILuaObjectSerializer>>();
	}
}
