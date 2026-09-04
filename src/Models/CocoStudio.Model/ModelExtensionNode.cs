using System;
using Mono.Addins;

namespace CocoStudio.Model
{
	// Token: 0x02000088 RID: 136
	public class ModelExtensionNode : TypeExtensionNode<ModelExtensionAttribute>, IComparable<ModelExtensionNode>
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x00014150 File Offset: 0x00012350
		// (set) Token: 0x060004A1 RID: 1185 RVA: 0x00014167 File Offset: 0x00012367
		public new ModelExtensionAttribute Data { get; private set; }

		// Token: 0x060004A2 RID: 1186 RVA: 0x00014170 File Offset: 0x00012370
		protected override void Read(NodeElement elem)
		{
			base.Read(elem);
			object[] customAttributes = base.Type.GetCustomAttributes(typeof(ModelExtensionAttribute), false);
			if (customAttributes.Length > 0)
			{
				this.Data = (customAttributes[0] as ModelExtensionAttribute);
			}
			else
			{
				this.Data = base.Data;
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x000141C8 File Offset: 0x000123C8
		public int CompareTo(ModelExtensionNode other)
		{
			int result;
			if (this.Data.IsDefault && !other.Data.IsDefault)
			{
				result = -1;
			}
			else if (!this.Data.IsDefault && other.Data.IsDefault)
			{
				result = 1;
			}
			else
			{
				result = this.Data.Order.CompareTo(other.Data.Order);
			}
			return result;
		}
	}
}
