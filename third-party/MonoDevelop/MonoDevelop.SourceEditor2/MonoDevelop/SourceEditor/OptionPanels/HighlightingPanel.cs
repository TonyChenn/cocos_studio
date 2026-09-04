using System;
using System.IO;
using GLib;
using Gtk;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Dialogs;
using Stetic;

namespace MonoDevelop.SourceEditor.OptionPanels
{
	public class HighlightingPanel : Bin, IOptionsPanel
	{
		private string schemeName;

		private ListStore styleStore = new ListStore(typeof(string), typeof(ColorScheme));

		private OptionsDialog dialog;

		private VBox vbox4;

		private Label GtkLabel5;

		private Alignment GtkAlignment;

		private VBox vbox5;

		private HBox hbox1;

		private ScrolledWindow scrolledwindow1;

		private TreeView styleTreeview;

		private VBox vbox1;

		private Button buttonNew;

		private Button buttonEdit;

		private HBox hbox2;

		private Button addButton;

		private Button removeButton;

		private Button buttonExport;

		public HighlightingPanel()
		{
			Build();
			styleTreeview.AppendColumn("", new CellRendererText(), "markup", 0);
			styleTreeview.Model = styleStore;
			new SourceEditorDisplayBinding();
			schemeName = DefaultSourceEditorOptions.Instance.ColorScheme;
		}

		protected override void OnDestroyed()
		{
			DefaultSourceEditorOptions.Instance.ColorScheme = schemeName;
			if (styleStore != null)
			{
				styleStore.Dispose();
				styleStore = null;
			}
			base.OnDestroyed();
		}

		private string GetMarkup(string name, string description)
		{
			return $"<b>{Markup.EscapeText(name)}</b> - {Markup.EscapeText(description)}";
		}

		public virtual Widget CreatePanelWidget()
		{
			addButton.Clicked += AddColorScheme;
			removeButton.Clicked += RemoveColorScheme;
			buttonEdit.Clicked += HandleButtonEdithandleClicked;
			buttonNew.Clicked += HandleButtonNewClicked;
			buttonExport.Clicked += HandleButtonExportClicked;
			styleTreeview.Selection.Changed += HandleStyleTreeviewSelectionChanged;
			EnableHighlightingCheckbuttonToggled(this, EventArgs.Empty);
			ShowStyles();
			HandleStyleTreeviewSelectionChanged(null, null);
			return this;
		}

		private void HandleButtonNewClicked(object sender, EventArgs e)
		{
			NewColorShemeDialog newColorShemeDialog = new NewColorShemeDialog();
			MessageService.ShowCustomDialog(newColorShemeDialog, dialog);
			ShowStyles();
		}

		private void HandleStyleTreeviewSelectionChanged(object sender, EventArgs e)
		{
			removeButton.Sensitive = false;
			buttonEdit.Sensitive = false;
			buttonExport.Sensitive = false;
			if (!styleTreeview.Selection.GetSelected(out var iter))
			{
				return;
			}
			ColorScheme colorScheme = (ColorScheme)styleStore.GetValue(iter, 1);
			if (colorScheme != null)
			{
				DefaultSourceEditorOptions.Instance.ColorScheme = colorScheme.Name;
				buttonExport.Sensitive = true;
				string fileName = colorScheme.FileName;
				if (fileName != null)
				{
					removeButton.Sensitive = true;
					buttonEdit.Sensitive = true;
				}
			}
		}

		private void HandleButtonEdithandleClicked(object sender, EventArgs e)
		{
			if (styleTreeview.Selection.GetSelected(out var iter))
			{
				ColorShemeEditor colorShemeEditor = new ColorShemeEditor(this);
				ColorScheme sheme = (ColorScheme)styleStore.GetValue(iter, 1);
				colorShemeEditor.SetSheme(sheme);
				MessageService.ShowCustomDialog(colorShemeEditor, dialog);
			}
		}

		private ColorScheme LoadStyle(string styleName, bool showException = true)
		{
			try
			{
				return Mono.TextEditor.Highlighting.SyntaxModeService.GetColorStyle(styleName);
			}
			catch (Exception ex)
			{
				if (showException)
				{
					MessageService.ShowError("Error while importing color style " + styleName, (ex.InnerException ?? ex).Message);
				}
				return Mono.TextEditor.Highlighting.SyntaxModeService.DefaultColorStyle;
			}
		}

