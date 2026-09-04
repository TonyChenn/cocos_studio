using System;
using System.Collections.Generic;
using System.Linq;
using Cairo;
using GLib;
using Gdk;
using Gtk;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.Addins;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using Mono.TextEditor.Vi;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Ide.CodeTemplates;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;
using Xwt.Drawing;

namespace MonoDevelop.SourceEditor
{
	public class ExtensibleTextEditor : TextEditor
	{
		internal object MemoryProbe = Counters.EditorsInMemory.CreateMemoryProbe();

		private SourceEditorView view;

		private ExtensionContext extensionContext;

		private Adjustment cachedHAdjustment;

		private Adjustment cachedVAdjustment;

		private static bool? testNewViMode;

		private bool isInKeyStroke;

		private int oldOffset = -1;

		private ITextEditorResolverProvider textEditorResolverProvider;

		public ITextEditorExtension Extension { get; set; }

		public new ISourceEditorOptions Options => (ISourceEditorOptions)base.Options;

		internal SourceEditorView View => view;

		public ExtensionContext ExtensionContext
		{
			get
			{
				return extensionContext;
			}
			set
			{
				if (extensionContext != null)
				{
					extensionContext.RemoveExtensionNodeHandler("MonoDevelop/SourceEditor2/TooltipProviders", OnTooltipProviderChanged);
					ClearTooltipProviders();
				}
				extensionContext = value;
				if (extensionContext != null)
				{
					extensionContext.AddExtensionNodeHandler("MonoDevelop/SourceEditor2/TooltipProviders", OnTooltipProviderChanged);
				}
			}
		}

		private static bool TestNewViMode
		{
			get
			{
				if (!testNewViMode.HasValue)
				{
					testNewViMode = Environment.GetEnvironmentVariable("TEST_NEW_VI_MODE") != null;
				}
				return testNewViMode.Value;
			}
		}

		private IEnumerable<char> TextWithoutCommentsAndStrings => from p in GetTextWithoutCommentsAndStrings(base.Document, 0, base.Document.TextLength)
			select p.Key;

		internal ParsedDocument ParsedDocument => IdeApp.Workbench.ActiveDocument?.ParsedDocument;

		public Project Project => IdeApp.Workbench.ActiveDocument?.Project;

		public ITextEditorResolverProvider TextEditorResolverProvider
		{
			get
			{
				return textEditorResolverProvider;
			}
			internal set
			{
				textEditorResolverProvider = value;
			}
		}

		static ExtensibleTextEditor()
		{
			testNewViMode = null;
			Xwt.Drawing.Image icon = Xwt.Drawing.Image.FromResource("gutter-bookmark-15.png");
			BookmarkMarker.DrawBookmarkFunc = delegate(TextEditor editor, Cairo.Context cr, DocumentLine lineSegment, double x, double y, double width, double height)
			{
				if (lineSegment.IsBookmarked)
				{
					cr.DrawImage(editor, icon, Math.Floor(x + (width - icon.Width) / 2.0), Math.Floor(y + (height - icon.Height) / 2.0));
				}
			};
		}

		public ExtensibleTextEditor(SourceEditorView view, ISourceEditorOptions options, TextDocument doc)
			: base(doc, options)
		{
			Initialize(view);
		}

		public ExtensibleTextEditor(SourceEditorView view)
		{
			base.Options = new StyledSourceEditorOptions(view.Project, null);
			Initialize(view);
		}

		private void Initialize(SourceEditorView view)
		{
			this.view = view;
			base.Caret.PositionChanged += delegate
			{
				if (Extension != null)
				{
					try
					{
						Extension.CursorPositionChanged();
					}
					catch (Exception ex)
					{
						ReportExtensionError(ex);
					}
				}
			};
			base.Document.TextReplaced += HandleSkipCharsOnReplace;
			UpdateEditMode();
			base.DoPopupMenu = ShowPopup;
		}

