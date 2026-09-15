using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class PointSprite : IDisposable
	{
		public PointSprite(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(PointSprite obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~PointSprite()
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
								CocoStudioEngineAdapterPINVOKE.delete_PointSprite(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_PointSprite(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public Vec2 pos
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.PointSprite_pos_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.PointSprite_pos_set(this.swigCPtr, Vec2.getCPtr(value));
			}
		}

		public Color4B color
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.PointSprite_color_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.PointSprite_color_set(this.swigCPtr, Color4B.getCPtr(value));
			}
		}

		public float size
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.PointSprite_size_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.PointSprite_size_set(this.swigCPtr, value);
			}
		}

		public PointSprite() : this(CocoStudioEngineAdapterPINVOKE.new_PointSprite(), true)
		{
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}
