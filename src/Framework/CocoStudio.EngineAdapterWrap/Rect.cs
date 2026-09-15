using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	internal class Rect : IDisposable
	{
		public Rect(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(Rect obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~Rect()
		{
			this.Dispose();
		}

		public virtual void Dispose()
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
								CocoStudioEngineAdapterPINVOKE.delete_Rect(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Rect(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public Vec2 origin
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Rect_origin_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Rect_origin_set(this.swigCPtr, Vec2.getCPtr(value));
			}
		}

		public Size size
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Rect_size_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Size(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Rect_size_set(this.swigCPtr, Size.getCPtr(value));
			}
		}

		public Rect() : this(CocoStudioEngineAdapterPINVOKE.new_Rect__SWIG_0(), true)
		{
		}

		public Rect(float x, float y, float width, float height) : this(CocoStudioEngineAdapterPINVOKE.new_Rect__SWIG_1(x, y, width, height), true)
		{
		}

		public Rect(Vec2 pos, Size dimension) : this(CocoStudioEngineAdapterPINVOKE.new_Rect__SWIG_2(Vec2.getCPtr(pos), Size.getCPtr(dimension)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public Rect(Rect other) : this(CocoStudioEngineAdapterPINVOKE.new_Rect__SWIG_3(Rect.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void setRect(float x, float y, float width, float height)
		{
			CocoStudioEngineAdapterPINVOKE.Rect_setRect(this.swigCPtr, x, y, width, height);
		}

		public float getMinX()
		{
			return CocoStudioEngineAdapterPINVOKE.Rect_getMinX(this.swigCPtr);
		}

		public float getMidX()
		{
			return CocoStudioEngineAdapterPINVOKE.Rect_getMidX(this.swigCPtr);
		}

		public float getMaxX()
		{
			return CocoStudioEngineAdapterPINVOKE.Rect_getMaxX(this.swigCPtr);
		}

		public float getMinY()
		{
			return CocoStudioEngineAdapterPINVOKE.Rect_getMinY(this.swigCPtr);
		}

		public float getMidY()
		{
			return CocoStudioEngineAdapterPINVOKE.Rect_getMidY(this.swigCPtr);
		}

		public float getMaxY()
		{
			return CocoStudioEngineAdapterPINVOKE.Rect_getMaxY(this.swigCPtr);
		}

		public bool equals(Rect rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Rect_equals(this.swigCPtr, Rect.getCPtr(rect));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool containsPoint(Vec2 point)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Rect_containsPoint(this.swigCPtr, Vec2.getCPtr(point));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool intersectsRect(Rect rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Rect_intersectsRect(this.swigCPtr, Rect.getCPtr(rect));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool intersectsCircle(Vec2 center, float radius)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Rect_intersectsCircle(this.swigCPtr, Vec2.getCPtr(center), radius);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public Rect unionWithRect(Rect rect)
		{
			Rect result = new Rect(CocoStudioEngineAdapterPINVOKE.Rect_unionWithRect(this.swigCPtr, Rect.getCPtr(rect)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void merge(Rect rect)
		{
			CocoStudioEngineAdapterPINVOKE.Rect_merge(this.swigCPtr, Rect.getCPtr(rect));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static Rect ZERO
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Rect_ZERO_get();
				return (intPtr == IntPtr.Zero) ? null : new Rect(intPtr, false);
			}
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}