		private void HandleSkipCharsOnReplace(object sender, DocumentChangeEventArgs args)
		{
			List<TextEditorData.SkipChar> skipChars = GetTextEditorData().SkipChars;
			for (int i = 0; i < skipChars.Count; i++)
			{
				TextEditorData.SkipChar skipChar = skipChars[i];
				if (args.Offset > skipChar.Offset)
				{
					skipChars.RemoveAt(i);
					i--;
				}
				else if (args.Offset <= skipChar.Offset)
				{
					skipChar.Offset += args.ChangeDelta;
				}
			}
		}

		private void UpdateEditMode()
		{
			if (Options.UseViModes)
			{
				if (TestNewViMode)
				{
					if (!(base.CurrentMode is NewIdeViMode))
					{
						base.CurrentMode = new NewIdeViMode(this);
					}
				}
				else if (!(base.CurrentMode is IdeViMode))
				{
					base.CurrentMode = new IdeViMode(this);
				}
			}
			else
			{
				SimpleEditMode simpleEditMode = new SimpleEditMode();
				simpleEditMode.KeyBindings[EditMode.GetKeyCode(Gdk.Key.Tab)] = new TabAction(this).Action;
				simpleEditMode.KeyBindings[EditMode.GetKeyCode(Gdk.Key.BackSpace)] = EditActions.AdvancedBackspace;
				base.CurrentMode = simpleEditMode;
			}
		}

		private void UnregisterAdjustments()
		{
			if (cachedHAdjustment != null)
			{
				cachedHAdjustment.ValueChanged -= HAdjustment_ValueChanged;
			}
			if (cachedVAdjustment != null)
			{
				cachedVAdjustment.ValueChanged -= VAdjustment_ValueChanged;
			}
			cachedHAdjustment = null;
			cachedVAdjustment = null;
		}

		protected override void OnDestroyed()
		{
			UnregisterAdjustments();
			Extension = null;
			ExtensionContext = null;
			view = null;
			base.OnDestroyed();
			if (Options != null)
			{
				Options.Dispose();
				base.Options = null;
			}
		}

		private void OnTooltipProviderChanged(object s, ExtensionNodeEventArgs a)
		{
			TooltipProvider provider;
			try
			{
				provider = (TooltipProvider)a.ExtensionObject;
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Can't create tooltip provider:" + a.ExtensionNode, ex);
				return;
			}
			if (a.Change == ExtensionChange.Add)
			{
				AddTooltipProvider(provider);
			}
			else
			{
				RemoveTooltipProvider(provider);
			}
		}

		public void FireOptionsChange()
		{
			OptionsChanged(null, null);
		}

		protected override void OptionsChanged(object sender, EventArgs args)
		{
			if (view != null && view.Control != null && !Options.ShowFoldMargin)
			{
				base.Document.ClearFoldSegments();
			}
			UpdateEditMode();
			base.OptionsChanged(sender, args);
		}

		protected override bool OnKeyPressEvent(EventKey evnt)
		{
			isInKeyStroke = true;
			try
			{
				return base.OnKeyPressEvent(evnt);
			}
			finally
			{
				isInKeyStroke = false;
			}
		}

		private bool ExtensionKeyPress(Gdk.Key key, uint ch, ModifierType state)
		{
			isInKeyStroke = true;
			try
			{
				return Extension.KeyPress(key, (char)ch, state);
			}
			catch (Exception ex)
			{
				ReportExtensionError(ex);
			}
			finally
			{
				isInKeyStroke = false;
			}
			return false;
		}

		private void ReportExtensionError(Exception ex)
		{
			LoggingService.LogInternalError("Error in text editor extension chain", ex);
		}

