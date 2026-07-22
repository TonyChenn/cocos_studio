using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000011 RID: 17
	internal class ThreeNumberEditor : BaseEditor
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000031BF File Offset: 0x000013BF
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000031C2 File Offset: 0x000013C2
		public ThreeNumberEditor()
		{
			this.xHeadText = "X";
			this.yHeadText = "Y";
			this.zHeadText = "Z";
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000031EB File Offset: 0x000013EB
		public ThreeNumberEditor(string xHead, string yHead, string zHead)
		{
			this.xHeadText = xHead;
			this.yHeadText = yHead;
			this.zHeadText = zHead;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003208 File Offset: 0x00001408
		protected override Widget OnCreateWidget()
		{
			this.xInnerEntry = new NoUndoNumEntry();
			this.xInnerEntry.DecimalPlaces = 3;
			FullEntryShell widget = EntryShellBuilder.CreateShell(this.xHeadText, this.xInnerEntry);
			this.yInnerEntry = new NoUndoNumEntry();
			this.yInnerEntry.DecimalPlaces = 3;
			FullEntryShell widget2 = EntryShellBuilder.CreateShell(this.yHeadText, this.yInnerEntry);
			this.zInnerEntry = new NoUndoNumEntry();
			this.zInnerEntry.DecimalPlaces = 3;
			FullEntryShell widget3 = EntryShellBuilder.CreateShell(this.zHeadText, this.zInnerEntry);
			HBox hbox = new HBox();
			hbox.Spacing = 4;
			hbox.PackStart(widget);
			hbox.PackStart(widget2);
			hbox.PackStart(widget3);
			hbox.ShowAll();
			base.SetControl();
			this.xInnerEntry.EntryValueChanged += this.XEntryValueChangedHandler;
			this.yInnerEntry.EntryValueChanged += this.YEntryValueChangedHandler;
			this.zInnerEntry.EntryValueChanged += this.ZEntryValueChangedHandler;
			return hbox;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003304 File Offset: 0x00001504
		protected override void OnSetControl()
		{
			Point3F point3F = (Point3F)base.PropertyItem.Values[0];
			this.xInnerEntry.Value = point3F.X;
			this.yInnerEntry.Value = point3F.Y;
			this.zInnerEntry.Value = point3F.Z;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000335C File Offset: 0x0000155C
		protected virtual void OnXValueChanged(EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					Point3F point3F = (Point3F)base.PropertyItem.Values[i];
					point3F.X = e.Value;
					base.PropertyItem.Values[i] = point3F;
				}
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000033D8 File Offset: 0x000015D8
		protected virtual void OnYValueChanged(EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					Point3F point3F = (Point3F)base.PropertyItem.Values[i];
					point3F.Y = e.Value;
					base.PropertyItem.Values[i] = point3F;
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003454 File Offset: 0x00001654
		protected virtual void OnZValueChanged(EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					Point3F point3F = (Point3F)base.PropertyItem.Values[i];
					point3F.Z = e.Value;
					base.PropertyItem.Values[i] = point3F;
				}
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000034D0 File Offset: 0x000016D0
		protected void CompareNumber(Func<Node3DObject, Node3DObject, bool> funcX, Func<Node3DObject, Node3DObject, bool> funcY, Func<Node3DObject, Node3DObject, bool> funcZ)
		{
			Point3F point3F = (Point3F)base.PropertyItem.Values[0];
			if (PropertyItem.Objects.Count <= 1)
			{
				object firstObject = PropertyItem.FirstObject;
				this.xInnerEntry.Value = point3F.X;
				this.yInnerEntry.Value = point3F.Y;
				this.zInnerEntry.Value = point3F.Z;
				return;
			}
			if (base.IsWhipNode<Node3DObject>(funcX))
			{
				this.xInnerEntry.SetToSubState();
			}
			else
			{
				this.xInnerEntry.Value = point3F.X;
			}
			if (base.IsWhipNode<Node3DObject>(funcY))
			{
				this.yInnerEntry.SetToSubState();
			}
			else
			{
				this.yInnerEntry.Value = point3F.Y;
			}
			if (base.IsWhipNode<Node3DObject>(funcZ))
			{
				this.zInnerEntry.SetToSubState();
				return;
			}
			this.zInnerEntry.Value = point3F.Z;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000035AF File Offset: 0x000017AF
		private void XEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			this.OnXValueChanged(e);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000035B8 File Offset: 0x000017B8
		private void YEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			this.OnYValueChanged(e);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000035C1 File Offset: 0x000017C1
		private void ZEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			this.OnZValueChanged(e);
		}

		// Token: 0x04000042 RID: 66
		protected NoUndoNumEntry xInnerEntry;

		// Token: 0x04000043 RID: 67
		protected NoUndoNumEntry yInnerEntry;

		// Token: 0x04000044 RID: 68
		protected NoUndoNumEntry zInnerEntry;

		// Token: 0x04000045 RID: 69
		private string xHeadText;

		// Token: 0x04000046 RID: 70
		private string yHeadText;

		// Token: 0x04000047 RID: 71
		private string zHeadText;
	}
}
