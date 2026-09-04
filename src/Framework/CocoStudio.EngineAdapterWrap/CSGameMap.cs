using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000037 RID: 55
	public class CSGameMap : CSNode2D
	{
		// Token: 0x060008E3 RID: 2275 RVA: 0x0000AD7F File Offset: 0x00008F7F
		public CSGameMap(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSGameMap_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0000ADA0 File Offset: 0x00008FA0
		public static HandleRef getCPtr(CSGameMap obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0000ADCC File Offset: 0x00008FCC
		~CSGameMap()
		{
			this.Dispose();
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0000AE30 File Offset: 0x00009030
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
								CocoStudioEngineAdapterPINVOKE.delete_CSGameMap(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSGameMap(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0000AF30 File Offset: 0x00009130
		public CSGameMap() : this(CocoStudioEngineAdapterPINVOKE.new_CSGameMap(), true)
		{
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0000AF44 File Offset: 0x00009144
		public ResourceData GetFileData()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSGameMap_GetFileData(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0000AF84 File Offset: 0x00009184
		public void SetFileData(ResourceData resourceData)
		{
			CocoStudioEngineAdapterPINVOKE.CSGameMap_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(resourceData.Path, (CSResourceData.CSEnumResourceType)resourceData.Type, resourceData.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x04000060 RID: 96
		private HandleRef swigCPtr;

		// Token: 0x04000061 RID: 97
		public static readonly int TMXLayerTag = CocoStudioEngineAdapterPINVOKE.CSGameMap_TMXLayerTag_get();
	}
}
