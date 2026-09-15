using System;
using CocoStudio.Basic;

namespace CocoStudio.Projects
{
	public abstract class CocosItemBinding : ICocosItemBinding
	{
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

		protected virtual CocosItem OnCreateItem(CocosItemCreateInfo info)
		{
			throw new NotImplementedException();
		}

		public bool CanCreateItem(string fileType)
		{
			return this.OnCanCreateItem(fileType);
		}

		protected virtual bool OnCanCreateItem(string fileType)
		{
			throw new NotImplementedException();
		}
	}
}
