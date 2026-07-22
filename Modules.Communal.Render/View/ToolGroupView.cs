using System;
using Gtk;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Render.View
{
	// Token: 0x02000039 RID: 57
	public class ToolGroupView : HBox
	{
		// Token: 0x0600029A RID: 666 RVA: 0x0000E420 File Offset: 0x0000C620
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

		// Token: 0x0600029B RID: 667 RVA: 0x0000E570 File Offset: 0x0000C770
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

		// Token: 0x0600029C RID: 668 RVA: 0x0000E5E8 File Offset: 0x0000C7E8
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

		// Token: 0x0600029D RID: 669 RVA: 0x0000E6A8 File Offset: 0x0000C8A8
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

		// Token: 0x0600029E RID: 670 RVA: 0x0000E744 File Offset: 0x0000C944
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

		// Token: 0x0600029F RID: 671 RVA: 0x0000E798 File Offset: 0x0000C998
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

		// Token: 0x060002A0 RID: 672 RVA: 0x0000E804 File Offset: 0x0000CA04
		private void ToggleButtonCheckedChanged(object sender, EventArgs e)
		{
			IconToggleButton iconToggleButton = sender as IconToggleButton;
			BaseTool baseTool = iconToggleButton.Tag as BaseTool;
			baseTool.IsSelected = iconToggleButton.IsChecked;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000E834 File Offset: 0x0000CA34
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

		// Token: 0x040000C0 RID: 192
		private ToolGroup toolGroup;
	}
}
