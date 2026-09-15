using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSGuidesService : CSObject
	{
		public CSGuidesService(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSGuidesService_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSGuidesService obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSGuidesService()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSGuidesService(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSGuidesService(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public static CSGuidesService GetInstance()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSGuidesService_GetInstance();
			return (intPtr == IntPtr.Zero) ? null : new CSGuidesService(intPtr, false);
		}

		public void SetVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuidesService_SetVisible(this.swigCPtr, visible);
		}

		public bool GetVisible()
		{
			return CocoStudioEngineAdapterPINVOKE.CSGuidesService_GetVisible(this.swigCPtr);
		}

		public void Add(CSGuides referenceLine)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuidesService_Add(this.swigCPtr, CSGuides.getCPtr(referenceLine));
		}

		public void Remove(CSGuides referenceLine)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuidesService_Remove(this.swigCPtr, CSGuides.getCPtr(referenceLine));
		}

		public void Clear()
		{
			CocoStudioEngineAdapterPINVOKE.CSGuidesService_Clear(this.swigCPtr);
		}

		public CSGuides GetHoldGuides()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSGuidesService_GetHoldGuides(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSGuides(intPtr, false);
		}

		public void SetHoldGuides(CSGuides csGuides)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuidesService_SetHoldGuides(this.swigCPtr, CSGuides.getCPtr(csGuides));
		}

		public void SetColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuidesService_SetColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private HandleRef swigCPtr;
	}
}
