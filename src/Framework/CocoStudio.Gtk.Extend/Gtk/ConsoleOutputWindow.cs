using System;
using System.Diagnostics;
using CocoStudio.Basic;
using GLib;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace Gtk
{
	// Token: 0x020000A3 RID: 163
	public class ConsoleOutputWindow : Window
	{
		// Token: 0x06000391 RID: 913 RVA: 0x00012208 File Offset: 0x00010408
		public ConsoleOutputWindow() : base(WindowType.Toplevel)
		{
			this.Build();
			base.Title = LanguageInfo.Run_Console;
			base.Destroyed += this.DestroyedHandler;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00012274 File Offset: 0x00010474
		private void CloseWindow()
		{
			Timeout.Add(0U, delegate
			{
				if (!this.hasDestroyed)
				{
					this.Destroy();
					this.hasDestroyed = true;
				}
				return false;
			});
			if (this.process != null)
			{
				try
				{
					using (this.process)
					{
						this.process.OutputDataReceived -= this.OutputDataReceivedHandler;
						this.process.ErrorDataReceived -= this.OutputDataReceivedHandler;
						if (!this.process.HasExited)
						{
							this.process.Kill();
						}
					}
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("释放进程时出错", exception);
				}
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0001234C File Offset: 0x0001054C
		public void StartRunning(CocosMonitor monitor, System.Diagnostics.Process proc)
		{
			if (monitor != null && proc != null)
			{
				this.monitor = monitor;
				this.process = proc;
				monitor.Finished += this.FinishedHandler;
				this.process.OutputDataReceived += this.OutputDataReceivedHandler;
				this.process.ErrorDataReceived += this.OutputDataReceivedHandler;
				monitor.Start();
				proc.Start();
				proc.BeginOutputReadLine();
				proc.BeginErrorReadLine();
				if (!monitor.IsProcessing)
				{
					this.CloseWindow();
				}
				else
				{
					this.CenterToParentWindow(ApplicationCurrent.MainWindow);
					base.Show();
					base.Present();
				}
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0001240F File Offset: 0x0001060F
		private void FinishedHandler(object sender, FinishedArgs e)
		{
			this.CloseWindow();
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001241C File Offset: 0x0001061C
		private void DestroyedHandler(object sender, EventArgs e)
		{
			if (this.monitor != null && this.monitor.IsProcessing)
			{
				this.monitor.Finish(true);
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000124F8 File Offset: 0x000106F8
		private void OutputDataReceivedHandler(object sender, DataReceivedEventArgs e)
		{
			Timeout.Add(0U, delegate
			{
				TextIter endIter = this.textview_output.Buffer.EndIter;
				this.textview_output.Buffer.Insert(ref endIter, e.Data + "\r\n");
				this.textview_output.ScrollToIter(this.textview_output.Buffer.EndIter, 0.0, false, 0.0, 0.0);
				return false;
			});
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00012530 File Offset: 0x00010730
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Gtk.ConsoleOutputWindow";
			base.Title = Catalog.GetString("ConsoleOutputWindow");
			base.WindowPosition = WindowPosition.CenterOnParent;
			this.eventbox_background = new EventBox();
			this.eventbox_background.Name = "eventbox_background";
			this.GtkScrolledWindow = new ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ShadowType.In;
			this.textview_output = new TextView();
			this.textview_output.CanFocus = true;
			this.textview_output.Name = "textview_output";
			this.textview_output.Editable = false;
			this.GtkScrolledWindow.Add(this.textview_output);
			this.eventbox_background.Add(this.GtkScrolledWindow);
			base.Add(this.eventbox_background);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 605;
			base.DefaultHeight = 442;
			base.Hide();
		}

		// Token: 0x04000444 RID: 1092
		private CocosMonitor monitor;

		// Token: 0x04000445 RID: 1093
		private System.Diagnostics.Process process;

		// Token: 0x04000446 RID: 1094
		private bool hasDestroyed = false;

		// Token: 0x04000447 RID: 1095
		private EventBox eventbox_background;

		// Token: 0x04000448 RID: 1096
		private ScrolledWindow GtkScrolledWindow;

		// Token: 0x04000449 RID: 1097
		private TextView textview_output;
	}
}
