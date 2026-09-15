using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	public class AnimateListEditor : BaseEditor
	{
		public override bool IsMultiLine
		{
			get
			{
				return true;
			}
		}

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

		private void singleTextBox_EntryValueChanged(object sender, EntryIntEventArgs e)
		{
			this.actionValue = new InnerActionValue(this.actionValue.ActionType, this.actionValue.AnimationNames, this.actionValue.ActivedAnimationName, (int)this.singleTextBox.Value);
			base.UpdatePropertyValue(this.actionValue, null);
		}

		private void animationCombox_Changed(object sender, EventArgs e)
		{
			this.actionValue = new InnerActionValue(this.actionValue.ActionType, this.actionValue.AnimationNames, this.animationCombox.ActiveText, this.actionValue.SingleFrameIndex);
			base.UpdatePropertyValue(this.actionValue, null);
		}

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

		private Table table;

		private Table firTable;

		private Label firLabel;

		private ComBoxEx typeCombox;

		private Alignment firAlign;

		private Table secTable;

		private Label secLabel;

		private EntryIntEx singleTextBox;

		private Alignment secAlign;

		private Table thiTable;

		private Label thiLabel;

		private ComBoxEx animationCombox;

		private Alignment thiAlign;

		private InnerActionValue actionValue;
	}
}
