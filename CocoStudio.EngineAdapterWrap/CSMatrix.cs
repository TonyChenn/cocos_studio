using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000017 RID: 23
	public class CSMatrix : IDisposable
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00006408 File Offset: 0x00004608
		public float CX
		{
			get
			{
				return this.X();
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00006420 File Offset: 0x00004620
		public float CY
		{
			get
			{
				return this.Y();
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00006438 File Offset: 0x00004638
		public float CM11
		{
			get
			{
				return this.M11();
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00006450 File Offset: 0x00004650
		public float CM12
		{
			get
			{
				return this.M12();
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00006468 File Offset: 0x00004668
		public float CM21
		{
			get
			{
				return this.M21();
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00006480 File Offset: 0x00004680
		public float CM22
		{
			get
			{
				return this.M22();
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006498 File Offset: 0x00004698
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is CSMatrix)
			{
				CSMatrix csmatrix = (CSMatrix)obj;
				result = (obj != null && this.CX == csmatrix.CX && this.CY == csmatrix.CY && this.CM11 == csmatrix.CM11 && this.CM12 == csmatrix.CM12 && this.CM21 == csmatrix.CM21 && this.CM22 == csmatrix.CM22);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006524 File Offset: 0x00004724
		public override int GetHashCode()
		{
			return (this.CX.GetHashCode() ^ this.CY.GetHashCode()) | (this.CM11.GetHashCode() ^ this.CM12.GetHashCode()) | (this.CM21.GetHashCode() ^ this.CM22.GetHashCode());
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000658F File Offset: 0x0000478F
		public CSMatrix(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000065B0 File Offset: 0x000047B0
		public static HandleRef getCPtr(CSMatrix obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000065DC File Offset: 0x000047DC
		~CSMatrix()
		{
			this.Dispose();
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00006640 File Offset: 0x00004840
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
								CocoStudioEngineAdapterPINVOKE.delete_CSMatrix(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSMatrix(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00006738 File Offset: 0x00004938
		public CSMatrix() : this(CocoStudioEngineAdapterPINVOKE.new_CSMatrix(), true)
		{
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000674C File Offset: 0x0000494C
		public float M11()
		{
			return CocoStudioEngineAdapterPINVOKE.CSMatrix_M11(this.swigCPtr);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000676C File Offset: 0x0000496C
		public float M12()
		{
			return CocoStudioEngineAdapterPINVOKE.CSMatrix_M12(this.swigCPtr);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000678C File Offset: 0x0000498C
		public float M21()
		{
			return CocoStudioEngineAdapterPINVOKE.CSMatrix_M21(this.swigCPtr);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000067AC File Offset: 0x000049AC
		public float M22()
		{
			return CocoStudioEngineAdapterPINVOKE.CSMatrix_M22(this.swigCPtr);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000067CC File Offset: 0x000049CC
		public float X()
		{
			return CocoStudioEngineAdapterPINVOKE.CSMatrix_X(this.swigCPtr);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000067EC File Offset: 0x000049EC
		public float Y()
		{
			return CocoStudioEngineAdapterPINVOKE.CSMatrix_Y(this.swigCPtr);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000680B File Offset: 0x00004A0B
		public void SetM11(float v)
		{
			CocoStudioEngineAdapterPINVOKE.CSMatrix_SetM11(this.swigCPtr, v);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000681B File Offset: 0x00004A1B
		public void SetM12(float v)
		{
			CocoStudioEngineAdapterPINVOKE.CSMatrix_SetM12(this.swigCPtr, v);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000682B File Offset: 0x00004A2B
		public void SetM21(float v)
		{
			CocoStudioEngineAdapterPINVOKE.CSMatrix_SetM21(this.swigCPtr, v);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000683B File Offset: 0x00004A3B
		public void SetM22(float v)
		{
			CocoStudioEngineAdapterPINVOKE.CSMatrix_SetM22(this.swigCPtr, v);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000684B File Offset: 0x00004A4B
		public void SetX(float v)
		{
			CocoStudioEngineAdapterPINVOKE.CSMatrix_SetX(this.swigCPtr, v);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000685B File Offset: 0x00004A5B
		public void SetY(float v)
		{
			CocoStudioEngineAdapterPINVOKE.CSMatrix_SetY(this.swigCPtr, v);
		}

		// Token: 0x0400001C RID: 28
		private HandleRef swigCPtr;

		// Token: 0x0400001D RID: 29
		protected bool swigCMemOwn;
	}
}