		private static IEnumerable<KeyValuePair<char, int>> GetTextWithoutCommentsAndStrings(TextDocument doc, int start, int end)
		{
			bool isInString = false;
			bool isInChar = false;
			bool isInLineComment = false;
			bool isInBlockComment = false;
			for (int pos = start; pos < end; pos++)
			{
				char ch = doc.GetCharAt(pos);
				switch (ch)
				{
				case '\n':
				case '\r':
					isInLineComment = false;
					break;
				case '/':
					if (isInBlockComment)
					{
						if (pos > 0 && doc.GetCharAt(pos - 1) == '*')
						{
							isInBlockComment = false;
						}
					}
					else if (!isInString && !isInChar && pos + 1 < doc.TextLength)
					{
						char charAt = doc.GetCharAt(pos + 1);
						if (charAt == '/')
						{
							isInLineComment = true;
						}
						if (!isInLineComment && charAt == '*')
						{
							isInBlockComment = true;
						}
					}
					break;
				case '"':
					if (!isInChar && !isInLineComment && !isInBlockComment)
					{
						isInString = !isInString;
					}
					break;
				case '\'':
					if (!isInString && !isInLineComment && !isInBlockComment)
					{
						isInChar = !isInChar;
					}
					break;
				default:
					if (!isInString && !isInChar && !isInLineComment && !isInBlockComment)
					{
						yield return new KeyValuePair<char, int>(ch, pos);
					}
					break;
				}
			}
		}

		protected override bool OnIMProcessedKeyPressEvent(Gdk.Key key, uint ch, ModifierType state)
		{
			bool result = true;
			if (key == Gdk.Key.Escape)
			{
				if ((Extension != null) ? ExtensionKeyPress(key, ch, state) : base.OnIMProcessedKeyPressEvent(key, ch, state))
				{
					view.SourceEditorWidget.RemoveSearchWidget();
					return true;
				}
				return false;
			}
			if (base.Document == null)
			{
				return true;
			}
			bool flag = false;
			bool flag2 = false;
			DocumentLine line = base.Document.GetLine(base.Caret.Line);
			if (line == null)
			{
				return true;
			}
			CloneableStack<Mono.TextEditor.Highlighting.Span> cloneableStack = line.StartSpan.Clone();
			if (base.Document.SyntaxMode is SyntaxMode syntaxMode)
			{
				Mono.TextEditor.Highlighting.SyntaxModeService.ScanSpans(base.Document, syntaxMode, syntaxMode, cloneableStack, line.Offset, base.Caret.Offset);
			}
			foreach (Mono.TextEditor.Highlighting.Span item in cloneableStack)
			{
				if (!string.IsNullOrEmpty(item.Color) && (item.Color.StartsWith("String", StringComparison.Ordinal) || item.Color.StartsWith("Comment", StringComparison.Ordinal) || item.Color.StartsWith("Xml Attribute Value", StringComparison.Ordinal)))
				{
					flag2 = !item.Color.StartsWith("Comment", StringComparison.Ordinal);
					flag = true;
					break;
				}
			}
			bool flag3 = false;
			bool flag4 = key == Gdk.Key.Return && (state & (ModifierType.ShiftMask | ModifierType.ControlMask)) == 0 && base.Caret.Offset > 0 && base.Caret.Offset < base.Document.TextLength && base.Document.GetCharAt(base.Caret.Offset - 1) == '{' && base.Document.GetCharAt(base.Caret.Offset) == '}' && !flag;
			int num = "{[('\"".IndexOf((char)ch);
			List<TextEditorData.SkipChar> skipChars = GetTextEditorData().SkipChars;
			TextEditorData.SkipChar skipChar = skipChars.Find((TextEditorData.SkipChar sc) => sc.Char == (ushort)ch && sc.Offset == base.Caret.Offset);
			if (base.Caret.Offset > 0)
			{
				char charAt = base.Document.GetCharAt(base.Caret.Offset - 1);
				if (ch == 34 && ((!flag && charAt == '"') || (flag2 && charAt == '\\')))
				{
					skipChar = null;
					num = -1;
				}
			}
			char ch2 = '\0';
			bool flag5 = false;
			IDisposable disposable = null;
			if (skipChar == null && Options.AutoInsertMatchingBracket && num >= 0 && !base.IsSomethingSelected)
			{
				if (!flag)
				{
					char c = "}])'\""[num];
					char c2 = "{[('\""[num];
					int num2 = 0;
					foreach (char textWithoutCommentsAndString in TextWithoutCommentsAndStrings)
					{
						if (textWithoutCommentsAndString == c2)
						{
							num2++;
						}
						else if (textWithoutCommentsAndString == c)
						{
							num2--;
						}
					}
					if (num2 >= 0)
					{
						flag5 = true;
						ch2 = c;
					}
				}
				else
				{
					char charAt2 = base.Document.GetCharAt(base.Caret.Offset - 1);
					if (!flag && ch == 34 && charAt2 != '\\')
					{
						flag5 = true;
						ch2 = '"';
					}
				}
			}
			if (flag5)
			{
				disposable = base.Document.OpenUndoGroup();
			}
			bool isInInsertMode = base.Caret.IsInInsertMode;
			if (skipChar != null)
			{
				base.Caret.IsInInsertMode = false;
				skipChars.Remove(skipChar);
			}
			if (Extension != null)
			{
				if (!DefaultSourceEditorOptions.Instance.GenerateFormattingUndoStep)
				{
					using (base.Document.OpenUndoGroup())
					{
						if (ExtensionKeyPress(key, ch, state))
						{
							result = base.OnIMProcessedKeyPressEvent(key, ch, state);
						}
					}
				}
				else if (ExtensionKeyPress(key, ch, state))
				{
					result = base.OnIMProcessedKeyPressEvent(key, ch, state);
				}
				if (flag4)
				{
					HitReturn();
				}
			}
			else
			{
				result = base.OnIMProcessedKeyPressEvent(key, ch, state);
				if (flag4)
				{
					HitReturn();
				}
			}
			if (skipChar != null)
			{
				base.Caret.IsInInsertMode = isInInsertMode;
			}
			if (flag5)
			{
				GetTextEditorData().EnsureCaretIsNotVirtual();
				int offset = base.Caret.Offset;
				base.Caret.AutoUpdatePosition = false;
				Insert(offset, ch2.ToString());
				base.Caret.AutoUpdatePosition = true;
				GetTextEditorData().SetSkipChar(offset, ch2);
				disposable.Dispose();
			}
			if (!flag3)
			{
				return result;
			}
			return true;
		}

