using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000014 RID: 20
	public class CSRect : IDisposable
	{
		// Token: 0x06000142 RID: 322 RVA: 0x000061B1 File Offset: 0x000043B1
		public CSRect(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000061D0 File Offset: 0x000043D0
		public static HandleRef getCPtr(CSRect obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000061FC File Offset: 0x000043FC
		~CSRect()
		{
			this.Dispose();
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006260 File Offset: 0x00004460
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
								CocoStudioEngineAdapterPINVOKE.delete_CSRect(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSRect(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006358 File Offset: 0x00004558
		public CSRect(float x, float y, float width, float height) : this(CocoStudioEngineAdapterPINVOKE.new_CSRect__SWIG_0(x, y, width, height), true)
		{
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000636E File Offset: 0x0000456E
		public CSRect() : this(CocoStudioEngineAdapterPINVOKE.new_CSRect__SWIG_1(), true)
		{
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006380 File Offset: 0x00004580
		public float MinX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSRect_MinX(this.swigCPtr);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000063A0 File Offset: 0x000045A0
		public float MinY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSRect_MinY(this.swigCPtr);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000063C0 File Offset: 0x000045C0
		public float MaxX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSRect_MaxX(this.swigCPtr);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000063E0 File Offset: 0x000045E0
		public float MaxY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSRect_MaxY(this.swigCPtr);
		}

		// Token: 0x0400001A RID: 26
		private HandleRef swigCPtr;

		// Token: 0x0400001B RID: 27
		protected bool swigCMemOwn;
	}
}
