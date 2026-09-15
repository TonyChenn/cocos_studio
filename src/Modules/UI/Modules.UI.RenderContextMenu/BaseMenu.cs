using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Lib.Prism;
using CocoStudio.Model;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render.ExtensionModel;
using Modules.Communal.Render.Model;
using MonoDevelop.Components.Commands;

namespace Modules.UI.RenderContextMenu
{
	public class BaseMenu : CommandMenu, IObjectContextMenu, IActivateControl
	{
		public IReadOnlyList<VisualObject> SelectedObjectList
		{
			get
			{
				return SelectService.Instance.SelectedObjectList;
			}
		}

		public IReadOnlyList<VisualObject> SelectedParentObjectList
		{
			get
			{
				return SelectService.Instance.SelectedParentObjectList;
			}
		}

		public VisualObject SelectedObject
		{
			get
			{
				return SelectService.Instance.CurrentObject;
			}
		}

		public List<VisualObject> CopyObjectList { get; set; }

		public AbstractNodeObject BaseRootObject { get; set; }

		public bool IsCutObject { get; set; }

		public int CopyContinuationIndex { get; set; }

		public PointF SingleObjectPosition { get; set; }

		public PointF CopyObjectListCenter { get; set; }

		public BaseMenu() : base(Services.CommandService)
		{
			this.taskService = Services.TaskService;
			this.CopyObjectList = new List<VisualObject>();
			this.InitMenuItem();
		}

		protected virtual void InitMenuItem()
		{
			this.menuItemCutComponent = MenuCreator.CreateMenuItem(GlobalCommand.CutCmd, false, LanguageInfo.Command_Cut);
			this.menuItemCopyComponent = MenuCreator.CreateMenuItem(GlobalCommand.CopyCmd, false, LanguageInfo.Command_Copy);
			this.menuItemPasteComponent = MenuCreator.CreateDelayCloseMenuItem(GlobalCommand.PasteCmd, false, null);
			this.menuItemDeleteObject = MenuCreator.CreateMenuItem(GlobalCommand.DeleteCmd, false, null);
			base.Append(this.menuItemCutComponent);
			base.Append(this.menuItemCopyComponent);
			base.Append(this.menuItemPasteComponent);
			base.Append(this.menuItemDeleteObject);
		}

