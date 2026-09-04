using System;
using System.Collections.Generic;
using System.Linq;
using Gdk;
using Gtk;
using Mono.TextEditor;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using Stetic;
using Xwt.Drawing;

namespace MonoDevelop.SourceEditor
{
	public class SearchAndReplaceWidget : Bin
	{
		private const char historySeparator = '\n';

		private const int historyLimit = 20;

		private const string seachHistoryProperty = "MonoDevelop.FindReplaceDialogs.FindHistory";

		private const string replaceHistoryProperty = "MonoDevelop.FindReplaceDialogs.ReplaceHistory";

		public const string DefaultSearchEngine = "default";

		public const string RegexSearchEngine = "regex";

		private readonly TextEditor textEditor;

		private readonly Widget frame;

		private bool isReplaceMode = true;

		private Widget[] replaceWidgets;

		private Label resultInformLabel = new Label();

		private EventBox resultInformLabelEventBox;

		private int curSearchResult = -1;

		private string curSearchPattern;

		private DocumentLocation caretSave;

		private string oldPattern;

		private SearchResult result;

		internal static bool inReplaceUpdate = false;

		private Table table;

		private Button buttonSearchMode;

		private Arrow searchButtonModeArrow;

		private HBox hbox1;

		private SearchEntry searchEntry;

		private Button buttonSearchBackward;

		private EventBox eventbox2;

		private Gtk.Image image2;

		private Button buttonSearchForward;

		private EventBox eventbox3;

		private Gtk.Image image3;

		private Button closeButton;

		private EventBox eventbox4;

		private Gtk.Image image4;

		private HBox hbox2;

		private Entry entryReplace;

		private Button buttonReplace;

		private EventBox eventbox5;

		private Gtk.Image image5;

		private Button buttonReplaceAll;

		private HBox hbox3;

		private EventBox eventbox6;

		private Gtk.Image image6;

		private Label label1;

		public TextSegment SelectionSegment { get; set; }

		public bool IsInSelectionSearchMode { get; set; }

		public bool IsCaseSensitive
		{
			get
			{
				return PropertyService.Get("IsCaseSensitive", defaultValue: false);
			}
			set
			{
				if (IsCaseSensitive != value)
				{
					PropertyService.Set("IsCaseSensitive", value);
				}
			}
		}

		public static bool IsWholeWordOnly => PropertyService.Get("IsWholeWordOnly", defaultValue: false);

		public static string SearchEngine => PropertyService.Get("BufferSearchEngine", "default");

		public string ReplacePattern
		{
			get
			{
				return entryReplace.Text;
			}
			set
			{
				entryReplace.Text = value ?? "";
			}
		}

		public string SearchPattern
		{
			get
			{
				return searchEntry.Entry.Text;
			}
			set
			{
				searchEntry.Entry.Text = value ?? "";
			}
		}

		public bool SearchFocused => searchEntry.HasFocus;

		public bool DisableAutomaticSearchPatternCaseMatch { get; set; }

		public bool IsReplaceMode
		{
			get
			{
				return isReplaceMode;
			}
			set
			{
				isReplaceMode = value;
				searchButtonModeArrow.ArrowType = ((!isReplaceMode) ? ArrowType.Down : ArrowType.Up);
				table.RowSpacing = (isReplaceMode ? 6u : 0u);
				Widget[] array = replaceWidgets;
				foreach (Widget widget in array)
				{
					widget.Visible = isReplaceMode;
				}
				if (textEditor.Document.ReadOnly)
				{
					buttonSearchMode.Visible = false;
				}
			}
		}

		internal static event EventHandler ReplacePatternChanged;

		private void HandleViewTextEditorhandleSizeAllocated(object o, SizeAllocatedArgs args)
		{
			if (frame != null && textEditor != null)
			{
				int num = textEditor.Allocation.Width - base.Allocation.Width - 8;
				TextEditor.EditorContainerChild editorContainerChild = (TextEditor.EditorContainerChild)textEditor[frame];
				if (num != editorContainerChild.X)
				{
					searchEntry.WidthRequest = textEditor.Allocation.Width / 3;
					editorContainerChild.X = num;
					textEditor.QueueResize();
				}
			}
		}

		private static string GetShortcut(object commandId)
		{
			string accelKey = IdeApp.CommandService.GetCommand(commandId).AccelKey;
			if (string.IsNullOrEmpty(accelKey))
			{
				return "";
			}
			string text = KeyBindingManager.BindingToDisplayLabel(accelKey, concise: false);
			return "(" + text + ")";
		}

