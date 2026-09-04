using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Gtk;
using Xwt.Backends;
using Xwt.GtkBackend;

namespace CocoStudio.WindowsPlatform
{
	public class CSWindowWebViewBackend : WidgetBackend, IWebViewBackend, IWidgetBackend, IBackend, IWebView
	{
		private WebBrowser view;

		private string url;

		private Socket socket;

		private bool enableNavigatingEvent;

		private bool enableLoadingEvent;

		private bool enableLoadedEvent;

		private bool enableTitleChangedEvent;

		private string postData;

		private int x;

		private int y;

		private bool isMenuEnabled;

		private bool scrollBarsEnabled;

		private bool drawsBackground = true;

		private string customCss = string.Empty;

		private double loadProgress;

		public string Url
		{
			get
			{
				if (view != null && !string.IsNullOrEmpty(view.Url.AbsoluteUri))
				{
					url = view.Url.AbsoluteUri;
				}
				return url;
			}
			set
			{
				url = value;
				if (view != null)
				{
					if (view.IsBusy)
					{
						view.Stop();
					}
					if (postData == null)
					{
						view.Navigate(url);
					}
					else
					{
						view.Navigate(url, null, Encoding.GetEncoding("UTF-8").GetBytes(postData), "Content-Type: application/x-www-form-urlencoded");
					}
					postData = null;
				}
			}
		}

		public bool IsMenuEnabled
		{
			get
			{
				return isMenuEnabled;
			}
			set
			{
				isMenuEnabled = value;
				if (view != null)
				{
					view.IsWebBrowserContextMenuEnabled = value;
				}
			}
		}

		// Xwt 0.2 adds these settings to IWebViewBackend. Retain settings before the native browser is realized.
		public bool ContextMenuEnabled
		{
			get => IsMenuEnabled;
			set => IsMenuEnabled = value;
		}

		public bool ScrollBarsEnabled
		{
			get => scrollBarsEnabled;
			set
			{
				scrollBarsEnabled = value;
				if (view != null) view.ScrollBarsEnabled = value;
			}
		}

		public bool DrawsBackground
		{
			get => drawsBackground;
			set { drawsBackground = value; ApplyCustomStyle(); }
		}

		public string CustomCss
		{
			get => customCss;
			set { customCss = value ?? string.Empty; ApplyCustomStyle(); }
		}

		private void ApplyCustomStyle()
		{
			HtmlDocument document = view?.Document;
			if (document == null || document.GetElementsByTagName("head").Count == 0) return;
			HtmlElement style = document.GetElementById("__cocostudio_xwt_style");
			if (style == null)
			{
				style = document.CreateElement("style");
				style.SetAttribute("id", "__cocostudio_xwt_style");
				style.SetAttribute("type", "text/css");
				document.GetElementsByTagName("head")[0].AppendChild(style);
			}
			// MSHTML exposes CSS text through styleSheet rather than the style element's InnerText.
			object sheet = style.DomElement.GetType().InvokeMember("styleSheet", System.Reflection.BindingFlags.GetProperty, null, style.DomElement, null);
			sheet.GetType().InvokeMember("cssText", System.Reflection.BindingFlags.SetProperty, null, sheet,
				new object[] { customCss + (drawsBackground ? string.Empty : "\nhtml, body { background: transparent !important; }") });
		}

		public double LoadProgress => loadProgress;

		public bool CanGoBack => view.CanGoBack;

		public bool CanGoForward => view.CanGoForward;

		public string Title => view.Document.Title;

		protected new IWebViewEventSink EventSink => (IWebViewEventSink)base.EventSink;

		public event EventHandler<WebNewWindowEventArgs> WebNewWindow;

		public event EventHandler<WebNavigatingEventArgs> WebNavigating;

		public event EventHandler<WebJavaScriptCallEventArgs> WebJavaScriptCalled;

		[DllImport("user32.dll")]
		internal static extern IntPtr SetParent([In] IntPtr hwndChild, [In] IntPtr hwndNewParent);

		public override void Initialize()
		{
			base.Initialize();
			socket = new Socket();
			base.Widget = socket;
			base.Widget.Realized += HandleGtkRealized;
			base.Widget.SizeAllocated += HandleGtkSizeAllocated;
			base.Widget.Show();
		}

