using System;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000060 RID: 96
	internal class LayerSizeEditor : BaseEditor
	{
		// Token: 0x06000342 RID: 834 RVA: 0x0000DAD0 File Offset: 0x0000BCD0
		protected override Widget OnCreateWidget()
		{
			this.xInnerEntry = new NoUndoNumEntry();
			this.xInnerEntry.DecimalPlaces = 2;
			this.xInnerEntry.MaxValue = int.MaxValue;
			this.xInnerEntry.MinValue = 0;
			FullEntryShell widget = EntryShellBuilder.CreateShell("W", this.xInnerEntry, LanguageInfo.NewFile_Pixel);
			this.yInnerEntry = new NoUndoNumEntry();
			this.yInnerEntry.DecimalPlaces = 2;
			this.yInnerEntry.MaxValue = int.MaxValue;
			this.yInnerEntry.MinValue = 0;
			FullEntryShell widget2 = EntryShellBuilder.CreateShell("H", this.yInnerEntry, LanguageInfo.NewFile_Pixel);
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

		// Token: 0x06000343 RID: 835 RVA: 0x0000DBD8 File Offset: 0x0000BDD8
		protected override void OnSetControl()
		{
			SizeF sizeF = base.PropertyItem.FirstValue as SizeF;
			this.xInnerEntry.Value = sizeF.Width;
			this.yInnerEntry.Value = sizeF.Height;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000DC1C File Offset: 0x0000BE1C
		private void XEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					SizeF sizeF = base.PropertyItem.Values[i] as SizeF;
					sizeF.Width = e.Value;
					base.PropertyItem.Values[i] = sizeF;
				}
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000DCAC File Offset: 0x0000BEAC
		private void YEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					SizeF sizeF = base.PropertyItem.Values[i] as SizeF;
					sizeF.Height = e.Value;
					base.PropertyItem.Values[i] = sizeF;
				}
			}
		}

		// Token: 0x04000193 RID: 403
		private NoUndoNumEntry xInnerEntry;

		// Token: 0x04000194 RID: 404
		private NoUndoNumEntry yInnerEntry;
	}
}
