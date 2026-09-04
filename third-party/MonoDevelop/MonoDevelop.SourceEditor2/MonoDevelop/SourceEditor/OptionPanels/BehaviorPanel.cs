using System;
using Gtk;
using Mono.TextEditor;
using Mono.Unix;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Dialogs;
using Stetic;

namespace MonoDevelop.SourceEditor.OptionPanels
{
	public class BehaviorPanel : Bin, IOptionsPanel
	{
		private VBox vbox1;

		private Label GtkLabel5;

		private Alignment alignment3;

		private VBox vbox4;

		private CheckButton autoInsertBraceCheckbutton;

		private HBox hbox2;

		private Label fixed1;

		private CheckButton smartSemicolonPlaceCheckbutton;

		private CheckButton checkbuttonOnTheFlyFormatting;

		private CheckButton checkbuttonFormatOnSave;

		private CheckButton checkbuttonAutoSetSearchPatternCasing;

		private CheckButton checkbuttonGenerateFormattingUndoStep;

		private Label GtkLabel6;

		private Alignment GtkAlignment;

		private VBox vbox2;

		private HBox hbox1;

		private Label label1;

		private ComboBox indentationCombobox;

		private CheckButton tabAsReindentCheckbutton;

		private Label GtkLabel8;

		private Alignment alignment4;

		private VBox vbox5;

		private CheckButton useViModesCheck;

		private HBox hbox3;

		private Label label2;

		private ComboBox controlLeftRightCombobox;

		public BehaviorPanel()
		{
			Build();
			indentationCombobox.InsertText(0, GettextCatalog.GetString("None"));
			indentationCombobox.InsertText(1, GettextCatalog.GetString("Automatic"));
			indentationCombobox.InsertText(2, GettextCatalog.GetString("Smart"));
			controlLeftRightCombobox.InsertText(0, GettextCatalog.GetString("Unix"));
			controlLeftRightCombobox.InsertText(1, GettextCatalog.GetString("Windows"));
			autoInsertBraceCheckbutton.Toggled += HandleAutoInsertBraceCheckbuttonToggled;
		}

		public virtual Widget CreatePanelWidget()
		{
			autoInsertBraceCheckbutton.Active = DefaultSourceEditorOptions.Instance.AutoInsertMatchingBracket;
			smartSemicolonPlaceCheckbutton.Active = DefaultSourceEditorOptions.Instance.SmartSemicolonPlacement;
			tabAsReindentCheckbutton.Active = DefaultSourceEditorOptions.Instance.TabIsReindent;
			indentationCombobox.Active = (int)DefaultSourceEditorOptions.Instance.IndentStyle;
			controlLeftRightCombobox.Active = (int)DefaultSourceEditorOptions.Instance.WordNavigationStyle;
			useViModesCheck.Active = DefaultSourceEditorOptions.Instance.UseViModes;
			checkbuttonOnTheFlyFormatting.Active = DefaultSourceEditorOptions.Instance.OnTheFlyFormatting;
			checkbuttonGenerateFormattingUndoStep.Active = DefaultSourceEditorOptions.Instance.GenerateFormattingUndoStep;
			checkbuttonFormatOnSave.Active = PropertyService.Get("AutoFormatDocumentOnSave", defaultValue: false);
			checkbuttonAutoSetSearchPatternCasing.Active = PropertyService.Get("AutoSetPatternCasing", defaultValue: false);
			HandleAutoInsertBraceCheckbuttonToggled(null, null);
			return this;
		}

		private void HandleAutoInsertBraceCheckbuttonToggled(object sender, EventArgs e)
		{
			smartSemicolonPlaceCheckbutton.Sensitive = autoInsertBraceCheckbutton.Active;
		}

