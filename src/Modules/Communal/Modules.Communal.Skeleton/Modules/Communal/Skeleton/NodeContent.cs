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
	public class NodeContent : EventBox
	{
		public NodeContent ParentNode { get; set; }

		public List<NodeContent> ChildNodes { get; set; }

		public Point NodePoint { get; set; }

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

		public string LabelText
		{
			get
			{
				return this.label.Text;
			}
		}

		public int ZOrder { get; set; }

		public bool IsHasParent { get; set; }

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

		private FixedEx ParentContain
		{
			get
			{
				return base.Parent as FixedEx;
			}
		}

		public int StartPosition { get; set; }

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

		protected override bool OnEnterNotifyEvent(EventCrossing evnt)
		{
			this.SetEntryBgColor();
			return base.OnEnterNotifyEvent(evnt);
		}

		protected override bool OnLeaveNotifyEvent(EventCrossing evnt)
		{
			this.SetLeaveBgColor();
			return base.OnLeaveNotifyEvent(evnt);
		}

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

		public void DisposeContent()
		{
			this.currentObject.PropertyChanged -= this.currentObject_PropertyChanged;
		}

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

		public virtual void ResetBgColor()
		{
			this.isChoice = false;
			base.ModifyBg(StateType.Normal, NodeContent.bgColor);
		}

		public void SetChoiceBgColor()
		{
			this.isChoice = true;
			base.ModifyBg(StateType.Normal, NodeContent.choiceColor);
		}

		public virtual void SetEntryBgColor()
		{
			if (this.isChoice)
			{
				return;
			}
			base.ModifyBg(StateType.Normal, NodeContent.movColor);
		}

		public virtual void SetLeaveBgColor()
		{
			if (this.isChoice)
			{
				this.SetChoiceBgColor();
				return;
			}
			this.ResetBgColor();
		}

		private Label label = new Label();

		private static Color bgColor = new Color(70, 180, byte.MaxValue);

		private static Color normalFgColor = new Color(0, 0, 0);

		private static Color choiceColor = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private static Color movColor = new Color(174, 221, 253);

		private int lbHeight = 120;

		private int lbWidth = 20;

		private AbstractNodeObject currentObject;

		private int? treeDegree = null;

		private double scale = 1.0;

		protected bool isChoice;

		private NodeContent moveContent;

		private double pointx;

		private double pointy;

		private bool isPressLeft;
	}
}
