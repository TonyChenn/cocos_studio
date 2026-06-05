using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000012 RID: 18
	public class CSLight : CSNode3D
	{
		// Token: 0x060000EB RID: 235 RVA: 0x0000531B File Offset: 0x0000351B
		public CSLight(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSLight_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000533C File Offset: 0x0000353C
		public static HandleRef getCPtr(CSLight obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005368 File Offset: 0x00003568
		~CSLight()
		{
			this.Dispose();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000053CC File Offset: 0x000035CC
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
								CocoStudioEngineAdapterPINVOKE.delete_CSLight(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSLight(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000054CC File Offset: 0x000036CC
		public CSLight() : this(CocoStudioEngineAdapterPINVOKE.new_CSLight(), true)
		{
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000054DD File Offset: 0x000036DD
		public void SetLightType(int type)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetLightType(this.swigCPtr, type);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000054F0 File Offset: 0x000036F0
		public int GetLightType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_GetLightType(this.swigCPtr);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000550F File Offset: 0x0000370F
		public void SetLightFlag(int flag)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetLightFlag(this.swigCPtr, flag);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005520 File Offset: 0x00003720
		public int GetLightFlag()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_GetLightFlag(this.swigCPtr);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000553F File Offset: 0x0000373F
		public void SetIntensity(float intensity)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetIntensity(this.swigCPtr, intensity);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005550 File Offset: 0x00003750
		public float GetIntensity()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_GetIntensity(this.swigCPtr);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000556F File Offset: 0x0000376F
		public void SetEnabled(bool enable)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetEnabled(this.swigCPtr, enable);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005580 File Offset: 0x00003780
		public bool IsEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_IsEnabled(this.swigCPtr);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000559F File Offset: 0x0000379F
		public void SetRange(float range)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetRange(this.swigCPtr, range);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000055B0 File Offset: 0x000037B0
		public float GetRange()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_GetRange(this.swigCPtr);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000055CF File Offset: 0x000037CF
		public void SetInnerAngle(float angle)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetInnerAngle(this.swigCPtr, angle);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000055E0 File Offset: 0x000037E0
		public float GetInnerAngle()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_GetInnerAngle(this.swigCPtr);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000055FF File Offset: 0x000037FF
		public void SetOuterAngle(float angle)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetOuterAngle(this.swigCPtr, angle);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005610 File Offset: 0x00003810
		public float GetOuterAngle()
		{
			return CocoStudioEngineAdapterPINVOKE.CSLight_GetOuterAngle(this.swigCPtr);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005630 File Offset: 0x00003830
		public bool OnMouseMove(CocoStudio.Model.PointF point)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSLight_OnMouseMove(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000567C File Offset: 0x0000387C
		public bool OnMouseDown(CocoStudio.Model.PointF point)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSLight_OnMouseDown(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000056C8 File Offset: 0x000038C8
		public bool OnMouseUp(CocoStudio.Model.PointF point)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSLight_OnMouseUp(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005714 File Offset: 0x00003914
		public bool HitControlPoint(CocoStudio.Model.PointF point)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSLight_HitControlPoint(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000575D File Offset: 0x0000395D
		public void RefreshLightState(bool enabled)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_RefreshLightState(this.swigCPtr, enabled);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000576D File Offset: 0x0000396D
		public void RefreshLightIndex(int index)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_RefreshLightIndex(this.swigCPtr, index);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005780 File Offset: 0x00003980
		public override Color GetDisplayColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSLight_GetDisplayColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000057BF File Offset: 0x000039BF
		public override void RestoreRenderMode()
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_RestoreRenderMode(this.swigCPtr);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000057D0 File Offset: 0x000039D0
		public override void SetPixelRenderMode(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSLight_SetPixelRenderMode(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x04000018 RID: 24
		private HandleRef swigCPtr;
	}
}
