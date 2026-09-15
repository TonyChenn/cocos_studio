using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class T2F_Quad : IDisposable
	{
		public T2F_Quad(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(T2F_Quad obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~T2F_Quad()
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
								CocoStudioEngineAdapterPINVOKE.delete_T2F_Quad(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_T2F_Quad(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public Tex2F bl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.T2F_Quad_bl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Tex2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.T2F_Quad_bl_set(this.swigCPtr, Tex2F.getCPtr(value));
			}
		}

		public Tex2F br
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.T2F_Quad_br_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Tex2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.T2F_Quad_br_set(this.swigCPtr, Tex2F.getCPtr(value));
			}
		}

		public Tex2F tl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.T2F_Quad_tl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Tex2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.T2F_Quad_tl_set(this.swigCPtr, Tex2F.getCPtr(value));
			}
		}

		public Tex2F tr
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.T2F_Quad_tr_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Tex2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.T2F_Quad_tr_set(this.swigCPtr, Tex2F.getCPtr(value));
			}
		}

		public T2F_Quad() : this(CocoStudioEngineAdapterPINVOKE.new_T2F_Quad(), true)
		{
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}
