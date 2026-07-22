using System;
using System.ComponentModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200005E RID: 94
	internal class ShadowOffsetEditor : BaseEditor
	{
		// Token: 0x06000338 RID: 824 RVA: 0x0000D790 File Offset: 0x0000B990
		protected override Widget OnCreateWidget()
		{
			this.xInnerEntry = new NoUndoNumEntry();
			this.xInnerEntry.DecimalPlaces = 2;
			FullEntryShell widget = EntryShellBuilder.CreateShell("X", this.xInnerEntry, LanguageInfo.NewFile_Pixel);
			this.yInnerEntry = new NoUndoNumEntry();
			this.yInnerEntry.DecimalPlaces = 2;
			FullEntryShell widget2 = EntryShellBuilder.CreateShell("Y", this.yInnerEntry, LanguageInfo.NewFile_Pixel);
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

		// Token: 0x06000339 RID: 825 RVA: 0x0000D85C File Offset: 0x0000BA5C
		protected override void OnSetControl()
		{
			ILabelEffect labelEffect = PropertyItem.Objects[0] as ILabelEffect;
			this.xInnerEntry.Value = labelEffect.ShadowOffsetX;
			this.yInnerEntry.Value = labelEffect.ShadowOffsetY;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000D8A0 File Offset: 0x0000BAA0
		private void XEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					ILabelEffect labelEffect = PropertyItem.Objects[i] as ILabelEffect;
					if (labelEffect != null)
					{
						labelEffect.ShadowOffsetX = e.Value;
					}
				}
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000D924 File Offset: 0x0000BB24
		private void YEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					ILabelEffect labelEffect = PropertyItem.Objects[i] as ILabelEffect;
					if (labelEffect != null)
					{
						labelEffect.ShadowOffsetY = e.Value;
					}
				}
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000D9A8 File Offset: 0x0000BBA8
		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "ShadowOffsetX" || e.PropertyName == "ShadowOffsetY")
			{
				base.SetControl();
			}
		}

		// Token: 0x0400018F RID: 399
		private NoUndoNumEntry xInnerEntry;

		// Token: 0x04000190 RID: 400
		private NoUndoNumEntry yInnerEntry;
	}
}
