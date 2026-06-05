using System;

namespace CocoStudio.Model
{
	// Token: 0x02000015 RID: 21
	public static class ResourceType
	{
		// Token: 0x06000094 RID: 148 RVA: 0x0000313C File Offset: 0x0000133C
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