		private void HitReturn()
		{
			int num = base.Caret.Offset - 1;
			while (num > 0 && char.IsWhiteSpace(GetCharAt(num - 1)))
			{
				num--;
			}
			base.Caret.Offset = num;
			ExtensionKeyPress(Gdk.Key.Return, 0u, ModifierType.None);
		}

		internal string GetErrorInformationAt(int offset)
		{
			DocumentLocation documentLocation = base.Document.OffsetToLocation(offset);
			DocumentLine line = base.Document.GetLine(documentLocation.Line);
			if (line == null)
			{
				return null;
			}
			if (line.Markers.FirstOrDefault((TextLineMarker m) => m is ErrorMarker) is ErrorMarker errorMarker)
			{
				if (errorMarker.Info.ErrorType == ErrorType.Warning)
				{
					return GettextCatalog.GetString("<b>Parser Warning</b>: {0}", Markup.EscapeText(errorMarker.Info.Message));
				}
				return GettextCatalog.GetString("<b>Parser Error</b>: {0}", Markup.EscapeText(errorMarker.Info.Message));
			}
			return null;
		}

		public ResolveResult GetLanguageItem(int offset, out DomRegion region)
		{
			oldOffset = offset;
			region = DomRegion.Empty;
			if (textEditorResolverProvider != null)
			{
				return textEditorResolverProvider.GetLanguageItem(view.WorkbenchWindow.Document, offset, out region);
			}
			return null;
		}

		public CodeTemplateContext GetTemplateContext()
		{
			if (base.IsSomethingSelected)
			{
				ResolveResult languageItem = GetLanguageItem(base.Caret.Offset, base.Document.GetTextAt(base.SelectionRange));
				if (languageItem != null && !languageItem.IsError)
				{
					return CodeTemplateContext.InExpression;
				}
			}
			return CodeTemplateContext.Standard;
		}

		public ResolveResult GetLanguageItem(int offset, string expression)
		{
			oldOffset = offset;
			if (textEditorResolverProvider != null)
			{
				return textEditorResolverProvider.GetLanguageItem(view.WorkbenchWindow.Document, offset, expression);
			}
			return null;
		}

