using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using Gdk;
using Gtk;

namespace Modules.Communal.Skeleton
{
	// Token: 0x0200001E RID: 30
	public class NodeContent : EventBox
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0000677C File Offset: 0x0000497C
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00006784 File Offset: 0x00004984
		public NodeContent ParentNode { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000678D File Offset: 0x0000498D
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00006795 File Offset: 0x00004995
		public List<NodeContent> ChildNodes { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000124 RID: 292 RVA: 0x0000679E File Offset: 0x0000499E
		// (set) Token: 0x06000125 RID: 293 RVA: 0x000067A6 File Offset: 0x000049A6
		public Point NodePoint { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000126 RID: 294 RVA: 0x000067AF File Offset: 0x000049AF
		// (set) Token: 0x06000127 RID: 295 RVA: 0x000067B7 File Offset: 0x000049B7
		public AbstractNodeObject CurrentObject
		{
			get
			{
				return this.currentObject;
			}
			set
			{
				this.currentObject = value;
				if (this.currentObject.IsSelected)
				{
					this.SetChoiceBgColor();
				}
				this.currentObject.PropertyChanged += this.currentObject_PropertyChanged;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000128 RID: 296 RVA: 0x000067F4 File Offset: 0x000049F4
		// (set) Token: 0x06000129 RID: 297 RVA: 0x000068BB File Offset: 0x00004ABB
		public int? TreeDegree
		{
			get
			{
				if (this.treeDegree == null)
				{
					if (this.ChildNodes == null || this.ChildNodes.Count == 0)
					{
						this.treeDegree = new int?((int)(this.Scale * 80.0));
					}
					else
					{
						this.treeDegree = this.ChildNodes.Sum((NodeContent w) => w.TreeDegree) + (this.ChildNodes.Count - 1) * (int)(this.Scale * 20.0);
					}
				}
				return this.treeDegree;
			}
			set
			{
				this.treeDegree = null;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600012A RID: 298 RVA: 0x000068C9 File Offset: 0x00004AC9
		// (set) Token: 0x0600012B RID: 299 RVA: 0x000068D1 File Offset: 0x00004AD1
		public double Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				if (this.scale != value)
				{
					this.scale = value;
					this.label.SetFontSize(this.scale * 12.0);
				}
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600012C RID: 300 RVA: 0x000068FE File Offset: 0x00004AFE
		public string LabelText
		{
			get
			{
				return this.label.Text;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600012D RID: 301 RVA: 0x0000690B File Offset: 0x00004B0B
		// (set) Token: 0x0600012E RID: 302 RVA: 0x00006913 File Offset: 0x00004B13
		public int ZOrder { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000691C File Offset: 0x00004B1C
		// (set) Token: 0x06000130 RID: 304 RVA: 0x00006924 File Offset: 0x00004B24
		public bool IsHasParent { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000131 RID: 305 RVA: 0x0000692D File Offset: 0x00004B2D
		// (set) Token: 0x06000132 RID: 306 RVA: 0x00006935 File Offset: 0x00004B35
		public bool IsChoice
		{
			get
			{
				return this.isChoice;
			}
			protected set
			{
				this.isChoice = value;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000133 RID: 307 RVA: 0x0000693E File Offset: 0x00004B3E
		private FixedEx ParentContain
		{
			get
			{
				return base.Parent as FixedEx;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000134 RID: 308 RVA: 0x0000694B File Offset: 0x00004B4B
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00006953 File Offset: 0x00004B53
		public int StartPosition { get; set; }

		// Token: 0x06000136 RID: 310 RVA: 0x0000695C File Offset: 0x00004B5C
		public NodeContent(string contentStr = "default")
		{
			base.Add(this.label);
			base.ShowAll();
			this.label.WidthRequest = this.lbHeight;
			this.label.HeightRequest = this.lbWidth;
			this.label.Text = contentStr;
			this.label.ModifyFg(StateType.Normal, NodeContent.normalFgColor);
			base.ButtonPressEvent += this.NodeContent_ButtonPressEvent;
			base.ButtonReleaseEvent += this.NodeContent_ButtonReleaseEvent;
			base.MotionNotifyEvent += this.NodeContent_MotionNotifyEvent;
			base.ModifyBg(StateType.Normal, NodeContent.bgColor);
			base.TooltipText = contentStr;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006A3F File Offset: 0x00004C3F
		protected override bool OnEnterNotifyEvent(EventCrossing evnt)
		{
			this.SetEntryBgColor();
			return base.OnEnterNotifyEvent(evnt);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00006A4E File Offset: 0x00004C4E
		protected override bool OnLeaveNotifyEvent(EventCrossing evnt)
		{
			this.SetLeaveBgColor();
			return base.OnLeaveNotifyEvent(evnt);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006A64 File Offset: 0x00004C64
		private void currentObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsSelected")
			{
				if (this.ParentContain == null)
				{
					return;
				}
				if (this.currentObject.IsSelected)
				{
					this.SetChoiceBgColor();
					if (this.ParentContain.SelectList.FirstOrDefault((NodeContent w) => w == this) == null)
					{
						this.ParentContain.SelectList.Add(this);
						return;
					}
				}
				else
				{
					this.ResetBgColor();
					this.ParentContain.SelectList.Remove(this);
				}
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006AEE File Offset: 0x00004CEE
		public void DisposeContent()
		{
			this.currentObject.PropertyChanged -= this.currentObject_PropertyChanged;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006B08 File Offset: 0x00004D08
		private void SetChildContentPosition(NodeContent content, int x, int y)
		{
			foreach (NodeContent nodeContent in content.ChildNodes)
			{
				if (!nodeContent.IsChoice)
				{
					Fixed.FixedChild fixedChild = (Fixed.FixedChild)this.ParentContain[nodeContent];
					fixedChild.X += x;
					fixedChild.Y += y;
					nodeContent.NodePoint = new Point(fixedChild.X, fixedChild.Y);
					this.SetChildContentPosition(nodeContent, x, y);
				}
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006BAC File Offset: 0x00004DAC
		private void NodeContent_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			NodeContent nodeContent = o as NodeContent;
			if (this.isPressLeft)
			{
				if (this.ParentContain.CurrentType == ChoiceType.Normal)
				{
					double num = args.Event.X - this.pointx;
					double num2 = args.Event.Y - this.pointy;
					foreach (NodeContent nodeContent2 in this.ParentContain.SelectList)
					{
						Fixed.FixedChild fixedChild = (Fixed.FixedChild)this.ParentContain[nodeContent2];
						fixedChild.X += (int)num;
						fixedChild.Y += (int)num2;
						nodeContent2.NodePoint = new Point(fixedChild.X, fixedChild.Y);
						this.SetChildContentPosition(nodeContent2, (int)num, (int)num2);
					}
					this.ParentContain.QueueDraw();
				}
			}
			else if (this.ParentContain.IsBinding)
			{
				NodeContent endContent = this.ParentContain.GetEndContent(this.ParentContain.RootNodeContent, (double)this.NodePoint.X + args.Event.X, (double)this.NodePoint.Y + args.Event.Y);
				if (this.moveContent != null && endContent != this.moveContent)
				{
					this.moveContent.SetLeaveBgColor();
				}
				if (endContent != null && endContent.CurrentObject != this.CurrentObject && !(endContent is SkinContent) && !endContent.CurrentObject.IsAncestor(this.currentObject))
				{
					endContent.SetEntryBgColor();
					this.moveContent = endContent;
				}
			}
			this.ParentContain.endDrawX = nodeContent.NodePoint.X + (int)args.Event.X;
			this.ParentContain.endDrawY = nodeContent.NodePoint.Y + (int)args.Event.Y;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006DA8 File Offset: 0x00004FA8
		private void NodeContent_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			this.isPressLeft = false;
			if (this.ParentContain.IsBinding)
			{
				this.ParentContain.IsBinding = false;
				NodeContent endContent = this.ParentContain.GetEndContent(this.ParentContain.RootNodeContent, (double)this.NodePoint.X + args.Event.X, (double)this.NodePoint.Y + args.Event.Y);
				if (endContent != null && endContent.CurrentObject != this.CurrentObject)
				{
					BoneObject boneObject = endContent.CurrentObject as BoneObject;
					if (boneObject != null && this.CurrentObject.Parent != boneObject)
					{
						BindingBoneTool.BindingNodeToBone(this.CurrentObject, boneObject);
					}
				}
				if (this.ParentContain != null)
				{
					this.ParentContain.QueueDraw();
				}
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006E8C File Offset: 0x0000508C
		private void NodeContent_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			NodeContent control = o as NodeContent;
			if (args.Event.Button == 1U)
			{
				if (!KeyboardExtend.IsModifyKeyPressed(ModifierType.ControlMask))
				{
					if (this.ParentContain.SelectList.FirstOrDefault((NodeContent w) => w == control) == null || this.ParentContain.CurrentType == ChoiceType.Binding)
					{
						this.ParentContain.SelectList.Clear();
						this.currentObject.IsSelected = true;
					}
				}
				else if (control.IsChoice)
				{
					this.currentObject.IsSelected = false;
				}
				else if (this.ParentContain.SelectList.FirstOrDefault((NodeContent w) => w == control) == null)
				{
					this.currentObject.IsSelected = true;
				}
				foreach (NodeContent nodeContent in this.ParentContain.SelectList)
				{
					if (this.GetParent(nodeContent.CurrentObject) != null)
					{
						nodeContent.IsHasParent = true;
					}
					else
					{
						nodeContent.IsHasParent = false;
					}
				}
				List<VisualObject> list = new List<VisualObject>();
				List<VisualObject> list2 = new List<VisualObject>();
				foreach (NodeContent nodeContent2 in this.ParentContain.SelectList)
				{
					if (!nodeContent2.IsHasParent)
					{
						list2.Add(nodeContent2.CurrentObject);
					}
					list.Add(nodeContent2.CurrentObject);
				}
				Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>().Publish(new SelectedVisualObjectsChangeEventArgs(list, list2, false));
				this.pointx = args.Event.X;
				this.pointy = args.Event.Y;
				if (this.ParentContain.CurrentType == ChoiceType.Normal)
				{
					this.isPressLeft = true;
					this.ParentContain.IsBinding = false;
					return;
				}
				this.ParentContain.startDrawX = (this.ParentContain.endDrawX = control.NodePoint.X + (int)this.pointx);
				this.ParentContain.startDrawY = (this.ParentContain.endDrawY = control.NodePoint.Y + (int)this.pointy);
				this.ParentContain.IsBinding = true;
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000712C File Offset: 0x0000532C
		private AbstractNodeObject GetParent(AbstractNodeObject obj)
		{
			if (obj.Parent != null)
			{
				NodeContent nodeContent = this.ParentContain.SelectList.FirstOrDefault((NodeContent w) => w.CurrentObject == obj.Parent);
				if (nodeContent != null)
				{
					return nodeContent.CurrentObject;
				}
			}
			return null;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007182 File Offset: 0x00005382
		public virtual void ResetBgColor()
		{
			this.isChoice = false;
			base.ModifyBg(StateType.Normal, NodeContent.bgColor);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00007197 File Offset: 0x00005397
		public void SetChoiceBgColor()
		{
			this.isChoice = true;
			base.ModifyBg(StateType.Normal, NodeContent.choiceColor);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000071AC File Offset: 0x000053AC
		public virtual void SetEntryBgColor()
		{
			if (this.isChoice)
			{
				return;
			}
			base.ModifyBg(StateType.Normal, NodeContent.movColor);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000071C3 File Offset: 0x000053C3
		public virtual void SetLeaveBgColor()
		{
			if (this.isChoice)
			{
				this.SetChoiceBgColor();
				return;
			}
			this.ResetBgColor();
		}

		// Token: 0x0400004B RID: 75
		private Label label = new Label();

		// Token: 0x0400004C RID: 76
		private static Color bgColor = new Color(70, 180, byte.MaxValue);

		// Token: 0x0400004D RID: 77
		private static Color normalFgColor = new Color(0, 0, 0);

		// Token: 0x0400004E RID: 78
		private static Color choiceColor = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x0400004F RID: 79
		private static Color movColor = new Color(174, 221, 253);

		// Token: 0x04000050 RID: 80
		private int lbHeight = 120;

		// Token: 0x04000051 RID: 81
		private int lbWidth = 20;

		// Token: 0x04000052 RID: 82
		private AbstractNodeObject currentObject;

		// Token: 0x04000053 RID: 83
		private int? treeDegree = null;

		// Token: 0x04000054 RID: 84
		private double scale = 1.0;

		// Token: 0x04000055 RID: 85
		protected bool isChoice;

		// Token: 0x04000056 RID: 86
		private NodeContent moveContent;

		// Token: 0x04000057 RID: 87
		private double pointx;

		// Token: 0x04000058 RID: 88
		private double pointy;

		// Token: 0x04000059 RID: 89
		private bool isPressLeft;
	}
}
