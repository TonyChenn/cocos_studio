using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000032 RID: 50
	public class CSCanvas : CSVisualObject
	{
		// Token: 0x06000897 RID: 2199 RVA: 0x00009ACC File Offset: 0x00007CCC
		public CSCanvas(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSCanvas_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00009AEC File Offset: 0x00007CEC
		public static HandleRef getCPtr(CSCanvas obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00009B18 File Offset: 0x00007D18
		~CSCanvas()
		{
			this.Dispose();
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00009B7C File Offset: 0x00007D7C
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
								CocoStudioEngineAdapterPINVOKE.delete_CSCanvas(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSCanvas(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00009C7C File Offset: 0x00007E7C
		public CSCanvas() : this(CocoStudioEngineAdapterPINVOKE.new_CSCanvas(), true)
		{
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00009C8D File Offset: 0x00007E8D
		public override void SetVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetVisible(this.swigCPtr, visible);
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00009CA0 File Offset: 0x00007EA0
		public override void SetPosition(PointF position)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetPosition(this.swigCPtr, Vec2.getCPtr(new Vec2(position.X, position.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00009CE4 File Offset: 0x00007EE4
		public override void SetScale(ScaleValue scale)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetScale(this.swigCPtr, CSScale.getCPtr(new CSScale(scale.ScaleX, scale.ScaleY)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00009D28 File Offset: 0x00007F28
		public override int HitTest(PointF point)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSCanvas_HitTest(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00009D74 File Offset: 0x00007F74
		public override SizeF GetSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSCanvas_GetSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new SizeF(size.width, size.height);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00009DF0 File Offset: 0x00007FF0
		public override void SetSize(SizeF cSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetSize(this.swigCPtr, Size.getCPtr(new Size(cSize.Width, cSize.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00009E32 File Offset: 0x00008032
		public virtual void ResetCanvas()
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_ResetCanvas(this.swigCPtr);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00009E41 File Offset: 0x00008041
		public void SetLayerColorVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetLayerColorVisible(this.swigCPtr, visible);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00009E51 File Offset: 0x00008051
		public void SetCenterLineVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetCenterLineVisible(this.swigCPtr, visible);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00009E61 File Offset: 0x00008061
		public void SetBackgroundVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetBackgroundVisible(this.swigCPtr, visible);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00009E71 File Offset: 0x00008071
		public virtual void SetGridVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetGridVisible(this.swigCPtr, visible);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00009E81 File Offset: 0x00008081
		public virtual void RefreshGrid(float y)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_RefreshGrid(this.swigCPtr, y);
		}

		// Token: 0x04000055 RID: 85
		private HandleRef swigCPtr;
	}
}
