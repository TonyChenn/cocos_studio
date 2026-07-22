using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200009D RID: 157
	internal class ScrollAreaSizeEditor : BaseEditor
	{
		// Token: 0x06000554 RID: 1364 RVA: 0x000176A8 File Offset: 0x000158A8
		protected override Widget OnCreateWidget()
		{
			this.xInnerEntry = new NoUndoNumEntry();
			this.xInnerEntry.IsInteger = true;
			this.xInnerEntry.DecimalPlaces = 0;
			EntryShell widget = EntryShellBuilder.CreateShell("W", this.xInnerEntry, LanguageInfo.NewFile_Pixel);
			this.yInnerEntry = new NoUndoNumEntry();
			this.yInnerEntry.IsInteger = true;
			this.yInnerEntry.DecimalPlaces = 0;
			EntryShell widget2 = EntryShellBuilder.CreateShell("H", this.yInnerEntry, LanguageInfo.NewFile_Pixel);
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

		// Token: 0x06000555 RID: 1365 RVA: 0x00017790 File Offset: 0x00015990
		protected override void OnSetControl()
		{
			SizeValue sizeValue = (SizeValue)base.PropertyItem.Values[0];
			if (PropertyItem.Objects.Count > 1)
			{
				Func<ScrollViewObject, ScrollViewObject, bool> func = (ScrollViewObject a, ScrollViewObject b) => a.InnerNodeSize.Width == b.InnerNodeSize.Width;
				Func<ScrollViewObject, ScrollViewObject, bool> func2 = (ScrollViewObject a, ScrollViewObject b) => a.InnerNodeSize.Height == b.InnerNodeSize.Height;
				if (base.IsWhipNode<ScrollViewObject>(func))
				{
					this.xInnerEntry.SetToSubState();
				}
				else
				{
					this.xInnerEntry.Value = (float)sizeValue.Width;
				}
				if (base.IsWhipNode<ScrollViewObject>(func2))
				{
					this.yInnerEntry.SetToSubState();
				}
				else
				{
					this.yInnerEntry.Value = (float)sizeValue.Height;
				}
			}
			else
			{
				this.xInnerEntry.Value = (float)sizeValue.Width;
				this.yInnerEntry.Value = (float)sizeValue.Height;
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00017894 File Offset: 0x00015A94
		private void XEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					SizeValue sizeValue = base.PropertyItem.Values[i] as SizeValue;
					sizeValue.Width = (int)e.Value;
					base.PropertyItem.Values[i] = sizeValue;
					SizeValue sizeValue2 = (SizeValue)base.PropertyItem.Values[i];
					if (sizeValue != sizeValue2)
					{
						this.xInnerEntry.Value = (float)sizeValue2.Width;
					}
				}
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00017960 File Offset: 0x00015B60
		private void YEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					SizeValue sizeValue = base.PropertyItem.Values[i] as SizeValue;
					sizeValue.Height = (int)e.Value;
					base.PropertyItem.Values[i] = sizeValue;
					SizeValue sizeValue2 = (SizeValue)base.PropertyItem.Values[i];
					if (sizeValue != sizeValue2)
					{
						this.yInnerEntry.Value = (float)sizeValue2.Height;
					}
				}
			}
		}

		// Token: 0x0400027D RID: 637
		private NoUndoNumEntry xInnerEntry;

		// Token: 0x0400027E RID: 638
		private NoUndoNumEntry yInnerEntry;
	}
}
