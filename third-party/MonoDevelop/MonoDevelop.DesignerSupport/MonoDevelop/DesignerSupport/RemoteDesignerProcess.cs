using System;
using System.Threading;
using System.Web;
using GLib;
using Gtk;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.DesignerSupport
{
	[AddinDependency("MonoDevelop.DesignerSupport")]
	public abstract class RemoteDesignerProcess : RemoteProcessObject
	{
		private Frame designerFrame;

		private Frame propGridFrame;

		private Plug designerPlug;

		private Plug propGridPlug;

		private System.Threading.Thread gtkThread;

		private bool exceptionOccurred;

		private bool disposed;

		public bool ExceptionOccurred => exceptionOccurred;

		protected Widget PropertyGridWidget
		{
			get
			{
				return propGridFrame.Child;
			}
			set
			{
				if (propGridFrame.Child != null)
				{
					propGridFrame.Remove(propGridFrame.Child);
				}
				if (value != null)
				{
					propGridFrame.Add(value);
					propGridFrame.Child.Show();
				}
			}
		}

		protected Widget DesignerWidget
		{
			get
			{
				return designerFrame.Child;
			}
			set
			{
				if (designerFrame.Child != null)
				{
					designerFrame.Remove(designerFrame.Child);
				}
				if (value != null)
				{
					designerFrame.Add(value);
					designerFrame.ShowAll();
				}
			}
		}

		public RemoteDesignerProcess()
		{
			Application.Init();
			designerFrame = new Frame();
			propGridFrame = new Frame();
			designerFrame.Shadow = ShadowType.None;
			propGridFrame.Shadow = ShadowType.None;
			designerFrame.BorderWidth = 0u;
			designerFrame.Show();
			propGridFrame.Show();
		}

		protected void StartGuiThread()
		{
			if (gtkThread == null)
			{
				gtkThread = new System.Threading.Thread(GuiThread);
				gtkThread.Start();
			}
		}

		private void GuiThread()
		{
			ExceptionManager.UnhandledException += OnUnhandledException;
			bool flag = true;
			while (flag)
			{
				try
				{
					flag = false;
					Application.Run();
				}
				catch (Exception e)
				{
					flag = true;
					exceptionOccurred = true;
					HandleError(e);
				}
				System.Threading.Thread.Sleep(500);
			}
			ExceptionManager.UnhandledException -= OnUnhandledException;
		}

		public bool ExecuteSuccessfully(EventHandler handler)
		{
			bool result = true;
			try
			{
				handler(null, EventArgs.Empty);
			}
			catch (Exception e)
			{
				exceptionOccurred = true;
				result = false;
				HandleError(e);
			}
			return result;
		}

		public virtual void RecoverFromException()
		{
			exceptionOccurred = false;
		}

		protected virtual void HandleError(Exception e)
		{
			LoggingService.LogError("An exception occurred in the designer GUI thread", e);
			string err = "<b><big>The designer has encountered a fatal error:</big></b>\n\n" + HttpUtility.HtmlEncode(e.ToString());
			if (gtkThread != null && gtkThread.ManagedThreadId != System.Threading.Thread.CurrentThread.ManagedThreadId)
			{
				Application.Invoke(delegate
				{
					ShowText(err);
				});
			}
			else
			{
				ShowText(err);
			}
		}

		protected void ShowText(string markup)
		{
			Label label = new Label();
			label.Markup = markup;
			Frame frame = new Frame();
			frame.Add(label);
			frame.BorderWidth = 10u;
			frame.Shadow = ShadowType.None;
			ScrolledWindow scrolledWindow = new ScrolledWindow();
			scrolledWindow.AddWithViewport(frame);
			scrolledWindow.BorderWidth = 0u;
			scrolledWindow.ShadowType = ShadowType.None;
			scrolledWindow.ShowAll();
			DesignerWidget = scrolledWindow;
		}

		public void AttachDesigner(uint socket)
		{
			Application.Invoke(delegate
			{
				designerPlug = AttachChildViaPlug(socket, designerFrame);
			});
		}

		public void AttachPropertyGrid(uint socket)
		{
			Application.Invoke(delegate
			{
				propGridPlug = AttachChildViaPlug(socket, propGridFrame);
			});
		}

		private Plug AttachChildViaPlug(uint socket, Widget child)
		{
			Plug plug = new Plug(socket);
			if (child.Parent != null)
			{
				((Container)child.Parent).Remove(child);
			}
			plug.Add(child);
			plug.Unrealized += RemovePlugChildWhenUnrealised;
			plug.Show();
			return plug;
		}

		private void RemovePlugChildWhenUnrealised(object sender, EventArgs e)
		{
			Plug plug = (Plug)sender;
			if (plug.Child != null)
			{
				plug.Remove(plug.Child);
			}
		}

		public override void Dispose()
		{
			if (!disposed)
			{
				disposed = true;
				Application.Invoke(delegate
				{
					Application.Quit();
					DisposePhase2();
				});
			}
		}

		private void DisposePhase2()
		{
			if (designerPlug != null)
			{
				designerPlug.Dispose();
			}
			if (propGridPlug != null)
			{
				propGridPlug.Dispose();
			}
			if (designerFrame != null)
			{
				designerFrame.Dispose();
			}
			if (propGridFrame != null)
			{
				propGridFrame.Dispose();
			}
			base.Dispose();
		}

		private void OnUnhandledException(UnhandledExceptionEventArgs args)
		{
			HandleError((Exception)args.ExceptionObject);
		}
	}
}
