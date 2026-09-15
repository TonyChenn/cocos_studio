using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSSkeletonNode : CSBoneNode
	{
		public CSSkeletonNode(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSkeletonNode_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSSkeletonNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSSkeletonNode()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSSkeletonNode(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSSkeletonNode(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSSkeletonNode() : this(CocoStudioEngineAdapterPINVOKE.new_CSSkeletonNode(), true)
		{
		}

		public void ResetAllSubBoneScaledWidth()
		{
			CocoStudioEngineAdapterPINVOKE.CSSkeletonNode_ResetAllSubBoneScaledWidth(this.swigCPtr);
		}

		public override void SetLength(float length)
		{
			CocoStudioEngineAdapterPINVOKE.CSSkeletonNode_SetLength(this.swigCPtr, length);
		}

		private HandleRef swigCPtr;
	}
}
