using System;
using GLib;
using Gdk;
using Gtk;
using Mono.Debugging.Client;
using Mono.TextEditor;
using MonoDevelop.Debugger;

namespace MonoDevelop.SourceEditor
{
	public class PinnedWatchWidget : EventBox
	{
		private const int defaultMaxHeight = 240;

		private readonly ObjectValueTreeView valueTree;

		private ObjectValue objectValue;

		private bool mousePressed;

		private double originX;

		private double originY;

		private TextEditor Editor { get; set; }

		public PinnedWatch Watch { get; private set; }

		public ObjectValue ObjectValue
		{
			get
			{
				return objectValue;
			}
			set
			{
				if (objectValue == value)
				{
					return;
				}
				if (objectValue != null && value != null)
				{
					valueTree.ReplaceValue(objectValue, value);
				}
				else
				{
					valueTree.ClearValues();
					if (value != null)
					{
						valueTree.AddValue(value);
					}
				}
				objectValue = value;
			}
		}

		public PinnedWatchWidget(TextEditor editor, PinnedWatch watch)
		{
			objectValue = watch.Value;
			Editor = editor;
			Watch = watch;
			valueTree = new ObjectValueTreeView();
			valueTree.AllowAdding = false;
			valueTree.AllowEditing = true;
			valueTree.AllowPinning = true;
			valueTree.HeadersVisible = false;
			valueTree.CompactView = true;
			valueTree.PinnedWatch = watch;
			if (objectValue != null)
			{
				valueTree.AddValue(objectValue);
			}
			valueTree.ButtonPressEvent += HandleValueTreeButtonPressEvent;
			valueTree.ButtonReleaseEvent += HandleValueTreeButtonReleaseEvent;
			valueTree.MotionNotifyEvent += HandleValueTreeMotionNotifyEvent;
			Frame frame = new Frame();
			frame.ShadowType = ShadowType.Out;
			frame.Add(valueTree);
			Add(frame);
			HandleEditorOptionsChanged(null, null);
			ShowAll();
			Editor.EditorOptionsChanged += HandleEditorOptionsChanged;
			DebuggingService.PausedEvent += HandleDebuggingServicePausedEvent;
			DebuggingService.ResumedEvent += HandleDebuggingServiceResumedEvent;
		}

		private void HandleDebuggingServiceResumedEvent(object sender, EventArgs e)
		{
			valueTree.ChangeCheckpoint();
			valueTree.AllowEditing = false;
			valueTree.AllowExpanding = false;
		}

		private void HandleDebuggingServicePausedEvent(object sender, EventArgs e)
		{
			valueTree.AllowExpanding = true;
			valueTree.AllowEditing = true;
		}

		private void HandleEditorOptionsChanged(object sender, EventArgs e)
		{
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();
			Editor.EditorOptionsChanged -= HandleEditorOptionsChanged;
			DebuggingService.PausedEvent -= HandleDebuggingServicePausedEvent;
			DebuggingService.ResumedEvent -= HandleDebuggingServiceResumedEvent;
		}

		protected override void OnSizeAllocated(Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);
		}

		protected override void OnSizeRequested(ref Requisition requisition)
		{
			base.OnSizeRequested(ref requisition);
			requisition.Height = Math.Min(Math.Max(base.Allocation.Height, 240), requisition.Height);
		}

		[ConnectBefore]
		private void HandleValueTreeButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			originX = args.Event.XRoot;
			originY = args.Event.YRoot;
			valueTree.GetPathAtPos((int)args.Event.X, (int)args.Event.Y, out var path, out var column, out var cell_x, out var _);
			Rectangle cellArea = valueTree.GetCellArea(path, column);
			if (!mousePressed && valueTree.Columns[0] == column && cell_x >= cellArea.Left)
			{
				mousePressed = true;
				Editor.MoveToTop(this);
			}
		}

		[ConnectBefore]
		private void HandleValueTreeButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (mousePressed)
			{
				mousePressed = false;
			}
		}

		[ConnectBefore]
		private void HandleValueTreeMotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			if (mousePressed)
			{
				Watch.OffsetX += (int)(args.Event.XRoot - originX);
				Watch.OffsetY += (int)(args.Event.YRoot - originY);
				originX = args.Event.XRoot;
				originY = args.Event.YRoot;
			}
		}

		protected override bool OnEnterNotifyEvent(EventCrossing evnt)
		{
			return base.OnEnterNotifyEvent(evnt);
		}

		protected override bool OnLeaveNotifyEvent(EventCrossing evnt)
		{
			return base.OnLeaveNotifyEvent(evnt);
		}
	}
}
