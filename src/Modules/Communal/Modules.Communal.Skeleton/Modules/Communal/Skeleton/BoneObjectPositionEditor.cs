using System;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace Modules.Communal.Skeleton
{
	// Token: 0x02000026 RID: 38
	public class BoneObjectPositionEditor : BaseEditor
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00008F2F File Offset: 0x0000712F
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00008F34 File Offset: 0x00007134
		protected override Widget OnCreateWidget()
		{
			this.xInnerEntry = new NoUndoNumEntry();
			this.xInnerEntry.DecimalPlaces = 2;
			this.xInnerEntry.SetEntryProperty(false, 2, 1f);
			EntryShell widget = EntryShellBuilder.CreateShell("X", this.xInnerEntry);
			this.yInnerEntry = new NoUndoNumEntry();
			this.yInnerEntry.DecimalPlaces = 2;
			this.yInnerEntry.SetEntryProperty(false, 2, 1f);
			EntryShell widget2 = EntryShellBuilder.CreateShell("Y", this.yInnerEntry);
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

		// Token: 0x060001A5 RID: 421 RVA: 0x00009040 File Offset: 0x00007240
		protected override void OnSetControl()
		{
			BoneObject boneObject = PropertyItem.FirstObject as BoneObject;
			PointF position = boneObject.Position;
			if (PropertyItem.Objects.Count <= 1)
			{
				this.xInnerEntry.Value = position.X;
				this.yInnerEntry.Value = position.Y;
				return;
			}
			Func<BoneObject, BoneObject, bool> func = (BoneObject a, BoneObject b) => a.Position.X == b.Position.X;
			Func<BoneObject, BoneObject, bool> func2 = (BoneObject a, BoneObject b) => a.Position.Y == b.Position.Y;
			if (base.IsWhipNode<BoneObject>(func))
			{
				this.xInnerEntry.SetToSubState();
			}
			else
			{
				this.xInnerEntry.Value = position.X;
			}
			if (base.IsWhipNode<BoneObject>(func2))
			{
				this.yInnerEntry.SetToSubState();
				return;
			}
			this.yInnerEntry.Value = position.Y;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000911C File Offset: 0x0000731C
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

		// Token: 0x060001A7 RID: 423 RVA: 0x00009198 File Offset: 0x00007398
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

		// Token: 0x04000081 RID: 129
		private NoUndoNumEntry xInnerEntry;

		// Token: 0x04000082 RID: 130
		private NoUndoNumEntry yInnerEntry;
	}
}
