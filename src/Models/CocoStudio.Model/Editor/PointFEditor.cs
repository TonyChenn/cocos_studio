using System;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000065 RID: 101
	public class PointFEditor : BaseEditor
	{
		// Token: 0x0600035E RID: 862 RVA: 0x0000EB5C File Offset: 0x0000CD5C
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

		// Token: 0x0600035F RID: 863 RVA: 0x0000ECBC File Offset: 0x0000CEBC
		protected override void OnSetControl()
		{
			PointF pointF = base.PropertyItem.Values[0] as PointF;
			this.xInnerEntry.Value = pointF.X;
			this.yInnerEntry.Value = pointF.Y;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000ED08 File Offset: 0x0000CF08
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

		// Token: 0x06000361 RID: 865 RVA: 0x0000ED98 File Offset: 0x0000CF98
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

		// Token: 0x040001A0 RID: 416
		private NoUndoNumEntry xInnerEntry;

		// Token: 0x040001A1 RID: 417
		private NoUndoNumEntry yInnerEntry;
	}
}
