using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class Quaternion : IDisposable
	{
		public Quaternion(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(Quaternion obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~Quaternion()
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
								CocoStudioEngineAdapterPINVOKE.delete_Quaternion(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Quaternion(this.swigCPtr);
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
				return CocoStudioEngineAdapterPINVOKE.Quaternion_x_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quaternion_x_set(this.swigCPtr, value);
			}
		}

		public float y
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Quaternion_y_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quaternion_y_set(this.swigCPtr, value);
			}
		}

		public float z
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Quaternion_z_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quaternion_z_set(this.swigCPtr, value);
			}
		}

		public float w
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Quaternion_w_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quaternion_w_set(this.swigCPtr, value);
			}
		}

		public Quaternion() : this(CocoStudioEngineAdapterPINVOKE.new_Quaternion__SWIG_0(), true)
		{
		}

		public Quaternion(float xx, float yy, float zz, float ww) : this(CocoStudioEngineAdapterPINVOKE.new_Quaternion__SWIG_1(xx, yy, zz, ww), true)
		{
		}

		public Quaternion(Vec3 axis, float angle) : this(CocoStudioEngineAdapterPINVOKE.new_Quaternion__SWIG_2(Vec3.getCPtr(axis), angle), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public Quaternion(Quaternion copy) : this(CocoStudioEngineAdapterPINVOKE.new_Quaternion__SWIG_3(Quaternion.getCPtr(copy)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static Quaternion identity()
		{
			return new Quaternion(CocoStudioEngineAdapterPINVOKE.Quaternion_identity(), false);
		}

		public static Quaternion zero()
		{
			return new Quaternion(CocoStudioEngineAdapterPINVOKE.Quaternion_zero(), false);
		}

		public bool isIdentity()
		{
			return CocoStudioEngineAdapterPINVOKE.Quaternion_isIdentity(this.swigCPtr);
		}

		public bool isZero()
		{
			return CocoStudioEngineAdapterPINVOKE.Quaternion_isZero(this.swigCPtr);
		}

		public static void createFromAxisAngle(Vec3 axis, float angle, Quaternion dst)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_createFromAxisAngle(Vec3.getCPtr(axis), angle, Quaternion.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void conjugate()
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_conjugate(this.swigCPtr);
		}

		public Quaternion getConjugated()
		{
			return new Quaternion(CocoStudioEngineAdapterPINVOKE.Quaternion_getConjugated(this.swigCPtr), true);
		}

		public bool inverse()
		{
			return CocoStudioEngineAdapterPINVOKE.Quaternion_inverse(this.swigCPtr);
		}

		public Quaternion getInversed()
		{
			return new Quaternion(CocoStudioEngineAdapterPINVOKE.Quaternion_getInversed(this.swigCPtr), true);
		}

		public void multiply(Quaternion q)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_multiply__SWIG_0(this.swigCPtr, Quaternion.getCPtr(q));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void multiply(Quaternion q1, Quaternion q2, Quaternion dst)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_multiply__SWIG_1(Quaternion.getCPtr(q1), Quaternion.getCPtr(q2), Quaternion.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void normalize()
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_normalize(this.swigCPtr);
		}

		public Quaternion getNormalized()
		{
			return new Quaternion(CocoStudioEngineAdapterPINVOKE.Quaternion_getNormalized(this.swigCPtr), true);
		}

		public void set(float xx, float yy, float zz, float ww)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_set__SWIG_0(this.swigCPtr, xx, yy, zz, ww);
		}

		public void set(Vec3 axis, float angle)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_set__SWIG_1(this.swigCPtr, Vec3.getCPtr(axis), angle);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void set(Quaternion q)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_set__SWIG_2(this.swigCPtr, Quaternion.getCPtr(q));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void setIdentity()
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_setIdentity(this.swigCPtr);
		}

		public float toAxisAngle(Vec3 e)
		{
			return CocoStudioEngineAdapterPINVOKE.Quaternion_toAxisAngle(this.swigCPtr, Vec3.getCPtr(e));
		}

		public static void lerp(Quaternion q1, Quaternion q2, float t, Quaternion dst)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_lerp(Quaternion.getCPtr(q1), Quaternion.getCPtr(q2), t, Quaternion.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void slerp(Quaternion q1, Quaternion q2, float t, Quaternion dst)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_slerp(Quaternion.getCPtr(q1), Quaternion.getCPtr(q2), t, Quaternion.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void squad(Quaternion q1, Quaternion q2, Quaternion s1, Quaternion s2, float t, Quaternion dst)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_squad(Quaternion.getCPtr(q1), Quaternion.getCPtr(q2), Quaternion.getCPtr(s1), Quaternion.getCPtr(s2), t, Quaternion.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static Quaternion ZERO
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quaternion_ZERO_get();
				return (intPtr == IntPtr.Zero) ? null : new Quaternion(intPtr, false);
			}
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}
