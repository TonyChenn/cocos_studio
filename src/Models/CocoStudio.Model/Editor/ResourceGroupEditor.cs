using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000099 RID: 153
	internal class ResourceGroupEditor : BaseEditor
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x00016BDC File Offset: 0x00014DDC
		public override bool CanCaching
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00016BF0 File Offset: 0x00014DF0
		public override bool IsMultiLine
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00016C04 File Offset: 0x00014E04
		protected override Widget OnCreateWidget()
		{
			this.imageEventBoxList = new List<ImageEventBox>();
			List<string> list = base.PropertyItem.Values[0] as List<string>;
			Table table = new Table(2U, (uint)list.Count, false);
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
				this.imageEventBoxList.Add(imageEventBox);
			}
			table.ShowAll();
			return table;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00016D19 File Offset: 0x00014F19
		protected override void OnSetControl()
		{
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00016D1C File Offset: 0x00014F1C
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

		// Token: 0x04000270 RID: 624
		private List<ImageEventBox> imageEventBoxList;
	}
}