		private void HandleGtkRealized(object sender, EventArgs e)
		{
			Size size = new Size(base.Widget.WidthRequest, base.Widget.HeightRequest);
			if (view == null)
			{
				view = new WebBrowser();
				view.ScriptErrorsSuppressed = true;
				view.AllowWebBrowserDrop = false;
				view.ScrollBarsEnabled = scrollBarsEnabled;
				view.IsWebBrowserContextMenuEnabled = isMenuEnabled;
				view.ObjectForScripting = this;
				view.ProgressChanged += HandleProgressChanged;
				view.Navigated += HandleNavigated;
				view.Navigating += view_Navigating;
				view.DocumentTitleChanged += HandleDocumentTitleChanged;
				view.NewWindow += view_NewWindow;
				view.DocumentCompleted += view_DocumentCompleted;
			}
			IntPtr handle = view.Handle;
			IntPtr hwndNewParent = (IntPtr)socket.Id;
			SetParent(handle, hwndNewParent);
			if (url != null)
			{
				view.Navigate(url);
			}
			if (size.Width > 0 && size.Height > 0)
			{
				view.Size = size;
			}
		}

		public void GetJsInfo(string type, string info)
		{
			if (type == EJsInfoType._x.ToString())
			{
				if (WebNewWindow != null)
				{
					WebNewWindow(this, new WebNewWindowEventArgs(info, x, y));
				}
			}
		}

		private void view_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			if (e.Url.ToString().Contains("javascript:void(0)") && e.TargetFrameName == "")
			{
				e.Cancel = true;
			}
			else if (WebNavigating != null)
			{
				string text = e.Url.AbsoluteUri.ToString();
				WebNavigating(this, new WebNavigatingEventArgs(text));
			}
		}

		public void PostData(string postData)
		{
			this.postData = postData;
		}

		private void view_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			ApplyCustomStyle();
			if (view.Document != null)
			{
				view.Document.MouseDown += Document_MouseDown;
			}
		}

		private void Document_MouseDown(object sender, HtmlElementEventArgs e)
		{
			x = e.ClientMousePosition.X;
			y = e.ClientMousePosition.Y;
		}

		private void HandleGtkSizeAllocated(object sender, SizeAllocatedArgs e)
		{
			Size size = new Size(e.Allocation.Width, e.Allocation.Height);
			view.Size = size;
		}

		public void GoBack()
		{
			view.GoBack();
		}

		public void GoForward()
		{
			view.GoForward();
		}

		public void Reload()
		{
			view.Refresh();
		}

		public void StopLoading()
		{
			view.Stop();
		}

		public void LoadHtml(string content, string base_uri)
		{
			view.DocumentText = content;
		}

		public override void EnableEvent(object eventId)
		{
			base.EnableEvent(eventId);
			if (eventId is WebViewEvent)
			{
				switch ((WebViewEvent)eventId)
				{
				case WebViewEvent.NavigateToUrl:
					enableNavigatingEvent = true;
					break;
				case WebViewEvent.Loading:
					enableLoadingEvent = true;
					break;
				case WebViewEvent.Loaded:
					enableLoadedEvent = true;
					break;
				case WebViewEvent.TitleChanged:
					enableTitleChangedEvent = true;
					break;
				}
			}
		}

		public override void DisableEvent(object eventId)
		{
			base.DisableEvent(eventId);
			if (eventId is WebViewEvent)
			{
				switch ((WebViewEvent)eventId)
				{
				case WebViewEvent.NavigateToUrl:
					enableNavigatingEvent = false;
					break;
				case WebViewEvent.Loading:
					enableLoadingEvent = false;
					break;
				case WebViewEvent.Loaded:
					enableLoadedEvent = false;
					break;
				case WebViewEvent.TitleChanged:
					enableTitleChangedEvent = false;
					break;
				}
			}
		}

		private void HandleProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
		{
			if (e.CurrentProgress == -1)
			{
				loadProgress = 1.0;
				HandleLoaded(view, EventArgs.Empty);
			}
			else if (e.MaximumProgress == 0)
			{
				loadProgress = 1.0;
			}
			else
			{
				loadProgress = (double)e.CurrentProgress / (double)e.MaximumProgress;
			}
		}

		private void view_NewWindow(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			if (WebNewWindow != null)
			{
				WebNewWindow(sender, new WebNewWindowEventArgs(view.StatusText, x, y));
			}
		}

		private void HandleDocumentTitleChanged(object sender, EventArgs e)
		{
			if (enableTitleChangedEvent)
			{
				base.ApplicationContext.InvokeUserCode(delegate
				{
					EventSink.OnTitleChanged();
				});
			}
		}

		private void HandleNavigated(object sender, WebBrowserNavigatedEventArgs e)
		{
			if (enableLoadingEvent)
			{
				base.ApplicationContext.InvokeUserCode(delegate
				{
					EventSink.OnLoading();
				});
			}
		}

		private void HandleLoaded(object sender, EventArgs e)
		{
			if (enableLoadedEvent)
			{
				base.ApplicationContext.InvokeUserCode(delegate
				{
					EventSink.OnLoaded();
				});
			}
		}

		public void Activated(Widget parentWidget)
		{
		}

		public void Deactivated()
		{
		}
	}
}
