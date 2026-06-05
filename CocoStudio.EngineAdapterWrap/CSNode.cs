using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200000D RID: 13
	public class CSNode : CSVisualObject
	{
		// Token: 0x060000AE RID: 174 RVA: 0x000044D9 File Offset: 0x000026D9
		public CSNode(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSNode_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000044F8 File Offset: 0x000026F8
		public static HandleRef getCPtr(CSNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004524 File Offset: 0x00002724
		~CSNode()
		{
			this.Dispose();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004588 File Offset: 0x00002788
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
								CocoStudioEngineAdapterPINVOKE.delete_CSNode(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSNode(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004688 File Offset: 0x00002888
		public CSNode() : this(CocoStudioEngineAdapterPINVOKE.new_CSNode(), true)
		{
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000469C File Offset: 0x0000289C
		public virtual void Init(bool useScript)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode_Init(this.swigCPtr, useScript);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000046CC File Offset: 0x000028CC
		public virtual void SetScriptFile(ScriptFileData scriptFile)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode_SetScriptFile(this.swigCPtr, CSScriptFileData.getCPtr(new CSScriptFileData(scriptFile.ScriptFile, (CSScriptFileData.CSScriptType)scriptFile.FileType)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x04000013 RID: 19
		private HandleRef swigCPtr;
	}
}
