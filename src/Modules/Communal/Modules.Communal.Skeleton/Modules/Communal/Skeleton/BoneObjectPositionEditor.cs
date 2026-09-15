using System;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace Modules.Communal.Skeleton
{
	public class BoneObjectPositionEditor : BaseEditor
	{
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

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