		public virtual void ApplyChanges()
		{
			DefaultSourceEditorOptions.Instance.AutoInsertMatchingBracket = autoInsertBraceCheckbutton.Active;
			DefaultSourceEditorOptions.Instance.SmartSemicolonPlacement = smartSemicolonPlaceCheckbutton.Active;
			DefaultSourceEditorOptions.Instance.IndentStyle = (IndentStyle)indentationCombobox.Active;
			DefaultSourceEditorOptions.Instance.TabIsReindent = tabAsReindentCheckbutton.Active;
			DefaultSourceEditorOptions.Instance.WordNavigationStyle = (WordNavigationStyle)controlLeftRightCombobox.Active;
			DefaultSourceEditorOptions.Instance.UseViModes = useViModesCheck.Active;
			DefaultSourceEditorOptions.Instance.OnTheFlyFormatting = checkbuttonOnTheFlyFormatting.Active;
			DefaultSourceEditorOptions.Instance.GenerateFormattingUndoStep = checkbuttonGenerateFormattingUndoStep.Active;
			PropertyService.Set("AutoSetPatternCasing", checkbuttonAutoSetSearchPatternCasing.Active);
			PropertyService.Set("AutoFormatDocumentOnSave", checkbuttonFormatOnSave.Active);
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
			base.Name = "MonoDevelop.SourceEditor.OptionPanels.BehaviorPanel";
			vbox1 = new VBox();
			vbox1.Name = "vbox1";
			vbox1.Spacing = 6;
			GtkLabel5 = new Label();
			GtkLabel5.Name = "GtkLabel5";
			GtkLabel5.Xalign = 0f;
			GtkLabel5.LabelProp = Catalog.GetString("<b>Automatic behaviors</b>");
			GtkLabel5.UseMarkup = true;
			vbox1.Add(GtkLabel5);
			Box.BoxChild boxChild = (Box.BoxChild)vbox1[GtkLabel5];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			alignment3 = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment3.Name = "alignment3";
			alignment3.LeftPadding = 12u;
			vbox4 = new VBox();
			vbox4.Name = "vbox4";
			vbox4.Spacing = 6;
			autoInsertBraceCheckbutton = new CheckButton();
			autoInsertBraceCheckbutton.CanFocus = true;
			autoInsertBraceCheckbutton.Name = "autoInsertBraceCheckbutton";
			autoInsertBraceCheckbutton.Label = Catalog.GetString("_Insert matching brace");
			autoInsertBraceCheckbutton.DrawIndicator = true;
			autoInsertBraceCheckbutton.UseUnderline = true;
			vbox4.Add(autoInsertBraceCheckbutton);
			Box.BoxChild boxChild2 = (Box.BoxChild)vbox4[autoInsertBraceCheckbutton];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			hbox2 = new HBox();
			hbox2.Name = "hbox2";
			hbox2.Spacing = 6;
			fixed1 = new Label();
			fixed1.Name = "fixed1";
			hbox2.Add(fixed1);
			Box.BoxChild boxChild3 = (Box.BoxChild)hbox2[fixed1];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			boxChild3.Padding = 6u;
			smartSemicolonPlaceCheckbutton = new CheckButton();
			smartSemicolonPlaceCheckbutton.CanFocus = true;
			smartSemicolonPlaceCheckbutton.Name = "smartSemicolonPlaceCheckbutton";
			smartSemicolonPlaceCheckbutton.Label = Catalog.GetString("_Smart semicolon placement");
			smartSemicolonPlaceCheckbutton.Active = true;
			smartSemicolonPlaceCheckbutton.DrawIndicator = true;
			smartSemicolonPlaceCheckbutton.UseUnderline = true;
			hbox2.Add(smartSemicolonPlaceCheckbutton);
			Box.BoxChild boxChild4 = (Box.BoxChild)hbox2[smartSemicolonPlaceCheckbutton];
			boxChild4.Position = 1;
			vbox4.Add(hbox2);
			Box.BoxChild boxChild5 = (Box.BoxChild)vbox4[hbox2];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			checkbuttonOnTheFlyFormatting = new CheckButton();
			checkbuttonOnTheFlyFormatting.CanFocus = true;
			checkbuttonOnTheFlyFormatting.Name = "checkbuttonOnTheFlyFormatting";
			checkbuttonOnTheFlyFormatting.Label = Catalog.GetString("_Enable on the fly code formatting");
			checkbuttonOnTheFlyFormatting.DrawIndicator = true;
			checkbuttonOnTheFlyFormatting.UseUnderline = true;
			vbox4.Add(checkbuttonOnTheFlyFormatting);
			Box.BoxChild boxChild6 = (Box.BoxChild)vbox4[checkbuttonOnTheFlyFormatting];
			boxChild6.Position = 2;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			checkbuttonFormatOnSave = new CheckButton();
			checkbuttonFormatOnSave.CanFocus = true;
			checkbuttonFormatOnSave.Name = "checkbuttonFormatOnSave";
			checkbuttonFormatOnSave.Label = Catalog.GetString("_Format document on save");
			checkbuttonFormatOnSave.DrawIndicator = true;
			checkbuttonFormatOnSave.UseUnderline = true;
			vbox4.Add(checkbuttonFormatOnSave);
			Box.BoxChild boxChild7 = (Box.BoxChild)vbox4[checkbuttonFormatOnSave];
			boxChild7.Position = 3;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			checkbuttonAutoSetSearchPatternCasing = new CheckButton();
			checkbuttonAutoSetSearchPatternCasing.CanFocus = true;
			checkbuttonAutoSetSearchPatternCasing.Name = "checkbuttonAutoSetSearchPatternCasing";
			checkbuttonAutoSetSearchPatternCasing.Label = Catalog.GetString("_Automatically set search pattern case sensitivity");
			checkbuttonAutoSetSearchPatternCasing.DrawIndicator = true;
			checkbuttonAutoSetSearchPatternCasing.UseUnderline = true;
			vbox4.Add(checkbuttonAutoSetSearchPatternCasing);
			Box.BoxChild boxChild8 = (Box.BoxChild)vbox4[checkbuttonAutoSetSearchPatternCasing];
			boxChild8.Position = 4;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			checkbuttonGenerateFormattingUndoStep = new CheckButton();
			checkbuttonGenerateFormattingUndoStep.CanFocus = true;
			checkbuttonGenerateFormattingUndoStep.Name = "checkbuttonGenerateFormattingUndoStep";
			checkbuttonGenerateFormattingUndoStep.Label = Catalog.GetString("_Generate additional undo steps for formatting");
			checkbuttonGenerateFormattingUndoStep.DrawIndicator = true;
			checkbuttonGenerateFormattingUndoStep.UseUnderline = true;
			vbox4.Add(checkbuttonGenerateFormattingUndoStep);
			Box.BoxChild boxChild9 = (Box.BoxChild)vbox4[checkbuttonGenerateFormattingUndoStep];
			boxChild9.Position = 5;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			alignment3.Add(vbox4);
			vbox1.Add(alignment3);
			Box.BoxChild boxChild10 = (Box.BoxChild)vbox1[alignment3];
			boxChild10.Position = 1;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			GtkLabel6 = new Label();
			GtkLabel6.Name = "GtkLabel6";
			GtkLabel6.Xalign = 0f;
			GtkLabel6.LabelProp = Catalog.GetString("<b>Indentation</b>");
			GtkLabel6.UseMarkup = true;
			vbox1.Add(GtkLabel6);
			Box.BoxChild boxChild11 = (Box.BoxChild)vbox1[GtkLabel6];
			boxChild11.Position = 2;
			boxChild11.Expand = false;
			boxChild11.Fill = false;
			GtkAlignment = new Alignment(0f, 0f, 1f, 1f);
			GtkAlignment.Name = "GtkAlignment";
			GtkAlignment.LeftPadding = 12u;
			vbox2 = new VBox();
			vbox2.Name = "vbox2";
			vbox2.Spacing = 6;
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 6;
			label1 = new Label();
			label1.Name = "label1";
			label1.LabelProp = Catalog.GetString("_Indentation mode:");
			label1.UseUnderline = true;
			hbox1.Add(label1);
			Box.BoxChild boxChild12 = (Box.BoxChild)hbox1[label1];
			boxChild12.Position = 0;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			indentationCombobox = ComboBox.NewText();
			indentationCombobox.Name = "indentationCombobox";
			hbox1.Add(indentationCombobox);
			Box.BoxChild boxChild13 = (Box.BoxChild)hbox1[indentationCombobox];
			boxChild13.Position = 1;
			boxChild13.Expand = false;
			boxChild13.Fill = false;
			vbox2.Add(hbox1);
			Box.BoxChild boxChild14 = (Box.BoxChild)vbox2[hbox1];
			boxChild14.Position = 0;
			boxChild14.Expand = false;
			boxChild14.Fill = false;
			tabAsReindentCheckbutton = new CheckButton();
			tabAsReindentCheckbutton.CanFocus = true;
			tabAsReindentCheckbutton.Name = "tabAsReindentCheckbutton";
			tabAsReindentCheckbutton.Label = Catalog.GetString("Interpret tab _keystroke as reindent command");
			tabAsReindentCheckbutton.DrawIndicator = true;
			tabAsReindentCheckbutton.UseUnderline = true;
			vbox2.Add(tabAsReindentCheckbutton);
			Box.BoxChild boxChild15 = (Box.BoxChild)vbox2[tabAsReindentCheckbutton];
			boxChild15.Position = 1;
			boxChild15.Expand = false;
			boxChild15.Fill = false;
			GtkAlignment.Add(vbox2);
			vbox1.Add(GtkAlignment);
			Box.BoxChild boxChild16 = (Box.BoxChild)vbox1[GtkAlignment];
			boxChild16.Position = 3;
			boxChild16.Expand = false;
			boxChild16.Fill = false;
			GtkLabel8 = new Label();
			GtkLabel8.Name = "GtkLabel8";
			GtkLabel8.Xalign = 0f;
			GtkLabel8.LabelProp = Catalog.GetString("<b>Navigation</b>");
			GtkLabel8.UseMarkup = true;
			vbox1.Add(GtkLabel8);
			Box.BoxChild boxChild17 = (Box.BoxChild)vbox1[GtkLabel8];
			boxChild17.Position = 4;
			boxChild17.Expand = false;
			boxChild17.Fill = false;
			alignment4 = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment4.Name = "alignment4";
			alignment4.LeftPadding = 12u;
			vbox5 = new VBox();
			vbox5.Name = "vbox5";
			vbox5.Spacing = 6;
			useViModesCheck = new CheckButton();
			useViModesCheck.CanFocus = true;
			useViModesCheck.Name = "useViModesCheck";
			useViModesCheck.Label = Catalog.GetString("Use _Vi modes");
			useViModesCheck.DrawIndicator = true;
			useViModesCheck.UseUnderline = true;
			vbox5.Add(useViModesCheck);
			Box.BoxChild boxChild18 = (Box.BoxChild)vbox5[useViModesCheck];
			boxChild18.Position = 0;
			boxChild18.Expand = false;
			boxChild18.Fill = false;
			hbox3 = new HBox();
			hbox3.Name = "hbox3";
			hbox3.Spacing = 6;
			label2 = new Label();
			label2.Name = "label2";
			label2.LabelProp = Catalog.GetString("Word _break mode:");
			label2.UseUnderline = true;
			hbox3.Add(label2);
			Box.BoxChild boxChild19 = (Box.BoxChild)hbox3[label2];
			boxChild19.Position = 0;
			boxChild19.Expand = false;
			boxChild19.Fill = false;
			controlLeftRightCombobox = ComboBox.NewText();
			controlLeftRightCombobox.Name = "controlLeftRightCombobox";
			hbox3.Add(controlLeftRightCombobox);
			Box.BoxChild boxChild20 = (Box.BoxChild)hbox3[controlLeftRightCombobox];
			boxChild20.Position = 1;
			boxChild20.Expand = false;
			boxChild20.Fill = false;
			vbox5.Add(hbox3);
			Box.BoxChild boxChild21 = (Box.BoxChild)vbox5[hbox3];
			boxChild21.Position = 1;
			boxChild21.Expand = false;
			boxChild21.Fill = false;
			alignment4.Add(vbox5);
			vbox1.Add(alignment4);
			Box.BoxChild boxChild22 = (Box.BoxChild)vbox1[alignment4];
			boxChild22.Position = 5;
			boxChild22.Expand = false;
			boxChild22.Fill = false;
			Add(vbox1);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			Show();
		}
	}
}
