using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000013 RID: 19
	public class CSNode2D : CSNode
	{
		// Token: 0x06000107 RID: 263 RVA: 0x0000581B File Offset: 0x00003A1B
		public CSNode2D(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSNode2D_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000583C File Offset: 0x00003A3C
		public static HandleRef getCPtr(CSNode2D obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005868 File Offset: 0x00003A68
		~CSNode2D()
		{
			this.Dispose();
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000058CC File Offset: 0x00003ACC
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
								CocoStudioEngineAdapterPINVOKE.delete_CSNode2D(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSNode2D(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000059CC File Offset: 0x00003BCC
		public CSNode2D() : this(CocoStudioEngineAdapterPINVOKE.new_CSNode2D(), true)
		{
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000059E0 File Offset: 0x00003BE0
		public virtual void InitIcon(string iconFile)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_InitIcon__SWIG_0(this.swigCPtr, iconFile);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005A10 File Offset: 0x00003C10
		public virtual void InitIcon(SizeF iconSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_InitIcon__SWIG_1(this.swigCPtr, Size.getCPtr(new Size(iconSize.Width, iconSize.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00005A52 File Offset: 0x00003C52
		public virtual void SetIconVisible(bool iconVisible)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetIconVisible(this.swigCPtr, iconVisible);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00005A64 File Offset: 0x00003C64
		public virtual bool GetIconVisible()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetIconVisible(this.swigCPtr);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005A83 File Offset: 0x00003C83
		public virtual void RefreshLayout()
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_RefreshLayout(this.swigCPtr);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005A92 File Offset: 0x00003C92
		public virtual void RefreshChildrenLayout()
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_RefreshChildrenLayout(this.swigCPtr);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00005AA4 File Offset: 0x00003CA4
		public virtual ScaleValue GetBoxAnchorPoint()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSNode2D_GetBoxAnchorPoint(this.swigCPtr);
			CSScale csscale = new CSScale(cPtr, false);
			return new ScaleValue(csscale.GetScaleX(), csscale.GetScaleY(), 0.1, -99999999.0, 99999999.0);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00005AF8 File Offset: 0x00003CF8
		public virtual SizeF GetBoxSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSNode2D_GetBoxSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new SizeF(size.width, size.height);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005B74 File Offset: 0x00003D74
		public override RectF GetBoundingRect()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSNode2D_GetBoundingRect(this.swigCPtr);
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

		// Token: 0x06000115 RID: 277 RVA: 0x00005C44 File Offset: 0x00003E44
		public virtual void SetHorizontalEdge(int hEage)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetHorizontalEdge(this.swigCPtr, hEage);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005C54 File Offset: 0x00003E54
		public virtual int GetHorizontalEdge()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetHorizontalEdge(this.swigCPtr);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005C73 File Offset: 0x00003E73
		public virtual void SetVerticalEdge(int vEage)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetVerticalEdge(this.swigCPtr, vEage);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005C84 File Offset: 0x00003E84
		public virtual int GetVerticalEdge()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetVerticalEdge(this.swigCPtr);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005CA3 File Offset: 0x00003EA3
		public virtual void SetPositionPercentXEnabled(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPositionPercentXEnabled(this.swigCPtr, isEnabled);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005CB4 File Offset: 0x00003EB4
		public virtual bool IsUsingPositionPercentX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_IsUsingPositionPercentX(this.swigCPtr);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005CD3 File Offset: 0x00003ED3
		public virtual void SetPositionPercentYEnabled(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPositionPercentYEnabled(this.swigCPtr, isEnabled);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005CE4 File Offset: 0x00003EE4
		public virtual bool IsUsingPositionPercentY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_IsUsingPositionPercentY(this.swigCPtr);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005D03 File Offset: 0x00003F03
		public virtual void SetPositionPercentX(float percent)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPositionPercentX(this.swigCPtr, percent);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005D14 File Offset: 0x00003F14
		public virtual float GetPositionPercentX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetPositionPercentX(this.swigCPtr);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005D33 File Offset: 0x00003F33
		public virtual void SetPositionPercentY(float percent)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPositionPercentY(this.swigCPtr, percent);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005D44 File Offset: 0x00003F44
		public virtual float GetPositionPercentY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetPositionPercentY(this.swigCPtr);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005D63 File Offset: 0x00003F63
		public virtual void SetLeftMargin(float margin)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetLeftMargin(this.swigCPtr, margin);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005D74 File Offset: 0x00003F74
		public virtual float GetLeftMargin()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetLeftMargin(this.swigCPtr);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005D93 File Offset: 0x00003F93
		public virtual void SetRightMargin(float margin)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetRightMargin(this.swigCPtr, margin);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005DA4 File Offset: 0x00003FA4
		public virtual float GetRightMargin()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetRightMargin(this.swigCPtr);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005DC3 File Offset: 0x00003FC3
		public virtual void SetTopMargin(float margin)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetTopMargin(this.swigCPtr, margin);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005DD4 File Offset: 0x00003FD4
		public virtual float GetTopMargin()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetTopMargin(this.swigCPtr);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005DF3 File Offset: 0x00003FF3
		public virtual void SetBottomMargin(float margin)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetBottomMargin(this.swigCPtr, margin);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00005E04 File Offset: 0x00004004
		public virtual float GetBottomMargin()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetBottomMargin(this.swigCPtr);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005E23 File Offset: 0x00004023
		public virtual void SetPercentWidthEnable(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPercentWidthEnable(this.swigCPtr, isEnabled);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005E34 File Offset: 0x00004034
		public virtual bool GetPercentWidthEnable()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetPercentWidthEnable(this.swigCPtr);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005E53 File Offset: 0x00004053
		public virtual void SetPercentHeightEnable(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPercentHeightEnable(this.swigCPtr, isEnabled);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005E64 File Offset: 0x00004064
		public virtual bool GetPercentHeightEnable()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetPercentHeightEnable(this.swigCPtr);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005E83 File Offset: 0x00004083
		public virtual void SetPercentWidth(float percent)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPercentWidth(this.swigCPtr, percent);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005E94 File Offset: 0x00004094
		public virtual float GetPercentWidth()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetPercentWidth(this.swigCPtr);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005EB3 File Offset: 0x000040B3
		public virtual void SetPercentHeight(float percent)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPercentHeight(this.swigCPtr, percent);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005EC4 File Offset: 0x000040C4
		public virtual float GetPercentHeight()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetPercentHeight(this.swigCPtr);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005EE3 File Offset: 0x000040E3
		public virtual void SetSizeWidth(float width)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetSizeWidth(this.swigCPtr, width);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00005EF4 File Offset: 0x000040F4
		public virtual float GetSizeWidth()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetSizeWidth(this.swigCPtr);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00005F13 File Offset: 0x00004113
		public virtual void SetSizeHeight(float height)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetSizeHeight(this.swigCPtr, height);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005F24 File Offset: 0x00004124
		public virtual float GetSizeHeight()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetSizeHeight(this.swigCPtr);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005F43 File Offset: 0x00004143
		public virtual void SetStretchWidthEnable(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetStretchWidthEnable(this.swigCPtr, isEnabled);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005F54 File Offset: 0x00004154
		public virtual bool GetStretchWidthEnable()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetStretchWidthEnable(this.swigCPtr);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005F73 File Offset: 0x00004173
		public virtual void SetStretchHeightEnable(bool isEnabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetStretchHeightEnable(this.swigCPtr, isEnabled);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005F84 File Offset: 0x00004184
		public virtual bool GetStretchHeightEnable()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode2D_GetStretchHeightEnable(this.swigCPtr);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005FA4 File Offset: 0x000041A4
		public override void SetPosition(PointF position)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetPosition(this.swigCPtr, Vec2.getCPtr(new Vec2(position.X, position.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005FE8 File Offset: 0x000041E8
		public override void SetScale(ScaleValue scale)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetScale(this.swigCPtr, CSScale.getCPtr(new CSScale(scale.ScaleX, scale.ScaleY)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000602C File Offset: 0x0000422C
		public override void SetAnchorPoint(ScaleValue anchorPoint)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetAnchorPoint(this.swigCPtr, CSScale.getCPtr(new CSScale(anchorPoint.ScaleX, anchorPoint.ScaleY)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006070 File Offset: 0x00004270
		public override void SetSize(SizeF cSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetSize(this.swigCPtr, Size.getCPtr(new Size(cSize.Width, cSize.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000060B4 File Offset: 0x000042B4
		public override int HitTest(PointF point)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSNode2D_HitTest(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006100 File Offset: 0x00004300
		public override bool RectTest(RectF rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSNode2D_RectTest(this.swigCPtr, Rect.getCPtr(new Rect(rect.X, rect.Y, rect.Width, rect.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006154 File Offset: 0x00004354
		public override CSMatrix GetWorldMatrix()
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSNode2D_GetWorldMatrix(this.swigCPtr), true);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000617C File Offset: 0x0000437C
		public override CSMatrix GetAnchorWorldMatrix()
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSNode2D_GetAnchorWorldMatrix(this.swigCPtr), true);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000061A1 File Offset: 0x000043A1
		public override void SetObjectState(CSVisualObject.ObjectState boxState)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode2D_SetObjectState(this.swigCPtr, (int)boxState);
		}

		// Token: 0x04000019 RID: 25
		private HandleRef swigCPtr;
	}
}
