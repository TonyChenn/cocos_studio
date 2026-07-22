using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200009E RID: 158
	internal class SkewEditor : BaseEditor
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x00017A8C File Offset: 0x00015C8C
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00017AA0 File Offset: 0x00015CA0
		protected override Widget OnCreateWidget()
		{
			this.xInnerEntry = new NoUndoNumEntry();
			this.xInnerEntry.DecimalPlaces = 2;
			FullEntryShell widget = EntryShellBuilder.CreateShell("X", this.xInnerEntry, LanguageInfo.Property_Angle);
			this.yInnerEntry = new NoUndoNumEntry();
			this.yInnerEntry.DecimalPlaces = 2;
			FullEntryShell widget2 = EntryShellBuilder.CreateShell("Y", this.yInnerEntry, LanguageInfo.Property_Angle);
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

		// Token: 0x0600055D RID: 1373 RVA: 0x00017B6C File Offset: 0x00015D6C
		protected override void OnSetControl()
		{
			AbstractNodeObject abstractNodeObject = PropertyItem.FirstObject as AbstractNodeObject;
			ScaleValue rotationSkew = abstractNodeObject.RotationSkew;
			if (PropertyItem.Objects.Count > 1)
			{
				Func<AbstractNodeObject, AbstractNodeObject, bool> func = (AbstractNodeObject a, AbstractNodeObject b) => a.RotationSkew.ScaleX == b.RotationSkew.ScaleX;
				Func<AbstractNodeObject, AbstractNodeObject, bool> func2 = (AbstractNodeObject a, AbstractNodeObject b) => a.RotationSkew.ScaleY == b.RotationSkew.ScaleY;
				if (base.IsWhipNode<AbstractNodeObject>(func))
				{
					this.xInnerEntry.SetToSubState();
				}
				else
				{
					this.xInnerEntry.Value = rotationSkew.ScaleX;
				}
				if (base.IsWhipNode<AbstractNodeObject>(func2))
				{
					this.yInnerEntry.SetToSubState();
				}
				else
				{
					this.yInnerEntry.Value = rotationSkew.ScaleY;
				}
			}
			else
			{
				this.xInnerEntry.Value = rotationSkew.ScaleX;
				this.yInnerEntry.Value = rotationSkew.ScaleY;
			}
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x00017C6C File Offset: 0x00015E6C
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

		// Token: 0x0600055F RID: 1375 RVA: 0x00017CFC File Offset: 0x00015EFC
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

		// Token: 0x04000281 RID: 641
		private NoUndoNumEntry xInnerEntry;

		// Token: 0x04000282 RID: 642
		private NoUndoNumEntry yInnerEntry;
	}
}