		public void RegisterEvent()
		{
			this.eventAggregator = Services.EventsService;
			this.eventAggregator.GetEvent<DeleteVisualObjectsEvent>().Subscribe(new Action<ReadOnlyCollection<VisualObject>>(this.DeleteEventHandle));
			this.eventAggregator.GetEvent<CopyVisualObjectsEvent>().Subscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CopyEventHandle));
			this.eventAggregator.GetEvent<PasteVisualObjectsEvent>().Subscribe(new Action<PasteObjectsChangeEventArgs>(this.PasteEventHandle));
			this.eventAggregator.GetEvent<CutVisualObjectsEvent>().Subscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CutEventHandle));
		}

		protected override void OnShown()
		{
			base.OnShown();
			this.UpdateOperationSensitive(true);
		}

		protected virtual void UpdateOperationSensitive(bool hasSelected = true)
		{
			bool flag = false;
			bool flag2 = false;
			if (this.SelectedObjectList != null && this.SelectedObjectList.Count >= 1)
			{
				flag = true;
				flag2 = (this.SelectedObject == Services.ProjectOperations.CurrentSelectedProject.GetRootNode());
			}
			this.menuItemCutComponent.Sensitive = (flag && hasSelected && !flag2);
			this.menuItemDeleteObject.Sensitive = (flag && hasSelected && !flag2);
			this.menuItemCopyComponent.Sensitive = (flag && hasSelected && !flag2);
			this.menuItemPasteComponent.Sensitive = (this.PasteMenuIsAction(this.CopyObjectList) && hasSelected);
		}

		private void CopyEventHandle(ReadOnlyCollection<VisualObject> obj)
		{
			this.OnCopy(obj);
		}

		private void CutEventHandle(ReadOnlyCollection<VisualObject> obj)
		{
			this.OnCut(obj);
		}

		private void PasteEventHandle(PasteObjectsChangeEventArgs obj)
		{
			this.OnPaste(obj);
		}

		private void DeleteEventHandle(ReadOnlyCollection<VisualObject> obj)
		{
			this.OnDelete(obj);
		}

		protected virtual void OnCopy(ReadOnlyCollection<VisualObject> objectList)
		{
			if (objectList != null && objectList.Count != 0)
			{
				if (this.SelectedParentObjectList != null && this.SelectedParentObjectList.Count > 0)
				{
					this.CopyObjectList.Clear();
					foreach (VisualObject item in this.SelectedParentObjectList)
					{
						this.CopyObjectList.Add(item);
					}
					this.BaseRootObject = Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
					this.IsCutObject = false;
					this.CopyContinuationIndex = 1;
				}
			}
		}

		protected virtual void OnCut(ReadOnlyCollection<VisualObject> objectList)
		{
			if (objectList != null && objectList.Count != 0)
			{
				this.CutObject();
			}
		}

		private void CutObject()
		{
			if (this.SelectedParentObjectList != null && this.SelectedParentObjectList.Count > 0)
			{
				this.SetObjectCenterPosition(this.SelectedParentObjectList);
				using (CompositeTask.Run("剪切对象", null))
				{
					this.CopyObjectList.Clear();
					foreach (VisualObject visualObject in this.SelectedParentObjectList)
					{
						AbstractNodeObject abstractNodeObject = (AbstractNodeObject)visualObject;
						abstractNodeObject.IsSelected = false;
						AbstractNodeObject abstractNodeObject2 = abstractNodeObject.Clone() as AbstractNodeObject;
						abstractNodeObject.Parent.Children.Remove(abstractNodeObject);
						abstractNodeObject2.Name = abstractNodeObject.Name;
						this.CopyObjectList.Add(abstractNodeObject2);
					}
					this.BaseRootObject = Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
					this.UpdateSelectedObjects(null, null);
				}
				this.IsCutObject = true;
				this.CopyContinuationIndex = 1;
			}
		}

		protected virtual void OnPaste(PasteObjectsChangeEventArgs obj)
		{
			if (this.PasteMenuIsAction(this.CopyObjectList) && this.CheckObjectPaste(this.CopyObjectList))
			{
				PointF position = (obj == null) ? null : obj.PastePosition;
				this.PasteObject(position);
			}
		}

		public bool PasteMenuIsAction(IReadOnlyList<VisualObject> pasteList)
		{
			return pasteList != null && pasteList.Count != 0;
		}

		public IEnumerable<VisualObject> GetAllChildNode(VisualObject rootNode)
		{
			List<VisualObject> list = new List<VisualObject>();
			IEnumerable<VisualObject> visualChildren = rootNode.GetVisualChildren();
			if (visualChildren != null && visualChildren.Count<VisualObject>() > 0)
			{
				foreach (VisualObject rootNode2 in visualChildren)
				{
					IEnumerable<VisualObject> allChildNode = this.GetAllChildNode(rootNode2);
					list.AddRange(allChildNode);
				}
				list.AddRange(visualChildren);
			}
			return list;
		}

		private bool CheckObjectPaste(IEnumerable<VisualObject> pasteList)
		{
			GameFile gameFile = Services.ProjectOperations.CurrentSelectedProject.CocosFile as GameFile;
			foreach (VisualObject visualObject in pasteList)
			{
				FileNodeObject fileNodeObject = visualObject as FileNodeObject;
				if (fileNodeObject != null && fileNodeObject.FileData != null)
				{
					if (fileNodeObject.FileData.FullPath == gameFile.FileName)
					{
						LogConfig.Output.Error(LanguageInfo.Output_PasteFailure);
						return false;
					}
					ResourceItem resourceItem = ProjectsService.Instance.CurrentResourceGroup.FindResourceItem(fileNodeObject.FileData.FullPath);
					CocosItem cocosItem = resourceItem as CocosItem;
					if (cocosItem != null)
					{
						string value = cocosItem.CheckNest(gameFile.CocosItem);
						if (!string.IsNullOrEmpty(value))
						{
							LogConfig.Output.Error(LanguageInfo.Output_PasteFailure);
							return false;
						}
					}
				}
				IEnumerable<VisualObject> allChildNode = this.GetAllChildNode(visualObject);
				if (!this.CheckObjectPaste(allChildNode))
				{
					return false;
				}
			}
			return true;
		}

		public void PasteObject(PointF position)
		{
			if (this.CopyObjectList != null && this.CopyObjectList.Count > 0)
			{
				using (CompositeTask.Run("粘贴对象", null))
				{
					AbstractNodeObject rootObject = Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
					IList<VisualObject> objectList = this.CopyObjectList.OrderByDescending(delegate(VisualObject i)
					{
						int result = 0;
						AbstractNodeObject abstractNodeObject = i as AbstractNodeObject;
						if (abstractNodeObject != null)
						{
							ObjectCopyHelper.GetChildGlobalIndex(rootObject, abstractNodeObject, ref result);
						}
						return result;
					}).ToList<VisualObject>();
					if (this.IsCutObject)
					{
						this.PasteCutObjects(position, rootObject, objectList);
					}
					else
					{
						this.PasteCopyObjects(position, rootObject, objectList);
					}
				}
				this.IsCutObject = false;
			}
		}

		private void PasteCutObjects(PointF position, AbstractNodeObject rootObject, IList<VisualObject> objectList)
		{
			List<VisualObject> list = new List<VisualObject>();
			float num = 0f;
			float num2 = 0f;
			bool isSingle = objectList.Count == 1;
			PointF pointF = this.ComputeOffsetForPasteObjects(position, isSingle, true);
			if (pointF != null)
			{
				num = pointF.X;
				num2 = pointF.Y;
			}
			for (int i = 0; i < objectList.Count; i++)
			{
				AbstractNodeObject abstractNodeObject = objectList[i] as AbstractNodeObject;
				if (abstractNodeObject.Parent != null)
				{
					if (this.BaseRootObject == rootObject)
					{
						abstractNodeObject.Parent.Children.Add(abstractNodeObject);
					}
					else
					{
						rootObject.Children.Add(abstractNodeObject);
					}
					if (abstractNodeObject.Visible && abstractNodeObject.CanEdit)
					{
						list.Add(abstractNodeObject);
					}
					if (abstractNodeObject.OperationFlag.HasFlag(OperationMask.MoveFlag) && (num != 0f || num2 != 0f) && this.BaseRootObject == rootObject)
					{
						PointF pointF2 = abstractNodeObject.Parent.TransformToSelf(new PointF(0f, 0f));
						PointF pointF3 = abstractNodeObject.Parent.TransformToSelf(new PointF(num, num2));
						PointF pointF4 = new PointF(pointF3.X - pointF2.X, pointF3.Y - pointF2.Y);
						PointF position2 = abstractNodeObject.Position;
						abstractNodeObject.Position = new PointF(position2.X + pointF4.X, position2.Y + pointF4.Y);
					}
				}
			}
			this.UpdateSelectedObjects(list, list);
			this.CopyObjectList.Clear();
		}

		private void PasteCopyObjects(PointF position, AbstractNodeObject rootObject, IList<VisualObject> objectList)
		{
			List<VisualObject> list = new List<VisualObject>();
			ScaleValue scale = GameWindow.Current.GetCanvasObject().Scale;
			float num = (float)(10 * this.CopyContinuationIndex) * scale.ScaleX;
			float num2 = (float)(-10 * this.CopyContinuationIndex) * scale.ScaleY;
			bool isSingle = objectList.Count == 1;
			this.SetObjectCenterPosition(new ReadOnlyCollection<VisualObject>(objectList));
			PointF pointF = this.ComputeOffsetForPasteObjects(position, isSingle, false);
			if (pointF != null)
			{
				num = pointF.X;
				num2 = pointF.Y;
			}
			for (int i = objectList.Count - 1; i >= 0; i--)
			{
				AbstractNodeObject abstractNodeObject = objectList[i] as AbstractNodeObject;
				if (abstractNodeObject.Parent != null)
				{
					PointF pointF2 = abstractNodeObject.TransformToScene(new PointF(abstractNodeObject.AnchorPoint.ScaleX * abstractNodeObject.Size.Width, abstractNodeObject.AnchorPoint.ScaleY * abstractNodeObject.Size.Height));
					pointF2.X += num;
					pointF2.Y += num2;
					AbstractNodeObject abstractNodeObject2;
					if (this.BaseRootObject == rootObject)
					{
						abstractNodeObject2 = abstractNodeObject.Parent;
					}
					else
					{
						abstractNodeObject2 = rootObject;
					}
					PointF position2 = abstractNodeObject2.TransformSceneForChild(pointF2);
					AbstractNodeObject newGUI = abstractNodeObject.Clone() as AbstractNodeObject;
					IEnumerable<AbstractNodeObject> enumerable = from n in abstractNodeObject2.Children
					where n.Name == newGUI.Name
					select n;
					if (enumerable != null && enumerable.Count<AbstractNodeObject>() > 0)
					{
						newGUI.Name = this.GetCloneNewName(newGUI.Name, abstractNodeObject2);
					}
					abstractNodeObject2.Children.Add(newGUI);
					bool flag = false;
					if (abstractNodeObject2 != rootObject && abstractNodeObject2.Parent == null)
					{
						flag = true;
					}
					if (newGUI.Visible && newGUI.CanEdit && !flag)
					{
						list.Add(newGUI);
					}
					if (this.IsFreeLayoutParent(abstractNodeObject2) && !this.HasPositionFrame(newGUI) && this.BaseRootObject == rootObject)
					{
						newGUI.Position = position2;
					}
				}
			}
			this.UpdateSelectedObjects(list, list);
		}

		private PointF ComputeOffsetForPasteObjects(PointF position, bool isSingle, bool isCut)
		{
			PointF pointF = new PointF(0f, 0f);
			PointF result;
			if (position != null)
			{
				int num;
				PointF pointF2;
				if (this.SelectedParentObjectList.Count == 0)
				{
					num = 0;
					pointF2 = position;
				}
				else
				{
					num = 10;
					if (this.SelectedParentObjectList.Count == 1)
					{
						VisualObject visualObject = this.SelectedParentObjectList.FirstOrDefault<VisualObject>();
						pointF2 = visualObject.TransformToScene(new PointF(visualObject.AnchorPoint.ScaleX * visualObject.Size.Width, visualObject.AnchorPoint.ScaleY * visualObject.Size.Height));
					}
					else
					{
						pointF2 = ObjectRectangelHelper.GetObjectListCenter(this.SelectedParentObjectList);
					}
				}
				if (pointF2 != null)
				{
					PointF pointF3;
					if (isSingle)
					{
						pointF3 = this.SingleObjectPosition;
					}
					else
					{
						pointF3 = this.CopyObjectListCenter;
					}
					pointF.X = pointF2.X - pointF3.X + (float)num;
					pointF.Y = pointF2.Y - pointF3.Y - (float)num;
				}
				result = pointF;
			}
			else
			{
				if (isCut)
				{
					pointF = new PointF(0f, 0f);
				}
				else
				{
					this.CopyContinuationIndex++;
					pointF = null;
				}
				result = pointF;
			}
			return result;
		}

		private void SetObjectCenterPosition(IReadOnlyList<VisualObject> objectList)
		{
			if (objectList.Count == 1)
			{
				VisualObject visualObject = objectList.FirstOrDefault<VisualObject>();
				this.SingleObjectPosition = visualObject.TransformToScene(new PointF(visualObject.AnchorPoint.ScaleX * visualObject.Size.Width, visualObject.AnchorPoint.ScaleY * visualObject.Size.Height));
			}
			this.CopyObjectListCenter = ObjectRectangelHelper.GetObjectListCenter(objectList);
		}

		private string GetCloneNewName(string baseName, AbstractNodeObject parentNode)
		{
			int num = 0;
			string newName;
			for (;;)
			{
				newName = baseName + "_" + num;
				List<AbstractNodeObject> list = (from n in parentNode.Children
				where n.Name == newName
				select n).ToList<AbstractNodeObject>();
				if (list == null || list.Count == 0)
				{
					break;
				}
				num++;
			}
			return newName;
		}

		private bool IsFreeLayoutParent(AbstractNodeObject parent)
		{
			return !(parent is ListViewObject) && !(parent is PageViewObject);
		}

		private bool HasPositionFrame(AbstractNodeObject nodeObject)
		{
			if (nodeObject.Timelines != null)
			{
				foreach (Timeline timeline in nodeObject.Timelines)
				{
					if (timeline.PropertyInfo.Name == PropertySupport.ExtractPropertyInfo<PointF>(() => nodeObject.Position).Name)
					{
						if (timeline.Frames.Count > 0)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		protected virtual void OnDelete(ReadOnlyCollection<VisualObject> obj)
		{
			if (this.SelectedParentObjectList != null && this.SelectedParentObjectList.Count > 0)
			{
				List<VisualObject> list = this.SelectedParentObjectList.ToList<VisualObject>();
				this.UpdateSelectedObjects(null, null);
				foreach (VisualObject visualObject in list)
				{
					AbstractNodeObject abstractNodeObject = visualObject as AbstractNodeObject;
					if (abstractNodeObject.Parent != null)
					{
						abstractNodeObject.IsSelected = false;
						abstractNodeObject.Parent.Children.Remove(abstractNodeObject);
					}
					if (this.CopyObjectList != null)
					{
						if (this.CopyObjectList.Contains(visualObject))
						{
							this.CopyObjectList.Remove(visualObject);
						}
					}
				}
				list.Clear();
			}
		}

		public void UnregisterEvent()
		{
			this.eventAggregator = Services.EventsService;
			this.eventAggregator.GetEvent<DeleteVisualObjectsEvent>().Unsubscribe(new Action<ReadOnlyCollection<VisualObject>>(this.DeleteEventHandle));
			this.eventAggregator.GetEvent<CopyVisualObjectsEvent>().Unsubscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CopyEventHandle));
			this.eventAggregator.GetEvent<PasteVisualObjectsEvent>().Unsubscribe(new Action<PasteObjectsChangeEventArgs>(this.PasteEventHandle));
			this.eventAggregator.GetEvent<CutVisualObjectsEvent>().Unsubscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CutEventHandle));
		}

		public override void Dispose()
		{
			this.UnregisterEvent();
			base.Dispose();
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

		public virtual void CanShow(ContextMenuShowingArgs args)
		{
		}

		public virtual string Type
		{
			get
			{
				return string.Empty;
			}
		}

		public virtual void Activated(CocosItem project)
		{
			this.RegisterEvent();
		}

		public new virtual void Deactivated()
		{
			this.UnregisterEvent();
		}

		protected MenuItem menuItemCopyComponent;

		protected MenuItem menuItemPasteComponent;

		protected MenuItem menuItemCutComponent;

		protected MenuItem menuItemDeleteObject;

		protected IEventAggregator eventAggregator;

		private IUndoManager taskService;
	}
}
