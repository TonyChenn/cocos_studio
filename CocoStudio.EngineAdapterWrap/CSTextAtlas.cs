using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000057 RID: 87
	public class CSTextAtlas : CSWidget
	{
		// Token: 0x06000A6F RID: 2671 RVA: 0x00010689 File Offset: 0x0000E889
		public CSTextAtlas(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSTextAtlas_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x000106A8 File Offset: 0x0000E8A8
		public static HandleRef getCPtr(CSTextAtlas obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x000106D4 File Offset: 0x0000E8D4
		~CSTextAtlas()
		{
			this.Dispose();
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00010738 File Offset: 0x0000E938
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
								CocoStudioEngineAdapterPINVOKE.delete_CSTextAtlas(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSTextAtlas(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00010838 File Offset: 0x0000EA38
		public CSTextAtlas() : this(CocoStudioEngineAdapterPINVOKE.new_CSTextAtlas(), true)
		{
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0001084C File Offset: 0x0000EA4C
		public virtual void SetStartChar(string character)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextAtlas_SetStartChar(this.swigCPtr, character);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0001087C File Offset: 0x0000EA7C
		public virtual string GetStartChar()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextAtlas_GetStartChar(this.swigCPtr);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0001089C File Offset: 0x0000EA9C
		public virtual int GetCharacterWidth()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextAtlas_GetCharacterWidth(this.swigCPtr);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x000108BB File Offset: 0x0000EABB
		public virtual void SetCharacterWidth(int width)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextAtlas_SetCharacterWidth(this.swigCPtr, width);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x000108CC File Offset: 0x0000EACC
		public virtual int GetCharacterHeight()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextAtlas_GetCharacterHeight(this.swigCPtr);
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x000108EB File Offset: 0x0000EAEB
		public virtual void SetCharacterHeight(int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextAtlas_SetCharacterHeight(this.swigCPtr, height);
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x000108FC File Offset: 0x0000EAFC
		public virtual string GetText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextAtlas_GetText(this.swigCPtr);
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0001091C File Offset: 0x0000EB1C
		public virtual void SetText(string strText)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextAtlas_SetText(this.swigCPtr, strText);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0001094C File Offset: 0x0000EB4C
		public virtual ResourceData GetAtlasFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSTextAtlas_GetAtlasFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0001098C File Offset: 0x0000EB8C
		public virtual bool SetAtlasFile(ResourceData file)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSTextAtlas_SetAtlasFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x0400009B RID: 155
		private HandleRef swigCPtr;
	}
}
