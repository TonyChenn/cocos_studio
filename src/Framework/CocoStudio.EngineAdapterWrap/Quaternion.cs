using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000070 RID: 112
	public class Quaternion : IDisposable
	{
		// Token: 0x06000C41 RID: 3137 RVA: 0x000164A5 File Offset: 0x000146A5
		public Quaternion(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x000164C4 File Offset: 0x000146C4
		public static HandleRef getCPtr(Quaternion obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x000164F0 File Offset: 0x000146F0
		~Quaternion()
		{
			this.Dispose();
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00016554 File Offset: 0x00014754
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

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x0001665C File Offset: 0x0001485C
		// (set) Token: 0x06000C45 RID: 3141 RVA: 0x0001664C File Offset: 0x0001484C
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

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0001668C File Offset: 0x0001488C
		// (set) Token: 0x06000C47 RID: 3143 RVA: 0x0001667B File Offset: 0x0001487B
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x000166BC File Offset: 0x000148BC
		// (set) Token: 0x06000C49 RID: 3145 RVA: 0x000166AB File Offset: 0x000148AB
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

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x000166EC File Offset: 0x000148EC
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x000166DB File Offset: 0x000148DB
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

		// Token: 0x06000C4D RID: 3149 RVA: 0x0001670B File Offset: 0x0001490B
		public Quaternion() : this(CocoStudioEngineAdapterPINVOKE.new_Quaternion__SWIG_0(), true)
		{
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0001671C File Offset: 0x0001491C
		public Quaternion(float xx, float yy, float zz, float ww) : this(CocoStudioEngineAdapterPINVOKE.new_Quaternion__SWIG_1(xx, yy, zz, ww), true)
		{
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00016734 File Offset: 0x00014934
		public Quaternion(Vec3 axis, float angle) : this(CocoStudioEngineAdapterPINVOKE.new_Quaternion__SWIG_2(Vec3.getCPtr(axis), angle), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x0001676C File Offset: 0x0001496C
		public Quaternion(Quaternion copy) : this(CocoStudioEngineAdapterPINVOKE.new_Quaternion__SWIG_3(Quaternion.getCPtr(copy)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x000167A0 File Offset: 0x000149A0
		public static Quaternion identity()
		{
			return new Quaternion(CocoStudioEngineAdapterPINVOKE.Quaternion_identity(), false);
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x000167C0 File Offset: 0x000149C0
		public static Quaternion zero()
		{
			return new Quaternion(CocoStudioEngineAdapterPINVOKE.Quaternion_zero(), false);
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x000167E0 File Offset: 0x000149E0
		public bool isIdentity()
		{
			return CocoStudioEngineAdapterPINVOKE.Quaternion_isIdentity(this.swigCPtr);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00016800 File Offset: 0x00014A00
		public bool isZero()
		{
			return CocoStudioEngineAdapterPINVOKE.Quaternion_isZero(this.swigCPtr);
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00016820 File Offset: 0x00014A20
		public static void createFromAxisAngle(Vec3 axis, float angle, Quaternion dst)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_createFromAxisAngle(Vec3.getCPtr(axis), angle, Quaternion.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00016853 File Offset: 0x00014A53
		public void conjugate()
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_conjugate(this.swigCPtr);
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00016864 File Offset: 0x00014A64
		public Quaternion getConjugated()
		{
			return new Quaternion(CocoStudioEngineAdapterPINVOKE.Quaternion_getConjugated(this.swigCPtr), true);
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0001688C File Offset: 0x00014A8C
		public bool inverse()
		{
			return CocoStudioEngineAdapterPINVOKE.Quaternion_inverse(this.swigCPtr);
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x000168AC File Offset: 0x00014AAC
		public Quaternion getInversed()
		{
			return new Quaternion(CocoStudioEngineAdapterPINVOKE.Quaternion_getInversed(this.swigCPtr), true);
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x000168D4 File Offset: 0x00014AD4
		public void multiply(Quaternion q)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_multiply__SWIG_0(this.swigCPtr, Quaternion.getCPtr(q));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00016908 File Offset: 0x00014B08
		public static void multiply(Quaternion q1, Quaternion q2, Quaternion dst)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_multiply__SWIG_1(Quaternion.getCPtr(q1), Quaternion.getCPtr(q2), Quaternion.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x00016940 File Offset: 0x00014B40
		public void normalize()
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_normalize(this.swigCPtr);
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x00016950 File Offset: 0x00014B50
		public Quaternion getNormalized()
		{
			return new Quaternion(CocoStudioEngineAdapterPINVOKE.Quaternion_getNormalized(this.swigCPtr), true);
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x00016975 File Offset: 0x00014B75
		public void set(float xx, float yy, float zz, float ww)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_set__SWIG_0(this.swigCPtr, xx, yy, zz, ww);
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0001698C File Offset: 0x00014B8C
		public void set(Vec3 axis, float angle)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_set__SWIG_1(this.swigCPtr, Vec3.getCPtr(axis), angle);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x000169C0 File Offset: 0x00014BC0
		public void set(Quaternion q)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_set__SWIG_2(this.swigCPtr, Quaternion.getCPtr(q));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x000169F2 File Offset: 0x00014BF2
		public void setIdentity()
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_setIdentity(this.swigCPtr);
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00016A04 File Offset: 0x00014C04
		public float toAxisAngle(Vec3 e)
		{
			return CocoStudioEngineAdapterPINVOKE.Quaternion_toAxisAngle(this.swigCPtr, Vec3.getCPtr(e));
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00016A2C File Offset: 0x00014C2C
		public static void lerp(Quaternion q1, Quaternion q2, float t, Quaternion dst)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_lerp(Quaternion.getCPtr(q1), Quaternion.getCPtr(q2), t, Quaternion.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x00016A68 File Offset: 0x00014C68
		public static void slerp(Quaternion q1, Quaternion q2, float t, Quaternion dst)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_slerp(Quaternion.getCPtr(q1), Quaternion.getCPtr(q2), t, Quaternion.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00016AA4 File Offset: 0x00014CA4
		public static void squad(Quaternion q1, Quaternion q2, Quaternion s1, Quaternion s2, float t, Quaternion dst)
		{
			CocoStudioEngineAdapterPINVOKE.Quaternion_squad(Quaternion.getCPtr(q1), Quaternion.getCPtr(q2), Quaternion.getCPtr(s1), Quaternion.getCPtr(s2), t, Quaternion.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x00016AEC File Offset: 0x00014CEC
		public static Quaternion ZERO
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quaternion_ZERO_get();
				return (intPtr == IntPtr.Zero) ? null : new Quaternion(intPtr, false);
			}
		}

		// Token: 0x040000D4 RID: 212
		private HandleRef swigCPtr;

		// Token: 0x040000D5 RID: 213
		protected bool swigCMemOwn;
	}
}
