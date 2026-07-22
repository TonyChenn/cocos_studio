using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Lib.Prism;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Event;
using CocoStudio.Model.Interface;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model
{
	// Token: 0x020000DA RID: 218
	public abstract class DragOperation : IDragOperation
	{
		// Token: 0x060006C1 RID: 1729 RVA: 0x0001AE54 File Offset: 0x00019054
		public void DragEnter(DragMotionArgs e, VisualObject target)
		{
			AbstractNodeObject abstractNodeObject = target as AbstractNodeObject;
			if (abstractNodeObject != null)
			{
				this.OldObjectState = abstractNodeObject.ObjectBoudingState;
				if (abstractNodeObject.CanReceiveDragObject(e.Context.GetDragData() as ModelDragData, this.dragEnter))
				{
					abstractNodeObject.ObjectBoudingState = CSVisualObject.ObjectState.DragOver;
				}
				this.dragEnter = true;
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001AEB8 File Offset: 0x000190B8
		public void DragLeave(DragMotionArgs e, VisualObject target)
		{
			AbstractNodeObject abstractNodeObject = target as AbstractNodeObject;
			if (abstractNodeObject != null)
			{
				abstractNodeObject.ObjectBoudingState = this.OldObjectState;
				this.OldObjectState = CSVisualObject.ObjectState.Default;
				e.SetAllowDragAction((DragAction)0);
				this.dragEnter = false;
			}
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001AF00 File Offset: 0x00019100
		public bool DragOver(DragMotionArgs e, VisualObject target)
		{
			AbstractNodeObject abstractNodeObject = target as AbstractNodeObject;
			bool result;
			if (abstractNodeObject == null)
			{
				result = false;
			}
			else
			{
				if (e.Context.GetDataPresent(typeof(ResourceInfoDragData)))
				{
					if (!abstractNodeObject.CanReceiveDragResource(e.Context.GetDragData() as ResourceInfoDragData, this.dragEnter))
					{
						e.SetAllowDragAction((DragAction)0);
						return false;
					}
				}
				this.dragEnter = false;
				result = true;
			}
			return result;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001AF7C File Offset: 0x0001917C
		public void DragDrop(DragDropArgs e, VisualObject target)
		{
			AbstractNodeObject abstractNodeObject = target as AbstractNodeObject;
			if (abstractNodeObject != null)
			{
				if (e.Context.GetDataPresent(typeof(ResourceInfoDragData)))
				{
					string text = this.CanDragDropArgs(e);
					if (text != null)
					{
						LogConfig.Output.Error(text, null);
						return;
					}
				}
				using (CompositeTask.Run("Drag drop GameObject", null))
				{
					List<VisualObject> list = new List<VisualObject>();
					if (e.Context.GetDataPresent(typeof(ResourceInfoDragData)))
					{
						ResourceInfoDragData resourceInfoDragData = (ResourceInfoDragData)e.Context.GetDragData();
						if (resourceInfoDragData == null)
						{
							return;
						}
						if (abstractNodeObject.CanReceiveDragResource(resourceInfoDragData, this.dragEnter))
						{
							abstractNodeObject = Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
						}
						foreach (ResourceItem resourceItem in resourceInfoDragData.Items)
						{
							if (!(resourceItem is ResourceFolder))
							{
								AbstractNodeObject abstractNodeObject2 = this.CreateObjectFromFile(resourceItem);
								if (abstractNodeObject2 != null)
								{
									this.AddChildToTarget(abstractNodeObject2, new PointF((float)e.X, (float)e.Y), abstractNodeObject);
									list.Add(abstractNodeObject2);
								}
							}
						}
					}
					else if (e.Context.GetDataPresent(typeof(ModelDragData)))
					{
						ModelDragData modelDragData = (ModelDragData)e.Context.GetDragData();
						AbstractNodeObject abstractNodeObject3 = modelDragData.MetaData.CreateObject();
						if (!abstractNodeObject.CanReceiveDragObject(modelDragData, this.dragEnter))
						{
							abstractNodeObject = Services.ProjectOperations.CurrentSelectedProject.GetRootNode();
						}
						if (abstractNodeObject3 != null)
						{
							this.AddChildToTarget(abstractNodeObject3, new PointF((float)e.X, (float)e.Y), abstractNodeObject);
							list.Add(abstractNodeObject3);
						}
					}
					abstractNodeObject.ObjectBoudingState = CSVisualObject.ObjectState.Default;
					EventAggregator.Instance.GetEvent<SelectedVisualObjectsChangeEvent>().Publish(new SelectedVisualObjectsChangeEventArgs(list, list, false));
				}
				this.dragEnter = false;
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001B20C File Offset: 0x0001940C
		protected virtual string CanDragDropArgs(DragDropArgs e)
		{
			ResourceInfoDragData resourceInfoDragData = e.Context.GetDragData() as ResourceInfoDragData;
			GameFile gameFile = Services.ProjectOperations.CurrentSelectedProject.CocosFile as GameFile;
			foreach (ResourceItem resourceItem in resourceInfoDragData.Items)
			{
				if (resourceItem.FullPath == gameFile.FileName)
				{
					return LanguageInfo.MessageBox207_NestedSelfError;
				}
				CocosItem cocosItem = resourceItem as CocosItem;
				string text;
				if (cocosItem != null)
				{
					text = cocosItem.CheckNest(gameFile.CocosItem);
				}
				else
				{
					string fileType = Services.ProjectOperations.CurrentSelectedProject.GetFileType();
					if (NodeType.Scene3D.ToString() == fileType)
					{
						text = this.Check3DProject(resourceItem);
					}
					else
					{
						text = this.Check2DProject(resourceItem);
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001B340 File Offset: 0x00019540
		private string Check2DProject(ResourceItem item)
		{
			string result;
			if (item is MeshFile || item is PuFile || item is Sprite3DFile)
			{
				result = LanguageInfo.MessageBox246_3DObjectIn2DScene;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0001B380 File Offset: 0x00019580
		private string Check3DProject(ResourceItem item)
		{
			string result;
			if (item is MeshFile || item is PuFile || item is Sprite3DFile)
			{
				result = null;
			}
			else
			{
				result = LanguageInfo.MessageBox245_2DObjectIn3DScene;
			}
			return result;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0001B3C0 File Offset: 0x000195C0
		protected virtual AbstractNodeObject CreateObjectFromFile(ResourceItem resourceFile)
		{
			return null;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001B3D3 File Offset: 0x000195D3
		protected virtual void AddChildToTarget(AbstractNodeObject childNode, PointF coord, AbstractNodeObject target)
		{
		}

		// Token: 0x060006CA RID: 1738
		public abstract bool CanHandle(CocosItem project);

		// Token: 0x040002DE RID: 734
		private bool dragEnter = false;

		// Token: 0x040002DF RID: 735
		private CSVisualObject.ObjectState OldObjectState = CSVisualObject.ObjectState.Default;
	}
}
