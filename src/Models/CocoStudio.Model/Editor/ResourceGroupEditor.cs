using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class ResourceGroupEditor : BaseEditor
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
			this.imageEventBoxList = new List<ImageEventBox>();
			List<string> list = base.PropertyItem.Values[0] as List<string>;
			bool supportsLocateButton = ResourceLocateButton.SupportsGroupEditor(PropertyItem.FirstObject);
			Table table = new Table(supportsLocateButton ? 3U : 2U, (uint)list.Count, false);
			table.ColumnSpacing = 6U;
			for (int i = 0; i < list.Count; i++)
			{
				string name = list[i];
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(PropertyItem.FirstObject.GetType()).Find(name, false);
				Label label = new Label(LanguageOption.GetValueBykey(propertyDescriptor.DisplayName));
				label.Sensitive = false;
				label.SetFontSize(10.0);
				table.Attach(label, (uint)i, (uint)(i + 1), 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
				ImageEventBox imageEventBox = new ImageEventBox(base.PropertyItem, propertyDescriptor, null);
				imageEventBox.SetDefaultFileMarker();
				table.Attach(imageEventBox, (uint)i, (uint)(i + 1), 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
				if (supportsLocateButton)
				{
					ResourceLocateButton locateButton = new ResourceLocateButton(imageEventBox);
					table.Attach(locateButton.CreateCenteredAlignment(), (uint)i, (uint)(i + 1), 2U, 3U, AttachOptions.Fill, AttachOptions.Fill, 0U, 2U);
				}
				this.imageEventBoxList.Add(imageEventBox);
			}
			table.ShowAll();
			return table;
		}

		protected override void OnSetControl()
		{
		}

		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			foreach (ImageEventBox imageEventBox in this.imageEventBoxList)
			{
				if (imageEventBox.PropertyName == e.PropertyName)
				{
					imageEventBox.Refresh();
				}
			}
		}

		private List<ImageEventBox> imageEventBoxList;
	}
}
