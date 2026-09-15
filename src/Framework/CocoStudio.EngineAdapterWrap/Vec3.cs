using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class Vec3 : IDisposable
	{
		public Vec3(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(Vec3 obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~Vec3()
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
								CocoStudioEngineAdapterPINVOKE.delete_Vec3(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Vec3(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public float x
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Vec3_x_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Vec3_x_set(this.swigCPtr, value);
			}
		}

		public float y
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Vec3_y_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Vec3_y_set(this.swigCPtr, value);
			}
		}

		public float z
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Vec3_z_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Vec3_z_set(this.swigCPtr, value);
			}
		}

		public Vec3() : this(CocoStudioEngineAdapterPINVOKE.new_Vec3__SWIG_0(), true)
		{
		}

		public Vec3(float xx, float yy, float zz) : this(CocoStudioEngineAdapterPINVOKE.new_Vec3__SWIG_1(xx, yy, zz), true)
		{
		}

		public Vec3(Vec3 p1, Vec3 p2) : this(CocoStudioEngineAdapterPINVOKE.new_Vec3__SWIG_2(Vec3.getCPtr(p1), Vec3.getCPtr(p2)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public Vec3(Vec3 copy) : this(CocoStudioEngineAdapterPINVOKE.new_Vec3__SWIG_3(Vec3.getCPtr(copy)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static Vec3 fromColor(uint color)
		{
			return new Vec3(CocoStudioEngineAdapterPINVOKE.Vec3_fromColor(color), true);
		}

		public bool isZero()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec3_isZero(this.swigCPtr);
		}

		public bool isOne()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec3_isOne(this.swigCPtr);
		}

		public static float angle(Vec3 v1, Vec3 v2)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec3_angle(Vec3.getCPtr(v1), Vec3.getCPtr(v2));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void add(Vec3 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_add__SWIG_0(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void add(float xx, float yy, float zz)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_add__SWIG_1(this.swigCPtr, xx, yy, zz);
		}

		public static void add(Vec3 v1, Vec3 v2, Vec3 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_add__SWIG_2(Vec3.getCPtr(v1), Vec3.getCPtr(v2), Vec3.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void clamp(Vec3 min, Vec3 max)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_clamp__SWIG_0(this.swigCPtr, Vec3.getCPtr(min), Vec3.getCPtr(max));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void clamp(Vec3 v, Vec3 min, Vec3 max, Vec3 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_clamp__SWIG_1(Vec3.getCPtr(v), Vec3.getCPtr(min), Vec3.getCPtr(max), Vec3.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void cross(Vec3 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_cross__SWIG_0(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void cross(Vec3 v1, Vec3 v2, Vec3 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_cross__SWIG_1(Vec3.getCPtr(v1), Vec3.getCPtr(v2), Vec3.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public float distance(Vec3 v)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec3_distance(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public float distanceSquared(Vec3 v)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec3_distanceSquared(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public float dot(Vec3 v)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec3_dot__SWIG_0(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public float length()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec3_length(this.swigCPtr);
		}

		public float lengthSquared()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec3_lengthSquared(this.swigCPtr);
		}

		public void negate()
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_negate(this.swigCPtr);
		}

		public void normalize()
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_normalize(this.swigCPtr);
		}

		public Vec3 getNormalized()
		{
			return new Vec3(CocoStudioEngineAdapterPINVOKE.Vec3_getNormalized(this.swigCPtr), true);
		}

		public void scale(float scalar)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_scale(this.swigCPtr, scalar);
		}

		public void set(float xx, float yy, float zz)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_set__SWIG_0(this.swigCPtr, xx, yy, zz);
		}

		public void set(Vec3 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_set__SWIG_1(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void set(Vec3 p1, Vec3 p2)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_set__SWIG_2(this.swigCPtr, Vec3.getCPtr(p1), Vec3.getCPtr(p2));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void setZero()
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_setZero(this.swigCPtr);
		}

		public void subtract(Vec3 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_subtract__SWIG_0(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void subtract(Vec3 v1, Vec3 v2, Vec3 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_subtract__SWIG_1(Vec3.getCPtr(v1), Vec3.getCPtr(v2), Vec3.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void smooth(Vec3 target, float elapsedTime, float responseTime)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_smooth(this.swigCPtr, Vec3.getCPtr(target), elapsedTime, responseTime);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public Vec3 lerp(Vec3 target, float alpha)
		{
			Vec3 result = new Vec3(CocoStudioEngineAdapterPINVOKE.Vec3_lerp(this.swigCPtr, Vec3.getCPtr(target), alpha), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static Vec3 ZERO
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec3_ZERO_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
		}

		public static Vec3 ONE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec3_ONE_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
		}

		public static Vec3 UNIT_X
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec3_UNIT_X_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
		}

		public static Vec3 UNIT_Y
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec3_UNIT_Y_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
		}

		public static Vec3 UNIT_Z
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec3_UNIT_Z_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}
