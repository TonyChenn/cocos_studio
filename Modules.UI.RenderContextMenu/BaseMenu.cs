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
	// Token: 0x02000015 RID: 21
	public class BaseMenu : CommandMenu, IObjectContextMenu, IActivateControl
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00003538 File Offset: 0x00001738
		public IReadOnlyList<VisualObject> SelectedObjectList
		{
			get
			{
				return SelectService.Instance.SelectedObjectList;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003554 File Offset: 0x00001754
		public IReadOnlyList<VisualObject> SelectedParentObjectList
		{
			get
			{
				return SelectService.Instance.SelectedParentObjectList;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00003570 File Offset: 0x00001770
		public VisualObject SelectedObject
		{
			get
			{
				return SelectService.Instance.CurrentObject;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0000358C File Offset: 0x0000178C
		// (set) Token: 0x06000078 RID: 120 RVA: 0x000035A3 File Offset: 0x000017A3
		public List<VisualObject> CopyObjectList { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000035AC File Offset: 0x000017AC
		// (set) Token: 0x0600007A RID: 122 RVA: 0x000035C3 File Offset: 0x000017C3
		public AbstractNodeObject BaseRootObject { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000035CC File Offset: 0x000017CC
		// (set) Token: 0x0600007C RID: 124 RVA: 0x000035E3 File Offset: 0x000017E3
		public bool IsCutObject { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000035EC File Offset: 0x000017EC
		// (set) Token: 0x0600007E RID: 126 RVA: 0x00003603 File Offset: 0x00001803
		public int CopyContinuationIndex { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000360C File Offset: 0x0000180C
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00003623 File Offset: 0x00001823
		public PointF SingleObjectPosition { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000081 RID: 129 RVA: 0x0000362C File Offset: 0x0000182C
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00003643 File Offset: 0x00001843
		public PointF CopyObjectListCenter { get; set; }

		// Token: 0x06000083 RID: 131 RVA: 0x0000364C File Offset: 0x0000184C
		public BaseMenu() : base(Services.CommandService)
		{
			this.taskService = Services.TaskService;
			this.CopyObjectList = new List<VisualObject>();
			this.InitMenuItem();
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000367C File Offset: 0x0000187C
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

		// Token: 0x06000085 RID: 133 RVA: 0x00003710 File Offset: 0x00001910
		public void RegisterEvent()
		{
			this.eventAggregator = Services.EventsService;
			this.eventAggregator.GetEvent<DeleteVisualObjectsEvent>().Subscribe(new Action<ReadOnlyCollection<VisualObject>>(this.DeleteEventHandle));
			this.eventAggregator.GetEvent<CopyVisualObjectsEvent>().Subscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CopyEventHandle));
			this.eventAggregator.GetEvent<PasteVisualObjectsEvent>().Subscribe(new Action<PasteObjectsChangeEventArgs>(this.PasteEventHandle));
			this.eventAggregator.GetEvent<CutVisualObjectsEvent>().Subscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CutEventHandle));
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000379D File Offset: 0x0000199D
		protected override void OnShown()
		{
			base.OnShown();
			this.UpdateOperationSensitive(true);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000037B0 File Offset: 0x000019B0
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

		// Token: 0x06000088 RID: 136 RVA: 0x00003864 File Offset: 0x00001A64
		private void CopyEventHandle(ReadOnlyCollection<VisualObject> obj)
		{
			this.OnCopy(obj);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000386F File Offset: 0x00001A6F
		private void CutEventHandle(ReadOnlyCollection<VisualObject> obj)
		{
			this.OnCut(obj);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000387A File Offset: 0x00001A7A
		private void PasteEventHandle(PasteObjectsChangeEventArgs obj)
		{
			this.OnPaste(obj);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003885 File Offset: 0x00001A85
		private void DeleteEventHandle(ReadOnlyCollection<VisualObject> obj)
		{
			this.OnDelete(obj);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003890 File Offset: 0x00001A90
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

		// Token: 0x0600008D RID: 141 RVA: 0x00003964 File Offset: 0x00001B64
		protected virtual void OnCut(ReadOnlyCollection<VisualObject> objectList)
		{
			if (objectList != null && objectList.Count != 0)
			{
				this.CutObject();
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003994 File Offset: 0x00001B94
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

		// Token: 0x0600008F RID: 143 RVA: 0x00003ACC File Offset: 0x00001CCC
		protected virtual void OnPaste(PasteObjectsChangeEventArgs obj)
		{
			if (this.PasteMenuIsAction(this.CopyObjectList) && this.CheckObjectPaste(this.CopyObjectList))
			{
				PointF position = (obj == null) ? null : obj.PastePosition;
				this.PasteObject(position);
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003B18 File Offset: 0x00001D18
		public bool PasteMenuIsAction(IReadOnlyList<VisualObject> pasteList)
		{
			return pasteList != null && pasteList.Count != 0;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003B48 File Offset: 0x00001D48
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

		// Token: 0x06000092 RID: 146 RVA: 0x00003BE8 File Offset: 0x00001DE8
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

		// Token: 0x06000093 RID: 147 RVA: 0x00003D50 File Offset: 0x00001F50
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

		// Token: 0x06000094 RID: 148 RVA: 0x00003E24 File Offset: 0x00002024
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

		// Token: 0x06000095 RID: 149 RVA: 0x00004010 File Offset: 0x00002210
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

		// Token: 0x06000096 RID: 150 RVA: 0x000042A8 File Offset: 0x000024A8
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

		// Token: 0x06000097 RID: 151 RVA: 0x00004420 File Offset: 0x00002620
		private void SetObjectCenterPosition(IReadOnlyList<VisualObject> objectList)
		{
			if (objectList.Count == 1)
			{
				VisualObject visualObject = objectList.FirstOrDefault<VisualObject>();
				this.SingleObjectPosition = visualObject.TransformToScene(new PointF(visualObject.AnchorPoint.ScaleX * visualObject.Size.Width, visualObject.AnchorPoint.ScaleY * visualObject.Size.Height));
			}
			this.CopyObjectListCenter = ObjectRectangelHelper.GetObjectListCenter(objectList);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004494 File Offset: 0x00002694
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

		// Token: 0x06000099 RID: 153 RVA: 0x0000450C File Offset: 0x0000270C
		private bool IsFreeLayoutParent(AbstractNodeObject parent)
		{
			return !(parent is ListViewObject) && !(parent is PageViewObject);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004540 File Offset: 0x00002740
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

		// Token: 0x0600009B RID: 155 RVA: 0x00004640 File Offset: 0x00002840
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

		// Token: 0x0600009C RID: 156 RVA: 0x00004734 File Offset: 0x00002934
		public void UnregisterEvent()
		{
			this.eventAggregator = Services.EventsService;
			this.eventAggregator.GetEvent<DeleteVisualObjectsEvent>().Unsubscribe(new Action<ReadOnlyCollection<VisualObject>>(this.DeleteEventHandle));
			this.eventAggregator.GetEvent<CopyVisualObjectsEvent>().Unsubscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CopyEventHandle));
			this.eventAggregator.GetEvent<PasteVisualObjectsEvent>().Unsubscribe(new Action<PasteObjectsChangeEventArgs>(this.PasteEventHandle));
			this.eventAggregator.GetEvent<CutVisualObjectsEvent>().Unsubscribe(new Action<ReadOnlyCollection<VisualObject>>(this.CutEventHandle));
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000047C1 File Offset: 0x000029C1
		public override void Dispose()
		{
			this.UnregisterEvent();
			base.Dispose();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000047D4 File Offset: 0x000029D4
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

		// Token: 0x0600009F RID: 159 RVA: 0x00004825 File Offset: 0x00002A25
		public virtual void CanShow(ContextMenuShowingArgs args)
		{
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004828 File Offset: 0x00002A28
		public virtual string Type
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000483F File Offset: 0x00002A3F
		public virtual void Activated(CocosItem project)
		{
			this.RegisterEvent();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004849 File Offset: 0x00002A49
		public new virtual void Deactivated()
		{
			this.UnregisterEvent();
		}

		// Token: 0x0400002D RID: 45
		protected MenuItem menuItemCopyComponent;

		// Token: 0x0400002E RID: 46
		protected MenuItem menuItemPasteComponent;

		// Token: 0x0400002F RID: 47
		protected MenuItem menuItemCutComponent;

		// Token: 0x04000030 RID: 48
		protected MenuItem menuItemDeleteObject;

		// Token: 0x04000031 RID: 49
		protected IEventAggregator eventAggregator;

		// Token: 0x04000032 RID: 50
		private IUndoManager taskService;
	}
}
