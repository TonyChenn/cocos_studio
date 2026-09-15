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
	internal class CallBackPropertyEditor : BaseEditor
	{
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

		private void entry_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return && this.entry.IsFocus)
			{
				this.SetData();
			}
		}

		private void entry_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.SetData();
		}

		private void SetData()
		{
			this.entry.Text = this.entry.Text.Trim(new char[]
			{
				' '
			});
			base.UpdatePropertyValue(this.entry.Text, this.callBackProperty);
			base.ReportUserData("CallBack");
		}

		private void combox_Changed(object sender, EventArgs e)
		{
			this.entryShell.Sensitive = (this.combox.Active > 0);
			base.UpdatePropertyValue((EnumCallBack)(this.combox.Active - 1), null);
			base.ReportUserData("CallBack");
		}

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

		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (e.PropertyName == base.PropertyItem.Name || e.PropertyName == "CallBackName")
			{
				base.SetControl();
			}
		}

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

		private ComboBox combox;

		private EntryShell entryShell;

		private EntryCallBackEx entry;

		private PropertyInfo callBackProperty;
	}
}