		public SearchAndReplaceWidget(TextEditor textEditor, Widget frame)
		{
			SearchAndReplaceWidget searchAndReplaceWidget = this;
			if (textEditor == null)
			{
				throw new ArgumentNullException("textEditor");
			}
			this.textEditor = textEditor;
			this.frame = frame;
			textEditor.SizeAllocated += HandleViewTextEditorhandleSizeAllocated;
			textEditor.TextViewMargin.SearchRegionsUpdated += HandleWidgetTextEditorTextViewMarginSearchRegionsUpdated;
			textEditor.Caret.PositionChanged += HandleWidgetTextEditorCaretPositionChanged;
			base.SizeAllocated += HandleViewTextEditorhandleSizeAllocated;
			base.Name = "SearchAndReplaceWidget";
			base.Events = EventMask.AllEventsMask;
			DisableAutomaticSearchPatternCaseMatch = false;
			Build();
			buttonReplace.TooltipText = GettextCatalog.GetString("Replace");
			buttonSearchForward.TooltipText = GettextCatalog.GetString("Find next {0}", GetShortcut(SearchCommands.FindNext));
			buttonSearchBackward.TooltipText = GettextCatalog.GetString("Find previous {0}", GetShortcut(SearchCommands.FindPrevious));
			buttonSearchMode.TooltipText = GettextCatalog.GetString("Toggle between search and replace mode");
			searchEntry.Ready = true;
			searchEntry.Visible = true;
			searchEntry.WidthRequest = textEditor.Allocation.Width / 3;
			searchEntry.ForceFilterButtonVisible = true;
			replaceWidgets = new Widget[3] { entryReplace, buttonReplace, buttonReplaceAll };
			base.FocusChain = new Widget[6] { searchEntry, buttonSearchForward, buttonSearchBackward, entryReplace, buttonReplace, buttonReplaceAll };
			FilterHistory("MonoDevelop.FindReplaceDialogs.FindHistory");
			FilterHistory("MonoDevelop.FindReplaceDialogs.ReplaceHistory");
			if (Platform.IsMac)
			{
				EventBox[] array = new EventBox[5] { eventbox2, eventbox3, eventbox4, eventbox5, eventbox6 };
				foreach (EventBox eventBox in array)
				{
					eventBox.VisibleWindow = true;
					eventBox.ModifyBg(StateType.Normal, new Gdk.Color(245, 245, 245));
				}
			}
			if (string.IsNullOrEmpty(textEditor.SearchPattern))
			{
				textEditor.SearchPattern = SearchAndReplaceOptions.SearchPattern;
			}
			else if (textEditor.SearchPattern != SearchAndReplaceOptions.SearchPattern)
			{
				SearchAndReplaceOptions.SearchPattern = textEditor.SearchPattern;
			}
			UpdateSearchPattern();
			SetSearchOptions();
			searchEntry.Entry.KeyReleaseEvent += delegate
			{
				CheckSearchPatternCasing(SearchPattern);
			};
			searchEntry.Entry.Changed += delegate
			{
				SetSearchPattern(SearchPattern);
				string searchPattern = SearchAndReplaceOptions.SearchPattern;
				SearchAndReplaceOptions.SearchPattern = SearchPattern;
				if (searchPattern != SearchAndReplaceOptions.SearchPattern)
				{
					UpdateSearchEntry();
				}
				List<string> history = GetHistory("MonoDevelop.FindReplaceDialogs.FindHistory");
				if (history.Count > 0 && history[0] == searchPattern)
				{
					ChangeHistory("MonoDevelop.FindReplaceDialogs.FindHistory", SearchAndReplaceOptions.SearchPattern);
				}
				else
				{
					UpdateSearchHistory(SearchAndReplaceOptions.SearchPattern);
				}
			};
			entryReplace.Text = SearchAndReplaceOptions.ReplacePattern ?? "";
			Widget[] children = base.Children;
			foreach (Widget widget in children)
			{
				widget.KeyPressEvent += delegate(object sender, KeyPressEventArgs args)
				{
					if (args.Event.Key == Gdk.Key.Escape)
					{
						RemoveSearchWidget();
					}
				};
			}
			closeButton.Clicked += delegate
			{
				RemoveSearchWidget();
			};
			buttonSearchMode.Clicked += delegate
			{
				IsReplaceMode = !IsReplaceMode;
			};
			base.FocusChildSet += delegate
			{
				StoreWidgetState();
			};
			searchEntry.Entry.Activated += delegate
			{
				UpdateSearchHistory(searchAndReplaceWidget.SearchPattern);
				FindNext(textEditor);
			};
			buttonSearchForward.Clicked += delegate
			{
				UpdateSearchHistory(searchAndReplaceWidget.SearchPattern);
				FindNext(textEditor);
			};
			buttonSearchBackward.Clicked += delegate
			{
				UpdateSearchHistory(searchAndReplaceWidget.SearchPattern);
				FindPrevious(textEditor);
			};
			searchEntry.RequestMenu += HandleSearchEntryhandleRequestMenu;
			entryReplace.Changed += delegate
			{
				SearchAndReplaceOptions.ReplacePattern = ReplacePattern;
				if (!inReplaceUpdate)
				{
					FireReplacePatternChanged();
				}
			};
			entryReplace.Activated += delegate
			{
				UpdateSearchHistory(SearchPattern);
				UpdateReplaceHistory(ReplacePattern);
				Replace();
				entryReplace.GrabFocus();
			};
			buttonReplace.Clicked += delegate
			{
				UpdateSearchHistory(SearchPattern);
				UpdateReplaceHistory(ReplacePattern);
				Replace();
			};
			buttonReplaceAll.Clicked += delegate
			{
				UpdateSearchHistory(SearchPattern);
				UpdateReplaceHistory(ReplacePattern);
				ReplaceAll();
			};
			buttonSearchForward.KeyPressEvent += OnNavigateKeyPressEvent;
			buttonSearchBackward.KeyPressEvent += OnNavigateKeyPressEvent;
			searchEntry.Entry.KeyPressEvent += OnNavigateKeyPressEvent;
			entryReplace.KeyPressEvent += OnNavigateKeyPressEvent;
			buttonReplace.KeyPressEvent += OnNavigateKeyPressEvent;
			buttonReplaceAll.KeyPressEvent += OnNavigateKeyPressEvent;
			resultInformLabelEventBox = searchEntry.AddLabelWidget(resultInformLabel);
			resultInformLabelEventBox.BorderWidth = 2u;
			resultInformLabel.Xpad = 2;
			resultInformLabel.Show();
			searchEntry.FilterButtonPixbuf = Xwt.Drawing.Image.FromResource("searchoptions.png");
			if (textEditor.IsSomethingSelected)
			{
				if (textEditor.MainSelection.MinLine == textEditor.MainSelection.MaxLine)
				{
					SetSearchPattern();
				}
				else
				{
					IsInSelectionSearchMode = true;
					SelectionSegment = textEditor.SelectionRange;
					SetSearchOptions();
				}
			}
			SetSearchPattern(SearchAndReplaceOptions.SearchPattern);
			textEditor.HighlightSearchPattern = true;
			textEditor.TextViewMargin.RefreshSearchMarker();
			if (textEditor.Document.ReadOnly)
			{
				buttonSearchMode.Visible = false;
				IsReplaceMode = false;
			}
			SearchAndReplaceOptions.SearchPatternChanged += HandleSearchPatternChanged;
			SearchAndReplaceOptions.ReplacePatternChanged += HandleReplacePatternChanged;
		}

