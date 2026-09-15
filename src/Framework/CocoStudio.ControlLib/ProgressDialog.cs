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
	public class ProgressDialog : Dialog
	{
		public event EventHandler OperationCancelled;

		public ProgressDialog(bool allowCancel, bool showDetails) : this(null, allowCancel, showDetails)
		{
		}

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

		private void SetMultiLanguageInfo()
		{
			base.Title = Option.CurrentApp.ToString();
			base.Title = LanguageInfo.Menu_File_Import;
			this.btnClose.Label = LanguageInfo.Dialog_ButtonClose;
			this.btnCancel.Label = LanguageInfo.Dialog_ButtonCancel;
			this.expanderLabel.Text = LanguageInfo.Dialog_New_ShowOuput;
			this.label.Text = LanguageInfo.Dialog_Import_Importing;
		}

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

		private void AddText(string s)
		{
			TextIter endIter = this.buffer.EndIter;
			this.buffer.InsertWithTags(ref endIter, s, new TextTag[]
			{
				this.tag
			});
		}

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

		private void Unindent()
		{
			if (this.ident >= 0)
			{
				this.ident--;
				this.tag = this.tags[this.ident];
			}
		}

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

		protected virtual void OnBtnCloseClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}

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

		private TextBuffer buffer;

		private TextTag tag;

		private TextTag bold;

		private int ident = 0;

		private List<TextTag> tags = new List<TextTag>();

		private Stack<string> indents = new Stack<string>();

		private IAsyncOperation asyncOperation;

		private static object SnyObject = new object();

		private VBox vbox2;

		private Label label;

		private HBox hbox1;

		private ProgressBar progressBar;

		private Button btnCancel;

		private Button btnClose;

		private Expander expander;

		private ScrolledWindow GtkScrolledWindow;

		private TextView detailsTextView;

		private Label expanderLabel;

		private Button button103;
	}
}
