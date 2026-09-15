using System;
using System.Collections.Generic;
using System.Reflection;

namespace Modules.Communal.PropertyGrid
{
	public class PropertyValueItem
	{
		public string PropertyName { get; private set; }

		public bool IsSettingControl { get; internal set; }

		public bool IsSettingValue { get; internal set; }

		public object this[int index]
		{
			get
			{
				object result;
				if (index < 0 || index >= this.propertyInfoList.Count)
				{
					result = null;
				}
				else
				{
					PropertyInfo propertyInfo = this.propertyInfoList[index];
					result = propertyInfo.GetValue(PropertyItem.Objects[index]);
				}
				return result;
			}
			set
			{
				if (index >= 0 && index < this.propertyInfoList.Count)
				{
					if (!this.IsSettingControl)
					{
						this.IsSettingValue = true;
						PropertyInfo propertyInfo = this.propertyInfoList[index];
						propertyInfo.SetValue(PropertyItem.Objects[index], value);
						this.IsSettingValue = false;
					}
				}
			}
		}

		public object this[object obj]
		{
			get
			{
				int index = this.GetIndex(obj);
				return this[index];
			}
			set
			{
				int index = this.GetIndex(obj);
				this[index] = value;
			}
		}

		public PropertyValueItem(string name)
		{
			this.PropertyName = name;
			this.propertyInfoList = new List<PropertyInfo>();
			this.IsSettingControl = (this.IsSettingValue = false);
			foreach (object obj in PropertyItem.Objects)
			{
				Type type = obj.GetType();
				PropertyInfo property = type.GetProperty(this.PropertyName, BindingFlags.Instance | BindingFlags.Public);
				this.propertyInfoList.Add(property);
			}
		}

		private int GetIndex(object obj)
		{
			for (int i = 0; i < PropertyItem.Objects.Count; i++)
			{
				if (PropertyItem.Objects[i] == obj)
				{
					return i;
				}
			}
			return -1;
		}

		private List<PropertyInfo> propertyInfoList;
	}
}
