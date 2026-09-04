using System;
using Mono.Addins;

namespace CocoStudio.Model
{
	// Token: 0x02000089 RID: 137
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ModelExtensionAttribute : CustomExtensionAttribute
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0002CFF0 File Offset: 0x0002B1F0
		// (set) Token: 0x060004A5 RID: 1189 RVA: 0x0002D007 File Offset: 0x0002B207
		[NodeAttribute]
		public int Order { get; private set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0002D010 File Offset: 0x0002B210
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x0002D027 File Offset: 0x0002B227
		[NodeAttribute]
		public bool IsDefault { get; private set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x0002D030 File Offset: 0x0002B230
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x0002D047 File Offset: 0x0002B247
		[NodeAttribute]
		public EnumModelType ModelType { get; private set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x0002D050 File Offset: 0x0002B250
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x0002D067 File Offset: 0x0002B267
		public Type MetaDataType { get; private set; }

		// Token: 0x060004AC RID: 1196 RVA: 0x0002D070 File Offset: 0x0002B270
		public ModelExtensionAttribute()
		{
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0002D07B File Offset: 0x0002B27B
		public ModelExtensionAttribute([NodeAttribute("Order")] int order) : this(false, order)
		{
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0002D088 File Offset: 0x0002B288
		public ModelExtensionAttribute([NodeAttribute("Order")] int order, Type metaDataType) : this(false, order, metaDataType)
		{
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0002D096 File Offset: 0x0002B296
		public ModelExtensionAttribute(int order, EnumModelType modelType) : this(false, order, modelType)
		{
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0002D0A4 File Offset: 0x0002B2A4
		internal ModelExtensionAttribute([NodeAttribute("IsDefault")] bool isDefault, [NodeAttribute("Order")] int order)
		{
			this.Order = order;
			this.IsDefault = isDefault;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0002D0BF File Offset: 0x0002B2BF
		internal ModelExtensionAttribute(bool isDefault, int order, Type metaDataType) : this(isDefault, order)
		{
			this.MetaDataType = metaDataType;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0002D0D4 File Offset: 0x0002B2D4
		internal ModelExtensionAttribute([NodeAttribute("IsDefault")] bool isDefault, [NodeAttribute("Order")] int order, [NodeAttribute("ModelType")] EnumModelType modelType) : this(isDefault, order)
		{
			this.ModelType = modelType;
		}
	}
}
