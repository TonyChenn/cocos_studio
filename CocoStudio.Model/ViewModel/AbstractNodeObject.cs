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
	// Token: 0x020000DE RID: 222
	[TypeExtensionPoint(ExtensionAttributeType = typeof(ModelExtensionAttribute), NodeType = typeof(ModelExtensionNode))]
	[EngineClassName("Node")]
	public abstract class AbstractNodeObject : VisualObject
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060006D8 RID: 1752 RVA: 0x0001C0F4 File Offset: 0x0001A2F4
		// (remove) Token: 0x060006D9 RID: 1753 RVA: 0x0001C130 File Offset: 0x0001A330
		public event EventHandler ParentChanged;

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0001C16C File Offset: 0x0001A36C
		// (set) Token: 0x060006DB RID: 1755 RVA: 0x0001C184 File Offset: 0x0001A384
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

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0001C1D4 File Offset: 0x0001A3D4
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x0001C1EB File Offset: 0x0001A3EB
		[UndoProperty]
		public virtual NodeCollection Children { get; private set; }

		// Token: 0x060006DE RID: 1758 RVA: 0x0001C1F4 File Offset: 0x0001A3F4
		public AbstractNodeObject()
		{
			this.CreateCSObject();
			this.InitNode(false);
			this.InitData(false);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001C228 File Offset: 0x0001A428
		public AbstractNodeObject(ScriptFileData fileData)
		{
			this.ScriptData = fileData;
			bool scriptData = this.ScriptData != null;
			this.CreateCSObject();
			this.InitNode(scriptData);
			this.InitData(scriptData);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001C27D File Offset: 0x0001A47D
		protected virtual void CreateCSObject()
		{
			this.innerNode = new CSNode();
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0001C28C File Offset: 0x0001A48C
		private void InitNode(bool script)
		{
			if (script)
			{
				this.innerNode.SetScriptFile(this.ScriptData);
			}
			this.innerNode.Init(script);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001C2C4 File Offset: 0x0001A4C4
		internal override CSVisualObject GetCSVisual()
		{
			return this.innerNode;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001C2DC File Offset: 0x0001A4DC
		protected virtual void InitData(bool useScript)
		{
			this.Tag = VisualObject.tag++;
			this.Children = new NodeCollection(this);
			this.CallBackType = EnumCallBack.None;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001C308 File Offset: 0x0001A508
		protected internal virtual string GetNamePrefix()
		{
			string name = base.GetType().Name;
			return name.Substring(0, name.Length - 6) + "_";
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0001C340 File Offset: 0x0001A540
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x0001C360 File Offset: 0x0001A560
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

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x0001C3C4 File Offset: 0x0001A5C4
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x0001C3DC File Offset: 0x0001A5DC
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

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0001C3E8 File Offset: 0x0001A5E8
		// (set) Token: 0x060006EA RID: 1770 RVA: 0x0001C400 File Offset: 0x0001A600
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

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x0001C450 File Offset: 0x0001A650
		// (set) Token: 0x060006EC RID: 1772 RVA: 0x0001C468 File Offset: 0x0001A668
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

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x0001C4B8 File Offset: 0x0001A6B8
		// (set) Token: 0x060006EE RID: 1774 RVA: 0x0001C4D0 File Offset: 0x0001A6D0
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

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x0001C54C File Offset: 0x0001A74C
		// (set) Token: 0x060006F0 RID: 1776 RVA: 0x0001C564 File Offset: 0x0001A764
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

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0001C590 File Offset: 0x0001A790
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x0001C5A7 File Offset: 0x0001A7A7
		public virtual ScriptFileData ScriptData { get; set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0001C5B0 File Offset: 0x0001A7B0
		// (set) Token: 0x060006F4 RID: 1780 RVA: 0x0001C5C7 File Offset: 0x0001A7C7
		public virtual string CustomClassName { get; set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0001C5D0 File Offset: 0x0001A7D0
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x0001C5E7 File Offset: 0x0001A7E7
		public virtual string CallBackName { get; set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0001C5F0 File Offset: 0x0001A7F0
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x0001C607 File Offset: 0x0001A807
		public virtual EnumCallBack CallBackType { get; set; }

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001C610 File Offset: 0x0001A810
		public bool IsAncestor(AbstractNodeObject obj)
		{
			return obj != null && (obj == this.Parent || (this.Parent != null && this.Parent.IsAncestor(obj)));
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001C664 File Offset: 0x0001A864
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

		// Token: 0x060006FB RID: 1787 RVA: 0x0001C7A0 File Offset: 0x0001A9A0
		internal virtual void InsertChild(int index, AbstractNodeObject nObject)
		{
			this.GetCSVisual().InsertChild(index, nObject.GetCSVisual());
			nObject.OperationFlag |= OperationMask.MoveFlag;
			nObject.Parent = this;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001C7CD File Offset: 0x0001A9CD
		internal virtual void RemoveChild(AbstractNodeObject nObject)
		{
			this.GetCSVisual().RemoveChild(nObject.GetCSVisual());
			nObject.Parent = null;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001C7EA File Offset: 0x0001A9EA
		internal virtual void AfterAdded()
		{
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001C7ED File Offset: 0x0001A9ED
		internal virtual void BeforeRemoved()
		{
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001C7F0 File Offset: 0x0001A9F0
		public override IEnumerable<VisualObject> GetVisualChildren()
		{
			return this.Children;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001C808 File Offset: 0x0001AA08
		public virtual PointF TransformSceneForChild(PointF scene)
		{
			return this.TransformToSelf(scene);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001C824 File Offset: 0x0001AA24
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

		// Token: 0x06000702 RID: 1794 RVA: 0x0001C8E8 File Offset: 0x0001AAE8
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

		// Token: 0x06000703 RID: 1795 RVA: 0x0001C9C0 File Offset: 0x0001ABC0
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

		// Token: 0x06000704 RID: 1796 RVA: 0x0001CA9C File Offset: 0x0001AC9C
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

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x0001CB14 File Offset: 0x0001AD14
		// (set) Token: 0x06000706 RID: 1798 RVA: 0x0001CB31 File Offset: 0x0001AD31
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

		// Token: 0x06000707 RID: 1799 RVA: 0x0001CB44 File Offset: 0x0001AD44
		public virtual bool CanReceiveDragObject(ModelDragData objectData, bool showLog)
		{
			return false;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0001CB58 File Offset: 0x0001AD58
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

		// Token: 0x06000709 RID: 1801 RVA: 0x0001CBA0 File Offset: 0x0001ADA0
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

		// Token: 0x0600070A RID: 1802 RVA: 0x0001CCD0 File Offset: 0x0001AED0
		internal void CacheWorldMatrix()
		{
			this.cachedWorldMatix = this.GetCSVisual().GetAnchorWorldMatrix();
		}

		// Token: 0x040002EA RID: 746
		protected CSNode innerNode;

		// Token: 0x040002EB RID: 747
		private AbstractNodeObject parent;

		// Token: 0x040002EC RID: 748
		private int tagAttr = 0;

		// Token: 0x040002ED RID: 749
		private string _frameEvent = string.Empty;

		// Token: 0x040002EE RID: 750
		private string userData;

		// Token: 0x040002EF RID: 751
		private CSMatrix cachedWorldMatix;
	}
}
