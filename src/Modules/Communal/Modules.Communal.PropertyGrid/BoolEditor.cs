using System;
using Gtk;

namespace Modules.Communal.PropertyGrid
{
	public class BoolEditor : BaseEditor
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
			this.widget = new CheckButtonEx();
			base.SetControl();
			this.widget.Clicked += this.CheckButtonClickedHandler;
			CheckButtonHBox checkButtonHBox = new CheckButtonHBox();
			checkButtonHBox.HeightRequest = 24;
			checkButtonHBox.PackStart(this.widget, false, false, 0U);
			checkButtonHBox.ShowAll();
			return checkButtonHBox;
		}

		private void CheckButtonClickedHandler(object sender, EventArgs e)
		{
			this.OnCheckButtonClicked();
		}

		protected virtual void OnCheckButtonClicked()
		{
			if (this.widget.Inconsistent)
			{
				this.widget.Inconsistent = false;
				if (!this.widget.Active)
				{
					this.widget.Active = true;
				}
			}
			base.UpdatePropertyValue(this.widget.Active, null);
		}

		protected override void OnSetControl()
		{
			if (PropertyItem.Objects.Count > 1)
			{
				bool flag = false;
				bool flag2 = true;
				foreach (object obj in PropertyItem.Objects)
				{
					object obj2 = base.PropertyItem.Values[obj];
					if (obj2 != null && (bool)obj2)
					{
						flag = (bool)obj2;
					}
					else if (obj2 != null && !(bool)obj2)
					{
						flag2 = (bool)obj2;
					}
				}
				this.widget.Inconsistent = ((!flag || !flag2) && (flag || flag2));
				if (!this.widget.Inconsistent)
				{
					this.widget.Active = flag;
				}
			}
			else
			{
				object obj = base.PropertyItem.FirstValue;
				if (obj != null)
				{
					this.widget.Active = (bool)obj;
				}
			}
		}

		private CheckButtonEx widget;
	}
}
