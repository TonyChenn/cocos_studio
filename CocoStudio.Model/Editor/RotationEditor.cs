using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200006E RID: 110
	internal class RotationEditor : BaseEditor
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x000125EC File Offset: 0x000107EC
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00012600 File Offset: 0x00010800
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

		// Token: 0x060003CB RID: 971 RVA: 0x00012664 File Offset: 0x00010864
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

		// Token: 0x060003CC RID: 972 RVA: 0x000126FF File Offset: 0x000108FF
		private void EntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			base.UpdatePropertyValue(e.Value, null);
		}

		// Token: 0x040001F1 RID: 497
		private NoUndoNumEntry innerEntry;
	}
}
