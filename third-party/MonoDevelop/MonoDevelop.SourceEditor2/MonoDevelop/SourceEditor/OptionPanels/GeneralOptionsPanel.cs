using Gtk;
using Mono.Unix;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Dialogs;
using Stetic;

namespace MonoDevelop.SourceEditor.OptionPanels
{
	internal class GeneralOptionsPanel : Bin, IOptionsPanel
	{
		private VBox vbox1;

		private Label GtkLabel13;

		private Alignment alignment2;

		private VBox vbox4;

		private HBox hbox3;

		private Fixed fixed2;

		private HBox hbox1;

		private Label label1;

		private ComboBox comboboxLineEndings;

		private Label GtkLabel14;

		private Alignment alignment3;

		private VBox vbox5;

		private CheckButton foldingCheckbutton;

		private CheckButton foldregionsCheckbutton;

		private CheckButton foldCommentsCheckbutton;

		private Label GtkLabel15;

		private Alignment alignment4;

		private VBox vbox6;

		private CheckButton wordWrapCheckbutton;

		private CheckButton antiAliasingCheckbutton;

		public GeneralOptionsPanel()
		{
			Build();
			comboboxLineEndings.AppendText(GettextCatalog.GetString("Always ask for conversion"));
			comboboxLineEndings.AppendText(GettextCatalog.GetString("Leave line endings as is"));
			comboboxLineEndings.AppendText(GettextCatalog.GetString("Always convert line endings"));
			comboboxLineEndings.Active = (int)DefaultSourceEditorOptions.Instance.LineEndingConversion;
		}

		public virtual Widget CreatePanelWidget()
		{
			foldingCheckbutton.Active = DefaultSourceEditorOptions.Instance.ShowFoldMargin;
			foldregionsCheckbutton.Active = DefaultSourceEditorOptions.Instance.DefaultRegionsFolding;
			foldCommentsCheckbutton.Active = DefaultSourceEditorOptions.Instance.DefaultCommentFolding;
			wordWrapCheckbutton.Visible = false;
			antiAliasingCheckbutton.Visible = false;
			GtkLabel15.Visible = false;
			return this;
		}

