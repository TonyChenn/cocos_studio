using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000038 RID: 56
	public class CSImageView : CSWidget
	{
		// Token: 0x060008EB RID: 2283 RVA: 0x0000AFD8 File Offset: 0x000091D8
		public CSImageView(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSImageView_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0000AFF8 File Offset: 0x000091F8
		public static HandleRef getCPtr(CSImageView obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0000B024 File Offset: 0x00009224
		~CSImageView()
		{
			this.Dispose();
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0000B088 File Offset: 0x00009288
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
								CocoStudioEngineAdapterPINVOKE.delete_CSImageView(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSImageView(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0000B188 File Offset: 0x00009388
		public CSImageView() : this(CocoStudioEngineAdapterPINVOKE.new_CSImageView(), true)
		{
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0000B19C File Offset: 0x0000939C
		public virtual bool GetFlipX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSImageView_GetFlipX(this.swigCPtr);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0000B1BB File Offset: 0x000093BB
		public virtual void SetFlipX(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSImageView_SetFlipX(this.swigCPtr, flip);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0000B1CC File Offset: 0x000093CC
		public virtual bool GetFlipY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSImageView_GetFlipY(this.swigCPtr);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0000B1EB File Offset: 0x000093EB
		public virtual void SetFlipY(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSImageView_SetFlipY(this.swigCPtr, flip);
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0000B1FC File Offset: 0x000093FC
		public override bool GetScale9Enabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSImageView_GetScale9Enabled(this.swigCPtr);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0000B21B File Offset: 0x0000941B
		public override void SetScale9Enabled(bool bEnaled)
		{
			CocoStudioEngineAdapterPINVOKE.CSImageView_SetScale9Enabled(this.swigCPtr, bEnaled);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0000B22B File Offset: 0x0000942B
		public override void SetScale9Rect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSImageView_SetScale9Rect(this.swigCPtr, x, y, width, height);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0000B240 File Offset: 0x00009440
		public virtual ResourceData GetFileData()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSImageView_GetFileData(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0000B280 File Offset: 0x00009480
		public virtual void SetFileData(ResourceData resourceData)
		{
			CocoStudioEngineAdapterPINVOKE.CSImageView_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(resourceData.Path, (CSResourceData.CSEnumResourceType)resourceData.Type, resourceData.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x04000062 RID: 98
		private HandleRef swigCPtr;
	}
}
