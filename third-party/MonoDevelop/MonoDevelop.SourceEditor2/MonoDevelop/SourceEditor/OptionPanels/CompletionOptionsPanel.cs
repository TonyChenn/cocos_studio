using System;
using System.ComponentModel;
using GLib;
using Gtk;
using Mono.Unix;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide.Gui.Dialogs;
using Stetic;

namespace MonoDevelop.SourceEditor.OptionPanels
{
	[ToolboxItem(false)]
	internal class CompletionOptionsPanel : Bin, IOptionsPanel
	{
		private VBox vbox1;

		private Alignment alignment3;

		private VBox vbox5;

		private CheckButton autoCodeCompletionCheckbutton;

		private HBox hbox4;

		private Fixed fixed3;

		private CheckButton includeKeywordsCheckbutton;

		private HBox hbox5;

		private Fixed fixed4;

		private CheckButton includeCodeSnippetsCheckbutton;

		private CheckButton showImportsCheckbutton;

		private CheckButton insertParenthesesCheckbutton;

		private HBox hbox2;

		private Fixed fixed1;

		private RadioButton openingRadiobutton;

		private HBox hbox3;

		private Fixed fixed2;

		private RadioButton bothRadiobutton;

		public CompletionOptionsPanel()
		{
			Build();
			insertParenthesesCheckbutton.Toggled += InsertParensToggled;
			autoCodeCompletionCheckbutton.Toggled += AutomaticCompletionToggled;
			includeKeywordsCheckbutton.Visible = (includeCodeSnippetsCheckbutton.Visible = false);
			hbox4.Visible = (hbox5.Visible = false);
		}

		private void InsertParensToggled(object sender, EventArgs e)
		{
			RadioButton radioButton = openingRadiobutton;
			bool sensitive = (bothRadiobutton.Sensitive = insertParenthesesCheckbutton.Active);
			radioButton.Sensitive = sensitive;
		}

		void IOptionsPanel.Initialize(OptionsDialog dialog, object dataObject)
		{
		}

		Widget IOptionsPanel.CreatePanelWidget()
		{
			autoCodeCompletionCheckbutton.Active = DefaultSourceEditorOptions.Instance.EnableAutoCodeCompletion;
			showImportsCheckbutton.Active = CompletionTextEditorExtension.AddImportedItemsToCompletionList;
			includeKeywordsCheckbutton.Active = CompletionTextEditorExtension.IncludeKeywordsInCompletionList;
			includeCodeSnippetsCheckbutton.Active = CompletionTextEditorExtension.IncludeCodeSnippetsInCompletionList;
			insertParenthesesCheckbutton.Active = CompletionTextEditorExtension.AddParenthesesAfterCompletion;
			openingRadiobutton.Active = CompletionTextEditorExtension.AddOpeningOnly;
			bothRadiobutton.Active = !CompletionTextEditorExtension.AddOpeningOnly;
			InsertParensToggled(this, EventArgs.Empty);
			AutomaticCompletionToggled(this, EventArgs.Empty);
			return this;
		}

		private void AutomaticCompletionToggled(object sender, EventArgs e)
		{
			CheckButton checkButton = includeKeywordsCheckbutton;
			bool sensitive = (includeCodeSnippetsCheckbutton.Sensitive = !autoCodeCompletionCheckbutton.Active);
			checkButton.Sensitive = sensitive;
		}

		bool IOptionsPanel.IsVisible()
		{
			return true;
		}

		bool IOptionsPanel.ValidateChanges()
		{
			return true;
		}

