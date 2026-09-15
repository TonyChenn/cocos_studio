using System;
using Gtk;

namespace CocoStudio.Lib.Prism
{
	public class DefaultDispatcher : IDispatcherFacade
	{
		public void BeginInvoke(Delegate method, object arg)
		{
			Application.Invoke(delegate(object param0, EventArgs param1)
			{
				method.DynamicInvoke(new object[]
				{
					arg
				});
			});
		}

		public void BeginInvoke(EventHandler method, EventArgs args)
		{
			throw new NotImplementedException();
		}
	}
}
