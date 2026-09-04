using System;
using System.Collections.Generic;
using Gtk;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.ProgressMonitoring;
using Stetic;
using Xwt.Drawing;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	internal class ComponentSelectorDialog : Dialog
	{
		private const int ColChecked = 0;

		private const int ColName = 1;

		private const int ColNamespace = 2;

		private const int ColLibrary = 3;

		private const int ColPath = 4;

		private const int ColIcon = 5;

		private const int ColItem = 6;

		private const int ColShowCheck = 7;

		private const int ColBold = 8;

		private TreeStore store;

		private ComponentIndex index;

		private bool indexModified;

		private bool showCategories;

		private Dictionary<ItemToolboxNode, ItemToolboxNode> currentItems = new Dictionary<ItemToolboxNode, ItemToolboxNode>();

		private VBox vbox2;

		private HBox hbox1;

		private Label label1;

		private ComboBox comboType;

		private VSeparator vseparator1;

		private Button button24;

		private ScrolledWindow scrolledwindow1;

		private TreeView listView;

		private CheckButton checkGroupByCat;

		private Button buttonCancel;

		private Button buttonOk;

		public ComponentSelectorDialog(IToolboxConsumer currentConsumer)
		{
			using (IProgressMonitor monitor = new MessageDialogProgressMonitor(showProgress: true, allowCancel: true, showDetails: false, hideWhenDone: true))
			{
				index = DesignerSupport.Service.ToolboxService.GetComponentIndex(monitor);
			}
			Build();
			store = new TreeStore(typeof(bool), typeof(string), typeof(string), typeof(string), typeof(string), typeof(Xwt.Drawing.Image), typeof(ItemToolboxNode), typeof(bool), typeof(int));
			TreeViewColumn treeViewColumn = new TreeViewColumn();
			CellRendererToggle cellRendererToggle = new CellRendererToggle();
			treeViewColumn.PackStart(cellRendererToggle, expand: false);
			treeViewColumn.AddAttribute(cellRendererToggle, "active", 0);
			treeViewColumn.AddAttribute(cellRendererToggle, "visible", 7);
			cellRendererToggle.Toggled += OnToggleItem;
			treeViewColumn.SortColumnId = 0;
			listView.AppendColumn(treeViewColumn);
			treeViewColumn = new TreeViewColumn();
			treeViewColumn.Spacing = 3;
			treeViewColumn.Title = GettextCatalog.GetString("Name");
			CellRendererImage cell = new CellRendererImage();
			CellRendererText cell2 = new CellRendererText
			{
				Width = 150
			};
			treeViewColumn.PackStart(cell, expand: false);
			treeViewColumn.PackStart(cell2, expand: false);
			treeViewColumn.AddAttribute(cell, "image", 5);
			treeViewColumn.AddAttribute(cell, "visible", 7);
			treeViewColumn.AddAttribute(cell2, "text", 1);
			treeViewColumn.AddAttribute(cell2, "weight", 8);
			listView.AppendColumn(treeViewColumn);
			treeViewColumn.Resizable = true;
			treeViewColumn.SortColumnId = 1;
			treeViewColumn = listView.AppendColumn(GettextCatalog.GetString("Library"), new CellRendererText(), "text", 3);
			treeViewColumn.Resizable = true;
			treeViewColumn.SortColumnId = 3;
			treeViewColumn = listView.AppendColumn(GettextCatalog.GetString("Location"), new CellRendererText(), "text", 4);
			treeViewColumn.Resizable = true;
			treeViewColumn.SortColumnId = 4;
			store.SetSortColumnId(1, SortType.Ascending);
			listView.SearchColumn = 1;
			listView.Model = store;
			foreach (ItemToolboxNode userItem in DesignerSupport.Service.ToolboxService.UserItems)
			{
				currentItems[userItem] = userItem;
			}
			List<string> list = new List<string>();
			foreach (ComponentIndexFile file in index.Files)
			{
				foreach (ItemToolboxNode component in file.Components)
				{
					if (!list.Contains(component.ItemDomain))
					{
						list.Add(component.ItemDomain);
					}
				}
			}
			string text = null;
			if (currentConsumer != null)
			{
				text = currentConsumer.DefaultItemDomain;
			}
			comboType.AppendText(GettextCatalog.GetString("All"));
			comboType.Active = 0;
			for (int i = 0; i < list.Count; i++)
			{
				string text2 = list[i];
				comboType.AppendText(text2);
				if (text2 == text)
				{
					comboType.Active = i + 1;
				}
			}
		}

		public void Fill()
		{
			store.Clear();
			foreach (ComponentIndexFile file in index.Files)
			{
				foreach (ItemToolboxNode component in file.Components)
				{
					if (comboType.Active <= 0 || comboType.ActiveText == component.ItemDomain)
					{
						AddItem(file, component);
					}
				}
			}
		}

		private void AddItem(ComponentIndexFile ifile, ItemToolboxNode co)
		{
			Xwt.Drawing.Image image = ((co.Icon != null) ? co.Icon.WithSize(16.0, 16.0) : null);
			if (showCategories)
			{
				bool flag = false;
				if (store.GetIterFirst(out var iter))
				{
					do
					{
						if (co.Category == (string)store.GetValue(iter, 1))
						{
							flag = true;
							break;
						}
					}
					while (store.IterNext(ref iter));
				}
				if (!flag)
				{
					iter = store.AppendValues(false, co.Category, string.Empty, string.Empty, string.Empty, null, null, false, 700);
				}
				store.AppendValues(iter, currentItems.ContainsKey(co), co.Name, string.Empty, ifile.Name, ifile.Location, image, co, true, 400);
			}
			else
			{
				store.AppendValues(currentItems.ContainsKey(co), co.Name, string.Empty, ifile.Name, ifile.Location, image, co, true, 400);
			}
		}

		protected virtual void OnComboTypeChanged(object sender, EventArgs e)
		{
			Fill();
		}

		private void OnToggleItem(object ob, ToggledArgs args)
		{
			if (store.GetIterFromString(out var iter, args.Path))
			{
				bool flag = (bool)store.GetValue(iter, 0);
				ItemToolboxNode itemToolboxNode = (ItemToolboxNode)store.GetValue(iter, 6);
				if (!flag)
				{
					currentItems.Add(itemToolboxNode, itemToolboxNode);
				}
				else
				{
					currentItems.Remove(itemToolboxNode);
				}
				store.SetValue(iter, 0, !flag);
			}
		}

		protected void OnButtonOkClicked(object sender, EventArgs e)
		{
			if (indexModified)
			{
				index.Save();
			}
			DesignerSupport.Service.ToolboxService.UpdateUserItems(currentItems.Values);
			Respond(ResponseType.Ok);
		}

		protected virtual void OnButton24Clicked(object sender, EventArgs e)
		{
			SelectFileDialog selectFileDialog = new SelectFileDialog(GettextCatalog.GetString("Add items to toolbox"));
			selectFileDialog.SelectMultiple = true;
			selectFileDialog.TransientFor = this;
			SelectFileDialog selectFileDialog2 = selectFileDialog;
			selectFileDialog2.AddFilter(null, "*.dll");
			if (!selectFileDialog2.Run())
			{
				return;
			}
			indexModified = true;
			using (MessageDialogProgressMonitor messageDialogProgressMonitor = new MessageDialogProgressMonitor(showProgress: true, allowCancel: false, showDetails: false, hideWhenDone: true))
			{
				messageDialogProgressMonitor.BeginTask(GettextCatalog.GetString("Looking for components..."), selectFileDialog2.SelectedFiles.Length);
				FilePath[] selectedFiles = selectFileDialog2.SelectedFiles;
				foreach (string text in selectedFiles)
				{
					ComponentIndexFile componentIndexFile = index.AddFile(text);
					messageDialogProgressMonitor.Step(1);
					if (componentIndexFile != null)
					{
						foreach (ItemToolboxNode component in componentIndexFile.Components)
						{
							currentItems.Add(component, component);
						}
					}
					else
					{
						MessageService.ShowWarning(GettextCatalog.GetString("The file '{0}' does not contain any component.", text));
					}
				}
			}
			Fill();
		}

		protected virtual void OnCheckbutton1Clicked(object sender, EventArgs e)
		{
			showCategories = checkGroupByCat.Active;
			Fill();
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			base.Name = "MonoDevelop.DesignerSupport.Toolbox.ComponentSelectorDialog";
			base.Title = Catalog.GetString("Toolbox Item Selector");
			base.WindowPosition = WindowPosition.CenterOnParent;
			VBox vBox = base.VBox;
			vBox.Name = "dialog1_VBox";
			vBox.BorderWidth = 2u;
			vbox2 = new VBox();
			vbox2.Name = "vbox2";
			vbox2.Spacing = 6;
			vbox2.BorderWidth = 6u;
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 6;
			label1 = new Label();
			label1.Name = "label1";
			label1.Xalign = 0f;
			label1.LabelProp = Catalog.GetString("Type of component:");
			hbox1.Add(label1);
			Box.BoxChild boxChild = (Box.BoxChild)hbox1[label1];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			comboType = ComboBox.NewText();
			comboType.Name = "comboType";
			hbox1.Add(comboType);
			Box.BoxChild boxChild2 = (Box.BoxChild)hbox1[comboType];
			boxChild2.Position = 1;
			vseparator1 = new VSeparator();
			vseparator1.Name = "vseparator1";
			hbox1.Add(vseparator1);
			Box.BoxChild boxChild3 = (Box.BoxChild)hbox1[vseparator1];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			button24 = new Button();
			button24.CanFocus = true;
			button24.Name = "button24";
			button24.UseUnderline = true;
			Alignment alignment = new Alignment(0.5f, 0.5f, 0f, 0f);
			HBox hBox = new HBox();
			hBox.Spacing = 2;
			Gtk.Image image = new Gtk.Image();
			image.Pixbuf = IconLoader.LoadIcon(this, "gtk-add", IconSize.Menu);
			hBox.Add(image);
			Label label = new Label();
			label.LabelProp = Catalog.GetString("Add Assembly...");
			label.UseUnderline = true;
			hBox.Add(label);
			alignment.Add(hBox);
			button24.Add(alignment);
			hbox1.Add(button24);
			Box.BoxChild boxChild4 = (Box.BoxChild)hbox1[button24];
			boxChild4.Position = 3;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			vbox2.Add(hbox1);
			Box.BoxChild boxChild5 = (Box.BoxChild)vbox2[hbox1];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			scrolledwindow1 = new ScrolledWindow();
			scrolledwindow1.CanFocus = true;
			scrolledwindow1.Name = "scrolledwindow1";
			scrolledwindow1.ShadowType = ShadowType.In;
			listView = new TreeView();
			listView.CanFocus = true;
			listView.Name = "listView";
			scrolledwindow1.Add(listView);
			vbox2.Add(scrolledwindow1);
			Box.BoxChild boxChild6 = (Box.BoxChild)vbox2[scrolledwindow1];
			boxChild6.Position = 1;
			checkGroupByCat = new CheckButton();
			checkGroupByCat.CanFocus = true;
			checkGroupByCat.Name = "checkGroupByCat";
			checkGroupByCat.Label = Catalog.GetString("Group by component category");
			checkGroupByCat.DrawIndicator = true;
			checkGroupByCat.UseUnderline = true;
			vbox2.Add(checkGroupByCat);
			Box.BoxChild boxChild7 = (Box.BoxChild)vbox2[checkGroupByCat];
			boxChild7.Position = 2;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			vBox.Add(vbox2);
			Box.BoxChild boxChild8 = (Box.BoxChild)vBox[vbox2];
			boxChild8.Position = 0;
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
			actionArea.Add(buttonOk);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[buttonOk];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 642;
			base.DefaultHeight = 433;
			Hide();
			comboType.Changed += OnComboTypeChanged;
			button24.Clicked += OnButton24Clicked;
			checkGroupByCat.Clicked += OnCheckbutton1Clicked;
			buttonOk.Clicked += OnButtonOkClicked;
		}
	}
}
