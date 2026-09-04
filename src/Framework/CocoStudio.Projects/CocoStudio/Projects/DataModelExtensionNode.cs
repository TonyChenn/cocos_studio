using System;
using Mono.Addins;

namespace CocoStudio.Projects
{
	// Token: 0x02000024 RID: 36
	public class DataModelExtensionNode : TypeExtensionNode<DataModelExtensionAttribute>
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004110 File Offset: 0x00002310
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00004118 File Offset: 0x00002318
		public new DataModelExtensionAttribute Data { get; private set; }

		// Token: 0x060000C8 RID: 200 RVA: 0x00004124 File Offset: 0x00002324
		protected override void Read(NodeElement elem)
		{
			base.Read(elem);
			object[] customAttributes = base.Type.GetCustomAttributes(typeof(DataModelExtensionAttribute), false);
			if (customAttributes.Length > 0)
			{
				this.Data = (customAttributes[0] as DataModelExtensionAttribute);
				return;
			}
			this.Data = base.Data;
		}
	}
}
