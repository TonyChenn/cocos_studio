using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000052 RID: 82
	public class CSSlice3D : CSNode3D
	{
		// Token: 0x06000A05 RID: 2565 RVA: 0x0000F011 File Offset: 0x0000D211
		public CSSlice3D(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSlice3D_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0000F030 File Offset: 0x0000D230
		public static HandleRef getCPtr(CSSlice3D obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0000F05C File Offset: 0x0000D25C
		~CSSlice3D()
		{
			this.Dispose();
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0000F0C0 File Offset: 0x0000D2C0
		public override void Dispose()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSSlice3D(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSSlice3D(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0000F1C0 File Offset: 0x0000D3C0
		public CSSlice3D() : this(CocoStudioEngineAdapterPINVOKE.new_CSSlice3D(), true)
		{
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0000F1D4 File Offset: 0x0000D3D4
		public void SetFileData(ResourceData data)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(data.Path, (CSResourceData.CSEnumResourceType)data.Type, data.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0000F21C File Offset: 0x0000D41C
		public SizeF GetSliceSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetSliceSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new SizeF(size.width, size.height);
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0000F298 File Offset: 0x0000D498
		public void SetSliceSize(SizeF size)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetSliceSize(this.swigCPtr, Size.getCPtr(new Size(size.Width, size.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0000F2DC File Offset: 0x0000D4DC
		public bool GetFlipX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetFlipX(this.swigCPtr);
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0000F2FB File Offset: 0x0000D4FB
		public void SetFlipX(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetFlipX(this.swigCPtr, flip);
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0000F30C File Offset: 0x0000D50C
		public bool GetFlipY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetFlipY(this.swigCPtr);
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0000F32B File Offset: 0x0000D52B
		public void SetFlipY(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetFlipY(this.swigCPtr, flip);
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0000F33C File Offset: 0x0000D53C
		public bool getUVActive()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlice3D_getUVActive(this.swigCPtr);
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0000F35B File Offset: 0x0000D55B
		public void setUVActive(bool active)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_setUVActive(this.swigCPtr, active);
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0000F36C File Offset: 0x0000D56C
		public void SetAnimationSpeed(PointF UV)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetAnimationSpeed(this.swigCPtr, Vec2.getCPtr(new Vec2(UV.X, UV.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0000F3B0 File Offset: 0x0000D5B0
		public PointF GetAnimationSpeed()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetAnimationSpeed(this.swigCPtr);
			Vec2 vec = new Vec2(cPtr, true);
			return new PointF(vec.x, vec.y);
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0000F3EC File Offset: 0x0000D5EC
		public PointF GetRowAndColumn()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetRowAndColumn(this.swigCPtr);
			Vec2 vec = new Vec2(cPtr, true);
			return new PointF(vec.x, vec.y);
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0000F428 File Offset: 0x0000D628
		public void SetRowAndColumn(PointF rowColumn)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetRowAndColumn(this.swigCPtr, Vec2.getCPtr(new Vec2(rowColumn.X, rowColumn.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0000F46C File Offset: 0x0000D66C
		public bool GetTextureActive()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetTextureActive(this.swigCPtr);
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0000F48B File Offset: 0x0000D68B
		public void SetTextureActive(bool active)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetTextureActive(this.swigCPtr, active);
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0000F49B File Offset: 0x0000D69B
		public void SetFramerate(float frame)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetFramerate(this.swigCPtr, frame);
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0000F4AC File Offset: 0x0000D6AC
		public float GetFramerate()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetFramerate(this.swigCPtr);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0000F4CB File Offset: 0x0000D6CB
		public void SetBillBoardMode(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetBillBoardMode(this.swigCPtr, iType);
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0000F4DC File Offset: 0x0000D6DC
		public int getBillBoardMode()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlice3D_getBillBoardMode(this.swigCPtr);
		}

		// Token: 0x04000096 RID: 150
		private HandleRef swigCPtr;
	}
}
