using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000058 RID: 88
	public class CSTextBMFont : CSWidget
	{
		// Token: 0x06000A7E RID: 2686 RVA: 0x000109D9 File Offset: 0x0000EBD9
		public CSTextBMFont(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSTextBMFont_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x000109F8 File Offset: 0x0000EBF8
		public static HandleRef getCPtr(CSTextBMFont obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00010A24 File Offset: 0x0000EC24
		~CSTextBMFont()
		{
			this.Dispose();
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00010A88 File Offset: 0x0000EC88
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
								CocoStudioEngineAdapterPINVOKE.delete_CSTextBMFont(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSTextBMFont(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00010B88 File Offset: 0x0000ED88
		public CSTextBMFont() : this(CocoStudioEngineAdapterPINVOKE.new_CSTextBMFont(), true)
		{
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00010B9C File Offset: 0x0000ED9C
		public virtual string GetText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextBMFont_GetText(this.swigCPtr);
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00010BBC File Offset: 0x0000EDBC
		public virtual void SetText(string sText)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextBMFont_SetText(this.swigCPtr, sText);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x00010BEC File Offset: 0x0000EDEC
		public virtual ResourceData GetFntFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSTextBMFont_GetFntFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00010C2C File Offset: 0x0000EE2C
		public virtual void SetFntFile(ResourceData fileName)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextBMFont_SetFntFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(fileName.Path, (CSResourceData.CSEnumResourceType)fileName.Type, fileName.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0400009C RID: 156
		private HandleRef swigCPtr;
	}
}
