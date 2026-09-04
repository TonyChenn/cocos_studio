using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000053 RID: 83
	public class CSSlider : CSWidget
	{
		// Token: 0x06000A1D RID: 2589 RVA: 0x0000F4FB File Offset: 0x0000D6FB
		public CSSlider(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSlider_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0000F51C File Offset: 0x0000D71C
		public static HandleRef getCPtr(CSSlider obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0000F548 File Offset: 0x0000D748
		~CSSlider()
		{
			this.Dispose();
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0000F5AC File Offset: 0x0000D7AC
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
								CocoStudioEngineAdapterPINVOKE.delete_CSSlider(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSSlider(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0000F6AC File Offset: 0x0000D8AC
		public CSSlider() : this(CocoStudioEngineAdapterPINVOKE.new_CSSlider(), true)
		{
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0000F6C0 File Offset: 0x0000D8C0
		public virtual int GetPercent()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlider_GetPercent(this.swigCPtr);
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0000F6DF File Offset: 0x0000D8DF
		public virtual void SetPercent(int percent)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetPercent(this.swigCPtr, percent);
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0000F6F0 File Offset: 0x0000D8F0
		public override bool GetScale9Enabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlider_GetScale9Enabled(this.swigCPtr);
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0000F70F File Offset: 0x0000D90F
		public override void SetScale9Enabled(bool bEnaled)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetScale9Enabled(this.swigCPtr, bEnaled);
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0000F71F File Offset: 0x0000D91F
		public override void SetScale9Rect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetScale9Rect(this.swigCPtr, x, y, width, height);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0000F734 File Offset: 0x0000D934
		public virtual ResourceData GetGroundBarTexture()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlider_GetGroundBarTexture(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0000F774 File Offset: 0x0000D974
		public virtual void SetGroundBarTexture(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetGroundBarTexture(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0000F7BC File Offset: 0x0000D9BC
		public virtual ResourceData GetProgressBarTexture()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlider_GetProgressBarTexture(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0000F7FC File Offset: 0x0000D9FC
		public virtual void SetProgressBarTexture(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetProgressBarTexture(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0000F844 File Offset: 0x0000DA44
		public virtual ResourceData GetBallNormalTexture()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlider_GetBallNormalTexture(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0000F884 File Offset: 0x0000DA84
		public virtual void SetBallNormalTexture(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetBallNormalTexture(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0000F8CC File Offset: 0x0000DACC
		public virtual ResourceData GetBallPressedTexture()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlider_GetBallPressedTexture(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0000F90C File Offset: 0x0000DB0C
		public virtual void SetBallPressedTexture(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetBallPressedTexture(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0000F954 File Offset: 0x0000DB54
		public virtual ResourceData GetBallDisabledTexture()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlider_GetBallDisabledTexture(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0000F994 File Offset: 0x0000DB94
		public virtual void SetBallDisabledTexture(ResourceData file)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlider_SetBallDisabledTexture(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0000F9DC File Offset: 0x0000DBDC
		public override SizeF GetWidgetAutoSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlider_GetWidgetAutoSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new SizeF(size.width, size.height);
		}

		// Token: 0x04000097 RID: 151
		private HandleRef swigCPtr;
	}
}
