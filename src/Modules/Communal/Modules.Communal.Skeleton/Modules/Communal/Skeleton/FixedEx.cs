using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Cairo;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using Gdk;
using Gtk;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Skeleton
{
	public class FixedEx : Fixed
	{
		public bool IsBinding { get; set; }

		private void FixedDraw_ExposeEvent(object o, ExposeEventArgs args)
		{
			this.DrawLine();
		}

		public void DrawLine()
		{
			this.DrawContent(this.rootNodeContent);
		}

		private void DrawContent(NodeContent content)
		{
			if (content == null)
			{
				return;
			}
			using (Context context = CairoHelper.Create(base.GdkWindow))
			{
				context.Antialias = Antialias.None;
				context.LineJoin = LineJoin.Bevel;
				context.SetSourceRGBA(1.0, 1.0, 1.0, 0.8999999761581421);
				if (content.ChildNodes != null)
				{
					foreach (NodeContent nodeContent in content.ChildNodes)
					{
						this.VerfyLine(context, content.NodePoint.X + (int)((double)FixedEx.xOffer * this.CurrentScale), content.NodePoint.Y + (int)((double)FixedEx.yOffer * this.CurrentScale), nodeContent.NodePoint.X + (int)((double)FixedEx.xOffer * this.CurrentScale), nodeContent.NodePoint.Y + (int)((double)FixedEx.yOffer * this.CurrentScale));
						this.DrawContent(nodeContent);
					}
				}
				if (this.IsBinding && this.CurrentType == ChoiceType.Binding)
				{
					context.SetSourceRGBA(255.0, 255.0, 0.0, 255.0);
					context.MoveTo((double)this.startDrawX, (double)this.startDrawY);
					context.LineTo((double)this.endDrawX, (double)this.endDrawY);
					context.Stroke();
				}
			}
		}

		private void VerfyLine(Context cr, int x, int y, int x1, int y1)
		{
			int num = (y + y1) / 2;
			cr.MoveTo((double)x, (double)y);
			cr.LineTo((double)x, (double)num);
			cr.MoveTo((double)x, (double)num);
			cr.LineTo((double)x1, (double)num);
			cr.MoveTo((double)x1, (double)num);
			cr.LineTo((double)x1, (double)y1);
			cr.Stroke();
		}

		public SkeletonGraphDialog SkeletonDialog { private get; set; }

		private void FixedEx_Destroyed(object sender, EventArgs e)
		{
			this.DisposeRootNode(null);
			this.DisposeContentNode(null);
		}

		private void DisposeRootNode(AbstractNodeObject node = null)
		{
			if (node == null)
			{
				foreach (NodeContent nodeContent in this.NodeList)
				{
					nodeContent.CurrentObject.Children.CollectionChanged -= this.Children_CollectionChanged;
				}
				return;
			}
			node.Children.CollectionChanged -= this.Children_CollectionChanged;
			foreach (AbstractNodeObject abstractNodeObject in node.Children)
			{
				abstractNodeObject.Children.CollectionChanged -= this.Children_CollectionChanged;
				this.DisposeRootNode(abstractNodeObject);
			}
		}

		private void DisposeContentNode(NodeContent content = null)
		{
			if (content == null)
			{
				foreach (Widget widget in base.Children)
				{
					((NodeContent)widget).DisposeContent();
				}
				return;
			}
			content.DisposeContent();
			foreach (NodeContent content2 in content.ChildNodes)
			{
				this.DisposeContentNode(content2);
			}
		}

		internal void SetOrderListView(BoneOrderList listview)
		{
			this._orderListView = listview;
		}

		private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Remove)
			{
				IList oldItems = e.OldItems;
				foreach (object obj3 in oldItems)
				{
					AbstractNodeObject obj = obj3 as AbstractNodeObject;
					BoneObject boneObject = obj as BoneObject;
					if (boneObject != null)
					{
						this._orderListView.RemoveBoneItem(boneObject);
					}
					NodeContent current = this.NodeList.FirstOrDefault((NodeContent w) => w.CurrentObject == obj);
					if (current != null)
					{
						this.DisposeRootNode(obj);
						NodeContent nodeContent = this.NodeList.FirstOrDefault((NodeContent w) => w == current.ParentNode);
						nodeContent.ChildNodes.Remove(current);
						this.RemoveContent(current);
						nodeContent.TreeDegree = null;
					}
				}
				this.RefreshDialog();
			}
			else if (e.Action == NotifyCollectionChangedAction.Add)
			{
				IList newItems = e.NewItems;
				foreach (object obj2 in newItems)
				{
					AbstractNodeObject obj = obj2 as AbstractNodeObject;
					BoneObject boneObject2 = obj as BoneObject;
					if (boneObject2 != null)
					{
						this._orderListView.AddBoneItem(boneObject2);
					}
					NodeContent nodeContent2 = this.NodeList.FirstOrDefault((NodeContent w) => w.CurrentObject == obj.Parent);
					if (nodeContent2 != null)
					{
						this.GetNodeContent(nodeContent2, obj, 0);
						if (nodeContent2.ParentNode != null)
						{
							nodeContent2.ParentNode.TreeDegree = null;
							nodeContent2.TreeDegree = null;
							this.RecursionContent(nodeContent2.ParentNode, nodeContent2.ParentNode.NodePoint.X, nodeContent2.ParentNode.NodePoint.Y, nodeContent2.ParentNode.StartPosition);
						}
						else
						{
							nodeContent2.TreeDegree = null;
							this.RecursionContent(nodeContent2, nodeContent2.NodePoint.X, nodeContent2.NodePoint.Y, nodeContent2.StartPosition);
						}
					}
				}
				this.RefreshDialog();
			}
			base.QueueDraw();
		}

		private void RefreshDialog()
		{
			if (this.SkeletonDialog != null)
			{
				int num = (from w in this.NodeList
				where w is BoneContent
				select w).Count<NodeContent>();
				this.SkeletonDialog.BoneSkinSum(num, this.NodeList.Count - num - 1);
			}
		}

		private void RemoveContent(NodeContent content)
		{
			foreach (NodeContent content2 in content.ChildNodes)
			{
				this.RemoveContent(content2);
			}
			this.NodeList.Remove(content);
			base.Remove(content);
		}

		public NodeContent RootNodeContent
		{
			get
			{
				return this.rootNodeContent;
			}
		}

		public ChoiceType CurrentType
		{
			get
			{
				return this.currentType;
			}
			set
			{
				this.currentType = value;
			}
		}

		public double CurrentScale
		{
			get
			{
				return this.currentScale;
			}
			set
			{
				if (this.currentScale != value)
				{
					this.currentScale = value;
					this.InitData();
				}
			}
		}

		public FixedEx()
		{
			base.ModifyBg(StateType.Normal, new Gdk.Color(63, 63, 63));
			base.ExposeEvent += this.FixedDraw_ExposeEvent;
			base.MotionNotifyEvent += this.FixedEx_MotionNotifyEvent;
			base.Destroyed += this.FixedEx_Destroyed;
		}

		private void FixedEx_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			if (this.IsBinding)
			{
				base.QueueDraw();
			}
		}

		public void InitData()
		{
			if (this.rootSkeleton != null)
			{
				this.DisposeRootNode(null);
				this.DisposeContentNode(null);
			}
			if (Services.Workbench.ActiveDocument == null)
			{
				return;
			}
			CocosItem file = Services.Workbench.ActiveDocument.File;
			this.rootSkeleton = (file.GetRootNode() as BoneObject);
			this.RemoveAll();
			this.NodeList.Clear();
			if (this.rootSkeleton == null)
			{
				this.rootNodeContent = null;
				base.QueueDraw();
				return;
			}
			this.rootNodeContent = new NodeContent(this.rootSkeleton.Name);
			if (this.rootNodeContent.ChildNodes == null)
			{
				this.rootNodeContent.ChildNodes = new List<NodeContent>();
			}
			this.rootNodeContent.ChildNodes.Clear();
			this.rootNodeContent.NodePoint = new Gdk.Point(0, 0);
			this.rootNodeContent.CurrentObject = this.rootSkeleton;
			this.rootNodeContent.Scale = this.currentScale;
			this.NodeList.Add(this.rootNodeContent);
			this.rootSkeleton.Children.CollectionChanged += this.Children_CollectionChanged;
			if (this.rootSkeleton.Children.Count > 0)
			{
				foreach (AbstractNodeObject obj in this.rootSkeleton.Children)
				{
					this.GetNodeContent(this.rootNodeContent, obj, 0);
				}
			}
			this.InitControl();
			this.SelectList.ForEach(delegate(NodeContent w)
			{
				w.SetChoiceBgColor();
			});
			this.RefreshDialog();
			base.QueueDraw();
		}

		private void InitControl()
		{
			this.rootNodeContent.NodePoint = new Gdk.Point(0, 0);
			this.RecursionContent(this.rootNodeContent, (int)((double)(this.rootNodeContent.TreeDegree ?? 0) * this.currentScale) / 2, 10, 0);
			base.ShowAll();
		}

		private void RecursionContent(NodeContent content, int x = 0, int y = 10, int startPosition = 0)
		{
			content.WidthRequest = (int)(80.0 * this.currentScale);
			content.HeightRequest = (int)(20.0 * this.currentScale);
			if (!base.Children.Contains(content))
			{
				base.Add(content);
			}
			Fixed.FixedChild fixedChild = (Fixed.FixedChild)this[content];
			fixedChild.X = x;
			fixedChild.Y = y;
			content.NodePoint = new Gdk.Point(fixedChild.X, fixedChild.Y);
			content.StartPosition = startPosition;
			if (content.ChildNodes == null)
			{
				return;
			}
			int num = startPosition;
			for (int i = 0; i < content.ChildNodes.Count; i++)
			{
				this.RecursionContent(content.ChildNodes[i], num + (int)((double)(content.ChildNodes[i].TreeDegree ?? 0) * this.currentScale) / 2, y + (int)(75.0 * this.CurrentScale), num);
				num += (content.ChildNodes[i].TreeDegree ?? 0);
				num += (int)(20.0 * this.currentScale);
			}
		}

		private void GetNodeContent(NodeContent parent, AbstractNodeObject obj, int tier = 0)
		{
			if (obj == null)
			{
				return;
			}
			BoneObject boneObject = obj as BoneObject;
			NodeContent nodeContent;
			if (boneObject != null)
			{
				nodeContent = new BoneContent(obj.Name);
			}
			else
			{
				nodeContent = new SkinContent(obj.Name);
			}
			nodeContent.ParentNode = parent;
			parent.ChildNodes.Add(nodeContent);
			nodeContent.ZOrder = obj.ZOrder;
			nodeContent.CurrentObject = obj;
			nodeContent.Scale = this.currentScale;
			this.NodeList.Add(nodeContent);
			obj.Children.CollectionChanged += this.Children_CollectionChanged;
			if (obj.IsSelected)
			{
				this.SelectList.Add(nodeContent);
			}
			if (nodeContent.ChildNodes == null)
			{
				nodeContent.ChildNodes = new List<NodeContent>();
			}
			if (boneObject == null)
			{
				return;
			}
			foreach (NodeObject nodeObject in boneObject.Skins)
			{
				SkinContent skinContent = new SkinContent(nodeObject.Name);
				skinContent.ChildNodes = new List<NodeContent>();
				skinContent.ParentNode = nodeContent;
				nodeContent.ChildNodes.Add(skinContent);
				skinContent.ZOrder = nodeObject.ZOrder;
				skinContent.CurrentObject = nodeObject;
				skinContent.Scale = this.currentScale;
				this.NodeList.Add(skinContent);
				nodeObject.Children.CollectionChanged += this.Children_CollectionChanged;
				if (nodeObject.IsSelected)
				{
					this.SelectList.Add(skinContent);
				}
			}
			foreach (BoneObject obj2 in boneObject.Bones)
			{
				this.GetNodeContent(nodeContent, obj2, tier + 1);
			}
		}

		public NodeContent GetEndContent(NodeContent content, double x, double y)
		{
			NodeContent nodeContent = null;
			if (x < (double)content.NodePoint.X + 80.0 * this.CurrentScale && x > (double)content.NodePoint.X && y < (double)content.NodePoint.Y + 20.0 * this.CurrentScale && y > (double)content.NodePoint.Y)
			{
				return content;
			}
			foreach (NodeContent content2 in content.ChildNodes)
			{
				nodeContent = this.GetEndContent(content2, x, y);
				if (nodeContent != null)
				{
					break;
				}
			}
			return nodeContent;
		}

		public void Unbinding()
		{
			UnBindingBoneTool.UnBindingSelectedNodes();
			if (SelectService.Instance.SelectedObjectList.Count > 0)
			{
				this.InitData();
				base.QueueDraw();
			}
		}

		private const int contentWidth = 80;

		private const int contentHeight = 20;

		private const int contentXOffset = 20;

		private const int countentYOffset = 75;

		private const int startY = 10;

		private static readonly int xOffer = 40;

		private static readonly int yOffer = 15;

		private static readonly Gdk.Color bgColor = new Gdk.Color(33, 33, 35);

		private readonly int foldLineNum = 4;

		public int startDrawX;

		public int startDrawY;

		public int endDrawX;

		public int endDrawY;

		private BoneOrderList _orderListView;

		private NodeContent rootNodeContent;

		private ChoiceType currentType;

		private double currentScale = 1.0;

		public List<NodeContent> SelectList = new List<NodeContent>();

		private List<NodeContent> NodeList = new List<NodeContent>();

		private BoneObject rootSkeleton;
	}
}
