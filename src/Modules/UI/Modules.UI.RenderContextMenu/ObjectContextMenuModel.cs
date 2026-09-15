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
	public class ObjectContextMenuModel
	{
		public ObjectContextMenuModel(BaseMenu baseMenu)
		{
			this.eventAggregator = Services.EventsService;
			this.taskService = Services.TaskService;
			this.baseMenu = baseMenu;
			Services.ProjectOperations.CurrentSelectedSolutionClosed += this.ProjectOperations_CurrentSelectedSolutionClosed;
		}

		private void MoveNodeOrderEventHandle(AdjustRenderOrderEventArgs obj)
		{
			this.MoveNodeOrder(obj.Action);
		}

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

		private void ProjectOperations_CurrentSelectedSolutionClosed(object sender, SolutionEventArgs e)
		{
			if (this.baseMenu.CopyObjectList != null)
			{
				this.baseMenu.CopyObjectList.Clear();
			}
		}

		private void AlignObjectsEventHandle(int alignType)
		{
			this.AlignObject((AlignType)alignType);
		}

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

		public void RegisterEvent()
		{
			this.eventAggregator = Services.EventsService;
			this.eventAggregator.GetEvent<AlignVisualObjectsEvent>().Subscribe(new Action<int>(this.AlignObjectsEventHandle));
			this.eventAggregator.GetEvent<AdjustRenderOrderEvent>().Subscribe(new Action<AdjustRenderOrderEventArgs>(this.MoveNodeOrderEventHandle));
		}

		public void UnregisterEvent()
		{
			this.eventAggregator = Services.EventsService;
			this.eventAggregator.GetEvent<AlignVisualObjectsEvent>().Unsubscribe(new Action<int>(this.AlignObjectsEventHandle));
			this.eventAggregator.GetEvent<AdjustRenderOrderEvent>().Unsubscribe(new Action<AdjustRenderOrderEventArgs>(this.MoveNodeOrderEventHandle));
		}

		private IUndoManager taskService;

		private IEventAggregator eventAggregator;

		private BaseMenu baseMenu;

		private bool IsCutObject = false;
	}
}
