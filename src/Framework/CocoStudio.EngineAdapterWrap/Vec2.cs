using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class Vec2 : IDisposable
	{
		public Vec2(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(Vec2 obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~Vec2()
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

		public Vec2() : this(CocoStudioEngineAdapterPINVOKE.new_Vec2__SWIG_0(), true)
		{
		}

		public Vec2(float xx, float yy) : this(CocoStudioEngineAdapterPINVOKE.new_Vec2__SWIG_1(xx, yy), true)
		{
		}

		public Vec2(Vec2 p1, Vec2 p2) : this(CocoStudioEngineAdapterPINVOKE.new_Vec2__SWIG_2(Vec2.getCPtr(p1), Vec2.getCPtr(p2)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public Vec2(Vec2 copy) : this(CocoStudioEngineAdapterPINVOKE.new_Vec2__SWIG_3(Vec2.getCPtr(copy)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public bool isZero()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_isZero(this.swigCPtr);
		}

		public bool isOne()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_isOne(this.swigCPtr);
		}

		public static float angle(Vec2 v1, Vec2 v2)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_angle(Vec2.getCPtr(v1), Vec2.getCPtr(v2));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void add(Vec2 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_add__SWIG_0(this.swigCPtr, Vec2.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void add(Vec2 v1, Vec2 v2, Vec2 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_add__SWIG_1(Vec2.getCPtr(v1), Vec2.getCPtr(v2), Vec2.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void clamp(Vec2 min, Vec2 max)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_clamp__SWIG_0(this.swigCPtr, Vec2.getCPtr(min), Vec2.getCPtr(max));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void clamp(Vec2 v, Vec2 min, Vec2 max, Vec2 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_clamp__SWIG_1(Vec2.getCPtr(v), Vec2.getCPtr(min), Vec2.getCPtr(max), Vec2.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public float distance(Vec2 v)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_distance(this.swigCPtr, Vec2.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public float distanceSquared(Vec2 v)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_distanceSquared(this.swigCPtr, Vec2.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public float dot(Vec2 v)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_dot__SWIG_0(this.swigCPtr, Vec2.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public float length()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_length(this.swigCPtr);
		}

		public float lengthSquared()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_lengthSquared(this.swigCPtr);
		}

		public void negate()
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_negate(this.swigCPtr);
		}

		public void normalize()
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_normalize(this.swigCPtr);
		}

		public Vec2 getNormalized()
		{
			return new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_getNormalized(this.swigCPtr), true);
		}

		public void scale(float scalar)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_scale__SWIG_0(this.swigCPtr, scalar);
		}

		public void scale(Vec2 scale)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_scale__SWIG_1(this.swigCPtr, Vec2.getCPtr(scale));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void rotate(Vec2 point, float angle)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_rotate__SWIG_0(this.swigCPtr, Vec2.getCPtr(point), angle);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void set(float xx, float yy)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_set__SWIG_0(this.swigCPtr, xx, yy);
		}

		public void set(Vec2 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_set__SWIG_1(this.swigCPtr, Vec2.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void set(Vec2 p1, Vec2 p2)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_set__SWIG_2(this.swigCPtr, Vec2.getCPtr(p1), Vec2.getCPtr(p2));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void setZero()
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_setZero(this.swigCPtr);
		}

		public void subtract(Vec2 v)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_subtract__SWIG_0(this.swigCPtr, Vec2.getCPtr(v));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public static void subtract(Vec2 v1, Vec2 v2, Vec2 dst)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_subtract__SWIG_1(Vec2.getCPtr(v1), Vec2.getCPtr(v2), Vec2.getCPtr(dst));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void smooth(Vec2 target, float elapsedTime, float responseTime)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_smooth(this.swigCPtr, Vec2.getCPtr(target), elapsedTime, responseTime);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void setPoint(float xx, float yy)
		{
			CocoStudioEngineAdapterPINVOKE.Vec2_setPoint(this.swigCPtr, xx, yy);
		}

		public bool equals(Vec2 target)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_equals(this.swigCPtr, Vec2.getCPtr(target));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public bool fuzzyEquals(Vec2 target, float variance)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_fuzzyEquals(this.swigCPtr, Vec2.getCPtr(target), variance);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public float getLength()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_getLength(this.swigCPtr);
		}

		public float getLengthSq()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_getLengthSq(this.swigCPtr);
		}

		public float getDistanceSq(Vec2 other)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_getDistanceSq(this.swigCPtr, Vec2.getCPtr(other));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public float getDistance(Vec2 other)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_getDistance(this.swigCPtr, Vec2.getCPtr(other));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public float getAngle()
		{
			return CocoStudioEngineAdapterPINVOKE.Vec2_getAngle__SWIG_0(this.swigCPtr);
		}

		public float getAngle(Vec2 other)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_getAngle__SWIG_1(this.swigCPtr, Vec2.getCPtr(other));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public float cross(Vec2 other)
		{
			float result = CocoStudioEngineAdapterPINVOKE.Vec2_cross(this.swigCPtr, Vec2.getCPtr(other));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public Vec2 getPerp()
		{
			return new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_getPerp(this.swigCPtr), true);
		}

		public Vec2 getMidpoint(Vec2 other)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_getMidpoint(this.swigCPtr, Vec2.getCPtr(other)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public Vec2 getClampPoint(Vec2 min_inclusive, Vec2 max_inclusive)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_getClampPoint(this.swigCPtr, Vec2.getCPtr(min_inclusive), Vec2.getCPtr(max_inclusive)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public Vec2 getRPerp()
		{
			return new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_getRPerp(this.swigCPtr), true);
		}

		public Vec2 project(Vec2 other)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_project(this.swigCPtr, Vec2.getCPtr(other)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public Vec2 rotate(Vec2 other)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_rotate__SWIG_1(this.swigCPtr, Vec2.getCPtr(other)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public Vec2 unrotate(Vec2 other)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_unrotate(this.swigCPtr, Vec2.getCPtr(other)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public Vec2 lerp(Vec2 other, float alpha)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_lerp(this.swigCPtr, Vec2.getCPtr(other), alpha), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public Vec2 rotateByAngle(Vec2 pivot, float angle)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_rotateByAngle(this.swigCPtr, Vec2.getCPtr(pivot), angle), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static Vec2 forAngle(float a)
		{
			return new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_forAngle(a), true);
		}

		public static bool isLineOverlap(Vec2 A, Vec2 B, Vec2 C, Vec2 D)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_isLineOverlap(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static bool isLineParallel(Vec2 A, Vec2 B, Vec2 C, Vec2 D)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_isLineParallel(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static bool isSegmentOverlap(Vec2 A, Vec2 B, Vec2 C, Vec2 D, Vec2 S, Vec2 E)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_isSegmentOverlap__SWIG_0(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D), Vec2.getCPtr(S), Vec2.getCPtr(E));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static bool isSegmentOverlap(Vec2 A, Vec2 B, Vec2 C, Vec2 D, Vec2 S)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_isSegmentOverlap__SWIG_1(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D), Vec2.getCPtr(S));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static bool isSegmentOverlap(Vec2 A, Vec2 B, Vec2 C, Vec2 D)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_isSegmentOverlap__SWIG_2(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static bool isSegmentIntersect(Vec2 A, Vec2 B, Vec2 C, Vec2 D)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Vec2_isSegmentIntersect(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static Vec2 getIntersectPoint(Vec2 A, Vec2 B, Vec2 C, Vec2 D)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.Vec2_getIntersectPoint(Vec2.getCPtr(A), Vec2.getCPtr(B), Vec2.getCPtr(C), Vec2.getCPtr(D)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public static Vec2 ZERO
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ZERO_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		public static Vec2 ONE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ONE_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		public static Vec2 UNIT_X
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_UNIT_X_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		public static Vec2 UNIT_Y
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_UNIT_Y_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		public static Vec2 ANCHOR_MIDDLE
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_MIDDLE_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		public static Vec2 ANCHOR_BOTTOM_LEFT
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_BOTTOM_LEFT_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		public static Vec2 ANCHOR_TOP_LEFT
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_TOP_LEFT_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		public static Vec2 ANCHOR_BOTTOM_RIGHT
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_BOTTOM_RIGHT_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		public static Vec2 ANCHOR_TOP_RIGHT
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_TOP_RIGHT_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		public static Vec2 ANCHOR_MIDDLE_RIGHT
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_MIDDLE_RIGHT_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		public static Vec2 ANCHOR_MIDDLE_LEFT
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_MIDDLE_LEFT_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		public static Vec2 ANCHOR_MIDDLE_TOP
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_MIDDLE_TOP_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		public static Vec2 ANCHOR_MIDDLE_BOTTOM
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Vec2_ANCHOR_MIDDLE_BOTTOM_get();
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}
