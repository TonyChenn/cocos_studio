using System;

namespace CocoStudio.Model
{
	public static class ResourceType
	{
		public static int ToLuaType(this EnumResourceType type)
		{
			int result;
			switch (type)
			{
			case EnumResourceType.None:
			case EnumResourceType.Normal:
				result = 0;
				break;
			case EnumResourceType.PlistSubImage:
				result = 1;
				break;
			case EnumResourceType.Default:
				result = 0;
				break;
			case EnumResourceType.MarkedSubImage:
				result = 1;
				break;
			default:
				result = 0;
				break;
			}
			return result;
		}
	}
}
