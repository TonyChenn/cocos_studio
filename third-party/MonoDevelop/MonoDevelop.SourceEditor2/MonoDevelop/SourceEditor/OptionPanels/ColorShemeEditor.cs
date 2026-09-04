using System;
using System.IO;
using Cairo;
using GLib;
using Gdk;
using Gtk;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using Mono.Unix;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using Stetic;
using Xwt.Drawing;

namespace MonoDevelop.SourceEditor.OptionPanels
{
	public class ColorShemeEditor : Dialog
	{
		private TextEditor textEditor;

		private ColorScheme colorSheme;

		private TreeStore colorStore = new TreeStore(typeof(string), typeof(ColorScheme.PropertyDecsription), typeof(object));

		private string fileName;

		private HighlightingPanel panel;

		private HBox hbox1;

		private Label label2;

		private Entry entryName;

		private Label label3;

		private Entry entryDescription;

		private HPaned hpaned1;

		private VBox vbox2;

		private Label label1;

		private ScrolledWindow GtkScrolledWindow;

		private TreeView treeviewColors;

		private VBox vbox3;

		private Notebook notebookColorChooser;

		private VBox vbox4;

		private Table table1;

		private ColorButton colorbuttonBg;

		private ColorButton colorbuttonFg;

		private Label label5;

		private Label label7;

		private CheckButton checkbuttonBold;

		private CheckButton checkbuttonItalic;

		private Label label4;

		private VBox vbox5;

		private Table table2;

		private ColorButton colorbuttonBorder;

		private ColorButton colorbuttonPrimary;

		private ColorButton colorbuttonSecondary;

		private Label label10;

		private Label label11;

		private Label label9;

		private Label label6;

		private Label label8;

		private ScrolledWindow scrolledwindowTextEditor;

		private Button buttonCancel;

		private Button buttonOk;

		public ColorShemeEditor(HighlightingPanel panel)
		{
			this.panel = panel;
			Build();
			textEditor = new TextEditor();
			textEditor.Options = DefaultSourceEditorOptions.Instance;
			scrolledwindowTextEditor.Child = textEditor;
			textEditor.ShowAll();
			treeviewColors.AppendColumn(GettextCatalog.GetString("Name"), (CellRenderer)new CellRendererText(), (CellLayoutDataFunc)SyntaxCellRenderer);
			treeviewColors.HeadersVisible = false;
			treeviewColors.Model = colorStore;
			treeviewColors.Selection.Changed += HandleTreeviewColorsSelectionChanged;
			colorbuttonFg.ColorSet += Stylechanged;
			colorbuttonBg.ColorSet += Stylechanged;
			colorbuttonPrimary.ColorSet += Stylechanged;
			colorbuttonSecondary.ColorSet += Stylechanged;
			colorbuttonBorder.ColorSet += Stylechanged;
			colorbuttonBg.UseAlpha = true;
			checkbuttonBold.Toggled += Stylechanged;
			checkbuttonItalic.Toggled += Stylechanged;
			buttonOk.Clicked += HandleButtonOkClicked;
			HandleTreeviewColorsSelectionChanged(null, null);
			notebookColorChooser.ShowTabs = false;
		}

		private void SyntaxCellRenderer(CellLayout cell_layout, CellRenderer cell, TreeModel tree_model, TreeIter iter)
		{
			CellRendererText cellRendererText = (CellRendererText)cell;
			ColorScheme.PropertyDecsription propertyDecsription = (ColorScheme.PropertyDecsription)colorStore.GetValue(iter, 1);
			string markup = Markup.EscapeText(propertyDecsription.Attribute.Name);
			cellRendererText.Markup = markup;
		}

		private void ApplyStyle(ColorScheme sheme)
		{
			sheme.Name = entryName.Text;
			sheme.Description = entryDescription.Text;
			if (colorStore.GetIterFirst(out var iter))
			{
				do
				{
					ColorScheme.PropertyDecsription propertyDecsription = (ColorScheme.PropertyDecsription)colorStore.GetValue(iter, 1);
					object value = colorStore.GetValue(iter, 2);
					propertyDecsription.Info.SetValue(sheme, value, null);
				}
				while (colorStore.IterNext(ref iter));
			}
		}