		private string GetExpressionBeforeOffset(int offset)
		{
			int num = offset;
			while (num > 0 && IsIdChar(base.Document.GetCharAt(num)))
			{
				num--;
			}
			while (offset < base.Document.TextLength && IsIdChar(base.Document.GetCharAt(offset)))
			{
				offset++;
			}
			num++;
			if (offset - num > 0 && num < base.Document.TextLength)
			{
				return base.Document.GetTextAt(num, offset - num);
			}
			return string.Empty;
		}

		private bool IsIdChar(char c)
		{
			if (!char.IsLetterOrDigit(c))
			{
				return c == '_';
			}
			return true;
		}

		protected override bool OnFocusOutEvent(EventFocus evnt)
		{
			CompletionWindowManager.HideWindow();
			ParameterInformationWindowManager.HideWindow(null, view);
			return base.OnFocusOutEvent(evnt);
		}

		private void ShowPopup(EventButton evt)
		{
			view.FireCompletionContextChanged();
			CompletionWindowManager.HideWindow();
			ParameterInformationWindowManager.HideWindow(null, view);
			HideTooltip();
			ExtensionContext ctx = ExtensionContext ?? AddinManager.AddinEngine;
			CommandEntrySet entrySet = IdeApp.CommandService.CreateCommandEntrySet(ctx, "/MonoDevelop/SourceEditor2/ContextMenu/Editor");
			if (Platform.IsMac)
			{
				IdeApp.CommandService.ShowContextMenu(this, evt, entrySet, this);
				return;
			}
			Menu menu = IdeApp.CommandService.CreateMenu(entrySet);
			MenuItem menuItem = CreateInputMethodMenuItem(GettextCatalog.GetString("_Input Methods"));
			if (menuItem != null)
			{
				menu.Append(new SeparatorMenuItem());
				menu.Append(menuItem);
			}
			menu.Hidden += HandleMenuHidden;
			if (evt != null)
			{
				GtkWorkarounds.ShowContextMenu(menu, this, evt);
				return;
			}
			Cairo.Point point = LocationToPoint(base.Caret.Location);
			GtkWorkarounds.ShowContextMenu(menu, this, new Gdk.Rectangle(point.X, point.Y, 1, (int)base.LineHeight));
		}

		private void HandleMenuHidden(object sender, EventArgs e)
		{
			Menu menu = sender as Menu;
			menu.Hidden -= HandleMenuHidden;
			GLib.Timeout.Add(10u, delegate
			{
				menu.Destroy();
				return false;
			});
		}

		private int FindPrevWordStart(int offset)
		{
			while (--offset >= 0 && !char.IsWhiteSpace(base.Document.GetCharAt(offset)))
			{
			}
			return ++offset;
		}

		public string GetWordBeforeCaret()
		{
			int offset = base.Caret.Offset;
			int num = FindPrevWordStart(offset);
			return base.Document.GetTextAt(num, offset - num);
		}

		public bool IsTemplateKnown()
		{
			string wordBeforeCaret = GetWordBeforeCaret();
			bool result = false;
			foreach (CodeTemplate codeTemplate in CodeTemplateService.GetCodeTemplates(base.Document.MimeType))
			{
				if (codeTemplate.Shortcut == wordBeforeCaret)
				{
					result = true;
				}
				else if (codeTemplate.Shortcut.StartsWith(wordBeforeCaret))
				{
					result = false;
					break;
				}
			}
			return result;
		}

		public bool DoInsertTemplate()
		{
			string wordBeforeCaret = GetWordBeforeCaret();
			foreach (CodeTemplate codeTemplate in CodeTemplateService.GetCodeTemplates(base.Document.MimeType))
			{
				if (codeTemplate.Shortcut == wordBeforeCaret)
				{
					InsertTemplate(codeTemplate, view.WorkbenchWindow.Document);
					return true;
				}
			}
			return false;
		}

