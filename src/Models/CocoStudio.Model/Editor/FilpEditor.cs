using System;
using Gtk;
using Modules.Communal.PropertyGrid;
using Xwt.Drawing;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200008F RID: 143
	internal class FilpEditor : BaseEditor
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x000155D0 File Offset: 0x000137D0
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000155E4 File Offset: 0x000137E4
		protected override Widget OnCreateWidget()
		{
			Xwt.Drawing.Image icon = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.horizontalFilp.png");
			this.toggleBtnX = new IconToggleButton(icon, null);
			Xwt.Drawing.Image icon2 = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.verticalFilp.png");
			this.toggleBtnY = new IconToggleButton(icon2, null);
			HBox hbox = new HBox();
			hbox.Spacing = 6;
			hbox.PackStart(this.toggleBtnX, false, false, 0U);
			hbox.PackStart(this.toggleBtnY, false, false, 0U);
			hbox.ShowAll();
			base.SetControl();
			this.toggleBtnX.CheckChanged += this.HorizonButtonToggleHandler;
			this.toggleBtnY.CheckChanged += this.VerticalButtonToggleHandler;
			return hbox;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00015694 File Offset: 0x00013894
		protected override void OnSetControl()
		{
			IFlipped flipped = PropertyItem.FirstObject as IFlipped;
			bool isChecked = flipped.FlipX;
			bool isChecked2 = flipped.FlipY;
			if (PropertyItem.Objects.Count > 1)
			{
				Func<IFlipped, IFlipped, bool> func = (IFlipped a, IFlipped b) => a.FlipX == b.FlipX;
				Func<IFlipped, IFlipped, bool> func2 = (IFlipped a, IFlipped b) => a.FlipY == b.FlipY;
				if (base.IsWhipNode<IFlipped>(func))
				{
					isChecked = false;
				}
				if (base.IsWhipNode<IFlipped>(func2))
				{
					isChecked2 = false;
				}
			}
			this.toggleBtnX.IsChecked = isChecked;
			this.toggleBtnY.IsChecked = isChecked2;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001575C File Offset: 0x0001395C
		private void HorizonButtonToggleHandler(object sender, EventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					FilpValue value = base.PropertyItem.GetValue<FilpValue>(i);
					value.FlipX = this.toggleBtnX.IsChecked;
					base.PropertyItem.Values[i] = value;
				}
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000157E4 File Offset: 0x000139E4
		private void VerticalButtonToggleHandler(object sender, EventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					FilpValue value = base.PropertyItem.GetValue<FilpValue>(i);
					value.FlipY = this.toggleBtnY.IsChecked;
					base.PropertyItem.Values[i] = value;
				}
			}
		}

		// Token: 0x04000245 RID: 581
		private IconToggleButton toggleBtnX;

		// Token: 0x04000246 RID: 582
		private IconToggleButton toggleBtnY;
	}
}
