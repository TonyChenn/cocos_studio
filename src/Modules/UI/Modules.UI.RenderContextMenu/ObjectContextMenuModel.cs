using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.Lib.Prism;
using CocoStudio.Model;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.UndoManager;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000020 RID: 32
	public class ObjectContextMenuModel
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x000067EC File Offset: 0x000049EC
		public ObjectContextMenuModel(BaseMenu baseMenu)
		{
			this.eventAggregator = Services.EventsService;
			this.taskService = Services.TaskService;
			this.baseMenu = baseMenu;
			Services.ProjectOperations.CurrentSelectedSolutionClosed += this.ProjectOperations_CurrentSelectedSolutionClosed;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000683D File Offset: 0x00004A3D
		private void MoveNodeOrderEventHandle(AdjustRenderOrderEventArgs obj)
		{
			this.MoveNodeOrder(obj.Action);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00006850 File Offset: 0x00004A50
		public void MoveNodeOrder(MoveOrderType movetype)
		{
			Services.TaskService.BeginCompositeTask("MoveOrderNode");
			if (this.baseMenu.SelectedObjectList != null && this.baseMenu.SelectedObjectList.Count == 1)
			{
				NodeObject nodeObject = this.baseMenu.SelectedObject as NodeObject;
				if (nodeObject != null)
				{
					AbstractNodeObject parent = nodeObject.Parent;
					List<VisualObject> list = new List<VisualObject>
					{
						nodeObject
					};
					if (parent != null && parent.Children != null)
					{
						int num = parent.Children.IndexOf(nodeObject);
						switch (movetype)
						{
						case MoveOrderType.Up:
							if (parent.Children.Count - 1 <= num)
							{
								return;
							}
							num++;
							break;
						case MoveOrderType.Down:
							if (num == 0)
							{
								return;
							}
							num--;
							break;
						case MoveOrderType.Top:
							num = parent.Children.Count - 1;
							break;
						case MoveOrderType.Bottom:
							num = 0;
							break;
						}
						parent.Children.Remove(nodeObject);
						parent.Children.Insert(num, nodeObject);
						this.eventAggregator.GetEvent<SelectedVisualObjectsChangeEvent>().Publish(new SelectedVisualObjectsChangeEventArgs(list, list, false));
						Services.TaskService.EndCompositeTask();
					}
				}
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000069B0 File Offset: 0x00004BB0
		internal bool IsCanMoveIndex(MoveOrderType moveType)
		{
			bool result;
			try
			{
				if (this.baseMenu.SelectedObjectList == null || this.baseMenu.SelectedObjectList.Count != 1)
				{
					result = false;
				}
				else
				{
					NodeObject nodeObject = this.baseMenu.SelectedObject as NodeObject;
					if (nodeObject == null)
					{
						result = false;
					}
					else
					{
						bool flag = false;
						int num = nodeObject.Parent.Children.IndexOf(nodeObject);
						int num2 = nodeObject.Parent.Children.Count - 1;
						switch (moveType)
						{
						case MoveOrderType.Up:
						case MoveOrderType.Top:
							if (num != num2)
							{
								flag = true;
							}
							break;
						case MoveOrderType.Down:
						case MoveOrderType.Bottom:
							if (num != 0)
							{
								flag = true;
							}
							break;
						}
						result = flag;
					}
				}
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
				result = false;
			}
			return result;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00006AA8 File Offset: 0x00004CA8
		private void ProjectOperations_CurrentSelectedSolutionClosed(object sender, SolutionEventArgs e)
		{
			if (this.baseMenu.CopyObjectList != null)
			{
				this.baseMenu.CopyObjectList.Clear();
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00006ADB File Offset: 0x00004CDB
		private void AlignObjectsEventHandle(int alignType)
		{
			this.AlignObject((AlignType)alignType);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00006AE8 File Offset: 0x00004CE8
		private void UpdateSelectedObjects(List<VisualObject> parentObjects = null, List<VisualObject> objects = null)
		{
			if (parentObjects == null)
			{
				parentObjects = new List<VisualObject>();
			}
			if (objects == null)
			{
				objects = new List<VisualObject>();
			}
			SelectedVisualObjectsChangeEventArgs payload = new SelectedVisualObjectsChangeEventArgs(parentObjects, objects, false);
			this.eventAggregator.GetEvent<SelectedVisualObjectsChangeEvent>().Publish(payload);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00006B3C File Offset: 0x00004D3C
		public void AddObject(AbstractNodeObject newObject, PointF position)
		{
			if (newObject == null)
			{
				throw new InvalidOperationException("Not support now.");
			}
			using (CompositeTask.Run("StructTreeAddChild", null))
			{
				AbstractNodeObject abstractNodeObject = this.baseMenu.SelectedObject as AbstractNodeObject;
				List<VisualObject> list = new List<VisualObject>();
				if (abstractNodeObject != null)
				{
					abstractNodeObject.IsExpanded = false;
					newObject.Position = newObject.Position;
					abstractNodeObject.Children.Add(newObject);
					abstractNodeObject.IsExpanded = true;
				}
				else
				{
					AbstractNodeObject rootNode = Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
					newObject.Position = rootNode.TransformToSelf(position);
					rootNode.Children.Add(newObject);
				}
				list.Add(newObject);
				if (!(abstractNodeObject is PageViewObject))
				{
					EventAggregator.Instance.GetEvent<SelectedVisualObjectsChangeEvent>().Publish(new SelectedVisualObjectsChangeEventArgs(list, list, false));
				}
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00006C3C File Offset: 0x00004E3C
		public void AlignObject(AlignType alignType)
		{
			if (ObjectRectangelHelper.CheckObjectAlign(alignType, this.baseMenu.SelectedParentObjectList))
			{
				using (CompositeTask.Run("对齐对象", null))
				{
					ObjectRectangelHelper.AlignObject(alignType, this.baseMenu.SelectedParentObjectList);
					Services.EventsService.GetEvent<AlignedObjectsEvent>().Publish(new AlignedObjectsArgs());
				}
			}
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00006CBC File Offset: 0x00004EBC
		public void RegisterEvent()
		{
			this.eventAggregator = Services.EventsService;
			this.eventAggregator.GetEvent<AlignVisualObjectsEvent>().Subscribe(new Action<int>(this.AlignObjectsEventHandle));
			this.eventAggregator.GetEvent<AdjustRenderOrderEvent>().Subscribe(new Action<AdjustRenderOrderEventArgs>(this.MoveNodeOrderEventHandle));
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00006D10 File Offset: 0x00004F10
		public void UnregisterEvent()
		{
			this.eventAggregator = Services.EventsService;
			this.eventAggregator.GetEvent<AlignVisualObjectsEvent>().Unsubscribe(new Action<int>(this.AlignObjectsEventHandle));
			this.eventAggregator.GetEvent<AdjustRenderOrderEvent>().Unsubscribe(new Action<AdjustRenderOrderEventArgs>(this.MoveNodeOrderEventHandle));
		}

		// Token: 0x04000072 RID: 114
		private IUndoManager taskService;

		// Token: 0x04000073 RID: 115
		private IEventAggregator eventAggregator;

		// Token: 0x04000074 RID: 116
		private BaseMenu baseMenu;

		// Token: 0x04000075 RID: 117
		private bool IsCutObject = false;
	}
}