		private void HandleReplacePatternChanged(object sender, EventArgs e)
		{
			ReplacePattern = SearchAndReplaceOptions.ReplacePattern;
		}

		private void HandleSearchPatternChanged(object sender, EventArgs e)
		{
			SearchPattern = SearchAndReplaceOptions.SearchPattern;
		}

		internal void CheckSearchPatternCasing(string searchPattern)
		{
			if (!DisableAutomaticSearchPatternCaseMatch && PropertyService.Get("AutoSetPatternCasing", defaultValue: true))
			{
				IsCaseSensitive = searchPattern.Any((char ch) => char.IsUpper(ch));
				SetSearchOptions();
			}
		}

		private void SetSearchOptions()
		{
			if (SearchEngine == "default")
			{
				if (!(textEditor.SearchEngine is BasicSearchEngine))
				{
					textEditor.SearchEngine = new BasicSearchEngine();
				}
			}
			else if (!(textEditor.SearchEngine is RegexSearchEngine))
			{
				textEditor.SearchEngine = new RegexSearchEngine();
			}
			textEditor.IsCaseSensitive = IsCaseSensitive;
			textEditor.IsWholeWordOnly = IsWholeWordOnly;
			textEditor.SearchRegion = (IsInSelectionSearchMode ? SelectionSegment : TextSegment.Invalid);
			string searchPattern = SearchPattern;
			if (textEditor.SearchEngine.IsValidPattern(searchPattern, out var _))
			{
				textEditor.SearchPattern = searchPattern;
			}
			textEditor.QueueDraw();
		}

