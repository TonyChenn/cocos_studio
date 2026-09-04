using System;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200009F RID: 159
	public class SliderEditor : BaseEditor
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x00017DEC File Offset: 0x00015FEC
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00017DFF File Offset: 0x00015FFF
		public SliderEditor()
		{
			this.showAsFloat = false;
			this.adaptToPercent = true;
			this.entryLabelText = "%";
			this.isScrollRound = false;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00017E3C File Offset: 0x0001603C
		public SliderEditor(bool isFloat, bool toPercent, bool isRound, string labelText)
		{
			this.showAsFloat = isFloat;
			this.adaptToPercent = toPercent;
			this.entryLabelText = LanguageOption.GetValueBykey(labelText);
			this.isScrollRound = isRound;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00017E7C File Offset: 0x0001607C
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

		// Token: 0x06000567 RID: 1383 RVA: 0x000181B4 File Offset: 0x000163B4
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

		// Token: 0x06000568 RID: 1384 RVA: 0x00018244 File Offset: 0x00016444
		private void EntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			if (!this.isChangedByHScale)
			{
				this.hscale.Adjustment.Value = (double)this.entry.Value;
			}
			this.SetPropertyValueByHScale();
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00018280 File Offset: 0x00016480
		private void HScaleChangeValueHandler(object o, EventArgs args)
		{
			this.isChangedByHScale = true;
			this.entry.Value = (float)this.hscale.Adjustment.Value;
			this.isChangedByHScale = false;
			this.SetPropertyValueByHScale();
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x000182B8 File Offset: 0x000164B8
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

		// Token: 0x04000285 RID: 645
		private bool adaptToPercent;

		// Token: 0x04000286 RID: 646
		private bool showAsFloat;

		// Token: 0x04000287 RID: 647
		private string entryLabelText;

		// Token: 0x04000288 RID: 648
		private bool isScrollRound;

		// Token: 0x04000289 RID: 649
		private float scaleRate = 1f;

		// Token: 0x0400028A RID: 650
		private bool isChangedByHScale = false;

		// Token: 0x0400028B RID: 651
		private SliderEditor.HScaleEx hscale;

		// Token: 0x0400028C RID: 652
		private NoUndoNumEntry entry;

		// Token: 0x020000A0 RID: 160
		private class HScaleEx : HScale
		{
			// Token: 0x1700016E RID: 366
			// (get) Token: 0x0600056B RID: 1387 RVA: 0x00018318 File Offset: 0x00016518
			// (set) Token: 0x0600056C RID: 1388 RVA: 0x0001832F File Offset: 0x0001652F
			public double ScrollStepValue { get; set; }

			// Token: 0x0600056D RID: 1389 RVA: 0x00018338 File Offset: 0x00016538
			public HScaleEx(double min, double max, double step) : base(min, max, step)
			{
				this.ScrollStepValue = step;
			}

			// Token: 0x0600056E RID: 1390 RVA: 0x00018355 File Offset: 0x00016555
			protected override void OnFocusGrabbed()
			{
				base.CanFocus = this.isPress;
				base.OnFocusGrabbed();
			}

			// Token: 0x0600056F RID: 1391 RVA: 0x0001836C File Offset: 0x0001656C
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

			// Token: 0x06000570 RID: 1392 RVA: 0x000183D4 File Offset: 0x000165D4
			protected override bool OnButtonPressEvent(EventButton evnt)
			{
				this.isPress = true;
				return base.OnButtonPressEvent(evnt);
			}

			// Token: 0x06000571 RID: 1393 RVA: 0x000183F4 File Offset: 0x000165F4
			protected override bool OnFocusOutEvent(EventFocus evnt)
			{
				this.isPress = false;
				return base.OnFocusOutEvent(evnt);
			}

			// Token: 0x0400028D RID: 653
			private bool isPress = false;
		}
	}
}
