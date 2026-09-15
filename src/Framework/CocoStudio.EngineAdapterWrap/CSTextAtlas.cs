using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSTextAtlas : CSWidget
	{
		public CSTextAtlas(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSTextAtlas_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSTextAtlas obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSTextAtlas()
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

		public CSTextAtlas() : this(CocoStudioEngineAdapterPINVOKE.new_CSTextAtlas(), true)
		{
		}

		public virtual void SetStartChar(string character)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextAtlas_SetStartChar(this.swigCPtr, character);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual string GetStartChar()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextAtlas_GetStartChar(this.swigCPtr);
		}

		public virtual int GetCharacterWidth()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextAtlas_GetCharacterWidth(this.swigCPtr);
		}

		public virtual void SetCharacterWidth(int width)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextAtlas_SetCharacterWidth(this.swigCPtr, width);
		}

		public virtual int GetCharacterHeight()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextAtlas_GetCharacterHeight(this.swigCPtr);
		}

		public virtual void SetCharacterHeight(int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextAtlas_SetCharacterHeight(this.swigCPtr, height);
		}

		public virtual string GetText()
		{
			return CocoStudioEngineAdapterPINVOKE.CSTextAtlas_GetText(this.swigCPtr);
		}

		public virtual void SetText(string strText)
		{
			CocoStudioEngineAdapterPINVOKE.CSTextAtlas_SetText(this.swigCPtr, strText);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ResourceData GetAtlasFile()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSTextAtlas_GetAtlasFile(this.swigCPtr);
			CSResourceData csresourceData = new CSResourceData(cPtr, true);
			return new ResourceData((EnumResourceType)csresourceData.GetResourceType(), csresourceData.GetPath(), csresourceData.GetPlistFile());
		}

		public virtual bool SetAtlasFile(ResourceData file)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSTextAtlas_SetAtlasFile(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(file.Path, (CSResourceData.CSEnumResourceType)file.Type, file.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		private HandleRef swigCPtr;
	}
}
