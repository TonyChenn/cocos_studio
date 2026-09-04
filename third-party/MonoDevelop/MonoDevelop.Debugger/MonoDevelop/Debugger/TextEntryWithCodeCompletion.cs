using System;
using System.Collections.Generic;
using GLib;
using Gdk;
using Gtk;
using MonoDevelop.Ide.CodeCompletion;
using Xwt;

namespace MonoDevelop.Debugger
{
	internal class TextEntryWithCodeCompletion : TextEntry, ICompletionWidget
	{
		private class NullDotKeyHandler : ICompletionKeyHandler
		{
			public bool PreProcessKey(CompletionListWindow listWindow, Gdk.Key key, char keyChar, ModifierType modifier, out KeyActions keyAction)
			{
				keyAction = KeyActions.None;
				if (keyChar == '.')
				{
					return true;
				}
				return false;
			}

			public bool PostProcessKey(CompletionListWindow listWindow, Gdk.Key key, char keyChar, ModifierType modifier, out KeyActions keyAction)
			{
				keyAction = KeyActions.None;
				if (keyChar == '.')
				{
					return true;
				}
				return false;
			}
		}

		private CodeCompletionContext ctx;

		private Entry gtkEntry;

		private ModifierType modifier;

		private bool keyHandled;

		private uint keyValue;

		private char keyChar;

		private Gdk.Key key;

		private CompletionDataList list;

		public CodeCompletionContext CurrentCodeCompletionContext => CreateCodeCompletionContext(CaretOffset);

		public int CaretOffset => gtkEntry.Position;

		public int TextLength => base.Text.Length;

		public int SelectedLength => 0;

		public Style GtkStyle => gtkEntry.Style;

		public event EventHandler CompletionContextChanged;

		public TextEntryWithCodeCompletion()
		{
			gtkEntry = Toolkit.CurrentEngine.GetNativeWidget(this) as Entry;
			if (gtkEntry == null)
			{
				throw new NotImplementedException();
			}
			gtkEntry.KeyReleaseEvent += HandleKeyReleaseEvent;
			gtkEntry.KeyPressEvent += HandleKeyPressEvent;
			CompletionWindowManager.WindowClosed += HandleWindowClosed;
		}

		private void HandleWindowClosed(object sender, EventArgs e)
		{
			ctx = null;
			if (CompletionContextChanged != null)
			{
				CompletionContextChanged(this, EventArgs.Empty);
			}
		}

		[ConnectBefore]
		private void HandleKeyPressEvent(object o, KeyPressEventArgs args)
		{
			keyHandled = false;
			keyChar = (char)args.Event.Key;
			keyValue = args.Event.KeyValue;
			modifier = args.Event.State;
			key = args.Event.Key;
			if (args.Event.Key == Gdk.Key.Down || args.Event.Key == Gdk.Key.Up)
			{
				keyChar = '\0';
			}
			if (list != null)
			{
				args.RetVal = (keyHandled = CompletionWindowManager.PreProcessKeyEvent(key, keyChar, modifier));
			}
		}

		private void HandleKeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if (!keyHandled)
			{
				string text = ((ctx == null) ? base.Text : base.Text.Substring(Math.Max(0, Math.Min(ctx.TriggerOffset, base.Text.Length))));
				CompletionWindowManager.UpdateWordSelection(text);
				CompletionWindowManager.PostProcessKeyEvent(key, keyChar, modifier);
				PopupCompletion();
			}
		}

		private void PopupCompletion()
		{
			char firstChar = (char)Keyval.ToUnicode(keyValue);
			if (ctx == null)
			{
				ctx = ((ICompletionWidget)this).CreateCodeCompletionContext(0);
				CompletionWindowManager.ShowWindow(null, firstChar, list, this, ctx);
				if (CompletionContextChanged != null)
				{
					CompletionContextChanged(this, EventArgs.Empty);
				}
			}
		}

		public void SetCodeCompletionList(IList<string> list)
		{
			this.list = new CompletionDataList();
			foreach (string item in list)
			{
				this.list.Add(item);
			}
			this.list.DefaultCompletionString = "System.Exception";
			this.list.AddKeyHandler(new NullDotKeyHandler());
		}

		public string GetText(int startOffset, int endOffset)
		{
			if (startOffset < 0 || startOffset > base.Text.Length)
			{
				startOffset = 0;
			}
			if (endOffset > base.Text.Length)
			{
				endOffset = base.Text.Length;
			}
			return base.Text.Substring(startOffset, endOffset - startOffset);
		}

		public char GetChar(int offset)
		{
			if (offset >= base.Text.Length)
			{
				return '\0';
			}
			return base.Text[offset];
		}

		protected override void OnLostFocus(EventArgs args)
		{
			base.OnLostFocus(args);
			CompletionWindowManager.HideWindow();
		}

		public void Replace(int offset, int count, string text)
		{
			if (count > 0)
			{
				base.Text = base.Text.Remove(offset, count);
			}
			if (!string.IsNullOrEmpty(text))
			{
				base.Text = base.Text.Insert(offset, text);
			}
		}

		public CodeCompletionContext CreateCodeCompletionContext(int triggerOffset)
		{
			CodeCompletionContext codeCompletionContext = new CodeCompletionContext();
			codeCompletionContext.TriggerLine = 0;
			codeCompletionContext.TriggerOffset = triggerOffset;
			codeCompletionContext.TriggerLineOffset = codeCompletionContext.TriggerOffset;
			codeCompletionContext.TriggerTextHeight = gtkEntry.SizeRequest().Height;
			codeCompletionContext.TriggerWordLength = CaretOffset;
			Xwt.Point point = ConvertToScreenCoordinates(new Xwt.Point(0.0, codeCompletionContext.TriggerTextHeight));
			codeCompletionContext.TriggerXCoord = (int)point.X;
			codeCompletionContext.TriggerYCoord = (int)point.Y;
			return codeCompletionContext;
		}

		public string GetCompletionText(CodeCompletionContext ctx)
		{
			return base.Text.Substring(ctx.TriggerOffset, ctx.TriggerWordLength);
		}

		public void SetCompletionText(CodeCompletionContext ctx, string partial_word, string complete_word)
		{
			base.Text = complete_word;
			gtkEntry.Position = complete_word.Length;
		}

		public void SetCompletionText(CodeCompletionContext ctx, string partial_word, string complete_word, int completeWordOffset)
		{
			base.Text = complete_word;
			gtkEntry.Position = complete_word.Length;
		}
	}
}
