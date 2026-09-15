using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSBoneNode : CSNode
	{
		public CSBoneNode(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSBoneNode_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSBoneNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSBoneNode()
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

		public CSBoneNode() : this(CocoStudioEngineAdapterPINVOKE.new_CSBoneNode(), true)
		{
		}

		public void Display(CSNode2D skin, bool hideOthers)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_Display__SWIG_0(this.swigCPtr, CSNode2D.getCPtr(skin), hideOthers);
		}

		public void Display(CSNode2D skin)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_Display__SWIG_1(this.swigCPtr, CSNode2D.getCPtr(skin));
		}

		public virtual void SetLength(float length)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_SetLength(this.swigCPtr, length);
		}

		public float GetLength()
		{
			return CocoStudioEngineAdapterPINVOKE.CSBoneNode_GetLength(this.swigCPtr);
		}

		public void SetDebugDrawEnable(bool isShow)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_SetDebugDrawEnable(this.swigCPtr, isShow);
		}

		public bool GetBoneRackShow()
		{
			return CocoStudioEngineAdapterPINVOKE.CSBoneNode_GetBoneRackShow(this.swigCPtr);
		}

		public void SetBoneRackColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_SetBoneRackColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public Color GetBoneRackColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSBoneNode_GetBoneRackColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

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

		public void SetBlendFunc(BlendFuncValue blendFunc)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_SetBlendFunc(this.swigCPtr, CSBlendFunc.getCPtr(new CSBlendFunc((uint)blendFunc.BlendSrc, (uint)blendFunc.BlendDst)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public BlendFuncValue GetBlendFunc()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSBoneNode_GetBlendFunc(this.swigCPtr);
			CSBlendFunc csblendFunc = new CSBlendFunc(cPtr, true);
			return new BlendFuncValue((BlendSrc)csblendFunc.Src, (BlendDst)csblendFunc.Dst);
		}

		public override void SetObjectState(CSVisualObject.ObjectState objState)
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_SetObjectState(this.swigCPtr, (int)objState);
		}

		public override int HitTest(CocoStudio.Model.PointF point)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSBoneNode_HitTest(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public override bool RectTest(RectF rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSBoneNode_RectTest(this.swigCPtr, Rect.getCPtr(new Rect(rect.X, rect.Y, rect.Width, rect.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public void ResetBoneScaledWidth()
		{
			CocoStudioEngineAdapterPINVOKE.CSBoneNode_ResetBoneScaledWidth(this.swigCPtr);
		}

		private HandleRef swigCPtr;
	}
}