		internal void InsertTemplate(CodeTemplate template, Document document)
		{
			using (base.Document.OpenUndoGroup())
			{
				CodeTemplate.TemplateResult templateResult = template.InsertTemplateContents(document);
				List<TextLink> textLinks = templateResult.TextLinks;
				TextLinkEditMode textLinkEditMode = new TextLinkEditMode(this, templateResult.InsertPosition, textLinks);
				textLinkEditMode.TextLinkMode = TextLinkMode.General;
				if (textLinkEditMode.ShouldStartTextLinkMode)
				{
					textLinkEditMode.OldMode = base.CurrentMode;
					textLinkEditMode.StartMode();
					base.CurrentMode = textLinkEditMode;
				}
			}
		}

		protected override void OnScrollAdjustmentsSet()
		{
			UnregisterAdjustments();
			if (base.HAdjustment != null)
			{
				cachedHAdjustment = base.HAdjustment;
				base.HAdjustment.ValueChanged += HAdjustment_ValueChanged;
			}
			if (base.VAdjustment != null)
			{
				cachedVAdjustment = base.VAdjustment;
				base.VAdjustment.ValueChanged += VAdjustment_ValueChanged;
			}
		}

		private void VAdjustment_ValueChanged(object sender, EventArgs e)
		{
			CompletionWindowManager.HideWindow();
			ParameterInformationWindowManager.HideWindow(null, view);
		}

		private void HAdjustment_ValueChanged(object sender, EventArgs e)
		{
			if (!isInKeyStroke)
			{
				CompletionWindowManager.HideWindow();
				ParameterInformationWindowManager.HideWindow(null, view);
			}
			else
			{
				CompletionWindowManager.RepositionWindow();
				ParameterInformationWindowManager.RepositionWindow(null, view);
			}
		}

		[CommandHandler(TextEditorCommands.LineEnd)]
		internal void OnLineEnd()
		{
			RunAction(CaretMoveActions.LineEnd);
		}

		[CommandHandler(TextEditorCommands.LineStart)]
		internal void OnLineStart()
		{
			RunAction(CaretMoveActions.LineHome);
		}

		[CommandHandler(TextEditorCommands.DeleteLeftChar)]
		internal void OnDeleteLeftChar()
		{
			RunAction(DeleteActions.Backspace);
		}

		[CommandHandler(TextEditorCommands.DeleteRightChar)]
		internal void OnDeleteRightChar()
		{
			RunAction(DeleteActions.Delete);
		}

		[CommandHandler(TextEditorCommands.CharLeft)]
		internal void OnCharLeft()
		{
			RunAction(CaretMoveActions.Left);
		}

		[CommandHandler(TextEditorCommands.CharRight)]
		internal void OnCharRight()
		{
			RunAction(CaretMoveActions.Right);
		}

		[CommandHandler(TextEditorCommands.LineUp)]
		internal void OnLineUp()
		{
			RunAction(CaretMoveActions.Up);
		}

		[CommandHandler(TextEditorCommands.LineDown)]
		internal void OnLineDown()
		{
			RunAction(CaretMoveActions.Down);
		}

		[CommandHandler(TextEditorCommands.DocumentStart)]
		internal void OnDocumentStart()
		{
			RunAction(CaretMoveActions.ToDocumentStart);
		}

		[CommandHandler(TextEditorCommands.DocumentEnd)]
		internal void OnDocumentEnd()
		{
			RunAction(CaretMoveActions.ToDocumentEnd);
		}

		[CommandHandler(TextEditorCommands.PageUp)]
		internal void OnPageUp()
		{
			RunAction(CaretMoveActions.PageUp);
		}

		[CommandHandler(TextEditorCommands.PageDown)]
		internal void OnPageDown()
		{
			RunAction(CaretMoveActions.PageDown);
		}

		[CommandHandler(TextEditorCommands.DeleteLine)]
		internal void OnDeleteLine()
		{
			RunAction(DeleteActions.CaretLine);
		}

		[CommandHandler(TextEditorCommands.DeleteToLineEnd)]
		internal void OnDeleteToLineEnd()
		{
			RunAction(DeleteActions.CaretLineToEnd);
		}

