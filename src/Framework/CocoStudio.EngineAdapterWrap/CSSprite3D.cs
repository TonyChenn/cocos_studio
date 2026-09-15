using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSSprite3D : CSNode3D
	{
		public CSSprite3D(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSprite3D_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSSprite3D obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSSprite3D()
		{
			this.Dispose();
		}

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

		public CSSprite3D() : this(CocoStudioEngineAdapterPINVOKE.new_CSSprite3D(), true)
		{
		}

		public void SetFileData(ResourceData data)
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(data.Path, (CSResourceData.CSEnumResourceType)data.Type, data.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void SetLightMask(int mask)
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_SetLightMask(this.swigCPtr, mask);
		}

		public int GetLightMask()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSprite3D_GetLightMask(this.swigCPtr);
		}

		public bool LoadAnimation()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSprite3D_LoadAnimation(this.swigCPtr);
		}

		public void RunAction()
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_RunAction(this.swigCPtr);
		}

		public void StopAction()
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_StopAction(this.swigCPtr);
		}

		public void RunAnimationIfPosible()
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_RunAnimationIfPosible(this.swigCPtr);
		}

		public virtual void SetBlendFunc(BlendFuncValue blendFunc)
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_SetBlendFunc(this.swigCPtr, CSBlendFunc.getCPtr(new CSBlendFunc((uint)blendFunc.BlendSrc, (uint)blendFunc.BlendDst)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual BlendFuncValue GetBlendFunc()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSprite3D_GetBlendFunc(this.swigCPtr);
			CSBlendFunc csblendFunc = new CSBlendFunc(cPtr, true);
			return new BlendFuncValue((BlendSrc)csblendFunc.Src, (BlendDst)csblendFunc.Dst);
		}

		public override void RestoreRenderMode()
		{
			CocoStudioEngineAdapterPINVOKE.CSSprite3D_RestoreRenderMode(this.swigCPtr);
		}

		private HandleRef swigCPtr;
	}
}
