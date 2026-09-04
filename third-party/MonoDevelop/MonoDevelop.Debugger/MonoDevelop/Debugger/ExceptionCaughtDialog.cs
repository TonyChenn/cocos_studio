using System;
using System.IO;
using System.Linq;
using Gdk;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using Xwt.Drawing;

namespace MonoDevelop.Debugger
{
	internal class ExceptionCaughtDialog : Dialog
	{
		protected enum ModelColumn
		{
			StackFrame,
			Markup,
			IsUserCode
		}

		private static readonly Xwt.Drawing.Image WarningIconPixbuf = Xwt.Drawing.Image.FromResource("exception-48.png");

		private readonly ExceptionCaughtMessage message;

		private readonly ExceptionInfo exception;

		private ExceptionInfo selected;

		private bool destroyed;

		protected ObjectValueTreeView ExceptionValueTreeView { get; private set; }

		protected TreeView StackTraceTreeView { get; private set; }

		protected CheckButton OnlyShowMyCodeCheckbox { get; private set; }

		protected Label ExceptionMessageLabel { get; private set; }

		protected Label ExceptionTypeLabel { get; private set; }

		public ExceptionCaughtDialog(ExceptionInfo ex, ExceptionCaughtMessage msg)
		{
			selected = (exception = ex);
			message = msg;
			Build();
			UpdateDisplay();
			exception.Changed += ExceptionChanged;
		}

		private Widget CreateExceptionInfoHeader()
		{
			ExceptionMessageLabel = new Label
			{
				UseMarkup = true,
				Selectable = true,
				Wrap = true,
				WidthRequest = 500,
				Xalign = 0f,
				Yalign = 0f
			};
			ExceptionTypeLabel = new Label
			{
				UseMarkup = true,
				Xalign = 0f
			};
			ExceptionMessageLabel.Show();
			ExceptionTypeLabel.Show();
			Alignment alignment = new Alignment(0f, 0f, 0f, 0f);
			alignment.Child = ExceptionMessageLabel;
			alignment.BorderWidth = 6u;
			alignment.Show();
			InfoFrame infoFrame = new InfoFrame(alignment);
			infoFrame.Show();
			VBox vBox = new VBox(homogeneous: false, 12);
			vBox.PackStart(ExceptionTypeLabel, expand: false, fill: true, 0u);
			vBox.PackStart(infoFrame, expand: true, fill: true, 0u);
			vBox.Show();
			return vBox;
		}

		private Widget CreateExceptionHeader()
		{
			ImageView imageView = new ImageView(WarningIconPixbuf);
			imageView.Show();
			HBox hBox = new HBox(homogeneous: false, 12);
			hBox.BorderWidth = 12u;
			HBox hBox2 = hBox;
			hBox2.PackStart(imageView, expand: false, fill: true, 0u);
			hBox2.PackStart(CreateExceptionInfoHeader(), expand: true, fill: true, 0u);
			hBox2.Show();
			return hBox2;
		}

		private Widget CreateExceptionValueTreeView()
		{
			ExceptionValueTreeView = new ObjectValueTreeView();
			ExceptionValueTreeView.Frame = DebuggingService.CurrentFrame;
			ExceptionValueTreeView.ModifyBase(StateType.Normal, new Gdk.Color(223, 228, 235));
			ExceptionValueTreeView.AllowPopupMenu = false;
			ExceptionValueTreeView.AllowExpanding = true;
			ExceptionValueTreeView.AllowPinning = false;
			ExceptionValueTreeView.AllowEditing = false;
			ExceptionValueTreeView.AllowAdding = false;
			ExceptionValueTreeView.RulesHint = false;
			ExceptionValueTreeView.Selection.Changed += ExceptionValueSelectionChanged;
			ExceptionValueTreeView.Show();
			ScrolledWindow scrolledWindow = new ScrolledWindow();
			scrolledWindow.HeightRequest = 180;
			scrolledWindow.HscrollbarPolicy = PolicyType.Automatic;
			scrolledWindow.VscrollbarPolicy = PolicyType.Automatic;
			ScrolledWindow scrolledWindow2 = scrolledWindow;
			scrolledWindow2.ShadowType = ShadowType.None;
			scrolledWindow2.Add(ExceptionValueTreeView);
			scrolledWindow2.Show();
			return scrolledWindow2;
		}

		private static void StackFrameLayout(CellLayout layout, CellRenderer cr, TreeModel model, TreeIter iter)
		{
			ExceptionStackFrame exceptionStackFrame = (ExceptionStackFrame)model.GetValue(iter, 0);
			StackFrameCellRenderer stackFrameCellRenderer = (StackFrameCellRenderer)cr;
			stackFrameCellRenderer.Markup = (string)model.GetValue(iter, 1);
			stackFrameCellRenderer.Frame = exceptionStackFrame;
			if (exceptionStackFrame == null)
			{
				stackFrameCellRenderer.IsUserCode = false;
			}
			else
			{
				stackFrameCellRenderer.IsUserCode = (bool)model.GetValue(iter, 2);
			}
		}

