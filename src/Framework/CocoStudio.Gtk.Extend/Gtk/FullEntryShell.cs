using System;
using System.Linq;

namespace Gtk
{
	// Token: 0x0200005F RID: 95
	public class FullEntryShell : EntryShell
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00008F70 File Offset: 0x00007170
		// (set) Token: 0x060001FF RID: 511 RVA: 0x00008F88 File Offset: 0x00007188
		public string UnitText
		{
			get
			{
				return this._unitText;
			}
			set
			{
				this._unitText = value;
				this.unitLabel.Text = this._unitText + " ";
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000200 RID: 512 RVA: 0x00008FB0 File Offset: 0x000071B0
		// (set) Token: 0x06000201 RID: 513 RVA: 0x00008FC8 File Offset: 0x000071C8
		public Widget AdditionWidget
		{
			get
			{
				return this._additionWidget;
			}
			set
			{
				if (this._additionWidget != null && this.mainHBox.Children.Contains(this._additionWidget))
				{
					this.mainHBox.Remove(this._additionWidget);
				}
				this._additionWidget = value;
				if (this._additionWidget != null)
				{
					this.mainHBox.PackStart(this._additionWidget, false, false, 0U);
					this._additionWidget.ShowAll();
				}
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00009048 File Offset: 0x00007248
		public FullEntryShell(string titleText, Entry entry, string unitText, Widget widget) : base(entry)
		{
			this.mainHBox = new HBox();
			if (!string.IsNullOrEmpty(titleText))
			{
				Label label = new Label(titleText);
				if (titleText.Length == 1)
				{
					label.WidthRequest = 16;
				}
				EventBox eventBox = new EventBox();
				eventBox.WidthRequest = 1;
				eventBox.ModifyBg(StateType.Normal, WindowStyle.WindowLineColor);
				eventBox.ModifyBg(StateType.Insensitive, WindowStyle.LineDarkColor);
				this.mainHBox.PackStart(label, false, false, 0U);
				this.mainHBox.PackStart(eventBox, false, false, 0U);
			}
			this.mainHBox.PackStart(base.InnerEntry);
			if (!string.IsNullOrEmpty(unitText))
			{
				this.unitLabel = new Label();
				this.unitLabel.Sensitive = false;
				this.mainHBox.PackStart(this.unitLabel, false, false, 0U);
				this.UnitText = unitText;
			}
			if (widget != null)
			{
				this.AdditionWidget = widget;
			}
			base.Add(this.mainHBox);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00009152 File Offset: 0x00007352
		protected override void OnCreate()
		{
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00009155 File Offset: 0x00007355
		public void SetUnitLabelFontSize(int fontSize)
		{
			this.unitLabel.SetFontSize((double)fontSize);
		}

		// Token: 0x04000301 RID: 769
		private HBox mainHBox;

		// Token: 0x04000302 RID: 770
		private Label unitLabel;

		// Token: 0x04000303 RID: 771
		private string _unitText;

		// Token: 0x04000304 RID: 772
		private Widget _additionWidget;
	}
}
