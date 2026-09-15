using System;

namespace CocoStudio.Core.ExtensionModel
{
	public abstract class MenuHandler
	{
		internal void InternalRun()
		{
			this.Run();
		}

		internal void InternalRun(object dataItem)
		{
			this.Run(dataItem);
		}

		internal void InternalUpdate(MenuInfo info)
		{
			this.Update(info);
		}

		internal void InternalUpdate(MenuArrayInfo info)
		{
			this.Update(info);
		}

		protected virtual void Run()
		{
		}

		protected virtual void Run(object dataItem)
		{
			this.Run();
		}

		protected virtual void Update(MenuInfo info)
		{
		}

		protected virtual void Update(MenuArrayInfo info)
		{
		}
	}
}
