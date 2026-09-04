using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000054 RID: 84
	public class CSSprite : CSNode2D
	{
		// Token: 0x06000A32 RID: 2610 RVA: 0x0000FA56 File Offset: 0x0000DC56
		public CSSprite(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSprite_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0000FA78 File Offset: 0x0000DC78
		public static HandleRef getCPtr(CSSprite obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0000FAA4 File Offset: 0x0000DCA4
		~CSSprite()
		{
			this.Dispose();
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0000FB08 File Offset: 0x0000DD08
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
								CocoStudioEngineAdapterPINVOKE.delete_CSSprite(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSSprite(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0000FC08 File Offset: 0x0000DE08
		public CSSprite() : this(CocoStudioEngineAdapterPINVOKE.new_CSSprite(), true)
		{
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0000FC1C File Offset: 0x0000DE1C
		public bool GetFlipX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSprite_GetFlipX(this.swigCPtr);
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x0000FC3B File Offset: 0x0000DE3B
		public void SetFlipX(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite_SetFlipX(this.swigCPtr, flip);
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0000FC4C File Offset: 0x0000DE4C
		public bool GetFlipY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSprite_GetFlipY(this.swigCPtr);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0000FC6B File Offset: 0x0000DE6B
		public void SetFlipY(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite_SetFlipY(this.swigCPtr, flip);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0000FC7C File Offset: 0x0000DE7C
		public ResourceData GetFileData()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSprite_GetFileData(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0000FCBC File Offset: 0x0000DEBC
		public void SetFileData(ResourceData resourceData)
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(resourceData.Path, (CSResourceData.CSEnumResourceType)resourceData.Type, resourceData.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0000FD04 File Offset: 0x0000DF04
		public virtual void SetBlendFunc(BlendFuncValue blendFunc)
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite_SetBlendFunc(this.swigCPtr, CSBlendFunc.getCPtr(new CSBlendFunc((uint)blendFunc.BlendSrc, (uint)blendFunc.BlendDst)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0000FD48 File Offset: 0x0000DF48
		public virtual BlendFuncValue GetBlendFunc()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSprite_GetBlendFunc(this.swigCPtr);
			CSBlendFunc csblendFunc = new CSBlendFunc(cPtr, true);
			return new BlendFuncValue((BlendSrc)csblendFunc.Src, (BlendDst)csblendFunc.Dst);
		}

		// Token: 0x04000098 RID: 152
		private HandleRef swigCPtr;
	}
}
