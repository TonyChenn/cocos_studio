using System;
using GLib;
using Gdk;
using Gtk;
using Mono.Debugging.Client;
using Mono.Unix;
using MonoDevelop.Ide.CodeCompletion;
using Pango;
using Stetic;

namespace MonoDevelop.Debugger
{
	public class ExpressionEvaluatorDialog : Dialog, ICompletionWidget
	{
		private Mono.Debugging.Client.CompletionData currentCompletionData;

		private CodeCompletionContext ctx;

		private ModifierType modifier;

		private bool keyHandled;

		private uint keyValue;

		private char keyChar;

		private Gdk.Key key;

		private EventHandler completionContextChanged;

		private VBox vbox2;

		private HBox hbox1;

		private Entry entry;

		private Button buttonEval;

		private ScrolledWindow GtkScrolledWindow;

		private ObjectValueTreeView valueTree;

		private Button buttonOk;

		public string Expression
		{
			get
			{
				return entry.Text;
			}
			set
			{
				entry.Text = value;
				UpdateExpression();
			}
		}

		CodeCompletionContext ICompletionWidget.CurrentCodeCompletionContext => ((ICompletionWidget)this).CreateCodeCompletionContext(entry.Position);

		int ICompletionWidget.CaretOffset => entry.Position;

		int ICompletionWidget.TextLength => entry.Text.Length;

		int ICompletionWidget.SelectedLength => 0;

		Gtk.Style ICompletionWidget.GtkStyle => entry.Style;

		event EventHandler ICompletionWidget.CompletionContextChanged
		{
			add
			{
				completionContextChanged = (EventHandler)Delegate.Combine(completionContextChanged, value);
			}
			remove
			{
				completionContextChanged = (EventHandler)Delegate.Remove(completionContextChanged, value);
			}
		}

		public ExpressionEvaluatorDialog()
		{
			Build();
			valueTree.Frame = DebuggingService.CurrentFrame;
			valueTree.AllowExpanding = true;
			entry.KeyReleaseEvent += OnEditKeyRelease;
			entry.KeyPressEvent += OnEditKeyPress;
			entry.FocusOutEvent += OnEditFocusOut;
			CompletionWindowManager.WindowClosed += HandleCompletionWindowClosed;
		}

		protected override void OnDestroyed()
		{
			CompletionWindowManager.WindowClosed -= HandleCompletionWindowClosed;
			CompletionWindowManager.HideWindow();
			base.OnDestroyed();
		}

		private void UpdateExpression()
		{
			valueTree.ClearValues();
			valueTree.ClearExpressions();
			if (entry.Text.Length > 0)
			{
				valueTree.AddExpression(entry.Text);
			}
		}

		protected virtual void OnButtonEvalClicked(object sender, EventArgs e)
		{
			CompletionWindowManager.HideWindow();
			UpdateExpression();
		}

		private void HandleCompletionWindowClosed(object sender, EventArgs e)
		{
			currentCompletionData = null;
		}

		private void PopupCompletion(Entry entry)
		{
			Application.Invoke(delegate
			{
				char c = (char)Keyval.ToUnicode(keyValue);
				if (currentCompletionData == null && IsCompletionChar(c))
				{
					string exp = entry.Text.Substring(0, entry.CursorPosition);
					currentCompletionData = GetCompletionData(exp);
					if (currentCompletionData != null)
					{
						DebugCompletionDataList list = new DebugCompletionDataList(currentCompletionData);
						ctx = ((ICompletionWidget)this).CreateCodeCompletionContext(entry.CursorPosition - currentCompletionData.ExpressionLength);
						CompletionWindowManager.ShowWindow(null, c, list, this, ctx);
					}
					else
					{
						currentCompletionData = null;
					}
				}
			});
		}

		private void OnEditKeyRelease(object sender, EventArgs e)
		{
			if (!keyHandled)
			{
				string text = ((ctx == null) ? entry.Text : entry.Text.Substring(Math.Max(0, Math.Min(ctx.TriggerOffset, entry.Text.Length))));
				CompletionWindowManager.UpdateWordSelection(text);
				CompletionWindowManager.PostProcessKeyEvent(key, keyChar, modifier);
				PopupCompletion((Entry)sender);
			}
		}

		[ConnectBefore]
		private void OnEditKeyPress(object sender, KeyPressEventArgs args)
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
			if (currentCompletionData != null)
			{
				args.RetVal = (keyHandled = CompletionWindowManager.PreProcessKeyEvent(key, keyChar, modifier));
			}
		}

		private void OnEditFocusOut(object sender, FocusOutEventArgs args)
		{
			CompletionWindowManager.HideWindow();
		}

		private static bool IsCompletionChar(char c)
		{
			if (!char.IsLetterOrDigit(c) && !char.IsPunctuation(c) && !char.IsSymbol(c))
			{
				return char.IsWhiteSpace(c);
			}
			return true;
		}

		private Mono.Debugging.Client.CompletionData GetCompletionData(string exp)
		{
			if (valueTree.Frame != null)
			{
				return valueTree.Frame.GetExpressionCompletionData(exp);
			}
			return null;
		}

		string ICompletionWidget.GetText(int startOffset, int endOffset)
		{
			if (startOffset < 0 || startOffset > entry.Text.Length)
			{
				startOffset = 0;
			}
			if (endOffset > entry.Text.Length)
			{
				endOffset = entry.Text.Length;
			}
			return entry.Text.Substring(startOffset, endOffset - startOffset);
		}

