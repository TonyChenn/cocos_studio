using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Interface;
using CocoStudio.Model.ViewModel.HitTest;
using CocoStudio.UndoManager;
using Gtk;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000078 RID: 120
	public abstract class VisualObject : BaseObject, IComparable, ICloneable, ITimeline, IOperationMask
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x0001B694 File Offset: 0x00019894
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x0001B6AC File Offset: 0x000198AC
		protected PointF lastClickPoint
		{
			get
			{
				return this.lastclickpoint;
			}
			set
			{
				this.lastclickpoint = value;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x0001B6B8 File Offset: 0x000198B8
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x0001B6CF File Offset: 0x000198CF
		public bool IsHitTestVisible { get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x0001B6D8 File Offset: 0x000198D8
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x0001B6F0 File Offset: 0x000198F0
		public virtual OperationMask OperationFlag
		{
			get
			{
				return this._operationMask;
			}
			set
			{
				this._operationMask = value;
				this.RaisePropertyChanged<OperationMask>(() => this.OperationFlag);
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0001B740 File Offset: 0x00019940
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x0001B758 File Offset: 0x00019958
		[UndoProperty]
		public virtual bool CanEdit
		{
			get
			{
				return this._canEdit;
			}
			set
			{
				this._canEdit = value;
				this.RaisePropertyChanged<bool>(() => this.CanEdit);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0001B7A8 File Offset: 0x000199A8
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x0001B7BF File Offset: 0x000199BF
		public virtual bool IsExpanded { get; set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0001B7C8 File Offset: 0x000199C8
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x0001B7E0 File Offset: 0x000199E0
		public virtual bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				this._isSelected = value;
				this.RaisePropertyChanged<bool>(() => this.IsSelected);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x0001B830 File Offset: 0x00019A30
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x0001B847 File Offset: 0x00019A47
		public virtual int ActionTag { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x0001B850 File Offset: 0x00019A50
		public virtual int OrderOfArrival
		{
			get
			{
				return this.GetCSVisual().GetOrderOfArrival();
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x0001B870 File Offset: 0x00019A70
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x0001B88D File Offset: 0x00019A8D
		public virtual PointF Position
		{
			get
			{
				return this.GetCSVisual().GetPosition();
			}
			set
			{
				this.GetCSVisual().SetPosition(value);
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0001B8A0 File Offset: 0x00019AA0
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x0001B8C0 File Offset: 0x00019AC0
		public virtual ScaleValue AnchorPoint
		{
			get
			{
				return this.GetCSVisual().GetAnchorPoint();
			}
			set
			{
				if (value != null)
				{
					this.GetCSVisual().SetAnchorPoint(value);
				}
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x0001B8E8 File Offset: 0x00019AE8
		// (set) Token: 0x06000417 RID: 1047 RVA: 0x0001B905 File Offset: 0x00019B05
		public virtual ScaleValue Scale
		{
			get
			{
				return this.GetCSVisual().GetScale();
			}
			set
			{
				this.GetCSVisual().SetScale(value);
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x0001B918 File Offset: 0x00019B18
		// (set) Token: 0x06000419 RID: 1049 RVA: 0x0001B92F File Offset: 0x00019B2F
		public virtual bool UniformScale { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x0001B938 File Offset: 0x00019B38
		// (set) Token: 0x0600041B RID: 1051 RVA: 0x0001B955 File Offset: 0x00019B55
		public virtual float Rotation
		{
			get
			{
				return this.GetCSVisual().GetRotation();
			}
			set
			{
				this.GetCSVisual().SetRotation(value);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0001B968 File Offset: 0x00019B68
		// (set) Token: 0x0600041D RID: 1053 RVA: 0x0001B9B0 File Offset: 0x00019BB0
		public virtual ScaleValue RotationSkew
		{
			get
			{
				return new ScaleValue(this.GetCSVisual().GetRotationSkewX(), this.GetCSVisual().GetRotationSkewY(), 0.1, -99999999.0, 99999999.0);
			}
			set
			{
				this.GetCSVisual().SetRotationSkewX(value.ScaleX);
				this.GetCSVisual().SetRotationSkewY(value.ScaleY);
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0001B9D8 File Offset: 0x00019BD8
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x0001B9F8 File Offset: 0x00019BF8
		public virtual int Alpha
		{
			get
			{
				return this.GetCSVisual().GetAlpha();
			}
			set
			{
				if (this.GetCSVisual().GetAlpha() != value)
				{
					this.GetCSVisual().SetAlpha(value);
					this.RaisePropertyChanged<int>(() => this.Alpha);
				}
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0001BA68 File Offset: 0x00019C68
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x0001BA88 File Offset: 0x00019C88
		public virtual int ZOrder
		{
			get
			{
				return this.GetCSVisual().GetZOrder();
			}
			set
			{
				this.GetCSVisual().SetZOrder(value);
				this.RaisePropertyChanged<int>(() => this.ZOrder);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0001BAE0 File Offset: 0x00019CE0
		// (set) Token: 0x06000423 RID: 1059 RVA: 0x0001BB00 File Offset: 0x00019D00
		[UndoProperty]
		public virtual SizeF Size
		{
			get
			{
				return this.GetCSVisual().GetSize();
			}
			set
			{
				this.GetCSVisual().SetSize(value);
				this.RaisePropertyChanged<SizeF>(() => this.Size);
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x0001BB58 File Offset: 0x00019D58
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x0001BB70 File Offset: 0x00019D70
		[UndoProperty]
		public virtual bool Visible
		{
			get
			{
				return this._treeVisible;
			}
			set
			{
				this._treeVisible = value;
				this.GetCSVisual().SetVisible(this._treeVisible && this._frameVisible);
				this.RaisePropertyChanged<bool>(() => this.Visible);
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0001BBE0 File Offset: 0x00019DE0
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x0001BBF8 File Offset: 0x00019DF8
		public virtual bool VisibleForFrame
		{
			get
			{
				return this._frameVisible;
			}
			set
			{
				this._frameVisible = value;
				this.GetCSVisual().SetVisible(this._treeVisible && this._frameVisible);
				this.RaisePropertyChanged<bool>(() => this.VisibleForFrame);
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001BC68 File Offset: 0x00019E68
		public VisualObject()
		{
			this.CanEdit = true;
			this.IsHitTestVisible = true;
			this.InitOperation();
			this.ActionTag = ActionTagManager.CreateObjectActionTag();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001BCDA File Offset: 0x00019EDA
		public virtual void InitOperation()
		{
			this.OperationFlag = OperationMask.AllFlag;
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0001BCEC File Offset: 0x00019EEC
		public FrameCollection Frames
		{
			get
			{
				if (this.handlerframes == null)
				{
					this.handlerframes = new FrameCollection(this);
				}
				return this.handlerframes;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x0001BD20 File Offset: 0x00019F20
		public ObservableCollection<Timeline> Timelines
		{
			get
			{
				return this.timelines;
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001BD38 File Offset: 0x00019F38
		internal virtual CSVisualObject GetCSVisual()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001BD40 File Offset: 0x00019F40
		public virtual object Clone()
		{
			return null;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001BD53 File Offset: 0x00019F53
		public void MouseDown(MouseEventArgs args)
		{
			this.lastClickPoint = args.Point;
			this.OnMouseDown(args);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001BD6C File Offset: 0x00019F6C
		public void MouseMove(MouseEventArgs args)
		{
			if (this.OperationFlag.HasFlag(OperationMask.MoveFlag) && !(this.lastClickPoint == null))
			{
				this.OnMouseMove(args);
				this.lastClickPoint = args.Point;
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001BDC0 File Offset: 0x00019FC0
		public void MouseUp(MouseEventArgs args)
		{
			if (!(this.lastClickPoint == null))
			{
				this.OnMouseUp(args);
				this.lastClickPoint = null;
			}
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0001BDF3 File Offset: 0x00019FF3
		public void MouseDoubleClick(MouseEventArgs args)
		{
			this.OnMouseDoubleClick(args);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0001BDFE File Offset: 0x00019FFE
		public void KeyDown(KeyPressEventArgs e)
		{
			this.OnKeyDown(e);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0001BE09 File Offset: 0x0001A009
		public void KeyUp(KeyReleaseEventArgs e)
		{
			this.OnKeyUp(e);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001BE14 File Offset: 0x0001A014
		public bool DragOver(DragMotionArgs e)
		{
			return this.OnDragOver(e);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0001BE2D File Offset: 0x0001A02D
		public void DragLeave(DragMotionArgs e)
		{
			this.OnDragLeave(e);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0001BE38 File Offset: 0x0001A038
		public void DragEnter(DragMotionArgs e)
		{
			this.OnDragEnter(e);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0001BE43 File Offset: 0x0001A043
		public void DragDrop(DragDropArgs e)
		{
			this.OnDragDrop(e);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0001BE4E File Offset: 0x0001A04E
		protected virtual void OnMouseDown(MouseEventArgs args)
		{
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0001BE51 File Offset: 0x0001A051
		protected virtual void OnMouseMove(MouseEventArgs args)
		{
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0001BE54 File Offset: 0x0001A054
		protected virtual void OnMouseUp(MouseEventArgs args)
		{
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0001BE57 File Offset: 0x0001A057
		protected virtual void OnMouseDoubleClick(MouseEventArgs args)
		{
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0001BE5A File Offset: 0x0001A05A
		protected virtual void OnKeyDown(KeyPressEventArgs e)
		{
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001BE5D File Offset: 0x0001A05D
		protected virtual void OnKeyUp(KeyReleaseEventArgs e)
		{
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0001BE60 File Offset: 0x0001A060
		protected virtual bool OnDragOver(DragMotionArgs e)
		{
			return DragOperationManager.Current.DragOver(e, this);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0001BE7E File Offset: 0x0001A07E
		protected virtual void OnDragLeave(DragMotionArgs e)
		{
			DragOperationManager.Current.DragLeave(e, this);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001BE8E File Offset: 0x0001A08E
		protected virtual void OnDragEnter(DragMotionArgs e)
		{
			DragOperationManager.Current.DragEnter(e, this);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0001BE9E File Offset: 0x0001A09E
		protected virtual void OnDragDrop(DragDropArgs e)
		{
			DragOperationManager.Current.DragDrop(e, this);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0001BEB0 File Offset: 0x0001A0B0
		public virtual HitTestResult HitTest(PointF point)
		{
			HitTestResult result;
			if (!this.IsHitTestVisible || !this.Visible || !this.VisibleForFrame || !this.CanEdit)
			{
				result = new HitTestResult(point, this.Visible);
			}
			else
			{
				result = this.HitTestCore(point);
			}
			return result;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0001BF00 File Offset: 0x0001A100
		protected virtual HitTestResult HitTestCore(PointF point)
		{
			int num = this.GetCSVisual().HitTest(point);
			HitTestResult result;
			if (num != -1)
			{
				result = new HitTestResult(this, point, MouseOperationType.OPERATION_POSITION, ControlPointType.POINT_NONE);
			}
			else
			{
				result = new HitTestResult(point, this.CanContinueTest());
			}
			return result;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0001BF40 File Offset: 0x0001A140
		public virtual RectTestResult RectTest(RectF rect)
		{
			RectTestResult result;
			if (!this.IsHitTestVisible || !this.Visible || !this.VisibleForFrame || !this.CanEdit)
			{
				result = new RectTestResult(rect, this.Visible);
			}
			else
			{
				result = this.RectTestCore(rect);
			}
			return result;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0001BF90 File Offset: 0x0001A190
		protected virtual RectTestResult RectTestCore(RectF rect)
		{
			RectTestResult result;
			if (this.GetCSVisual().RectTest(rect))
			{
				result = new RectTestResult(this, rect, this.CanContinueTest());
			}
			else
			{
				result = new RectTestResult(rect, this.CanContinueTest());
			}
			return result;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001BFD4 File Offset: 0x0001A1D4
		protected virtual bool CanContinueTest()
		{
			return this.Visible && this.VisibleForFrame;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0001BFF8 File Offset: 0x0001A1F8
		public virtual IEnumerable<VisualObject> GetVisualChildren()
		{
			return null;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0001C00C File Offset: 0x0001A20C
		public virtual int CompareTo(object other)
		{
			VisualObject visualObject = other as VisualObject;
			int result;
			if (this.ZOrder > visualObject.ZOrder)
			{
				result = -1;
			}
			else if (this.ZOrder < visualObject.ZOrder)
			{
				result = 1;
			}
			else if (this.OrderOfArrival > visualObject.OrderOfArrival)
			{
				result = -1;
			}
			else if (this.OrderOfArrival < visualObject.OrderOfArrival)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0001C08C File Offset: 0x0001A28C
		public virtual PointF TransformToSelf(PointF sencePoint)
		{
			return this.GetCSVisual().TransformToSelf(sencePoint);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0001C0AC File Offset: 0x0001A2AC
		public virtual PointF TransformToScene(PointF selfPoint)
		{
			return this.GetCSVisual().TransformToScene(selfPoint);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0001C0CC File Offset: 0x0001A2CC
		public virtual PointF TransformToParent(PointF sencePoint)
		{
			return this.GetCSVisual().TransformToParent(sencePoint);
		}

		// Token: 0x0400020E RID: 526
		private PointF lastclickpoint;

		// Token: 0x0400020F RID: 527
		internal static int tag = 0;

		// Token: 0x04000210 RID: 528
		private OperationMask _operationMask = OperationMask.AllFlag;

		// Token: 0x04000211 RID: 529
		private bool _canEdit = true;

		// Token: 0x04000212 RID: 530
		private bool _isSelected = false;

		// Token: 0x04000213 RID: 531
		private bool _treeVisible = true;

		// Token: 0x04000214 RID: 532
		private bool _frameVisible = true;

		// Token: 0x04000215 RID: 533
		private FrameCollection handlerframes = null;

		// Token: 0x04000216 RID: 534
		private ObservableCollection<Timeline> timelines = new ObservableCollection<Timeline>();
	}
}
