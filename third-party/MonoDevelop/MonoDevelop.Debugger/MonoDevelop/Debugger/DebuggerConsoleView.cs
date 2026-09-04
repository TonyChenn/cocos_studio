using System;
using System.Collections.Generic;
using Gdk;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Components;
using MonoDevelop.Ide;
using MonoDevelop.Ide.CodeCompletion;

namespace MonoDevelop.Debugger
{
	public class DebuggerConsoleView : ConsoleView, ICompletionWidget
	{
		private Mono.Debugging.Client.CompletionData currentCompletionData;

		private TextMark tokenBeginMark;

		private CodeCompletionContext ctx;

		private ModifierType modifier;

		private bool keyHandled;

		private uint keyValue;

		private char keyChar;

		private Gdk.Key key;

		private static readonly string[] SyntaxTokens = new string[28]
		{
			"=", "+=", "-=", "*=", "/=", "%=", "&=", "|=", "~=", "+",
			"-", "*", "/", "%", "&", "|", "~", "==", "!=", ">",
			">=", "<", "<=", "(", ")", "[", "]", ","
		};

		private EventHandler completionContextChanged;

		public bool Editable
		{
			get
			{
				return base.TextView.Editable;
			}
			set
			{
				base.TextView.CursorVisible = value;
				base.TextView.Editable = value;
			}
		}

		private string TokenText
		{
			get
			{
				return base.Buffer.GetText(TokenBegin, TokenEnd, include_hidden_chars: false);
			}
			set
			{
				TextIter start = TokenBegin;
				TextIter end = TokenEnd;
				base.Buffer.Delete(ref start, ref end);
				start = TokenBegin;
				base.Buffer.Insert(ref start, value);
			}
		}

		private TextIter TokenBegin => base.Buffer.GetIterAtMark(tokenBeginMark);

		private TextIter TokenEnd => base.Cursor;

		private int Position => base.Cursor.Offset - TokenBegin.Offset;

		CodeCompletionContext ICompletionWidget.CurrentCodeCompletionContext => ((ICompletionWidget)this).CreateCodeCompletionContext(Position);

		int ICompletionWidget.CaretOffset => Position;

		int ICompletionWidget.TextLength => TokenText.Length;

		int ICompletionWidget.SelectedLength => 0;

		Style ICompletionWidget.GtkStyle => base.Style;

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

		public DebuggerConsoleView()
		{
			SetFont(IdeApp.Preferences.CustomOutputPadFont);
			base.TextView.KeyReleaseEvent += OnEditKeyRelease;
			IdeApp.Preferences.CustomOutputPadFontChanged += OnCustomOutputPadFontChanged;
			CompletionWindowManager.WindowClosed += OnCompletionWindowClosed;
		}

		private static bool IsCompletionChar(char c)
		{
			if (!char.IsLetterOrDigit(c) && !char.IsPunctuation(c) && !char.IsSymbol(c))
			{
				return char.IsWhiteSpace(c);
			}
			return true;
		}

		private static Mono.Debugging.Client.CompletionData GetCompletionData(string exp)
		{
			if (DebuggingService.CurrentFrame != null)
			{
				return DebuggingService.CurrentFrame.GetExpressionCompletionData(exp);
			}
			return null;
		}

		private void OnCompletionWindowClosed(object sender, EventArgs e)
		{
			currentCompletionData = null;
		}

		private void PopupCompletion()
		{
			Application.Invoke(delegate
			{
				char c = (char)Keyval.ToUnicode(keyValue);
				if (currentCompletionData == null && IsCompletionChar(c))
				{
					string text = base.Buffer.GetText(TokenBegin, base.Cursor, include_hidden_chars: false);
					currentCompletionData = GetCompletionData(text);
					if (currentCompletionData != null)
					{
						DebugCompletionDataList list = new DebugCompletionDataList(currentCompletionData);
						ctx = ((ICompletionWidget)this).CreateCodeCompletionContext(text.Length - currentCompletionData.ExpressionLength);
						CompletionWindowManager.ShowWindow(null, c, list, this, ctx);
					}
					else
					{
						currentCompletionData = null;
					}
				}
			});
		}

