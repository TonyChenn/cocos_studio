using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200000B RID: 11
	internal class TwoNumberEditor : BaseEditor
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000077 RID: 119 RVA: 0x000028D8 File Offset: 0x00000AD8
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000028DB File Offset: 0x00000ADB
		public TwoNumberEditor(string labelX, string labelY)
		{
			this.xHeadText = labelX;
			this.yHeadText = labelY;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000028F4 File Offset: 0x00000AF4
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

		// Token: 0x0600007A RID: 122 RVA: 0x000029AC File Offset: 0x00000BAC
		protected override void OnSetControl()
		{
			PointF pointF = (PointF)base.PropertyItem.Values[0];
			this.xInnerEntry.Value = pointF.X;
			this.yInnerEntry.Value = pointF.Y;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000029F4 File Offset: 0x00000BF4
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

		// Token: 0x0600007C RID: 124 RVA: 0x00002A70 File Offset: 0x00000C70
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

		// Token: 0x0600007D RID: 125 RVA: 0x00002AEC File Offset: 0x00000CEC
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

		// Token: 0x0600007E RID: 126 RVA: 0x00002B93 File Offset: 0x00000D93
		private void XEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			this.OnXValueChanged(e);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002B9C File Offset: 0x00000D9C
		private void YEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			this.OnYValueChanged(e);
		}

		// Token: 0x04000036 RID: 54
		protected NoUndoNumEntry xInnerEntry;

		// Token: 0x04000037 RID: 55
		protected NoUndoNumEntry yInnerEntry;

		// Token: 0x04000038 RID: 56
		private string xHeadText;

		// Token: 0x04000039 RID: 57
		private string yHeadText;
	}
}
