using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000046 RID: 70
	public class CSRotationDrawPen : CSControlNodeDrawPen
	{
		// Token: 0x06000997 RID: 2455 RVA: 0x0000D663 File Offset: 0x0000B863
		public CSRotationDrawPen(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSRotationDrawPen_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0000D684 File Offset: 0x0000B884
		public static HandleRef getCPtr(CSRotationDrawPen obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0000D6B0 File Offset: 0x0000B8B0
		~CSRotationDrawPen()
		{
			this.Dispose();
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0000D714 File Offset: 0x0000B914
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
								CocoStudioEngineAdapterPINVOKE.delete_CSRotationDrawPen(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSRotationDrawPen(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0000D814 File Offset: 0x0000BA14
		public CSRotationDrawPen() : this(CocoStudioEngineAdapterPINVOKE.new_CSRotationDrawPen(), true)
		{
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0000D825 File Offset: 0x0000BA25
		public override void DrawControl()
		{
			CocoStudioEngineAdapterPINVOKE.CSRotationDrawPen_DrawControl(this.swigCPtr);
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0000D834 File Offset: 0x0000BA34
		public override CSControlNodeDrawPen.OperateState PointAtControl(PointF point)
		{
			CSControlNodeDrawPen.OperateState result = (CSControlNodeDrawPen.OperateState)CocoStudioEngineAdapterPINVOKE.CSRotationDrawPen_PointAtControl(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x0400007C RID: 124
		private HandleRef swigCPtr;
	}
}
