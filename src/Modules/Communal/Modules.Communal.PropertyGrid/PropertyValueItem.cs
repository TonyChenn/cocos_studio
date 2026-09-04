using System;
using System.Collections.Generic;
using System.Reflection;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x0200001A RID: 26
	public class PropertyValueItem
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004220 File Offset: 0x00002420
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00004237 File Offset: 0x00002437
		public string PropertyName { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004240 File Offset: 0x00002440
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00004257 File Offset: 0x00002457
		public bool IsSettingControl { get; internal set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00004260 File Offset: 0x00002460
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00004277 File Offset: 0x00002477
		public bool IsSettingValue { get; internal set; }

		// Token: 0x17000036 RID: 54
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

		// Token: 0x17000037 RID: 55
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

		// Token: 0x060000B4 RID: 180 RVA: 0x00004380 File Offset: 0x00002580
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

		// Token: 0x060000B5 RID: 181 RVA: 0x0000442C File Offset: 0x0000262C
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

		// Token: 0x04000031 RID: 49
		private List<PropertyInfo> propertyInfoList;
	}
}
