using System;
using System.ComponentModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class ShadowOffsetEditor : BaseEditor
	{
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

		protected override void OnSetControl()
		{
			ILabelEffect labelEffect = PropertyItem.Objects[0] as ILabelEffect;
			this.xInnerEntry.Value = labelEffect.ShadowOffsetX;
			this.yInnerEntry.Value = labelEffect.ShadowOffsetY;
		}

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

		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "ShadowOffsetX" || e.PropertyName == "ShadowOffsetY")
			{
				base.SetControl();
			}
		}

		private NoUndoNumEntry xInnerEntry;

		private NoUndoNumEntry yInnerEntry;
	}
}
