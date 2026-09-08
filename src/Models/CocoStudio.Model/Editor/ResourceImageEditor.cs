using System;
using System.ComponentModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200009A RID: 154
	public class ResourceImageEditor : BaseEditor
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00016D98 File Offset: 0x00014F98
		public override bool CanCaching
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x00016DAC File Offset: 0x00014FAC
		public override bool IsMultiLine
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00016DC0 File Offset: 0x00014FC0
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

		// Token: 0x06000540 RID: 1344 RVA: 0x00016E2C File Offset: 0x0001502C
		protected override void OnSetControl()
		{
			if (this.imageEventBox != null)
			{
				this.imageEventBox.Refresh();
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00016E58 File Offset: 0x00015058
		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.imageEventBox.PropertyName == e.PropertyName)
			{
				this.imageEventBox.Refresh();
			}
		}

		// Token: 0x04000271 RID: 625
		private ImageEventBox imageEventBox;
	}
}
