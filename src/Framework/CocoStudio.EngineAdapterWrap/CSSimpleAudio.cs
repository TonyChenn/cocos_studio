using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000050 RID: 80
	public class CSSimpleAudio : CSNode2D
	{
		// Token: 0x060009ED RID: 2541 RVA: 0x0000EA13 File Offset: 0x0000CC13
		public CSSimpleAudio(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0000EA34 File Offset: 0x0000CC34
		public static HandleRef getCPtr(CSSimpleAudio obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0000EA60 File Offset: 0x0000CC60
		~CSSimpleAudio()
		{
			this.Dispose();
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0000EAC4 File Offset: 0x0000CCC4
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
								CocoStudioEngineAdapterPINVOKE.delete_CSSimpleAudio(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSSimpleAudio(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0000EBC4 File Offset: 0x0000CDC4
		public CSSimpleAudio() : this(CocoStudioEngineAdapterPINVOKE.new_CSSimpleAudio(), true)
		{
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0000EBD8 File Offset: 0x0000CDD8
		public override string GetName()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_GetName(this.swigCPtr);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0000EBF8 File Offset: 0x0000CDF8
		public override void SetName(string nameStr)
		{
			CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_SetName(this.swigCPtr, nameStr);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0000EC28 File Offset: 0x0000CE28
		public float GetVolume()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_GetVolume(this.swigCPtr);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0000EC47 File Offset: 0x0000CE47
		public void SetVolume(float volume)
		{
			CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_SetVolume(this.swigCPtr, volume);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0000EC58 File Offset: 0x0000CE58
		public bool GetIsLoop()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_GetIsLoop(this.swigCPtr);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0000EC77 File Offset: 0x0000CE77
		public void SetIsLoop(bool isLoop)
		{
			CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_SetIsLoop(this.swigCPtr, isLoop);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0000EC87 File Offset: 0x0000CE87
		public void Start()
		{
			CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_Start(this.swigCPtr);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0000EC96 File Offset: 0x0000CE96
		public void Stop()
		{
			CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_Stop(this.swigCPtr);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0000ECA8 File Offset: 0x0000CEA8
		public ResourceData GetFileData()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_GetFileData(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0000ECE8 File Offset: 0x0000CEE8
		public void SetFileData(ResourceData resourceData)
		{
			CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(resourceData.Path, (CSResourceData.CSEnumResourceType)resourceData.Type, resourceData.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x04000094 RID: 148
		private HandleRef swigCPtr;
	}
}
