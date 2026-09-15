using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class MatrixNode : IDisposable
	{
		public float CX
		{
			get
			{
				return this.X();
			}
		}

		public float CY
		{
			get
			{
				return this.Y();
			}
		}

		public float CScaleX
		{
			get
			{
				return this.ScaleX();
			}
		}

		public float CScaleY
		{
			get
			{
				return this.ScaleY();
			}
		}

		public float CSkewX
		{
			get
			{
				return this.SkewX();
			}
		}

		public float CSkewY
		{
			get
			{
				return this.SkewY();
			}
		}

		public override bool Equals(object obj)
		{
			bool result;
			if (obj is MatrixNode)
			{
				MatrixNode matrixNode = (MatrixNode)obj;
				result = (obj != null && this.CX == matrixNode.CX && this.CY == matrixNode.CY && this.CScaleX == matrixNode.CScaleX && this.CScaleY == matrixNode.CScaleY && this.CSkewX == matrixNode.CSkewX && this.CSkewY == matrixNode.CSkewY);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public override int GetHashCode()
		{
			return (this.CX.GetHashCode() ^ this.CY.GetHashCode()) | (this.CScaleX.GetHashCode() ^ this.CScaleY.GetHashCode()) | (this.CSkewX.GetHashCode() ^ this.CSkewY.GetHashCode());
		}

		public MatrixNode(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(MatrixNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~MatrixNode()
		{
			this.Dispose();
		}

		public virtual void Dispose()
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
								CocoStudioEngineAdapterPINVOKE.delete_MatrixNode(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_MatrixNode(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public void init(CSVisualObject node)
		{
			CocoStudioEngineAdapterPINVOKE.MatrixNode_init(this.swigCPtr, CSVisualObject.getCPtr(node));
		}

		public void print()
		{
			CocoStudioEngineAdapterPINVOKE.MatrixNode_print(this.swigCPtr);
		}

		public float X()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_X(this.swigCPtr);
		}

		public float Y()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_Y(this.swigCPtr);
		}

		public float ScaleX()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_ScaleX(this.swigCPtr);
		}

		public float ScaleY()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_ScaleY(this.swigCPtr);
		}

		public float SkewX()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_SkewX(this.swigCPtr);
		}

		public float SkewY()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_SkewY(this.swigCPtr);
		}

		public float AnchorPointX()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_AnchorPointX(this.swigCPtr);
		}

		public float AnchorPointY()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_AnchorPointY(this.swigCPtr);
		}

		public MatrixNode() : this(CocoStudioEngineAdapterPINVOKE.new_MatrixNode(), true)
		{
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}
