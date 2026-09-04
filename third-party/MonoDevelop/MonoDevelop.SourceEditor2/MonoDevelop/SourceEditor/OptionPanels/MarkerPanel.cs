using System;
using Gtk;
using Mono.TextEditor;
using Mono.Unix;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Dialogs;
using Stetic;

namespace MonoDevelop.SourceEditor.OptionPanels
{
	public class MarkerPanel : Bin, IOptionsPanel
	{
		private bool showLineNumbers;

		private bool underlineErrors;

		private bool highlightMatchingBracket;

		private bool highlightCurrentLine;

		private bool showRuler;

		private bool enableAnimation;

		private bool enableHighlightUsages;

		private bool drawIndentMarkers;

		private ShowWhitespaces showWhitespaces;

		private bool enableQuickDiff;

		private IncludeWhitespaces includeWhitespaces;

		private VBox vbox1;

		private Label GtkLabel9;

		private Alignment alignment1;

		private VBox vbox3;

		private CheckButton showLineNumbersCheckbutton;

		private CheckButton underlineErrorsCheckbutton;

		private CheckButton highlightMatchingBracketCheckbutton;

		private CheckButton highlightCurrentLineCheckbutton;

		private CheckButton showRulerCheckbutton;

		private CheckButton enableAnimationCheckbutton1;

		private CheckButton enableHighlightUsagesCheckbutton;

		private CheckButton drawIndentMarkersCheckbutton;

		private CheckButton enableQuickDiffCheckbutton;

		private Table table1;

		private CheckButton checkbuttonLineEndings;

		private CheckButton checkbuttonSpaces;

		private CheckButton checkbuttonTabs;

		private Label label1;

		private ComboBox showWhitespacesCombobox;

		public MarkerPanel()
		{
			Build();
		}

