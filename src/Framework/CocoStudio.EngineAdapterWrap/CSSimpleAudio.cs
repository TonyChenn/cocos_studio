using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSSimpleAudio : CSNode2D
	{
		public CSSimpleAudio(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSSimpleAudio obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSSimpleAudio()
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

		public CSSimpleAudio() : this(CocoStudioEngineAdapterPINVOKE.new_CSSimpleAudio(), true)
		{
		}

		public override string GetName()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_GetName(this.swigCPtr);
		}

		public override void SetName(string nameStr)
		{
			CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_SetName(this.swigCPtr, nameStr);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public float GetVolume()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_GetVolume(this.swigCPtr);
		}

		public void SetVolume(float volume)
		{
			CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_SetVolume(this.swigCPtr, volume);
		}

		public bool GetIsLoop()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_GetIsLoop(this.swigCPtr);
		}

		public void SetIsLoop(bool isLoop)
		{
			CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_SetIsLoop(this.swigCPtr, isLoop);
		}

		public void Start()
		{
			CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_Start(this.swigCPtr);
		}

		public void Stop()
		{
			CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_Stop(this.swigCPtr);
		}

		public ResourceData GetFileData()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_GetFileData(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public void SetFileData(ResourceData resourceData)
		{
			CocoStudioEngineAdapterPINVOKE.CSSimpleAudio_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(resourceData.Path, (CSResourceData.CSEnumResourceType)resourceData.Type, resourceData.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private HandleRef swigCPtr;
	}
}
