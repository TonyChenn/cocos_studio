using System;
using Gdk;
using Mono.Debugging.Client;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace MonoDevelop.Debugger
{
	public class PreviewWindowManager
	{
		private static PreviewVisualizerWindow wnd;

		public static bool IsVisible
		{
			get
			{
				if (wnd != null)
				{
					return wnd.Visible;
				}
				return false;
			}
		}

		public static event EventHandler WindowClosed;

		public static event EventHandler WindowShown;

		public static void Show(ObjectValue val, Control widget, Rectangle previewButtonArea)
		{
			DestroyWindow();
			wnd = new PreviewVisualizerWindow(val, widget);
			wnd.ShowPopup(widget, previewButtonArea, PopupPosition.Left);
			wnd.Destroyed += HandleDestroyed;
			OnWindowShown(EventArgs.Empty);
		}

		private static void HandleDestroyed(object sender, EventArgs e)
		{
			wnd = null;
			OnWindowClosed(EventArgs.Empty);
		}

		public static void RepositionWindow(Rectangle? newCaret = null)
		{
			if (IsVisible)
			{
				wnd.RepositionWindow(newCaret);
			}
		}

		static PreviewWindowManager()
		{
			if (IdeApp.Workbench != null)
			{
				IdeApp.Workbench.RootWindow.Destroyed += delegate
				{
					DestroyWindow();
				};
			}
			IdeApp.CommandService.KeyPressed += HandleKeyPressed;
			DebuggingService.StoppedEvent += delegate
			{
				DestroyWindow();
			};
		}

		private static void HandleKeyPressed(object sender, KeyPressArgs e)
		{
			if (e.Key == Key.Escape)
			{
				DestroyWindow();
			}
		}

		public static void DestroyWindow()
		{
			if (wnd != null)
			{
				wnd.Destroy();
				wnd = null;
			}
		}

		private static void OnWindowClosed(EventArgs e)
		{
			WindowClosed?.Invoke(null, e);
		}

		private static void OnWindowShown(EventArgs e)
		{
			WindowShown?.Invoke(null, e);
		}
	}
}
