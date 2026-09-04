using System;
using CocoStudio.Basic;

namespace CocoStudio.Projects
{
	// Token: 0x02000061 RID: 97
	public abstract class CocosItemBinding : ICocosItemBinding
	{
		// Token: 0x060002D8 RID: 728 RVA: 0x0000B078 File Offset: 0x00009278
		public CocosItem CreateItem(CocosItemCreateInfo info)
		{
			CocosItem result;
			try
			{
				result = this.OnCreateItem(info);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Create Item failed.", exception);
				result = null;
			}
			return result;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000B0B8 File Offset: 0x000092B8
		protected virtual CocosItem OnCreateItem(CocosItemCreateInfo info)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000B0BF File Offset: 0x000092BF
		public bool CanCreateItem(string fileType)
		{
			return this.OnCanCreateItem(fileType);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000B0C8 File Offset: 0x000092C8
		protected virtual bool OnCanCreateItem(string fileType)
		{
			throw new NotImplementedException();
		}
	}
}
