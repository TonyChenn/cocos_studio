using System;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200008E RID: 142
	internal class CustomDataItemWrapper : ICustomDataItem
	{
		// Token: 0x060004A7 RID: 1191 RVA: 0x000108C4 File Offset: 0x0000EAC4
		public CustomDataItemWrapper(ICustomDataItemHandler handler, object ob)
		{
			this.itemHandler = handler;
			this.ob = ob;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000108DA File Offset: 0x0000EADA
		public DataCollection Serialize(ITypeSerializer handler)
		{
			return this.itemHandler.Serialize(this.ob, handler);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x000108EE File Offset: 0x0000EAEE
		public void Deserialize(ITypeSerializer handler, DataCollection data)
		{
			this.itemHandler.Deserialize(this.ob, handler, data);
		}

		// Token: 0x04000189 RID: 393
		private ICustomDataItemHandler itemHandler;

		// Token: 0x0400018A RID: 394
		private object ob;
	}
}