		[CommandHandler(TextEditorCommands.ScrollLineUp)]
		internal void OnScrollLineUp()
		{
			RunAction(ScrollActions.Up);
		}

		[CommandHandler(TextEditorCommands.ScrollLineDown)]
		internal void OnScrollLineDown()
		{
			RunAction(ScrollActions.Down);
		}

		[CommandHandler(TextEditorCommands.ScrollPageUp)]
		internal void OnScrollPageUp()
		{
			RunAction(ScrollActions.PageUp);
		}

		[CommandHandler(TextEditorCommands.ScrollPageDown)]
		internal void OnScrollPageDown()
		{
			RunAction(ScrollActions.PageDown);
		}

		[CommandHandler(TextEditorCommands.GotoMatchingBrace)]
		internal void OnGotoMatchingBrace()
		{
			RunAction(MiscActions.GotoMatchingBracket);
		}

		[CommandHandler(TextEditorCommands.SelectionMoveLeft)]
		internal void OnSelectionMoveLeft()
		{
			RunAction(SelectionActions.MoveLeft);
		}

		[CommandHandler(TextEditorCommands.SelectionMoveRight)]
		internal void OnSelectionMoveRight()
		{
			RunAction(SelectionActions.MoveRight);
		}

		[CommandHandler(TextEditorCommands.MovePrevWord)]
		internal void OnMovePrevWord()
		{
			RunAction(CaretMoveActions.PreviousWord);
		}

		[CommandHandler(TextEditorCommands.MoveNextWord)]
		internal void OnMoveNextWord()
		{
			RunAction(CaretMoveActions.NextWord);
		}

		[CommandHandler(TextEditorCommands.SelectionMovePrevWord)]
		internal void OnSelectionMovePrevWord()
		{
			RunAction(SelectionActions.MovePreviousWord);
		}

		[CommandHandler(TextEditorCommands.SelectionMoveNextWord)]
		internal void OnSelectionMoveNextWord()
		{
			RunAction(SelectionActions.MoveNextWord);
		}

		[CommandHandler(TextEditorCommands.MovePrevSubword)]
		internal void OnMovePrevSubword()
		{
			RunAction(CaretMoveActions.PreviousSubword);
		}

		[CommandHandler(TextEditorCommands.MoveNextSubword)]
		internal void OnMoveNextSubword()
		{
			RunAction(CaretMoveActions.NextSubword);
		}

		[CommandHandler(TextEditorCommands.SelectionMovePrevSubword)]
		internal void OnSelectionMovePrevSubword()
		{
			RunAction(SelectionActions.MovePreviousSubword);
		}

		[CommandHandler(TextEditorCommands.SelectionMoveNextSubword)]
		internal void OnSelectionMoveNextSubword()
		{
			RunAction(SelectionActions.MoveNextSubword);
		}

		[CommandHandler(TextEditorCommands.SelectionMoveUp)]
		internal void OnSelectionMoveUp()
		{
			RunAction(SelectionActions.MoveUp);
		}

		[CommandHandler(TextEditorCommands.SelectionMoveDown)]
		internal void OnSelectionMoveDown()
		{
			RunAction(SelectionActions.MoveDown);
		}

		[CommandHandler(TextEditorCommands.SelectionMoveHome)]
		internal void OnSelectionMoveHome()
		{
			RunAction(SelectionActions.MoveLineHome);
		}

		[CommandHandler(TextEditorCommands.SelectionMoveEnd)]
		internal void OnSelectionMoveEnd()
		{
			RunAction(SelectionActions.MoveLineEnd);
		}

		[CommandHandler(TextEditorCommands.SelectionMoveToDocumentStart)]
		internal void OnSelectionMoveToDocumentStart()
		{
			RunAction(SelectionActions.MoveToDocumentStart);
		}

		[CommandHandler(TextEditorCommands.ExpandSelectionToLine)]
		internal void OnExpandSelectionToLine()
		{
			RunAction(SelectionActions.ExpandSelectionToLine);
		}

