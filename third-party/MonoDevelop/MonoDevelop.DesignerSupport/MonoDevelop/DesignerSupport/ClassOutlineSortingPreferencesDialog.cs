using System.Collections.Generic;
using Gtk;
using Mono.Unix;
using MonoDevelop.Ide.Gui.Components;
using Stetic;

namespace MonoDevelop.DesignerSupport
{
	internal class ClassOutlineSortingPreferencesDialog : Dialog
	{
		private ClassOutlineSettings settings;

		private VBox vbox2;

		private Label label;

		private PriorityList priorityList;

		private Button buttonCancel;

		private Button buttonOk;

		public ClassOutlineSortingPreferencesDialog(ClassOutlineSettings settings)
		{
			Build();
			priorityList.Model = new ListStore(typeof(string), typeof(string));
			priorityList.AppendColumn("", new CellRendererText(), "text", 1);
			priorityList.Model.Clear();
			foreach (string item in settings.GroupOrder)
			{
				priorityList.Model.AppendValues(item, ClassOutlineSettings.GetGroupName(item));
			}
			this.settings = settings;
		}

		public void SaveSettings()
		{
			if (priorityList.Model.GetIterFirst(out var iter))
			{
				List<string> list = new List<string>();
				do
				{
					list.Add((string)priorityList.Model.GetValue(iter, 0));
				}
				while (priorityList.Model.IterNext(ref iter));
				settings.GroupOrder = list;
			}
			settings.Save();
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			base.Name = "MonoDevelop.DesignerSupport.ClassOutlineSortingPreferencesDialog";
			base.Title = Catalog.GetString("Document Outline Preferences");
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			base.DestroyWithParent = true;
			VBox vBox = base.VBox;
			vBox.Name = "dialog1_VBox";
			vBox.BorderWidth = 2u;
			vbox2 = new VBox();
			vbox2.Name = "vbox2";
			vbox2.Spacing = 6;
			vbox2.BorderWidth = 6u;
			label = new Label();
			label.WidthRequest = 400;
			label.Name = "label";
			label.LabelProp = Catalog.GetString("Group sorting order when grouping is enabled:");
			label.Wrap = true;
			vbox2.Add(label);
			Box.BoxChild boxChild = (Box.BoxChild)vbox2[label];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			priorityList = new PriorityList();
			priorityList.Name = "priorityList";
			vbox2.Add(priorityList);
			Box.BoxChild boxChild2 = (Box.BoxChild)vbox2[priorityList];
			boxChild2.Position = 1;
			vBox.Add(vbox2);
			Box.BoxChild boxChild3 = (Box.BoxChild)vBox[vbox2];
			boxChild3.Position = 0;
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
			base.DefaultWidth = 424;
			base.DefaultHeight = 367;
			Hide();
		}
	}
}
