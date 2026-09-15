using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class Quad2 : IDisposable
	{
		public Quad2(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(Quad2 obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~Quad2()
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
								CocoStudioEngineAdapterPINVOKE.delete_Quad2(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_Quad2(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public Vec2 tl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad2_tl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad2_tl_set(this.swigCPtr, Vec2.getCPtr(value));
			}
		}

		public Vec2 tr
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad2_tr_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad2_tr_set(this.swigCPtr, Vec2.getCPtr(value));
			}
		}

		public Vec2 bl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad2_bl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad2_bl_set(this.swigCPtr, Vec2.getCPtr(value));
			}
		}

		public Vec2 br
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.Quad2_br_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.Quad2_br_set(this.swigCPtr, Vec2.getCPtr(value));
			}
		}

		public Quad2() : this(CocoStudioEngineAdapterPINVOKE.new_Quad2(), true)
		{
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}
