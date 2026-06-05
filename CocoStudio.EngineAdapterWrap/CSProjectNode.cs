using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000041 RID: 65
	public class CSProjectNode : CSNode2D
	{
		// Token: 0x06000967 RID: 2407 RVA: 0x0000CC8D File Offset: 0x0000AE8D
		public CSProjectNode(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSProjectNode_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0000CCAC File Offset: 0x0000AEAC
		public static HandleRef getCPtr(CSProjectNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0000CCD8 File Offset: 0x0000AED8
		~CSProjectNode()
		{
			this.Dispose();
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0000CD3C File Offset: 0x0000AF3C
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
								CocoStudioEngineAdapterPINVOKE.delete_CSProjectNode(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSProjectNode(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x0000CE3C File Offset: 0x0000B03C
		public CSProjectNode() : this(CocoStudioEngineAdapterPINVOKE.new_CSProjectNode(), true)
		{
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0000CE50 File Offset: 0x0000B050
		public virtual float GetWidth()
		{
			return CocoStudioEngineAdapterPINVOKE.CSProjectNode_GetWidth(this.swigCPtr);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0000CE70 File Offset: 0x0000B070
		public virtual float GetHeight()
		{
			return CocoStudioEngineAdapterPINVOKE.CSProjectNode_GetHeight(this.swigCPtr);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0000CE90 File Offset: 0x0000B090
		public virtual ResourceData GetFileData()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSProjectNode_GetFileData(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0000CED0 File Offset: 0x0000B0D0
		public virtual void SetFileData(ResourceData resourceData)
		{
			CocoStudioEngineAdapterPINVOKE.CSProjectNode_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(resourceData.Path, (CSResourceData.CSEnumResourceType)resourceData.Type, resourceData.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0000CF18 File Offset: 0x0000B118
		public virtual void SetProjectNode(CSVisualObject vObject)
		{
			CocoStudioEngineAdapterPINVOKE.CSProjectNode_SetProjectNode(this.swigCPtr, CSVisualObject.getCPtr(vObject));
		}

		// Token: 0x0400006B RID: 107
		private HandleRef swigCPtr;
	}
}
