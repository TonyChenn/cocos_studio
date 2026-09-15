using System;
using System.Collections.Generic;

namespace Gtk
{
	internal class RadioButtonManager
	{
		public static RadioButtonManager Instance { get; private set; } = new RadioButtonManager();

		public RadioButtonManager()
		{
			this.buttonList = new List<IconRadioButton>();
		}

		internal void Add(IconRadioButton btn)
		{
			if (!this.buttonList.Contains(btn))
			{
				btn.CheckChanged += this.ButtonCheckChangedHandler;
				btn.DeleteEvent += this.ButtonDeleteEventHandler;
				this.buttonList.Add(btn);
			}
		}

		private void ButtonCheckChangedHandler(object sender, EventArgs e)
		{
			IconRadioButton iconRadioButton = sender as IconRadioButton;
			if (iconRadioButton.IsChecked)
			{
				if (string.IsNullOrEmpty(iconRadioButton.GroupName))
				{
					Container container = iconRadioButton.Parent as Container;
					if (container != null)
					{
						foreach (Widget widget in container.Children)
						{
							if (widget != iconRadioButton)
							{
								IconRadioButton iconRadioButton2 = widget as IconRadioButton;
								if (iconRadioButton2 != null && iconRadioButton2.IsChecked)
								{
									iconRadioButton2.IsChecked = false;
								}
							}
						}
					}
				}
				else
				{
					foreach (IconRadioButton iconRadioButton3 in this.buttonList)
					{
						if (iconRadioButton3.IsChecked)
						{
							if (iconRadioButton3 != iconRadioButton)
							{
								if (iconRadioButton.GroupName.Equals(iconRadioButton3.GroupName))
								{
									iconRadioButton3.IsChecked = false;
								}
							}
						}
					}
				}
			}
		}

		private void ButtonDeleteEventHandler(object o, DeleteEventArgs args)
		{
			IconRadioButton iconRadioButton = o as IconRadioButton;
			iconRadioButton.CheckChanged -= this.ButtonCheckChangedHandler;
			iconRadioButton.DeleteEvent -= this.ButtonDeleteEventHandler;
			this.buttonList.Remove(iconRadioButton);
		}

		private List<IconRadioButton> buttonList;
	}
}
