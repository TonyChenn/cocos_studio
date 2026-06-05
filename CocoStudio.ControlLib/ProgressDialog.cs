using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using GLib;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using MonoDevelop.Core;
using Pango;
using Stetic;

namespace CocoStudio.ControlLib
{
	// Token: 0x02000008 RID: 8
	public class ProgressDialog : Dialog
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600001C RID: 28 RVA: 0x00002848 File Offset: 0x00000A48
		// (remove) Token: 0x0600001D RID: 29 RVA: 0x00002884 File Offset: 0x00000A84
		public event EventHandler OperationCancelled;

		// Token: 0x0600001E RID: 30 RVA: 0x000028C0 File Offset: 0x00000AC0
		public ProgressDialog(bool allowCancel, bool showDetails) : this(null, allowCancel, showDetails)
		{
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000028D0 File Offset: 0x00000AD0
		public ProgressDialog(Window parent, bool allowCancel, bool showDetails)
		{
			this.Build();
			base.Resizable = false;
			base.TransientFor = parent;
			base.WidthRequest = 540;
			this.GtkScrolledWindow.HeightRequest = 300;
			base.HasSeparator = false;
			base.ActionArea.Hide();
			this.btnCancel.Visible = allowCancel;
			this.btnCancel.WidthRequest = 45;
			this.btnClose.WidthRequest = 45;
			this.expander.Visible = showDetails;
			this.detailsTextView.WrapMode = Gtk.WrapMode.Word;
			this.buffer = this.detailsTextView.Buffer;
			this.detailsTextView.Editable = false;
			this.bold = new TextTag("bold");
			this.bold.Weight = Weight.Bold;
			this.buffer.TagTable.Add(this.bold);
			this.tag = new TextTag("0");
			this.tag.Indent = 10;
			this.buffer.TagTable.Add(this.tag);
			this.tags.Add(this.tag);
			this.SetMultiLanguageInfo();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002A34 File Offset: 0x00000C34
		private void SetMultiLanguageInfo()
		{
			base.Title = Option.CurrentApp.ToString();
			base.Title = LanguageInfo.Menu_File_Import;
			this.btnClose.Label = LanguageInfo.Dialog_ButtonClose;
			this.btnCancel.Label = LanguageInfo.Dialog_ButtonCancel;
			this.expanderLabel.Text = LanguageInfo.Dialog_New_ShowOuput;
			this.label.Text = LanguageInfo.Dialog_Import_Importing;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002AA8 File Offset: 0x00000CA8
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002AC0 File Offset: 0x00000CC0
		public IAsyncOperation AsyncOperation
		{
			get
			{
				return this.asyncOperation;
			}
			set
			{
				this.asyncOperation = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002ACC File Offset: 0x00000CCC
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002AE9 File Offset: 0x00000CE9
		public string Message
		{
			get
			{
				return this.label.Text;
			}
			set
			{
				this.label.Text = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002AFC File Offset: 0x00000CFC
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002B19 File Offset: 0x00000D19
		public double Progress
		{
			get
			{
				return this.progressBar.Fraction;
			}
			set
			{
				this.progressBar.Fraction = value;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002BC4 File Offset: 0x00000DC4
		public void BeginTask(string name)
		{
			if (name != null && name.Length > 0)
			{
				this.Indent();
				this.indents.Push(name);
				this.Message = name;
			}
			else
			{
				this.indents.Push(null);
			}
			if (name != null)
			{
				GLib.Timeout.Add(0U, delegate
				{
					TextIter endIter = this.buffer.EndIter;
					string text = name + "\n";
					this.buffer.InsertWithTags(ref endIter, text, new TextTag[]
					{
						this.tag,
						this.bold
					});
					this.detailsTextView.ScrollMarkOnscreen(this.buffer.InsertMark);
					return false;
				});
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002C6C File Offset: 0x00000E6C
		public void EndTask()
		{
			if (this.indents.Count > 0)
			{
				string text = this.indents.Pop();
				if (text != null)
				{
					this.Unindent();
					this.Message = text;
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002D20 File Offset: 0x00000F20
		public void WriteText(string text)
		{
			GLib.Timeout.Add(0U, delegate
			{
				this.AddText(text);
				if (text.EndsWith("\n"))
				{
					this.detailsTextView.ScrollMarkOnscreen(this.buffer.InsertMark);
				}
				return false;
			});
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002D58 File Offset: 0x00000F58
		private void AddText(string s)
		{
			TextIter endIter = this.buffer.EndIter;
			this.buffer.InsertWithTags(ref endIter, s, new TextTag[]
			{
				this.tag
			});
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002D94 File Offset: 0x00000F94
		private void Indent()
		{
			this.ident++;
			if (this.ident >= this.tags.Count)
			{
				this.tag = new TextTag(this.ident.ToString());
				this.tag.Indent = 10 + 15 * (this.ident - 1);
				this.buffer.TagTable.Add(this.tag);
				this.tags.Add(this.tag);
			}
			else
			{
				this.tag = this.tags[this.ident];
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002E40 File Offset: 0x00001040
		private void Unindent()
		{
			if (this.ident >= 0)
			{
				this.ident--;
				this.tag = this.tags[this.ident];
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002F3C File Offset: 0x0000113C
		public void ShowDone(bool warnings, bool errors)
		{
			GLib.Timeout.Add(0U, delegate
			{
				this.progressBar.Fraction = 1.0;
				this.btnCancel.Hide();
				this.btnClose.Show();
				if (errors)
				{
					this.label.Text = LanguageInfo.Dialog_Import_Failed;
				}
				else if (warnings)
				{
					this.label.Text = LanguageInfo.Dialog_Import_Failed;
				}
				else
				{
					this.label.Text = LanguageInfo.Dialog_Import_Success;
				}
				return false;
			});
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002F7C File Offset: 0x0000117C
		protected void OnBtnCancelClicked(object sender, EventArgs e)
		{
			if (this.asyncOperation != null)
			{
				this.asyncOperation.Cancel();
			}
			if (this.OperationCancelled != null)
			{
				this.OperationCancelled(this, null);
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002FBE File Offset: 0x000011BE
		protected virtual void OnBtnCloseClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002FC8 File Offset: 0x000011C8
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "MonoDevelop.Ide.Gui.Dialogs.ProgressDialog";
			base.Title = "";
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.Modal = true;
			VBox vbox = base.VBox;
			vbox.Name = "dialog1_VBox";
			vbox.BorderWidth = 2U;
			this.vbox2 = new VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			this.vbox2.BorderWidth = 12U;
			this.label = new Label();
			this.label.Name = "label";
			this.label.Xalign = 0f;
			this.label.LabelProp = "label";
			this.vbox2.Add(this.label);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox2[this.label];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.hbox1 = new HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			this.progressBar = new ProgressBar();
			this.progressBar.Name = "progressBar";
			this.hbox1.Add(this.progressBar);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox1[this.progressBar];
			boxChild2.Position = 0;
			this.btnCancel = new Button();
			this.btnCancel.CanDefault = true;
			this.btnCancel.CanFocus = true;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseStock = true;
			this.btnCancel.UseUnderline = true;
			this.btnCancel.Label = "gtk-cancel";
			this.hbox1.Add(this.btnCancel);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox1[this.btnCancel];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.btnClose = new Button();
			this.btnClose.CanDefault = true;
			this.btnClose.CanFocus = true;
			this.btnClose.Name = "btnClose";
			this.btnClose.UseStock = true;
			this.btnClose.UseUnderline = true;
			this.btnClose.Label = "gtk-close";
			this.hbox1.Add(this.btnClose);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox1[this.btnClose];
			boxChild4.Position = 2;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			this.vbox2.Add(this.hbox1);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.vbox2[this.hbox1];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.expander = new Expander(null);
			this.expander.CanFocus = true;
			this.expander.Name = "expander";
			this.GtkScrolledWindow = new ScrolledWindow();
			this.GtkScrolledWindow.HeightRequest = 250;
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ShadowType.In;
			this.detailsTextView = new TextView();
			this.detailsTextView.CanFocus = true;
			this.detailsTextView.Name = "detailsTextView";
			this.GtkScrolledWindow.Add(this.detailsTextView);
			this.expander.Add(this.GtkScrolledWindow);
			this.expanderLabel = new Label();
			this.expanderLabel.Name = "expanderLabel";
			this.expanderLabel.LabelProp = Catalog.GetString("Details");
			this.expanderLabel.UseUnderline = true;
			this.expander.LabelWidget = this.expanderLabel;
			this.vbox2.Add(this.expander);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.vbox2[this.expander];
			boxChild6.Position = 2;
			vbox.Add(this.vbox2);
			Box.BoxChild boxChild7 = (Box.BoxChild)vbox[this.vbox2];
			boxChild7.Position = 0;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5U;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			this.button103 = new Button();
			this.button103.CanFocus = true;
			this.button103.Name = "button103";
			this.button103.UseUnderline = true;
			this.button103.Label = Catalog.GetString("GtkButton");
			base.AddActionWidget(this.button103, 0);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[this.button103];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 544;
			base.DefaultHeight = 170;
			this.btnClose.Hide();
			actionArea.Hide();
			base.Hide();
			this.btnCancel.Clicked += this.OnBtnCancelClicked;
			this.btnClose.Clicked += this.OnBtnCloseClicked;
		}

		// Token: 0x04000011 RID: 17
		private TextBuffer buffer;

		// Token: 0x04000012 RID: 18
		private TextTag tag;

		// Token: 0x04000013 RID: 19
		private TextTag bold;

		// Token: 0x04000014 RID: 20
		private int ident = 0;

		// Token: 0x04000015 RID: 21
		private List<TextTag> tags = new List<TextTag>();

		// Token: 0x04000016 RID: 22
		private Stack<string> indents = new Stack<string>();

		// Token: 0x04000017 RID: 23
		private IAsyncOperation asyncOperation;

		// Token: 0x04000019 RID: 25
		private static object SnyObject = new object();

		// Token: 0x0400001A RID: 26
		private VBox vbox2;

		// Token: 0x0400001B RID: 27
		private Label label;

		// Token: 0x0400001C RID: 28
		private HBox hbox1;

		// Token: 0x0400001D RID: 29
		private ProgressBar progressBar;

		// Token: 0x0400001E RID: 30
		private Button btnCancel;

		// Token: 0x0400001F RID: 31
		private Button btnClose;

		// Token: 0x04000020 RID: 32
		private Expander expander;

		// Token: 0x04000021 RID: 33
		private ScrolledWindow GtkScrolledWindow;

		// Token: 0x04000022 RID: 34
		private TextView detailsTextView;

		// Token: 0x04000023 RID: 35
		private Label expanderLabel;

		// Token: 0x04000024 RID: 36
		private Button button103;
	}
}
