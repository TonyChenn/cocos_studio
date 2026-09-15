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
	public abstract class DragOperation : IDragOperation
	{
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

		protected virtual AbstractNodeObject CreateObjectFromFile(ResourceItem resourceFile)
		{
			return null;
		}

		protected virtual void AddChildToTarget(AbstractNodeObject childNode, PointF coord, AbstractNodeObject target)
		{
		}

		public abstract bool CanHandle(CocosItem project);

		private bool dragEnter = false;

		private CSVisualObject.ObjectState OldObjectState = CSVisualObject.ObjectState.Default;
	}
}
