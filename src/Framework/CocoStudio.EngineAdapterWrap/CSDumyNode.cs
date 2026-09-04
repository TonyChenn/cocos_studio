using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200000F RID: 15
	public class CSDumyNode : CSNode3D
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00004BDB File Offset: 0x00002DDB
		public CSDumyNode(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSDumyNode_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004BFC File Offset: 0x00002DFC
		public static HandleRef getCPtr(CSDumyNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004C28 File Offset: 0x00002E28
		~CSDumyNode()
		{
			this.Dispose();
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004C8C File Offset: 0x00002E8C
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

		// Token: 0x060000D2 RID: 210 RVA: 0x00004D8C File Offset: 0x00002F8C
		public CSDumyNode() : this(CocoStudioEngineAdapterPINVOKE.new_CSDumyNode(), true)
		{
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00004DA0 File Offset: 0x00002FA0
		public void EffectResult(ControlResult result)
		{
			CocoStudioEngineAdapterPINVOKE.CSDumyNode_EffectResult(this.swigCPtr, ControlResult.getCPtr(result));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004DD4 File Offset: 0x00002FD4
		public void EffectToTarget(CSVisualObject node, ControlResult result, bool center, bool global)
		{
			CocoStudioEngineAdapterPINVOKE.CSDumyNode_EffectToTarget(this.swigCPtr, CSVisualObject.getCPtr(node), ControlResult.getCPtr(result), center, global);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x04000015 RID: 21
		private HandleRef swigCPtr;
	}
}
