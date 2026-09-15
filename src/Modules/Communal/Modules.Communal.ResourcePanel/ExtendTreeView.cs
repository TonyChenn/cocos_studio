using System;
using System.Collections.Generic;
using System.Linq;
using Gdk;
using Gtk;
using MonoDevelop.Components;

namespace Modules.Communal.ResourcePanel
{
	public class ExtendTreeView : ContextMenuTreeView
	{
		public TreeModel CurrentModel
		{
			get
			{
				if (this.Filter == null)
				{
					return base.Model;
				}
				if (this.IsSearchState)
				{
					return this.Filter;
				}
				return this.Filter.ChildModel;
			}
		}

		public event EventHandler<WidgetEventArgs> OnMouseClick;

		public event EventHandler<WidgetEventArgs> OnMouseDoubleClick;

		public bool IsSearchState { get; set; }

		public event EventHandler OnMouseLeave;

		public ExtendTreeView()
		{
			base.WidgetEvent += this.RTreeView_WidgetEvent;
		}

		private void RTreeView_WidgetEvent(object o, WidgetEventArgs args)
		{
			EventType type = args.Event.Type;
			if (type != EventType.ButtonPress)
			{
				if (type != EventType.LeaveNotify)
				{
					return;
				}
				if (this.OnMouseLeave != null)
				{
					this.OnMouseLeave(this, EventArgs.Empty);
				}
				return;
			}
			else
			{
				EventButton eventButton = (EventButton)args.Event;
				TreePath treePath;
				if (this.IsClickToggleButton(eventButton, out treePath))
				{
					if (eventButton.IsDoubleClick(1U))
					{
						this.OnMouseDoubleClickHandle(args);
						return;
					}
					this.OnMouseButtonDownHandle(args);
					return;
				}
				else
				{
					if (treePath == null)
					{
						return;
					}
					bool rowExpanded = base.GetRowExpanded(treePath);
					if (rowExpanded)
					{
						base.CollapseRow(treePath);
						return;
					}
					base.ExpandToPath(treePath);
					return;
				}
			}
		}

		protected void OnMouseButtonDownHandle(WidgetEventArgs args)
		{
			if (this.OnMouseClick != null)
			{
				this.OnMouseClick(this, args);
			}
		}

		protected void OnMouseDoubleClickHandle(WidgetEventArgs args)
		{
			if (this.OnMouseDoubleClick != null)
			{
				this.OnMouseDoubleClick(this, args);
			}
		}

		private bool IsClickToggleButton(EventButton evnt)
		{
			TreePath treePath;
			TreeViewDropPosition treeViewDropPosition;
			return base.GetDestRowAtPos((int)evnt.X, (int)evnt.Y, out treePath, out treeViewDropPosition) && ((double)((treePath.Depth - 1) * this.toggleButtonWidth) >= evnt.X || evnt.X >= (double)(treePath.Depth * this.toggleButtonWidth));
		}

		protected override bool OnDragMotion(DragContext context, int x, int y, uint time_)
		{
			return base.OnDragMotion(context, x, y, time_);
		}

		protected override bool OnDragDrop(DragContext context, int x, int y, uint time_)
		{
			CellRenderHelper.HoverdItem = null;
			return base.OnDragDrop(context, x, y, time_);
		}

		protected override void OnDragLeave(DragContext context, uint time_)
		{
			CellRenderHelper.HoverdItem = null;
			base.OnDragLeave(context, time_);
		}

		protected override bool OnLeaveNotifyEvent(EventCrossing evnt)
		{
			CellRenderHelper.HoverdItem = null;
			return base.OnLeaveNotifyEvent(evnt);
		}

		private bool IsClickToggleButton(EventButton evnt, out TreePath treePath)
		{
			TreeViewDropPosition treeViewDropPosition;
			return base.GetDestRowAtPos((int)evnt.X, (int)evnt.Y, out treePath, out treeViewDropPosition) && ((double)((treePath.Depth - 1) * this.toggleButtonWidth) >= evnt.X || evnt.X >= (double)(treePath.Depth * this.toggleButtonWidth));
		}

		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			if (!this.ModifierIsNone)
			{
				int x = (int)evnt.X;
				int y = (int)evnt.Y;
				TreePath treePath;
				base.GetPathAtPos(x, y, out treePath);
				if (treePath == null)
				{
					base.Selection.UnselectAll();
				}
			}
			return this.IsClickToggleButton(evnt) && base.OnButtonPressEvent(evnt);
		}

		protected override bool OnKeyPressEvent(EventKey evnt)
		{
			if (evnt.State != ModifierType.None)
			{
				this.ModifierIsNone = true;
			}
			return true;
		}

		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			if (evnt.State == ModifierType.None)
			{
				this.ModifierIsNone = false;
			}
			return true;
		}

		public TreeModelFilter Filter { get; set; }

		public void SetSelectes(IEnumerable<TreePath> selectesPath)
		{
			if (selectesPath == null)
			{
				return;
			}
			List<TreePath> paths = selectesPath.ToList<TreePath>();
			base.Selection.UnselectAll();
			if (paths.Count == 0)
			{
				return;
			}
			base.Selection.SelectFunction = ((TreeSelection selection, TreeModel model, TreePath path, bool path_currently_selected) => paths.Contains(path));
			base.ScrollToCell(paths[0], base.Columns.FirstOrDefault<TreeViewColumn>(), false, 0.5f, 0.5f);
			base.Selection.SelectAll();
			base.Selection.SelectFunction = ((TreeSelection selection, TreeModel model, TreePath path, bool path_currently_selected) => true);
		}

		private int toggleButtonWidth = 14;

		private bool ModifierIsNone;
	}
}
