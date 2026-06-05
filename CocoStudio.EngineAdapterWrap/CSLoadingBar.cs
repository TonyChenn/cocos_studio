using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200003D RID: 61
	public class CSLoadingBar : CSWidget
	{
		// Token: 0x06000937 RID: 2359 RVA: 0x0000C172 File Offset: 0x0000A372
		public CSLoadingBar(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSLoadingBar_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x0000C194 File Offset: 0x0000A394
		public static HandleRef getCPtr(CSLoadingBar obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x0000C1C0 File Offset: 0x0000A3C0
		~CSLoadingBar()
		{
			this.Dispose();
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0000C224 File Offset: 0x0000A424
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
								CocoStudioEngineAdapterPINVOKE.delete_CSLoadingBar(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSLoadingBar(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0000C324 File Offset: 0x0000A524
		public CSLoadingBar() : this(CocoStudioEngineAdapterPINVOKE.new_CSLoadingBar(), true)
		{
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0000C338 File Offset: 0x0000A538
		public virtual int GetProgressPercent()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLoadingBar_GetProgressPercent(this.swigCPtr);
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0000C357 File Offset: 0x0000A557
		public virtual void SetProgressPercent(int iInfo)
		{
			CocoStudioEngineAdapterPINVOKE.CSLoadingBar_SetProgressPercent(this.swigCPtr, iInfo);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x0000C368 File Offset: 0x0000A568
		public virtual int GetProgressType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLoadingBar_GetProgressType(this.swigCPtr);
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0000C387 File Offset: 0x0000A587
		public virtual void SetProgressType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSLoadingBar_SetProgressType(this.swigCPtr, iType);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0000C398 File Offset: 0x0000A598
		public override bool GetScale9Enabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLoadingBar_GetScale9Enabled(this.swigCPtr);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0000C3B7 File Offset: 0x0000A5B7
		public override void SetScale9Enabled(bool bEnaled)
		{
			CocoStudioEngineAdapterPINVOKE.CSLoadingBar_SetScale9Enabled(this.swigCPtr, bEnaled);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0000C3C7 File Offset: 0x0000A5C7
		public override void SetScale9Rect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSLoadingBar_SetScale9Rect(this.swigCPtr, x, y, width, height);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0000C3DC File Offset: 0x0000A5DC
		public virtual ResourceData GetFileData()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSLoadingBar_GetFileData(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0000C41C File Offset: 0x0000A61C
		public virtual void SetFileData(ResourceData resourceData)
		{
			CocoStudioEngineAdapterPINVOKE.CSLoadingBar_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(resourceData.Path, (CSResourceData.CSEnumResourceType)resourceData.Type, resourceData.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x04000067 RID: 103
		private HandleRef swigCPtr;
	}
}
