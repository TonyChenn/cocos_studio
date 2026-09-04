using System;
using Mono.TextEditor;
using Mono.TextEditor.Vi;
using MonoDevelop.Core;
using MonoDevelop.Ide.Fonts;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Projects.Policies;

namespace MonoDevelop.SourceEditor
{
	public class DefaultSourceEditorOptions : TextEditorOptions, ISourceEditorOptions, ITextEditorOptions, IDisposable
	{
		private static DefaultSourceEditorOptions instance;

		private static bool inited;

		private bool defaultRegionsFolding;

		private bool defaultCommentFolding;

		private bool tabIsReindent;

		private bool autoInsertMatchingBracket;

		private bool smartSemicolonPlacement;

		private bool underlineErrors;

		private IndentStyle indentStyle;

		private EditorFontType editorFontType;

		private bool enableHighlightUsages;

		private LineEndingConversion lineEndingConversion;

		private bool useViModes;

		private bool onTheFlyFormatting;

		private string defaultEolMarker;

		private WordNavigationStyle wordNavigationStyle = (Platform.IsWindows ? WordNavigationStyle.Windows : WordNavigationStyle.Unix);

		private IWordFindStrategy wordFindStrategy;

		public static DefaultSourceEditorOptions Instance => instance;

		public bool EnableAutoCodeCompletion
		{
			get
			{
				return CompletionTextEditorExtension.EnableAutoCodeCompletion;
			}
			set
			{
				CompletionTextEditorExtension.EnableAutoCodeCompletion.Set(value);
			}
		}

		public bool DefaultRegionsFolding
		{
			get
			{
				return defaultRegionsFolding;
			}
			set
			{
				if (value != defaultRegionsFolding)
				{
					defaultRegionsFolding = value;
					PropertyService.Set("DefaultRegionsFolding", value);
					OnChanged(EventArgs.Empty);
				}
			}
		}

		public bool DefaultCommentFolding
		{
			get
			{
				return defaultCommentFolding;
			}
			set
			{
				if (value != defaultCommentFolding)
				{
					defaultCommentFolding = value;
					PropertyService.Set("DefaultCommentFolding", value);
					OnChanged(EventArgs.Empty);
				}
			}
		}

		public bool EnableSemanticHighlighting => true;

		public bool TabIsReindent
		{
			get
			{
				return tabIsReindent;
			}
			set
			{
				if (value != tabIsReindent)
				{
					tabIsReindent = value;
					PropertyService.Set("TabIsReindent", value);
					OnChanged(EventArgs.Empty);
				}
			}
		}

		public bool AutoInsertMatchingBracket
		{
			get
			{
				return autoInsertMatchingBracket;
			}
			set
			{
				if (value != autoInsertMatchingBracket)
				{
					autoInsertMatchingBracket = value;
					PropertyService.Set("AutoInsertMatchingBracket", value);
					OnChanged(EventArgs.Empty);
				}
			}
		}

		public bool SmartSemicolonPlacement
		{
			get
			{
				return smartSemicolonPlacement;
			}
			set
			{
				if (value != smartSemicolonPlacement)
				{
					smartSemicolonPlacement = value;
					PropertyService.Set("SmartSemicolonPlacement", value);
					OnChanged(EventArgs.Empty);
				}
			}
		}

		public bool UnderlineErrors
		{
			get
			{
				return underlineErrors;
			}
			set
			{
				if (value != underlineErrors)
				{
					underlineErrors = value;
					PropertyService.Set("UnderlineErrors", value);
					OnChanged(EventArgs.Empty);
				}
			}
		}

		public override IndentStyle IndentStyle
		{
			get
			{
				return indentStyle;
			}
			set
			{
				if (value != indentStyle)
				{
					indentStyle = value;
					PropertyService.Set("IndentStyle", value);
					OnChanged(EventArgs.Empty);
				}
			}
		}

		public EditorFontType EditorFontType
		{
			get
			{
				return editorFontType;
			}
			set
			{
				if (value != editorFontType)
				{
					editorFontType = value;
					PropertyService.Set("EditorFontType", value);
					OnChanged(EventArgs.Empty);
				}
			}
		}

		public bool EnableHighlightUsages
		{
			get
			{
				return enableHighlightUsages;
			}
			set
			{
				if (value != enableHighlightUsages)
				{
					enableHighlightUsages = value;
					PropertyService.Set("EnableHighlightUsages", value);
					OnChanged(EventArgs.Empty);
				}
			}
		}

		public LineEndingConversion LineEndingConversion
		{
			get
			{
				return lineEndingConversion;
			}
			set
			{
				if (value != lineEndingConversion)
				{
					lineEndingConversion = value;
					PropertyService.Set("LineEndingConversion", value);
					OnChanged(EventArgs.Empty);
				}
			}
		}

