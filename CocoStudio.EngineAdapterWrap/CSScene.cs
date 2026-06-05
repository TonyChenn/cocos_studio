using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200004A RID: 74
	public class CSScene : CSVisualObject
	{
		// Token: 0x060009BC RID: 2492 RVA: 0x0000DFBC File Offset: 0x0000C1BC
		public CSScene(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSScene_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0000DFDC File Offset: 0x0000C1DC
		public static HandleRef getCPtr(CSScene obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0000E008 File Offset: 0x0000C208
		~CSScene()
		{
			this.Dispose();
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0000E06C File Offset: 0x0000C26C
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
								CocoStudioEngineAdapterPINVOKE.delete_CSScene(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSScene(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0000E16C File Offset: 0x0000C36C
		public void ChangeMode(bool b2D)
		{
			CocoStudioEngineAdapterPINVOKE.CSScene_ChangeMode(this.swigCPtr, b2D);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0000E17C File Offset: 0x0000C37C
		public CSSceneCamera GetCamera()
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSScene_GetCamera(this.swigCPtr);
			return (intPtr == IntPtr.Zero) ? null : new CSSceneCamera(intPtr, false);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0000E1B4 File Offset: 0x0000C3B4
		public void OnViewSizeChange(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSScene_OnViewSizeChange(this.swigCPtr, x, y, width, height);
		}

		// Token: 0x04000081 RID: 129
		private HandleRef swigCPtr;
	}
}
