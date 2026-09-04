using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200000E RID: 14
	public class CSNode3D : CSNode
	{
		// Token: 0x060000B5 RID: 181 RVA: 0x0000470E File Offset: 0x0000290E
		public CSNode3D(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSNode3D_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004730 File Offset: 0x00002930
		public static HandleRef getCPtr(CSNode3D obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000475C File Offset: 0x0000295C
		~CSNode3D()
		{
			this.Dispose();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000047C0 File Offset: 0x000029C0
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
								CocoStudioEngineAdapterPINVOKE.delete_CSNode3D(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSNode3D(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000048C0 File Offset: 0x00002AC0
		public CSNode3D() : this(CocoStudioEngineAdapterPINVOKE.new_CSNode3D(), true)
		{
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000048D4 File Offset: 0x00002AD4
		public virtual bool IsFlipped()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode3D_IsFlipped(this.swigCPtr);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000048F3 File Offset: 0x00002AF3
		public virtual void RefreshObjectFace()
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_RefreshObjectFace(this.swigCPtr);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00004904 File Offset: 0x00002B04
		public virtual void SetPixelRenderMode(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_SetPixelRenderMode(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000494F File Offset: 0x00002B4F
		public virtual void RestoreRenderMode()
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_RestoreRenderMode(this.swigCPtr);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000495E File Offset: 0x00002B5E
		public virtual void SetNodeCameraMask(uint mask, bool applyChildren)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_SetNodeCameraMask(this.swigCPtr, mask, applyChildren);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004970 File Offset: 0x00002B70
		public virtual uint GetNodeCameraMask()
		{
			return CocoStudioEngineAdapterPINVOKE.CSNode3D_GetNodeCameraMask(this.swigCPtr);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000498F File Offset: 0x00002B8F
		public virtual void Roll(float degree, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Roll__SWIG_0(this.swigCPtr, degree, (int)space);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000049A0 File Offset: 0x00002BA0
		public virtual void Roll(float degree)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Roll__SWIG_1(this.swigCPtr, degree);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000049B0 File Offset: 0x00002BB0
		public virtual void Pitch(float degree, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Pitch__SWIG_0(this.swigCPtr, degree, (int)space);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000049C1 File Offset: 0x00002BC1
		public virtual void Pitch(float degree)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Pitch__SWIG_1(this.swigCPtr, degree);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000049D1 File Offset: 0x00002BD1
		public virtual void Yaw(float degree, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Yaw__SWIG_0(this.swigCPtr, degree, (int)space);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000049E2 File Offset: 0x00002BE2
		public virtual void Yaw(float degree)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Yaw__SWIG_1(this.swigCPtr, degree);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000049F4 File Offset: 0x00002BF4
		public virtual void Rotate3D(Point3F axis, float degree, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Rotate3D__SWIG_0(this.swigCPtr, Vec3.getCPtr(new Vec3(axis.X, axis.Y, axis.Z)), degree, (int)space);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004A40 File Offset: 0x00002C40
		public virtual void Rotate3D(Point3F axis, float degree)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Rotate3D__SWIG_1(this.swigCPtr, Vec3.getCPtr(new Vec3(axis.X, axis.Y, axis.Z)), degree);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004A8C File Offset: 0x00002C8C
		public virtual void Rotate3D(Quaternion q, CSVisualObject.TransformSpace space)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Rotate3D__SWIG_2(this.swigCPtr, Quaternion.getCPtr(q), (int)space);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004AC0 File Offset: 0x00002CC0
		public virtual void Rotate3D(Quaternion q)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Rotate3D__SWIG_3(this.swigCPtr, Quaternion.getCPtr(q));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004AF4 File Offset: 0x00002CF4
		public virtual void Rotate3D(Quaternion q, CSNode3D target)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_Rotate3D__SWIG_4(this.swigCPtr, Quaternion.getCPtr(q), CSNode3D.getCPtr(target));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004B2C File Offset: 0x00002D2C
		public override void SetObjectState(CSVisualObject.ObjectState boxState)
		{
			CocoStudioEngineAdapterPINVOKE.CSNode3D_SetObjectState(this.swigCPtr, (int)boxState);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004B3C File Offset: 0x00002D3C
		public override float HitTest3D(CocoStudio.Model.PointF screenPoint)
		{
			float result = CocoStudioEngineAdapterPINVOKE.CSNode3D_HitTest3D(this.swigCPtr, Vec2.getCPtr(new Vec2(screenPoint.X, screenPoint.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004B88 File Offset: 0x00002D88
		public override bool RectTest3D(RectF rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSNode3D_RectTest3D(this.swigCPtr, Rect.getCPtr(new Rect(rect.X, rect.Y, rect.Width, rect.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x04000014 RID: 20
		private HandleRef swigCPtr;
	}
}
