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
	// Token: 0x02000005 RID: 5
	public class OutputPadWidget : EventBox, IOutputPad, IService
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002389 File Offset: 0x00000589
		public OutputPadWidget()
		{
			this.Build();
			this.InitContextMenu();
			this.InitEvent();
			Services.RegisterService<IOutputPad>(this);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000023B0 File Offset: 0x000005B0
		private void Build()
		{
			this.outputTip = new OutputTipUC();
			this.scrolledWnd = new ScrolledWindow();
			base.Add(this.scrolledWnd);
			this.textView = new TextView();
			this.textView.Editable = false;
			this.scrolledWnd.Add(this.textView);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000240C File Offset: 0x0000060C
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

		// Token: 0x06000010 RID: 16 RVA: 0x00002498 File Offset: 0x00000698
		private void InitEvent()
		{
			this.textView.PopulatePopup += this.TextViewPopulatePopupHandler;
			this.textView.ButtonPressEvent += this.TextViewButtonPressedHandler;
			this.textView.ButtonReleaseEvent += this.TextViewButtonReleaseHandler;
			LogConfig.Output.Output += this.OutputRecivedHandler;
			LogConfig.OutputWithoutTip.Output += this.OutputRecivedHandler;
			Services.ProjectOperations.CurrentProjectChanged += this.CurrentDocumentChangedHandler;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002558 File Offset: 0x00000758
		public void Clear()
		{
			GLib.Timeout.Add(0U, delegate
			{
				this.textView.Buffer.Clear();
				return false;
			});
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000025BB File Offset: 0x000007BB
		public void ScrollToEnd()
		{
			GLib.Timeout.Add(50U, delegate
			{
				this.textView.ScrollToIter(this.textView.Buffer.EndIter, 0.0, false, 0.0, 0.0);
				return false;
			});
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002670 File Offset: 0x00000870
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

		// Token: 0x06000014 RID: 20 RVA: 0x000026A8 File Offset: 0x000008A8
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

		// Token: 0x06000015 RID: 21 RVA: 0x00002704 File Offset: 0x00000904
		[CommandHandler(CmdEnum.SelectAllCmd)]
		private void SelectAllCommandHandler()
		{
			TextIter startIter = this.textView.Buffer.StartIter;
			TextIter endIter = this.textView.Buffer.EndIter;
			this.textView.Buffer.SelectRange(startIter, endIter);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002748 File Offset: 0x00000948
		public override void Dispose()
		{
			LogConfig.Output.Output -= this.OutputRecivedHandler;
			LogConfig.OutputWithoutTip.Output -= this.OutputRecivedHandler;
			Services.ProjectOperations.CurrentProjectChanged -= this.CurrentDocumentChangedHandler;
			base.Dispose();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000027A2 File Offset: 0x000009A2
		private void ClearMenuItemActivated(object sender, EventArgs e)
		{
			this.Clear();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000027AC File Offset: 0x000009AC
		private void OutputRecivedHandler(string obj)
		{
			this.AppendText(obj);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000027B7 File Offset: 0x000009B7
		private void CurrentDocumentChangedHandler(object sender, ProjectsOperations.ProjectEventArgs e)
		{
			this.Clear();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000027C1 File Offset: 0x000009C1
		private void TextViewPopulatePopupHandler(object o, PopulatePopupArgs args)
		{
			args.Menu.Detach();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000027D0 File Offset: 0x000009D0
		private void TextViewButtonPressedHandler(object o, ButtonPressEventArgs args)
		{
			this.contextMenu.Popdown();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000027DF File Offset: 0x000009DF
		private void TextViewButtonReleaseHandler(object o, ButtonReleaseEventArgs args)
		{
			Services.CommandService.ShowContextMenu(this, args.Event, this.contextMenu, null);
		}

		// Token: 0x04000004 RID: 4
		private ScrolledWindow scrolledWnd;

		// Token: 0x04000005 RID: 5
		private TextView textView;

		// Token: 0x04000006 RID: 6
		private Menu contextMenu;

		// Token: 0x04000007 RID: 7
		private OutputTipUC outputTip;
	}
}
