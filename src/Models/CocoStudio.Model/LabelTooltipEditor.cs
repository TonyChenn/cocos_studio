using System;
using System.ComponentModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model
{
	// Token: 0x0200005F RID: 95
	internal class LabelTooltipEditor : BaseEditor
	{
		// Token: 0x0600033E RID: 830 RVA: 0x0000D9F4 File Offset: 0x0000BBF4
		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000D9F8 File Offset: 0x0000BBF8
		protected override Widget OnCreateWidget()
		{
			this.table = new Table(1U, 2U, false);
			this.label = new Label();
			this.label.Text = LanguageOption.GetValueBykey(base.PropertyItem.Values[0].ToString());
			this.label.ModifyFg(StateType.Normal, WindowStyle.LableToolTipColor);
			this.label.SetFontSize(10.0);
			this.label.Wrap = true;
			this.table.Attach(this.label, 0U, 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.table.Attach(new Label(), 1U, 2U, 0U, 1U, AttachOptions.Expand, AttachOptions.Fill, 0U, 0U);
			this.table.ShowAll();
			return this.table;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000DAC3 File Offset: 0x0000BCC3
		protected override void OnSetControl()
		{
		}

		// Token: 0x04000191 RID: 401
		private Table table;

		// Token: 0x04000192 RID: 402
		private Label label;
	}
}
