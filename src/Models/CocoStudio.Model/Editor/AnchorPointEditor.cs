using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class AnchorPointEditor : BaseEditor
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
			this.xInnerEntry.ScrollNum = 0.1f;
			FullEntryShell widget = EntryShellBuilder.CreateShell("X", this.xInnerEntry);
			this.yInnerEntry = new NoUndoNumEntry();
			this.yInnerEntry.DecimalPlaces = 2;
			this.yInnerEntry.ScrollNum = 0.1f;
			FullEntryShell widget2 = EntryShellBuilder.CreateShell("Y", this.yInnerEntry);
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
			AbstractNodeObject abstractNodeObject = PropertyItem.FirstObject as AbstractNodeObject;
			ScaleValue anchorPoint = abstractNodeObject.AnchorPoint;
			if (PropertyItem.Objects.Count > 1)
			{
				Func<AbstractNodeObject, AbstractNodeObject, bool> func = (AbstractNodeObject a, AbstractNodeObject b) => Math.Round((double)a.AnchorPoint.ScaleX, 2) == Math.Round((double)b.AnchorPoint.ScaleX, 2);
				Func<AbstractNodeObject, AbstractNodeObject, bool> func2 = (AbstractNodeObject a, AbstractNodeObject b) => Math.Round((double)a.AnchorPoint.ScaleY, 2) == Math.Round((double)b.AnchorPoint.ScaleY, 2);
				if (base.IsWhipNode<AbstractNodeObject>(func))
				{
					this.xInnerEntry.SetToSubState();
				}
				else
				{
					this.xInnerEntry.Value = anchorPoint.ScaleX;
				}
				if (base.IsWhipNode<AbstractNodeObject>(func2))
				{
					this.yInnerEntry.SetToSubState();
				}
				else
				{
					this.yInnerEntry.Value = anchorPoint.ScaleY;
				}
			}
			else
			{
				this.xInnerEntry.Value = anchorPoint.ScaleX;
				this.yInnerEntry.Value = anchorPoint.ScaleY;
			}
		}

		private void XEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					ScaleValue scaleValue = base.PropertyItem.Values[i] as ScaleValue;
					scaleValue.ScaleX = e.Value;
					base.PropertyItem.Values[i] = scaleValue;
				}
			}
		}

		private void YEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					ScaleValue scaleValue = base.PropertyItem.Values[i] as ScaleValue;
					scaleValue.ScaleY = e.Value;
					base.PropertyItem.Values[i] = scaleValue;
				}
			}
		}

		private NoUndoNumEntry xInnerEntry;

		private NoUndoNumEntry yInnerEntry;
	}
}
