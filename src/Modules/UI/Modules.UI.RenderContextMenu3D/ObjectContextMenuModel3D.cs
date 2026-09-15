using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.Lib.Prism;
using CocoStudio.Model;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.MultiLanguage;
using Modules.UI.RenderContextMenu;

namespace Modules.UI.RenderContextMenu3D
{
	public class ObjectContextMenuModel3D
	{
		public List<VisualObject> CopyObjectList { get; set; }

		public ReadOnlyCollection<VisualObject> SelectedParentObjectList { get; set; }

		public ObjectContextMenuModel3D()
		{
			this.eventAggregator = Services.EventsService;
			this.taskService = Services.TaskService;
			this.SelectedObjectList = new ReadOnlyCollection<VisualObject>(new List<VisualObject>());
			this.SelectedParentObjectList = new ReadOnlyCollection<VisualObject>(new List<VisualObject>());
			Services.ProjectOperations.CurrentSelectedSolutionClosed += this.ProjectOperations_CurrentSelectedSolutionClosed;
			this.CopyObjectList = new List<VisualObject>();
		}

		private void SelectedObjectsChangeEventHandle(SelectedVisualObjectsChangeEventArgs args)
		{
			this.SelectedObject = args.SelectedParentObject.FirstOrDefault<VisualObject>();
			this.SelectedObjectList = args.SelectedObject;
			this.SelectedParentObjectList = args.SelectedParentObject;
			if (this.SelectedObjectList != null && this.SelectedObjectList.Count > 0)
			{
				if (this.SelectedObjectList.Count == 1)
				{
					this.SelectedOne();
					return;
				}
				this.SelectedMultipe();
			}
		}

		private void SelectedOne()
		{
		}

		private void SelectedMultipe()
		{
		}

		private void DeleteVisualObjectsEventHandle(ReadOnlyCollection<VisualObject> objectList)
		{
			this.SelectedParentObjectList = objectList;
			using (CompositeTask.Run("删除对象", null))
			{
				this.DeleteObject();
			}
		}

		private void CopyVisualObjectsEventHandle(ReadOnlyCollection<VisualObject> objectList)
		{
			if (objectList == null || objectList.Count == 0)
			{
				return;
			}
			this.CopyObject();
		}

		private void PasteVisualObjectsEventHandle(PasteObjectsChangeEventArgs args)
		{
			if (this.PasteMenuIsAction(this.CopyObjectList))
			{
				PointF position = (args == null) ? null : args.PastePosition;
				this.PasteObject(position);
			}
		}

		private void CutVisualObjectsEventHandle(ReadOnlyCollection<VisualObject> objectList)
		{
			if (objectList == null || objectList.Count == 0)
			{
				return;
			}
			this.CutObject();
		}

