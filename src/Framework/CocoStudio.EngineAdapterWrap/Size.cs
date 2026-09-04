using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000072 RID: 114
	internal class Size : IDisposable
	{
		// Token: 0x06000C81 RID: 3201 RVA: 0x0001705A File Offset: 0x0001525A
		public Size(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0001707C File Offset: 0x0001527C
		public static HandleRef getCPtr(Size obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x000170A8 File Offset: 0x000152A8
		~Size()
		{
			this.Dispose();
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0001710C File Offset: 0x0001530C
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
								CocoStudioEngineAdapterPINVOKE.delete_Size(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Size(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x00017214 File Offset: 0x00015414
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x00017204 File Offset: 0x00015404
		public float width
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Size_width_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Size_width_set(this.swigCPtr, value);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x00017244 File Offset: 0x00015444
		// (set) Token: 0x06000C87 RID: 3207 RVA: 0x00017233 File Offset: 0x00015433
		public float height
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Size_height_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Size_height_set(this.swigCPtr, value);
			}
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00017263 File Offset: 0x00015463
		public Size() : this(CocoStudioEngineAdapterPINVOKE.new_Size__SWIG_0(), true)
		{
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00017274 File Offset: 0x00015474
		public Size(float width, float height) : this(CocoStudioEngineAdapterPINVOKE.new_Size__SWIG_1(width, height), true)
		{
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00017288 File Offset: 0x00015488
		public Size(Size other) : this(CocoStudioEngineAdapterPINVOKE.new_Size__SWIG_2(Size.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x000172BC File Offset: 0x000154BC
		public Size(Vec2 point) : this(CocoStudioEngineAdapterPINVOKE.new_Size__SWIG_3(Vec2.getCPtr(point)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x000172F0 File Offset: 0x000154F0
		public void setSize(float width, float height)
		{
			CocoStudioEngineAdapterPINVOKE.Size_setSize(this.swigCPtr, width, height);
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x00017304 File Offset: 0x00015504
		public bool equals(Size target)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.Size_equals(this.swigCPtr, Size.getCPtr(target));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x0001733C File Offset: 0x0001553C
		public static Size ZERO
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Size_ZERO_get();
				return (intPtr == IntPtr.Zero) ? null : new Size(intPtr, false);
			}
		}

		// Token: 0x040000D8 RID: 216
		private HandleRef swigCPtr;

		// Token: 0x040000D9 RID: 217
		protected bool swigCMemOwn;
	}
}
