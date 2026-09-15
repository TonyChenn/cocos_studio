using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class Quad3 : IDisposable
	{
		public Quad3(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(Quad3 obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~Quad3()
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
								CocoStudioEngineAdapterPINVOKE.delete_Quad3(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Quad3(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public Vec3 bl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad3_bl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad3_bl_set(this.swigCPtr, Vec3.getCPtr(value));
			}
		}

		public Vec3 br
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad3_br_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad3_br_set(this.swigCPtr, Vec3.getCPtr(value));
			}
		}

		public Vec3 tl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad3_tl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad3_tl_set(this.swigCPtr, Vec3.getCPtr(value));
			}
		}

		public Vec3 tr
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad3_tr_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec3(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad3_tr_set(this.swigCPtr, Vec3.getCPtr(value));
			}
		}

		public Quad3() : this(CocoStudioEngineAdapterPINVOKE.new_Quad3(), true)
		{
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}