		private void ProjectOperations_CurrentSelectedSolutionClosed(object sender, SolutionEventArgs e)
		{
			if (this.CopyObjectList != null)
			{
				this.CopyObjectList.Clear();
			}
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
				AbstractNodeObject abstractNodeObject = this.SelectedObject as AbstractNodeObject;
				List<VisualObject> list = new List<VisualObject>();
				if (abstractNodeObject != null)
				{
					abstractNodeObject.IsExpanded = false;
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

		public void DeleteObject()
		{
			if (this.SelectedParentObjectList == null || this.SelectedParentObjectList.Count <= 0)
			{
				return;
			}
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
				if (this.CopyObjectList != null && this.CopyObjectList.Contains(visualObject))
				{
					this.CopyObjectList.Remove(visualObject);
				}
			}
			list.Clear();
		}

		public void CopyObject()
		{
			if (this.SelectedParentObjectList == null || this.SelectedParentObjectList.Count <= 0)
			{
				return;
			}
			this.CopyObjectList.Clear();
			foreach (VisualObject item in this.SelectedParentObjectList)
			{
				this.CopyObjectList.Add(item);
			}
			this.BaseRootObject = Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
			this.IsCutObject = false;
		}

		private void SetObjectCenterPositin(ReadOnlyCollection<VisualObject> objectList)
		{
			if (objectList.Count == 1)
			{
				VisualObject visualObject = objectList.FirstOrDefault<VisualObject>();
				this.SingleObjectPosition = visualObject.TransformToScene(new PointF(visualObject.AnchorPoint.ScaleX * visualObject.Size.Width, visualObject.AnchorPoint.ScaleY * visualObject.Size.Height));
			}
		}

		private void CutObject()
		{
			if (this.SelectedParentObjectList == null || this.SelectedParentObjectList.Count <= 0)
			{
				return;
			}
			this.SetObjectCenterPositin(this.SelectedParentObjectList);
			using (CompositeTask.Run("剪切对象", null))
			{
				this.CopyObjectList.Clear();
				foreach (VisualObject visualObject in this.SelectedParentObjectList)
				{
					AbstractNodeObject abstractNodeObject = (AbstractNodeObject)visualObject;
					abstractNodeObject.IsSelected = false;
					AbstractNodeObject abstractNodeObject2 = abstractNodeObject.Clone() as AbstractNodeObject;
					abstractNodeObject2.Name = abstractNodeObject.Name;
					this.CopyObjectList.Add(abstractNodeObject2);
					abstractNodeObject.Parent.Children.Remove(abstractNodeObject);
				}
				this.BaseRootObject = Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
				this.UpdateSelectedObjects(null, null);
			}
			this.IsCutObject = true;
		}

		private void ResetName(AbstractNodeObject newOne, AbstractNodeObject oldOne)
		{
			if (newOne == null || oldOne == null)
			{
				return;
			}
			newOne.Name = oldOne.Name;
			if (newOne.Children.Count > 0 && oldOne.Children.Count > 0)
			{
				for (int i = 0; i < newOne.Children.Count; i++)
				{
					this.ResetName(newOne.Children[i], oldOne.Children[i]);
				}
			}
		}

		public void PasteObject(PointF position)
		{
			if (this.CopyObjectList == null || this.CopyObjectList.Count <= 0)
			{
				return;
			}
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

		private void PasteCopyObjects(PointF position, AbstractNodeObject rootObject, IList<VisualObject> objectList)
		{
			List<VisualObject> list = new List<VisualObject>();
			for (int i = objectList.Count - 1; i >= 0; i--)
			{
				Node3DObject node3DObject = objectList[i] as Node3DObject;
				if (node3DObject.Parent != null)
				{
					AbstractNodeObject abstractNodeObject;
					if (this.BaseRootObject == rootObject)
					{
						abstractNodeObject = node3DObject.Parent;
					}
					else
					{
						abstractNodeObject = rootObject;
					}
					Node3DObject newGUI = node3DObject.Clone() as Node3DObject;
					IEnumerable<AbstractNodeObject> enumerable = from n in abstractNodeObject.Children
					where n.Name == newGUI.Name
					select n;
					if (enumerable != null && enumerable.Count<AbstractNodeObject>() > 0)
					{
						newGUI.Name = this.GetCloneNewName(newGUI.Name, abstractNodeObject);
					}
					abstractNodeObject.Children.Add(newGUI);
					if (newGUI.Visible && newGUI.CanEdit)
					{
						list.Add(newGUI);
					}
				}
			}
			this.UpdateSelectedObjects(list, list);
		}

		private void PasteCutObjects(PointF position, AbstractNodeObject rootObject, IList<VisualObject> objectList)
		{
			List<VisualObject> list = new List<VisualObject>();
			int count = objectList.Count;
			for (int i = 0; i < objectList.Count; i++)
			{
				Node3DObject node3DObject = objectList[i] as Node3DObject;
				if (node3DObject.Parent != null)
				{
					if (this.BaseRootObject == rootObject)
					{
						node3DObject.Parent.Children.Add(node3DObject);
					}
					else
					{
						rootObject.Children.Add(node3DObject);
					}
					if (node3DObject.Visible && node3DObject.CanEdit)
					{
						list.Add(node3DObject);
					}
				}
			}
			this.UpdateSelectedObjects(list, list);
			this.CopyObjectList.Clear();
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

		public bool PasteMenuIsAction(IEnumerable<VisualObject> pasteList)
		{
			return pasteList != null && pasteList.Count<VisualObject>() != 0 && this.CheckObjectPaste(pasteList);
		}

		private bool CheckObjectPaste(IEnumerable<VisualObject> pasteList)
		{
			GameFile gameFile = Services.ProjectOperations.CurrentSelectedProject.CocosFile as GameFile;
			foreach (VisualObject visualObject in pasteList)
			{
				FileNodeObject fileNodeObject = visualObject as FileNodeObject;
				if (fileNodeObject != null && fileNodeObject.FileData != null && fileNodeObject.FileData.FullPath == gameFile.FileName)
				{
					LogConfig.Output.Error(LanguageInfo.MessageBox207_NestedSelfError);
					return false;
				}
			}
			return true;
		}

		public IEnumerable<VisualObject> GetAllChildeNode(VisualObject rootNode)
		{
			List<VisualObject> list = new List<VisualObject>();
			IEnumerable<VisualObject> visualChildren = rootNode.GetVisualChildren();
			if (visualChildren != null && visualChildren.Count<VisualObject>() > 0)
			{
				foreach (VisualObject rootNode2 in visualChildren)
				{
					IEnumerable<VisualObject> allChildeNode = this.GetAllChildeNode(rootNode2);
					list.AddRange(allChildeNode);
				}
				list.AddRange(visualChildren);
			}
			return list;
		}

		public void RegisterEvent()
		{
			this.eventAggregator = Services.EventsService;
			this.eventAggregator.GetEvent<SelectedVisualObjectsChangeEvent>().Subscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.SelectedObjectsChangeEventHandle));
			this.eventAggregator.GetEvent<DeleteVisualObjectsEvent>().Subscribe(new Action<ReadOnlyCollection<VisualObject>>(this.DeleteVisualObjectsEventHandle));
			this.eventAggregator.GetEvent<CopyVisualObjectsEvent>().Subscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CopyVisualObjectsEventHandle));
			this.eventAggregator.GetEvent<PasteVisualObjectsEvent>().Subscribe(new Action<PasteObjectsChangeEventArgs>(this.PasteVisualObjectsEventHandle));
			this.eventAggregator.GetEvent<CutVisualObjectsEvent>().Subscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CutVisualObjectsEventHandle));
		}

		public void UnregisterEvent()
		{
			this.eventAggregator = Services.EventsService;
			this.eventAggregator.GetEvent<SelectedVisualObjectsChangeEvent>().Unsubscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.SelectedObjectsChangeEventHandle));
			this.eventAggregator.GetEvent<DeleteVisualObjectsEvent>().Unsubscribe(new Action<ReadOnlyCollection<VisualObject>>(this.DeleteVisualObjectsEventHandle));
			this.eventAggregator.GetEvent<CopyVisualObjectsEvent>().Unsubscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CopyVisualObjectsEventHandle));
			this.eventAggregator.GetEvent<PasteVisualObjectsEvent>().Unsubscribe(new Action<PasteObjectsChangeEventArgs>(this.PasteVisualObjectsEventHandle));
			this.eventAggregator.GetEvent<CutVisualObjectsEvent>().Unsubscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CutVisualObjectsEventHandle));
		}

		public ReadOnlyCollection<VisualObject> SelectedObjectList;

		public VisualObject SelectedObject;

		private IUndoManager taskService;

		private IEventAggregator eventAggregator;

		private PointF CopyObjectListCenter = new PointF(0f, 0f);

		private PointF SingleObjectPosition = new PointF(0f, 0f);

		private VisualObject BaseRootObject;

		private bool IsCutObject;
	}
}
