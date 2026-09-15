using System;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.Editor
{
	internal class ChangeUnitButton : TriangleComboButton
	{
		public bool IsPercent { get; set; }

		public event EventHandler UnitChanged;

		public ChangeUnitButton()
		{
			this.menu = new Menu();
			this.percentMenuItem = new CheckMenuItem("%  " + LanguageInfo.Property_ParentPercentage);
			this.percentMenuItem.ButtonReleaseEvent += this.PercentMenuButtonReleasedHandler;
			this.percentMenuItem.Active = this.IsPercent;
			this.pixelMenuItem = new CheckMenuItem(LanguageInfo.NewFile_Pixel);
			this.pixelMenuItem.ButtonPressEvent += this.PixelMenuButtonReleasedHandler;
			this.pixelMenuItem.Active = !this.IsPercent;
			this.menu.Add(this.percentMenuItem);
			this.menu.Add(this.pixelMenuItem);
			this.menu.ShowAll();
			base.Clicked += this.ButtonClickedHandler;
		}

		private void ButtonClickedHandler(object sender, EventArgs e)
		{
			this.percentMenuItem.Active = this.IsPercent;
			this.pixelMenuItem.Active = !this.IsPercent;
			this.menu.Popup();
		}

		private void PercentMenuButtonReleasedHandler(object o, ButtonReleaseEventArgs args)
		{
			this.IsPercent = true;
			if (this.UnitChanged != null)
			{
				this.UnitChanged(this, new EventArgs());
			}
		}

		private void PixelMenuButtonReleasedHandler(object o, ButtonPressEventArgs args)
		{
			this.IsPercent = false;
			if (this.UnitChanged != null)
			{
				this.UnitChanged(this, new EventArgs());
			}
		}

		private Menu menu;

		private CheckMenuItem percentMenuItem;

		private CheckMenuItem pixelMenuItem;
	}
}
