using System.ComponentModel;
using Gtk;
using Mono.Unix;
using MonoDevelop.Projects;
using Stetic;

namespace CocoStudio.LuaBinding
{
	[ToolboxItem(true)]
	internal class CompilerParametersPanelWidget : Bin
	{
		private ListStore _VersionsStore;

		private DotNetProject project;

		private DotNetProjectConfiguration configuration;

		private Table MainTable;

		private Label label1;

		private Label label3;

		private ComboBox LanguageVersion;

		private Entry MainFileEntry;

		public string DefaultFile
		{
			get
			{
				return MainFileEntry.Text ?? string.Empty;
			}
			set
			{
				MainFileEntry.Text = value ?? string.Empty;
			}
		}

		public LangVersion LangVersion
		{
			get
			{
				switch (LanguageVersion.Active)
				{
				case 0:
					return LangVersion.Lua;
				case 1:
					return LangVersion.Lua51;
				case 2:
					return LangVersion.Lua52;
				case 3:
					return LangVersion.LuaJIT;
				case 4:
					return LangVersion.GarrysMod;
				default:
					return LangVersion.Lua;
				}
			}
			set
			{
				switch (value)
				{
				case LangVersion.Lua:
					LanguageVersion.Active = 0;
					break;
				case LangVersion.Lua51:
					LanguageVersion.Active = 1;
					break;
				case LangVersion.Lua52:
					LanguageVersion.Active = 2;
					break;
				case LangVersion.LuaJIT:
					LanguageVersion.Active = 3;
					break;
				case LangVersion.GarrysMod:
					LanguageVersion.Active = 4;
					break;
				}
			}
		}

		public CompilerParametersPanelWidget()
		{
			Build();
			_VersionsStore = new ListStore(typeof(string), typeof(LangVersion));
			_VersionsStore.AppendValues("Default", LangVersion.Lua);
			_VersionsStore.AppendValues("Lua 5.1", LangVersion.Lua51);
			_VersionsStore.AppendValues("Lua 5.2", LangVersion.Lua52);
			_VersionsStore.AppendValues("LuaJIT", LangVersion.LuaJIT);
			_VersionsStore.AppendValues("Garry's Mod", LangVersion.GarrysMod);
			LanguageVersion.Model = _VersionsStore;
			base.Visible = true;
		}

		public void Load(DotNetProject project, DotNetProjectConfiguration configuration)
		{
			this.project = project;
			this.configuration = configuration;
		}

		public void Store()
		{
			project.CompileTarget = CompileTarget.Exe;
			configuration.DebugMode = false;
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			Stetic.BinContainer.Attach(this);
			base.Name = "LuaBinding.CompilerParametersPanelWidget";
			MainTable = new Table(2u, 2u, homogeneous: false);
			MainTable.Name = "MainTable";
			MainTable.RowSpacing = 6u;
			MainTable.ColumnSpacing = 6u;
			label1 = new Label();
			label1.Name = "label1";
			label1.Xalign = 0f;
			label1.LabelProp = Catalog.GetString("Lua version");
			MainTable.Add(label1);
			Table.TableChild tableChild = (Table.TableChild)MainTable[label1];
			tableChild.XOptions = AttachOptions.Fill;
			tableChild.YOptions = AttachOptions.Fill;
			label3 = new Label();
			label3.Name = "label3";
			label3.Xalign = 0f;
			label3.LabelProp = Catalog.GetString("Main file");
			MainTable.Add(label3);
			Table.TableChild tableChild2 = (Table.TableChild)MainTable[label3];
			tableChild2.TopAttach = 1u;
			tableChild2.BottomAttach = 2u;
			tableChild2.XOptions = AttachOptions.Fill;
			tableChild2.YOptions = AttachOptions.Fill;
			LanguageVersion = ComboBox.NewText();
			LanguageVersion.Name = "LanguageVersion";
			MainTable.Add(LanguageVersion);
			Table.TableChild tableChild3 = (Table.TableChild)MainTable[LanguageVersion];
			tableChild3.LeftAttach = 1u;
			tableChild3.RightAttach = 2u;
			tableChild3.XOptions = AttachOptions.Fill;
			tableChild3.YOptions = AttachOptions.Fill;
			MainFileEntry = new Entry();
			MainFileEntry.CanFocus = true;
			MainFileEntry.Name = "MainFileEntry";
			MainFileEntry.IsEditable = true;
			MainFileEntry.WidthChars = 22;
			MainFileEntry.InvisibleChar = '●';
			MainTable.Add(MainFileEntry);
			Table.TableChild tableChild4 = (Table.TableChild)MainTable[MainFileEntry];
			tableChild4.TopAttach = 1u;
			tableChild4.BottomAttach = 2u;
			tableChild4.LeftAttach = 1u;
			tableChild4.RightAttach = 2u;
			tableChild4.XOptions = AttachOptions.Fill;
			tableChild4.YOptions = AttachOptions.Fill;
			Add(MainTable);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			Hide();
		}
	}
}
