using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class V3F_T2F_Quad : IDisposable
	{
		public V3F_T2F_Quad(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(V3F_T2F_Quad obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~V3F_T2F_Quad()
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
								CocoStudioEngineAdapterPINVOKE.delete_V3F_T2F_Quad(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_V3F_T2F_Quad(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public V3F_T2F bl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_bl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V3F_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_bl_set(this.swigCPtr, V3F_T2F.getCPtr(value));
			}
		}

		public V3F_T2F br
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_br_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V3F_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_br_set(this.swigCPtr, V3F_T2F.getCPtr(value));
			}
		}

		public V3F_T2F tl
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_tl_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V3F_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_tl_set(this.swigCPtr, V3F_T2F.getCPtr(value));
			}
		}

		public V3F_T2F tr
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_tr_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new V3F_T2F(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.V3F_T2F_Quad_tr_set(this.swigCPtr, V3F_T2F.getCPtr(value));
			}
		}

		public V3F_T2F_Quad() : this(CocoStudioEngineAdapterPINVOKE.new_V3F_T2F_Quad(), true)
		{
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}
