using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000075 RID: 117
	public class CSSkeletonNode : CSBoneNode
	{
		// Token: 0x06000CAC RID: 3244 RVA: 0x00017AE3 File Offset: 0x00015CE3
		public CSSkeletonNode(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSkeletonNode_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00017B04 File Offset: 0x00015D04
		public static HandleRef getCPtr(CSSkeletonNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00017B30 File Offset: 0x00015D30
		~CSSkeletonNode()
		{
			this.Dispose();
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00017B94 File Offset: 0x00015D94
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

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00017C94 File Offset: 0x00015E94
		public CSSkeletonNode() : this(CocoStudioEngineAdapterPINVOKE.new_CSSkeletonNode(), true)
		{
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00017CA5 File Offset: 0x00015EA5
		public void ResetAllSubBoneScaledWidth()
		{
			CocoStudioEngineAdapterPINVOKE.CSSkeletonNode_ResetAllSubBoneScaledWidth(this.swigCPtr);
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00017CB4 File Offset: 0x00015EB4
		public override void SetLength(float length)
		{
			CocoStudioEngineAdapterPINVOKE.CSSkeletonNode_SetLength(this.swigCPtr, length);
		}

		// Token: 0x040000DC RID: 220
		private HandleRef swigCPtr;
	}
}
