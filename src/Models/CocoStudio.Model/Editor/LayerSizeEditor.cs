using System;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class LayerSizeEditor : BaseEditor
	{
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

		protected override void OnSetControl()
		{
			SizeF sizeF = base.PropertyItem.FirstValue as SizeF;
			this.xInnerEntry.Value = sizeF.Width;
			this.yInnerEntry.Value = sizeF.Height;
		}

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

		private NoUndoNumEntry xInnerEntry;

		private NoUndoNumEntry yInnerEntry;
	}
}
