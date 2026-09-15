using System;
using System.Linq;

namespace Gtk
{
	public class FullEntryShell : EntryShell
	{
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

		protected override void OnCreate()
		{
		}

		public void SetUnitLabelFontSize(int fontSize)
		{
			this.unitLabel.SetFontSize((double)fontSize);
		}

		private HBox mainHBox;

		private Label unitLabel;

		private string _unitText;

		private Widget _additionWidget;
	}
}
