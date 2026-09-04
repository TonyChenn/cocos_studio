using System.ComponentModel;
using Gtk;
using Mono.Unix;
using Stetic;

namespace CocoStudio.LuaBinding
{
	[ToolboxItem(true)]
	public class InterpreterOptions : Bin
	{
		private Table MainTabel;

		private Entry Interpreter51;

		private Entry Interpreter52;

		private Entry InterpreterDefault;

		private Entry InterpreterJIT;

		private Label label4;

		private Label label5;

		private Label label6;

		private Label label7;

		public string LuaDefault
		{
			get
			{
				return InterpreterDefault.Text ?? string.Empty;
			}
			set
			{
				InterpreterDefault.Text = value ?? string.Empty;
			}
		}

		public string Lua51
		{
			get
			{
				return Interpreter51.Text ?? string.Empty;
			}
			set
			{
				Interpreter51.Text = value ?? string.Empty;
			}
		}

		public string Lua52
		{
			get
			{
				return Interpreter52.Text ?? string.Empty;
			}
			set
			{
				Interpreter52.Text = value ?? string.Empty;
			}
		}

		public string LuaJIT
		{
			get
			{
				return InterpreterJIT.Text ?? string.Empty;
			}
			set
			{
				InterpreterJIT.Text = value ?? string.Empty;
			}
		}

		public InterpreterOptions()
		{
			Build();
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			Stetic.BinContainer.Attach(this);
			base.Name = "LuaBinding.InterpreterOptions";
			MainTabel = new Table(4u, 2u, homogeneous: false);
			MainTabel.Name = "MainTabel";
			MainTabel.RowSpacing = 6u;
			MainTabel.ColumnSpacing = 6u;
			Interpreter51 = new Entry();
			Interpreter51.CanFocus = true;
			Interpreter51.Name = "Interpreter51";
			Interpreter51.IsEditable = true;
			Interpreter51.InvisibleChar = '●';
			MainTabel.Add(Interpreter51);
			Table.TableChild tableChild = (Table.TableChild)MainTabel[Interpreter51];
			tableChild.TopAttach = 1u;
			tableChild.BottomAttach = 2u;
			tableChild.LeftAttach = 1u;
			tableChild.RightAttach = 2u;
			tableChild.YOptions = AttachOptions.Fill;
			Interpreter52 = new Entry();
			Interpreter52.CanFocus = true;
			Interpreter52.Name = "Interpreter52";
			Interpreter52.IsEditable = true;
			Interpreter52.InvisibleChar = '●';
			MainTabel.Add(Interpreter52);
			Table.TableChild tableChild2 = (Table.TableChild)MainTabel[Interpreter52];
			tableChild2.TopAttach = 2u;
			tableChild2.BottomAttach = 3u;
			tableChild2.LeftAttach = 1u;
			tableChild2.RightAttach = 2u;
			tableChild2.YOptions = AttachOptions.Fill;
			InterpreterDefault = new Entry();
			InterpreterDefault.CanFocus = true;
			InterpreterDefault.Name = "InterpreterDefault";
			InterpreterDefault.IsEditable = true;
			InterpreterDefault.InvisibleChar = '●';
			MainTabel.Add(InterpreterDefault);
			Table.TableChild tableChild3 = (Table.TableChild)MainTabel[InterpreterDefault];
			tableChild3.LeftAttach = 1u;
			tableChild3.RightAttach = 2u;
			tableChild3.YOptions = AttachOptions.Fill;
			InterpreterJIT = new Entry();
			InterpreterJIT.CanFocus = true;
			InterpreterJIT.Name = "InterpreterJIT";
			InterpreterJIT.IsEditable = true;
			InterpreterJIT.InvisibleChar = '●';
			MainTabel.Add(InterpreterJIT);
			Table.TableChild tableChild4 = (Table.TableChild)MainTabel[InterpreterJIT];
			tableChild4.TopAttach = 3u;
			tableChild4.BottomAttach = 4u;
			tableChild4.LeftAttach = 1u;
			tableChild4.RightAttach = 2u;
			tableChild4.YOptions = AttachOptions.Fill;
			label4 = new Label();
			label4.Name = "label4";
			label4.Xalign = 0f;
			label4.LabelProp = Catalog.GetString("Default interpreter");
			MainTabel.Add(label4);
			Table.TableChild tableChild5 = (Table.TableChild)MainTabel[label4];
			tableChild5.XOptions = AttachOptions.Fill;
			tableChild5.YOptions = AttachOptions.Fill;
			label5 = new Label();
			label5.Name = "label5";
			label5.Xalign = 0f;
			label5.LabelProp = Catalog.GetString("Lua 5.1 interpreter");
			MainTabel.Add(label5);
			Table.TableChild tableChild6 = (Table.TableChild)MainTabel[label5];
			tableChild6.TopAttach = 1u;
			tableChild6.BottomAttach = 2u;
			tableChild6.XOptions = AttachOptions.Fill;
			tableChild6.YOptions = AttachOptions.Fill;
			label6 = new Label();
			label6.Name = "label6";
			label6.Xalign = 0f;
			label6.LabelProp = Catalog.GetString("Lua 5.2 interpreter");
			MainTabel.Add(label6);
			Table.TableChild tableChild7 = (Table.TableChild)MainTabel[label6];
			tableChild7.TopAttach = 2u;
			tableChild7.BottomAttach = 3u;
			tableChild7.XOptions = AttachOptions.Fill;
			tableChild7.YOptions = AttachOptions.Fill;
			label7 = new Label();
			label7.Name = "label7";
			label7.Xalign = 0f;
			label7.LabelProp = Catalog.GetString("LuaJIT interpreter");
			MainTabel.Add(label7);
			Table.TableChild tableChild8 = (Table.TableChild)MainTabel[label7];
			tableChild8.TopAttach = 3u;
			tableChild8.BottomAttach = 4u;
			tableChild8.XOptions = AttachOptions.Fill;
			tableChild8.YOptions = AttachOptions.Fill;
			Add(MainTabel);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			Hide();
		}
	}
}
