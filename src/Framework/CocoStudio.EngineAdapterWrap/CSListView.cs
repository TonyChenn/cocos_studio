using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200003C RID: 60
	public class CSListView : CSScrollView
	{
		// Token: 0x06000929 RID: 2345 RVA: 0x0000BEF0 File Offset: 0x0000A0F0
		public CSListView(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSListView_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0000BF10 File Offset: 0x0000A110
		public static HandleRef getCPtr(CSListView obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0000BF3C File Offset: 0x0000A13C
		~CSListView()
		{
			this.Dispose();
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0000BFA0 File Offset: 0x0000A1A0
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
								CocoStudioEngineAdapterPINVOKE.delete_CSListView(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSListView(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0000C0A0 File Offset: 0x0000A2A0
		public CSListView() : this(CocoStudioEngineAdapterPINVOKE.new_CSListView(), true)
		{
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x0000C0B4 File Offset: 0x0000A2B4
		public virtual int GetItemSpace()
		{
			return CocoStudioEngineAdapterPINVOKE.CSListView_GetItemSpace(this.swigCPtr);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0000C0D3 File Offset: 0x0000A2D3
		public virtual void SetItemSpace(int space)
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_SetItemSpace(this.swigCPtr, space);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0000C0E4 File Offset: 0x0000A2E4
		public virtual int GetGravityType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSListView_GetGravityType(this.swigCPtr);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0000C103 File Offset: 0x0000A303
		public virtual void SetGravityType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_SetGravityType(this.swigCPtr, iType);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0000C113 File Offset: 0x0000A313
		public override void InsertChild(int index, CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_InsertChild(this.swigCPtr, index, CSVisualObject.getCPtr(child));
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0000C129 File Offset: 0x0000A329
		public override void AddChild(CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_AddChild(this.swigCPtr, CSVisualObject.getCPtr(child));
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0000C13E File Offset: 0x0000A33E
		public override void RemoveChild(CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_RemoveChild(this.swigCPtr, CSVisualObject.getCPtr(child));
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0000C153 File Offset: 0x0000A353
		public override void SetDirectionType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_SetDirectionType(this.swigCPtr, iType);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0000C163 File Offset: 0x0000A363
		public virtual void RefreshInnerLayout()
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_RefreshInnerLayout(this.swigCPtr);
		}

		// Token: 0x04000066 RID: 102
		private HandleRef swigCPtr;
	}
}
