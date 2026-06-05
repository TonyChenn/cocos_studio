using System;
using System.Collections.Generic;

namespace Gtk
{
	// Token: 0x0200000F RID: 15
	internal class RadioButtonManager
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000037A4 File Offset: 0x000019A4
		// (set) Token: 0x06000071 RID: 113 RVA: 0x000037BA File Offset: 0x000019BA
		public static RadioButtonManager Instance { get; private set; } = new RadioButtonManager();

		// Token: 0x06000073 RID: 115 RVA: 0x000037D0 File Offset: 0x000019D0
		public RadioButtonManager()
		{
			this.buttonList = new List<IconRadioButton>();
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000037E8 File Offset: 0x000019E8
		internal void Add(IconRadioButton btn)
		{
			if (!this.buttonList.Contains(btn))
			{
				btn.CheckChanged += this.ButtonCheckChangedHandler;
				btn.DeleteEvent += this.ButtonDeleteEventHandler;
				this.buttonList.Add(btn);
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003840 File Offset: 0x00001A40
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

		// Token: 0x06000076 RID: 118 RVA: 0x0000398C File Offset: 0x00001B8C
		private void ButtonDeleteEventHandler(object o, DeleteEventArgs args)
		{
			IconRadioButton iconRadioButton = o as IconRadioButton;
			iconRadioButton.CheckChanged -= this.ButtonCheckChangedHandler;
			iconRadioButton.DeleteEvent -= this.ButtonDeleteEventHandler;
			this.buttonList.Remove(iconRadioButton);
		}

		// Token: 0x0400002C RID: 44
		private List<IconRadioButton> buttonList;
	}
}
