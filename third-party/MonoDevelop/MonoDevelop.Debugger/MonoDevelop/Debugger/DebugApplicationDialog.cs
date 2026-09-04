using System.Collections.Generic;
using Gtk;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Ide.Gui.Components;
using Stetic;

namespace MonoDevelop.Debugger
{
	internal class DebugApplicationDialog : Dialog
	{
		private VBox vbox5;

		private Table table1;

		private Entry argsEntry;

		private FileEntry fileEntry;

		private FolderEntry folderEntry;

		private Label label7;

		private Label label8;

		private Label label9;

		private Label label6;

		private EnvVarList envVarList;

		private Button buttonCancel;

		private Button buttonOk;

		public Dictionary<string, string> EnvironmentVariables
		{
			get
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				envVarList.StoreValues(dictionary);
				return dictionary;
			}
			set
			{
				envVarList.LoadValues(value);
			}
		}

		public string WorkingDirectory
		{
			get
			{
				return folderEntry.Path;
			}
			set
			{
				folderEntry.Path = value;
			}
		}

		public string SelectedFile
		{
			get
			{
				return fileEntry.Path;
			}
			set
			{
				fileEntry.Path = value;
			}
		}

		public string Arguments
		{
			get
			{
				return argsEntry.Text;
			}
			set
			{
				argsEntry.Text = value;
			}
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			base.Name = "MonoDevelop.Debugger.DebugApplicationDialog";
			base.Title = Catalog.GetString("Debug Application");
			base.WindowPosition = WindowPosition.CenterOnParent;
			VBox vBox = base.VBox;
			vBox.Name = "dialog1_VBox";
			vBox.BorderWidth = 2u;
			vbox5 = new VBox();
			vbox5.Name = "vbox5";
			vbox5.Spacing = 6;
			vbox5.BorderWidth = 9u;
			table1 = new Table(3u, 2u, homogeneous: false);
			table1.Name = "table1";
			table1.RowSpacing = 6u;
			table1.ColumnSpacing = 6u;
			argsEntry = new Entry();
			argsEntry.CanFocus = true;
			argsEntry.Name = "argsEntry";
			argsEntry.IsEditable = true;
			argsEntry.InvisibleChar = '•';
			table1.Add(argsEntry);
			Table.TableChild tableChild = (Table.TableChild)table1[argsEntry];
			tableChild.TopAttach = 1u;
			tableChild.BottomAttach = 2u;
			tableChild.LeftAttach = 1u;
			tableChild.RightAttach = 2u;
			tableChild.XOptions = AttachOptions.Fill;
			tableChild.YOptions = AttachOptions.Fill;
			fileEntry = new FileEntry();
			fileEntry.Name = "fileEntry";
			fileEntry.DisplayAsRelativePath = false;
			table1.Add(fileEntry);
			Table.TableChild tableChild2 = (Table.TableChild)table1[fileEntry];
			tableChild2.LeftAttach = 1u;
			tableChild2.RightAttach = 2u;
			tableChild2.YOptions = AttachOptions.Fill;
			folderEntry = new FolderEntry();
			folderEntry.Name = "folderEntry";
			folderEntry.DisplayAsRelativePath = false;
			table1.Add(folderEntry);
			Table.TableChild tableChild3 = (Table.TableChild)table1[folderEntry];
			tableChild3.TopAttach = 2u;
			tableChild3.BottomAttach = 3u;
			tableChild3.LeftAttach = 1u;
			tableChild3.RightAttach = 2u;
			tableChild3.XOptions = AttachOptions.Fill;
			tableChild3.YOptions = AttachOptions.Fill;
			label7 = new Label();
			label7.Name = "label7";
			label7.Xalign = 0f;
			label7.LabelProp = Catalog.GetString("Command");
			table1.Add(label7);
			Table.TableChild tableChild4 = (Table.TableChild)table1[label7];
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			label8 = new Label();
			label8.Name = "label8";
			label8.Xalign = 0f;
			label8.LabelProp = Catalog.GetString("Arguments");
			table1.Add(label8);
			Table.TableChild tableChild5 = (Table.TableChild)table1[label8];
			tableChild5.TopAttach = 1u;
			tableChild5.BottomAttach = 2u;
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			label9 = new Label();
			label9.Name = "label9";
			label9.Xalign = 0f;
			label9.LabelProp = Catalog.GetString("Working Directory");
			table1.Add(label9);
			Table.TableChild tableChild6 = (Table.TableChild)table1[label9];
			tableChild6.TopAttach = 2u;
			tableChild6.BottomAttach = 3u;
			tableChild6.XOptions = AttachOptions.Fill;
			tableChild6.YOptions = AttachOptions.Fill;
			vbox5.Add(table1);
			Box.BoxChild boxChild = (Box.BoxChild)vbox5[table1];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			label6 = new Label();
			label6.Name = "label6";
			label6.LabelProp = Catalog.GetString("Environment Variables");
			vbox5.Add(label6);
			Box.BoxChild boxChild2 = (Box.BoxChild)vbox5[label6];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			envVarList = new EnvVarList();
			envVarList.CanFocus = true;
			envVarList.Name = "envVarList";
			envVarList.ShadowType = ShadowType.In;
			vbox5.Add(envVarList);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox5[envVarList];
			boxChild3.Position = 2;
			vBox.Add(vbox5);
			Box.BoxChild boxChild4 = (Box.BoxChild)vBox[vbox5];
			boxChild4.Position = 0;
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
			base.DefaultWidth = 656;
			base.DefaultHeight = 300;
			Show();
		}

		public DebugApplicationDialog()
		{
			Build();
		}
	}
}
