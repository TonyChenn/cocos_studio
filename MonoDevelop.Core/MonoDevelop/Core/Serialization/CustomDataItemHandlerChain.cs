using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200008F RID: 143
	internal class CustomDataItemHandlerChain : ICustomDataItemHandler
	{
		// Token: 0x060004AA RID: 1194 RVA: 0x00010903 File Offset: 0x0000EB03
		public CustomDataItemHandlerChain(ICustomDataItemHandler mainHandler, ICustomDataItemHandler subHandler)
		{
			this.mainHandler = mainHandler;
			this.subHandler = subHandler;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00010919 File Offset: 0x0000EB19
		public DataCollection Serialize(object obj, ITypeSerializer handler)
		{
			return this.subHandler.Serialize(obj, new ChainedTypeSerializer(this.mainHandler, handler));
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00010933 File Offset: 0x0000EB33
		public void Deserialize(object obj, ITypeSerializer handler, DataCollection data)
		{
			this.subHandler.Deserialize(obj, new ChainedTypeSerializer(this.mainHandler, handler), data);
		}

		// Token: 0x0400018B RID: 395
		private ICustomDataItemHandler mainHandler;

		// Token: 0x0400018C RID: 396
		private ICustomDataItemHandler subHandler;
	}
}
