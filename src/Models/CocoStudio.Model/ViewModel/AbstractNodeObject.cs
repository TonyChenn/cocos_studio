using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.ViewModel
{
	[TypeExtensionPoint(ExtensionAttributeType = typeof(ModelExtensionAttribute), NodeType = typeof(ModelExtensionNode))]
	[EngineClassName("Node")]
	public abstract class AbstractNodeObject : VisualObject
	{
		public event EventHandler ParentChanged;

		public AbstractNodeObject Parent
		{
			get
			{
				return this.parent;
			}
			internal set
			{
				this.parent = value;
				this.RaisePropertyChanged<AbstractNodeObject>(() => this.Parent);
			}
		}

		[UndoProperty]
		public virtual NodeCollection Children { get; private set; }

		public AbstractNodeObject()
		{
			this.CreateCSObject();
			this.InitNode(false);
			this.InitData(false);
		}

		public AbstractNodeObject(ScriptFileData fileData)
		{
			this.ScriptData = fileData;
			bool scriptData = this.ScriptData != null;
			this.CreateCSObject();
			this.InitNode(scriptData);
			this.InitData(scriptData);
		}

		protected virtual void CreateCSObject()
		{
			this.innerNode = new CSNode();
		}

		private void InitNode(bool script)
		{
			if (script)
			{
				this.innerNode.SetScriptFile(this.ScriptData);
			}
			this.innerNode.Init(script);
		}

		internal override CSVisualObject GetCSVisual()
		{
			return this.innerNode;
		}

		protected virtual void InitData(bool useScript)
		{
			this.Tag = VisualObject.tag++;
			this.Children = new NodeCollection(this);
			this.CallBackType = EnumCallBack.None;
		}

		protected internal virtual string GetNamePrefix()
		{
			string name = base.GetType().Name;
			return name.Substring(0, name.Length - 6) + "_";
		}

		[UndoProperty]
		public override SizeF Size
		{
			get
			{
				return this.GetCSVisual().GetSize();
			}
			set
			{
				if (value != null)
				{
					this.GetCSVisual().SetSize(value);
					this.RaisePropertyChanged<SizeF>(() => this.Size);
				}
			}
		}

		[Browsable(true)]
		[UndoProperty]
		[Category("Group_Routine")]
		[DisplayName("Display_Name")]
		public override string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				base.Name = value;
			}
		}

		[Browsable(true)]
		[UndoProperty]
		[DefaultValue(-1)]
		[PropertyOrder(3)]
		[DisplayName("Display_Target")]
		[Category("Group_Routine")]
		public virtual int Tag
		{
			get
			{
				return this.tagAttr;
			}
			set
			{
				this.tagAttr = value;
				this.RaisePropertyChanged<int>(() => this.Tag);
			}
		}

		[UndoProperty]
		[Category("Group_Advanced")]
		[DisplayName("Display_FrameEvents")]
		[PropertyOrder(1002)]
		[Browsable(true)]
		[FrameProperty(typeof(EventFrame))]
		public virtual string FrameEvent
		{
			get
			{
				return this._frameEvent;
			}
			set
			{
				this._frameEvent = value;
				this.RaisePropertyChanged<string>(() => this.FrameEvent);
			}
		}

		[Browsable(true)]
		[PropertyOrder(1003)]
		[Category("Group_Advanced")]
		[DisplayName("Custom_UserData")]
		[UndoProperty]
		public virtual string UserData
		{
			get
			{
				return this.userData;
			}
			set
			{
				if (value != this.userData)
				{
					if (value == null)
					{
						value = "";
					}
					this.userData = value;
				}
				this.RaisePropertyChanged<string>(() => this.UserData);
			}
		}

		public override bool IsSelected
		{
			get
			{
				return base.IsSelected;
			}
			set
			{
				base.IsSelected = value;
				CSVisualObject.ObjectState objectState = value ? CSVisualObject.ObjectState.Seleted : CSVisualObject.ObjectState.Default;
				this.GetCSVisual().SetObjectState(objectState);
			}
		}

		public virtual ScriptFileData ScriptData { get; set; }

		public virtual string CustomClassName { get; set; }

		public virtual string CallBackName { get; set; }

		public virtual EnumCallBack CallBackType { get; set; }

		public bool IsAncestor(AbstractNodeObject obj)
		{
			return obj != null && (obj == this.Parent || (this.Parent != null && this.Parent.IsAncestor(obj)));
		}

		internal virtual void AncestorObjectChanged(BaseObject sourceObj, NotifyCollectionChangedAction action)
		{
			EventArgs e = null;
			if (action == NotifyCollectionChangedAction.Add)
			{
				Services.ProjectOperations.CurrentSelectedProject.AddName(this.Name);
				e = EventArgs.Empty;
			}
			else if (action == NotifyCollectionChangedAction.Remove)
			{
				Services.ProjectOperations.CurrentSelectedProject.RemoveName(this.Name);
			}
			if (this.ParentChanged != null)
			{
				this.ParentChanged(this, e);
			}
			if (this.Children != null)
			{
				foreach (AbstractNodeObject abstractNodeObject in this.Children)
				{
					abstractNodeObject.AncestorObjectChanged(sourceObj, action);
				}
			}
			if (base.Timelines != null)
			{
				foreach (Timeline timeline in base.Timelines)
				{
					timeline.AncestorObjectChanged(sourceObj, action);
				}
			}
		}

		internal virtual void InsertChild(int index, AbstractNodeObject nObject)
		{
			this.GetCSVisual().InsertChild(index, nObject.GetCSVisual());
			nObject.OperationFlag |= OperationMask.MoveFlag;
			nObject.Parent = this;
		}

		internal virtual void RemoveChild(AbstractNodeObject nObject)
		{
			this.GetCSVisual().RemoveChild(nObject.GetCSVisual());
			nObject.Parent = null;
		}

		internal virtual void AfterAdded()
		{
		}

		internal virtual void BeforeRemoved()
		{
		}

		public override IEnumerable<VisualObject> GetVisualChildren()
		{
			return this.Children;
		}

		public virtual PointF TransformSceneForChild(PointF scene)
		{
			return this.TransformToSelf(scene);
		}

		public override object Clone()
		{
			AbstractNodeObject abstractNodeObject = null;
			ModelMetaData metaData = ModelManager.Instance.GetMetaData(this);
			if (null != metaData)
			{
				abstractNodeObject = metaData.CreateObject();
			}
			else
			{
				abstractNodeObject = (Activator.CreateInstance(base.GetType()) as AbstractNodeObject);
			}
			this.SetValue(abstractNodeObject);
			this.SetAnimation(abstractNodeObject);
			foreach (AbstractNodeObject abstractNodeObject2 in this.Children)
			{
				AbstractNodeObject item = (AbstractNodeObject)abstractNodeObject2.Clone();
				abstractNodeObject.Children.Add(item);
			}
			return abstractNodeObject;
		}

		protected virtual void SetAnimation(object cObject)
		{
			foreach (Timeline timeline in base.Timelines)
			{
				Timeline timeline2 = Timeline.CreateTimeline(timeline.PropertyInfo, cObject as AbstractNodeObject);
				foreach (Frame frame in timeline.Frames)
				{
					Frame item = frame.Clone() as Frame;
					timeline2.Frames.Add(item);
				}
			}
		}

		protected virtual void SetValue(object cObject)
		{
			AbstractNodeObject abstractNodeObject = cObject as AbstractNodeObject;
			if (abstractNodeObject != null)
			{
				abstractNodeObject.ScriptData = this.ScriptData;
				abstractNodeObject.Name = this.Name;
				abstractNodeObject.CanEdit = this.CanEdit;
				abstractNodeObject.OperationFlag = this.OperationFlag;
				abstractNodeObject.Alpha = this.Alpha;
				abstractNodeObject.Visible = this.Visible;
				abstractNodeObject.ZOrder = this.ZOrder;
				abstractNodeObject.VisibleForFrame = this.VisibleForFrame;
				abstractNodeObject.Parent = this.Parent;
				abstractNodeObject.FrameEvent = this.FrameEvent;
				abstractNodeObject.CustomClassName = this.CustomClassName;
				abstractNodeObject.CallBackName = this.CallBackName;
				abstractNodeObject.CallBackType = this.CallBackType;
				abstractNodeObject.UserData = this.UserData;
			}
		}

		public virtual bool CanDrop(object node, TreeViewDropPosition mode, bool copy)
		{
			bool result;
			if (copy || !(node is AbstractNodeObject) || this.IsAncestor(node as AbstractNodeObject))
			{
				result = false;
			}
			else
			{
				AbstractNodeObject abstractNodeObject = node as AbstractNodeObject;
				switch (mode)
				{
				case TreeViewDropPosition.Before:
				case TreeViewDropPosition.After:
					if (this.Parent == null || !this.Parent.CanDrop(node, TreeViewDropPosition.IntoOrAfter, copy))
					{
						return false;
					}
					break;
				}
				result = true;
			}
			return result;
		}

		public virtual CSVisualObject.ObjectState ObjectBoudingState
		{
			get
			{
				return this.GetCSVisual().GetObjectState();
			}
			set
			{
				this.GetCSVisual().SetObjectState(value);
			}
		}

		public virtual bool CanReceiveDragObject(ModelDragData objectData, bool showLog)
		{
			return false;
		}

		public virtual bool CanReceiveDragResource(ResourceInfoDragData objectData, bool showLog)
		{
			bool result;
			if (objectData == null)
			{
				result = false;
			}
			else
			{
				GameFile gameFile = Services.ProjectOperations.CurrentSelectedProject.CocosFile as GameFile;
				result = (gameFile != null);
			}
			return result;
		}

		internal void ApplyCachedWorldMatrix()
		{
			if (this.cachedWorldMatix == null)
			{
				this.CacheWorldMatrix();
			}
			else
			{
				this.GetCSVisual().ApplySelfWorldMatirx(this.cachedWorldMatix);
				this.RaisePropertyChanged<PointF>(() => this.Position, false);
				this.RaisePropertyChanged<ScaleValue>(() => this.RotationSkew, false);
				this.RaisePropertyChanged<float>(() => this.Rotation, false);
				this.RaisePropertyChanged<ScaleValue>(() => this.Scale, false);
			}
		}

		internal void CacheWorldMatrix()
		{
			this.cachedWorldMatix = this.GetCSVisual().GetAnchorWorldMatrix();
		}

		protected CSNode innerNode;

		private AbstractNodeObject parent;

		private int tagAttr = 0;

		private string _frameEvent = string.Empty;

		private string userData;

		private CSMatrix cachedWorldMatix;
	}
}
