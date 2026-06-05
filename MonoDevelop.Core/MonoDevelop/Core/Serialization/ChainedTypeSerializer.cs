using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000090 RID: 144
	internal class ChainedTypeSerializer : ITypeSerializer
	{
		// Token: 0x060004AD RID: 1197 RVA: 0x0001094E File Offset: 0x0000EB4E
		public ChainedTypeSerializer(ICustomDataItemHandler handler, ITypeSerializer serializer)
		{
			this.handler = handler;
			this.serializer = serializer;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00010964 File Offset: 0x0000EB64
		public DataCollection Serialize(object instance)
		{
			return this.handler.Serialize(instance, this.serializer);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00010978 File Offset: 0x0000EB78
		public void Deserialize(object instance, DataCollection data)
		{
			this.handler.Deserialize(instance, this.serializer, data);
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0001098D File Offset: 0x0000EB8D
		public SerializationContext SerializationContext
		{
			get
			{
				return this.serializer.SerializationContext;
			}
		}

		// Token: 0x0400018D RID: 397
		private ICustomDataItemHandler handler;

		// Token: 0x0400018E RID: 398
		private ITypeSerializer serializer;
	}
}
