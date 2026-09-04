using System;
using Gtk;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000011 RID: 17
	internal class DefaultEditor : BaseEditor
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003004 File Offset: 0x00001204
		// (set) Token: 0x06000071 RID: 113 RVA: 0x0000301B File Offset: 0x0000121B
		public TextEntry Entry { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003024 File Offset: 0x00001224
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003038 File Offset: 0x00001238
		protected override Widget OnCreateWidget()
		{
			this.Entry = new TextEntry();
			this.Entry.ValueSet += this.EntryValueSetHandler;
			base.SetControl();
			EntryShell entryShell = new EntryShell(this.Entry);
			entryShell.ShowAll();
			return entryShell;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000308C File Offset: 0x0000128C
		protected override void OnSetControl()
		{
			if (!base.CheckIsSameValue())
			{
				this.Entry.SetToSubState();
			}
			else
			{
				object firstValue = base.PropertyItem.FirstValue;
				if (firstValue != null)
				{
					this.Entry.Text = firstValue.ToString();
				}
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000030DC File Offset: 0x000012DC
		private void EntryValueSetHandler(object sender, EventArgs e)
		{
			base.UpdatePropertyValue(this.Entry.Text, null);
			if (base.PropertyItem.Name == "UserData" || base.PropertyItem.Name == "FrameEvent")
			{
				base.ReportUserData(base.PropertyItem.Name);
			}
		}
	}
}
