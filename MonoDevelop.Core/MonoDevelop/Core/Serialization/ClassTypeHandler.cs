using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000062 RID: 98
	internal class ClassTypeHandler : ITypeSerializer
	{
		// Token: 0x0600033E RID: 830 RVA: 0x0000CCAC File Offset: 0x0000AEAC
		internal ClassTypeHandler(SerializationContext ctx, ClassDataType cdt)
		{
			this.ctx = ctx;
			this.cdt = cdt;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000CCC2 File Offset: 0x0000AEC2
		public DataCollection Serialize(object instance)
		{
			return this.cdt.Serialize(this.ctx, instance);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000CCD6 File Offset: 0x0000AED6
		public void Deserialize(object instance, DataCollection data)
		{
			this.cdt.DeserializeNoCustom(this.ctx, instance, data);
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000CCEB File Offset: 0x0000AEEB
		public SerializationContext SerializationContext
		{
			get
			{
				return this.ctx;
			}
		}

		// Token: 0x0400011F RID: 287
		private SerializationContext ctx;

		// Token: 0x04000120 RID: 288
		private ClassDataType cdt;
	}
}
