using System;
using Gtk;
using Modules.Communal.PropertyGrid;
using Xwt.Drawing;

namespace CocoStudio.Model.Editor
{
	internal class FilpEditor : BaseEditor
	{
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

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

		private IconToggleButton toggleBtnX;

		private IconToggleButton toggleBtnY;
	}
}
