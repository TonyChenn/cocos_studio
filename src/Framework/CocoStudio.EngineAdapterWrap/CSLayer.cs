using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSLayer : CSNode2D
	{
		public CSLayer(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSLayer_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSLayer obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSLayer()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSLayer(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSLayer(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSLayer() : this(CocoStudioEngineAdapterPINVOKE.new_CSLayer(), true)
		{
		}

		public virtual bool IsTouchEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLayer_IsTouchEnabled(this.swigCPtr);
		}

		public virtual void SetTouchEnabled(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSLayer_SetTouchEnabled(this.swigCPtr, isEnabled);
		}

		private HandleRef swigCPtr;
	}
}
