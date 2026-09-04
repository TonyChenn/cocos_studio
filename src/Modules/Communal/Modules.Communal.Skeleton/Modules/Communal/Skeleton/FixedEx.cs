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
	// Token: 0x02000020 RID: 32
	public class FixedEx : Fixed
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00007247 File Offset: 0x00005447
		// (set) Token: 0x06000149 RID: 329 RVA: 0x0000724F File Offset: 0x0000544F
		public bool IsBinding { get; set; }

		// Token: 0x0600014A RID: 330 RVA: 0x00007258 File Offset: 0x00005458
		private void FixedDraw_ExposeEvent(object o, ExposeEventArgs args)
		{
			this.DrawLine();
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007260 File Offset: 0x00005460
		public void DrawLine()
		{
			this.DrawContent(this.rootNodeContent);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00007270 File Offset: 0x00005470
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

		// Token: 0x0600014D RID: 333 RVA: 0x00007424 File Offset: 0x00005624
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

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600014E RID: 334 RVA: 0x0000747E File Offset: 0x0000567E
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00007486 File Offset: 0x00005686
		public SkeletonGraphDialog SkeletonDialog { private get; set; }

		// Token: 0x06000150 RID: 336 RVA: 0x0000748F File Offset: 0x0000568F
		private void FixedEx_Destroyed(object sender, EventArgs e)
		{
			this.DisposeRootNode(null);
			this.DisposeContentNode(null);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000074A0 File Offset: 0x000056A0
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

		// Token: 0x06000152 RID: 338 RVA: 0x00007578 File Offset: 0x00005778
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

		// Token: 0x06000153 RID: 339 RVA: 0x000075FC File Offset: 0x000057FC
		internal void SetOrderListView(BoneOrderList listview)
		{
			this._orderListView = listview;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000764C File Offset: 0x0000584C
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

		// Token: 0x06000155 RID: 341 RVA: 0x0000790C File Offset: 0x00005B0C
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

		// Token: 0x06000156 RID: 342 RVA: 0x0000796C File Offset: 0x00005B6C
		private void RemoveContent(NodeContent content)
		{
			foreach (NodeContent content2 in content.ChildNodes)
			{
				this.RemoveContent(content2);
			}
			this.NodeList.Remove(content);
			base.Remove(content);
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000157 RID: 343 RVA: 0x000079D4 File Offset: 0x00005BD4
		public NodeContent RootNodeContent
		{
			get
			{
				return this.rootNodeContent;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000158 RID: 344 RVA: 0x000079DC File Offset: 0x00005BDC
		// (set) Token: 0x06000159 RID: 345 RVA: 0x000079E4 File Offset: 0x00005BE4
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600015A RID: 346 RVA: 0x000079ED File Offset: 0x00005BED
		// (set) Token: 0x0600015B RID: 347 RVA: 0x000079F5 File Offset: 0x00005BF5
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

		// Token: 0x0600015C RID: 348 RVA: 0x00007A10 File Offset: 0x00005C10
		public FixedEx()
		{
			base.ModifyBg(StateType.Normal, new Gdk.Color(63, 63, 63));
			base.ExposeEvent += this.FixedDraw_ExposeEvent;
			base.MotionNotifyEvent += this.FixedEx_MotionNotifyEvent;
			base.Destroyed += this.FixedEx_Destroyed;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007A97 File Offset: 0x00005C97
		private void FixedEx_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			if (this.IsBinding)
			{
				base.QueueDraw();
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007AB0 File Offset: 0x00005CB0
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

		// Token: 0x0600015F RID: 351 RVA: 0x00007C68 File Offset: 0x00005E68
		private void InitControl()
		{
			this.rootNodeContent.NodePoint = new Gdk.Point(0, 0);
			this.RecursionContent(this.rootNodeContent, (int)((double)(this.rootNodeContent.TreeDegree ?? 0) * this.currentScale) / 2, 10, 0);
			base.ShowAll();
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00007CC8 File Offset: 0x00005EC8
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

		// Token: 0x06000161 RID: 353 RVA: 0x00007E10 File Offset: 0x00006010
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

		// Token: 0x06000162 RID: 354 RVA: 0x00007FD0 File Offset: 0x000061D0
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

		// Token: 0x06000163 RID: 355 RVA: 0x0000808C File Offset: 0x0000628C
		public void Unbinding()
		{
			UnBindingBoneTool.UnBindingSelectedNodes();
			if (SelectService.Instance.SelectedObjectList.Count > 0)
			{
				this.InitData();
				base.QueueDraw();
			}
		}

		// Token: 0x04000061 RID: 97
		private const int contentWidth = 80;

		// Token: 0x04000062 RID: 98
		private const int contentHeight = 20;

		// Token: 0x04000063 RID: 99
		private const int contentXOffset = 20;

		// Token: 0x04000064 RID: 100
		private const int countentYOffset = 75;

		// Token: 0x04000065 RID: 101
		private const int startY = 10;

		// Token: 0x04000066 RID: 102
		private static readonly int xOffer = 40;

		// Token: 0x04000067 RID: 103
		private static readonly int yOffer = 15;

		// Token: 0x04000068 RID: 104
		private static readonly Gdk.Color bgColor = new Gdk.Color(33, 33, 35);

		// Token: 0x04000069 RID: 105
		private readonly int foldLineNum = 4;

		// Token: 0x0400006A RID: 106
		public int startDrawX;

		// Token: 0x0400006B RID: 107
		public int startDrawY;

		// Token: 0x0400006C RID: 108
		public int endDrawX;

		// Token: 0x0400006D RID: 109
		public int endDrawY;

		// Token: 0x0400006E RID: 110
		private BoneOrderList _orderListView;

		// Token: 0x0400006F RID: 111
		private NodeContent rootNodeContent;

		// Token: 0x04000070 RID: 112
		private ChoiceType currentType;

		// Token: 0x04000071 RID: 113
		private double currentScale = 1.0;

		// Token: 0x04000072 RID: 114
		public List<NodeContent> SelectList = new List<NodeContent>();

		// Token: 0x04000073 RID: 115
		private List<NodeContent> NodeList = new List<NodeContent>();

		// Token: 0x04000074 RID: 116
		private BoneObject rootSkeleton;
	}
}
