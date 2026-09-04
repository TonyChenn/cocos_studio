using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000083 RID: 131
	public class Vec2 : IDisposable
	{
		// Token: 0x06000D33 RID: 3379 RVA: 0x00019AC1 File Offset: 0x00017CC1
		public Vec2(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00019AE0 File Offset: 0x00017CE0
		public static HandleRef getCPtr(Vec2 obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00019B0C File Offset: 0x00017D0C
		~Vec2()
		{
			this.Dispose();
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00019B70 File Offset: 0x00017D70
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
								CocoStudioEngineAdapterPINVOKE.delete_Vec2(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Vec2(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000D38 RID: 3384 RVA: 0x00019C78 File Offset: 0x00017E78
		// (set) Token: 0x06000D37 RID: 3383 RVA: 0x00019C68 File Offset: 0x00017E68
		public float x
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Vec2_x_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Vec2_x_set(this.swigCPtr, value);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000D3A RID: 3386 RVA: 0x00019CA8 File Offset: 0x00017EA8
		// (set) Token: 0x06000D39 RID: 3385 RVA: 0x00019C97 File Offset: 0x00017E97
		public float y
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Vec2_y_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Vec2_y_set(this.swigCPtr, value);
			}
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00019CC7 File Offset: 0x00017EC7
		public Vec2() : this(CocoStudioEngineAdapterPINVOKE.new_Vec2__SWIG_0(), true)
		{
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00019CD8 File Offset: 0x00017ED8
		public Vec2(float xx, float yy) : this(CocoStudioEngineAdapterPINVOKE.new_Vec2__SWIG_1(xx, yy), true)
		{
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00019CEC File Offset: 0x00017EEC
		public Vec2(Vec2 p1, Vec2 p2) : this(CocoStudioEngineAdapterPINVOKE.new_Vec2__SWIG_2(Vec2.getCPtr(p1), Vec2.getCPtr(p2)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00019D28 File Offset: 0x00017F28
		public Vec2(Vec2 copy) : this(CocoStudioEngineAdapterPINVOKE.new_Vec2__SWIG_3(Vec2.getCPtr(copy)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00019D5C File Offset: 0x00017F5C
		public bool isZero()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_isZero(this.swigCPtr);
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00019D7C File Offset: 0x00017F7C
		public bool isOne()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_isOne(this.swigCPtr);
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00019D9C File Offset: 0x00017F9C
		public static float angle(Vec2 v1, Vec2 v2)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_angle(Vec2.getCPtr(v1), Vec2.getCPtr(v2));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00019DD4 File Offset: 0x00017FD4
		public void add(Vec2 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_add__SWIG_0(this.swigCPtr, Vec2.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00019E08 File Offset: 0x00018008
		public static void add(Vec2 v1, Vec2 v2, Vec2 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_add__SWIG_1(Vec2.getCPtr(v1), Vec2.getCPtr(v2), Vec2.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00019E40 File Offset: 0x00018040
		public void clamp(Vec2 min, Vec2 max)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_clamp__SWIG_0(this.swigCPtr, Vec2.getCPtr(min), Vec2.getCPtr(max));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00019E78 File Offset: 0x00018078
		public static void clamp(Vec2 v, Vec2 min, Vec2 max, Vec2 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_clamp__SWIG_1(Vec2.getCPtr(v), Vec2.getCPtr(min), Vec2.getCPtr(max), Vec2.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00019EB8 File Offset: 0x000180B8
		public float distance(Vec2 v)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_distance(this.swigCPtr, Vec2.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00019EF0 File Offset: 0x000180F0
		public float distanceSquared(Vec2 v)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_distanceSquared(this.swigCPtr, Vec2.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00019F28 File Offset: 0x00018128
		public float dot(Vec2 v)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_dot__SWIG_0(this.swigCPtr, Vec2.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00019F60 File Offset: 0x00018160
		public float length()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_length(this.swigCPtr);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00019F80 File Offset: 0x00018180
		public float lengthSquared()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_lengthSquared(this.swigCPtr);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00019F9F File Offset: 0x0001819F
		public void negate()
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_negate(this.swigCPtr);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00019FAE File Offset: 0x000181AE
		public void normalize()
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_normalize(this.swigCPtr);
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00019FC0 File Offset: 0x000181C0
		public Vec2 getNormalized()
		{
			return new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_getNormalized(this.swigCPtr), true);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00019FE5 File Offset: 0x000181E5
		public void scale(float scalar)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_scale__SWIG_0(this.swigCPtr, scalar);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00019FF8 File Offset: 0x000181F8
		public void scale(Vec2 scale)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_scale__SWIG_1(this.swigCPtr, Vec2.getCPtr(scale));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0001A02C File Offset: 0x0001822C
		public void rotate(Vec2 point, float angle)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_rotate__SWIG_0(this.swigCPtr, Vec2.getCPtr(point), angle);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0001A05F File Offset: 0x0001825F
		public void set(float xx, float yy)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_set__SWIG_0(this.swigCPtr, xx, yy);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0001A070 File Offset: 0x00018270
		public void set(Vec2 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_set__SWIG_1(this.swigCPtr, Vec2.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0001A0A4 File Offset: 0x000182A4
		public void set(Vec2 p1, Vec2 p2)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_set__SWIG_2(this.swigCPtr, Vec2.getCPtr(p1), Vec2.getCPtr(p2));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0001A0DC File Offset: 0x000182DC
		public void setZero()
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_setZero(this.swigCPtr);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0001A0EC File Offset: 0x000182EC
		public void subtract(Vec2 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_subtract__SWIG_0(this.swigCPtr, Vec2.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0001A120 File Offset: 0x00018320
		public static void subtract(Vec2 v1, Vec2 v2, Vec2 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_subtract__SWIG_1(Vec2.getCPtr(v1), Vec2.getCPtr(v2), Vec2.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0001A158 File Offset: 0x00018358
		public void smooth(Vec2 target, float elapsedTime, float responseTime)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_smooth(this.swigCPtr, Vec2.getCPtr(target), elapsedTime, responseTime);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0001A18C File Offset: 0x0001838C
		public void setPoint(float xx, float yy)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_setPoint(this.swigCPtr, xx, yy);
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0001A1A0 File Offset: 0x000183A0
		public bool equals(Vec2 target)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_equals(this.swigCPtr, Vec2.getCPtr(target));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0001A1D8 File Offset: 0x000183D8
		public bool fuzzyEquals(Vec2 target, float variance)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_fuzzyEquals(this.swigCPtr, Vec2.getCPtr(target), variance);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0001A210 File Offset: 0x00018410
		public float getLength()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_getLength(this.swigCPtr);
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0001A230 File Offset: 0x00018430
		public float getLengthSq()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_getLengthSq(this.swigCPtr);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0001A250 File Offset: 0x00018450
		public float getDistanceSq(Vec2 other)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_getDistanceSq(this.swigCPtr, Vec2.getCPtr(other));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0001A288 File Offset: 0x00018488
		public float getDistance(Vec2 other)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_getDistance(this.swigCPtr, Vec2.getCPtr(other));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x0001A2C0 File Offset: 0x000184C0
		public float getAngle()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_getAngle__SWIG_0(this.swigCPtr);
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x0001A2E0 File Offset: 0x000184E0
		public float getAngle(Vec2 other)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_getAngle__SWIG_1(this.swigCPtr, Vec2.getCPtr(other));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x0001A318 File Offset: 0x00018518
		public float cross(Vec2 other)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_cross(this.swigCPtr, Vec2.getCPtr(other));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0001A350 File Offset: 0x00018550
		public Vec2 getPerp()
		{
			return new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_getPerp(this.swigCPtr), true);
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0001A378 File Offset: 0x00018578
		public Vec2 getMidpoint(Vec2 other)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_getMidpoint(this.swigCPtr, Vec2.getCPtr(other)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0001A3B8 File Offset: 0x000185B8
		public Vec2 getClampPoint(Vec2 min_inclusive, Vec2 max_inclusive)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_getClampPoint(this.swigCPtr, Vec2.getCPtr(min_inclusive), Vec2.getCPtr(max_inclusive)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x0001A3FC File Offset: 0x000185FC
		public Vec2 getRPerp()
		{
			return new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_getRPerp(this.swigCPtr), true);
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0001A424 File Offset: 0x00018624
		public Vec2 project(Vec2 other)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_project(this.swigCPtr, Vec2.getCPtr(other)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0001A464 File Offset: 0x00018664
		public Vec2 rotate(Vec2 other)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_rotate__SWIG_1(this.swigCPtr, Vec2.getCPtr(other)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0001A4A4 File Offset: 0x000186A4
		public Vec2 unrotate(Vec2 other)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_unrotate(this.swigCPtr, Vec2.getCPtr(other)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0001A4E4 File Offset: 0x000186E4
		public Vec2 lerp(Vec2 other, float alpha)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_lerp(this.swigCPtr, Vec2.getCPtr(other), alpha), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0001A524 File Offset: 0x00018724
		public Vec2 rotateByAngle(Vec2 pivot, float angle)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_rotateByAngle(this.swigCPtr, Vec2.getCPtr(pivot), angle), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0001A564 File Offset: 0x00018764
		public static Vec2 forAngle(float a)
		{
			return new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_forAngle(a), true);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x0001A584 File Offset: 0x00018784
		public static bool isLineOverlap(Vec2 A, Vec2 B, Vec2 C, Vec2 D)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_isLineOverlap(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x0001A5C8 File Offset: 0x000187C8
		public static bool isLineParallel(Vec2 A, Vec2 B, Vec2 C, Vec2 D)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_isLineParallel(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x0001A60C File Offset: 0x0001880C
		public static bool isSegmentOverlap(Vec2 A, Vec2 B, Vec2 C, Vec2 D, Vec2 S, Vec2 E)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_isSegmentOverlap__SWIG_0(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D), Vec2.getCPtr(S), Vec2.getCPtr(E));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0001A660 File Offset: 0x00018860
		public static bool isSegmentOverlap(Vec2 A, Vec2 B, Vec2 C, Vec2 D, Vec2 S)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_isSegmentOverlap__SWIG_1(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D), Vec2.getCPtr(S));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x0001A6AC File Offset: 0x000188AC
		public static bool isSegmentOverlap(Vec2 A, Vec2 B, Vec2 C, Vec2 D)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_isSegmentOverlap__SWIG_2(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x0001A6F0 File Offset: 0x000188F0
		public static bool isSegmentIntersect(Vec2 A, Vec2 B, Vec2 C, Vec2 D)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_isSegmentIntersect(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0001A734 File Offset: 0x00018934
		public static Vec2 getIntersectPoint(Vec2 A, Vec2 B, Vec2 C, Vec2 D)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_getIntersectPoint(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x0001A780 File Offset: 0x00018980
		public static Vec2 ZERO
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ZERO_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x0001A7B4 File Offset: 0x000189B4
		public static Vec2 ONE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ONE_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000D75 RID: 3445 RVA: 0x0001A7E8 File Offset: 0x000189E8
		public static Vec2 UNIT_X
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_UNIT_X_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x0001A81C File Offset: 0x00018A1C
		public static Vec2 UNIT_Y
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_UNIT_Y_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000D77 RID: 3447 RVA: 0x0001A850 File Offset: 0x00018A50
		public static Vec2 ANCHOR_MIDDLE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_MIDDLE_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x0001A884 File Offset: 0x00018A84
		public static Vec2 ANCHOR_BOTTOM_LEFT
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_BOTTOM_LEFT_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x0001A8B8 File Offset: 0x00018AB8
		public static Vec2 ANCHOR_TOP_LEFT
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_TOP_LEFT_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000D7A RID: 3450 RVA: 0x0001A8EC File Offset: 0x00018AEC
		public static Vec2 ANCHOR_BOTTOM_RIGHT
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_BOTTOM_RIGHT_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x0001A920 File Offset: 0x00018B20
		public static Vec2 ANCHOR_TOP_RIGHT
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_TOP_RIGHT_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x0001A954 File Offset: 0x00018B54
		public static Vec2 ANCHOR_MIDDLE_RIGHT
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_MIDDLE_RIGHT_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000D7D RID: 3453 RVA: 0x0001A988 File Offset: 0x00018B88
		public static Vec2 ANCHOR_MIDDLE_LEFT
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_MIDDLE_LEFT_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x0001A9BC File Offset: 0x00018BBC
		public static Vec2 ANCHOR_MIDDLE_TOP
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_MIDDLE_TOP_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x0001A9F0 File Offset: 0x00018BF0
		public static Vec2 ANCHOR_MIDDLE_BOTTOM
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_MIDDLE_BOTTOM_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		// Token: 0x040000FB RID: 251
		private HandleRef swigCPtr;

		// Token: 0x040000FC RID: 252
		protected bool swigCMemOwn;
	}
}