		public virtual Widget CreatePanelWidget()
		{
			showLineNumbersCheckbutton.Active = (showLineNumbers = DefaultSourceEditorOptions.Instance.ShowLineNumberMargin);
			showLineNumbersCheckbutton.Toggled += delegate
			{
				DefaultSourceEditorOptions.Instance.ShowLineNumberMargin = showLineNumbersCheckbutton.Active;
			};
			underlineErrorsCheckbutton.Active = (underlineErrors = DefaultSourceEditorOptions.Instance.UnderlineErrors);
			underlineErrorsCheckbutton.Toggled += delegate
			{
				DefaultSourceEditorOptions.Instance.UnderlineErrors = underlineErrorsCheckbutton.Active;
				foreach (Document document in IdeApp.Workbench.Documents)
				{
					document.StartReparseThread();
				}
			};
			highlightMatchingBracketCheckbutton.Active = (highlightMatchingBracket = DefaultSourceEditorOptions.Instance.HighlightMatchingBracket);
			highlightMatchingBracketCheckbutton.Toggled += delegate
			{
				DefaultSourceEditorOptions.Instance.HighlightMatchingBracket = highlightMatchingBracketCheckbutton.Active;
			};
			highlightCurrentLineCheckbutton.Active = (highlightCurrentLine = DefaultSourceEditorOptions.Instance.HighlightCaretLine);
			highlightCurrentLineCheckbutton.Toggled += delegate
			{
				DefaultSourceEditorOptions.Instance.HighlightCaretLine = highlightCurrentLineCheckbutton.Active;
			};
			showRulerCheckbutton.Active = (showRuler = DefaultSourceEditorOptions.Instance.ShowRuler);
			showRulerCheckbutton.Toggled += delegate
			{
				DefaultSourceEditorOptions.Instance.ShowRuler = showRulerCheckbutton.Active;
			};
			enableAnimationCheckbutton1.Active = (enableAnimation = DefaultSourceEditorOptions.Instance.EnableAnimations);
			enableAnimationCheckbutton1.Toggled += delegate
			{
				DefaultSourceEditorOptions.Instance.EnableAnimations = enableAnimationCheckbutton1.Active;
			};
			enableHighlightUsagesCheckbutton.Active = (enableHighlightUsages = DefaultSourceEditorOptions.Instance.EnableHighlightUsages);
			enableHighlightUsagesCheckbutton.Toggled += delegate
			{
				DefaultSourceEditorOptions.Instance.EnableHighlightUsages = enableHighlightUsagesCheckbutton.Active;
			};
			drawIndentMarkersCheckbutton.Active = (drawIndentMarkers = DefaultSourceEditorOptions.Instance.DrawIndentationMarkers);
			drawIndentMarkersCheckbutton.Toggled += delegate
			{
				DefaultSourceEditorOptions.Instance.DrawIndentationMarkers = drawIndentMarkersCheckbutton.Active;
			};
			showWhitespacesCombobox.AppendText(GettextCatalog.GetString("Never"));
			showWhitespacesCombobox.AppendText(GettextCatalog.GetString("Selection"));
			showWhitespacesCombobox.AppendText(GettextCatalog.GetString("Always"));
			showWhitespacesCombobox.Active = (int)(showWhitespaces = DefaultSourceEditorOptions.Instance.ShowWhitespaces);
			showWhitespacesCombobox.Changed += delegate
			{
				DefaultSourceEditorOptions.Instance.ShowWhitespaces = (ShowWhitespaces)showWhitespacesCombobox.Active;
			};
			checkbuttonSpaces.Active = DefaultSourceEditorOptions.Instance.IncludeWhitespaces.HasFlag(IncludeWhitespaces.Space);
			checkbuttonSpaces.Toggled += CheckbuttonSpaces_Toggled;
			checkbuttonTabs.Active = DefaultSourceEditorOptions.Instance.IncludeWhitespaces.HasFlag(IncludeWhitespaces.Tab);
			checkbuttonTabs.Toggled += CheckbuttonSpaces_Toggled;
			checkbuttonLineEndings.Active = DefaultSourceEditorOptions.Instance.IncludeWhitespaces.HasFlag(IncludeWhitespaces.LineEndings);
			checkbuttonLineEndings.Toggled += CheckbuttonSpaces_Toggled;
			includeWhitespaces = DefaultSourceEditorOptions.Instance.IncludeWhitespaces;
			enableQuickDiffCheckbutton.Active = (enableQuickDiff = DefaultSourceEditorOptions.Instance.EnableQuickDiff);
			enableQuickDiffCheckbutton.Toggled += delegate
			{
				DefaultSourceEditorOptions.Instance.EnableQuickDiff = enableQuickDiffCheckbutton.Active;
			};
			return this;
		}

		private void CheckbuttonSpaces_Toggled(object sender, EventArgs e)
		{
			IncludeWhitespaces includeWhitespaces = IncludeWhitespaces.None;
			if (checkbuttonSpaces.Active)
			{
				includeWhitespaces |= IncludeWhitespaces.Space;
			}
			if (checkbuttonTabs.Active)
			{
				includeWhitespaces |= IncludeWhitespaces.Tab;
			}
			if (checkbuttonLineEndings.Active)
			{
				includeWhitespaces |= IncludeWhitespaces.LineEndings;
			}
			DefaultSourceEditorOptions.Instance.IncludeWhitespaces = includeWhitespaces;
		}

		public virtual void ApplyChanges()
		{
			showLineNumbers = showLineNumbersCheckbutton.Active;
			underlineErrors = underlineErrorsCheckbutton.Active;
			highlightMatchingBracket = highlightMatchingBracketCheckbutton.Active;
			highlightCurrentLine = highlightCurrentLineCheckbutton.Active;
			showRuler = showRulerCheckbutton.Active;
			enableAnimation = enableAnimationCheckbutton1.Active;
			enableHighlightUsages = enableHighlightUsagesCheckbutton.Active;
			drawIndentMarkers = drawIndentMarkersCheckbutton.Active;
			showWhitespaces = (ShowWhitespaces)showWhitespacesCombobox.Active;
			enableQuickDiff = enableQuickDiffCheckbutton.Active;
			IncludeWhitespaces includeWhitespaces = IncludeWhitespaces.None;
			if (checkbuttonSpaces.Active)
			{
				includeWhitespaces |= IncludeWhitespaces.Space;
			}
			if (checkbuttonTabs.Active)
			{
				includeWhitespaces |= IncludeWhitespaces.Tab;
			}
			if (checkbuttonLineEndings.Active)
			{
				includeWhitespaces |= IncludeWhitespaces.LineEndings;
			}
			this.includeWhitespaces = includeWhitespaces;
		}

