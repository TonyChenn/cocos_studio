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
	public abstract class VisualObject : BaseObject, IComparable, ICloneable, ITimeline, IOperationMask
	{
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

		public bool IsHitTestVisible { get; set; }

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

		public virtual bool IsExpanded { get; set; }

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

		public virtual int ActionTag { get; set; }

		public virtual int OrderOfArrival
		{
			get
			{
				return this.GetCSVisual().GetOrderOfArrival();
			}
		}

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

		public virtual bool UniformScale { get; set; }

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

		public VisualObject()
		{
			this.CanEdit = true;
			this.IsHitTestVisible = true;
			this.InitOperation();
			this.ActionTag = ActionTagManager.CreateObjectActionTag();
		}

		public virtual void InitOperation()
		{
			this.OperationFlag = OperationMask.AllFlag;
		}

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

		public ObservableCollection<Timeline> Timelines
		{
			get
			{
				return this.timelines;
			}
		}

		internal virtual CSVisualObject GetCSVisual()
		{
			throw new NotImplementedException();
		}

		public virtual object Clone()
		{
			return null;
		}

		public void MouseDown(MouseEventArgs args)
		{
			this.lastClickPoint = args.Point;
			this.OnMouseDown(args);
		}

		public void MouseMove(MouseEventArgs args)
		{
			if (this.OperationFlag.HasFlag(OperationMask.MoveFlag) && !(this.lastClickPoint == null))
			{
				this.OnMouseMove(args);
				this.lastClickPoint = args.Point;
			}
		}

		public void MouseUp(MouseEventArgs args)
		{
			if (!(this.lastClickPoint == null))
			{
				this.OnMouseUp(args);
				this.lastClickPoint = null;
			}
		}

		public void MouseDoubleClick(MouseEventArgs args)
		{
			this.OnMouseDoubleClick(args);
		}

		public void KeyDown(KeyPressEventArgs e)
		{
			this.OnKeyDown(e);
		}

		public void KeyUp(KeyReleaseEventArgs e)
		{
			this.OnKeyUp(e);
		}

		public bool DragOver(DragMotionArgs e)
		{
			return this.OnDragOver(e);
		}

		public void DragLeave(DragMotionArgs e)
		{
			this.OnDragLeave(e);
		}

		public void DragEnter(DragMotionArgs e)
		{
			this.OnDragEnter(e);
		}

		public void DragDrop(DragDropArgs e)
		{
			this.OnDragDrop(e);
		}

		protected virtual void OnMouseDown(MouseEventArgs args)
		{
		}

		protected virtual void OnMouseMove(MouseEventArgs args)
		{
		}

		protected virtual void OnMouseUp(MouseEventArgs args)
		{
		}

		protected virtual void OnMouseDoubleClick(MouseEventArgs args)
		{
		}

		protected virtual void OnKeyDown(KeyPressEventArgs e)
		{
		}

		protected virtual void OnKeyUp(KeyReleaseEventArgs e)
		{
		}

		protected virtual bool OnDragOver(DragMotionArgs e)
		{
			return DragOperationManager.Current.DragOver(e, this);
		}

		protected virtual void OnDragLeave(DragMotionArgs e)
		{
			DragOperationManager.Current.DragLeave(e, this);
		}

		protected virtual void OnDragEnter(DragMotionArgs e)
		{
			DragOperationManager.Current.DragEnter(e, this);
		}

		protected virtual void OnDragDrop(DragDropArgs e)
		{
			DragOperationManager.Current.DragDrop(e, this);
		}

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

		protected virtual bool CanContinueTest()
		{
			return this.Visible && this.VisibleForFrame;
		}

		public virtual IEnumerable<VisualObject> GetVisualChildren()
		{
			return null;
		}

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

		public virtual PointF TransformToSelf(PointF sencePoint)
		{
			return this.GetCSVisual().TransformToSelf(sencePoint);
		}

		public virtual PointF TransformToScene(PointF selfPoint)
		{
			return this.GetCSVisual().TransformToScene(selfPoint);
		}

		public virtual PointF TransformToParent(PointF sencePoint)
		{
			return this.GetCSVisual().TransformToParent(sencePoint);
		}

		private PointF lastclickpoint;

		internal static int tag = 0;

		private OperationMask _operationMask = OperationMask.AllFlag;

		private bool _canEdit = true;

		private bool _isSelected = false;

		private bool _treeVisible = true;

		private bool _frameVisible = true;

		private FrameCollection handlerframes = null;

		private ObservableCollection<Timeline> timelines = new ObservableCollection<Timeline>();
	}
}
