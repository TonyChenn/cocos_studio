using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000011 RID: 17
	public class CSGuidesService : CSObject
	{
		// Token: 0x060000DE RID: 222 RVA: 0x00005033 File Offset: 0x00003233
		public CSGuidesService(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSGuidesService_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005054 File Offset: 0x00003254
		public static HandleRef getCPtr(CSGuidesService obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005080 File Offset: 0x00003280
		~CSGuidesService()
		{
			this.Dispose();
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000050E4 File Offset: 0x000032E4
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
								CocoStudioEngineAdapterPINVOKE.delete_CSGuidesService(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSGuidesService(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000051E4 File Offset: 0x000033E4
		public static CSGuidesService GetInstance()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSGuidesService_GetInstance();
			return (intPtr == IntPtr.Zero) ? null : new CSGuidesService(intPtr, false);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005216 File Offset: 0x00003416
		public void SetVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuidesService_SetVisible(this.swigCPtr, visible);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005228 File Offset: 0x00003428
		public bool GetVisible()
		{
			return CocoStudioEngineAdapterPINVOKE.CSGuidesService_GetVisible(this.swigCPtr);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005247 File Offset: 0x00003447
		public void Add(CSGuides referenceLine)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuidesService_Add(this.swigCPtr, CSGuides.getCPtr(referenceLine));
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000525C File Offset: 0x0000345C
		public void Remove(CSGuides referenceLine)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuidesService_Remove(this.swigCPtr, CSGuides.getCPtr(referenceLine));
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005271 File Offset: 0x00003471
		public void Clear()
		{
			CocoStudioEngineAdapterPINVOKE.CSGuidesService_Clear(this.swigCPtr);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005280 File Offset: 0x00003480
		public CSGuides GetHoldGuides()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSGuidesService_GetHoldGuides(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSGuides(intPtr, false);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000052B8 File Offset: 0x000034B8
		public void SetHoldGuides(CSGuides csGuides)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuidesService_SetHoldGuides(this.swigCPtr, CSGuides.getCPtr(csGuides));
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000052D0 File Offset: 0x000034D0
		public void SetColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuidesService_SetColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x04000017 RID: 23
		private HandleRef swigCPtr;
	}
}
