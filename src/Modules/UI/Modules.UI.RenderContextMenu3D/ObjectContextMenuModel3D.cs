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
	// Token: 0x02000003 RID: 3
	public class ObjectContextMenuModel3D
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000023B9 File Offset: 0x000005B9
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000023C1 File Offset: 0x000005C1
		public List<VisualObject> CopyObjectList { get; set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000015 RID: 21 RVA: 0x000023CA File Offset: 0x000005CA
		// (set) Token: 0x06000016 RID: 22 RVA: 0x000023D2 File Offset: 0x000005D2
		public ReadOnlyCollection<VisualObject> SelectedParentObjectList { get; set; }

		// Token: 0x06000017 RID: 23 RVA: 0x000023DC File Offset: 0x000005DC
		public ObjectContextMenuModel3D()
		{
			this.eventAggregator = Services.EventsService;
			this.taskService = Services.TaskService;
			this.SelectedObjectList = new ReadOnlyCollection<VisualObject>(new List<VisualObject>());
			this.SelectedParentObjectList = new ReadOnlyCollection<VisualObject>(new List<VisualObject>());
			Services.ProjectOperations.CurrentSelectedSolutionClosed += this.ProjectOperations_CurrentSelectedSolutionClosed;
			this.CopyObjectList = new List<VisualObject>();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002470 File Offset: 0x00000670
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

		// Token: 0x06000019 RID: 25 RVA: 0x000024D7 File Offset: 0x000006D7
		private void SelectedOne()
		{
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024D9 File Offset: 0x000006D9
		private void SelectedMultipe()
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000024DC File Offset: 0x000006DC
		private void DeleteVisualObjectsEventHandle(ReadOnlyCollection<VisualObject> objectList)
		{
			this.SelectedParentObjectList = objectList;
			using (CompositeTask.Run("删除对象", null))
			{
				this.DeleteObject();
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002520 File Offset: 0x00000720
		private void CopyVisualObjectsEventHandle(ReadOnlyCollection<VisualObject> objectList)
		{
			if (objectList == null || objectList.Count == 0)
			{
				return;
			}
			this.CopyObject();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002534 File Offset: 0x00000734
		private void PasteVisualObjectsEventHandle(PasteObjectsChangeEventArgs args)
		{
			if (this.PasteMenuIsAction(this.CopyObjectList))
			{
				PointF position = (args == null) ? null : args.PastePosition;
				this.PasteObject(position);
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002563 File Offset: 0x00000763
		private void CutVisualObjectsEventHandle(ReadOnlyCollection<VisualObject> objectList)
		{
			if (objectList == null || objectList.Count == 0)
			{
				return;
			}
			this.CutObject();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002577 File Offset: 0x00000777
		private void ProjectOperations_CurrentSelectedSolutionClosed(object sender, SolutionEventArgs e)
		{
			if (this.CopyObjectList != null)
			{
				this.CopyObjectList.Clear();
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000258C File Offset: 0x0000078C
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

		// Token: 0x06000021 RID: 33 RVA: 0x000025C8 File Offset: 0x000007C8
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

		// Token: 0x06000022 RID: 34 RVA: 0x0000268C File Offset: 0x0000088C
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

		// Token: 0x06000023 RID: 35 RVA: 0x00002750 File Offset: 0x00000950
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

		// Token: 0x06000024 RID: 36 RVA: 0x000027E0 File Offset: 0x000009E0
		private void SetObjectCenterPositin(ReadOnlyCollection<VisualObject> objectList)
		{
			if (objectList.Count == 1)
			{
				VisualObject visualObject = objectList.FirstOrDefault<VisualObject>();
				this.SingleObjectPosition = visualObject.TransformToScene(new PointF(visualObject.AnchorPoint.ScaleX * visualObject.Size.Width, visualObject.AnchorPoint.ScaleY * visualObject.Size.Height));
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000283C File Offset: 0x00000A3C
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

		// Token: 0x06000026 RID: 38 RVA: 0x0000293C File Offset: 0x00000B3C
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

		// Token: 0x06000027 RID: 39 RVA: 0x000029E0 File Offset: 0x00000BE0
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

		// Token: 0x06000028 RID: 40 RVA: 0x00002AB4 File Offset: 0x00000CB4
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

		// Token: 0x06000029 RID: 41 RVA: 0x00002BB0 File Offset: 0x00000DB0
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

		// Token: 0x0600002A RID: 42 RVA: 0x00002C60 File Offset: 0x00000E60
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

		// Token: 0x0600002B RID: 43 RVA: 0x00002CC0 File Offset: 0x00000EC0
		public bool PasteMenuIsAction(IEnumerable<VisualObject> pasteList)
		{
			return pasteList != null && pasteList.Count<VisualObject>() != 0 && this.CheckObjectPaste(pasteList);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002CD8 File Offset: 0x00000ED8
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

		// Token: 0x0600002D RID: 45 RVA: 0x00002D78 File Offset: 0x00000F78
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

		// Token: 0x0600002E RID: 46 RVA: 0x00002DF0 File Offset: 0x00000FF0
		public void RegisterEvent()
		{
			this.eventAggregator = Services.EventsService;
			this.eventAggregator.GetEvent<SelectedVisualObjectsChangeEvent>().Subscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.SelectedObjectsChangeEventHandle));
			this.eventAggregator.GetEvent<DeleteVisualObjectsEvent>().Subscribe(new Action<ReadOnlyCollection<VisualObject>>(this.DeleteVisualObjectsEventHandle));
			this.eventAggregator.GetEvent<CopyVisualObjectsEvent>().Subscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CopyVisualObjectsEventHandle));
			this.eventAggregator.GetEvent<PasteVisualObjectsEvent>().Subscribe(new Action<PasteObjectsChangeEventArgs>(this.PasteVisualObjectsEventHandle));
			this.eventAggregator.GetEvent<CutVisualObjectsEvent>().Subscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CutVisualObjectsEventHandle));
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002E9C File Offset: 0x0000109C
		public void UnregisterEvent()
		{
			this.eventAggregator = Services.EventsService;
			this.eventAggregator.GetEvent<SelectedVisualObjectsChangeEvent>().Unsubscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.SelectedObjectsChangeEventHandle));
			this.eventAggregator.GetEvent<DeleteVisualObjectsEvent>().Unsubscribe(new Action<ReadOnlyCollection<VisualObject>>(this.DeleteVisualObjectsEventHandle));
			this.eventAggregator.GetEvent<CopyVisualObjectsEvent>().Unsubscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CopyVisualObjectsEventHandle));
			this.eventAggregator.GetEvent<PasteVisualObjectsEvent>().Unsubscribe(new Action<PasteObjectsChangeEventArgs>(this.PasteVisualObjectsEventHandle));
			this.eventAggregator.GetEvent<CutVisualObjectsEvent>().Unsubscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CutVisualObjectsEventHandle));
		}

		// Token: 0x0400000C RID: 12
		public ReadOnlyCollection<VisualObject> SelectedObjectList;

		// Token: 0x0400000D RID: 13
		public VisualObject SelectedObject;

		// Token: 0x0400000E RID: 14
		private IUndoManager taskService;

		// Token: 0x0400000F RID: 15
		private IEventAggregator eventAggregator;

		// Token: 0x04000010 RID: 16
		private PointF CopyObjectListCenter = new PointF(0f, 0f);

		// Token: 0x04000011 RID: 17
		private PointF SingleObjectPosition = new PointF(0f, 0f);

		// Token: 0x04000012 RID: 18
		private VisualObject BaseRootObject;

		// Token: 0x04000013 RID: 19
		private bool IsCutObject;
	}
}
