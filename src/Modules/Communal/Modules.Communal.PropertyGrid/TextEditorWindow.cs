using System;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using CocoStudio.UndoManager;
using Gdk;
using Gtk;
using Gtk.Controls;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000021 RID: 33
	public class TextEditorWindow : Gtk.Window
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x000055D0 File Offset: 0x000037D0
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x000055AC File Offset: 0x000037AC
		public object TriggerObject
		{
			get
			{
				return this.triggerObject;
			}
			set
			{
				if (this.triggerObject != value)
				{
					this.triggerObject = value;
				}
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000562C File Offset: 0x0000382C
		public TextEditorWindow(object triggerobject, string textproperty = null, string color = null, string fontsize = null, bool canlinefeed = false) : base(Gtk.WindowType.Toplevel)
		{
			base.TypeHint = WindowTypeHint.Dialog;
			base.Resizable = false;
			base.Modal = true;
			this.TriggerObject = triggerobject;
			base.TransientFor = ApplicationCurrent.MainWindow;
			this.textProperty = textproperty;
			this.colorType = color;
			this.fontSizeType = fontsize;
			this.canLineFeed = canlinefeed;
			this.Build();
			this.InitEditor();
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000056B4 File Offset: 0x000038B4
		private void InitEditor()
		{
			this.buttonOk.Label = LanguageInfo.Dialog_ButtonOK;
			this.buttonOk.Name = "MainButton";
			this.buttonCancel.Label = LanguageInfo.Dialog_ButtonCancel;
			this.combox = new ComboBoxEntry();
			this.combox.WidthRequest = 60;
			this.hbox1.Add(this.combox);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox1[this.combox];
			this.colorButtonGtk = new ColorEx();
			this.colorButtonGtk.WidthRequest = 105;
			this.hbox1.Add(this.colorButtonGtk);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox1[this.colorButtonGtk];
			this.textEntry = new Entry();
			this.textEntry.WidthRequest = 278;
			this.vbox2.Add(this.textEntry);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox2[this.textEntry];
			boxChild3.Position = 0;
			if (Platform.IsMac)
			{
				Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox1[this.buttonOk];
				Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox1[this.buttonCancel];
				boxChild.Position = 1;
				boxChild2.Position = 2;
				boxChild4.Position = 3;
				boxChild5.Position = 4;
			}
			this.fontSizeSpinButton.SetRange(5.0, 100.0);
			base.ShowAll();
			this.fontSizeSpinButton.Hide();
			this.FontSizeComboxInit();
			this.InitText();
			this.InitEvent();
			this.textview.AcceptsTab = false;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000587C File Offset: 0x00003A7C
		private void FontSizeComboxInit()
		{
			if (this.fontSizeType == null)
			{
				this.combox.Hide();
			}
			else
			{
				this.combox.Entry.FocusOutEvent += this.Entry_FocusOutEvent;
				this.combox.Entry.MaxLength = 3;
				this.combox.Changed += this.combox_Changed;
				this.fontsizeproperty = this.TriggerObject.GetType().GetProperty(this.fontSizeType);
				this.fontsize = (int)this.fontsizeproperty.GetValue(this.TriggerObject, null);
				ListStore listStore = new ListStore(new Type[]
				{
					typeof(string)
				});
				foreach (int num in this.comboxList)
				{
					listStore.AppendValues(new object[]
					{
						num.ToString()
					});
				}
				this.combox.Model = listStore;
				CellRendererText cell = new CellRendererText();
				this.combox.PackStart(cell, true);
				this.combox.AddAttribute(cell, "text", 0);
				int num2 = this.IndexCombox(this.fontsize);
				if (num2 == -1)
				{
					this.combox.Entry.Text = this.fontsize.ToString();
				}
				else
				{
					this.combox.Active = num2;
					this.combox.Entry.Text = this.comboxList[num2].ToString();
				}
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005A30 File Offset: 0x00003C30
		private void combox_Changed(object sender, EventArgs e)
		{
			ComboBoxEntry comboBoxEntry = sender as ComboBoxEntry;
			if (comboBoxEntry.Active != -1)
			{
				comboBoxEntry.Entry.Text = this.comboxList[comboBoxEntry.Active].ToString();
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005A74 File Offset: 0x00003C74
		private void Entry_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			string text = this.combox.Entry.Text;
			if (!Regex.IsMatch(text, "^[0-9]+$"))
			{
				this.combox.Entry.Text = this.fontsize.ToString();
			}
			else
			{
				int num = Convert.ToInt32(text);
				if (num < 5)
				{
					this.combox.Entry.Text = "5";
				}
				else if (num > 100)
				{
					this.combox.Entry.Text = "100";
				}
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005B10 File Offset: 0x00003D10
		private int IndexCombox(int num)
		{
			for (int i = 0; i < this.comboxList.Length; i++)
			{
				if (this.comboxList[i] == num)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005B54 File Offset: 0x00003D54
		private void InitEvent()
		{
			this.buttonOk.Clicked += this.OnbuttonOk_Click;
			this.buttonCancel.Clicked += this.OnbuttonCancel_Click;
			base.KeyReleaseEvent += this.TextEditorWindow_KeyReleaseEvent;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00005BA8 File Offset: 0x00003DA8
		private void TextEditorWindow_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Return && !this.canLineFeed)
			{
				this.SaveAll();
				this.Destroy();
			}
			else if (args.Event.Key == Gdk.Key.Escape)
			{
				this.Destroy();
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005C0C File Offset: 0x00003E0C
		private void InitText()
		{
			this.textproperty = this.TriggerObject.GetType().GetProperty(this.textProperty);
			string text = (string)this.textproperty.GetValue(this.TriggerObject, null);
			if (this.canLineFeed)
			{
				this.textEntry.Hide();
				this.textview.Buffer.Text = text;
				this.textview.Buffer.SelectRange(this.textview.Buffer.StartIter, this.textview.Buffer.EndIter);
			}
			else
			{
				this.GtkScrolledWindow.Hide();
				this.textEntry.Text = text;
			}
			if (this.colorType == null)
			{
				this.colorButtonGtk.Hide();
			}
			else
			{
				this.colorproperty = this.TriggerObject.GetType().GetProperty(this.colorType);
				System.Drawing.Color color = (System.Drawing.Color)this.colorproperty.GetValue(this.TriggerObject, null);
				this.colorButtonGtk.ColorButton.ColorValue = color;
				this.colorButtonGtk.Color = color;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005D3E File Offset: 0x00003F3E
		protected void OnbuttonOk_Click(object sender, EventArgs e)
		{
			this.SaveAll();
			this.Destroy();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005D4F File Offset: 0x00003F4F
		protected void OnbuttonCancel_Click(object sender, EventArgs e)
		{
			this.Destroy();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005D5C File Offset: 0x00003F5C
		private void SaveAll()
		{
			using (CompositeTask.Run("编辑文本", null))
			{
				if (this.canLineFeed)
				{
					this.textproperty.SetValue(this.TriggerObject, this.textview.Buffer.Text, null);
				}
				else
				{
					this.textproperty.SetValue(this.TriggerObject, this.textEntry.Text, null);
				}
				if (this.fontSizeType != null)
				{
					int num = -1;
					int.TryParse(this.combox.Entry.Text, out num);
					if (num > 0)
					{
						if (num < 5)
						{
							num = 5;
						}
						if (num > 100)
						{
							num = 100;
						}
						this.fontsizeproperty.SetValue(this.TriggerObject, num, null);
					}
				}
				if (this.colorType != null)
				{
					System.Drawing.Color colorValue = this.colorButtonGtk.ColorButton.ColorValue;
					this.colorproperty.SetValue(this.TriggerObject, colorValue, null);
				}
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005E94 File Offset: 0x00004094
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Modules.Communal.PropertyGrid.TextEditorWindow";
			base.Title = "";
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.DefaultWidth = 300;
			this.vbox2 = new VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			this.vbox2.BorderWidth = 10U;
			this.GtkScrolledWindow = new ScrolledWindow();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ShadowType.In;
			this.textview = new TextView();
			this.textview.WidthRequest = 278;
			this.textview.HeightRequest = 60;
			this.textview.CanFocus = true;
			this.textview.Name = "textview";
			this.textview.LeftMargin = 6;
			this.textview.RightMargin = 6;
			this.GtkScrolledWindow.Add(this.textview);
			this.vbox2.Add(this.GtkScrolledWindow);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox2[this.GtkScrolledWindow];
			boxChild.Position = 0;
			this.hbox1 = new HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			this.fontSizeSpinButton = new SpinButton(0.0, 100.0, 1.0);
			this.fontSizeSpinButton.WidthRequest = 50;
			this.fontSizeSpinButton.CanFocus = true;
			this.fontSizeSpinButton.Name = "fontSizeSpinButton";
			this.fontSizeSpinButton.Adjustment.PageIncrement = 10.0;
			this.fontSizeSpinButton.ClimbRate = 1.0;
			this.fontSizeSpinButton.Numeric = true;
			this.hbox1.Add(this.fontSizeSpinButton);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox1[this.fontSizeSpinButton];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			this.buttonCancel = new Button();
			this.buttonCancel.WidthRequest = 55;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = Catalog.GetString("Cancel");
			this.hbox1.Add(this.buttonCancel);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox1[this.buttonCancel];
			boxChild3.PackType = PackType.End;
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.buttonOk = new Button();
			this.buttonOk.WidthRequest = 55;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = Catalog.GetString("Ok");
			this.hbox1.Add(this.buttonOk);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox1[this.buttonOk];
			boxChild4.PackType = PackType.End;
			boxChild4.Position = 2;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.vbox2.Add(this.hbox1);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox2[this.hbox1];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			base.Add(this.vbox2);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultHeight = 300;
			base.Show();
		}

		// Token: 0x0400004B RID: 75
		private object triggerObject;

		// Token: 0x0400004C RID: 76
		private string textProperty;

		// Token: 0x0400004D RID: 77
		private string colorType;

		// Token: 0x0400004E RID: 78
		private string fontSizeType;

		// Token: 0x0400004F RID: 79
		private ColorEx colorButtonGtk;

		// Token: 0x04000050 RID: 80
		private ComboBoxEntry combox;

		// Token: 0x04000051 RID: 81
		private Entry textEntry;

		// Token: 0x04000052 RID: 82
		private bool canLineFeed;

		// Token: 0x04000053 RID: 83
		private PropertyInfo textproperty;

		// Token: 0x04000054 RID: 84
		private PropertyInfo fontsizeproperty;

		// Token: 0x04000055 RID: 85
		private PropertyInfo colorproperty;

		// Token: 0x04000056 RID: 86
		private int[] comboxList = new int[]
		{
			6,
			7,
			8,
			9,
			10,
			11,
			12,
			13,
			14,
			16,
			18,
			20,
			22,
			24,
			36,
			48,
			72
		};

		// Token: 0x04000057 RID: 87
		private int fontsize;

		// Token: 0x04000058 RID: 88
		private VBox vbox2;

		// Token: 0x04000059 RID: 89
		private ScrolledWindow GtkScrolledWindow;

		// Token: 0x0400005A RID: 90
		private TextView textview;

		// Token: 0x0400005B RID: 91
		private HBox hbox1;

		// Token: 0x0400005C RID: 92
		private SpinButton fontSizeSpinButton;

		// Token: 0x0400005D RID: 93
		private Button buttonCancel;

		// Token: 0x0400005E RID: 94
		private Button buttonOk;
	}
}
