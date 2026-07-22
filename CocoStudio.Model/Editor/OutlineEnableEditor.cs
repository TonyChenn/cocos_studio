using System;
using System.ComponentModel;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200005D RID: 93
	internal class OutlineEnableEditor : BaseEditor
	{
		// Token: 0x06000332 RID: 818 RVA: 0x0000D540 File Offset: 0x0000B740
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

		// Token: 0x06000333 RID: 819 RVA: 0x0000D678 File Offset: 0x0000B878
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

		// Token: 0x06000334 RID: 820 RVA: 0x0000D6FA File Offset: 0x0000B8FA
		private void CheckButtonClickedHandler(object sender, EventArgs e)
		{
			this.labelEffectObj.OutlineEnabled = this.checkBtn.Active;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000D714 File Offset: 0x0000B914
		protected override void OnSetControl()
		{
			this.checkBtn.Active = this.labelEffectObj.OutlineEnabled;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000D730 File Offset: 0x0000B930
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

		// Token: 0x0400018C RID: 396
		private TooltipIcon noticeIcon;

		// Token: 0x0400018D RID: 397
		private CheckButtonEx checkBtn;

		// Token: 0x0400018E RID: 398
		private ILabelEffect labelEffectObj;
	}
}
