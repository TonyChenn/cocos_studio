using System;

namespace Gtk
{
	public interface IWebView
	{
		event EventHandler<WebNewWindowEventArgs> WebNewWindow;

		event EventHandler<WebNavigatingEventArgs> WebNavigating;

		event EventHandler<WebJavaScriptCallEventArgs> WebJavaScriptCalled;

		void Activated(Widget parentWidget);

		void Deactivated();

		void PostData(string postData);

		bool IsMenuEnabled { get; set; }
	}
}
