using System;
using System.Reflection;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000030 RID: 48
	public class PropertyAccessorHandler
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00006A10 File Offset: 0x00004C10
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00006A27 File Offset: 0x00004C27
		public string PropertyName { get; private set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00006A30 File Offset: 0x00004C30
		// (set) Token: 0x0600021A RID: 538 RVA: 0x00006A47 File Offset: 0x00004C47
		public Type PropertyType { get; private set; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00006A50 File Offset: 0x00004C50
		// (set) Token: 0x0600021C RID: 540 RVA: 0x00006A67 File Offset: 0x00004C67
		public Func<object, object[], object> GetValue { get; private set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00006A70 File Offset: 0x00004C70
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00006A87 File Offset: 0x00004C87
		public Action<object, object, object[]> SetValue { get; private set; }

		// Token: 0x0600021F RID: 543 RVA: 0x00006A90 File Offset: 0x00004C90
		public PropertyAccessorHandler(PropertyInfo propInfo)
		{
			this.PropertyName = propInfo.Name;
			this.PropertyType = propInfo.PropertyType;
			if (propInfo.CanRead)
			{
				this.GetValue = new Func<object, object[], object>(propInfo.GetValue);
			}
			if (propInfo.CanWrite)
			{
				this.SetValue = new Action<object, object, object[]>(propInfo.SetValue);
			}
		}
	}
}