		protected override void OnDestroyed()
		{
			DefaultSourceEditorOptions.Instance.ShowLineNumberMargin = showLineNumbers;
			DefaultSourceEditorOptions.Instance.UnderlineErrors = underlineErrors;
			DefaultSourceEditorOptions.Instance.HighlightMatchingBracket = highlightMatchingBracket;
			DefaultSourceEditorOptions.Instance.HighlightCaretLine = highlightCurrentLine;
			DefaultSourceEditorOptions.Instance.ShowRuler = showRuler;
			DefaultSourceEditorOptions.Instance.EnableAnimations = enableAnimation;
			DefaultSourceEditorOptions.Instance.EnableHighlightUsages = enableHighlightUsages;
			DefaultSourceEditorOptions.Instance.DrawIndentationMarkers = drawIndentMarkers;
			DefaultSourceEditorOptions.Instance.ShowWhitespaces = showWhitespaces;
			DefaultSourceEditorOptions.Instance.EnableQuickDiff = enableQuickDiff;
			DefaultSourceEditorOptions.Instance.IncludeWhitespaces = includeWhitespaces;
			base.OnDestroyed();
		}

		public void Initialize(OptionsDialog dialog, object dataObject)
		{
		}

		public bool IsVisible()
		{
			return true;
		}

