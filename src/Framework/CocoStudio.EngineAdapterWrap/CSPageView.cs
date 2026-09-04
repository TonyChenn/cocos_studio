using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200003E RID: 62
	public class CSPageView : CSPanel
	{
		// Token: 0x06000945 RID: 2373 RVA: 0x0000C464 File Offset: 0x0000A664
		public CSPageView(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSPageView_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0000C484 File Offset: 0x0000A684
		public static HandleRef getCPtr(CSPageView obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0000C4B0 File Offset: 0x0000A6B0
		~CSPageView()
		{
			this.Dispose();
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0000C514 File Offset: 0x0000A714
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
								CocoStudioEngineAdapterPINVOKE.delete_CSPageView(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSPageView(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0000C614 File Offset: 0x0000A814
		public CSPageView() : this(CocoStudioEngineAdapterPINVOKE.new_CSPageView(), true)
		{
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0000C625 File Offset: 0x0000A825
		public override void AddChild(CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSPageView_AddChild(this.swigCPtr, CSVisualObject.getCPtr(child));
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0000C63A File Offset: 0x0000A83A
		public override void RemoveChild(CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSPageView_RemoveChild(this.swigCPtr, CSVisualObject.getCPtr(child));
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0000C64F File Offset: 0x0000A84F
		public override void InsertChild(int index, CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSPageView_InsertChild(this.swigCPtr, index, CSVisualObject.getCPtr(child));
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0000C668 File Offset: 0x0000A868
		public override void SetSize(SizeF cPoint)
		{
			CocoStudioEngineAdapterPINVOKE.CSPageView_SetSize(this.swigCPtr, Size.getCPtr(new Size(cPoint.Width, cPoint.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x04000068 RID: 104
		private HandleRef swigCPtr;
	}
}
