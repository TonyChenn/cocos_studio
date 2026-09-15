using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSCanvas : CSVisualObject
	{
		public CSCanvas(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSCanvas_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSCanvas obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSCanvas()
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

		public CSCanvas() : this(CocoStudioEngineAdapterPINVOKE.new_CSCanvas(), true)
		{
		}

		public override void SetVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetVisible(this.swigCPtr, visible);
		}

		public override void SetPosition(PointF position)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetPosition(this.swigCPtr, Vec2.getCPtr(new Vec2(position.X, position.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public override void SetScale(ScaleValue scale)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetScale(this.swigCPtr, CSScale.getCPtr(new CSScale(scale.ScaleX, scale.ScaleY)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public override int HitTest(PointF point)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSCanvas_HitTest(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

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

		public override void SetSize(SizeF cSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetSize(this.swigCPtr, Size.getCPtr(new Size(cSize.Width, cSize.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void ResetCanvas()
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_ResetCanvas(this.swigCPtr);
		}

		public void SetLayerColorVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetLayerColorVisible(this.swigCPtr, visible);
		}

		public void SetCenterLineVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetCenterLineVisible(this.swigCPtr, visible);
		}

		public void SetBackgroundVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetBackgroundVisible(this.swigCPtr, visible);
		}

		public virtual void SetGridVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_SetGridVisible(this.swigCPtr, visible);
		}

		public virtual void RefreshGrid(float y)
		{
			CocoStudioEngineAdapterPINVOKE.CSCanvas_RefreshGrid(this.swigCPtr, y);
		}

		private HandleRef swigCPtr;
	}
}
