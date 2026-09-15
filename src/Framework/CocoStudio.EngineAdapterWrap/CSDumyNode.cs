using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSDumyNode : CSNode3D
	{
		public CSDumyNode(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSDumyNode_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSDumyNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSDumyNode()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSDumyNode(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSDumyNode(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSDumyNode() : this(CocoStudioEngineAdapterPINVOKE.new_CSDumyNode(), true)
		{
		}

		public void EffectResult(ControlResult result)
		{
			CocoStudioEngineAdapterPINVOKE.CSDumyNode_EffectResult(this.swigCPtr, ControlResult.getCPtr(result));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public void EffectToTarget(CSVisualObject node, ControlResult result, bool center, bool global)
		{
			CocoStudioEngineAdapterPINVOKE.CSDumyNode_EffectToTarget(this.swigCPtr, CSVisualObject.getCPtr(node), ControlResult.getCPtr(result), center, global);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private HandleRef swigCPtr;
	}
}
