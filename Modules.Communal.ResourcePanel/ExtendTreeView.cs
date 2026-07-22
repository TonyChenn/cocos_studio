using System;
using System.Collections.Generic;
using System.Linq;
using Gdk;
using Gtk;
using MonoDevelop.Components;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200002B RID: 43
	public class ExtendTreeView : ContextMenuTreeView
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00008D04 File Offset: 0x00006F04
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

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000193 RID: 403 RVA: 0x00008D30 File Offset: 0x00006F30
		// (remove) Token: 0x06000194 RID: 404 RVA: 0x00008D68 File Offset: 0x00006F68
		public event EventHandler<WidgetEventArgs> OnMouseClick;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000195 RID: 405 RVA: 0x00008DA0 File Offset: 0x00006FA0
		// (remove) Token: 0x06000196 RID: 406 RVA: 0x00008DD8 File Offset: 0x00006FD8
		public event EventHandler<WidgetEventArgs> OnMouseDoubleClick;

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00008E0D File Offset: 0x0000700D
		// (set) Token: 0x06000198 RID: 408 RVA: 0x00008E15 File Offset: 0x00007015
		public bool IsSearchState { get; set; }

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000199 RID: 409 RVA: 0x00008E20 File Offset: 0x00007020
		// (remove) Token: 0x0600019A RID: 410 RVA: 0x00008E58 File Offset: 0x00007058
		public event EventHandler OnMouseLeave;

		// Token: 0x0600019B RID: 411 RVA: 0x00008E8D File Offset: 0x0000708D
		public ExtendTreeView()
		{
			base.WidgetEvent += this.RTreeView_WidgetEvent;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008EB0 File Offset: 0x000070B0
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

		// Token: 0x0600019D RID: 413 RVA: 0x00008F3C File Offset: 0x0000713C
		protected void OnMouseButtonDownHandle(WidgetEventArgs args)
		{
			if (this.OnMouseClick != null)
			{
				this.OnMouseClick(this, args);
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008F53 File Offset: 0x00007153
		protected void OnMouseDoubleClickHandle(WidgetEventArgs args)
		{
			if (this.OnMouseDoubleClick != null)
			{
				this.OnMouseDoubleClick(this, args);
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008F6C File Offset: 0x0000716C
		private bool IsClickToggleButton(EventButton evnt)
		{
			TreePath treePath;
			TreeViewDropPosition treeViewDropPosition;
			return base.GetDestRowAtPos((int)evnt.X, (int)evnt.Y, out treePath, out treeViewDropPosition) && ((double)((treePath.Depth - 1) * this.toggleButtonWidth) >= evnt.X || evnt.X >= (double)(treePath.Depth * this.toggleButtonWidth));
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00008FC8 File Offset: 0x000071C8
		protected override bool OnDragMotion(DragContext context, int x, int y, uint time_)
		{
			return base.OnDragMotion(context, x, y, time_);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00008FD5 File Offset: 0x000071D5
		protected override bool OnDragDrop(DragContext context, int x, int y, uint time_)
		{
			CellRenderHelper.HoverdItem = null;
			return base.OnDragDrop(context, x, y, time_);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00008FE8 File Offset: 0x000071E8
		protected override void OnDragLeave(DragContext context, uint time_)
		{
			CellRenderHelper.HoverdItem = null;
			base.OnDragLeave(context, time_);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00008FF8 File Offset: 0x000071F8
		protected override bool OnLeaveNotifyEvent(EventCrossing evnt)
		{
			CellRenderHelper.HoverdItem = null;
			return base.OnLeaveNotifyEvent(evnt);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00009008 File Offset: 0x00007208
		private bool IsClickToggleButton(EventButton evnt, out TreePath treePath)
		{
			TreeViewDropPosition treeViewDropPosition;
			return base.GetDestRowAtPos((int)evnt.X, (int)evnt.Y, out treePath, out treeViewDropPosition) && ((double)((treePath.Depth - 1) * this.toggleButtonWidth) >= evnt.X || evnt.X >= (double)(treePath.Depth * this.toggleButtonWidth));
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00009068 File Offset: 0x00007268
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

		// Token: 0x060001A6 RID: 422 RVA: 0x000090B8 File Offset: 0x000072B8
		protected override bool OnKeyPressEvent(EventKey evnt)
		{
			if (evnt.State != ModifierType.None)
			{
				this.ModifierIsNone = true;
			}
			return true;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000090CA File Offset: 0x000072CA
		protected override bool OnKeyReleaseEvent(EventKey evnt)
		{
			if (evnt.State == ModifierType.None)
			{
				this.ModifierIsNone = false;
			}
			return true;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x000090DC File Offset: 0x000072DC
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x000090E4 File Offset: 0x000072E4
		public TreeModelFilter Filter { get; set; }

		// Token: 0x060001AA RID: 426 RVA: 0x0000910C File Offset: 0x0000730C
		public void SetSelectes(IEnumerable<TreePath> selectesPath)
		{
			if (selectesPath == null)
			{
				return;
			}
			base.Selection.UnselectAll();
			base.Selection.SelectFunction = ((TreeSelection selection, TreeModel model, TreePath path, bool path_currently_selected) => selectesPath.Contains(path));
			base.ScrollToCell(selectesPath.FirstOrDefault<TreePath>(), base.Columns.FirstOrDefault<TreeViewColumn>(), false, 0.5f, 0.5f);
			base.Selection.SelectAll();
			base.Selection.SelectFunction = ((TreeSelection selection, TreeModel model, TreePath path, bool path_currently_selected) => true);
		}

		// Token: 0x04000072 RID: 114
		private int toggleButtonWidth = 14;

		// Token: 0x04000073 RID: 115
		private bool ModifierIsNone;
	}
}
