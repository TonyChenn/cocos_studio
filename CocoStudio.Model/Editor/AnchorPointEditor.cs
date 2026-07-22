using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200008C RID: 140
	internal class AnchorPointEditor : BaseEditor
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00014E90 File Offset: 0x00013090
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00014EA4 File Offset: 0x000130A4
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

		// Token: 0x060004D9 RID: 1241 RVA: 0x00014F88 File Offset: 0x00013188
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

		// Token: 0x060004DA RID: 1242 RVA: 0x00015088 File Offset: 0x00013288
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

		// Token: 0x060004DB RID: 1243 RVA: 0x00015118 File Offset: 0x00013318
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

		// Token: 0x0400023D RID: 573
		private NoUndoNumEntry xInnerEntry;

		// Token: 0x0400023E RID: 574
		private NoUndoNumEntry yInnerEntry;
	}
}
