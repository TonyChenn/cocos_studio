using System;
using Gtk;
using Mono.Debugging.Client;
using Mono.Unix;
using Stetic;

namespace MonoDevelop.Debugger
{
	public class BusyEvaluatorDialog : Dialog
	{
		private VBox vbox2;

		private Label label1;

		private HBox hbox1;

		private Label label2;

		private Label labelMethod;

		private Button buttonCancel;

		private Button buttonOk;

		public BusyEvaluatorDialog()
		{
			Build();
		}

		public void UpdateBusyState(BusyStateEventArgs args)
		{
			if (!args.IsBusy)
			{
				Hide();
				return;
			}
			labelMethod.Text = args.Description;
			Show();
		}

		protected virtual void OnButtonCancelClicked(object sender, EventArgs e)
		{
			Hide();
			DebuggingService.Stop();
		}

		protected virtual void OnButtonOkClicked(object sender, EventArgs e)
		{
			Hide();
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			base.Name = "MonoDevelop.Debugger.BusyEvaluatorDialog";
			base.Title = Catalog.GetString("The Debugger is Busy");
			base.WindowPosition = WindowPosition.CenterOnParent;
			VBox vBox = base.VBox;
			vBox.Name = "dialog1_VBox";
			vBox.BorderWidth = 2u;
			vbox2 = new VBox();
			vbox2.Name = "vbox2";
			vbox2.Spacing = 6;
			vbox2.BorderWidth = 9u;
			label1 = new Label();
			label1.Name = "label1";
			label1.Xalign = 0f;
			label1.LabelProp = Catalog.GetString("The Debugger is waiting for an expression evaluation to finish.");
			vbox2.Add(label1);
			Box.BoxChild boxChild = (Box.BoxChild)vbox2[label1];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 6;
			label2 = new Label();
			label2.Name = "label2";
			label2.Xalign = 0f;
			label2.LabelProp = Catalog.GetString("Method:");
			hbox1.Add(label2);
			Box.BoxChild boxChild2 = (Box.BoxChild)hbox1[label2];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			labelMethod = new Label();
			labelMethod.Name = "labelMethod";
			labelMethod.Xalign = 0f;
			labelMethod.LabelProp = "<method>";
			labelMethod.Wrap = true;
			labelMethod.Selectable = true;
			labelMethod.MaxWidthChars = 120;
			hbox1.Add(labelMethod);
			Box.BoxChild boxChild3 = (Box.BoxChild)hbox1[labelMethod];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			vbox2.Add(hbox1);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox2[hbox1];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			vBox.Add(vbox2);
			Box.BoxChild boxChild5 = (Box.BoxChild)vBox[vbox2];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5u;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			buttonCancel = new Button();
			buttonCancel.CanDefault = true;
			buttonCancel.CanFocus = true;
			buttonCancel.Name = "buttonCancel";
			buttonCancel.UseUnderline = true;
			buttonCancel.Label = Catalog.GetString("Stop Debugger");
			actionArea.Add(buttonCancel);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[buttonCancel];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			buttonOk = new Button();
			buttonOk.CanDefault = true;
			buttonOk.CanFocus = true;
			buttonOk.Name = "buttonOk";
			buttonOk.UseUnderline = true;
			buttonOk.Label = Catalog.GetString("Keep Waiting");
			actionArea.Add(buttonOk);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[buttonOk];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 381;
			base.DefaultHeight = 126;
			Hide();
			buttonCancel.Clicked += OnButtonCancelClicked;
			buttonOk.Clicked += OnButtonOkClicked;
		}
	}
}
