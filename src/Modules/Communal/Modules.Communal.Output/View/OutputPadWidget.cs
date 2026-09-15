using System;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using Gdk;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Components.Commands;

namespace Modules.Communal.Output.View
{
	public class OutputPadWidget : EventBox, IOutputPad, IService
	{
		public OutputPadWidget()
		{
			this.Build();
			this.InitContextMenu();
			this.InitEvent();
			Services.RegisterService<IOutputPad>(this);
		}

		private void Build()
		{
			this.outputTip = new OutputTipUC();
			this.scrolledWnd = new ScrolledWindow();
			base.Add(this.scrolledWnd);
			this.textView = new TextView();
			this.textView.Editable = false;
			this.scrolledWnd.Add(this.textView);
		}

		private void InitContextMenu()
		{
			this.contextMenu = MenuCreator.CreatePopupMenu();
			MenuItem menuItem = new MenuItem(LanguageInfo.Command_ClearAll);
			MenuItem child = MenuCreator.CreateMenuItem(GlobalCommand.SelectAllCmd, false, null);
			MenuItem child2 = MenuCreator.CreateMenuItem(GlobalCommand.CopyCmd, false, null);
			this.contextMenu.Append(menuItem);
			this.contextMenu.Append(MenuCreator.CreateMenuSeperator());
			this.contextMenu.Append(child);
			this.contextMenu.Append(child2);
			menuItem.Activated += this.ClearMenuItemActivated;
		}

		private void InitEvent()
		{
			this.textView.PopulatePopup += this.TextViewPopulatePopupHandler;
			this.textView.ButtonPressEvent += this.TextViewButtonPressedHandler;
			this.textView.ButtonReleaseEvent += this.TextViewButtonReleaseHandler;
			LogConfig.Output.Output += this.OutputRecivedHandler;
			LogConfig.OutputWithoutTip.Output += this.OutputRecivedHandler;
			Services.ProjectOperations.CurrentProjectChanged += this.CurrentDocumentChangedHandler;
		}

		public void Clear()
		{
			GLib.Timeout.Add(0U, delegate
			{
				this.textView.Buffer.Clear();
				return false;
			});
		}

		public void ScrollToEnd()
		{
			GLib.Timeout.Add(50U, delegate
			{
				this.textView.ScrollToIter(this.textView.Buffer.EndIter, 0.0, false, 0.0, 0.0);
				return false;
			});
		}

		private void AppendText(string text)
		{
			GLib.Timeout.Add(0U, delegate
			{
				TextIter endIter = this.textView.Buffer.EndIter;
				this.textView.Buffer.Insert(ref endIter, text + "\n");
				this.textView.ScrollToIter(this.textView.Buffer.EndIter, 0.0, false, 0.0, 0.0);
				return false;
			});
		}

		[CommandHandler(CmdEnum.CopyCmd)]
		private void CopyCommandHandler()
		{
			TextIter start;
			TextIter end;
			if (this.textView.Buffer.GetSelectionBounds(out start, out end))
			{
				string text = this.textView.Buffer.GetText(start, end, true);
				Clipboard clipboard = this.textView.GetClipboard(Gdk.Selection.Clipboard);
				clipboard.Text = text;
			}
		}

		[CommandHandler(CmdEnum.SelectAllCmd)]
		private void SelectAllCommandHandler()
		{
			TextIter startIter = this.textView.Buffer.StartIter;
			TextIter endIter = this.textView.Buffer.EndIter;
			this.textView.Buffer.SelectRange(startIter, endIter);
		}

		public override void Dispose()
		{
			LogConfig.Output.Output -= this.OutputRecivedHandler;
			LogConfig.OutputWithoutTip.Output -= this.OutputRecivedHandler;
			Services.ProjectOperations.CurrentProjectChanged -= this.CurrentDocumentChangedHandler;
			base.Dispose();
		}

		private void ClearMenuItemActivated(object sender, EventArgs e)
		{
			this.Clear();
		}

		private void OutputRecivedHandler(string obj)
		{
			this.AppendText(obj);
		}

		private void CurrentDocumentChangedHandler(object sender, ProjectsOperations.ProjectEventArgs e)
		{
			this.Clear();
		}

		private void TextViewPopulatePopupHandler(object o, PopulatePopupArgs args)
		{
			args.Menu.Detach();
		}

		private void TextViewButtonPressedHandler(object o, ButtonPressEventArgs args)
		{
			this.contextMenu.Popdown();
		}

		private void TextViewButtonReleaseHandler(object o, ButtonReleaseEventArgs args)
		{
			Services.CommandService.ShowContextMenu(this, args.Event, this.contextMenu, null);
		}

		private ScrolledWindow scrolledWnd;

		private TextView textView;

		private Menu contextMenu;

		private OutputTipUC outputTip;
	}
}