		private Widget CreateStackTraceTreeView()
		{
			ListStore model = new ListStore(typeof(ExceptionStackFrame), typeof(string), typeof(bool));
			StackTraceTreeView = new TreeView(model);
			StackTraceTreeView.FixedHeightMode = false;
			StackTraceTreeView.HeadersVisible = false;
			StackTraceTreeView.ShowExpanders = false;
			StackTraceTreeView.RulesHint = true;
			StackTraceTreeView.Show();
			StackFrameCellRenderer renderer = new StackFrameCellRenderer(StackTraceTreeView.PangoContext);
			renderer.Width = base.DefaultWidth;
			StackTraceTreeView.AppendColumn("", (CellRenderer)renderer, (CellLayoutDataFunc)StackFrameLayout);
			StackTraceTreeView.SizeAllocated += delegate(object o, SizeAllocatedArgs args)
			{
				renderer.Width = args.Allocation.Width;
			};
			StackTraceTreeView.RowActivated += StackFrameActivated;
			ScrolledWindow scrolledWindow = new ScrolledWindow();
			scrolledWindow.HeightRequest = 180;
			scrolledWindow.HscrollbarPolicy = PolicyType.Automatic;
			scrolledWindow.VscrollbarPolicy = PolicyType.Automatic;
			ScrolledWindow scrolledWindow2 = scrolledWindow;
			scrolledWindow2.ShadowType = ShadowType.None;
			scrolledWindow2.Add(StackTraceTreeView);
			scrolledWindow2.Show();
			return scrolledWindow2;
		}

		private Widget CreateButtonBox()
		{
			HButtonBox hButtonBox = new HButtonBox();
			hButtonBox.Layout = ButtonBoxStyle.End;
			hButtonBox.Spacing = 12;
			HButtonBox hButtonBox2 = hButtonBox;
			Button button = new Button(Gtk.Stock.Copy);
			button.Clicked += CopyClicked;
			button.Show();
			hButtonBox2.PackStart(button, expand: false, fill: true, 0u);
			Button button2 = new Button(Gtk.Stock.Close);
			button2.Activated += CloseClicked;
			button2.Clicked += CloseClicked;
			button2.Show();
			hButtonBox2.PackStart(button2, expand: false, fill: true, 0u);
			hButtonBox2.Show();
			return hButtonBox2;
		}

		private static Widget CreateSeparator()
		{
			HSeparator hSeparator = new HSeparator();
			hSeparator.Show();
			return hSeparator;
		}

		private void Build()
		{
			base.Title = GettextCatalog.GetString("Exception Caught");
			base.DefaultHeight = 500;
			base.DefaultWidth = 600;
			base.VBox.Spacing = 0;
			base.VBox.PackStart(CreateExceptionHeader(), expand: false, fill: true, 0u);
			VPaned vPaned = new VPaned();
			vPaned.Add1(CreateExceptionValueTreeView());
			vPaned.Add2(CreateStackTraceTreeView());
			vPaned.Show();
			VBox vBox = new VBox(homogeneous: false, 0);
			vBox.PackStart(CreateSeparator(), expand: false, fill: true, 0u);
			vBox.PackStart(vPaned, expand: true, fill: true, 0u);
			vBox.PackStart(CreateSeparator(), expand: false, fill: true, 0u);
			vBox.Show();
			base.VBox.PackStart(vBox, expand: true, fill: true, 0u);
			HBox hBox = new HBox(homogeneous: false, 12);
			hBox.BorderWidth = 6u;
			HBox hBox2 = hBox;
			OnlyShowMyCodeCheckbox = new CheckButton(GettextCatalog.GetString("_Only show my code."));
			OnlyShowMyCodeCheckbox.Toggled += OnlyShowMyCodeToggled;
			OnlyShowMyCodeCheckbox.Show();
			OnlyShowMyCodeCheckbox.Active = DebuggingService.GetUserOptions().ProjectAssembliesOnly;
			Alignment alignment = new Alignment(0f, 0.5f, 0f, 0f);
			alignment.Child = OnlyShowMyCodeCheckbox;
			Alignment alignment2 = alignment;
			alignment2.Show();
			hBox2.PackStart(alignment2, expand: true, fill: true, 0u);
			hBox2.PackStart(CreateButtonBox(), expand: false, fill: true, 0u);
			hBox2.Show();
			base.VBox.PackStart(hBox2, expand: false, fill: true, 0u);
			base.ActionArea.Hide();
		}

