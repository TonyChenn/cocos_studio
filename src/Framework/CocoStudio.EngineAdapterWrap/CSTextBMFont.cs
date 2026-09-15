using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSTextBMFont : CSWidget
	{
		public CSTextBMFont(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSTextBMFont_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSTextBMFont obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSTextBMFont()
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

		public CSTextBMFont() : this(CocoStudioEngineAdapterPINVOKE.new_CSTextBMFont(), true)
		{
		}

		public virtual string GetText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextBMFont_GetText(this.swigCPtr);
		}

		public virtual void SetText(string sText)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextBMFont_SetText(this.swigCPtr, sText);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetFntFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSTextBMFont_GetFntFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual void SetFntFile(ResourceData fileName)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextBMFont_SetFntFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(fileName.Path, (CSResourceData.CSEnumResourceType)fileName.Type, fileName.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private HandleRef swigCPtr;
	}
}