		private static bool EatWhitespace(string text, ref int index)
		{
			while (index < text.Length && char.IsWhiteSpace(text[index]))
			{
				index++;
			}
			return index < text.Length;
		}

		private static bool EatIdentifier(string text, ref int index)
		{
			int num = index;
			if (index >= text.Length)
			{
				return false;
			}
			while (index < text.Length && (char.IsLetterOrDigit(text[index]) || text[index] == '_'))
			{
				index++;
			}
			return index > num;
		}

		private static bool EatLiteralString(string text, ref int index)
		{
			index++;
			if (index >= text.Length || text[index] != '"')
			{
				return false;
			}
			index++;
			while (index < text.Length)
			{
				if (text[index++] == '"' && index < text.Length && text[index] == '"')
				{
					index++;
				}
			}
			return index < text.Length;
		}

		private static bool EatQuotedString(string text, ref int index)
		{
			char c = text[index++];
			bool flag = false;
			while (index < text.Length)
			{
				if (flag)
				{
					flag = false;
				}
				else if (text[index] == '\\')
				{
					flag = true;
				}
				else if (text[index] == c)
				{
					index++;
					break;
				}
				index++;
			}
			return index < text.Length;
		}

		private static string ReadSyntaxToken(string text, ref int index)
		{
			if (index + 1 >= text.Length)
			{
				return null;
			}
			string text2 = text.Substring(index, Math.Min(text.Length - index, 2));
			string text3 = null;
			int num = 0;
			for (int i = 0; i < SyntaxTokens.Length; i++)
			{
				if (num >= 2)
				{
					break;
				}
				if (text2.StartsWith(SyntaxTokens[i], StringComparison.Ordinal) && SyntaxTokens[i].Length > num)
				{
					text3 = SyntaxTokens[i];
					num = text3.Length;
				}
			}
			if (text3 != null)
			{
				index += num;
			}
			return text3;
		}

		private void UpdateTokenBeginMarker()
		{
			string text = base.Buffer.GetText(base.InputLineBegin, base.Cursor, include_hidden_chars: false);
			Stack<string> stack = new Stack<string>();
			Stack<int> stack2 = new Stack<int>();
			int index = 0;
			if (!EatWhitespace(text, ref index))
			{
				return;
			}
			stack2.Push(index);
			while (EatIdentifier(text, ref index) && EatWhitespace(text, ref index))
			{
				if (text[index] == '.')
				{
					index++;
					continue;
				}
				if (text[index] == '@')
				{
					if (EatLiteralString(text, ref index))
					{
						continue;
					}
					break;
				}
				if (text[index] == '"' || text[index] == '\'')
				{
					if (EatQuotedString(text, ref index))
					{
						continue;
					}
					break;
				}
				string text2;
				while ((text2 = ReadSyntaxToken(text, ref index)) != null)
				{
					EatWhitespace(text, ref index);
					switch (text2)
					{
					case ")":
					case "]":
						if (stack.Contains(text2))
						{
							do
							{
								stack2.Pop();
							}
							while (stack.Pop() != text2);
						}
						break;
					case "(":
						stack.Push(")");
						stack2.Push(index);
						break;
					case "[":
						stack.Push("]");
						stack2.Push(index);
						break;
					default:
						stack.Push(text2);
						stack2.Push(index);
						break;
					}
				}
			}
			index = stack2.Peek();
			TextIter iterAtOffset = base.Buffer.GetIterAtOffset(base.InputLineBegin.Offset + index);
			base.Buffer.MoveMark(tokenBeginMark, iterAtOffset);
		}

		private void OnEditKeyRelease(object sender, KeyReleaseEventArgs args)
		{
			UpdateTokenBeginMarker();
			if (!keyHandled)
			{
				string text = TokenText;
				if (ctx != null)
				{
					text = text.Substring(Math.Max(0, Math.Min(ctx.TriggerOffset, text.Length)));
				}
				CompletionWindowManager.UpdateWordSelection(text);
				CompletionWindowManager.PostProcessKeyEvent(key, keyChar, modifier);
				PopupCompletion();
			}
		}

		protected override bool ProcessKeyPressEvent(KeyPressEventArgs args)
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
			if (currentCompletionData != null && (keyHandled = CompletionWindowManager.PreProcessKeyEvent(key, keyChar, modifier)))
			{
				return true;
			}
			return base.ProcessKeyPressEvent(args);
		}

