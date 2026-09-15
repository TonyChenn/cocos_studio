using System;
using Xwt;
using Xwt.GtkBackend;

	namespace Gtk.Web
	{
		public class WebView : Xwt.WebView, IWebView
		{
		public Widget GtkWidget
		{
			get
			{
				return (base.BackendHost.Backend as WidgetBackend).Widget;
			}
		}

		private IWebView IWebView
		{
			get
			{
				return base.BackendHost.Backend as IWebView;
			}
		}

		public static WebView Instance
		{
			get
			{
				if (WebView.instance == null)
				{
					WebView.instance = new WebView();
				}
				return WebView.instance;
			}
		}

		private WebView()
		{
		}

		public event EventHandler<WebNewWindowEventArgs> WebNewWindow
		{
			add
			{
				if (this.IWebView != null)
				{
					this.IWebView.WebNewWindow += value;
				}
			}
			remove
			{
				if (this.IWebView != null)
				{
					this.IWebView.WebNewWindow -= value;
				}
			}
		}

		public event EventHandler<WebJavaScriptCallEventArgs> WebJavaScriptCalled
		{
			add
			{
				if (this.IWebView != null)
				{
					this.IWebView.WebJavaScriptCalled += value;
				}
			}
			remove
			{
				if (this.IWebView != null)
				{
					this.IWebView.WebJavaScriptCalled -= value;
				}
			}
		}

		public event EventHandler<WebNavigatingEventArgs> WebNavigating
		{
			add
			{
				if (this.IWebView != null)
				{
					this.IWebView.WebNavigating += value;
				}
			}
			remove
			{
				if (this.IWebView != null)
				{
					this.IWebView.WebNavigating -= value;
				}
			}
		}

		public void Activated(Widget parentWidget)
		{
			if (this.IWebView != null)
			{
				this.IWebView.Activated(parentWidget);
			}
		}

		public void Deactivated()
		{
			if (this.IWebView != null)
			{
				this.IWebView.Deactivated();
			}
		}

		public void PostData(string postData)
		{
			this.IWebView.PostData(postData);
		}

		public bool IsMenuEnabled
		{
			get
			{
				return this.IWebView.IsMenuEnabled;
			}
			set
			{
				this.IWebView.IsMenuEnabled = value;
			}
		}

		private static WebView instance;
	}
}
