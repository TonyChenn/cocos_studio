using System;
using Xwt;
using Xwt.GtkBackend;

namespace Gtk.Web
{
	// Token: 0x02000090 RID: 144
	public class WebView : WebView, IWebView
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000C87C File Offset: 0x0000AA7C
		public Widget GtkWidget
		{
			get
			{
				return (base.BackendHost.Backend as WidgetBackend).Widget;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000C8A4 File Offset: 0x0000AAA4
		private IWebView IWebView
		{
			get
			{
				return base.BackendHost.Backend as IWebView;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000C8C8 File Offset: 0x0000AAC8
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

		// Token: 0x0600030A RID: 778 RVA: 0x0000C8FA File Offset: 0x0000AAFA
		private WebView()
		{
		}

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x0600030B RID: 779 RVA: 0x0000C908 File Offset: 0x0000AB08
		// (remove) Token: 0x0600030C RID: 780 RVA: 0x0000C930 File Offset: 0x0000AB30
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

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x0600030D RID: 781 RVA: 0x0000C958 File Offset: 0x0000AB58
		// (remove) Token: 0x0600030E RID: 782 RVA: 0x0000C980 File Offset: 0x0000AB80
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

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x0600030F RID: 783 RVA: 0x0000C9A8 File Offset: 0x0000ABA8
		// (remove) Token: 0x06000310 RID: 784 RVA: 0x0000C9D0 File Offset: 0x0000ABD0
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

		// Token: 0x06000311 RID: 785 RVA: 0x0000C9F8 File Offset: 0x0000ABF8
		public void Activated(Widget parentWidget)
		{
			if (this.IWebView != null)
			{
				this.IWebView.Activated(parentWidget);
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000CA24 File Offset: 0x0000AC24
		public void Deactivated()
		{
			if (this.IWebView != null)
			{
				this.IWebView.Deactivated();
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000CA4D File Offset: 0x0000AC4D
		public void PostData(string postData)
		{
			this.IWebView.PostData(postData);
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000CA60 File Offset: 0x0000AC60
		// (set) Token: 0x06000315 RID: 789 RVA: 0x0000CA7D File Offset: 0x0000AC7D
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

		// Token: 0x0400039D RID: 925
		private static WebView instance;
	}
}