		private void HandleSearchEntryhandleRequestMenu(object sender, EventArgs e)
		{
			if (searchEntry.Menu != null)
			{
				searchEntry.Menu.Destroy();
			}
			searchEntry.Menu = new Menu();
			CheckMenuItem caseSensitive = new CheckMenuItem(GettextCatalog.GetString("_Case sensitive"));
			caseSensitive.Active = IsCaseSensitive;
			caseSensitive.DrawAsRadio = false;
			caseSensitive.Toggled += delegate
			{
				SetIsCaseSensitive(caseSensitive.Active);
				UpdateSearchEntry();
			};
			searchEntry.Menu.Add(caseSensitive);
			CheckMenuItem wholeWordsOnly = new CheckMenuItem(GettextCatalog.GetString("_Whole words only"));
			wholeWordsOnly.Active = IsWholeWordOnly;
			wholeWordsOnly.DrawAsRadio = false;
			wholeWordsOnly.Toggled += delegate
			{
				SetIsWholeWordOnly(wholeWordsOnly.Active);
				UpdateSearchEntry();
			};
			searchEntry.Menu.Add(wholeWordsOnly);
			CheckMenuItem regexSearch = new CheckMenuItem(GettextCatalog.GetString("_Regex search"));
			regexSearch.Active = SearchEngine == "regex";
			regexSearch.DrawAsRadio = false;
			regexSearch.Toggled += delegate
			{
				SetIsRegexSearch(regexSearch.Active);
				UpdateSearchEntry();
			};
			searchEntry.Menu.Add(regexSearch);
			CheckMenuItem inselectionSearch = new CheckMenuItem(GettextCatalog.GetString("_Search In Selection"));
			inselectionSearch.Active = IsInSelectionSearchMode;
			inselectionSearch.DrawAsRadio = false;
			inselectionSearch.Toggled += delegate
			{
				IsInSelectionSearchMode = inselectionSearch.Active;
				UpdateSearchEntry();
			};
			searchEntry.Menu.Add(inselectionSearch);
			List<string> history = GetHistory("MonoDevelop.FindReplaceDialogs.FindHistory");
			if (history.Count <= 0)
			{
				return;
			}
			searchEntry.Menu.Add(new SeparatorMenuItem());
			MenuItem menuItem = new MenuItem(GettextCatalog.GetString("Recent Searches"));
			menuItem.Sensitive = false;
			searchEntry.Menu.Add(menuItem);
			foreach (string item in history)
			{
				if (!(item == searchEntry.Entry.Text))
				{
					MenuItem menuItem2 = new MenuItem(item);
					menuItem2.Name = item;
					menuItem2.Activated += delegate(object mySender, EventArgs myE)
					{
						MenuItem menuItem4 = (MenuItem)mySender;
						SearchAndReplaceOptions.SearchPattern = "";
						searchEntry.Entry.Text = menuItem4.Name;
						FilterHistory("MonoDevelop.FindReplaceDialogs.FindHistory");
					};
					searchEntry.Menu.Add(menuItem2);
				}
			}
			searchEntry.Menu.Add(new SeparatorMenuItem());
			MenuItem menuItem3 = new MenuItem(GettextCatalog.GetString("Clear Recent Searches"));
			menuItem3.Activated += delegate
			{
				StoreHistory("MonoDevelop.FindReplaceDialogs.FindHistory", null);
			};
			searchEntry.Menu.Add(menuItem3);
		}

		private void HandleWidgetTextEditorCaretPositionChanged(object sender, DocumentLocationEventArgs e)
		{
			UpdateResultInformLabel();
		}

		private void HandleWidgetTextEditorTextViewMarginSearchRegionsUpdated(object sender, EventArgs e)
		{
			UpdateResultInformLabel();
		}

		protected override bool OnEnterNotifyEvent(EventCrossing evnt)
		{
			base.GdkWindow.Cursor = null;
			return base.OnEnterNotifyEvent(evnt);
		}

		[CommandHandler(EditCommands.SelectAll)]
		public void SelectAllCommand()
		{
			if (searchEntry.HasFocus)
			{
				searchEntry.Entry.SelectRegion(0, searchEntry.Entry.Text.Length);
			}
			else if (IsReplaceMode && entryReplace.HasFocus)
			{
				entryReplace.SelectRegion(0, entryReplace.Text.Length);
			}
		}

		public void UpdateSearchPattern()
		{
			searchEntry.Entry.Text = textEditor.SearchPattern ?? "";
			SetSearchPattern(textEditor.SearchPattern);
			SearchAndReplaceOptions.SearchPattern = textEditor.SearchPattern;
		}

