using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000073 RID: 115
	public class CSBoneNode : CSNode
	{
		// Token: 0x06000C90 RID: 3216 RVA: 0x0001736E File Offset: 0x0001556E
		public CSBoneNode(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSBoneNode_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00017390 File Offset: 0x00015590
		public static HandleRef getCPtr(CSBoneNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x000173BC File Offset: 0x000155BC
		~CSBoneNode()
		{
			this.Dispose();
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x00017420 File Offset: 0x00015620
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
								CocoStudioEngineAdapterPINVOKE.delete_CSBoneNode(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSBoneNode(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x00017520 File Offset: 0x00015720
		public CSBoneNode() : this(CocoStudioEngineAdapterPINVOKE.new_CSBoneNode(), true)
		{
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00017531 File Offset: 0x00015731
		public void Display(CSNode2D skin, bool hideOthers)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_Display__SWIG_0(this.swigCPtr, CSNode2D.getCPtr(skin), hideOthers);
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x00017547 File Offset: 0x00015747
		public void Display(CSNode2D skin)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_Display__SWIG_1(this.swigCPtr, CSNode2D.getCPtr(skin));
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0001755C File Offset: 0x0001575C
		public virtual void SetLength(float length)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_SetLength(this.swigCPtr, length);
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0001756C File Offset: 0x0001576C
		public float GetLength()
		{
			return CocoStudioEngineAdapterPINVOKE.CSBoneNode_GetLength(this.swigCPtr);
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0001758B File Offset: 0x0001578B
		public void SetDebugDrawEnable(bool isShow)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_SetDebugDrawEnable(this.swigCPtr, isShow);
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0001759C File Offset: 0x0001579C
		public bool GetBoneRackShow()
		{
			return CocoStudioEngineAdapterPINVOKE.CSBoneNode_GetBoneRackShow(this.swigCPtr);
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x000175BC File Offset: 0x000157BC
		public void SetBoneRackColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_SetBoneRackColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00017608 File Offset: 0x00015808
		public Color GetBoneRackColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSBoneNode_GetBoneRackColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x00017648 File Offset: 0x00015848
		public CocoStudio.Model.SizeF GetBoxSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSBoneNode_GetBoxSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new CocoStudio.Model.SizeF(size.width, size.height);
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x000176C4 File Offset: 0x000158C4
		public void SetBlendFunc(BlendFuncValue blendFunc)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_SetBlendFunc(this.swigCPtr, CSBlendFunc.getCPtr(new CSBlendFunc((uint)blendFunc.BlendSrc, (uint)blendFunc.BlendDst)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00017708 File Offset: 0x00015908
		public BlendFuncValue GetBlendFunc()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSBoneNode_GetBlendFunc(this.swigCPtr);
			CSBlendFunc csblendFunc = new CSBlendFunc(cPtr, true);
			return new BlendFuncValue((BlendSrc)csblendFunc.Src, (BlendDst)csblendFunc.Dst);
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x00017741 File Offset: 0x00015941
		public override void SetObjectState(CSVisualObject.ObjectState objState)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_SetObjectState(this.swigCPtr, (int)objState);
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00017754 File Offset: 0x00015954
		public override int HitTest(CocoStudio.Model.PointF point)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSBoneNode_HitTest(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x000177A0 File Offset: 0x000159A0
		public override bool RectTest(RectF rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSBoneNode_RectTest(this.swigCPtr, Rect.getCPtr(new Rect(rect.X, rect.Y, rect.Width, rect.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x000177F3 File Offset: 0x000159F3
		public void ResetBoneScaledWidth()
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_ResetBoneScaledWidth(this.swigCPtr);
		}

		// Token: 0x040000DA RID: 218
		private HandleRef swigCPtr;
	}
}
