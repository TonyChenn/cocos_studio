using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.Unix;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using Stetic;

namespace MonoDevelop.Refactoring.Rename
{
	public class RenameItemDialog : Dialog
	{
		private RenameRefactoring rename;

		private RefactoringOptions options;

		private VBox vbox;

		private HBox hbox;

		private Label labelNewName;

		private Entry entry;

		private CheckButton renameFileFlag;

		private CheckButton includeOverloadsCheckbox;

		private HBox hbox1;

		private Image imageWarning;

		private Label labelWarning;

		private Button buttonCancel;

		private Button buttonPreview;

		private Button buttonOk;

		private RenameRefactoring.RenameProperties Properties
		{
			get
			{
				RenameRefactoring.RenameProperties renameProperties = new RenameRefactoring.RenameProperties();
				renameProperties.NewName = entry.Text;
				renameProperties.RenameFile = renameFileFlag.Visible && renameFileFlag.Active;
				renameProperties.IncludeOverloads = includeOverloadsCheckbox.Visible && includeOverloadsCheckbox.Active;
				return renameProperties;
			}
		}

		public RenameItemDialog(RefactoringOptions options, RenameRefactoring rename)
		{
			this.options = options;
			this.rename = rename;
			if (options.SelectedItem is IMethod && ((IMethod)options.SelectedItem).IsConstructor)
			{
				options.SelectedItem = ((IMethod)options.SelectedItem).DeclaringType;
			}
			Build();
			includeOverloadsCheckbox.Active = true;
			includeOverloadsCheckbox.Visible = false;
			if (options.SelectedItem is IType)
			{
				IType type = (IType)options.SelectedItem;
				if (type.Kind == TypeKind.TypeParameter)
				{
					base.Title = GettextCatalog.GetString("Rename Type Parameter");
					entry.Text = type.Name;
				}
				else
				{
					ITypeDefinition definition = type.GetDefinition();
					if (definition.DeclaringType == null)
					{
						renameFileFlag.Visible = true;
						renameFileFlag.Active = options.Document != null && options.Document.FileName.FileNameWithoutExtension.Contains(definition.Name);
					}
					else
					{
						renameFileFlag.Active = false;
					}
					if (definition.Kind == TypeKind.Interface)
					{
						base.Title = GettextCatalog.GetString("Rename Interface");
					}
					else
					{
						base.Title = GettextCatalog.GetString("Rename Class");
					}
				}
			}
			else if (options.SelectedItem is IField)
			{
				base.Title = GettextCatalog.GetString("Rename Field");
			}
			else if (options.SelectedItem is IProperty)
			{
				if (((IProperty)options.SelectedItem).IsIndexer)
				{
					base.Title = GettextCatalog.GetString("Rename Indexer");
				}
				else
				{
					base.Title = GettextCatalog.GetString("Rename Property");
				}
			}
			else if (options.SelectedItem is IEvent)
			{
				base.Title = GettextCatalog.GetString("Rename Event");
			}
			else if (options.SelectedItem is IMethod)
			{
				IMethod m = (IMethod)options.SelectedItem;
				if (m.IsConstructor || m.IsDestructor)
				{
					base.Title = GettextCatalog.GetString("Rename Class");
				}
				else
				{
					base.Title = GettextCatalog.GetString("Rename Method");
					includeOverloadsCheckbox.Visible = m.DeclaringType.GetMethods((IUnresolvedMethod x) => x.Name == m.Name).Count() > 1;
				}
			}
			else if (options.SelectedItem is IParameter)
			{
				base.Title = GettextCatalog.GetString("Rename Parameter");
			}
			else if (options.SelectedItem is IVariable)
			{
				base.Title = GettextCatalog.GetString("Rename Variable");
			}
			else if (options.SelectedItem is ITypeParameter)
			{
				base.Title = GettextCatalog.GetString("Rename Type Parameter");
			}
			else if (options.SelectedItem is INamespace)
			{
				base.Title = GettextCatalog.GetString("Rename namespace");
			}
			else
			{
				base.Title = GettextCatalog.GetString("Rename Item");
			}
			if (options.SelectedItem is IEntity)
			{
				IEntity entity = (IEntity)options.SelectedItem;
				if (entity.SymbolKind == SymbolKind.Constructor || entity.SymbolKind == SymbolKind.Destructor)
				{
					entry.Text = entity.DeclaringType.Name;
				}
				else
				{
					entry.Text = entity.Name;
				}
			}
			else if (options.SelectedItem is IType)
			{
				IType type2 = (IType)options.SelectedItem;
				entry.Text = type2.Name;
			}
			else if (options.SelectedItem is ITypeParameter)
			{
				ITypeParameter typeParameter = (ITypeParameter)options.SelectedItem;
				entry.Text = typeParameter.Name;
			}
			else if (options.SelectedItem is IVariable)
			{
				IVariable variable = (IVariable)options.SelectedItem;
				entry.Text = variable.Name;
			}
			else if (options.SelectedItem is INamespace)
			{
				INamespace obj = (INamespace)options.SelectedItem;
				entry.Text = obj.FullName;
			}
			entry.SelectRegion(0, -1);
			buttonPreview.Sensitive = (buttonOk.Sensitive = false);
			entry.Changed += OnEntryChanged;
			entry.Activated += OnEntryActivated;
			buttonOk.Clicked += OnOKClicked;
			buttonPreview.Clicked += OnPreviewClicked;
			entry.Changed += delegate
			{
				Button button = buttonPreview;
				bool sensitive = (buttonOk.Sensitive = ValidateName());
				button.Sensitive = sensitive;
			};
			ValidateName();
		}

		private bool ValidateName()
		{
			return true;
		}

