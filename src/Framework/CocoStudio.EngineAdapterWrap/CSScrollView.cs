using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200003B RID: 59
	public class CSScrollView : CSPanel
	{
		// Token: 0x0600091C RID: 2332 RVA: 0x0000BAD2 File Offset: 0x00009CD2
		public CSScrollView(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSScrollView_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x0000BAF4 File Offset: 0x00009CF4
		public static HandleRef getCPtr(CSScrollView obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x0000BB20 File Offset: 0x00009D20
		~CSScrollView()
		{
			this.Dispose();
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x0000BB84 File Offset: 0x00009D84
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
								CocoStudioEngineAdapterPINVOKE.delete_CSScrollView(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSScrollView(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0000BC84 File Offset: 0x00009E84
		public CSScrollView() : this(CocoStudioEngineAdapterPINVOKE.new_CSScrollView(), true)
		{
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0000BC98 File Offset: 0x00009E98
		public virtual SizeF GetInnerSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSScrollView_GetInnerSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new SizeF(size.width, size.height);
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0000BD14 File Offset: 0x00009F14
		public virtual void SetInnerSize(SizeF size)
		{
			CocoStudioEngineAdapterPINVOKE.CSScrollView_SetInnerSize(this.swigCPtr, Size.getCPtr(new Size(size.Width, size.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0000BD58 File Offset: 0x00009F58
		public virtual int GetDirectionType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSScrollView_GetDirectionType(this.swigCPtr);
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0000BD77 File Offset: 0x00009F77
		public virtual void SetDirectionType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSScrollView_SetDirectionType(this.swigCPtr, iType);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0000BD88 File Offset: 0x00009F88
		public virtual bool GetBounceEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSScrollView_GetBounceEnabled(this.swigCPtr);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0000BDA7 File Offset: 0x00009FA7
		public virtual void SetBounceEnabled(bool enabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSScrollView_SetBounceEnabled(this.swigCPtr, enabled);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0000BDB8 File Offset: 0x00009FB8
		public virtual PointF TransformToSelfInner(PointF scenePoint)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSScrollView_TransformToSelfInner(this.swigCPtr, Vec2.getCPtr(new Vec2(scenePoint.X, scenePoint.Y)));
			Vec2 vec = new Vec2(cPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return new PointF(vec.x, vec.y);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0000BE20 File Offset: 0x0000A020
		public override RectF GetBoundingRect()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSScrollView_GetBoundingRect(this.swigCPtr);
			Rect rect = new Rect(cPtr, true);
			if (rect.size.width < 0f || rect.size.height < 0f)
			{
				rect.origin.x = 0f;
				rect.origin.y = 0f;
				rect.size.width = 0f;
				rect.size.height = 0f;
			}
			return new RectF(rect.origin.x, rect.origin.y, rect.size.width, rect.size.height);
		}

		// Token: 0x04000065 RID: 101
		private HandleRef swigCPtr;
	}
}
