using System;
using Gtk;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Render.View
{
	public class ToolGroupView : HBox
	{
		public ToolGroupView(ToolGroup toolGroup)
		{
			this.toolGroup = toolGroup;
			IconRadioButton iconRadioButton = null;
			BaseTool[] items = toolGroup.Items;
			for (int i = 0; i < items.Length; i++)
			{
				BaseTool tool = items[i];
				if (tool.HasSeparator)
				{
					VSeparator child = new VSeparator();
					base.PackStart(child, false, false, 6U);
				}
				Widget button = this.CreateToolView(tool);
				if (!string.IsNullOrEmpty(tool.Tooltip))
				{
					button.TooltipText = tool.Tooltip;
				}
				button.SetSizeRequest(24, 24);
				base.PackStart(button, false, false, 0U);
				tool.EnabledChanged += delegate(object sender, EventArgs e)
				{
					button.Sensitive = tool.Enabled;
				};
				if (toolGroup.Current == tool)
				{
					iconRadioButton = (button as IconRadioButton);
				}
			}
			toolGroup.CurrentChanged += this.OnToolChanged;
			if (iconRadioButton != null)
			{
				iconRadioButton.IsChecked = true;
			}
		}

		private Widget CreateToolView(BaseTool tool)
		{
			Widget result;
			switch (tool.Type)
			{
			case ToolType.Button:
				result = this.CreateButton(tool);
				break;
			case ToolType.Radio:
				result = this.CreateRadioButton(tool);
				break;
			case ToolType.Toggle:
				result = this.CreateToggleButton(tool);
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		private Widget CreateButton(BaseTool tool)
		{
			IconButton iconButton = new IconButton(tool.Icon);
			iconButton.ButtonPressEvent += delegate(object sender, ButtonPressEventArgs args)
			{
				tool.OnMouseDown(args);
			};
			iconButton.ButtonReleaseEvent += delegate(object sender, ButtonReleaseEventArgs args)
			{
				tool.OnMouseUp(args);
			};
			return iconButton;
		}

		private Widget CreateToggleButton(BaseTool tool)
		{
			IconToggleButton button = new IconToggleButton(tool.Icon, null);
			button.Tag = tool;
			button.CheckChanged += this.ToggleButtonCheckedChanged;
			button.IsChecked = tool.IsSelected;
			tool.SelectedChanged += delegate(object sender, EventArgs args)
			{
				button.CheckChanged -= this.ToggleButtonCheckedChanged;
				button.IsChecked = tool.IsSelected;
				button.CheckChanged += this.ToggleButtonCheckedChanged;
			};
			return button;
		}

		private Widget CreateRadioButton(BaseTool tool)
		{
			IconRadioButton iconRadioButton = tool.CustomWidget as IconRadioButton;
			if (iconRadioButton == null)
			{
				iconRadioButton = new IconRadioButton(tool.Icon);
			}
			iconRadioButton.Tag = tool;
			iconRadioButton.CheckChanged += this.RadioButtonCheckChangedHandler;
			return iconRadioButton;
		}

		private void RadioButtonCheckChangedHandler(object sender, EventArgs e)
		{
			this.toolGroup.CurrentChanged -= this.OnToolChanged;
			IconRadioButton iconRadioButton = sender as IconRadioButton;
			if (iconRadioButton.IsChecked)
			{
				this.toolGroup.Current = (iconRadioButton.Tag as ITool);
			}
			this.toolGroup.CurrentChanged += this.OnToolChanged;
		}

		private void ToggleButtonCheckedChanged(object sender, EventArgs e)
		{
			IconToggleButton iconToggleButton = sender as IconToggleButton;
			BaseTool baseTool = iconToggleButton.Tag as BaseTool;
			baseTool.IsSelected = iconToggleButton.IsChecked;
		}

		private void OnToolChanged(object sender, CurrentToolChangedEventArgs e)
		{
			foreach (object obj in base.AllChildren)
			{
				IconRadioButton iconRadioButton = obj as IconRadioButton;
				if (iconRadioButton != null)
				{
					if (iconRadioButton.Tag == this.toolGroup.Current)
					{
						iconRadioButton.IsChecked = true;
						break;
					}
				}
			}
		}

		private ToolGroup toolGroup;
	}
}
