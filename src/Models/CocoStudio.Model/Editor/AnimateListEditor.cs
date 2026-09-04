using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200004E RID: 78
	public class AnimateListEditor : BaseEditor
	{
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00008660 File Offset: 0x00006860
		public override bool IsMultiLine
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00008674 File Offset: 0x00006874
		protected override Widget OnCreateWidget()
		{
			this.table = new Table(3U, 3U, false);
			this.firTable = new Table(1U, 2U, false);
			this.secTable = new Table(1U, 2U, false);
			this.thiTable = new Table(1U, 2U, false);
			this.firAlign = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.secAlign = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.thiAlign = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.firLabel = new Label();
			this.firLabel.Text = LanguageInfo.Animation_Type;
			this.secLabel = new Label();
			this.secLabel.Text = LanguageInfo.UIAnimation_Text_Frame;
			this.thiLabel = new Label();
			this.thiLabel.Text = LanguageInfo.Display_Name;
			this.typeCombox = new ComBoxEx();
			this.singleTextBox = new EntryIntEx();
			this.singleTextBox.SetEntryProperty(true, 0, 1f);
			this.animationCombox = new ComBoxEx();
			this.firTable.Attach(this.typeCombox, 0U, 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.firTable.ShowAll();
			this.secTable.Attach(this.singleTextBox, 0U, 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.secTable.ShowAll();
			this.thiTable.Attach(this.animationCombox, 0U, 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.thiTable.ShowAll();
			this.firAlign.Add(this.firTable);
			this.firAlign.ShowAll();
			this.table.Attach(this.firAlign, 1U, 2U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.table.Attach(this.firLabel, 0U, 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.secAlign.TopPadding = 6U;
			this.secAlign.Add(this.secTable);
			this.secAlign.ShowAll();
			this.table.Attach(this.secAlign, 1U, 2U, 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.table.Attach(this.secLabel, 0U, 1U, 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.thiAlign.TopPadding = 6U;
			this.thiAlign.Add(this.thiTable);
			this.thiAlign.ShowAll();
			this.table.Attach(this.thiAlign, 1U, 2U, 2U, 3U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.table.Attach(this.thiLabel, 0U, 1U, 2U, 3U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.table.ColumnSpacing = 6U;
			this.firLabel.Xalign = (this.secLabel.Xalign = (this.thiLabel.Xalign = 1f));
			this.typeCombox.WidthRequest = (this.singleTextBox.WidthRequest = (this.animationCombox.WidthRequest = 120));
			this.table.ShowAll();
			this.InitActionType();
			base.SetControl();
			this.typeCombox.Changed += this.typeCombox_Changed;
			this.animationCombox.Changed += this.animationCombox_Changed;
			this.singleTextBox.EntryValueChanged += this.singleTextBox_EntryValueChanged;
			return this.table;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x000089F8 File Offset: 0x00006BF8
		private void singleTextBox_EntryValueChanged(object sender, EntryIntEventArgs e)
		{
			this.actionValue = new InnerActionValue(this.actionValue.ActionType, this.actionValue.AnimationNames, this.actionValue.ActivedAnimationName, (int)this.singleTextBox.Value);
			base.UpdatePropertyValue(this.actionValue, null);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00008A4C File Offset: 0x00006C4C
		private void animationCombox_Changed(object sender, EventArgs e)
		{
			this.actionValue = new InnerActionValue(this.actionValue.ActionType, this.actionValue.AnimationNames, this.animationCombox.ActiveText, this.actionValue.SingleFrameIndex);
			base.UpdatePropertyValue(this.actionValue, null);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00008AA0 File Offset: 0x00006CA0
		private void typeCombox_Changed(object sender, EventArgs e)
		{
			if (this.typeCombox.Active == 2)
			{
				this.secLabel.Visible = (this.secAlign.Visible = true);
				this.thiLabel.Visible = (this.thiAlign.Visible = false);
			}
			else
			{
				this.secLabel.Visible = (this.secAlign.Visible = false);
				this.thiLabel.Visible = (this.thiAlign.Visible = true);
			}
			if (this.actionValue != null)
			{
				this.actionValue = new InnerActionValue((InnerActionType)this.typeCombox.Active, this.actionValue.AnimationNames, this.actionValue.ActivedAnimationName, this.actionValue.SingleFrameIndex);
			}
			else
			{
				string text = "-- ALL --";
				List<string> animationNames = new List<string>
				{
					text
				};
				this.actionValue = new InnerActionValue((InnerActionType)this.typeCombox.Active, animationNames, text, 0);
			}
			base.UpdatePropertyValue(this.actionValue, null);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00008BC8 File Offset: 0x00006DC8
		protected override void OnSetControl()
		{
			this.actionValue = (InnerActionValue)base.PropertyItem.Values[0];
			if (this.actionValue != null)
			{
				this.typeCombox.Active = (int)this.actionValue.ActionType;
				this.InitAnimationNames();
				this.singleTextBox.Value = (float)this.actionValue.SingleFrameIndex;
				if (this.typeCombox.Active == 2)
				{
					this.secLabel.Visible = (this.secAlign.Visible = true);
					this.thiLabel.Visible = (this.thiAlign.Visible = false);
				}
				else
				{
					this.secLabel.Visible = (this.secAlign.Visible = false);
					this.thiLabel.Visible = (this.thiAlign.Visible = true);
				}
				this.singleTextBox.MinValue = 0;
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00008CD8 File Offset: 0x00006ED8
		private void InitActionType()
		{
			this.typeCombox.Clear();
			ListStore listStore = new ListStore(new Type[]
			{
				typeof(string)
			});
			string[] names = Enum.GetNames(typeof(InnerActionType));
			if (names != null && names.Count<string>() > 0)
			{
				for (int i = 0; i < names.Count<string>(); i++)
				{
					listStore.AppendValues(new object[]
					{
						names[i]
					});
				}
			}
			this.typeCombox.Model = listStore;
			CellRendererText cell = new CellRendererText();
			this.typeCombox.PackStart(cell, true);
			this.typeCombox.AddAttribute(cell, "text", 0);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00008DA0 File Offset: 0x00006FA0
		private void InitAnimationNames()
		{
			this.animationCombox.Clear();
			ListStore listStore = new ListStore(new Type[]
			{
				typeof(string)
			});
			string[] array = null;
			if (this.actionValue.AnimationNames != null)
			{
				array = this.actionValue.AnimationNames.ToArray<string>();
			}
			int active = -1;
			if (array != null && array.Count<string>() > 0)
			{
				for (int i = 0; i < array.Count<string>(); i++)
				{
					if (array[i] == this.actionValue.ActivedAnimationName)
					{
						active = i;
					}
					listStore.AppendValues(new object[]
					{
						array[i]
					});
				}
			}
			this.animationCombox.Model = listStore;
			CellRendererText cell = new CellRendererText();
			this.animationCombox.PackStart(cell, true);
			this.animationCombox.AddAttribute(cell, "text", 0);
			this.animationCombox.Active = active;
		}

		// Token: 0x04000115 RID: 277
		private Table table;

		// Token: 0x04000116 RID: 278
		private Table firTable;

		// Token: 0x04000117 RID: 279
		private Label firLabel;

		// Token: 0x04000118 RID: 280
		private ComBoxEx typeCombox;

		// Token: 0x04000119 RID: 281
		private Alignment firAlign;

		// Token: 0x0400011A RID: 282
		private Table secTable;

		// Token: 0x0400011B RID: 283
		private Label secLabel;

		// Token: 0x0400011C RID: 284
		private EntryIntEx singleTextBox;

		// Token: 0x0400011D RID: 285
		private Alignment secAlign;

		// Token: 0x0400011E RID: 286
		private Table thiTable;

		// Token: 0x0400011F RID: 287
		private Label thiLabel;

		// Token: 0x04000120 RID: 288
		private ComBoxEx animationCombox;

		// Token: 0x04000121 RID: 289
		private Alignment thiAlign;

		// Token: 0x04000122 RID: 290
		private InnerActionValue actionValue;
	}
}
