using System;
using Gdk;
using Gtk;
using Mono.Debugging.Client;
using Mono.TextEditor;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Debugger;
using MonoDevelop.Ide;

namespace MonoDevelop.SourceEditor
{
	public class DebugValueWindow : PopoverWindow
	{
		private ObjectValueTreeView tree;

		private ScrolledWindow sw;

		public DebugValueWindow(TextEditor editor, int offset, StackFrame frame, ObjectValue value, PinnedWatch watch)
			: base(Gtk.WindowType.Toplevel)
		{
			base.TypeHint = WindowTypeHint.PopupMenu;
			base.AllowShrink = false;
			base.AllowGrow = false;
			base.Decorated = false;
			base.TransientFor = (Gtk.Window)editor.Toplevel;
			base.AcceptFocus = false;
			sw = new ScrolledWindow();
			sw.HscrollbarPolicy = PolicyType.Never;
			sw.VscrollbarPolicy = PolicyType.Never;
			tree = new ObjectValueTreeView();
			sw.Add(tree);
			base.ContentBox.Add(sw);
			tree.Frame = frame;
			tree.CompactView = true;
			tree.AllowAdding = false;
			tree.AllowEditing = true;
			tree.HeadersVisible = false;
			tree.AllowPinning = true;
			tree.RootPinAlwaysVisible = true;
			tree.PinnedWatch = watch;
			DocumentLocation documentLocation = editor.Document.OffsetToLocation(offset);
			tree.PinnedWatchLine = documentLocation.Line;
			tree.PinnedWatchFile = ((ExtensibleTextEditor)editor).View.ContentName;
			tree.AddValue(value);
			tree.Selection.UnselectAll();
			tree.SizeAllocated += OnTreeSizeChanged;
			ObjectValueTreeView objectValueTreeView = tree;
			EventHandler value2 = delegate
			{
				Destroy();
			};
			objectValueTreeView.PinStatusChanged += value2;
			sw.ShowAll();
			tree.StartEditing += delegate
			{
				base.Modal = true;
			};
			tree.EndEditing += delegate
			{
				base.Modal = false;
			};
			base.ShowArrow = true;
			base.Theme.CornerRadius = 3;
		}

		protected override bool OnEnterNotifyEvent(EventCrossing evnt)
		{
			if (!base.AcceptFocus)
			{
				base.AcceptFocus = true;
			}
			return base.OnEnterNotifyEvent(evnt);
		}

		private void OnTreeSizeChanged(object s, SizeAllocatedArgs a)
		{
			GetPosition(out var root_x, out var root_y);
			int num = (int)sw.Vadjustment.Upper;
			int num2 = (int)sw.Hadjustment.Upper;
			int num3 = root_y + num - base.Screen.Height;
			int num4 = root_x + num2 - base.Screen.Width;
			if (num3 > 0 && sw.VscrollbarPolicy == PolicyType.Never)
			{
				sw.VscrollbarPolicy = PolicyType.Always;
				sw.HeightRequest = num - num3 - 10;
			}
			else if (sw.VscrollbarPolicy == PolicyType.Always && sw.Vadjustment.Upper == sw.Vadjustment.PageSize)
			{
				sw.VscrollbarPolicy = PolicyType.Never;
				sw.HeightRequest = -1;
			}
			if (num4 > 0 && sw.HscrollbarPolicy == PolicyType.Never)
			{
				sw.HscrollbarPolicy = PolicyType.Always;
				sw.WidthRequest = num2 - num4 - 10;
			}
			else if (sw.HscrollbarPolicy == PolicyType.Always && sw.Hadjustment.Upper == sw.Hadjustment.PageSize)
			{
				sw.HscrollbarPolicy = PolicyType.Never;
				sw.WidthRequest = -1;
			}
			QueueDraw();
		}

		protected override void OnSizeAllocated(Rectangle allocation)
		{
			if (Platform.IsMac || Platform.IsWindows)
			{
				GetPosition(out var root_x, out var root_y);
				int num = root_y;
				Rectangle usableMonitorGeometry = DesktopService.GetUsableMonitorGeometry(base.Screen, base.Screen.GetMonitorAtPoint(root_x, root_y));
				if (allocation.Height <= usableMonitorGeometry.Height && root_y + allocation.Height >= usableMonitorGeometry.Y + usableMonitorGeometry.Height - 2)
				{
					root_y = usableMonitorGeometry.Top + (usableMonitorGeometry.Height - allocation.Height - 2);
				}
				if (root_y < usableMonitorGeometry.Top + 2)
				{
					root_y = usableMonitorGeometry.Top + 2;
				}
				if (root_y != num)
				{
					Move(root_x, root_y);
					base.ShowArrow = false;
				}
			}
			base.OnSizeAllocated(allocation);
		}
	}
}