		private void OnNavigateKeyPressEvent(object o, KeyPressEventArgs args)
		{
			args.RetVal = false;
			switch (args.Event.Key)
			{
			case Gdk.Key.Up:
			case Gdk.Key.Down:
				if (o != searchEntry.Entry)
				{
					args.RetVal = true;
					break;
				}
				if ((args.Event.State & ModifierType.ShiftMask) == ModifierType.ShiftMask && o == searchEntry.Entry)
				{
					searchEntry.PopupFilterMenu();
				}
				else
				{
					if (curSearchResult == -1)
					{
						curSearchPattern = searchEntry.Entry.Text;
					}
					List<string> history = GetHistory("MonoDevelop.FindReplaceDialogs.FindHistory");
					if (history.Count > 0)
					{
						curSearchResult += ((args.Event.Key != Gdk.Key.Up) ? 1 : (-1));
						if (curSearchResult >= history.Count)
						{
							curSearchResult = -1;
						}
						if (curSearchResult == -1)
						{
							searchEntry.Entry.Text = curSearchPattern;
						}
						else
						{
							if (curSearchResult < -1)
							{
								curSearchResult = history.Count - 1;
							}
							searchEntry.Entry.Text = history[curSearchResult];
						}
						searchEntry.Entry.Position = -1;
					}
				}
				args.RetVal = true;
				break;
			case Gdk.Key.N:
			case Gdk.Key.n:
				buttonSearchForward.GrabFocus();
				buttonSearchForward.Click();
				break;
			case Gdk.Key.P:
			case Gdk.Key.p:
				buttonSearchBackward.GrabFocus();
				buttonSearchBackward.Click();
				break;
			case Gdk.Key.Escape:
				RemoveSearchWidget();
				break;
			case Gdk.Key.slash:
				searchEntry.GrabFocus();
				break;
			case Gdk.Key.ISO_Left_Tab:
				if (IsReplaceMode)
				{
					if (o == entryReplace)
					{
						searchEntry.Entry.GrabFocus();
					}
					else if (o == buttonReplace)
					{
						entryReplace.GrabFocus();
					}
					else if (o == buttonReplaceAll)
					{
						buttonReplace.GrabFocus();
					}
					else if (o == buttonSearchBackward)
					{
						buttonReplaceAll.GrabFocus();
					}
					else if (o == buttonSearchForward)
					{
						buttonSearchBackward.GrabFocus();
					}
					else
					{
						buttonSearchForward.GrabFocus();
					}
					args.RetVal = true;
				}
				else
				{
					if (o == buttonSearchBackward)
					{
						searchEntry.Entry.GrabFocus();
					}
					else if (o == buttonSearchForward)
					{
						buttonSearchBackward.GrabFocus();
					}
					else
					{
						buttonSearchForward.GrabFocus();
					}
					args.RetVal = true;
				}
				break;
			case Gdk.Key.Tab:
				if (IsReplaceMode)
				{
					if (o == entryReplace)
					{
						buttonReplace.GrabFocus();
					}
					else if (o == buttonReplace)
					{
						buttonReplaceAll.GrabFocus();
					}
					else if (o == buttonReplaceAll)
					{
						buttonSearchBackward.GrabFocus();
					}
					else if (o == buttonSearchBackward)
					{
						buttonSearchForward.GrabFocus();
					}
					else if (o == buttonSearchForward)
					{
						searchEntry.Entry.GrabFocus();
					}
					else
					{
						entryReplace.GrabFocus();
					}
					args.RetVal = true;
				}
				else
				{
					if (o == buttonSearchBackward)
					{
						buttonSearchForward.GrabFocus();
					}
					else if (o == buttonSearchForward)
					{
						searchEntry.Entry.GrabFocus();
					}
					else
					{
						buttonSearchBackward.GrabFocus();
					}
					args.RetVal = true;
				}
				break;
			default:
				args.RetVal = true;
				break;
			case Gdk.Key.Return:
			case Gdk.Key.KP_Enter:
				break;
			}
		}

		protected override void OnFocusChildSet(Widget widget)
		{
			base.OnFocusChildSet(widget);
			TextSegment mainSearchResult = textEditor.TextViewMargin.MainSearchResult;
			textEditor.TextViewMargin.HideSelection = widget == table && !mainSearchResult.IsInvalid && textEditor.IsSomethingSelected && textEditor.SelectionRange.Offset == mainSearchResult.Offset && textEditor.SelectionRange.EndOffset == mainSearchResult.EndOffset;
			if (textEditor.TextViewMargin.HideSelection)
			{
				textEditor.QueueDraw();
			}
		}

		protected override void OnDestroyed()
		{
			SearchAndReplaceOptions.SearchPatternChanged -= HandleSearchPatternChanged;
			SearchAndReplaceOptions.ReplacePatternChanged -= HandleReplacePatternChanged;
			textEditor.TextViewMargin.HideSelection = false;
			textEditor.Caret.PositionChanged -= HandleWidgetTextEditorCaretPositionChanged;
			textEditor.TextViewMargin.SearchRegionsUpdated -= HandleWidgetTextEditorTextViewMarginSearchRegionsUpdated;
			base.SizeAllocated -= HandleViewTextEditorhandleSizeAllocated;
			textEditor.SizeAllocated -= HandleViewTextEditorhandleSizeAllocated;
			ReplacePatternChanged -= UpdateReplacePattern;
			if (frame != null)
			{
				textEditor.QueueDraw();
			}
			base.OnDestroyed();
		}

		public void Focus()
		{
			searchEntry.GrabFocusEntry();
		}

		private void StoreWidgetState()
		{
			caretSave = textEditor.Caret.Location;
		}

		private void GotoResult(SearchResult result)
		{
			try
			{
				if (result == null)
				{
					textEditor.ClearSelection();
					return;
				}
				textEditor.StopSearchResultAnimation();
				textEditor.Caret.Location = textEditor.OffsetToLocation(result.EndOffset);
				textEditor.SetSelection(result.Offset, result.EndOffset);
				textEditor.CenterToCaret();
				textEditor.AnimateSearchResult(result);
			}
			catch (Exception)
			{
			}
		}

		private static List<string> GetHistory(string propertyKey)
		{
			string text = PropertyService.Get<string>(propertyKey);
			if (string.IsNullOrEmpty(text))
			{
				return new List<string>();
			}
			return new List<string>(text.Split('\n'));
		}

		private static void StoreHistory(string propertyKey, List<string> history)
		{
			PropertyService.Set(propertyKey, (history != null) ? string.Join('\n'.ToString(), history.ToArray()) : null);
		}