		public bool ValidateChanges()
		{
			return true;
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			Stetic.BinContainer.Attach(this);
			base.Name = "MonoDevelop.SourceEditor.OptionPanels.MarkerPanel";
			vbox1 = new VBox();
			vbox1.Name = "vbox1";
			vbox1.Spacing = 6;
			GtkLabel9 = new Label();
			GtkLabel9.Name = "GtkLabel9";
			GtkLabel9.Xalign = 0f;
			GtkLabel9.LabelProp = Catalog.GetString("<b>General</b>");
			GtkLabel9.UseMarkup = true;
			vbox1.Add(GtkLabel9);
			Box.BoxChild boxChild = (Box.BoxChild)vbox1[GtkLabel9];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			alignment1 = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment1.Name = "alignment1";
			alignment1.LeftPadding = 12u;
			vbox3 = new VBox();
			vbox3.Name = "vbox3";
			vbox3.Spacing = 6;
			showLineNumbersCheckbutton = new CheckButton();
			showLineNumbersCheckbutton.CanFocus = true;
			showLineNumbersCheckbutton.Name = "showLineNumbersCheckbutton";
			showLineNumbersCheckbutton.Label = Catalog.GetString("_Show line numbers");
			showLineNumbersCheckbutton.DrawIndicator = true;
			showLineNumbersCheckbutton.UseUnderline = true;
			vbox3.Add(showLineNumbersCheckbutton);
			Box.BoxChild boxChild2 = (Box.BoxChild)vbox3[showLineNumbersCheckbutton];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			underlineErrorsCheckbutton = new CheckButton();
			underlineErrorsCheckbutton.CanFocus = true;
			underlineErrorsCheckbutton.Name = "underlineErrorsCheckbutton";
			underlineErrorsCheckbutton.Label = Catalog.GetString("_Underline errors");
			underlineErrorsCheckbutton.DrawIndicator = true;
			underlineErrorsCheckbutton.UseUnderline = true;
			vbox3.Add(underlineErrorsCheckbutton);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox3[underlineErrorsCheckbutton];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			highlightMatchingBracketCheckbutton = new CheckButton();
			highlightMatchingBracketCheckbutton.CanFocus = true;
			highlightMatchingBracketCheckbutton.Name = "highlightMatchingBracketCheckbutton";
			highlightMatchingBracketCheckbutton.Label = Catalog.GetString("_Highlight matching braces");
			highlightMatchingBracketCheckbutton.DrawIndicator = true;
			highlightMatchingBracketCheckbutton.UseUnderline = true;
			vbox3.Add(highlightMatchingBracketCheckbutton);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox3[highlightMatchingBracketCheckbutton];
			boxChild4.Position = 2;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			highlightCurrentLineCheckbutton = new CheckButton();
			highlightCurrentLineCheckbutton.CanFocus = true;
			highlightCurrentLineCheckbutton.Name = "highlightCurrentLineCheckbutton";
			highlightCurrentLineCheckbutton.Label = Catalog.GetString("Highlight _current line");
			highlightCurrentLineCheckbutton.DrawIndicator = true;
			highlightCurrentLineCheckbutton.UseUnderline = true;
			vbox3.Add(highlightCurrentLineCheckbutton);
			Box.BoxChild boxChild5 = (Box.BoxChild)vbox3[highlightCurrentLineCheckbutton];
			boxChild5.Position = 3;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			showRulerCheckbutton = new CheckButton();
			showRulerCheckbutton.CanFocus = true;
			showRulerCheckbutton.Name = "showRulerCheckbutton";
			showRulerCheckbutton.Label = Catalog.GetString("Show _column ruler");
			showRulerCheckbutton.DrawIndicator = true;
			showRulerCheckbutton.UseUnderline = true;
			vbox3.Add(showRulerCheckbutton);
			Box.BoxChild boxChild6 = (Box.BoxChild)vbox3[showRulerCheckbutton];
			boxChild6.Position = 4;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			enableAnimationCheckbutton1 = new CheckButton();
			enableAnimationCheckbutton1.CanFocus = true;
			enableAnimationCheckbutton1.Name = "enableAnimationCheckbutton1";
			enableAnimationCheckbutton1.Label = Catalog.GetString("_Enable animations");
			enableAnimationCheckbutton1.DrawIndicator = true;
			enableAnimationCheckbutton1.UseUnderline = true;
			vbox3.Add(enableAnimationCheckbutton1);
			Box.BoxChild boxChild7 = (Box.BoxChild)vbox3[enableAnimationCheckbutton1];
			boxChild7.Position = 5;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			enableHighlightUsagesCheckbutton = new CheckButton();
			enableHighlightUsagesCheckbutton.CanFocus = true;
			enableHighlightUsagesCheckbutton.Name = "enableHighlightUsagesCheckbutton";
			enableHighlightUsagesCheckbutton.Label = Catalog.GetString("Highlight _identifier references");
			enableHighlightUsagesCheckbutton.DrawIndicator = true;
			enableHighlightUsagesCheckbutton.UseUnderline = true;
			vbox3.Add(enableHighlightUsagesCheckbutton);
			Box.BoxChild boxChild8 = (Box.BoxChild)vbox3[enableHighlightUsagesCheckbutton];
			boxChild8.Position = 6;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			drawIndentMarkersCheckbutton = new CheckButton();
			drawIndentMarkersCheckbutton.CanFocus = true;
			drawIndentMarkersCheckbutton.Name = "drawIndentMarkersCheckbutton";
			drawIndentMarkersCheckbutton.Label = Catalog.GetString("_Show indentation guides");
			drawIndentMarkersCheckbutton.DrawIndicator = true;
			drawIndentMarkersCheckbutton.UseUnderline = true;
			vbox3.Add(drawIndentMarkersCheckbutton);
			Box.BoxChild boxChild9 = (Box.BoxChild)vbox3[drawIndentMarkersCheckbutton];
			boxChild9.Position = 7;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			enableQuickDiffCheckbutton = new CheckButton();
			enableQuickDiffCheckbutton.CanFocus = true;
			enableQuickDiffCheckbutton.Name = "enableQuickDiffCheckbutton";
			enableQuickDiffCheckbutton.Label = Catalog.GetString("_Visualize changed lines");
			enableQuickDiffCheckbutton.DrawIndicator = true;
			enableQuickDiffCheckbutton.UseUnderline = true;
			vbox3.Add(enableQuickDiffCheckbutton);
			Box.BoxChild boxChild10 = (Box.BoxChild)vbox3[enableQuickDiffCheckbutton];
			boxChild10.Position = 8;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			table1 = new Table(4u, 4u, homogeneous: false);
			table1.Name = "table1";
			table1.RowSpacing = 6u;
			table1.ColumnSpacing = 6u;
			checkbuttonLineEndings = new CheckButton();
			checkbuttonLineEndings.CanFocus = true;
			checkbuttonLineEndings.Name = "checkbuttonLineEndings";
			checkbuttonLineEndings.Label = Catalog.GetString("Include Line Endings");
			checkbuttonLineEndings.DrawIndicator = true;
			checkbuttonLineEndings.UseUnderline = true;
			table1.Add(checkbuttonLineEndings);
			Table.TableChild tableChild = (Table.TableChild)table1[checkbuttonLineEndings];
			tableChild.TopAttach = 3u;
			tableChild.BottomAttach = 4u;
			tableChild.LeftAttach = 1u;
			tableChild.RightAttach = 4u;
			tableChild.XOptions = AttachOptions.Fill;
			tableChild.YOptions = AttachOptions.Fill;
			checkbuttonSpaces = new CheckButton();
			checkbuttonSpaces.CanFocus = true;
			checkbuttonSpaces.Name = "checkbuttonSpaces";
			checkbuttonSpaces.Label = Catalog.GetString("Include _Spaces");
			checkbuttonSpaces.DrawIndicator = true;
			checkbuttonSpaces.UseUnderline = true;
			table1.Add(checkbuttonSpaces);
			Table.TableChild tableChild2 = (Table.TableChild)table1[checkbuttonSpaces];
			tableChild2.TopAttach = 1u;
			tableChild2.BottomAttach = 2u;
			tableChild2.LeftAttach = 1u;
			tableChild2.RightAttach = 4u;
			tableChild2.XOptions = AttachOptions.Fill;
			tableChild2.YOptions = AttachOptions.Fill;
			checkbuttonTabs = new CheckButton();
			checkbuttonTabs.CanFocus = true;
			checkbuttonTabs.Name = "checkbuttonTabs";
			checkbuttonTabs.Label = Catalog.GetString("Include Tabs");
			checkbuttonTabs.DrawIndicator = true;
			checkbuttonTabs.UseUnderline = true;
			table1.Add(checkbuttonTabs);
			Table.TableChild tableChild3 = (Table.TableChild)table1[checkbuttonTabs];
			tableChild3.TopAttach = 2u;
			tableChild3.BottomAttach = 3u;
			tableChild3.LeftAttach = 1u;
			tableChild3.RightAttach = 4u;
			tableChild3.XOptions = AttachOptions.Fill;
			tableChild3.YOptions = AttachOptions.Fill;
			label1 = new Label();
			label1.Name = "label1";
			label1.LabelProp = Catalog.GetString("_Show invisible characters:");
			label1.UseUnderline = true;
			table1.Add(label1);
			Table.TableChild tableChild4 = (Table.TableChild)table1[label1];
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			showWhitespacesCombobox = ComboBox.NewText();
			showWhitespacesCombobox.Name = "showWhitespacesCombobox";
			table1.Add(showWhitespacesCombobox);
			Table.TableChild tableChild5 = (Table.TableChild)table1[showWhitespacesCombobox];
			tableChild5.LeftAttach = 1u;
			tableChild5.RightAttach = 4u;
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			vbox3.Add(table1);
			Box.BoxChild boxChild11 = (Box.BoxChild)vbox3[table1];
			boxChild11.Position = 9;
			boxChild11.Fill = false;
			alignment1.Add(vbox3);
			vbox1.Add(alignment1);
			Box.BoxChild boxChild12 = (Box.BoxChild)vbox1[alignment1];
			boxChild12.Position = 1;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			Add(vbox1);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			Show();
		}
	}
}