		private bool TryGetExceptionInfo(TreePath path, out ExceptionInfo ex)
		{
			TreeStore treeStore = (TreeStore)ExceptionValueTreeView.Model;
			ex = exception;
			if (!treeStore.GetIter(out var iter, path))
			{
				return false;
			}
			ObjectValue objectValue = (ObjectValue)treeStore.GetValue(iter, 3);
			if (objectValue.Name != "InnerException")
			{
				return false;
			}
			int num = 0;
			TreeIter iter2;
			while (treeStore.IterParent(out iter2, iter))
			{
				iter = iter2;
				num++;
			}
			while (ex != null)
			{
				if (num == 0)
				{
					return true;
				}
				ex = ex.InnerException;
				num--;
			}
			return false;
		}

		private void ExceptionValueSelectionChanged(object sender, EventArgs e)
		{
			TreePath[] selectedRows = ExceptionValueTreeView.Selection.GetSelectedRows();
			if (selectedRows.Length > 0 && TryGetExceptionInfo(selectedRows[0], out var ex))
			{
				ShowStackTrace(ex);
				selected = ex;
			}
			else if (selected != exception)
			{
				ShowStackTrace(exception);
				selected = exception;
			}
		}

		private void StackFrameActivated(object o, RowActivatedArgs args)
		{
			TreeModel model = StackTraceTreeView.Model;
			if (!model.GetIter(out var iter, args.Path))
			{
				return;
			}
			ExceptionStackFrame exceptionStackFrame = (ExceptionStackFrame)model.GetValue(iter, 0);
			if (exceptionStackFrame == null || string.IsNullOrEmpty(exceptionStackFrame.File) || !File.Exists(exceptionStackFrame.File))
			{
				return;
			}
			try
			{
				IdeApp.Workbench.OpenDocument(exceptionStackFrame.File, null, exceptionStackFrame.Line, exceptionStackFrame.Column, OpenDocumentOptions.Debugger);
			}
			catch (FileNotFoundException)
			{
			}
		}

		private static bool IsUserCode(ExceptionStackFrame frame)
		{
			if (frame == null || string.IsNullOrEmpty(frame.File))
			{
				return false;
			}
			return IdeApp.Workspace.GetProjectsContainingFile(frame.File).Any();
		}

		private void ShowStackTrace(ExceptionInfo ex)
		{
			ListStore listStore = (ListStore)StackTraceTreeView.Model;
			bool flag = false;
			listStore.Clear();
			ExceptionStackFrame[] stackTrace = ex.StackTrace;
			foreach (ExceptionStackFrame exceptionStackFrame in stackTrace)
			{
				bool flag2 = IsUserCode(exceptionStackFrame);
				if (OnlyShowMyCodeCheckbox.Active && !flag2)
				{
					if (!flag)
					{
						string text = GettextCatalog.GetString("<b>[External Code]</b>");
						listStore.AppendValues(null, text, false);
						flag = true;
					}
				}
				else
				{
					listStore.AppendValues(exceptionStackFrame, null, flag2);
					flag = false;
				}
			}
			if (ex.StackIsEvaluating)
			{
				string text2 = GettextCatalog.GetString("Loading...");
				listStore.AppendValues(null, text2, false);
			}
		}

		private void UpdateDisplay()
		{
			if (!destroyed)
			{
				ExceptionValueTreeView.ClearValues();
				ExceptionTypeLabel.Markup = GettextCatalog.GetString("A <b>{0}</b> was thrown.", exception.Type);
				ExceptionMessageLabel.Markup = "<small>" + (exception.Message ?? string.Empty) + "</small>";
				if (!exception.IsEvaluating && exception.Instance != null)
				{
					ExceptionValueTreeView.AddValue(exception.Instance);
					ExceptionValueTreeView.ExpandRow(new TreePath("0"), open_all: false);
				}
				ShowStackTrace(exception);
			}
		}

		private void ExceptionChanged(object sender, EventArgs e)
		{
			Application.Invoke(delegate
			{
				UpdateDisplay();
			});
		}

		private void OnlyShowMyCodeToggled(object sender, EventArgs e)
		{
			ShowStackTrace(selected);
		}

		private void CloseClicked(object sender, EventArgs e)
		{
			message.Close();
		}

		private void CopyClicked(object sender, EventArgs e)
		{
			string text = exception.ToString();
			Clipboard clipboard = Clipboard.Get(Atom.Intern("CLIPBOARD", only_if_exists: false));
			clipboard.Text = text;
			Clipboard clipboard2 = Clipboard.Get(Atom.Intern("PRIMARY", only_if_exists: false));
			clipboard2.Text = text;
		}

		protected override bool OnDeleteEvent(Event evnt)
		{
			message.Close();
			return true;
		}

		protected override void OnDestroyed()
		{
			destroyed = true;
			exception.Changed -= ExceptionChanged;
			base.OnDestroyed();
		}
	}
}
