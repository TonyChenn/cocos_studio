using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSNode2D : CSNode
	{
		public CSNode2D(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSNode2D_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSNode2D obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSNode2D()
		{
			this.Dispose();
		}

		public override void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						HandleRef handle = new HandleRef(null, this.swigCPtr.Handle);
						if (this.IsContainOpenGLResource())
						{
							GtkInvokeHelp.BeginInvoke(delegate
							{
								this.swigCPtr = handle;
								CocoStudioEngineAdapterPINVOKE.delete_CSNode2D(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSNode2D(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSNode2D() : this(CocoStudioEngineAdapterPINVOKE.new_CSNode2D(), true)
		{
		}

		public virtual void InitIcon(string iconFile)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_InitIcon__SWIG_0(this.swigCPtr, iconFile);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void InitIcon(SizeF iconSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_InitIcon__SWIG_1(this.swigCPtr, Size.getCPtr(new Size(iconSize.Width, iconSize.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void SetIconVisible(bool iconVisible)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetIconVisible(this.swigCPtr, iconVisible);
		}

		public virtual bool GetIconVisible()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetIconVisible(this.swigCPtr);
		}

		public virtual void RefreshLayout()
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_RefreshLayout(this.swigCPtr);
		}

		public virtual void RefreshChildrenLayout()
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_RefreshChildrenLayout(this.swigCPtr);
		}

		public virtual ScaleValue GetBoxAnchorPoint()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSNode2D_GetBoxAnchorPoint(this.swigCPtr);
			CSScale csscale = new CSScale(cPtr, false);
			return new ScaleValue(csscale.GetScaleX(), csscale.GetScaleY(), 0.1, -99999999.0, 99999999.0);
		}

		public virtual SizeF GetBoxSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSNode2D_GetBoxSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new SizeF(size.width, size.height);
		}

		public override RectF GetBoundingRect()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSNode2D_GetBoundingRect(this.swigCPtr);
			Rect rect = new Rect(cPtr, true);
			if (rect.size.width < 0f || rect.size.height < 0f)
			{
				rect.origin.x = 0f;
				rect.origin.y = 0f;
				rect.size.width = 0f;
				rect.size.height = 0f;
			}
			return new RectF(rect.origin.x, rect.origin.y, rect.size.width, rect.size.height);
		}

		public virtual void SetHorizontalEdge(int hEage)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetHorizontalEdge(this.swigCPtr, hEage);
		}

		public virtual int GetHorizontalEdge()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetHorizontalEdge(this.swigCPtr);
		}

		public virtual void SetVerticalEdge(int vEage)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetVerticalEdge(this.swigCPtr, vEage);
		}

		public virtual int GetVerticalEdge()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetVerticalEdge(this.swigCPtr);
		}

		public virtual void SetPositionPercentXEnabled(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPositionPercentXEnabled(this.swigCPtr, isEnabled);
		}

		public virtual bool IsUsingPositionPercentX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_IsUsingPositionPercentX(this.swigCPtr);
		}

		public virtual void SetPositionPercentYEnabled(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPositionPercentYEnabled(this.swigCPtr, isEnabled);
		}

		public virtual bool IsUsingPositionPercentY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_IsUsingPositionPercentY(this.swigCPtr);
		}

		public virtual void SetPositionPercentX(float percent)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPositionPercentX(this.swigCPtr, percent);
		}

		public virtual float GetPositionPercentX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetPositionPercentX(this.swigCPtr);
		}

		public virtual void SetPositionPercentY(float percent)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPositionPercentY(this.swigCPtr, percent);
		}

		public virtual float GetPositionPercentY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetPositionPercentY(this.swigCPtr);
		}

		public virtual void SetLeftMargin(float margin)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetLeftMargin(this.swigCPtr, margin);
		}

		public virtual float GetLeftMargin()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetLeftMargin(this.swigCPtr);
		}

		public virtual void SetRightMargin(float margin)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetRightMargin(this.swigCPtr, margin);
		}

		public virtual float GetRightMargin()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetRightMargin(this.swigCPtr);
		}

		public virtual void SetTopMargin(float margin)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetTopMargin(this.swigCPtr, margin);
		}

		public virtual float GetTopMargin()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetTopMargin(this.swigCPtr);
		}

		public virtual void SetBottomMargin(float margin)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetBottomMargin(this.swigCPtr, margin);
		}

		public virtual float GetBottomMargin()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetBottomMargin(this.swigCPtr);
		}

		public virtual void SetPercentWidthEnable(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPercentWidthEnable(this.swigCPtr, isEnabled);
		}

		public virtual bool GetPercentWidthEnable()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetPercentWidthEnable(this.swigCPtr);
		}

		public virtual void SetPercentHeightEnable(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPercentHeightEnable(this.swigCPtr, isEnabled);
		}

		public virtual bool GetPercentHeightEnable()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetPercentHeightEnable(this.swigCPtr);
		}

		public virtual void SetPercentWidth(float percent)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPercentWidth(this.swigCPtr, percent);
		}

		public virtual float GetPercentWidth()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetPercentWidth(this.swigCPtr);
		}

		public virtual void SetPercentHeight(float percent)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPercentHeight(this.swigCPtr, percent);
		}

		public virtual float GetPercentHeight()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetPercentHeight(this.swigCPtr);
		}

		public virtual void SetSizeWidth(float width)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetSizeWidth(this.swigCPtr, width);
		}

		public virtual float GetSizeWidth()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetSizeWidth(this.swigCPtr);
		}

		public virtual void SetSizeHeight(float height)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetSizeHeight(this.swigCPtr, height);
		}

		public virtual float GetSizeHeight()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetSizeHeight(this.swigCPtr);
		}

		public virtual void SetStretchWidthEnable(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetStretchWidthEnable(this.swigCPtr, isEnabled);
		}

		public virtual bool GetStretchWidthEnable()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetStretchWidthEnable(this.swigCPtr);
		}

		public virtual void SetStretchHeightEnable(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetStretchHeightEnable(this.swigCPtr, isEnabled);
		}

		public virtual bool GetStretchHeightEnable()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetStretchHeightEnable(this.swigCPtr);
		}

		public override void SetPosition(PointF position)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPosition(this.swigCPtr, Vec2.getCPtr(new Vec2(position.X, position.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public override void SetScale(ScaleValue scale)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetScale(this.swigCPtr, CSScale.getCPtr(new CSScale(scale.ScaleX, scale.ScaleY)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public override void SetAnchorPoint(ScaleValue anchorPoint)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetAnchorPoint(this.swigCPtr, CSScale.getCPtr(new CSScale(anchorPoint.ScaleX, anchorPoint.ScaleY)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public override void SetSize(SizeF cSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetSize(this.swigCPtr, Size.getCPtr(new Size(cSize.Width, cSize.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public override int HitTest(PointF point)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSNode2D_HitTest(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public override bool RectTest(RectF rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSNode2D_RectTest(this.swigCPtr, Rect.getCPtr(new Rect(rect.X, rect.Y, rect.Width, rect.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public override CSMatrix GetWorldMatrix()
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSNode2D_GetWorldMatrix(this.swigCPtr), true);
		}

		public override CSMatrix GetAnchorWorldMatrix()
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSNode2D_GetAnchorWorldMatrix(this.swigCPtr), true);
		}

		public override void SetObjectState(CSVisualObject.ObjectState boxState)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetObjectState(this.swigCPtr, (int)boxState);
		}

		private HandleRef swigCPtr;
	}
}
