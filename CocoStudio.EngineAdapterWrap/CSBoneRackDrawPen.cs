using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000074 RID: 116
	public class CSBoneRackDrawPen : CSControlNodeDrawPen
	{
		// Token: 0x06000CA4 RID: 3236 RVA: 0x00017802 File Offset: 0x00015A02
		public CSBoneRackDrawPen(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSBoneRackDrawPen_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00017824 File Offset: 0x00015A24
		public static HandleRef getCPtr(CSBoneRackDrawPen obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x00017850 File Offset: 0x00015A50
		~CSBoneRackDrawPen()
		{
			this.Dispose();
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x000178B4 File Offset: 0x00015AB4
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
								CocoStudioEngineAdapterPINVOKE.delete_CSBoneRackDrawPen(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSBoneRackDrawPen(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x000179B4 File Offset: 0x00015BB4
		public void DrawBoneRack(PointF start, PointF end, float headWidth)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneRackDrawPen_DrawBoneRack(this.swigCPtr, Vec2.getCPtr(new Vec2(start.X, start.Y)), Vec2.getCPtr(new Vec2(end.X, end.Y)), headWidth);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x00017A14 File Offset: 0x00015C14
		public void DrawArrowLine(PointF start, PointF end)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneRackDrawPen_DrawArrowLine__SWIG_0(this.swigCPtr, Vec2.getCPtr(new Vec2(start.X, start.Y)), Vec2.getCPtr(new Vec2(end.X, end.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x00017A70 File Offset: 0x00015C70
		public void DrawArrowLine(PointF start, PointF end, Color4F color)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneRackDrawPen_DrawArrowLine__SWIG_1(this.swigCPtr, Vec2.getCPtr(new Vec2(start.X, start.Y)), Vec2.getCPtr(new Vec2(end.X, end.Y)), Color4F.getCPtr(color));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00017AD2 File Offset: 0x00015CD2
		public CSBoneRackDrawPen() : this(CocoStudioEngineAdapterPINVOKE.new_CSBoneRackDrawPen(), true)
		{
		}

		// Token: 0x040000DB RID: 219
		private HandleRef swigCPtr;
	}
}
