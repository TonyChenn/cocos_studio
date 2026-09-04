using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000040 RID: 64
	public class CSParticleSystem : CSNode2D
	{
		// Token: 0x0600095B RID: 2395 RVA: 0x0000C987 File Offset: 0x0000AB87
		public CSParticleSystem(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSParticleSystem_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0000C9A8 File Offset: 0x0000ABA8
		public static HandleRef getCPtr(CSParticleSystem obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0000C9D4 File Offset: 0x0000ABD4
		~CSParticleSystem()
		{
			this.Dispose();
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0000CA38 File Offset: 0x0000AC38
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
								CocoStudioEngineAdapterPINVOKE.delete_CSParticleSystem(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSParticleSystem(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0000CB38 File Offset: 0x0000AD38
		public CSParticleSystem() : this(CocoStudioEngineAdapterPINVOKE.new_CSParticleSystem(), true)
		{
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0000CB49 File Offset: 0x0000AD49
		public void Start()
		{
			CocoStudioEngineAdapterPINVOKE.CSParticleSystem_Start(this.swigCPtr);
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0000CB58 File Offset: 0x0000AD58
		public void Stop()
		{
			CocoStudioEngineAdapterPINVOKE.CSParticleSystem_Stop(this.swigCPtr);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0000CB68 File Offset: 0x0000AD68
		public bool IsPlaying()
		{
			return CocoStudioEngineAdapterPINVOKE.CSParticleSystem_IsPlaying(this.swigCPtr);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0000CB88 File Offset: 0x0000AD88
		public ResourceData GetFileData()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSParticleSystem_GetFileData(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0000CBC8 File Offset: 0x0000ADC8
		public void SetFileData(ResourceData resourceData)
		{
			CocoStudioEngineAdapterPINVOKE.CSParticleSystem_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(resourceData.Path, (CSResourceData.CSEnumResourceType)resourceData.Type, resourceData.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0000CC10 File Offset: 0x0000AE10
		public virtual void SetBlendFunc(BlendFuncValue blendFunc)
		{
			CocoStudioEngineAdapterPINVOKE.CSParticleSystem_SetBlendFunc(this.swigCPtr, CSBlendFunc.getCPtr(new CSBlendFunc((uint)blendFunc.BlendSrc, (uint)blendFunc.BlendDst)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0000CC54 File Offset: 0x0000AE54
		public virtual BlendFuncValue GetBlendFunc()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSParticleSystem_GetBlendFunc(this.swigCPtr);
			CSBlendFunc csblendFunc = new CSBlendFunc(cPtr, true);
			return new BlendFuncValue((BlendSrc)csblendFunc.Src, (BlendDst)csblendFunc.Dst);
		}

		// Token: 0x0400006A RID: 106
		private HandleRef swigCPtr;
	}
}