		void ICompletionWidget.Replace(int offset, int count, string text)
		{
			if (count > 0)
			{
				entry.Text = entry.Text.Remove(offset, count);
			}
			if (!string.IsNullOrEmpty(text))
			{
				entry.Text = entry.Text.Insert(offset, text);
			}
		}

		char ICompletionWidget.GetChar(int offset)
		{
			string text = entry.Text;
			if (offset >= text.Length)
			{
				return '\0';
			}
			return text[offset];
		}

		CodeCompletionContext ICompletionWidget.CreateCodeCompletionContext(int triggerOffset)
		{
			CodeCompletionContext codeCompletionContext = new CodeCompletionContext();
			codeCompletionContext.TriggerLine = 0;
			codeCompletionContext.TriggerOffset = triggerOffset;
			codeCompletionContext.TriggerLineOffset = codeCompletionContext.TriggerOffset;
			codeCompletionContext.TriggerTextHeight = entry.SizeRequest().Height;
			codeCompletionContext.TriggerWordLength = currentCompletionData.ExpressionLength;
			entry.GdkWindow.GetOrigin(out var x, out var y);
			entry.GetLayoutOffsets(out var x2, out var _);
			int index_ = entry.TextIndexToLayoutIndex(entry.Position);
			Pango.Rectangle rectangle = entry.Layout.IndexToPos(index_);
			x2 += Units.ToPixels(rectangle.X) + x;
			y += entry.Allocation.Height;
			codeCompletionContext.TriggerXCoord = x2;
			codeCompletionContext.TriggerYCoord = y;
			return codeCompletionContext;
		}

		string ICompletionWidget.GetCompletionText(CodeCompletionContext ctx)
		{
			return entry.Text.Substring(ctx.TriggerOffset, ctx.TriggerWordLength);
		}

		void ICompletionWidget.SetCompletionText(CodeCompletionContext ctx, string partial_word, string complete_word)
		{
			int position = entry.Position - partial_word.Length;
			entry.DeleteText(position, position + partial_word.Length);
			entry.InsertText(complete_word, ref position);
			entry.Position = position;
		}

		void ICompletionWidget.SetCompletionText(CodeCompletionContext ctx, string partial_word, string complete_word, int offset)
		{
			int position = entry.Position - partial_word.Length;
			entry.DeleteText(position, position + partial_word.Length);
			entry.InsertText(complete_word, ref position);
			entry.Position = position + offset;
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			base.Name = "MonoDevelop.Debugger.ExpressionEvaluatorDialog";
			base.Title = Catalog.GetString("Expression Evaluator");
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			VBox vBox = base.VBox;
			vBox.Name = "dialog1_VBox";
			vBox.BorderWidth = 2u;
			vbox2 = new VBox();
			vbox2.Name = "vbox2";
			vbox2.Spacing = 6;
			vbox2.BorderWidth = 9u;
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 6;
			entry = new Entry();
			entry.CanFocus = true;
			entry.Name = "entry";
			entry.IsEditable = true;
			entry.ActivatesDefault = true;
			entry.InvisibleChar = '●';
			hbox1.Add(entry);
			Box.BoxChild boxChild = (Box.BoxChild)hbox1[entry];
			boxChild.Position = 0;
			buttonEval = new Button();
			buttonEval.CanDefault = true;
			buttonEval.CanFocus = true;
			buttonEval.Name = "buttonEval";
			buttonEval.UseUnderline = true;
			buttonEval.Label = Catalog.GetString("Evaluate");
			hbox1.Add(buttonEval);
			Box.BoxChild boxChild2 = (Box.BoxChild)hbox1[buttonEval];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			vbox2.Add(hbox1);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox2[hbox1];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			GtkScrolledWindow = new ScrolledWindow();
			GtkScrolledWindow.Name = "GtkScrolledWindow";
			GtkScrolledWindow.ShadowType = ShadowType.In;
			valueTree = new ObjectValueTreeView();
			valueTree.CanFocus = true;
			valueTree.Name = "valueTree";
			valueTree.AllowAdding = false;
			valueTree.AllowEditing = false;
			valueTree.AllowPinning = false;
			valueTree.RootPinAlwaysVisible = false;
			valueTree.AllowExpanding = false;
			valueTree.PinnedWatchLine = 0;
			valueTree.CompactView = false;
			GtkScrolledWindow.Add(valueTree);
			vbox2.Add(GtkScrolledWindow);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox2[GtkScrolledWindow];
			boxChild4.Position = 1;
			vBox.Add(vbox2);
			Box.BoxChild boxChild5 = (Box.BoxChild)vBox[vbox2];
			boxChild5.Position = 0;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 6;
			actionArea.BorderWidth = 5u;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			buttonOk = new Button();
			buttonOk.CanDefault = true;
			buttonOk.CanFocus = true;
			buttonOk.Name = "buttonOk";
			buttonOk.UseStock = true;
			buttonOk.UseUnderline = true;
			buttonOk.Label = "gtk-close";
			AddActionWidget(buttonOk, -7);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[buttonOk];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 716;
			base.DefaultHeight = 410;
			buttonEval.HasDefault = true;
			Hide();
			buttonEval.Clicked += OnButtonEvalClicked;
		}
	}
}
