using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000051 RID: 81
	public class CSSkyBox : CSObject
	{
		// Token: 0x060009FC RID: 2556 RVA: 0x0000ED30 File Offset: 0x0000CF30
		public CSSkyBox(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSkyBox_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0000ED50 File Offset: 0x0000CF50
		public static HandleRef getCPtr(CSSkyBox obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0000ED7C File Offset: 0x0000CF7C
		~CSSkyBox()
		{
			this.Dispose();
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0000EDE0 File Offset: 0x0000CFE0
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
								CocoStudioEngineAdapterPINVOKE.delete_CSSkyBox(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSSkyBox(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0000EEE0 File Offset: 0x0000D0E0
		public CSSkyBox() : this(CocoStudioEngineAdapterPINVOKE.new_CSSkyBox(), true)
		{
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0000EEF4 File Offset: 0x0000D0F4
		public bool IsEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSkyBox_IsEnabled(this.swigCPtr);
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0000EF13 File Offset: 0x0000D113
		public void SetEnabled(bool enable)
		{
			CocoStudioEngineAdapterPINVOKE.CSSkyBox_SetEnabled(this.swigCPtr, enable);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0000EF23 File Offset: 0x0000D123
		public void ResetSkyBox()
		{
			CocoStudioEngineAdapterPINVOKE.CSSkyBox_ResetSkyBox(this.swigCPtr);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0000EF34 File Offset: 0x0000D134
		public void RefreshSkyBox(ResourceData leftfile, ResourceData rightfile, ResourceData topfile, ResourceData bottomfile, ResourceData forwordfile, ResourceData backfile)
		{
			CocoStudioEngineAdapterPINVOKE.CSSkyBox_RefreshSkyBox(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(leftfile.Path, (CSResourceData.CSEnumResourceType)leftfile.Type, leftfile.Plist)), CSResourceData.getCPtr(new CSResourceData(rightfile.Path, (CSResourceData.CSEnumResourceType)rightfile.Type, rightfile.Plist)), CSResourceData.getCPtr(new CSResourceData(topfile.Path, (CSResourceData.CSEnumResourceType)topfile.Type, topfile.Plist)), CSResourceData.getCPtr(new CSResourceData(bottomfile.Path, (CSResourceData.CSEnumResourceType)bottomfile.Type, bottomfile.Plist)), CSResourceData.getCPtr(new CSResourceData(forwordfile.Path, (CSResourceData.CSEnumResourceType)forwordfile.Type, forwordfile.Plist)), CSResourceData.getCPtr(new CSResourceData(backfile.Path, (CSResourceData.CSEnumResourceType)backfile.Type, backfile.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x04000095 RID: 149
		private HandleRef swigCPtr;
	}
}
