using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class TwoNumberEditor : BaseEditor
	{
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		public TwoNumberEditor(string labelX, string labelY)
		{
			this.xHeadText = labelX;
			this.yHeadText = labelY;
		}

		protected override Widget OnCreateWidget()
		{
			this.xInnerEntry = new NoUndoNumEntry();
			this.xInnerEntry.DecimalPlaces = 2;
			FullEntryShell widget = EntryShellBuilder.CreateShell(this.xHeadText, this.xInnerEntry);
			this.yInnerEntry = new NoUndoNumEntry();
			this.yInnerEntry.DecimalPlaces = 2;
			FullEntryShell widget2 = EntryShellBuilder.CreateShell(this.yHeadText, this.yInnerEntry);
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
			PointF pointF = (PointF)base.PropertyItem.Values[0];
			this.xInnerEntry.Value = pointF.X;
			this.yInnerEntry.Value = pointF.Y;
		}

		protected virtual void OnXValueChanged(EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					PointF pointF = (PointF)base.PropertyItem.Values[i];
					pointF.X = e.Value;
					base.PropertyItem.Values[i] = pointF;
				}
			}
		}

		protected virtual void OnYValueChanged(EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					PointF pointF = (PointF)base.PropertyItem.Values[i];
					pointF.Y = e.Value;
					base.PropertyItem.Values[i] = pointF;
				}
			}
		}

		protected void CompareNumber(Func<Slice3DObject, Slice3DObject, bool> funcX, Func<Slice3DObject, Slice3DObject, bool> funcY)
		{
			PointF pointF = (PointF)base.PropertyItem.Values[0];
			if (PropertyItem.Objects.Count <= 1)
			{
				object firstObject = PropertyItem.FirstObject;
				this.xInnerEntry.Value = pointF.X;
				this.yInnerEntry.Value = pointF.Y;
				return;
			}
			if (base.IsWhipNode<Slice3DObject>(funcX))
			{
				this.xInnerEntry.SetToSubState();
			}
			else
			{
				this.xInnerEntry.Value = pointF.X;
			}
			if (base.IsWhipNode<Slice3DObject>(funcY))
			{
				this.yInnerEntry.SetToSubState();
				return;
			}
			this.yInnerEntry.Value = pointF.Y;
		}

		private void XEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			this.OnXValueChanged(e);
		}

		private void YEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			this.OnYValueChanged(e);
		}

		protected NoUndoNumEntry xInnerEntry;

		protected NoUndoNumEntry yInnerEntry;

		private string xHeadText;

		private string yHeadText;
	}
}