		private static void UpdateHistory(string propertyKey, string item)
		{
			List<string> history = GetHistory(propertyKey);
			history.Remove(item);
			history.Insert(0, item);
			while (history.Count >= 20)
			{
				history.RemoveAt(19);
			}
			StoreHistory(propertyKey, history);
		}

		private static void FilterHistory(string propertyKey)
		{
			List<string> history = GetHistory(propertyKey);
			List<string> list = new List<string>();
			HashSet<string> hashSet = new HashSet<string>();
			foreach (string item in history)
			{
				if (!hashSet.Contains(item))
				{
					hashSet.Add(item);
					list.Add(item);
				}
			}
			StoreHistory(propertyKey, list);
		}

		internal static void UpdateSearchHistory(string item)
		{
			UpdateHistory("MonoDevelop.FindReplaceDialogs.FindHistory", item);
		}

		private static void ChangeHistory(string propertyKey, string item)
		{
			List<string> history = GetHistory(propertyKey);
			history.RemoveAt(0);
			history.Insert(0, item);
			StoreHistory(propertyKey, history);
		}

		private void SetIsCaseSensitive(bool value)
		{
			IsCaseSensitive = value;
			SetSearchOptions();
		}

		private void SetIsWholeWordOnly(bool value)
		{
			PropertyService.Set("IsWholeWordOnly", value);
			SetSearchOptions();
		}

		private void SetIsRegexSearch(bool value)
		{
			PropertyService.Set("BufferSearchEngine", value ? "regex" : "default");
			SetSearchOptions();
		}

		private void UpdateSearchEntry()
		{
			if (oldPattern != SearchPattern)
			{
				oldPattern = SearchPattern;
				SetSearchOptions();
				result = textEditor.SearchForward(textEditor.Document.LocationToOffset(caretSave));
			}
			GotoResult(result);
			UpdateResultInformLabel();
		}

		private void UpdateResultInformLabel()
		{
			if (string.IsNullOrEmpty(SearchPattern))
			{
				resultInformLabel.Text = "";
				resultInformLabelEventBox.ModifyBg(StateType.Normal, searchEntry.Entry.Style.Base(searchEntry.Entry.State));
				resultInformLabel.ModifyFg(StateType.Normal, searchEntry.Entry.Style.Foreground(StateType.Insensitive));
				return;
			}
			bool flag = textEditor.SearchEngine.IsValidPattern(SearchAndReplaceOptions.SearchPattern, out var error);
			if (!flag)
			{
				IdeApp.Workbench.StatusBar.ShowError(error);
			}
			else
			{
				IdeApp.Workbench.StatusBar.ShowReady();
			}
			if (!flag || textEditor.TextViewMargin.SearchResultMatchCount == 0)
			{
				resultInformLabel.Text = GettextCatalog.GetString("Not found");
				resultInformLabelEventBox.ModifyBg(StateType.Normal, GotoLineNumberWidget.errorColor);
				resultInformLabel.ModifyFg(StateType.Normal, searchEntry.Entry.Style.Foreground(StateType.Normal));
				return;
			}
			int num = 0;
			int num2 = -1;
			int offset = textEditor.Caret.Offset;
			TextSegment mainSearchResult = TextSegment.Invalid;
			foreach (TextSegment searchResult in textEditor.TextViewMargin.SearchResults)
			{
				if (searchResult.Offset <= offset && offset <= searchResult.EndOffset)
				{
					num2 = num + 1;
					mainSearchResult = searchResult;
					break;
				}
				num++;
			}
			if (num2 != -1)
			{
				resultInformLabel.Text = string.Format(GettextCatalog.GetString("{0} of {1}"), num2, textEditor.TextViewMargin.SearchResultMatchCount);
			}
			else
			{
				resultInformLabel.Text = string.Format(GettextCatalog.GetPluralString("{0} match", "{0} matches", textEditor.TextViewMargin.SearchResultMatchCount), textEditor.TextViewMargin.SearchResultMatchCount);
			}
			resultInformLabelEventBox.ModifyBg(StateType.Normal, searchEntry.Entry.Style.Base(searchEntry.Entry.State));
			resultInformLabel.ModifyFg(StateType.Normal, searchEntry.Entry.Style.Foreground(StateType.Insensitive));
			textEditor.TextViewMargin.HideSelection = base.FocusChild == table;
			textEditor.TextViewMargin.MainSearchResult = mainSearchResult;
		}

		private void UpdateReplaceHistory(string item)
		{
			UpdateHistory("MonoDevelop.FindReplaceDialogs.ReplaceHistory", item);
		}

		private void UpdateReplacePattern(object sender, EventArgs args)
		{
			entryReplace.Text = SearchAndReplaceOptions.ReplacePattern ?? "";
		}

		internal void SetSearchPattern()
		{
			string text = SourceEditorWidget.FormatPatternToSelectionOption(textEditor.SelectedText);
			if (!string.IsNullOrEmpty(text))
			{
				SetSearchPattern(text);
				SearchAndReplaceOptions.SearchPattern = text;
				UpdateSearchHistory(text);
				textEditor.TextViewMargin.MainSearchResult = textEditor.SelectionRange;
			}
		}

