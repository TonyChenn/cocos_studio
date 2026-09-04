using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200002E RID: 46
	public class CSWidget : CSNode2D
	{
		// Token: 0x0600083E RID: 2110 RVA: 0x00008869 File Offset: 0x00006A69
		public CSWidget(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSWidget_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00008888 File Offset: 0x00006A88
		public static HandleRef getCPtr(CSWidget obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x000088B4 File Offset: 0x00006AB4
		~CSWidget()
		{
			this.Dispose();
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00008918 File Offset: 0x00006B18
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
								CocoStudioEngineAdapterPINVOKE.delete_CSWidget(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSWidget(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00008A18 File Offset: 0x00006C18
		public CSWidget() : this(CocoStudioEngineAdapterPINVOKE.new_CSWidget(), true)
		{
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00008A2C File Offset: 0x00006C2C
		public virtual bool GetCustomSizeEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetCustomSizeEnabled(this.swigCPtr);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00008A4B File Offset: 0x00006C4B
		public virtual void SetCustomSizeEnabled(bool bEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetCustomSizeEnabled(this.swigCPtr, bEnabled);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00008A5C File Offset: 0x00006C5C
		public virtual SizeF GetWidgetAutoSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSWidget_GetWidgetAutoSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new SizeF(size.width, size.height);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00008AD8 File Offset: 0x00006CD8
		public virtual bool GetTouchEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetTouchEnabled(this.swigCPtr);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00008AF7 File Offset: 0x00006CF7
		public virtual void SetTouchEnabled(bool select)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetTouchEnabled(this.swigCPtr, select);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00008B08 File Offset: 0x00006D08
		public virtual int GetScale9Left()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Left(this.swigCPtr);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00008B27 File Offset: 0x00006D27
		public virtual void SetScale9Left(int left)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetScale9Left(this.swigCPtr, left);
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00008B38 File Offset: 0x00006D38
		public virtual int GetScale9Right()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Right(this.swigCPtr);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00008B57 File Offset: 0x00006D57
		public virtual void SetScale9Right(int right)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetScale9Right(this.swigCPtr, right);
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00008B68 File Offset: 0x00006D68
		public virtual int GetScale9Top()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Top(this.swigCPtr);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00008B87 File Offset: 0x00006D87
		public virtual void SetScale9Top(int top)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetScale9Top(this.swigCPtr, top);
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x00008B98 File Offset: 0x00006D98
		public virtual int GetScale9Bottom()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Bottom(this.swigCPtr);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00008BB7 File Offset: 0x00006DB7
		public virtual void SetScale9Bottom(int bottom)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetScale9Bottom(this.swigCPtr, bottom);
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00008BC8 File Offset: 0x00006DC8
		public virtual int GetScale9OriginX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9OriginX(this.swigCPtr);
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00008BE8 File Offset: 0x00006DE8
		public virtual int GetScale9OriginY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9OriginY(this.swigCPtr);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00008C08 File Offset: 0x00006E08
		public virtual int GetScale9Width()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Width(this.swigCPtr);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00008C28 File Offset: 0x00006E28
		public virtual int GetScale9Height()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Height(this.swigCPtr);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00008C48 File Offset: 0x00006E48
		public virtual bool GetScale9Enabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSWidget_GetScale9Enabled(this.swigCPtr);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00008C67 File Offset: 0x00006E67
		public virtual void SetScale9Enabled(bool bEnaled)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetScale9Enabled(this.swigCPtr, bEnaled);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00008C77 File Offset: 0x00006E77
		public virtual void SetScale9Rect(int x, int y, int width, int height)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_SetScale9Rect(this.swigCPtr, x, y, width, height);
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00008C8B File Offset: 0x00006E8B
		public virtual void CloneWidgetCustomProperty(CSWidget cWidget)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_CloneWidgetCustomProperty(this.swigCPtr, CSWidget.getCPtr(cWidget));
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00008CA0 File Offset: 0x00006EA0
		public virtual void ChangeState(bool isNormal)
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_ChangeState(this.swigCPtr, isNormal);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00008CB0 File Offset: 0x00006EB0
		public virtual void ResetState()
		{
			CocoStudioEngineAdapterPINVOKE.CSWidget_ResetState(this.swigCPtr);
		}

		// Token: 0x0400004F RID: 79
		private HandleRef swigCPtr;
	}
}
