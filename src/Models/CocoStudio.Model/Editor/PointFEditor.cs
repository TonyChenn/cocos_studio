using System;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	public class PointFEditor : BaseEditor
	{
		protected override Widget OnCreateWidget()
		{
			this.xInnerEntry = new NoUndoNumEntry();
			EntryShell widget = EntryShellBuilder.CreateShell("X", this.xInnerEntry);
			this.yInnerEntry = new NoUndoNumEntry();
			EntryShell widget2 = EntryShellBuilder.CreateShell("Y", this.yInnerEntry);
			ValueRangeAttribute valueRangeAttribute = base.PropertyItem.Attributes[typeof(ValueRangeAttribute)] as ValueRangeAttribute;
			if (valueRangeAttribute != null)
			{
				this.xInnerEntry.DecimalPlaces = (this.yInnerEntry.DecimalPlaces = 2);
				this.xInnerEntry.ScrollNum = (this.yInnerEntry.ScrollNum = valueRangeAttribute.Step);
				this.xInnerEntry.MaxValue = (this.yInnerEntry.MaxValue = valueRangeAttribute.MaxValue);
				this.xInnerEntry.MinValue = (this.yInnerEntry.MinValue = valueRangeAttribute.MinValue);
			}
			HBox hbox = new HBox();
			hbox.Spacing = 4;
			hbox.PackStart(widget);
			hbox.PackStart(widget2);
			hbox.ShowAll();
			base.SetControl();
			this.xInnerEntry.EntryValueChanged += this.XEntryValueChangedHandler;
			this.yInnerEntry.EntryValueChanged += this.YEntryValueChangedHandler;
			return hbox;
		}

		protected override void OnSetControl()
		{
			PointF pointF = base.PropertyItem.Values[0] as PointF;
			this.xInnerEntry.Value = pointF.X;
			this.yInnerEntry.Value = pointF.Y;
		}

		private void XEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					PointF pointF = base.PropertyItem.Values[i] as PointF;
					pointF.X = e.Value;
					base.PropertyItem.Values[i] = pointF;
				}
			}
		}

		private void YEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					PointF pointF = base.PropertyItem.Values[i] as PointF;
					pointF.Y = e.Value;
					base.PropertyItem.Values[i] = pointF;
				}
			}
		}

		private NoUndoNumEntry xInnerEntry;

		private NoUndoNumEntry yInnerEntry;
	}
}
