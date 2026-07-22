using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Modules.Communal.DragMenu
{
	// Token: 0x02000003 RID: 3
	internal class ResourcePropertyHelp
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000024B0 File Offset: 0x000006B0
		public static HashSet<string> GetResourceProperties(object instance)
		{
			HashSet<string> result = new HashSet<string>();
			PropertyInfo[] properties = instance.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				EditorAttribute[] array2 = propertyInfo.GetCustomAttributes(typeof(EditorAttribute), true) as EditorAttribute[];
				if (array2 != null)
				{
					foreach (EditorAttribute editorAttribute in array2)
					{
					}
				}
			}
			return result;
		}
	}
}
