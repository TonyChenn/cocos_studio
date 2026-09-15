using System;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	public class SliderEditor : BaseEditor
	{
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		public SliderEditor()
		{
			this.showAsFloat = false;
			this.adaptToPercent = true;
			this.entryLabelText = "%";
			this.isScrollRound = false;
		}

		public SliderEditor(bool isFloat, bool toPercent, bool isRound, string labelText)
		{
			this.showAsFloat = isFloat;
			this.adaptToPercent = toPercent;
			this.entryLabelText = LanguageOption.GetValueBykey(labelText);
			this.isScrollRound = isRound;
		}

		protected override Widget OnCreateWidget()
		{
			this.hscale = new SliderEditor.HScaleEx(0.0, 100.0, 1.0);
			this.hscale.CanFocus = true;
			this.hscale.DrawValue = false;
			this.hscale.Digits = 0;
			this.hscale.ValuePos = PositionType.Top;
			this.hscale.WidthRequest = 20;
			this.hscale.Adjustment.Upper = 100.0;
			this.hscale.Adjustment.Lower = 0.0;
			this.hscale.Adjustment.StepIncrement = 1.0;
			this.hscale.Adjustment.PageIncrement = 10.0;
			this.hscale.ValueChanged += this.HScaleChangeValueHandler;
			this.entry = new NoUndoNumEntry();
			this.entry.CanFocus = true;
			this.entry.WidthRequest = 40;
			this.entry.MaxValue = 100;
			this.entry.MinValue = 0;
			this.entry.IsInteger = !this.showAsFloat;
			this.entry.DecimalPlaces = (this.showAsFloat ? 2 : 0);
			this.entry.IsRound = this.isScrollRound;
			this.entry.EntryValueChanged += this.EntryValueChangedHandler;
			FullEntryShell fullEntryShell = EntryShellBuilder.CreateShell(this.entry, this.entryLabelText);
			fullEntryShell.WidthRequest = 60;
			HBox hbox = new HBox();
			hbox.Spacing = 6;
			hbox.PackStart(this.hscale, true, true, 0U);
			hbox.PackStart(fullEntryShell, false, false, 0U);
			hbox.ShowAll();
			ValueRangeAttribute valueRangeAttribute = base.PropertyItem.Attributes[typeof(ValueRangeAttribute)] as ValueRangeAttribute;
			if (valueRangeAttribute != null)
			{
				this.hscale.Adjustment.Lower = (double)valueRangeAttribute.MinValue;
				this.hscale.Adjustment.StepIncrement = (double)valueRangeAttribute.Step;
				this.hscale.Adjustment.PageIncrement = (double)valueRangeAttribute.PageStep;
				this.entry.MinValue = valueRangeAttribute.MinValue;
				this.entry.ScrollNum = valueRangeAttribute.Step;
				this.hscale.ScrollStepValue = (double)valueRangeAttribute.Step;
				if (this.adaptToPercent)
				{
					this.entry.MaxValue = 100;
					this.hscale.Adjustment.Upper = 100.0;
					this.hscale.Adjustment.PageIncrement = 10.0;
					this.scaleRate = (float)valueRangeAttribute.MaxValue / 100f;
				}
				else
				{
					this.scaleRate = 1f;
					this.entry.MaxValue = valueRangeAttribute.MaxValue;
					this.hscale.Adjustment.Upper = (double)this.entry.MaxValue;
				}
			}
			base.SetControl();
			return hbox;
		}

		protected override void OnSetControl()
		{
			if (!base.CheckIsSameValue())
			{
				this.entry.SetToSubState();
				this.hscale.Adjustment.Value = 0.0;
			}
			else
			{
				float num = Convert.ToSingle(base.PropertyItem.FirstValue);
				float num2 = this.showAsFloat ? num : ((float)Math.Round((double)(num / this.scaleRate)));
				this.hscale.Adjustment.Value = (double)num2;
				this.entry.Value = num2;
			}
		}

		private void EntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			if (!this.isChangedByHScale)
			{
				this.hscale.Adjustment.Value = (double)this.entry.Value;
			}
			this.SetPropertyValueByHScale();
		}

		private void HScaleChangeValueHandler(object o, EventArgs args)
		{
			this.isChangedByHScale = true;
			this.entry.Value = (float)this.hscale.Adjustment.Value;
			this.isChangedByHScale = false;
			this.SetPropertyValueByHScale();
		}

		private void SetPropertyValueByHScale()
		{
			float num = (float)Math.Round(this.hscale.Adjustment.Value * (double)this.scaleRate, 2);
			if (this.showAsFloat)
			{
				base.UpdatePropertyValue(num, null);
			}
			else
			{
				base.UpdatePropertyValue((int)num, null);
			}
		}

		private bool adaptToPercent;

		private bool showAsFloat;

		private string entryLabelText;

		private bool isScrollRound;

		private float scaleRate = 1f;

		private bool isChangedByHScale = false;

		private SliderEditor.HScaleEx hscale;

		private NoUndoNumEntry entry;

		private class HScaleEx : HScale
		{
			public double ScrollStepValue { get; set; }

			public HScaleEx(double min, double max, double step) : base(min, max, step)
			{
				this.ScrollStepValue = step;
			}

			protected override void OnFocusGrabbed()
			{
				base.CanFocus = this.isPress;
				base.OnFocusGrabbed();
			}

			protected override bool OnScrollEvent(EventScroll evnt)
			{
				bool result;
				if (!base.IsFocus)
				{
					result = false;
				}
				else
				{
					if (evnt.Direction == ScrollDirection.Up || evnt.Direction == ScrollDirection.Left)
					{
						base.Value += this.ScrollStepValue;
					}
					else
					{
						base.Value -= this.ScrollStepValue;
					}
					result = true;
				}
				return result;
			}

			protected override bool OnButtonPressEvent(EventButton evnt)
			{
				this.isPress = true;
				return base.OnButtonPressEvent(evnt);
			}

			protected override bool OnFocusOutEvent(EventFocus evnt)
			{
				this.isPress = false;
				return base.OnFocusOutEvent(evnt);
			}

			private bool isPress = false;
		}
	}
}
