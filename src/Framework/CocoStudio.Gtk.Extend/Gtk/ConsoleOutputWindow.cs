using System;
using System.Diagnostics;
using CocoStudio.Basic;
using GLib;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Stetic;

namespace Gtk
{
	public class ConsoleOutputWindow : Window
	{
		public ConsoleOutputWindow() : base(WindowType.Toplevel)
		{
			this.Build();
			base.Title = LanguageInfo.Run_Console;
			base.Destroyed += this.DestroyedHandler;
		}

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

		private void FinishedHandler(object sender, FinishedArgs e)
		{
			this.CloseWindow();
		}

		private void DestroyedHandler(object sender, EventArgs e)
		{
			if (this.monitor != null && this.monitor.IsProcessing)
			{
				this.monitor.Finish(true);
			}
		}

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

		private CocosMonitor monitor;

		private System.Diagnostics.Process process;

		private bool hasDestroyed = false;

		private EventBox eventbox_background;

		private ScrolledWindow GtkScrolledWindow;

		private TextView textview_output;
	}
}