		public virtual void ApplyChanges()
		{
			DefaultSourceEditorOptions.Instance.DefaultRegionsFolding = foldregionsCheckbutton.Active;
			DefaultSourceEditorOptions.Instance.DefaultCommentFolding = foldCommentsCheckbutton.Active;
			DefaultSourceEditorOptions.Instance.LineEndingConversion = (LineEndingConversion)comboboxLineEndings.Active;
			if (DefaultSourceEditorOptions.Instance.ShowFoldMargin != foldingCheckbutton.Active)
			{
				DefaultSourceEditorOptions.Instance.ShowFoldMargin = foldingCheckbutton.Active;
				HighlightingPanel.UpdateActiveDocument();
			}
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
			base.Name = "MonoDevelop.SourceEditor.OptionPanels.GeneralOptionsPanel";
			vbox1 = new VBox();
			vbox1.Name = "vbox1";
			vbox1.Spacing = 6;
			GtkLabel13 = new Label();
			GtkLabel13.Name = "GtkLabel13";
			GtkLabel13.Xalign = 0f;
			GtkLabel13.LabelProp = Catalog.GetString("<b>Coding</b>");
			GtkLabel13.UseMarkup = true;
			vbox1.Add(GtkLabel13);
			Box.BoxChild boxChild = (Box.BoxChild)vbox1[GtkLabel13];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			alignment2 = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment2.Name = "alignment2";
			alignment2.LeftPadding = 12u;
			vbox4 = new VBox();
			vbox4.Name = "vbox4";
			vbox4.Spacing = 6;
			hbox3 = new HBox();
			hbox3.Name = "hbox3";
			hbox3.Spacing = 6;
			fixed2 = new Fixed();
			fixed2.Name = "fixed2";
			fixed2.HasWindow = false;
			hbox3.Add(fixed2);
			Box.BoxChild boxChild2 = (Box.BoxChild)hbox3[fixed2];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Padding = 6u;
			vbox4.Add(hbox3);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox4[hbox3];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 6;
			label1 = new Label();
			label1.Name = "label1";
			label1.LabelProp = Catalog.GetString("_Line ending conversion:");
			label1.UseUnderline = true;
			hbox1.Add(label1);
			Box.BoxChild boxChild4 = (Box.BoxChild)hbox1[label1];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			comboboxLineEndings = ComboBox.NewText();
			comboboxLineEndings.Name = "comboboxLineEndings";
			hbox1.Add(comboboxLineEndings);
			Box.BoxChild boxChild5 = (Box.BoxChild)hbox1[comboboxLineEndings];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			vbox4.Add(hbox1);
			Box.BoxChild boxChild6 = (Box.BoxChild)vbox4[hbox1];
			boxChild6.Position = 1;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			alignment2.Add(vbox4);
			vbox1.Add(alignment2);
			Box.BoxChild boxChild7 = (Box.BoxChild)vbox1[alignment2];
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			GtkLabel14 = new Label();
			GtkLabel14.Name = "GtkLabel14";
			GtkLabel14.Xalign = 0f;
			GtkLabel14.LabelProp = Catalog.GetString("<b>Code Folding</b>");
			GtkLabel14.UseMarkup = true;
			vbox1.Add(GtkLabel14);
			Box.BoxChild boxChild8 = (Box.BoxChild)vbox1[GtkLabel14];
			boxChild8.Position = 2;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			alignment3 = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment3.Name = "alignment3";
			alignment3.LeftPadding = 12u;
			vbox5 = new VBox();
			vbox5.Name = "vbox5";
			vbox5.Spacing = 6;
			foldingCheckbutton = new CheckButton();
			foldingCheckbutton.CanFocus = true;
			foldingCheckbutton.Name = "foldingCheckbutton";
			foldingCheckbutton.Label = Catalog.GetString("Enable code _folding");
			foldingCheckbutton.DrawIndicator = true;
			foldingCheckbutton.UseUnderline = true;
			vbox5.Add(foldingCheckbutton);
			Box.BoxChild boxChild9 = (Box.BoxChild)vbox5[foldingCheckbutton];
			boxChild9.Position = 0;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			foldregionsCheckbutton = new CheckButton();
			foldregionsCheckbutton.CanFocus = true;
			foldregionsCheckbutton.Name = "foldregionsCheckbutton";
			foldregionsCheckbutton.Label = Catalog.GetString("Fold #_regions by default");
			foldregionsCheckbutton.DrawIndicator = true;
			foldregionsCheckbutton.UseUnderline = true;
			vbox5.Add(foldregionsCheckbutton);
			Box.BoxChild boxChild10 = (Box.BoxChild)vbox5[foldregionsCheckbutton];
			boxChild10.Position = 1;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			foldCommentsCheckbutton = new CheckButton();
			foldCommentsCheckbutton.CanFocus = true;
			foldCommentsCheckbutton.Name = "foldCommentsCheckbutton";
			foldCommentsCheckbutton.Label = Catalog.GetString("Fold _comments by default");
			foldCommentsCheckbutton.DrawIndicator = true;
			foldCommentsCheckbutton.UseUnderline = true;
			vbox5.Add(foldCommentsCheckbutton);
			Box.BoxChild boxChild11 = (Box.BoxChild)vbox5[foldCommentsCheckbutton];
			boxChild11.Position = 2;
			boxChild11.Expand = false;
			boxChild11.Fill = false;
			alignment3.Add(vbox5);
			vbox1.Add(alignment3);
			Box.BoxChild boxChild12 = (Box.BoxChild)vbox1[alignment3];
			boxChild12.Position = 3;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			GtkLabel15 = new Label();
			GtkLabel15.Name = "GtkLabel15";
			GtkLabel15.Xalign = 0f;
			GtkLabel15.LabelProp = Catalog.GetString("<b>Appearance</b>");
			GtkLabel15.UseMarkup = true;
			vbox1.Add(GtkLabel15);
			Box.BoxChild boxChild13 = (Box.BoxChild)vbox1[GtkLabel15];
			boxChild13.Position = 4;
			boxChild13.Expand = false;
			boxChild13.Fill = false;
			alignment4 = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment4.Name = "alignment4";
			alignment4.LeftPadding = 12u;
			vbox6 = new VBox();
			vbox6.Name = "vbox6";
			vbox6.Spacing = 6;
			wordWrapCheckbutton = new CheckButton();
			wordWrapCheckbutton.CanFocus = true;
			wordWrapCheckbutton.Name = "wordWrapCheckbutton";
			wordWrapCheckbutton.Label = Catalog.GetString("_Word wrap");
			wordWrapCheckbutton.DrawIndicator = true;
			wordWrapCheckbutton.UseUnderline = true;
			vbox6.Add(wordWrapCheckbutton);
			Box.BoxChild boxChild14 = (Box.BoxChild)vbox6[wordWrapCheckbutton];
			boxChild14.Position = 0;
			boxChild14.Expand = false;
			boxChild14.Fill = false;
			antiAliasingCheckbutton = new CheckButton();
			antiAliasingCheckbutton.CanFocus = true;
			antiAliasingCheckbutton.Name = "antiAliasingCheckbutton";
			antiAliasingCheckbutton.Label = Catalog.GetString("_Use anti aliasing");
			antiAliasingCheckbutton.DrawIndicator = true;
			antiAliasingCheckbutton.UseUnderline = true;
			vbox6.Add(antiAliasingCheckbutton);
			Box.BoxChild boxChild15 = (Box.BoxChild)vbox6[antiAliasingCheckbutton];
			boxChild15.Position = 1;
			boxChild15.Expand = false;
			boxChild15.Fill = false;
			alignment4.Add(vbox6);
			vbox1.Add(alignment4);
			Box.BoxChild boxChild16 = (Box.BoxChild)vbox1[alignment4];
			boxChild16.Position = 5;
			boxChild16.Expand = false;
			boxChild16.Fill = false;
			Add(vbox1);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			label1.MnemonicWidget = comboboxLineEndings;
			Show();
		}
	}
}