		internal void ShowStyles()
		{
			styleStore.Clear();
			TreeIter iter = styleStore.AppendValues(GetMarkup(GettextCatalog.GetString("Default"), GettextCatalog.GetString("The default color scheme.")), LoadStyle("Default"));
			string[] styles = Mono.TextEditor.Highlighting.SyntaxModeService.Styles;
			foreach (string text in styles)
			{
				if (text == "Default")
				{
					continue;
				}
				ColorScheme colorScheme = LoadStyle(text);
				string text2 = colorScheme.Name ?? "";
				string text3 = colorScheme.Description ?? "";
				if (string.IsNullOrEmpty(colorScheme.FileName))
				{
					try
					{
						text2 = GettextCatalog.GetString(text2);
						if (!string.IsNullOrEmpty(text3))
						{
							text3 = GettextCatalog.GetString(text3);
						}
					}
					catch
					{
					}
				}
				TreeIter treeIter = styleStore.AppendValues(GetMarkup(text2, text3), colorScheme);
				if (colorScheme.Name == DefaultSourceEditorOptions.Instance.ColorScheme)
				{
					iter = treeIter;
				}
			}
			styleTreeview.Selection.SelectIter(iter);
		}

		private void RemoveColorScheme(object sender, EventArgs args)
		{
			if (styleTreeview.Selection.GetSelected(out var iter))
			{
				ColorScheme colorScheme = (ColorScheme)styleStore.GetValue(iter, 1);
				string fileName = colorScheme.FileName;
				if (fileName != null && fileName.StartsWith(SourceEditorDisplayBinding.SyntaxModePath, StringComparison.Ordinal))
				{
					Mono.TextEditor.Highlighting.SyntaxModeService.Remove(colorScheme);
					File.Delete(fileName);
					ShowStyles();
				}
			}
		}

		private void HandleButtonExportClicked(object sender, EventArgs e)
		{
			SelectFileDialog selectFileDialog = new SelectFileDialog(GettextCatalog.GetString("Highlighting Scheme"), FileChooserAction.Save);
			selectFileDialog.TransientFor = base.Toplevel as Window;
			SelectFileDialog selectFileDialog2 = selectFileDialog;
			selectFileDialog2.AddFilter(GettextCatalog.GetString("Color schemes"), "*.json");
			if (selectFileDialog2.Run() && styleTreeview.Selection.GetSelected(out var iter))
			{
				ColorScheme colorScheme = (ColorScheme)styleStore.GetValue(iter, 1);
				string text = selectFileDialog2.SelectedFile.ToString();
				if (!text.EndsWith(".json", StringComparison.Ordinal))
				{
					text += ".json";
				}
				colorScheme.Save(text);
			}
		}

		private void AddColorScheme(object sender, EventArgs args)
		{
			SelectFileDialog selectFileDialog = new SelectFileDialog(GettextCatalog.GetString("Highlighting Scheme"), FileChooserAction.Open);
			selectFileDialog.TransientFor = base.Toplevel as Window;
			SelectFileDialog selectFileDialog2 = selectFileDialog;
			selectFileDialog2.AddFilter(GettextCatalog.GetString("Color schemes"), "*.json");
			selectFileDialog2.AddFilter(GettextCatalog.GetString("Visual Studio .NET settings"), "*.vssettings");
			if (selectFileDialog2.Run())
			{
				string destFileName = SourceEditorDisplayBinding.SyntaxModePath.Combine(selectFileDialog2.SelectedFile.FileName);
				bool flag = true;
				try
				{
					File.Copy(selectFileDialog2.SelectedFile.FullPath, destFileName);
				}
				catch (Exception ex)
				{
					flag = false;
					LoggingService.LogError("Can't copy syntax mode file.", ex);
				}
				if (flag)
				{
					SourceEditorDisplayBinding.LoadCustomStylesAndModes();
					ShowStyles();
				}
			}
		}

		private void EnableHighlightingCheckbuttonToggled(object sender, EventArgs e)
		{
		}

		internal static void UpdateActiveDocument()
		{
			if (IdeApp.Workbench.ActiveDocument != null)
			{
				IdeApp.Workbench.ActiveDocument.UpdateParseDocument();
				TextEditorData editor = IdeApp.Workbench.ActiveDocument.Editor;
				if (editor != null)
				{
					editor.Parent.TextViewMargin.PurgeLayoutCache();
					editor.Parent.QueueDraw();
				}
			}
		}

		public virtual void ApplyChanges()
		{
			if (styleTreeview.Selection.GetSelected(out var iter))
			{
				ColorScheme colorScheme = (ColorScheme)styleStore.GetValue(iter, 1);
				DefaultSourceEditorOptions.Instance.ColorScheme = (schemeName = colorScheme?.Name);
			}
		}

		public void Initialize(OptionsDialog dialog, object dataObject)
		{
			this.dialog = dialog;
		}

		public bool IsVisible()
		{
			return true;
		}

