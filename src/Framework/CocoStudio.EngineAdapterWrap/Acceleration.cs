using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class Acceleration : IDisposable
	{
		public Acceleration(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(Acceleration obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~Acceleration()
		{
			this.Dispose();
		}

		public virtual void Dispose()
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
								CocoStudioEngineAdapterPINVOKE.delete_Acceleration(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Acceleration(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public double x
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Acceleration_x_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Acceleration_x_set(this.swigCPtr, value);
			}
		}

		public double y
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Acceleration_y_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Acceleration_y_set(this.swigCPtr, value);
			}
		}

		public double z
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Acceleration_z_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Acceleration_z_set(this.swigCPtr, value);
			}
		}

		public double timestamp
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.Acceleration_timestamp_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Acceleration_timestamp_set(this.swigCPtr, value);
			}
		}

		public Acceleration() : this(CocoStudioEngineAdapterPINVOKE.new_Acceleration(), true)
		{
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}
