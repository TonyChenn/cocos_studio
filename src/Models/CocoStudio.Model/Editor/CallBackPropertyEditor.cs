using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using CocoStudio.Basic;
using CocoStudio.Model.Interface;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000050 RID: 80
	internal class CallBackPropertyEditor : BaseEditor
	{
		// Token: 0x060002AC RID: 684 RVA: 0x00009044 File Offset: 0x00007244
		protected override Widget OnCreateWidget()
		{
			this.combox = new ComboBox();
			this.combox.WidthRequest = 90;
			BorderEventBox borderEventBox = new BorderEventBox();
			borderEventBox.Add(this.combox);
			borderEventBox.ModifyBg(StateType.Normal, WindowStyle.LineDarkColor);
			borderEventBox.ModifyBg(StateType.Insensitive, WindowStyle.LineDarkColor);
			this.entry = new EntryCallBackEx();
			this.entry.HeightRequest = 23;
			this.entryShell = new EntryShell(this.entry);
			HBox hbox = new HBox();
			hbox.PackStart(borderEventBox, false, true, 0U);
			hbox.PackStart(this.entryShell, true, true, 0U);
			hbox.Spacing = 4;
			hbox.ShowAll();
			this.InitComboBox();
			base.SetControl();
			this.combox.Changed += this.combox_Changed;
			this.entry.KeyReleaseEvent += this.entry_KeyReleaseEvent;
			this.entry.FocusOutEvent += this.entry_FocusOutEvent;
			return hbox;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00009150 File Offset: 0x00007350
		private void entry_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return && this.entry.IsFocus)
			{
				this.SetData();
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000918F File Offset: 0x0000738F
		private void entry_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.SetData();
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000919C File Offset: 0x0000739C
		private void SetData()
		{
			this.entry.Text = this.entry.Text.Trim(new char[]
			{
				' '
			});
			base.UpdatePropertyValue(this.entry.Text, this.callBackProperty);
			base.ReportUserData("CallBack");
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x000091F8 File Offset: 0x000073F8
		private void combox_Changed(object sender, EventArgs e)
		{
			this.entryShell.Sensitive = (this.combox.Active > 0);
			base.UpdatePropertyValue((EnumCallBack)(this.combox.Active - 1), null);
			base.ReportUserData("CallBack");
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00009248 File Offset: 0x00007448
		protected override void OnSetControl()
		{
			EnumCallBack enumCallBack = (EnumCallBack)base.PropertyItem.Values[0];
			this.combox.Active = (int)(enumCallBack + 1);
			if (this.callBackProperty == null)
			{
				this.callBackProperty = PropertyItem.FirstObject.GetType().GetProperty("CallBackName");
			}
			object value = this.callBackProperty.GetValue(PropertyItem.FirstObject, null);
			this.entry.Text = value.ToString();
			this.entryShell.Sensitive = (this.combox.Active > 0);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000092E8 File Offset: 0x000074E8
		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (e.PropertyName == base.PropertyItem.Name || e.PropertyName == "CallBackName")
			{
				base.SetControl();
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00009330 File Offset: 0x00007530
		public void InitComboBox()
		{
			ListStore listStore = new ListStore(new Type[]
			{
				typeof(string)
			});
			string[] names = Enum.GetNames(typeof(EnumCallBack));
			listStore.AppendValues(new object[]
			{
				LanguageInfo.CallBack_None
			});
			for (int i = 1; i < names.Count<string>(); i++)
			{
				string text = ((EnumCallBack)(i - 1)).ToString();
				if (!(text == EnumCallBack.Event.ToString()) || PropertyItem.FirstObject is ICallBackEvent)
				{
					listStore.AppendValues(new object[]
					{
						text
					});
				}
			}
			this.combox.Model = listStore;
			CellRendererText cell = new CellRendererText();
			this.combox.PackStart(cell, true);
			this.combox.AddAttribute(cell, "text", 0);
		}

		// Token: 0x04000125 RID: 293
		private ComboBox combox;

		// Token: 0x04000126 RID: 294
		private EntryShell entryShell;

		// Token: 0x04000127 RID: 295
		private EntryCallBackEx entry;

		// Token: 0x04000128 RID: 296
		private PropertyInfo callBackProperty;
	}
}
