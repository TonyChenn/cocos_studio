using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Modules.Communal.DragMenu
{
	internal class ResourcePropertyHelp
	{
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
