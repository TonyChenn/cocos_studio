using System;
using System.Collections.Generic;
using Gtk;
using MonoDevelop.Core;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Ide.Gui.Dialogs;
using Xwt;

namespace MonoDevelop.SourceEditor.OptionPanels
{
	public class CompletionCharactersPanel : Xwt.VBox, IOptionsPanel
	{
		private ListView list;

		private Xwt.ListStore store;

		private DataField<string> language = new DataField<string>();

		private DataField<bool> completeOnSpace = new DataField<bool>();

		private DataField<string> completeOnChars = new DataField<string>();

		void IOptionsPanel.Initialize(OptionsDialog dialog, object dataObject)
		{
			base.ExpandHorizontal = true;
			base.ExpandVertical = true;
			base.HeightRequest = 400.0;
			list = new ListView();
			store = new Xwt.ListStore(language, completeOnSpace, completeOnChars);
			ListViewColumn listViewColumn = list.Columns.Add(GettextCatalog.GetString("Language"), language);
			listViewColumn.CanResize = true;
			CheckBoxCellView checkBoxCellView = new CheckBoxCellView(completeOnSpace);
			checkBoxCellView.Editable = true;
			ListViewColumn listViewColumn2 = list.Columns.Add(GettextCatalog.GetString("Complete on space"), checkBoxCellView);
			listViewColumn2.CanResize = true;
			TextCellView textCellView = new TextCellView(completeOnChars);
			textCellView.Editable = true;
			ListViewColumn listViewColumn3 = list.Columns.Add(GettextCatalog.GetString("Do complete on"), textCellView);
			listViewColumn3.CanResize = true;
			list.DataSource = store;
			PackStart(list, expand: true, fill: true);
			Xwt.HBox hBox = new Xwt.HBox();
			Xwt.Button button = new Xwt.Button("Reset to default");
			button.Clicked += delegate
			{
				FillStore(CompletionCharacters.GetDefaultCompletionCharacters());
			};
			hBox.PackEnd(button, expand: false, fill: false);
			PackEnd(hBox, expand: false, fill: true);
			FillStore(CompletionCharacters.GetCompletionCharacters());
		}

		private void FillStore(IEnumerable<CompletionCharacters> completionCharacters)
		{
			store.Clear();
			foreach (CompletionCharacters completionCharacter in completionCharacters)
			{
				int row = store.AddRow();
				store.SetValue(row, language, completionCharacter.Language);
				store.SetValue(row, completeOnSpace, completionCharacter.CompleteOnSpace);
				store.SetValue(row, completeOnChars, completionCharacter.CompleteOnChars);
			}
		}

		Gtk.Widget IOptionsPanel.CreatePanelWidget()
		{
			return (Gtk.Widget)Toolkit.CurrentEngine.GetNativeWidget(this);
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
			CompletionCharacters[] array = new CompletionCharacters[store.RowCount];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new CompletionCharacters(store.GetValue(i, language), store.GetValue(i, completeOnSpace), store.GetValue(i, completeOnChars));
				Console.WriteLine(array[i]);
			}
			CompletionCharacters.SetCompletionCharacters(array);
		}
	}
}