		void IOptionsPanel.ApplyChanges()
		{
			DefaultSourceEditorOptions.Instance.EnableAutoCodeCompletion = autoCodeCompletionCheckbutton.Active;
			CompletionTextEditorExtension.AddImportedItemsToCompletionList.Value = showImportsCheckbutton.Active;
			CompletionTextEditorExtension.IncludeKeywordsInCompletionList.Value = includeKeywordsCheckbutton.Active;
			CompletionTextEditorExtension.IncludeCodeSnippetsInCompletionList.Value = includeCodeSnippetsCheckbutton.Active;
			CompletionTextEditorExtension.AddParenthesesAfterCompletion.Value = insertParenthesesCheckbutton.Active;
			CompletionTextEditorExtension.AddOpeningOnly.Value = openingRadiobutton.Active;
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			Stetic.BinContainer.Attach(this);
			base.Name = "MonoDevelop.SourceEditor.OptionPanels.CompletionOptionsPanel";
			vbox1 = new VBox();
			vbox1.Name = "vbox1";
			vbox1.Spacing = 6;
			alignment3 = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment3.Name = "alignment3";
			alignment3.LeftPadding = 12u;
			vbox5 = new VBox();
			vbox5.Name = "vbox5";
			vbox5.Spacing = 6;
			autoCodeCompletionCheckbutton = new CheckButton();
			autoCodeCompletionCheckbutton.CanFocus = true;
			autoCodeCompletionCheckbutton.Name = "autoCodeCompletionCheckbutton";
			autoCodeCompletionCheckbutton.Label = Catalog.GetString("_Show completion list after a character is typed");
			autoCodeCompletionCheckbutton.DrawIndicator = true;
			autoCodeCompletionCheckbutton.UseUnderline = true;
			vbox5.Add(autoCodeCompletionCheckbutton);
			Box.BoxChild boxChild = (Box.BoxChild)vbox5[autoCodeCompletionCheckbutton];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			hbox4 = new HBox();
			hbox4.Name = "hbox4";
			hbox4.Spacing = 6;
			fixed3 = new Fixed();
			fixed3.Name = "fixed3";
			fixed3.HasWindow = false;
			hbox4.Add(fixed3);
			Box.BoxChild boxChild2 = (Box.BoxChild)hbox4[fixed3];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Padding = 6u;
			includeKeywordsCheckbutton = new CheckButton();
			includeKeywordsCheckbutton.CanFocus = true;
			includeKeywordsCheckbutton.Name = "includeKeywordsCheckbutton";
			includeKeywordsCheckbutton.Label = Catalog.GetString("Include _keywords in completion list");
			includeKeywordsCheckbutton.DrawIndicator = true;
			includeKeywordsCheckbutton.UseUnderline = true;
			hbox4.Add(includeKeywordsCheckbutton);
			Box.BoxChild boxChild3 = (Box.BoxChild)hbox4[includeKeywordsCheckbutton];
			boxChild3.Position = 1;
			vbox5.Add(hbox4);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox5[hbox4];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			hbox5 = new HBox();
			hbox5.Name = "hbox5";
			hbox5.Spacing = 6;
			fixed4 = new Fixed();
			fixed4.Name = "fixed4";
			fixed4.HasWindow = false;
			hbox5.Add(fixed4);
			Box.BoxChild boxChild5 = (Box.BoxChild)hbox5[fixed4];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Padding = 6u;
			includeCodeSnippetsCheckbutton = new CheckButton();
			includeCodeSnippetsCheckbutton.CanFocus = true;
			includeCodeSnippetsCheckbutton.Name = "includeCodeSnippetsCheckbutton";
			includeCodeSnippetsCheckbutton.Label = Catalog.GetString("Include _code snippets in completion list");
			includeCodeSnippetsCheckbutton.DrawIndicator = true;
			includeCodeSnippetsCheckbutton.UseUnderline = true;
			hbox5.Add(includeCodeSnippetsCheckbutton);
			Box.BoxChild boxChild6 = (Box.BoxChild)hbox5[includeCodeSnippetsCheckbutton];
			boxChild6.Position = 1;
			vbox5.Add(hbox5);
			Box.BoxChild boxChild7 = (Box.BoxChild)vbox5[hbox5];
			boxChild7.Position = 2;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			showImportsCheckbutton = new CheckButton();
			showImportsCheckbutton.CanFocus = true;
			showImportsCheckbutton.Name = "showImportsCheckbutton";
			showImportsCheckbutton.Label = Catalog.GetString("_Show import items");
			showImportsCheckbutton.DrawIndicator = true;
			showImportsCheckbutton.UseUnderline = true;
			vbox5.Add(showImportsCheckbutton);
			Box.BoxChild boxChild8 = (Box.BoxChild)vbox5[showImportsCheckbutton];
			boxChild8.Position = 3;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			insertParenthesesCheckbutton = new CheckButton();
			insertParenthesesCheckbutton.CanFocus = true;
			insertParenthesesCheckbutton.Name = "insertParenthesesCheckbutton";
			insertParenthesesCheckbutton.Label = Catalog.GetString("A_utomatically insert parentheses after completion:");
			insertParenthesesCheckbutton.DrawIndicator = true;
			insertParenthesesCheckbutton.UseUnderline = true;
			vbox5.Add(insertParenthesesCheckbutton);
			Box.BoxChild boxChild9 = (Box.BoxChild)vbox5[insertParenthesesCheckbutton];
			boxChild9.Position = 4;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			hbox2 = new HBox();
			hbox2.Name = "hbox2";
			hbox2.Spacing = 6;
			fixed1 = new Fixed();
			fixed1.Name = "fixed1";
			fixed1.HasWindow = false;
			hbox2.Add(fixed1);
			Box.BoxChild boxChild10 = (Box.BoxChild)hbox2[fixed1];
			boxChild10.Position = 0;
			boxChild10.Expand = false;
			boxChild10.Padding = 6u;
			openingRadiobutton = new RadioButton(Catalog.GetString("_Opening only"));
			openingRadiobutton.CanFocus = true;
			openingRadiobutton.Name = "openingRadiobutton";
			openingRadiobutton.DrawIndicator = true;
			openingRadiobutton.UseUnderline = true;
			openingRadiobutton.Group = new SList(IntPtr.Zero);
			hbox2.Add(openingRadiobutton);
			Box.BoxChild boxChild11 = (Box.BoxChild)hbox2[openingRadiobutton];
			boxChild11.Position = 1;
			vbox5.Add(hbox2);
			Box.BoxChild boxChild12 = (Box.BoxChild)vbox5[hbox2];
			boxChild12.Position = 5;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			hbox3 = new HBox();
			hbox3.Name = "hbox3";
			hbox3.Spacing = 6;
			fixed2 = new Fixed();
			fixed2.Name = "fixed2";
			fixed2.HasWindow = false;
			hbox3.Add(fixed2);
			Box.BoxChild boxChild13 = (Box.BoxChild)hbox3[fixed2];
			boxChild13.Position = 0;
			boxChild13.Expand = false;
			boxChild13.Padding = 6u;
			bothRadiobutton = new RadioButton(Catalog.GetString("_Both opening and closing"));
			bothRadiobutton.CanFocus = true;
			bothRadiobutton.Name = "bothRadiobutton";
			bothRadiobutton.DrawIndicator = true;
			bothRadiobutton.UseUnderline = true;
			bothRadiobutton.Group = openingRadiobutton.Group;
			hbox3.Add(bothRadiobutton);
			Box.BoxChild boxChild14 = (Box.BoxChild)hbox3[bothRadiobutton];
			boxChild14.Position = 1;
			vbox5.Add(hbox3);
			Box.BoxChild boxChild15 = (Box.BoxChild)vbox5[hbox3];
			boxChild15.Position = 6;
			boxChild15.Expand = false;
			boxChild15.Fill = false;
			alignment3.Add(vbox5);
			vbox1.Add(alignment3);
			Box.BoxChild boxChild16 = (Box.BoxChild)vbox1[alignment3];
			boxChild16.Position = 0;
			boxChild16.Expand = false;
			boxChild16.Fill = false;
			Add(vbox1);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			Hide();
		}
	}
}