		public static void RefreshAllColors()
		{
			foreach (Document document in IdeApp.Workbench.Documents)
			{
				TextEditorData editor = document.Editor;
				if (editor != null)
				{
					document.UpdateParseDocument();
					editor.Parent.TextViewMargin.PurgeLayoutCache();
					editor.Document.CommitUpdateAll();
				}
			}
		}

		private void HandleButtonOkClicked(object sender, EventArgs e)
		{
			ApplyStyle(colorSheme);
			try
			{
				if (fileName.EndsWith(".vssettings", StringComparison.Ordinal))
				{
					File.Delete(fileName);
					fileName += "Style.json";
				}
				colorSheme.Save(fileName);
				panel.ShowStyles();
			}
			catch (Exception ex)
			{
				LoggingService.LogInternalError(ex);
			}
			RefreshAllColors();
		}

		private void Stylechanged(object sender, EventArgs e)
		{
			if (treeviewColors.Selection.GetSelected(out var iter))
			{
				object value = colorStore.GetValue(iter, 2);
				if (value is ChunkStyle)
				{
					SetChunkStyle(iter, (ChunkStyle)value);
				}
				else if (value is AmbientColor)
				{
					SetAmbientColor(iter, (AmbientColor)value);
				}
			}
		}

		private Cairo.Color GetColorFromButton(ColorButton button)
		{
			return new Cairo.Color((double)(int)button.Color.Red / 65535.0, (double)(int)button.Color.Green / 65535.0, (double)(int)button.Color.Blue / 65535.0, (double)(int)button.Alpha / 65535.0);
		}

		private void SetAmbientColor(TreeIter iter, AmbientColor oldStyle)
		{
			AmbientColor ambientColor = new AmbientColor();
			ambientColor.Color = GetColorFromButton(colorbuttonPrimary);
			ambientColor.SecondColor = GetColorFromButton(colorbuttonSecondary);
			colorStore.SetValue(iter, 2, ambientColor);
			ColorScheme colorScheme = colorSheme.Clone();
			ApplyStyle(colorScheme);
			textEditor.TextViewMargin.PurgeLayoutCache();
			textEditor.Document.MimeType = "text/x-csharp";
			textEditor.GetTextEditorData().ColorStyle = colorScheme;
			textEditor.QueueDraw();
		}

		private void SetChunkStyle(TreeIter iter, ChunkStyle oldStyle)
		{
			ChunkStyle chunkStyle = new ChunkStyle(oldStyle);
			chunkStyle.Foreground = GetColorFromButton(colorbuttonFg);
			chunkStyle.Background = GetColorFromButton(colorbuttonBg);
			if (checkbuttonBold.Active)
			{
				chunkStyle.FontWeight = Xwt.Drawing.FontWeight.Bold;
			}
			else
			{
				chunkStyle.FontWeight = Xwt.Drawing.FontWeight.Normal;
			}
			if (checkbuttonItalic.Active)
			{
				chunkStyle.FontStyle = FontStyle.Italic;
			}
			else
			{
				chunkStyle.FontStyle = FontStyle.Normal;
			}
			colorStore.SetValue(iter, 2, chunkStyle);
			ColorScheme colorScheme = colorSheme.Clone();
			ApplyStyle(colorScheme);
			textEditor.TextViewMargin.PurgeLayoutCache();
			textEditor.Document.MimeType = "text/x-csharp";
			textEditor.GetTextEditorData().ColorStyle = colorScheme;
			textEditor.QueueDraw();
		}

		private void HandleTreeviewColorsSelectionChanged(object sender, EventArgs e)
		{
			colorbuttonBg.Sensitive = false;
			colorbuttonFg.Sensitive = false;
			checkbuttonBold.Sensitive = false;
			checkbuttonItalic.Sensitive = false;
			if (treeviewColors.Selection.GetSelected(out var iter))
			{
				object value = colorStore.GetValue(iter, 2);
				if (value is ChunkStyle)
				{
					SelectChunkStyle(iter, (ChunkStyle)value);
				}
				if (value is AmbientColor)
				{
					SelectAmbientColor(iter, (AmbientColor)value);
				}
			}
		}

		private void SetColorToButton(ColorButton button, Cairo.Color color)
		{
			button.Color = (HslColor)color;
			button.Alpha = (ushort)(color.A * 65535.0);
		}