		[CommandHandler(TextEditorCommands.SelectionMoveToDocumentEnd)]
		internal void OnSelectionMoveToDocumentEnd()
		{
			RunAction(SelectionActions.MoveToDocumentEnd);
		}

		[CommandHandler(TextEditorCommands.SwitchCaretMode)]
		internal void OnSwitchCaretMode()
		{
			RunAction(MiscActions.SwitchCaretMode);
		}

		[CommandHandler(TextEditorCommands.InsertTab)]
		internal void OnInsertTab()
		{
			RunAction(MiscActions.InsertTab);
		}

		[CommandHandler(TextEditorCommands.RemoveTab)]
		internal void OnRemoveTab()
		{
			RunAction(MiscActions.RemoveTab);
		}

		[CommandHandler(TextEditorCommands.InsertNewLine)]
		internal void OnInsertNewLine()
		{
			RunAction(MiscActions.InsertNewLine);
		}

		[CommandHandler(TextEditorCommands.InsertNewLineAtEnd)]
		internal void OnInsertNewLineAtEnd()
		{
			RunAction(MiscActions.InsertNewLineAtEnd);
		}

		[CommandHandler(TextEditorCommands.InsertNewLinePreserveCaretPosition)]
		internal void OnInsertNewLinePreserveCaretPosition()
		{
			RunAction(MiscActions.InsertNewLinePreserveCaretPosition);
		}

		[CommandHandler(TextEditorCommands.CompleteStatement)]
		internal void OnCompleteStatement()
		{
			Document activeDocument = IdeApp.Workbench.ActiveDocument;
			CodeGenerator.CreateGenerator(activeDocument)?.CompleteStatement(activeDocument);
		}

		[CommandHandler(TextEditorCommands.DeletePrevWord)]
		internal void OnDeletePrevWord()
		{
			RunAction(DeleteActions.PreviousWord);
		}

		[CommandHandler(TextEditorCommands.DeleteNextWord)]
		internal void OnDeleteNextWord()
		{
			RunAction(DeleteActions.NextWord);
		}

		[CommandHandler(TextEditorCommands.DeletePrevSubword)]
		internal void OnDeletePrevSubword()
		{
			RunAction(DeleteActions.PreviousSubword);
		}

		[CommandHandler(TextEditorCommands.DeleteNextSubword)]
		internal void OnDeleteNextSubword()
		{
			RunAction(DeleteActions.NextSubword);
		}

		[CommandHandler(TextEditorCommands.SelectionPageDownAction)]
		internal void OnSelectionPageDownAction()
		{
			RunAction(SelectionActions.MovePageDown);
		}

		[CommandHandler(TextEditorCommands.SelectionPageUpAction)]
		internal void OnSelectionPageUpAction()
		{
			RunAction(SelectionActions.MovePageUp);
		}

		[CommandHandler(SourceEditorCommands.PulseCaret)]
		internal void OnPulseCaretCommand()
		{
			StartCaretPulseAnimation();
		}

		[CommandHandler(TextEditorCommands.TransposeCharacters)]
		internal void TransposeCharacters()
		{
			RunAction(MiscActions.TransposeCharacters);
		}

		[CommandHandler(TextEditorCommands.DuplicateLine)]
		internal void DuplicateLine()
		{
			RunAction(MiscActions.DuplicateLine);
		}

		[CommandHandler(TextEditorCommands.RecenterEditor)]
		internal void RecenterEditor()
		{
			RunAction(MiscActions.RecenterEditor);
		}

		[CommandHandler(EditCommands.JoinWithNextLine)]
		internal void JoinLines()
		{
			using (base.Document.OpenUndoGroup())
			{
				RunAction(ViActions.Join);
			}
		}

		[CommandHandler(EditCommands.SortSelectedLines)]
		private void SortSelectedLines()
		{
			RunAction(MiscActions.SortSelectedLines);
		}

		[CommandUpdateHandler(EditCommands.SortSelectedLines)]
		private void UpdateSortSelectedLines(CommandInfo ci)
		{
			ci.Enabled = GetTextEditorData().IsMultiLineSelection;
		}
	}
}