		private void OnEntryChanged(object sender, EventArgs e)
		{
			Button button = buttonPreview;
			bool sensitive = (buttonOk.Sensitive = entry.Text.Length > 0);
			button.Sensitive = sensitive;
		}

		private void OnEntryActivated(object sender, EventArgs e)
		{
			if (buttonOk.Sensitive)
			{
				buttonOk.Click();
			}
		}

		private void OnOKClicked(object sender, EventArgs e)
		{
			RenameRefactoring.RenameProperties renameProperties = Properties;
			Destroy();
			List<Change> changes = rename.PerformChanges(options, renameProperties);
			IProgressMonitor backgroundProgressMonitor = IdeApp.Workbench.ProgressMonitors.GetBackgroundProgressMonitor(base.Title, null);
			RefactoringService.AcceptChanges(backgroundProgressMonitor, changes);
		}

		private void OnPreviewClicked(object sender, EventArgs e)
		{
			RenameRefactoring.RenameProperties renameProperties = Properties;
			Destroy();
			List<Change> changes = rename.PerformChanges(options, renameProperties);
			MessageService.ShowCustomDialog(new RefactoringPreviewDialog(changes));
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			base.Name = "MonoDevelop.Refactoring.Rename.RenameItemDialog";
			base.Title = Catalog.GetString("Rename {0}");
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.BorderWidth = 6u;
			VBox vBox = base.VBox;
			vBox.Name = "dialog1_VBox";
			vBox.BorderWidth = 2u;
			vbox = new VBox();
			vbox.Name = "vbox";
			vbox.Spacing = 6;
			vbox.BorderWidth = 6u;
			hbox = new HBox();
			hbox.Name = "hbox";
			hbox.Spacing = 6;
			labelNewName = new Label();
			labelNewName.Name = "labelNewName";
			labelNewName.LabelProp = Catalog.GetString("New na_me:");
			labelNewName.UseUnderline = true;
			hbox.Add(labelNewName);
			Box.BoxChild boxChild = (Box.BoxChild)hbox[labelNewName];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			entry = new Entry();
			entry.CanFocus = true;
			entry.Name = "entry";
			entry.IsEditable = true;
			entry.InvisibleChar = '●';
			hbox.Add(entry);
			Box.BoxChild boxChild2 = (Box.BoxChild)hbox[entry];
			boxChild2.Position = 1;
			vbox.Add(hbox);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox[hbox];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			renameFileFlag = new CheckButton();
			renameFileFlag.CanFocus = true;
			renameFileFlag.Name = "renameFileFlag";
			renameFileFlag.Label = Catalog.GetString("Rename file that contains public class");
			renameFileFlag.Active = true;
			renameFileFlag.DrawIndicator = true;
			renameFileFlag.UseUnderline = true;
			vbox.Add(renameFileFlag);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox[renameFileFlag];
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			includeOverloadsCheckbox = new CheckButton();
			includeOverloadsCheckbox.CanFocus = true;
			includeOverloadsCheckbox.Name = "includeOverloadsCheckbox";
			includeOverloadsCheckbox.Label = Catalog.GetString("Include overloads");
			includeOverloadsCheckbox.Active = true;
			includeOverloadsCheckbox.DrawIndicator = true;
			includeOverloadsCheckbox.UseUnderline = true;
			vbox.Add(includeOverloadsCheckbox);
			Box.BoxChild boxChild5 = (Box.BoxChild)vbox[includeOverloadsCheckbox];
			boxChild5.Position = 2;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 6;
			imageWarning = new Image();
			imageWarning.Name = "imageWarning";
			imageWarning.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-apply", IconSize.Button);
			hbox1.Add(imageWarning);
			Box.BoxChild boxChild6 = (Box.BoxChild)hbox1[imageWarning];
			boxChild6.Position = 0;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			labelWarning = new Label();
			labelWarning.Name = "labelWarning";
			hbox1.Add(labelWarning);
			Box.BoxChild boxChild7 = (Box.BoxChild)hbox1[labelWarning];
			boxChild7.Position = 1;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			vbox.Add(hbox1);
			Box.BoxChild boxChild8 = (Box.BoxChild)vbox[hbox1];
			boxChild8.Position = 3;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			vBox.Add(vbox);
			Box.BoxChild boxChild9 = (Box.BoxChild)vBox[vbox];
			boxChild9.Position = 0;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
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
			buttonPreview = new Button();
			buttonPreview.CanFocus = true;
			buttonPreview.Name = "buttonPreview";
			buttonPreview.UseUnderline = true;
			buttonPreview.Label = Catalog.GetString("_Preview");
			AddActionWidget(buttonPreview, 0);
			ButtonBox.ButtonBoxChild buttonBoxChild2 = (ButtonBox.ButtonBoxChild)actionArea[buttonPreview];
			buttonBoxChild2.Position = 1;
			buttonBoxChild2.Expand = false;
			buttonBoxChild2.Fill = false;
			buttonOk = new Button();
			buttonOk.CanDefault = true;
			buttonOk.CanFocus = true;
			buttonOk.Name = "buttonOk";
			buttonOk.UseStock = true;
			buttonOk.UseUnderline = true;
			buttonOk.Label = "gtk-ok";
			AddActionWidget(buttonOk, -5);
			ButtonBox.ButtonBoxChild buttonBoxChild3 = (ButtonBox.ButtonBoxChild)actionArea[buttonOk];
			buttonBoxChild3.Position = 2;
			buttonBoxChild3.Expand = false;
			buttonBoxChild3.Fill = false;
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 365;
			base.DefaultHeight = 174;
			labelNewName.MnemonicWidget = entry;
			renameFileFlag.Hide();
			includeOverloadsCheckbox.Hide();
			Hide();
		}
	}
}