		private void SelectAmbientColor(TreeIter iter, AmbientColor ambientColor)
		{
			notebookColorChooser.Page = 1;
			SetColorToButton(colorbuttonPrimary, ambientColor.Color);
			SetColorToButton(colorbuttonSecondary, ambientColor.SecondColor);
			colorbuttonSecondary.Sensitive = ambientColor.HasSecondColor;
			SetColorToButton(colorbuttonBorder, ambientColor.BorderColor);
			colorbuttonBorder.Sensitive = ambientColor.HasBorderColor;
		}

		private void SelectChunkStyle(TreeIter iter, ChunkStyle chunkStyle)
		{
			notebookColorChooser.Page = 0;
			SetColorToButton(colorbuttonFg, chunkStyle.Foreground);
			SetColorToButton(colorbuttonBg, chunkStyle.Background);
			checkbuttonBold.Active = chunkStyle.FontWeight == Xwt.Drawing.FontWeight.Bold;
			checkbuttonItalic.Active = chunkStyle.FontStyle == FontStyle.Italic;
			Label label = label4;
			bool visible = (colorbuttonFg.Visible = true);
			label.Visible = visible;
			colorbuttonFg.Sensitive = true;
			Label label2 = label5;
			bool visible2 = (colorbuttonBg.Visible = true);
			label2.Visible = visible2;
			colorbuttonBg.Sensitive = true;
			checkbuttonBold.Visible = true;
			checkbuttonBold.Sensitive = true;
			checkbuttonItalic.Visible = true;
			checkbuttonItalic.Sensitive = true;
		}

