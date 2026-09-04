using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000071 RID: 113
	internal class Rect : IDisposable
	{
		// Token: 0x06000C67 RID: 3175 RVA: 0x00016B1E File Offset: 0x00014D1E
		public Rect(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00016B40 File Offset: 0x00014D40
		public static HandleRef getCPtr(Rect obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00016B6C File Offset: 0x00014D6C
		~Rect()
		{
			this.Dispose();
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00016BD0 File Offset: 0x00014DD0
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

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x00016CE0 File Offset: 0x00014EE0
		// (set) Token: 0x06000C6B RID: 3179 RVA: 0x00016CC8 File Offset: 0x00014EC8
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x00016D30 File Offset: 0x00014F30
		// (set) Token: 0x06000C6D RID: 3181 RVA: 0x00016D18 File Offset: 0x00014F18
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

		// Token: 0x06000C6F RID: 3183 RVA: 0x00016D68 File Offset: 0x00014F68
		public Rect() : this(CocoStudioEngineAdapterPINVOKE.new_Rect__SWIG_0(), true)
		{
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00016D79 File Offset: 0x00014F79
		public Rect(float x, float y, float width, float height) : this(CocoStudioEngineAdapterPINVOKE.new_Rect__SWIG_1(x, y, width, height), true)
		{
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00016D90 File Offset: 0x00014F90
		public Rect(Vec2 pos, Size dimension) : this(CocoStudioEngineAdapterPINVOKE.new_Rect__SWIG_2(Vec2.getCPtr(pos), Size.getCPtr(dimension)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00016DCC File Offset: 0x00014FCC
		public Rect(Rect other) : this(CocoStudioEngineAdapterPINVOKE.new_Rect__SWIG_3(Rect.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00016E00 File Offset: 0x00015000
		public void setRect(float x, float y, float width, float height)
		{
			CocoStudioEngineAdapterPINVOKE.Rect_setRect(this.swigCPtr, x, y, width, height);
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x00016E14 File Offset: 0x00015014
		public float getMinX()
		{
			return CocoStudioEngineAdapterPINVOKE.Rect_getMinX(this.swigCPtr);
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x00016E34 File Offset: 0x00015034
		public float getMidX()
		{
			return CocoStudioEngineAdapterPINVOKE.Rect_getMidX(this.swigCPtr);
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00016E54 File Offset: 0x00015054
		public float getMaxX()
		{
			return CocoStudioEngineAdapterPINVOKE.Rect_getMaxX(this.swigCPtr);
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00016E74 File Offset: 0x00015074
		public float getMinY()
		{
			return CocoStudioEngineAdapterPINVOKE.Rect_getMinY(this.swigCPtr);
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00016E94 File Offset: 0x00015094
		public float getMidY()
		{
			return CocoStudioEngineAdapterPINVOKE.Rect_getMidY(this.swigCPtr);
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00016EB4 File Offset: 0x000150B4
		public float getMaxY()
		{
			return CocoStudioEngineAdapterPINVOKE.Rect_getMaxY(this.swigCPtr);
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00016ED4 File Offset: 0x000150D4
		public bool equals(Rect rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Rect_equals(this.swigCPtr, Rect.getCPtr(rect));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00016F0C File Offset: 0x0001510C
		public bool containsPoint(Vec2 point)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Rect_containsPoint(this.swigCPtr, Vec2.getCPtr(point));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00016F44 File Offset: 0x00015144
		public bool intersectsRect(Rect rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Rect_intersectsRect(this.swigCPtr, Rect.getCPtr(rect));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00016F7C File Offset: 0x0001517C
		public bool intersectsCircle(Vec2 center, float radius)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Rect_intersectsCircle(this.swigCPtr, Vec2.getCPtr(center), radius);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00016FB4 File Offset: 0x000151B4
		public Rect unionWithRect(Rect rect)
		{
			Rect result = new Rect(CocoStudioEngineAdapterPINVOKE.Rect_unionWithRect(this.swigCPtr, Rect.getCPtr(rect)), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00016FF4 File Offset: 0x000151F4
		public void merge(Rect rect)
		{
			CocoStudioEngineAdapterPINVOKE.Rect_merge(this.swigCPtr, Rect.getCPtr(rect));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x00017028 File Offset: 0x00015228
		public static Rect ZERO
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Rect_ZERO_get();
				return (intPtr == IntPtr.Zero) ? null : new Rect(intPtr, false);
			}
		}

		// Token: 0x040000D6 RID: 214
		private HandleRef swigCPtr;

		// Token: 0x040000D7 RID: 215
		protected bool swigCMemOwn;
	}
}
