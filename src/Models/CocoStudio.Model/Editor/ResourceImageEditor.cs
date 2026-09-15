using System;
using System.ComponentModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	public class ResourceImageEditor : BaseEditor
	{
		public override bool CanCaching
		{
			get
			{
				return false;
			}
		}

		public override bool IsMultiLine
		{
			get
			{
				return true;
			}
		}

		protected override Widget OnCreateWidget()
		{
			PropertyDescriptor pDescriptor = TypeDescriptor.GetProperties(PropertyItem.FirstObject.GetType()).Find(base.PropertyItem.Name, false);
			this.imageEventBox = new ImageEventBox(base.PropertyItem, pDescriptor, null);
			HBox hbox = new HBox();
			if (ResourceLocateButton.SupportsImageEditor(PropertyItem.FirstObject))
			{
				ResourceLocateButton locateButton = new ResourceLocateButton(this.imageEventBox);
				hbox.PackStart(locateButton.CreateCenteredAlignment(), false, false, 0U);
				hbox.Spacing = 4;
			}
			hbox.PackStart(this.imageEventBox, false, false, 0U);
			hbox.ShowAll();
			return hbox;
		}

		protected override void OnSetControl()
		{
			if (this.imageEventBox != null)
			{
				this.imageEventBox.Refresh();
			}
		}

		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.imageEventBox.PropertyName == e.PropertyName)
			{
				this.imageEventBox.Refresh();
			}
		}

		private ImageEventBox imageEventBox;
	}
}