		public void SetSearchPattern(string searchPattern)
		{
			textEditor.SearchPattern = searchPattern;
		}

		public static SearchResult FindNext(TextEditor textEditor)
		{
			textEditor.SearchPattern = SearchAndReplaceOptions.SearchPattern;
			SearchResult searchResult = textEditor.FindNext(setSelection: true);
			if (searchResult == null)
			{
				return null;
			}
			textEditor.CenterToCaret();
			if (searchResult == null)
			{
				IdeApp.Workbench.StatusBar.ShowError(GettextCatalog.GetString("Search pattern not found"));
			}
			else if (searchResult.SearchWrapped)
			{
				IdeApp.Workbench.StatusBar.ShowMessage(Stock.Find, GettextCatalog.GetString("Reached bottom, continued from top"));
			}
			else
			{
				IdeApp.Workbench.StatusBar.ShowReady();
			}
			return searchResult;
		}

		public static SearchResult FindPrevious(TextEditor textEditor)
		{
			textEditor.SearchPattern = SearchAndReplaceOptions.SearchPattern;
			SearchResult searchResult = textEditor.FindPrevious(setSelection: true);
			if (searchResult == null)
			{
				return null;
			}
			textEditor.CenterToCaret();
			if (searchResult == null)
			{
				IdeApp.Workbench.StatusBar.ShowError(GettextCatalog.GetString("Search pattern not found"));
			}
			else if (searchResult.SearchWrapped)
			{
				IdeApp.Workbench.StatusBar.ShowMessage(Stock.Find, GettextCatalog.GetString("Reached top, continued from bottom"));
			}
			else
			{
				IdeApp.Workbench.StatusBar.ShowReady();
			}
			return searchResult;
		}

		public void Replace()
		{
			textEditor.Replace(ReplacePattern);
			textEditor.CenterToCaret();
			textEditor.GrabFocus();
		}

		public void ReplaceAll()
		{
			int num = textEditor.ReplaceAll(ReplacePattern);
			if (num == 0)
			{
				IdeApp.Workbench.StatusBar.ShowError(GettextCatalog.GetString("Search pattern not found"));
			}
			else
			{
				IdeApp.Workbench.StatusBar.ShowMessage(GettextCatalog.GetPluralString("Found and replaced one occurrence", "Found and replaced {0} occurrences", num, num));
			}
			textEditor.GrabFocus();
			textEditor.CenterToCaret();
		}

		internal static void FireReplacePatternChanged()
		{
			inReplaceUpdate = true;
			if (ReplacePatternChanged != null)
			{
				ReplacePatternChanged(null, EventArgs.Empty);
			}
			inReplaceUpdate = false;
		}

