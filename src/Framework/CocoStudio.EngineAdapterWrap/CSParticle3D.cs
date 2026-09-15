using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSParticle3D : CSNode3D
	{
		public CSParticle3D(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSParticle3D_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSParticle3D obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSParticle3D()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSParticle3D(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSParticle3D(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSParticle3D() : this(CocoStudioEngineAdapterPINVOKE.new_CSParticle3D(), true)
		{
		}

		public void InitParticle3DSystem(string file)
		{
			CocoStudioEngineAdapterPINVOKE.CSParticle3D_InitParticle3DSystem(this.swigCPtr, file);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void StartParticleIfPossible()
		{
			CocoStudioEngineAdapterPINVOKE.CSParticle3D_StartParticleIfPossible(this.swigCPtr);
		}

		public override void SetPixelRenderMode(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSParticle3D_SetPixelRenderMode(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public override void RestoreRenderMode()
		{
			CocoStudioEngineAdapterPINVOKE.CSParticle3D_RestoreRenderMode(this.swigCPtr);
		}

		public override Color GetDisplayColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSParticle3D_GetDisplayColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		public void startParticle()
		{
			CocoStudioEngineAdapterPINVOKE.CSParticle3D_startParticle(this.swigCPtr);
		}

		public void stopParticle()
		{
			CocoStudioEngineAdapterPINVOKE.CSParticle3D_stopParticle(this.swigCPtr);
		}

		public bool isWorldPosition()
		{
			return CocoStudioEngineAdapterPINVOKE.CSParticle3D_isWorldPosition(this.swigCPtr);
		}

		private HandleRef swigCPtr;
	}
}
