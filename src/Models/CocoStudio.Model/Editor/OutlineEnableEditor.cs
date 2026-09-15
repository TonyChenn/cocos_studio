using System;
using System.ComponentModel;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class OutlineEnableEditor : BaseEditor
	{
		protected override Widget OnCreateWidget()
		{
			this.labelEffectObj = (PropertyItem.FirstObject as ILabelEffect);
			Widget result;
			if (this.labelEffectObj == null)
			{
				result = new EventBox();
			}
			else
			{
				this.checkBtn = new CheckButtonEx();
				this.checkBtn.Clicked += this.CheckButtonClickedHandler;
				HBox hbox = new HBox();
				Alignment alignment = new Alignment(0.5f, 0.5f, 1f, 1f);
				alignment.WidthRequest = 1;
				alignment.HeightRequest = 24;
				hbox.PackStart(alignment, false, false, 0U);
				hbox.PackStart(this.checkBtn, false, false, 0U);
				hbox.Spacing = -4;
				alignment.Show();
				this.checkBtn.Show();
				this.noticeIcon = new TooltipIcon();
				this.noticeIcon.Text = LanguageInfo.Property_OutlineTip;
				HBox hbox2 = new HBox();
				hbox2.Spacing = (int)PropertyPadStyle.mainColumnSpacing;
				hbox2.PackStart(hbox, false, false, 0U);
				hbox2.PackStart(this.noticeIcon, false, false, 0U);
				hbox.Show();
				this.noticeIcon.Show();
				base.SetControl();
				this.RefreshNotice();
				result = hbox2;
			}
			return result;
		}

		private void RefreshNotice()
		{
			object value = this.labelEffectObj.GetType().GetProperty("FontResource").GetValue(this.labelEffectObj, null);
			if (!(value is TTFFile))
			{
				this.labelEffectObj.OutlineEnabled = false;
				this.checkBtn.Sensitive = false;
				this.noticeIcon.Show();
			}
			else
			{
				this.checkBtn.Sensitive = true;
				this.noticeIcon.Hide();
			}
		}

		private void CheckButtonClickedHandler(object sender, EventArgs e)
		{
			this.labelEffectObj.OutlineEnabled = this.checkBtn.Active;
		}

		protected override void OnSetControl()
		{
			this.checkBtn.Active = this.labelEffectObj.OutlineEnabled;
		}

		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "FontResource")
			{
				this.RefreshNotice();
				base.SetControl();
			}
			else if (e.PropertyName == "OutlineEnabled")
			{
				base.SetControl();
			}
		}

		private TooltipIcon noticeIcon;

		private CheckButtonEx checkBtn;

		private ILabelEffect labelEffectObj;
	}
}
