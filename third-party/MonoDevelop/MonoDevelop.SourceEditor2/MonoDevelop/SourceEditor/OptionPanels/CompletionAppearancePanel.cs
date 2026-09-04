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
	internal class CompletionAppearancePanel : Bin, IOptionsPanel
	{
		private VBox vbox1;

		private HBox hbox1;

		private Label label2;

		private SpinButton spinbutton1;

		private Label label3;

		private Alignment alignment3;

		private VBox vbox5;

		private CheckButton filterByBrowsableCheckbutton;

		private HBox hbox2;

		private Fixed fixed1;

		private RadioButton normalOnlyRadiobutton;

		private Alignment alignment1;

		private Label label4;

		private HBox hbox3;

		private Fixed fixed2;

		private RadioButton includeAdvancedRadiobutton;

		private Alignment alignment2;

		private Label label5;

		public CompletionAppearancePanel()
		{
			Build();
			filterByBrowsableCheckbutton.Toggled += FilterToggled;
		}

		private void FilterToggled(object sender, EventArgs e)
		{
			Label label = label4;
			Label label2 = label5;
			RadioButton radioButton = normalOnlyRadiobutton;
			bool flag = (includeAdvancedRadiobutton.Sensitive = filterByBrowsableCheckbutton.Active);
			bool flag2 = (radioButton.Sensitive = flag);
			bool sensitive = (label2.Sensitive = flag2);
			label.Sensitive = sensitive;
		}

		void IOptionsPanel.Initialize(OptionsDialog dialog, object dataObject)
		{
		}

		Widget IOptionsPanel.CreatePanelWidget()
		{
			spinbutton1.Value = (int)CompletionTextEditorExtension.CompletionListRows;
			filterByBrowsableCheckbutton.Active = CompletionTextEditorExtension.FilterCompletionListByEditorBrowsable;
			normalOnlyRadiobutton.Active = !CompletionTextEditorExtension.IncludeEditorBrowsableAdvancedMembers;
			includeAdvancedRadiobutton.Active = CompletionTextEditorExtension.IncludeEditorBrowsableAdvancedMembers;
			FilterToggled(this, EventArgs.Empty);
			return this;
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
			CompletionTextEditorExtension.CompletionListRows.Value = spinbutton1.ValueAsInt;
			CompletionTextEditorExtension.FilterCompletionListByEditorBrowsable.Value = filterByBrowsableCheckbutton.Active;
			CompletionTextEditorExtension.IncludeEditorBrowsableAdvancedMembers.Value = includeAdvancedRadiobutton.Active;
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			Stetic.BinContainer.Attach(this);
			base.Name = "MonoDevelop.SourceEditor.OptionPanels.CompletionAppearancePanel";
			vbox1 = new VBox();
			vbox1.Name = "vbox1";
			vbox1.Spacing = 6;
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 6;
			label2 = new Label();
			label2.Name = "label2";
			label2.LabelProp = Catalog.GetString("Completion list has");
			hbox1.Add(label2);
			Box.BoxChild boxChild = (Box.BoxChild)hbox1[label2];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			spinbutton1 = new SpinButton(0.0, 100.0, 1.0);
			spinbutton1.CanFocus = true;
			spinbutton1.Name = "spinbutton1";
			spinbutton1.Adjustment.PageIncrement = 10.0;
			spinbutton1.ClimbRate = 1.0;
			spinbutton1.Numeric = true;
			hbox1.Add(spinbutton1);
			Box.BoxChild boxChild2 = (Box.BoxChild)hbox1[spinbutton1];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			label3 = new Label();
			label3.Name = "label3";
			label3.LabelProp = Catalog.GetString("rows");
			hbox1.Add(label3);
			Box.BoxChild boxChild3 = (Box.BoxChild)hbox1[label3];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			vbox1.Add(hbox1);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox1[hbox1];
			boxChild4.Position = 0;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			alignment3 = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment3.Name = "alignment3";
			alignment3.LeftPadding = 12u;
			vbox5 = new VBox();
			vbox5.Name = "vbox5";
			vbox5.Spacing = 6;
			filterByBrowsableCheckbutton = new CheckButton();
			filterByBrowsableCheckbutton.CanFocus = true;
			filterByBrowsableCheckbutton.Name = "filterByBrowsableCheckbutton";
			filterByBrowsableCheckbutton.Label = Catalog.GetString("_Filter members by [EditorBrowsable] attribute");
			filterByBrowsableCheckbutton.Active = true;
			filterByBrowsableCheckbutton.DrawIndicator = true;
			filterByBrowsableCheckbutton.UseUnderline = true;
			vbox5.Add(filterByBrowsableCheckbutton);
			Box.BoxChild boxChild5 = (Box.BoxChild)vbox5[filterByBrowsableCheckbutton];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			hbox2 = new HBox();
			hbox2.Name = "hbox2";
			hbox2.Spacing = 6;
			fixed1 = new Fixed();
			fixed1.Name = "fixed1";
			fixed1.HasWindow = false;
			hbox2.Add(fixed1);
			Box.BoxChild boxChild6 = (Box.BoxChild)hbox2[fixed1];
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			boxChild6.Padding = 6u;
			normalOnlyRadiobutton = new RadioButton(Catalog.GetString("_Show Normal members only"));
			normalOnlyRadiobutton.CanFocus = true;
			normalOnlyRadiobutton.Name = "normalOnlyRadiobutton";
			normalOnlyRadiobutton.DrawIndicator = true;
			normalOnlyRadiobutton.UseUnderline = true;
			normalOnlyRadiobutton.Group = new SList(IntPtr.Zero);
			hbox2.Add(normalOnlyRadiobutton);
			Box.BoxChild boxChild7 = (Box.BoxChild)hbox2[normalOnlyRadiobutton];
			boxChild7.Position = 1;
			vbox5.Add(hbox2);
			Box.BoxChild boxChild8 = (Box.BoxChild)vbox5[hbox2];
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			alignment1 = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment1.Name = "alignment1";
			alignment1.LeftPadding = 38u;
			label4 = new Label();
			label4.Name = "label4";
			label4.Xalign = 0f;
			label4.LabelProp = Catalog.GetString("<i>EditorBrowsableState.Always</i>");
			label4.UseMarkup = true;
			alignment1.Add(label4);
			vbox5.Add(alignment1);
			Box.BoxChild boxChild9 = (Box.BoxChild)vbox5[alignment1];
			boxChild9.Position = 2;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			hbox3 = new HBox();
			hbox3.Name = "hbox3";
			hbox3.Spacing = 6;
			fixed2 = new Fixed();
			fixed2.Name = "fixed2";
			fixed2.HasWindow = false;
			hbox3.Add(fixed2);
			Box.BoxChild boxChild10 = (Box.BoxChild)hbox3[fixed2];
			boxChild10.Position = 0;
			boxChild10.Expand = false;
			boxChild10.Padding = 6u;
			includeAdvancedRadiobutton = new RadioButton(Catalog.GetString("Show Normal and _Advanced members"));
			includeAdvancedRadiobutton.CanFocus = true;
			includeAdvancedRadiobutton.Name = "includeAdvancedRadiobutton";
			includeAdvancedRadiobutton.DrawIndicator = true;
			includeAdvancedRadiobutton.UseUnderline = true;
			includeAdvancedRadiobutton.Group = normalOnlyRadiobutton.Group;
			hbox3.Add(includeAdvancedRadiobutton);
			Box.BoxChild boxChild11 = (Box.BoxChild)hbox3[includeAdvancedRadiobutton];
			boxChild11.Position = 1;
			vbox5.Add(hbox3);
			Box.BoxChild boxChild12 = (Box.BoxChild)vbox5[hbox3];
			boxChild12.Position = 3;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			alignment2 = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment2.Name = "alignment2";
			alignment2.LeftPadding = 38u;
			label5 = new Label();
			label5.Name = "label5";
			label5.Xalign = 0f;
			label5.LabelProp = Catalog.GetString("<i>EditorBrowsableState.Always and EditorBrowsableState.Advanced</i>");
			label5.UseMarkup = true;
			alignment2.Add(label5);
			vbox5.Add(alignment2);
			Box.BoxChild boxChild13 = (Box.BoxChild)vbox5[alignment2];
			boxChild13.Position = 4;
			boxChild13.Expand = false;
			boxChild13.Fill = false;
			alignment3.Add(vbox5);
			vbox1.Add(alignment3);
			Box.BoxChild boxChild14 = (Box.BoxChild)vbox1[alignment3];
			boxChild14.Position = 1;
			boxChild14.Expand = false;
			boxChild14.Fill = false;
			Add(vbox1);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			Hide();
		}
	}
}
