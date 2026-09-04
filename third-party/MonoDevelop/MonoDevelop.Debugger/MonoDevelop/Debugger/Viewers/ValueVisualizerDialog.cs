using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using Mono.Debugging.Client;
using Mono.Unix;
using Stetic;

namespace MonoDevelop.Debugger.Viewers
{
	public class ValueVisualizerDialog : Dialog
	{
		private List<ValueVisualizer> visualizers;

		private List<ToggleButton> buttons;

		private Widget currentWidget;

		private ValueVisualizer currentVisualizer;

		private ObjectValue value;

		private VBox mainBox;

		private HBox hbox1;

		private Label label1;

		private Button buttonCancel;

		private Button buttonSave;

		public ValueVisualizerDialog()
		{
			Build();
			base.Modal = true;
		}

		public void Show(ObjectValue val)
		{
			value = val;
			visualizers = new List<ValueVisualizer>(DebuggingService.GetValueVisualizers(val));
			visualizers.Sort((ValueVisualizer v1, ValueVisualizer v2) => string.Compare(v1.Name, v2.Name, StringComparison.CurrentCultureIgnoreCase));
			buttons = new List<ToggleButton>();
			Button button = null;
			for (int num = 0; num < visualizers.Count; num++)
			{
				ToggleButton toggleButton = new ToggleButton();
				toggleButton.Label = visualizers[num].Name;
				toggleButton.Toggled += OnComboVisualizersChanged;
				if (visualizers[num].IsDefaultVisualizer(val))
				{
					button = toggleButton;
				}
				hbox1.PackStart(toggleButton, expand: false, fill: false, 0u);
				buttons.Add(toggleButton);
				toggleButton.CanFocus = false;
				toggleButton.Show();
			}
			if (button != null)
			{
				button.Click();
			}
			else if (buttons.Count > 0)
			{
				buttons[0].Click();
			}
			if (val.IsReadOnly || !visualizers.Any((ValueVisualizer v) => v.CanEdit(val)))
			{
				buttonCancel.Label = Stock.Close;
				buttonSave.Hide();
			}
		}

		protected virtual void OnComboVisualizersChanged(object sender, EventArgs e)
		{
			ToggleButton toggleButton = (ToggleButton)sender;
			if (!toggleButton.Active)
			{
				toggleButton.Toggled -= OnComboVisualizersChanged;
				toggleButton.Active = true;
				toggleButton.Toggled += OnComboVisualizersChanged;
				return;
			}
			if (currentWidget != null)
			{
				mainBox.Remove(currentWidget);
			}
			foreach (ToggleButton button in buttons)
			{
				if (button != toggleButton && button.Active)
				{
					button.Toggled -= OnComboVisualizersChanged;
					button.Active = false;
					button.Toggled += OnComboVisualizersChanged;
				}
			}
			currentVisualizer = visualizers[buttons.IndexOf(toggleButton)];
			currentWidget = currentVisualizer.GetVisualizerWidget(value);
			buttonSave.Sensitive = currentVisualizer.CanEdit(value);
			mainBox.PackStart(currentWidget, expand: true, fill: true, 0u);
			currentWidget.Show();
		}

		protected virtual void OnSaveClicked(object sender, EventArgs e)
		{
			if (currentVisualizer == null || currentVisualizer.StoreValue(value))
			{
				Respond(ResponseType.Ok);
			}
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			base.Name = "MonoDevelop.Debugger.Viewers.ValueVisualizerDialog";
			base.Title = Catalog.GetString("Value Visualizer");
			base.WindowPosition = WindowPosition.CenterOnParent;
			VBox vBox = base.VBox;
			vBox.Name = "dialog1_VBox";
			vBox.BorderWidth = 2u;
			mainBox = new VBox();
			mainBox.Name = "mainBox";
			mainBox.Spacing = 6;
			mainBox.BorderWidth = 6u;
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 6;
			label1 = new Label();
			label1.Name = "label1";
			label1.LabelProp = Catalog.GetString("View as:");
			hbox1.Add(label1);
			Box.BoxChild boxChild = (Box.BoxChild)hbox1[label1];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			mainBox.Add(hbox1);
			Box.BoxChild boxChild2 = (Box.BoxChild)mainBox[hbox1];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			vBox.Add(mainBox);
			Box.BoxChild boxChild3 = (Box.BoxChild)vBox[mainBox];
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
			buttonSave = new Button();
			buttonSave.CanDefault = true;
			buttonSave.CanFocus = true;
			buttonSave.Name = "buttonSave";
			buttonSave.UseStock = true;
			buttonSave.UseUnderline = true;
			buttonSave.Label = "gtk-save";
			AddActionWidget(buttonSave, -10);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[buttonSave];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 628;
			base.DefaultHeight = 433;
			Hide();
			buttonSave.Clicked += OnSaveClicked;
		}
	}
}
