using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class SkewEditor : BaseEditor
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
