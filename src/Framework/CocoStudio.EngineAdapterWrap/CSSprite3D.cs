using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000055 RID: 85
	public class CSSprite3D : CSNode3D
	{
		// Token: 0x06000A3F RID: 2623 RVA: 0x0000FD81 File Offset: 0x0000DF81
		public CSSprite3D(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSprite3D_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0000FDA0 File Offset: 0x0000DFA0
		public static HandleRef getCPtr(CSSprite3D obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0000FDCC File Offset: 0x0000DFCC
		~CSSprite3D()
		{
			this.Dispose();
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0000FE30 File Offset: 0x0000E030
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
								CocoStudioEngineAdapterPINVOKE.delete_CSSprite3D(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSSprite3D(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0000FF30 File Offset: 0x0000E130
		public CSSprite3D() : this(CocoStudioEngineAdapterPINVOKE.new_CSSprite3D(), true)
		{
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0000FF44 File Offset: 0x0000E144
		public void SetFileData(ResourceData data)
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(data.Path, (CSResourceData.CSEnumResourceType)data.Type, data.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0000FF8C File Offset: 0x0000E18C
		public void SetLightMask(int mask)
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_SetLightMask(this.swigCPtr, mask);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0000FF9C File Offset: 0x0000E19C
		public int GetLightMask()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSprite3D_GetLightMask(this.swigCPtr);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0000FFBC File Offset: 0x0000E1BC
		public bool LoadAnimation()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSprite3D_LoadAnimation(this.swigCPtr);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0000FFDB File Offset: 0x0000E1DB
		public void RunAction()
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_RunAction(this.swigCPtr);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0000FFEA File Offset: 0x0000E1EA
		public void StopAction()
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_StopAction(this.swigCPtr);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0000FFF9 File Offset: 0x0000E1F9
		public void RunAnimationIfPosible()
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_RunAnimationIfPosible(this.swigCPtr);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00010008 File Offset: 0x0000E208
		public virtual void SetBlendFunc(BlendFuncValue blendFunc)
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_SetBlendFunc(this.swigCPtr, CSBlendFunc.getCPtr(new CSBlendFunc((uint)blendFunc.BlendSrc, (uint)blendFunc.BlendDst)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0001004C File Offset: 0x0000E24C
		public virtual BlendFuncValue GetBlendFunc()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSprite3D_GetBlendFunc(this.swigCPtr);
			CSBlendFunc csblendFunc = new CSBlendFunc(cPtr, true);
			return new BlendFuncValue((BlendSrc)csblendFunc.Src, (BlendDst)csblendFunc.Dst);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00010085 File Offset: 0x0000E285
		public override void RestoreRenderMode()
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_RestoreRenderMode(this.swigCPtr);
		}

		// Token: 0x04000099 RID: 153
		private HandleRef swigCPtr;
	}
}