		public void SetSheme(ColorScheme style)
		{
			if (style == null)
			{
				throw new ArgumentNullException("style");
			}
			fileName = style.FileName;
			colorSheme = style;
			entryName.Text = style.Name;
			entryDescription.Text = style.Description;
			textEditor.Document.MimeType = "text/x-csharp";
			textEditor.GetTextEditorData().ColorStyle = style;
			textEditor.Text = "using System;\n\n// This is an example\nclass Example\n{\n\tpublic static void Main (string[] args)\n\t{\n\t\tConsole.WriteLine (\"Hello World\");\n\t}\n}";
			foreach (ColorScheme.PropertyDecsription textColor in ColorScheme.TextColors)
			{
				colorStore.AppendValues(textColor.Attribute.Name, textColor, textColor.Info.GetValue(style, null));
			}
			foreach (ColorScheme.PropertyDecsription ambientColor in ColorScheme.AmbientColors)
			{
				colorStore.AppendValues(ambientColor.Attribute.Name, ambientColor, ambientColor.Info.GetValue(style, null));
			}
			Stylechanged(null, null);
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			base.Name = "MonoDevelop.SourceEditor.OptionPanels.ColorShemeEditor";
			base.Title = Catalog.GetString("Edit color sheme");
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.BorderWidth = 6u;
			VBox vBox = base.VBox;
			vBox.Name = "dialog1_VBox";
			vBox.BorderWidth = 2u;
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 6;
			hbox1.BorderWidth = 6u;
			label2 = new Label();
			label2.Name = "label2";
			label2.Xalign = 0f;
			label2.LabelProp = Catalog.GetString("_Name:");
			label2.UseUnderline = true;
			hbox1.Add(label2);
			Box.BoxChild boxChild = (Box.BoxChild)hbox1[label2];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			entryName = new Entry();
			entryName.CanFocus = true;
			entryName.Name = "entryName";
			entryName.IsEditable = true;
			entryName.InvisibleChar = '●';
			hbox1.Add(entryName);
			Box.BoxChild boxChild2 = (Box.BoxChild)hbox1[entryName];
			boxChild2.Position = 1;
			label3 = new Label();
			label3.Name = "label3";
			label3.LabelProp = Catalog.GetString("_Description:");
			label3.UseUnderline = true;
			hbox1.Add(label3);
			Box.BoxChild boxChild3 = (Box.BoxChild)hbox1[label3];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			entryDescription = new Entry();
			entryDescription.CanFocus = true;
			entryDescription.Name = "entryDescription";
			entryDescription.IsEditable = true;
			entryDescription.InvisibleChar = '●';
			hbox1.Add(entryDescription);
			Box.BoxChild boxChild4 = (Box.BoxChild)hbox1[entryDescription];
			boxChild4.Position = 3;
			vBox.Add(hbox1);
			Box.BoxChild boxChild5 = (Box.BoxChild)vBox[hbox1];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			hpaned1 = new HPaned();
			hpaned1.CanFocus = true;
			hpaned1.Name = "hpaned1";
			hpaned1.Position = 415;
			vbox2 = new VBox();
			vbox2.Name = "vbox2";
			vbox2.Spacing = 6;
			label1 = new Label();
			label1.Name = "label1";
			label1.Xalign = 0f;
			label1.LabelProp = Catalog.GetString("_Colors");
			label1.UseUnderline = true;
			vbox2.Add(label1);
			Box.BoxChild boxChild6 = (Box.BoxChild)vbox2[label1];
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			GtkScrolledWindow = new ScrolledWindow();
			GtkScrolledWindow.Name = "GtkScrolledWindow";
			GtkScrolledWindow.ShadowType = ShadowType.In;
			treeviewColors = new TreeView();
			treeviewColors.CanFocus = true;
			treeviewColors.Name = "treeviewColors";
			GtkScrolledWindow.Add(treeviewColors);
			vbox2.Add(GtkScrolledWindow);
			Box.BoxChild boxChild7 = (Box.BoxChild)vbox2[GtkScrolledWindow];
			boxChild7.Position = 1;
			hpaned1.Add(vbox2);
			Paned.PanedChild panedChild = (Paned.PanedChild)hpaned1[vbox2];
			panedChild.Resize = false;
			vbox3 = new VBox();
			vbox3.Name = "vbox3";
			vbox3.Spacing = 6;
			notebookColorChooser = new Notebook();
			notebookColorChooser.CanFocus = true;
			notebookColorChooser.Name = "notebookColorChooser";
			notebookColorChooser.CurrentPage = 1;
			notebookColorChooser.ShowBorder = false;
			notebookColorChooser.BorderWidth = 8u;
			vbox4 = new VBox();
			vbox4.Name = "vbox4";
			vbox4.Spacing = 6;
			table1 = new Table(2u, 2u, homogeneous: false);
			table1.RowSpacing = 6u;
			table1.ColumnSpacing = 6u;
			colorbuttonBg = new ColorButton();
			colorbuttonBg.CanFocus = true;
			colorbuttonBg.Events = EventMask.ButtonMotionMask | EventMask.ButtonPressMask | EventMask.ButtonReleaseMask;
			colorbuttonBg.Name = "colorbuttonBg";
			table1.Add(colorbuttonBg);
			Table.TableChild tableChild = (Table.TableChild)table1[colorbuttonBg];
			tableChild.TopAttach = 1u;
			tableChild.BottomAttach = 2u;
			tableChild.LeftAttach = 1u;
			tableChild.RightAttach = 2u;
			tableChild.XOptions = AttachOptions.Fill;
			tableChild.YOptions = AttachOptions.Fill;
			colorbuttonFg = new ColorButton();
			colorbuttonFg.CanFocus = true;
			colorbuttonFg.Events = EventMask.ButtonMotionMask | EventMask.ButtonPressMask | EventMask.ButtonReleaseMask;
			colorbuttonFg.Name = "colorbuttonFg";
			table1.Add(colorbuttonFg);
			Table.TableChild tableChild2 = (Table.TableChild)table1[colorbuttonFg];
			tableChild2.LeftAttach = 1u;
			tableChild2.RightAttach = 2u;
			tableChild2.XOptions = AttachOptions.Fill;
			tableChild2.YOptions = AttachOptions.Fill;
			label5 = new Label();
			label5.Name = "label5";
			label5.Xalign = 1f;
			label5.LabelProp = Catalog.GetString("_Background:");
			label5.UseUnderline = true;
			table1.Add(label5);
			Table.TableChild tableChild3 = (Table.TableChild)table1[label5];
			tableChild3.TopAttach = 1u;
			tableChild3.BottomAttach = 2u;
			tableChild3.XOptions = AttachOptions.Fill;
			tableChild3.YOptions = AttachOptions.Fill;
			label7 = new Label();
			label7.Name = "label7";
			label7.Xalign = 1f;
			label7.LabelProp = Catalog.GetString("_Foreground:");
			label7.UseUnderline = true;
			table1.Add(label7);
			Table.TableChild tableChild4 = (Table.TableChild)table1[label7];
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			vbox4.Add(table1);
			Box.BoxChild boxChild8 = (Box.BoxChild)vbox4[table1];
			boxChild8.Position = 0;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			checkbuttonBold = new CheckButton();
			checkbuttonBold.CanFocus = true;
			checkbuttonBold.Name = "checkbuttonBold";
			checkbuttonBold.Label = Catalog.GetString("B_old");
			checkbuttonBold.DrawIndicator = true;
			checkbuttonBold.UseUnderline = true;
			vbox4.Add(checkbuttonBold);
			Box.BoxChild boxChild9 = (Box.BoxChild)vbox4[checkbuttonBold];
			boxChild9.Position = 1;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			checkbuttonItalic = new CheckButton();
			checkbuttonItalic.CanFocus = true;
			checkbuttonItalic.Name = "checkbuttonItalic";
			checkbuttonItalic.Label = Catalog.GetString("Italic");
			checkbuttonItalic.DrawIndicator = true;
			checkbuttonItalic.UseUnderline = true;
			vbox4.Add(checkbuttonItalic);
			Box.BoxChild boxChild10 = (Box.BoxChild)vbox4[checkbuttonItalic];
			boxChild10.Position = 2;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			notebookColorChooser.Add(vbox4);
			label4 = new Label();
			label4.Name = "label4";
			label4.LabelProp = Catalog.GetString("page1");
			notebookColorChooser.SetTabLabel(vbox4, label4);
			label4.ShowAll();
			vbox5 = new VBox();
			vbox5.Name = "vbox5";
			vbox5.Spacing = 6;
			table2 = new Table(3u, 2u, homogeneous: false);
			table2.Name = "table2";
			table2.RowSpacing = 6u;
			table2.ColumnSpacing = 6u;
			colorbuttonBorder = new ColorButton();
			colorbuttonBorder.CanFocus = true;
			colorbuttonBorder.Events = EventMask.ButtonMotionMask | EventMask.ButtonPressMask | EventMask.ButtonReleaseMask;
			colorbuttonBorder.Name = "colorbuttonBorder";
			table2.Add(colorbuttonBorder);
			Table.TableChild tableChild5 = (Table.TableChild)table2[colorbuttonBorder];
			tableChild5.TopAttach = 2u;
			tableChild5.BottomAttach = 3u;
			tableChild5.LeftAttach = 1u;
			tableChild5.RightAttach = 2u;
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			colorbuttonPrimary = new ColorButton();
			colorbuttonPrimary.CanFocus = true;
			colorbuttonPrimary.Events = EventMask.ButtonMotionMask | EventMask.ButtonPressMask | EventMask.ButtonReleaseMask;
			colorbuttonPrimary.Name = "colorbuttonPrimary";
			table2.Add(colorbuttonPrimary);
			Table.TableChild tableChild6 = (Table.TableChild)table2[colorbuttonPrimary];
			tableChild6.LeftAttach = 1u;
			tableChild6.RightAttach = 2u;
			tableChild6.XOptions = AttachOptions.Fill;
			tableChild6.YOptions = AttachOptions.Fill;
			colorbuttonSecondary = new ColorButton();
			colorbuttonSecondary.CanFocus = true;
			colorbuttonSecondary.Events = EventMask.ButtonMotionMask | EventMask.ButtonPressMask | EventMask.ButtonReleaseMask;
			colorbuttonSecondary.Name = "colorbuttonSecondary";
			table2.Add(colorbuttonSecondary);
			Table.TableChild tableChild7 = (Table.TableChild)table2[colorbuttonSecondary];
			tableChild7.TopAttach = 1u;
			tableChild7.BottomAttach = 2u;
			tableChild7.LeftAttach = 1u;
			tableChild7.RightAttach = 2u;
			tableChild7.XOptions = AttachOptions.Fill;
			tableChild7.YOptions = AttachOptions.Fill;
			label10 = new Label();
			label10.Name = "label10";
			label10.Xalign = 1f;
			label10.LabelProp = Catalog.GetString("_Secondary Color:");
			label10.UseUnderline = true;
			table2.Add(label10);
			Table.TableChild tableChild8 = (Table.TableChild)table2[label10];
			tableChild8.TopAttach = 1u;
			tableChild8.BottomAttach = 2u;
			tableChild8.XOptions = AttachOptions.Fill;
			tableChild8.YOptions = AttachOptions.Fill;
			label11 = new Label();
			label11.Name = "label11";
			label11.Xalign = 1f;
			label11.LabelProp = Catalog.GetString("_Border Color:");
			label11.UseUnderline = true;
			table2.Add(label11);
			Table.TableChild tableChild9 = (Table.TableChild)table2[label11];
			tableChild9.TopAttach = 2u;
			tableChild9.BottomAttach = 3u;
			tableChild9.XOptions = AttachOptions.Fill;
			tableChild9.YOptions = AttachOptions.Fill;
			label9 = new Label();
			label9.Name = "label9";
			label9.Xalign = 1f;
			label9.LabelProp = Catalog.GetString("_Primary Color:");
			label9.UseUnderline = true;
			table2.Add(label9);
			Table.TableChild tableChild10 = (Table.TableChild)table2[label9];
			tableChild10.XOptions = AttachOptions.Fill;
			tableChild10.YOptions = AttachOptions.Fill;
			vbox5.Add(table2);
			Box.BoxChild boxChild11 = (Box.BoxChild)vbox5[table2];
			boxChild11.Position = 0;
			boxChild11.Expand = false;
			boxChild11.Fill = false;
			notebookColorChooser.Add(vbox5);
			Notebook.NotebookChild notebookChild = (Notebook.NotebookChild)notebookColorChooser[vbox5];
			notebookChild.Position = 1;
			label6 = new Label();
			label6.Name = "label6";
			label6.LabelProp = Catalog.GetString("page2");
			notebookColorChooser.SetTabLabel(vbox5, label6);
			label6.ShowAll();
			vbox3.Add(notebookColorChooser);
			Box.BoxChild boxChild12 = (Box.BoxChild)vbox3[notebookColorChooser];
			boxChild12.Position = 0;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			label8 = new Label();
			label8.Name = "label8";
			label8.Xalign = 0f;
			label8.LabelProp = Catalog.GetString("_Preview:");
			label8.UseUnderline = true;
			vbox3.Add(label8);
			Box.BoxChild boxChild13 = (Box.BoxChild)vbox3[label8];
			boxChild13.Position = 1;
			boxChild13.Expand = false;
			boxChild13.Fill = false;
			scrolledwindowTextEditor = new ScrolledWindow();
			scrolledwindowTextEditor.CanFocus = true;
			scrolledwindowTextEditor.Name = "scrolledwindowTextEditor";
			scrolledwindowTextEditor.ShadowType = ShadowType.In;
			vbox3.Add(scrolledwindowTextEditor);
			Box.BoxChild boxChild14 = (Box.BoxChild)vbox3[scrolledwindowTextEditor];
			boxChild14.Position = 2;
			hpaned1.Add(vbox3);
			vBox.Add(hpaned1);
			Box.BoxChild boxChild15 = (Box.BoxChild)vBox[hpaned1];
			boxChild15.Position = 1;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 10;
			actionArea.BorderWidth = 5u;
			actionArea.LayoutStyle = ButtonBoxStyle.End;
			buttonCancel = new Button();
			buttonCancel.CanDefault = true;
			buttonCancel.CanFocus = true;
			buttonCancel.Name = "buttonCancel";
			buttonCancel.UseStock = true;
			buttonCancel.UseUnderline = true;
			buttonCancel.Label = "gtk-cancel";
			AddActionWidget(buttonCancel, -6);
			ButtonBox.ButtonBoxChild buttonBoxChild = (ButtonBox.ButtonBoxChild)actionArea[buttonCancel];
			buttonBoxChild.Expand = false;
			buttonBoxChild.Fill = false;
			buttonOk = new Button();
			buttonOk.CanDefault = true;
			buttonOk.CanFocus = true;
			buttonOk.Name = "buttonOk";
			buttonOk.UseStock = true;
			buttonOk.UseUnderline = true;
			buttonOk.Label = "gtk-ok";
			AddActionWidget(buttonOk, -5);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[buttonOk];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 760;
			base.DefaultHeight = 458;
			label2.MnemonicWidget = entryName;
			label3.MnemonicWidget = entryDescription;
			Show();
		}
	}
}
