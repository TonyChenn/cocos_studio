using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSScrollView : CSPanel
	{
		public CSScrollView(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSScrollView_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSScrollView obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSScrollView()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSScrollView(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSScrollView(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSScrollView() : this(CocoStudioEngineAdapterPINVOKE.new_CSScrollView(), true)
		{
		}

		public virtual SizeF GetInnerSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSScrollView_GetInnerSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new SizeF(size.width, size.height);
		}

		public virtual void SetInnerSize(SizeF size)
		{
			CocoStudioEngineAdapterPINVOKE.CSScrollView_SetInnerSize(this.swigCPtr, Size.getCPtr(new Size(size.Width, size.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual int GetDirectionType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSScrollView_GetDirectionType(this.swigCPtr);
		}

		public virtual void SetDirectionType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSScrollView_SetDirectionType(this.swigCPtr, iType);
		}

		public virtual bool GetBounceEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSScrollView_GetBounceEnabled(this.swigCPtr);
		}

		public virtual void SetBounceEnabled(bool enabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSScrollView_SetBounceEnabled(this.swigCPtr, enabled);
		}

		public virtual PointF TransformToSelfInner(PointF scenePoint)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSScrollView_TransformToSelfInner(this.swigCPtr, Vec2.getCPtr(new Vec2(scenePoint.X, scenePoint.Y)));
			Vec2 vec = new Vec2(cPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return new PointF(vec.x, vec.y);
		}

		public override RectF GetBoundingRect()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSScrollView_GetBoundingRect(this.swigCPtr);
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

		private HandleRef swigCPtr;
	}
}