		public bool ValidateChanges()
		{
			return true;
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			Stetic.BinContainer.Attach(this);
			base.Name = "MonoDevelop.SourceEditor.OptionPanels.HighlightingPanel";
			vbox4 = new VBox();
			vbox4.Name = "vbox4";
			vbox4.Spacing = 6;
			GtkLabel5 = new Label();
			GtkLabel5.Name = "GtkLabel5";
			GtkLabel5.Xalign = 0f;
			GtkLabel5.LabelProp = Catalog.GetString("<b>Color scheme</b>");
			GtkLabel5.UseMarkup = true;
			vbox4.Add(GtkLabel5);
			Box.BoxChild boxChild = (Box.BoxChild)vbox4[GtkLabel5];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			GtkAlignment = new Alignment(0f, 0f, 1f, 1f);
			GtkAlignment.Name = "GtkAlignment";
			GtkAlignment.LeftPadding = 12u;
			vbox5 = new VBox();
			vbox5.Name = "vbox5";
			vbox5.Spacing = 6;
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 6;
			scrolledwindow1 = new ScrolledWindow();
			scrolledwindow1.CanFocus = true;
			scrolledwindow1.Name = "scrolledwindow1";
			scrolledwindow1.ShadowType = ShadowType.In;
			Viewport viewport = new Viewport();
			viewport.ShadowType = ShadowType.None;
			styleTreeview = new TreeView();
			styleTreeview.CanFocus = true;
			styleTreeview.Name = "styleTreeview";
			styleTreeview.HeadersVisible = false;
			viewport.Add(styleTreeview);
			scrolledwindow1.Add(viewport);
			hbox1.Add(scrolledwindow1);
			Box.BoxChild boxChild2 = (Box.BoxChild)hbox1[scrolledwindow1];
			boxChild2.Position = 0;
			vbox1 = new VBox();
			vbox1.Name = "vbox1";
			vbox1.Spacing = 6;
			buttonNew = new Button();
			buttonNew.CanFocus = true;
			buttonNew.Name = "buttonNew";
			buttonNew.UseStock = true;
			buttonNew.UseUnderline = true;
			buttonNew.Label = "gtk-new";
			vbox1.Add(buttonNew);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox1[buttonNew];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			buttonEdit = new Button();
			buttonEdit.CanFocus = true;
			buttonEdit.Name = "buttonEdit";
			buttonEdit.UseUnderline = true;
			buttonEdit.Label = Catalog.GetString("_Edit");
			vbox1.Add(buttonEdit);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox1[buttonEdit];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			hbox1.Add(vbox1);
			Box.BoxChild boxChild5 = (Box.BoxChild)hbox1[vbox1];
			boxChild5.Position = 1;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			vbox5.Add(hbox1);
			Box.BoxChild boxChild6 = (Box.BoxChild)vbox5[hbox1];
			boxChild6.Position = 0;
			hbox2 = new HBox();
			hbox2.Name = "hbox2";
			hbox2.Spacing = 6;
			addButton = new Button();
			addButton.CanFocus = true;
			addButton.Name = "addButton";
			addButton.UseStock = true;
			addButton.UseUnderline = true;
			addButton.Label = "gtk-add";
			hbox2.Add(addButton);
			Box.BoxChild boxChild7 = (Box.BoxChild)hbox2[addButton];
			boxChild7.PackType = PackType.End;
			boxChild7.Position = 0;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			removeButton = new Button();
			removeButton.CanFocus = true;
			removeButton.Name = "removeButton";
			removeButton.UseStock = true;
			removeButton.UseUnderline = true;
			removeButton.Label = "gtk-remove";
			hbox2.Add(removeButton);
			Box.BoxChild boxChild8 = (Box.BoxChild)hbox2[removeButton];
			boxChild8.PackType = PackType.End;
			boxChild8.Position = 1;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			buttonExport = new Button();
			buttonExport.CanFocus = true;
			buttonExport.Name = "buttonExport";
			buttonExport.UseUnderline = true;
			buttonExport.Label = Catalog.GetString("Export");
			hbox2.Add(buttonExport);
			Box.BoxChild boxChild9 = (Box.BoxChild)hbox2[buttonExport];
			boxChild9.PackType = PackType.End;
			boxChild9.Position = 2;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			vbox5.Add(hbox2);
			Box.BoxChild boxChild10 = (Box.BoxChild)vbox5[hbox2];
			boxChild10.PackType = PackType.End;
			boxChild10.Position = 1;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			GtkAlignment.Add(vbox5);
			vbox4.Add(GtkAlignment);
			Box.BoxChild boxChild11 = (Box.BoxChild)vbox4[GtkAlignment];
			boxChild11.Position = 1;
			Add(vbox4);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			Show();
		}
	}
}
