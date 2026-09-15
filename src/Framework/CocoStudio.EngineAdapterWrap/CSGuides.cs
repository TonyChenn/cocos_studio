using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSGuides : CSObject
	{
		public CSGuides(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSGuides_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSGuides obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSGuides()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSGuides(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSGuides(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSGuides(LineDirection direction) : this(CocoStudioEngineAdapterPINVOKE.new_CSGuides((int)direction), true)
		{
		}

		public LineDirection GetDirection()
		{
			return (LineDirection)CocoStudioEngineAdapterPINVOKE.CSGuides_GetDirection(this.swigCPtr);
		}

		public void SetDirection(LineDirection direction)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuides_SetDirection(this.swigCPtr, (int)direction);
		}

		public float GetPosition()
		{
			return CocoStudioEngineAdapterPINVOKE.CSGuides_GetPosition(this.swigCPtr);
		}

		public void SetPosition(float position)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuides_SetPosition(this.swigCPtr, position);
		}

		private HandleRef swigCPtr;
	}
}
