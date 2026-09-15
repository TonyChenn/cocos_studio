using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class RotationEditor : BaseEditor
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
			this.innerEntry = new NoUndoNumEntry();
			this.innerEntry.DecimalPlaces = 2;
			base.SetControl();
			this.innerEntry.EntryValueChanged += this.EntryValueChangedHandler;
			FullEntryShell fullEntryShell = EntryShellBuilder.CreateShell(this.innerEntry, LanguageInfo.Property_Angle);
			fullEntryShell.ShowAll();
			return fullEntryShell;
		}

		protected override void OnSetControl()
		{
			VisualObject visualObject = PropertyItem.FirstObject as VisualObject;
			float rotation = visualObject.Rotation;
			if (PropertyItem.Objects.Count > 1)
			{
				Func<VisualObject, VisualObject, bool> func = (VisualObject a, VisualObject b) => a.Rotation == b.Rotation;
				if (base.IsWhipNode<VisualObject>(func))
				{
					this.innerEntry.SetToSubState();
				}
				else
				{
					this.innerEntry.Value = visualObject.Rotation;
				}
			}
			else
			{
				this.innerEntry.Value = Convert.ToSingle(rotation);
			}
		}

		private void EntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			base.UpdatePropertyValue(e.Value, null);
		}

		private NoUndoNumEntry innerEntry;
	}
}