		protected override void UpdateInputLineBegin()
		{
			if (tokenBeginMark == null)
			{
				tokenBeginMark = base.Buffer.CreateMark(null, base.Buffer.EndIter, left_gravity: true);
			}
			else
			{
				base.Buffer.MoveMark(tokenBeginMark, base.Buffer.EndIter);
			}
			base.UpdateInputLineBegin();
		}

		string ICompletionWidget.GetText(int startOffset, int endOffset)
		{
			string tokenText = TokenText;
			if (startOffset < 0 || startOffset > tokenText.Length)
			{
				startOffset = 0;
			}
			if (endOffset > tokenText.Length)
			{
				endOffset = tokenText.Length;
			}
			return tokenText.Substring(startOffset, endOffset - startOffset);
		}

		void ICompletionWidget.Replace(int offset, int count, string text)
		{
			if (count > 0)
			{
				TokenText = TokenText.Remove(offset, count);
			}
			if (!string.IsNullOrEmpty(text))
			{
				TokenText = TokenText.Insert(offset, text);
			}
		}

		char ICompletionWidget.GetChar(int offset)
		{
			string tokenText = TokenText;
			if (offset >= tokenText.Length)
			{
				return '\0';
			}
			return tokenText[offset];
		}

		CodeCompletionContext ICompletionWidget.CreateCodeCompletionContext(int triggerOffset)
		{
			CodeCompletionContext codeCompletionContext = new CodeCompletionContext();
			codeCompletionContext.TriggerLine = 0;
			codeCompletionContext.TriggerOffset = triggerOffset;
			codeCompletionContext.TriggerLineOffset = codeCompletionContext.TriggerOffset;
			codeCompletionContext.TriggerWordLength = currentCompletionData.ExpressionLength;
			base.TextView.GdkWindow.GetOrigin(out var x, out var y);
			base.TextView.GetLineYrange(base.Cursor, out var y2, out var height);
			Rectangle iterLocation = GetIterLocation(base.Cursor);
			codeCompletionContext.TriggerYCoord = y + y2 + height - (int)base.Vadjustment.Value;
			codeCompletionContext.TriggerXCoord = x + iterLocation.X;
			codeCompletionContext.TriggerTextHeight = height;
			return codeCompletionContext;
		}

		string ICompletionWidget.GetCompletionText(CodeCompletionContext ctx)
		{
			return TokenText.Substring(ctx.TriggerOffset, ctx.TriggerWordLength);
		}

		void ICompletionWidget.SetCompletionText(CodeCompletionContext ctx, string partial_word, string complete_word)
		{
			int num = Position - partial_word.Length;
			TextIter start = base.Buffer.GetIterAtOffset(TokenBegin.Offset + num);
			TextIter end = base.Buffer.GetIterAtOffset(start.Offset + partial_word.Length);
			base.Buffer.Delete(ref start, ref end);
			base.Buffer.Insert(ref start, complete_word);
			base.Buffer.PlaceCursor(start);
		}

		void ICompletionWidget.SetCompletionText(CodeCompletionContext ctx, string partial_word, string complete_word, int offset)
		{
			int num = Position - partial_word.Length;
			TextIter start = base.Buffer.GetIterAtOffset(TokenBegin.Offset + num);
			TextIter end = base.Buffer.GetIterAtOffset(start.Offset + partial_word.Length);
			base.Buffer.Delete(ref start, ref end);
			base.Buffer.Insert(ref start, complete_word);
			TextIter iterAtOffset = base.Buffer.GetIterAtOffset(start.Offset + offset);
			base.Buffer.PlaceCursor(iterAtOffset);
		}

		private void OnCustomOutputPadFontChanged(object sender, EventArgs e)
		{
			SetFont(IdeApp.Preferences.CustomOutputPadFont);
		}

		protected override void OnDestroyed()
		{
			IdeApp.Preferences.CustomOutputPadFontChanged -= OnCustomOutputPadFontChanged;
			CompletionWindowManager.WindowClosed -= OnCompletionWindowClosed;
			CompletionWindowManager.HideWindow();
			base.OnDestroyed();
		}
	}
}