		private void RemoveSearchWidget()
		{
			textEditor.HighlightSearchPattern = false;
			Destroy();
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			Stetic.BinContainer.Attach(this);
			base.Name = "MonoDevelop.SourceEditor.SearchAndReplaceWidget";
			table = new Table(2u, 2u, homogeneous: false);
			table.Name = "table";
			table.BorderWidth = 4u;
			buttonSearchMode = new Button();
			buttonSearchMode.CanFocus = true;
			buttonSearchMode.Name = "buttonSearchMode";
			searchButtonModeArrow = new Arrow(ArrowType.Up, ShadowType.None);
			searchButtonModeArrow.Name = "searchButtonModeArrow";
			buttonSearchMode.Add(searchButtonModeArrow);
			buttonSearchMode.Label = null;
			table.Add(buttonSearchMode);
			Table.TableChild tableChild = (Table.TableChild)table[buttonSearchMode];
			tableChild.XOptions = AttachOptions.Fill;
			tableChild.YOptions = AttachOptions.Fill;
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			searchEntry = new SearchEntry();
			searchEntry.Name = "searchEntry";
			searchEntry.ForceFilterButtonVisible = false;
			searchEntry.HasFrame = true;
			searchEntry.RoundedShape = false;
			searchEntry.IsCheckMenu = false;
			searchEntry.ActiveFilterID = 0;
			searchEntry.Ready = false;
			searchEntry.HasFocus = false;
			hbox1.Add(searchEntry);
			Box.BoxChild boxChild = (Box.BoxChild)hbox1[searchEntry];
			boxChild.Position = 0;
			buttonSearchBackward = new Button();
			buttonSearchBackward.CanFocus = true;
			buttonSearchBackward.Name = "buttonSearchBackward";
			buttonSearchBackward.Relief = ReliefStyle.None;
			eventbox2 = new EventBox();
			eventbox2.Name = "eventbox2";
			eventbox2.AboveChild = true;
			eventbox2.VisibleWindow = false;
			image2 = new Gtk.Image();
			image2.Name = "image2";
			image2.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-go-up", IconSize.Menu);
			eventbox2.Add(image2);
			buttonSearchBackward.Add(eventbox2);
			buttonSearchBackward.Label = null;
			hbox1.Add(buttonSearchBackward);
			Box.BoxChild boxChild2 = (Box.BoxChild)hbox1[buttonSearchBackward];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			buttonSearchForward = new Button();
			buttonSearchForward.CanFocus = true;
			buttonSearchForward.Name = "buttonSearchForward";
			buttonSearchForward.Relief = ReliefStyle.None;
			eventbox3 = new EventBox();
			eventbox3.Name = "eventbox3";
			eventbox3.AboveChild = true;
			eventbox3.VisibleWindow = false;
			image3 = new Gtk.Image();
			image3.Name = "image3";
			image3.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-go-down", IconSize.Menu);
			eventbox3.Add(image3);
			buttonSearchForward.Add(eventbox3);
			buttonSearchForward.Label = null;
			hbox1.Add(buttonSearchForward);
			Box.BoxChild boxChild3 = (Box.BoxChild)hbox1[buttonSearchForward];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			closeButton = new Button();
			closeButton.CanFocus = true;
			closeButton.Name = "closeButton";
			closeButton.Relief = ReliefStyle.None;
			eventbox4 = new EventBox();
			eventbox4.Name = "eventbox4";
			eventbox4.AboveChild = true;
			eventbox4.VisibleWindow = false;
			image4 = new Gtk.Image();
			image4.Name = "image4";
			image4.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-close", IconSize.Menu);
			eventbox4.Add(image4);
			closeButton.Add(eventbox4);
			closeButton.Label = null;
			hbox1.Add(closeButton);
			Box.BoxChild boxChild4 = (Box.BoxChild)hbox1[closeButton];
			boxChild4.Position = 3;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			table.Add(hbox1);
			Table.TableChild tableChild2 = (Table.TableChild)table[hbox1];
			tableChild2.LeftAttach = 1u;
			tableChild2.RightAttach = 2u;
			tableChild2.YOptions = AttachOptions.Fill;
			hbox2 = new HBox();
			hbox2.Name = "hbox2";
			entryReplace = new Entry();
			entryReplace.CanFocus = true;
			entryReplace.Name = "entryReplace";
			entryReplace.IsEditable = true;
			entryReplace.InvisibleChar = '●';
			hbox2.Add(entryReplace);
			Box.BoxChild boxChild5 = (Box.BoxChild)hbox2[entryReplace];
			boxChild5.Position = 0;
			buttonReplace = new Button();
			buttonReplace.CanFocus = true;
			buttonReplace.Name = "buttonReplace";
			buttonReplace.Relief = ReliefStyle.None;
			eventbox5 = new EventBox();
			eventbox5.Name = "eventbox5";
			eventbox5.AboveChild = true;
			eventbox5.VisibleWindow = false;
			image5 = new Gtk.Image();
			image5.Name = "image5";
			image5.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-find-and-replace", IconSize.Menu);
			eventbox5.Add(image5);
			buttonReplace.Add(eventbox5);
			buttonReplace.Label = null;
			hbox2.Add(buttonReplace);
			Box.BoxChild boxChild6 = (Box.BoxChild)hbox2[buttonReplace];
			boxChild6.Position = 1;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			buttonReplaceAll = new Button();
			buttonReplaceAll.CanFocus = true;
			buttonReplaceAll.Name = "buttonReplaceAll";
			buttonReplaceAll.Relief = ReliefStyle.None;
			hbox3 = new HBox();
			hbox3.Name = "hbox3";
			hbox3.Spacing = 6;
			eventbox6 = new EventBox();
			eventbox6.Name = "eventbox6";
			eventbox6.AboveChild = true;
			eventbox6.VisibleWindow = false;
			image6 = new Gtk.Image();
			image6.Name = "image6";
			image6.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-find-and-replace", IconSize.Menu);
			eventbox6.Add(image6);
			hbox3.Add(eventbox6);
			Box.BoxChild boxChild7 = (Box.BoxChild)hbox3[eventbox6];
			boxChild7.Position = 0;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			label1 = new Label();
			label1.Name = "label1";
			label1.LabelProp = Catalog.GetString("All");
			hbox3.Add(label1);
			Box.BoxChild boxChild8 = (Box.BoxChild)hbox3[label1];
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			buttonReplaceAll.Add(hbox3);
			buttonReplaceAll.Label = null;
			hbox2.Add(buttonReplaceAll);
			Box.BoxChild boxChild9 = (Box.BoxChild)hbox2[buttonReplaceAll];
			boxChild9.Position = 2;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			table.Add(hbox2);
			Table.TableChild tableChild3 = (Table.TableChild)table[hbox2];
			tableChild3.TopAttach = 1u;
			tableChild3.BottomAttach = 2u;
			tableChild3.LeftAttach = 1u;
			tableChild3.RightAttach = 2u;
			tableChild3.YOptions = AttachOptions.Fill;
			Add(table);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			Show();
		}
	}
}
