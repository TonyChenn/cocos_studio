using System;
using System.Collections.Generic;
using GLib;
using Gtk;
using Mono.Debugging.Client;
using Mono.Unix;
using MonoDevelop.Components;
using MonoDevelop.Core;
using Stetic;

namespace MonoDevelop.Debugger
{
	public class AttachToProcessDialog : Dialog
	{
		private List<DebuggerEngine> currentDebEngines;

		private Dictionary<long, List<DebuggerEngine>> procEngines;

		private List<ProcessInfo> procs;

		private ListStore store;

		private TreeViewState state;

		private uint timeoutHandler;

		private VBox vbox2;

		private HBox hbox1;

		private Label label1;

		private Entry entryFilter;

		private ScrolledWindow GtkScrolledWindow;

		private TreeView tree;

		private HBox hbox2;

		private Label label2;

		private ComboBox comboDebs;

		private Button buttonCancel;

		private Button buttonOk;

		public ProcessInfo SelectedProcess
		{
			get
			{
				tree.Selection.GetSelected(out var iter);
				return (ProcessInfo)store.GetValue(iter, 0);
			}
		}

		public DebuggerEngine SelectedDebugger => currentDebEngines[comboDebs.Active];

		public AttachToProcessDialog()
		{
			Build();
			store = new ListStore(typeof(ProcessInfo), typeof(string), typeof(string));
			tree.Model = store;
			tree.AppendColumn("PID", new CellRendererText(), "text", 1);
			tree.AppendColumn("Process Name", new CellRendererText(), "text", 2);
			state = new TreeViewState(tree, 1);
			Refresh();
			comboDebs.Sensitive = false;
			buttonOk.Sensitive = false;
			tree.Selection.UnselectAll();
			tree.Selection.Changed += OnSelectionChanged;
			if (store.GetIterFirst(out var iter))
			{
				tree.Selection.SelectIter(iter);
			}
			timeoutHandler = GLib.Timeout.Add(3000u, Refresh);
		}

		public override void Destroy()
		{
			if (timeoutHandler != 0)
			{
				Source.Remove(timeoutHandler);
			}
			base.Destroy();
		}

		private bool Refresh()
		{
			procEngines = new Dictionary<long, List<DebuggerEngine>>();
			procs = new List<ProcessInfo>();
			DebuggerEngine[] debuggerEngines = DebuggingService.GetDebuggerEngines();
			foreach (DebuggerEngine debuggerEngine in debuggerEngines)
			{
				if ((debuggerEngine.SupportedFeatures & DebuggerFeatures.Attaching) == 0)
				{
					continue;
				}
				try
				{
					ProcessInfo[] attachableProcesses = debuggerEngine.GetAttachableProcesses();
					ProcessInfo[] array = attachableProcesses;
					foreach (ProcessInfo processInfo in array)
					{
						if (!procEngines.TryGetValue(processInfo.Id, out var value))
						{
							value = new List<DebuggerEngine>();
							procEngines[processInfo.Id] = value;
							procs.Add(processInfo);
						}
						value.Add(debuggerEngine);
					}
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Could not get attachable processes.", ex);
				}
				comboDebs.AppendText(debuggerEngine.Name);
			}
			FillList();
			return true;
		}

		private void FillList()
		{
			state.Save();
			store.Clear();
			string text = entryFilter.Text;
			foreach (ProcessInfo proc in procs)
			{
				if (text.Length == 0 || proc.Id.ToString().Contains(text) || proc.Name.Contains(text))
				{
					store.AppendValues(proc, proc.Id.ToString(), proc.Name);
				}
			}
			state.Load();
			if (tree.Selection.CountSelectedRows() == 0 && store.GetIterFirst(out var iter))
			{
				tree.Selection.SelectIter(iter);
			}
		}