		public bool UseViModes
		{
			get
			{
				return useViModes;
			}
			set
			{
				if (useViModes != value)
				{
					useViModes = value;
					PropertyService.Set("UseViModes", value);
					OnChanged(EventArgs.Empty);
				}
			}
		}

		public bool OnTheFlyFormatting
		{
			get
			{
				return onTheFlyFormatting;
			}
			set
			{
				if (onTheFlyFormatting != value)
				{
					onTheFlyFormatting = value;
					PropertyService.Set("OnTheFlyFormatting", value);
					OnChanged(EventArgs.Empty);
				}
			}
		}

		public override string DefaultEolMarker => defaultEolMarker;

		[Obsolete("Use WordNavigationStyle")]
		public ControlLeftRightMode ControlLeftRightMode
		{
			get
			{
				if (WordNavigationStyle != WordNavigationStyle.Unix)
				{
					return ControlLeftRightMode.SharpDevelop;
				}
				return ControlLeftRightMode.MonoDevelop;
			}
			set
			{
				switch (value)
				{
				case ControlLeftRightMode.MonoDevelop:
				case ControlLeftRightMode.Emacs:
					WordNavigationStyle = WordNavigationStyle.Unix;
					break;
				default:
					WordNavigationStyle = WordNavigationStyle.Windows;
					break;
				}
			}
		}

		public WordNavigationStyle WordNavigationStyle
		{
			get
			{
				return wordNavigationStyle;
			}
			set
			{
				if (wordNavigationStyle != value)
				{
					wordNavigationStyle = value;
					PropertyService.Set("WordNavigationStyle", value);
					SetWordFindStrategy();
					OnChanged(EventArgs.Empty);
				}
			}
		}

