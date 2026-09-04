using System;
using Gtk;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x0200000F RID: 15
	public class BoolEditor : BaseEditor
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002CBC File Offset: 0x00000EBC
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00002CD0 File Offset: 0x00000ED0
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

		// Token: 0x06000066 RID: 102 RVA: 0x00002D33 File Offset: 0x00000F33
		private void CheckButtonClickedHandler(object sender, EventArgs e)
		{
			this.OnCheckButtonClicked();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002D40 File Offset: 0x00000F40
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

		// Token: 0x06000068 RID: 104 RVA: 0x00002DA8 File Offset: 0x00000FA8
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

		// Token: 0x04000015 RID: 21
		private CheckButtonEx widget;
	}
}
