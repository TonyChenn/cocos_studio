using System;
using System.IO;
using Gtk;
using Mono.TextEditor.Highlighting;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace MonoDevelop.SourceEditor.OptionPanels
{
	public class NewColorShemeDialog : Dialog
	{
		private ListStore store = new ListStore(typeof(string));

		private Table table1;

		private ComboBox comboboxBaseStyle;

		private Entry entryDescription;

		private Entry entryName;

		private Label label1;

		private Label label2;

		private Label label3;

		private Button buttonCancel;

		private Button buttonOk;

		public NewColorShemeDialog()
		{
			Build();
			string[] styles = Mono.TextEditor.Highlighting.SyntaxModeService.Styles;
			foreach (string text in styles)
			{
				store.AppendValues(text);
			}
			comboboxBaseStyle.Model = store;
			comboboxBaseStyle.Active = 0;
			entryName.Changed += HandleEntryNameChanged;
			entryDescription.Changed += HandleEntryNameChanged;
			buttonOk.Clicked += HandleButtonOkClicked;
			buttonOk.Sensitive = false;
		}

		private void HandleEntryNameChanged(object sender, EventArgs e)
		{
			buttonOk.Sensitive = !string.IsNullOrEmpty(entryName.Text);
		}

		private void HandleButtonOkClicked(object sender, EventArgs e)
		{
			if (!store.IterNthChild(out var iter, comboboxBaseStyle.Active))
			{
				return;
			}
			string text = (string)store.GetValue(iter, 0);
			ColorScheme colorStyle = Mono.TextEditor.Highlighting.SyntaxModeService.GetColorStyle(text);
			colorStyle = colorStyle.Clone();
			colorStyle.Name = entryName.Text;
			colorStyle.Description = entryDescription.Text;
			colorStyle.BaseScheme = text;
			string path = SourceEditorDisplayBinding.SyntaxModePath;
			string text2 = colorStyle.Name.Replace(" ", "_");
			while (File.Exists(System.IO.Path.Combine(path, text2 + "Style.json")))
			{
				text2 += "_";
			}
			string fileName = System.IO.Path.Combine(path, text2 + "Style.json");
			try
			{
				colorStyle.Save(fileName);
				colorStyle.FileName = fileName;
				Mono.TextEditor.Highlighting.SyntaxModeService.AddStyle(colorStyle);
			}
			catch (Exception ex)
			{
				LoggingService.LogInternalError(ex);
			}
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			base.Name = "MonoDevelop.SourceEditor.OptionPanels.NewColorShemeDialog";
			base.Title = Catalog.GetString("Create new color sheme");
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.BorderWidth = 6u;
			VBox vBox = base.VBox;
			vBox.Name = "dialog1_VBox";
			vBox.BorderWidth = 2u;
			table1 = new Table(3u, 2u, homogeneous: false);
			table1.Name = "table1";
			table1.RowSpacing = 6u;
			table1.ColumnSpacing = 6u;
			comboboxBaseStyle = ComboBox.NewText();
			comboboxBaseStyle.Name = "comboboxBaseStyle";
			table1.Add(comboboxBaseStyle);
			Table.TableChild tableChild = (Table.TableChild)table1[comboboxBaseStyle];
			tableChild.LeftAttach = 1u;
			tableChild.RightAttach = 2u;
			tableChild.YOptions = AttachOptions.Fill;
			entryDescription = new Entry();
			entryDescription.CanFocus = true;
			entryDescription.Name = "entryDescription";
			entryDescription.IsEditable = true;
			entryDescription.InvisibleChar = '●';
			table1.Add(entryDescription);
			Table.TableChild tableChild2 = (Table.TableChild)table1[entryDescription];
			tableChild2.TopAttach = 2u;
			tableChild2.BottomAttach = 3u;
			tableChild2.LeftAttach = 1u;
			tableChild2.RightAttach = 2u;
			tableChild2.XOptions = AttachOptions.Fill;
			tableChild2.YOptions = AttachOptions.Fill;
			entryName = new Entry();
			entryName.CanFocus = true;
			entryName.Name = "entryName";
			entryName.IsEditable = true;
			entryName.InvisibleChar = '●';
			table1.Add(entryName);
			Table.TableChild tableChild3 = (Table.TableChild)table1[entryName];
			tableChild3.TopAttach = 1u;
			tableChild3.BottomAttach = 2u;
			tableChild3.LeftAttach = 1u;
			tableChild3.RightAttach = 2u;
			tableChild3.XOptions = AttachOptions.Fill;
			tableChild3.YOptions = AttachOptions.Fill;
			label1 = new Label();
			label1.Name = "label1";
			label1.Xalign = 0f;
			label1.LabelProp = Catalog.GetString("_Based on:");
			label1.UseMarkup = true;
			label1.UseUnderline = true;
			table1.Add(label1);
			Table.TableChild tableChild4 = (Table.TableChild)table1[label1];
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			label2 = new Label();
			label2.Name = "label2";
			label2.Xalign = 0f;
			label2.LabelProp = Catalog.GetString("_Name:");
			label2.UseUnderline = true;
			table1.Add(label2);
			Table.TableChild tableChild5 = (Table.TableChild)table1[label2];
			tableChild5.TopAttach = 1u;
			tableChild5.BottomAttach = 2u;
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			label3 = new Label();
			label3.Name = "label3";
			label3.LabelProp = Catalog.GetString("_Description:");
			label3.UseUnderline = true;
			table1.Add(label3);
			Table.TableChild tableChild6 = (Table.TableChild)table1[label3];
			tableChild6.TopAttach = 2u;
			tableChild6.BottomAttach = 3u;
			tableChild6.XOptions = AttachOptions.Fill;
			tableChild6.YOptions = AttachOptions.Fill;
			vBox.Add(table1);
			Box.BoxChild boxChild = (Box.BoxChild)vBox[table1];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5u;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			buttonCancel = new Button();
			buttonCancel.CanDefault = true;
			buttonCancel.CanFocus = true;
			buttonCancel.Name = "buttonCancel";
			buttonCancel.UseStock = true;
			buttonCancel.UseUnderline = true;
			buttonCancel.Label = "gtk-cancel";
			AddActionWidget(buttonCancel, -6);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[buttonCancel];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			buttonOk = new Button();
			buttonOk.CanDefault = true;
			buttonOk.CanFocus = true;
			buttonOk.Name = "buttonOk";
			buttonOk.UseStock = true;
			buttonOk.UseUnderline = true;
			buttonOk.Label = "gtk-ok";
			AddActionWidget(buttonOk, -5);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[buttonOk];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 393;
			base.DefaultHeight = 148;
			label2.MnemonicWidget = entryName;
			label3.MnemonicWidget = entryDescription;
			Show();
		}
	}
}