		public override IWordFindStrategy WordFindStrategy
		{
			get
			{
				if (wordFindStrategy == null)
				{
					SetWordFindStrategy();
				}
				return wordFindStrategy;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override bool AllowTabsAfterNonTabs
		{
			set
			{
				if (value != AllowTabsAfterNonTabs)
				{
					PropertyService.Set("AllowTabsAfterNonTabs", value);
					base.AllowTabsAfterNonTabs = value;
				}
			}
		}

		public override bool TabsToSpaces
		{
			set
			{
				PropertyService.Set("TabsToSpaces", value);
				base.TabsToSpaces = value;
			}
		}

		public override int IndentationSize
		{
			set
			{
				PropertyService.Set("TabIndent", value);
				base.IndentationSize = value;
			}
		}

		public override int TabSize
		{
			get
			{
				return IndentationSize;
			}
			set
			{
				IndentationSize = value;
			}
		}

		public override bool RemoveTrailingWhitespaces
		{
			set
			{
				PropertyService.Set("RemoveTrailingWhitespaces", value);
				base.RemoveTrailingWhitespaces = value;
			}
		}

		public override bool ShowLineNumberMargin
		{
			set
			{
				PropertyService.Set("ShowLineNumberMargin", value);
				base.ShowLineNumberMargin = value;
			}
		}

		public override bool ShowFoldMargin
		{
			set
			{
				PropertyService.Set("ShowFoldMargin", value);
				base.ShowFoldMargin = value;
			}
		}

		public override bool HighlightCaretLine
		{
			set
			{
				PropertyService.Set("HighlightCaretLine", value);
				base.HighlightCaretLine = value;
			}
		}

		public override bool EnableSyntaxHighlighting
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public override bool HighlightMatchingBracket
		{
			set
			{
				PropertyService.Set("HighlightMatchingBracket", value);
				base.HighlightMatchingBracket = value;
			}
		}

		public override int RulerColumn
		{
			set
			{
				PropertyService.Set("RulerColumn", value);
				base.RulerColumn = value;
			}
		}

		public override bool ShowRuler
		{
			set
			{
				PropertyService.Set("ShowRuler", value);
				base.ShowRuler = value;
			}
		}

		public override bool EnableAnimations
		{
			set
			{
				PropertyService.Set("EnableAnimations", value);
				base.EnableAnimations = value;
			}
		}

		public override bool DrawIndentationMarkers
		{
			set
			{
				PropertyService.Set("DrawIndentationMarkers", value);
				base.DrawIndentationMarkers = value;
			}
		}

		public override ShowWhitespaces ShowWhitespaces
		{
			set
			{
				PropertyService.Set("ShowWhitespaces", value);
				base.ShowWhitespaces = value;
			}
		}

		public override IncludeWhitespaces IncludeWhitespaces
		{
			set
			{
				PropertyService.Set("IncludeWhitespaces", value);
				base.IncludeWhitespaces = value;
			}
		}

		public override bool WrapLines
		{
			set
			{
				PropertyService.Set("WrapLines", value);
				base.WrapLines = value;
			}
		}

		public override bool EnableQuickDiff
		{
			set
			{
				PropertyService.Set("EnableQuickDiff", value);
				base.EnableQuickDiff = value;
			}
		}

		public override string FontName
		{
			get
			{
				return FontService.FilterFontName(FontService.GetUnderlyingFontName("Editor"));
			}
			set
			{
				throw new InvalidOperationException("Set font through font service");
			}
		}

		public override string GutterFontName
		{
			get
			{
				return FontService.FilterFontName(FontService.GetUnderlyingFontName("Editor"));
			}
			set
			{
				throw new InvalidOperationException("Set font through font service");
			}
		}

		public override string ColorScheme
		{
			set
			{
				string text = ((!string.IsNullOrEmpty(value)) ? value : "Default");
				PropertyService.Set("ColorScheme", text);
				base.ColorScheme = text;
			}
		}

		public override bool GenerateFormattingUndoStep
		{
			set
			{
				PropertyService.Set("GenerateFormattingUndoStep", value);
				base.GenerateFormattingUndoStep = value;
			}
		}

		static DefaultSourceEditorOptions()
		{
			Init();
		}

		public static void Init()
		{
			if (!inited)
			{
				inited = true;
				TextStylePolicy defaultPolicy = PolicyService.GetDefaultPolicy<TextStylePolicy>("text/plain");
				instance = new DefaultSourceEditorOptions(defaultPolicy);
				PolicyService.DefaultPolicies.PolicyChanged += instance.HandlePolicyChanged;
			}
		}

		private void HandlePolicyChanged(object sender, PolicyChangedEventArgs args)
		{
			TextStylePolicy defaultPolicy = PolicyService.GetDefaultPolicy<TextStylePolicy>("text/plain");
			UpdateStylePolicy(defaultPolicy);
		}

		private DefaultSourceEditorOptions(TextStylePolicy currentPolicy)
		{
			LoadAllPrefs();
			UpdateStylePolicy(currentPolicy);
			PropertyService.PropertyChanged += UpdatePreferences;
			FontService.RegisterFontChangedCallback("Editor", UpdateFont);
			FontService.RegisterFontChangedCallback("Pad", UpdateFont);
		}

		public override void Dispose()
		{
			PropertyService.PropertyChanged -= UpdatePreferences;
			FontService.RemoveCallback(UpdateFont);
		}

		private void UpdateFont()
		{
			base.FontName = FontName;
			base.GutterFontName = GutterFontName;
			OnChanged(EventArgs.Empty);
		}

		private void UpdateStylePolicy(TextStylePolicy currentPolicy)
		{
			defaultEolMarker = TextStylePolicy.GetEolMarker(currentPolicy.EolMarker);
			base.TabsToSpaces = currentPolicy.TabsToSpaces;
			base.IndentationSize = currentPolicy.TabWidth;
			base.RulerColumn = currentPolicy.FileWidth;
			base.AllowTabsAfterNonTabs = !currentPolicy.NoTabsAfterNonTabs;
			base.RemoveTrailingWhitespaces = currentPolicy.RemoveTrailingWhitespace;
		}

		private void UpdatePreferences(object sender, PropertyChangedEventArgs args)
		{
			try
			{
				switch (args.Key)
				{
				case "TabIsReindent":
					TabIsReindent = (bool)args.NewValue;
					break;
				case "AutoInsertMatchingBracket":
					AutoInsertMatchingBracket = (bool)args.NewValue;
					break;
				case "UnderlineErrors":
					UnderlineErrors = (bool)args.NewValue;
					break;
				case "IndentStyle":
					if (args.NewValue == null)
					{
						LoggingService.LogWarning("tried to set indent style == null");
					}
					else if (!(args.NewValue is IndentStyle))
					{
						LoggingService.LogWarning(string.Concat("tried to set indent style to ", args.NewValue, " which isn't from type IndentStyle instead it is from:", args.NewValue.GetType()));
						IndentStyle = (IndentStyle)Enum.Parse(typeof(IndentStyle), args.NewValue.ToString());
					}
					else
					{
						IndentStyle = (IndentStyle)args.NewValue;
					}
					break;
				case "ShowLineNumberMargin":
					base.ShowLineNumberMargin = (bool)args.NewValue;
					break;
				case "ShowFoldMargin":
					base.ShowFoldMargin = (bool)args.NewValue;
					break;
				case "HighlightCaretLine":
					base.HighlightCaretLine = (bool)args.NewValue;
					break;
				case "HighlightMatchingBracket":
					base.HighlightMatchingBracket = (bool)args.NewValue;
					break;
				case "ShowRuler":
					base.ShowRuler = (bool)args.NewValue;
					break;
				case "FontName":
					base.FontName = (string)args.NewValue;
					break;
				case "GutterFontName":
					base.GutterFontName = (string)args.NewValue;
					break;
				case "ColorScheme":
					base.ColorScheme = (string)args.NewValue;
					break;
				case "DefaultRegionsFolding":
					DefaultRegionsFolding = (bool)args.NewValue;
					break;
				case "DefaultCommentFolding":
					DefaultCommentFolding = (bool)args.NewValue;
					break;
				case "UseViModes":
					UseViModes = (bool)args.NewValue;
					break;
				case "OnTheFlyFormatting":
					OnTheFlyFormatting = (bool)args.NewValue;
					break;
				case "WordNavigationStyle":
					WordNavigationStyle = (WordNavigationStyle)args.NewValue;
					break;
				case "EnableAnimations":
					base.EnableAnimations = (bool)args.NewValue;
					break;
				case "DrawIndentationMarkers":
					base.DrawIndentationMarkers = (bool)args.NewValue;
					break;
				case "EnableQuickDiff":
					base.EnableQuickDiff = (bool)args.NewValue;
					break;
				case "GenerateFormattingUndoStep":
					base.GenerateFormattingUndoStep = (bool)args.NewValue;
					break;
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("SourceEditorOptions error with property value for '" + (args.Key ?? "") + "'", ex);
			}
		}

		private void LoadAllPrefs()
		{
			tabIsReindent = PropertyService.Get("TabIsReindent", defaultValue: false);
			autoInsertMatchingBracket = PropertyService.Get("AutoInsertMatchingBracket", defaultValue: false);
			smartSemicolonPlacement = PropertyService.Get("SmartSemicolonPlacement", defaultValue: false);
			underlineErrors = PropertyService.Get("UnderlineErrors", defaultValue: true);
			indentStyle = PropertyService.Get("IndentStyle", IndentStyle.Smart);
			base.ShowLineNumberMargin = PropertyService.Get("ShowLineNumberMargin", defaultValue: true);
			base.ShowFoldMargin = PropertyService.Get("ShowFoldMargin", defaultValue: false);
			base.HighlightCaretLine = PropertyService.Get("HighlightCaretLine", defaultValue: false);
			base.HighlightMatchingBracket = PropertyService.Get("HighlightMatchingBracket", defaultValue: true);
			base.ShowRuler = PropertyService.Get("ShowRuler", defaultValue: false);
			base.FontName = PropertyService.Get("FontName", "Mono 10");
			base.GutterFontName = PropertyService.Get("GutterFontName", "");
			base.ColorScheme = PropertyService.Get("ColorScheme", "Default");
			defaultRegionsFolding = PropertyService.Get("DefaultRegionsFolding", defaultValue: false);
			defaultCommentFolding = PropertyService.Get("DefaultCommentFolding", defaultValue: true);
			useViModes = PropertyService.Get("UseViModes", defaultValue: false);
			onTheFlyFormatting = PropertyService.Get("OnTheFlyFormatting", defaultValue: true);
			WordNavigationStyle defaultValue = WordNavigationStyle.Unix;
			if (Platform.IsWindows || PropertyService.Get<string>("ControlLeftRightMode", null) == "SharpDevelop")
			{
				defaultValue = WordNavigationStyle.Windows;
			}
			WordNavigationStyle = PropertyService.Get("WordNavigationStyle", defaultValue);
			base.EnableAnimations = PropertyService.Get("EnableAnimations", defaultValue: true);
			EnableHighlightUsages = PropertyService.Get("EnableHighlightUsages", defaultValue: false);
			base.DrawIndentationMarkers = PropertyService.Get("DrawIndentationMarkers", defaultValue: false);
			lineEndingConversion = PropertyService.Get("LineEndingConversion", LineEndingConversion.Ask);
			base.GenerateFormattingUndoStep = PropertyService.Get("GenerateFormattingUndoStep", defaultValue: false);
			base.ShowWhitespaces = PropertyService.Get("ShowWhitespaces", ShowWhitespaces.Never);
			base.IncludeWhitespaces = PropertyService.Get("IncludeWhitespaces", IncludeWhitespaces.All);
			base.WrapLines = PropertyService.Get("WrapLines", defaultValue: false);
			base.EnableQuickDiff = PropertyService.Get("EnableQuickDiff", defaultValue: false);
		}

		private void SetWordFindStrategy()
		{
			if (useViModes)
			{
				wordFindStrategy = new ViWordFindStrategy();
				return;
			}
			WordNavigationStyle wordNavigationStyle = WordNavigationStyle;
			if (wordNavigationStyle == WordNavigationStyle.Windows)
			{
				wordFindStrategy = new SharpDevelopWordFindStrategy();
			}
			else
			{
				wordFindStrategy = new EmacsWordFindStrategy();
			}
		}
	}
}
