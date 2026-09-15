using System;

namespace CocoStudio.Lib.Prism
{
	public interface IDispatcherFacade
	{
		void BeginInvoke(Delegate method, object arg);
	}
}
