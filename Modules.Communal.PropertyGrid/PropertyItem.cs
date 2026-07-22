using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Model;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000017 RID: 23
	public class PropertyItem
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000037C0 File Offset: 0x000019C0
		public static object FirstObject
		{
			get
			{
				object result;
				if (PropertyItem.Objects == null || PropertyItem.Objects.Count == 0)
				{
					result = null;
				}
				else
				{
					result = PropertyItem.Objects[0];
				}
				return result;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003800 File Offset: 0x00001A00
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00003816 File Offset: 0x00001A16
		public static IReadOnlyList<object> Objects { get; internal set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00003820 File Offset: 0x00001A20
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00003837 File Offset: 0x00001A37
		public string Name { get; private set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003840 File Offset: 0x00001A40
		// (set) Token: 0x06000094 RID: 148 RVA: 0x0000385E File Offset: 0x00001A5E
		public object FirstValue
		{
			get
			{
				return this.Values[0];
			}
			set
			{
				this.Values[0] = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00003870 File Offset: 0x00001A70
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00003887 File Offset: 0x00001A87
		public PropertyValueItem Values { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00003890 File Offset: 0x00001A90
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000038A7 File Offset: 0x00001AA7
		public AttributeCollection Attributes { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000099 RID: 153 RVA: 0x000038B0 File Offset: 0x00001AB0
		// (set) Token: 0x0600009A RID: 154 RVA: 0x000038C7 File Offset: 0x00001AC7
		public OperationMask? RequestOperation { get; private set; }

		// Token: 0x0600009B RID: 155 RVA: 0x000038D0 File Offset: 0x00001AD0
		public PropertyItem(PropertyDescriptor propDesc)
		{
			this.Name = propDesc.Name;
			this.Attributes = propDesc.Attributes;
			this.Values = new PropertyValueItem(this.Name);
			this.RequestOperation = null;
			RequestOperationModeAttribute requestOperationModeAttribute = propDesc.Attributes[typeof(RequestOperationModeAttribute)] as RequestOperationModeAttribute;
			if (requestOperationModeAttribute != null)
			{
				this.RequestOperation = new OperationMask?(requestOperationModeAttribute.RequestMode);
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003958 File Offset: 0x00001B58
		public T GetValue<T>(int index)
		{
			object obj = this.Values[index];
			return (T)((object)obj);
		}
	}
}