		private void OnSelectionChanged(object s, EventArgs args)
		{
			((ListStore)comboDebs.Model).Clear();
			if (tree.Selection.GetSelected(out var iter))
			{
				ProcessInfo processInfo = (ProcessInfo)store.GetValue(iter, 0);
				currentDebEngines = procEngines[processInfo.Id];
				foreach (DebuggerEngine currentDebEngine in currentDebEngines)
				{
					comboDebs.AppendText(currentDebEngine.Name);
				}
				comboDebs.Sensitive = true;
				buttonOk.Sensitive = currentDebEngines.Count > 0;
				comboDebs.Active = 0;
			}
			else
			{
				comboDebs.Sensitive = false;
				buttonOk.Sensitive = false;
			}
		}

		protected virtual void OnEntryFilterChanged(object sender, EventArgs e)
		{
			FillList();
		}

		protected virtual void OnRowActivated(object o, RowActivatedArgs args)
		{
			Respond(ResponseType.Ok);
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			base.Name = "MonoDevelop.Debugger.AttachToProcessDialog";
			base.Title = Catalog.GetString("Attach to Process");
			base.WindowPosition = WindowPosition.CenterOnParent;
			base.BorderWidth = 3u;
			VBox vBox = base.VBox;
			vBox.Name = "dialog1_VBox";
			vBox.BorderWidth = 2u;
			vbox2 = new VBox();
			vbox2.Name = "vbox2";
			vbox2.Spacing = 12;
			vbox2.BorderWidth = 9u;
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 6;
			label1 = new Label();
			label1.Name = "label1";
			label1.Xalign = 0f;
			label1.LabelProp = Catalog.GetString("Attach to:");
			hbox1.Add(label1);
			Box.BoxChild boxChild = (Box.BoxChild)hbox1[label1];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			entryFilter = new Entry();
			entryFilter.CanFocus = true;
			entryFilter.Name = "entryFilter";
			entryFilter.IsEditable = true;
			entryFilter.InvisibleChar = '●';
			hbox1.Add(entryFilter);
			Box.BoxChild boxChild2 = (Box.BoxChild)hbox1[entryFilter];
			boxChild2.Position = 1;
			vbox2.Add(hbox1);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox2[hbox1];
			boxChild3.Position = 0;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			GtkScrolledWindow = new ScrolledWindow();
			GtkScrolledWindow.Name = "GtkScrolledWindow";
			GtkScrolledWindow.ShadowType = ShadowType.In;
			tree = new TreeView();
			tree.CanFocus = true;
			tree.Name = "tree";
			GtkScrolledWindow.Add(tree);
			vbox2.Add(GtkScrolledWindow);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox2[GtkScrolledWindow];
			boxChild4.Position = 1;
			hbox2 = new HBox();
			hbox2.Name = "hbox2";
			hbox2.Spacing = 6;
			label2 = new Label();
			label2.Name = "label2";
			label2.LabelProp = Catalog.GetString("Debugger:");
			hbox2.Add(label2);
			Box.BoxChild boxChild5 = (Box.BoxChild)hbox2[label2];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			comboDebs = ComboBox.NewText();
			comboDebs.Name = "comboDebs";
			hbox2.Add(comboDebs);
			Box.BoxChild boxChild6 = (Box.BoxChild)hbox2[comboDebs];
			boxChild6.Position = 1;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			vbox2.Add(hbox2);
			Box.BoxChild boxChild7 = (Box.BoxChild)vbox2[hbox2];
			boxChild7.Position = 2;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			vBox.Add(vbox2);
			Box.BoxChild boxChild8 = (Box.BoxChild)vBox[vbox2];
			boxChild8.Position = 0;
			HButtonBox actionArea = base.ActionArea;
			actionArea.Name = "dialog1_ActionArea";
			actionArea.Spacing = 6;
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
			buttonOk.UseUnderline = true;
			buttonOk.Label = Catalog.GetString("Attach");
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
			base.DefaultHeight = 413;
			Hide();
			entryFilter.Changed += OnEntryFilterChanged;
		}
	}
}
