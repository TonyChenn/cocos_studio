using System;
using System.ComponentModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model
{
	internal class LabelTooltipEditor : BaseEditor
	{
		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
		}

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

		protected override void OnSetControl()
		{
		}

		private Table table;

		private Label label;
	}
}
