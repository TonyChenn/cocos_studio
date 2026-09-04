using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000084 RID: 132
	public class Vec3 : IDisposable
	{
		// Token: 0x06000D80 RID: 3456 RVA: 0x0001AA22 File Offset: 0x00018C22
		public Vec3(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0001AA44 File Offset: 0x00018C44
		public static HandleRef getCPtr(Vec3 obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0001AA70 File Offset: 0x00018C70
		~Vec3()
		{
			this.Dispose();
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0001AAD4 File Offset: 0x00018CD4
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

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x0001ABDC File Offset: 0x00018DDC
		// (set) Token: 0x06000D84 RID: 3460 RVA: 0x0001ABCC File Offset: 0x00018DCC
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

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x0001AC0C File Offset: 0x00018E0C
		// (set) Token: 0x06000D86 RID: 3462 RVA: 0x0001ABFB File Offset: 0x00018DFB
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

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x0001AC3C File Offset: 0x00018E3C
		// (set) Token: 0x06000D88 RID: 3464 RVA: 0x0001AC2B File Offset: 0x00018E2B
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

		// Token: 0x06000D8A RID: 3466 RVA: 0x0001AC5B File Offset: 0x00018E5B
		public Vec3() : this(CocoStudioEngineAdapterPINVOKE.new_Vec3__SWIG_0(), true)
		{
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0001AC6C File Offset: 0x00018E6C
		public Vec3(float xx, float yy, float zz) : this(CocoStudioEngineAdapterPINVOKE.new_Vec3__SWIG_1(xx, yy, zz), true)
		{
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0001AC80 File Offset: 0x00018E80
		public Vec3(Vec3 p1, Vec3 p2) : this(CocoStudioEngineAdapterPINVOKE.new_Vec3__SWIG_2(Vec3.getCPtr(p1), Vec3.getCPtr(p2)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0001ACBC File Offset: 0x00018EBC
		public Vec3(Vec3 copy) : this(CocoStudioEngineAdapterPINVOKE.new_Vec3__SWIG_3(Vec3.getCPtr(copy)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0001ACF0 File Offset: 0x00018EF0
		public static Vec3 fromColor(uint color)
		{
			return new Vec3(CocoStudioEngineAdapterPINVOKE.Vec3_fromColor(color), true);
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0001AD10 File Offset: 0x00018F10
		public bool isZero()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec3_isZero(this.swigCPtr);
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0001AD30 File Offset: 0x00018F30
		public bool isOne()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec3_isOne(this.swigCPtr);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0001AD50 File Offset: 0x00018F50
		public static float angle(Vec3 v1, Vec3 v2)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec3_angle(Vec3.getCPtr(v1), Vec3.getCPtr(v2));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0001AD88 File Offset: 0x00018F88
		public void add(Vec3 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_add__SWIG_0(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0001ADBA File Offset: 0x00018FBA
		public void add(float xx, float yy, float zz)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_add__SWIG_1(this.swigCPtr, xx, yy, zz);
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0001ADCC File Offset: 0x00018FCC
		public static void add(Vec3 v1, Vec3 v2, Vec3 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_add__SWIG_2(Vec3.getCPtr(v1), Vec3.getCPtr(v2), Vec3.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0001AE04 File Offset: 0x00019004
		public void clamp(Vec3 min, Vec3 max)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_clamp__SWIG_0(this.swigCPtr, Vec3.getCPtr(min), Vec3.getCPtr(max));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0001AE3C File Offset: 0x0001903C
		public static void clamp(Vec3 v, Vec3 min, Vec3 max, Vec3 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_clamp__SWIG_1(Vec3.getCPtr(v), Vec3.getCPtr(min), Vec3.getCPtr(max), Vec3.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x0001AE7C File Offset: 0x0001907C
		public void cross(Vec3 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_cross__SWIG_0(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x0001AEB0 File Offset: 0x000190B0
		public static void cross(Vec3 v1, Vec3 v2, Vec3 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_cross__SWIG_1(Vec3.getCPtr(v1), Vec3.getCPtr(v2), Vec3.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x0001AEE8 File Offset: 0x000190E8
		public float distance(Vec3 v)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec3_distance(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0001AF20 File Offset: 0x00019120
		public float distanceSquared(Vec3 v)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec3_distanceSquared(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0001AF58 File Offset: 0x00019158
		public float dot(Vec3 v)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec3_dot__SWIG_0(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0001AF90 File Offset: 0x00019190
		public float length()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec3_length(this.swigCPtr);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0001AFB0 File Offset: 0x000191B0
		public float lengthSquared()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec3_lengthSquared(this.swigCPtr);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x0001AFCF File Offset: 0x000191CF
		public void negate()
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_negate(this.swigCPtr);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0001AFDE File Offset: 0x000191DE
		public void normalize()
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_normalize(this.swigCPtr);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0001AFF0 File Offset: 0x000191F0
		public Vec3 getNormalized()
		{
			return new Vec3(CocoStudioEngineAdapterPINVOKE.Vec3_getNormalized(this.swigCPtr), true);
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0001B015 File Offset: 0x00019215
		public void scale(float scalar)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_scale(this.swigCPtr, scalar);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0001B025 File Offset: 0x00019225
		public void set(float xx, float yy, float zz)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_set__SWIG_0(this.swigCPtr, xx, yy, zz);
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0001B038 File Offset: 0x00019238
		public void set(Vec3 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_set__SWIG_1(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0001B06C File Offset: 0x0001926C
		public void set(Vec3 p1, Vec3 p2)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_set__SWIG_2(this.swigCPtr, Vec3.getCPtr(p1), Vec3.getCPtr(p2));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0001B0A4 File Offset: 0x000192A4
		public void setZero()
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_setZero(this.swigCPtr);
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0001B0B4 File Offset: 0x000192B4
		public void subtract(Vec3 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_subtract__SWIG_0(this.swigCPtr, Vec3.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0001B0E8 File Offset: 0x000192E8
		public static void subtract(Vec3 v1, Vec3 v2, Vec3 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_subtract__SWIG_1(Vec3.getCPtr(v1), Vec3.getCPtr(v2), Vec3.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0001B120 File Offset: 0x00019320
		public void smooth(Vec3 target, float elapsedTime, float responseTime)
		{
			CocoStudioEngineAdapterPINVOKE.Vec3_smooth(this.swigCPtr, Vec3.getCPtr(target), elapsedTime, responseTime);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0001B154 File Offset: 0x00019354
		public Vec3 lerp(Vec3 target, float alpha)
		{
			Vec3 result = new Vec3(CocoStudioEngineAdapterPINVOKE.Vec3_lerp(this.swigCPtr, Vec3.getCPtr(target), alpha), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x0001B194 File Offset: 0x00019394
		public static Vec3 ZERO
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec3_ZERO_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x0001B1C8 File Offset: 0x000193C8
		public static Vec3 ONE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec3_ONE_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x0001B1FC File Offset: 0x000193FC
		public static Vec3 UNIT_X
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec3_UNIT_X_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x0001B230 File Offset: 0x00019430
		public static Vec3 UNIT_Y
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec3_UNIT_Y_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x0001B264 File Offset: 0x00019464
		public static Vec3 UNIT_Z
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec3_UNIT_Z_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
		}

		// Token: 0x040000FD RID: 253
		private HandleRef swigCPtr;

		// Token: 0x040000FE RID: 254
		protected bool swigCMemOwn;
	}
}
