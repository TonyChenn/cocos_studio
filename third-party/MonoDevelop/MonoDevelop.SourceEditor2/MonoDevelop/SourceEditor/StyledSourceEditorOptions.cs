using System;
using System.Collections.Generic;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Projects;
using MonoDevelop.Projects.Policies;
using Pango;

namespace MonoDevelop.SourceEditor
{
	internal class StyledSourceEditorOptions : ISourceEditorOptions, ITextEditorOptions, IDisposable
	{
		private PolicyContainer policyContainer;

		private EventHandler changed;

		private IEnumerable<string> mimeTypes;

		private TextStylePolicy currentPolicy;

		private string lastMimeType;

		private TextStylePolicy CurrentPolicy => currentPolicy;

		public bool OverrideDocumentEolMarker
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.OverrideDocumentEolMarker;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public string DefaultEolMarker
		{
			get
			{
				return TextStylePolicy.GetEolMarker(CurrentPolicy.EolMarker);
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public int RulerColumn
		{
			get
			{
				return CurrentPolicy.FileWidth;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public int TabSize
		{
			get
			{
				return CurrentPolicy.TabWidth;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public bool TabsToSpaces
		{
			get
			{
				return CurrentPolicy.TabsToSpaces;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public bool RemoveTrailingWhitespaces
		{
			get
			{
				return CurrentPolicy.RemoveTrailingWhitespace;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public bool AllowTabsAfterNonTabs
		{
			get
			{
				return !CurrentPolicy.NoTabsAfterNonTabs;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public int IndentationSize
		{
			get
			{
				return CurrentPolicy.IndentWidth;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public string IndentationString
		{
			get
			{
				if (!TabsToSpaces)
				{
					return "\t";
				}
				return new string(' ', TabSize);
			}
		}

		public bool CanResetZoom => DefaultSourceEditorOptions.Instance.CanResetZoom;

		public bool CanZoomIn => DefaultSourceEditorOptions.Instance.CanZoomIn;

		public bool CanZoomOut => DefaultSourceEditorOptions.Instance.CanZoomOut;

		public string ColorScheme
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.ColorScheme;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public bool EnableSyntaxHighlighting
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.EnableSyntaxHighlighting;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public FontDescription Font => DefaultSourceEditorOptions.Instance.Font;

		public string FontName
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.FontName;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public FontDescription GutterFont => DefaultSourceEditorOptions.Instance.GutterFont;

		public string GutterFontName
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.GutterFontName;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public bool HighlightCaretLine
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.HighlightCaretLine;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public bool HighlightMatchingBracket
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.HighlightMatchingBracket;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public bool ShowFoldMargin
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.ShowFoldMargin;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public bool ShowIconMargin
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.ShowIconMargin;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public bool ShowLineNumberMargin
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.ShowLineNumberMargin;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public bool ShowRuler
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.ShowRuler;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public bool EnableAnimations
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.EnableAnimations;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public IWordFindStrategy WordFindStrategy
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.WordFindStrategy;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public double Zoom
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.Zoom;
			}
			set
			{
				DefaultSourceEditorOptions.Instance.Zoom = value;
			}
		}

		public bool DrawIndentationMarkers
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.DrawIndentationMarkers;
			}
			set
			{
				DefaultSourceEditorOptions.Instance.DrawIndentationMarkers = value;
			}
		}

		public ShowWhitespaces ShowWhitespaces
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.ShowWhitespaces;
			}
			set
			{
				DefaultSourceEditorOptions.Instance.ShowWhitespaces = value;
			}
		}

		public IncludeWhitespaces IncludeWhitespaces
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.IncludeWhitespaces;
			}
			set
			{
				DefaultSourceEditorOptions.Instance.IncludeWhitespaces = value;
			}
		}

		public bool WrapLines
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.WrapLines;
			}
			set
			{
				DefaultSourceEditorOptions.Instance.WrapLines = value;
			}
		}

		public bool EnableQuickDiff
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.EnableQuickDiff;
			}
			set
			{
				DefaultSourceEditorOptions.Instance.EnableQuickDiff = value;
			}
		}

		public bool GenerateFormattingUndoStep
		{
			get
			{
				return DefaultSourceEditorOptions.Instance.GenerateFormattingUndoStep;
			}
			set
			{
				DefaultSourceEditorOptions.Instance.GenerateFormattingUndoStep = value;
			}
		}

		public bool AutoInsertMatchingBracket => DefaultSourceEditorOptions.Instance.AutoInsertMatchingBracket;

		public bool DefaultCommentFolding => DefaultSourceEditorOptions.Instance.DefaultCommentFolding;

		public bool DefaultRegionsFolding => DefaultSourceEditorOptions.Instance.DefaultRegionsFolding;

		public EditorFontType EditorFontType => DefaultSourceEditorOptions.Instance.EditorFontType;

		public bool EnableAutoCodeCompletion => DefaultSourceEditorOptions.Instance.EnableAutoCodeCompletion;

		public bool EnableSemanticHighlighting => DefaultSourceEditorOptions.Instance.EnableSemanticHighlighting;

		public IndentStyle IndentStyle
		{
			get
			{
				if ((DefaultSourceEditorOptions.Instance.IndentStyle == IndentStyle.Smart || DefaultSourceEditorOptions.Instance.IndentStyle == IndentStyle.Auto) && CurrentPolicy.RemoveTrailingWhitespace)
				{
					return IndentStyle.Virtual;
				}
				return DefaultSourceEditorOptions.Instance.IndentStyle;
			}
			set
			{
				throw new NotSupportedException("Use property 'IndentStyle' instead.");
			}
		}

		public bool TabIsReindent => DefaultSourceEditorOptions.Instance.TabIsReindent;

		public bool UnderlineErrors => DefaultSourceEditorOptions.Instance.UnderlineErrors;

		public bool UseViModes => DefaultSourceEditorOptions.Instance.UseViModes;

		public bool EnableSelectionWrappingKeys => DefaultSourceEditorOptions.Instance.AutoInsertMatchingBracket;

		public event EventHandler Changed
		{
			add
			{
				if (changed == null)
				{
					DefaultSourceEditorOptions.Instance.Changed += HandleDefaultsChanged;
				}
				changed = (EventHandler)Delegate.Combine(changed, value);
			}
			remove
			{
				changed = (EventHandler)Delegate.Remove(changed, value);
				if (changed == null)
				{
					DefaultSourceEditorOptions.Instance.Changed -= HandleDefaultsChanged;
				}
			}
		}

		public StyledSourceEditorOptions(Project styleParent, string mimeType)
		{
			UpdateStyleParent(styleParent, mimeType);
		}

		public void UpdateStyleParent(Project styleParent, string mimeType)
		{
			if (styleParent == null || policyContainer != styleParent.Policies || !(mimeType == lastMimeType))
			{
				lastMimeType = mimeType;
				if (policyContainer != null)
				{
					policyContainer.PolicyChanged -= HandlePolicyChanged;
				}
				if (string.IsNullOrEmpty(mimeType))
				{
					mimeType = "text/plain";
				}
				mimeTypes = DesktopService.GetMimeTypeInheritanceChain(mimeType);
				if (styleParent != null)
				{
					policyContainer = styleParent.Policies;
				}
				else
				{
					policyContainer = PolicyService.DefaultPolicies;
				}
				currentPolicy = policyContainer.Get<TextStylePolicy>(mimeTypes);
				policyContainer.PolicyChanged += HandlePolicyChanged;
				if (changed != null)
				{
					changed(this, EventArgs.Empty);
				}
			}
		}

		private void HandlePolicyChanged(object sender, PolicyChangedEventArgs args)
		{
			currentPolicy = policyContainer.Get<TextStylePolicy>(mimeTypes);
			if (changed != null)
			{
				changed(this, EventArgs.Empty);
			}
		}

		private void HandleDefaultsChanged(object sender, EventArgs e)
		{
			if (changed != null)
			{
				changed(this, EventArgs.Empty);
			}
		}

		public ColorScheme GetColorStyle()
		{
			return DefaultSourceEditorOptions.Instance.GetColorStyle();
		}

		public void ZoomIn()
		{
			DefaultSourceEditorOptions.Instance.ZoomIn();
		}

		public void ZoomOut()
		{
			DefaultSourceEditorOptions.Instance.ZoomOut();
		}

		public void ZoomReset()
		{
			DefaultSourceEditorOptions.Instance.ZoomReset();
		}

		public void Dispose()
		{
			mimeTypes = null;
			if (policyContainer != null)
			{
				policyContainer.PolicyChanged -= HandlePolicyChanged;
			}
			if (changed != null)
			{
				DefaultSourceEditorOptions.Instance.Changed -= HandleDefaultsChanged;
				changed = null;
			}
		}
	}
}
