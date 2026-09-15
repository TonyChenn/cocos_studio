using System;
using CocoStudio.Model.ViewModel;
using Gtk;

namespace Modules.UI.RenderContextMenu
{
	internal class TextEditMenuItem : MenuItem, IObjectMenuItem
	{
		public VisualObject TriggerObject
		{
			get
			{
				return this.triggerObject;
			}
			set
			{
				if (this.triggerObject != value)
				{
					this.triggerObject = value;
				}
			}
		}

		public TextEditMenuItem(string title) : base(title)
		{
			base.ButtonReleaseEvent += this.TextEditMenuItem_ButtonReleaseEvent;
		}

		private void TextEditMenuItem_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			Menu menu = base.Parent as Menu;
			menu.Deactivate();
			this.OpenTextEditWindow();
		}

		public string OpenTextEditWindow()
		{
			this.TriggerObject.MouseDoubleClick(null);
			return this.TriggerObject.GetType().Name;
		}

		public void UpdateMenuItemState()
		{
		}

		private VisualObject triggerObject;
	}
}
