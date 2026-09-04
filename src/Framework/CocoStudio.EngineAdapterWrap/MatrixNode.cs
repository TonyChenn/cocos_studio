using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200001C RID: 28
	public class MatrixNode : IDisposable
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00006908 File Offset: 0x00004B08
		public float CX
		{
			get
			{
				return this.X();
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000173 RID: 371 RVA: 0x00006920 File Offset: 0x00004B20
		public float CY
		{
			get
			{
				return this.Y();
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00006938 File Offset: 0x00004B38
		public float CScaleX
		{
			get
			{
				return this.ScaleX();
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000175 RID: 373 RVA: 0x00006950 File Offset: 0x00004B50
		public float CScaleY
		{
			get
			{
				return this.ScaleY();
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00006968 File Offset: 0x00004B68
		public float CSkewX
		{
			get
			{
				return this.SkewX();
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00006980 File Offset: 0x00004B80
		public float CSkewY
		{
			get
			{
				return this.SkewY();
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00006998 File Offset: 0x00004B98
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

		// Token: 0x06000179 RID: 377 RVA: 0x00006A24 File Offset: 0x00004C24
		public override int GetHashCode()
		{
			return (this.CX.GetHashCode() ^ this.CY.GetHashCode()) | (this.CScaleX.GetHashCode() ^ this.CScaleY.GetHashCode()) | (this.CSkewX.GetHashCode() ^ this.CSkewY.GetHashCode());
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00006A8F File Offset: 0x00004C8F
		public MatrixNode(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00006AB0 File Offset: 0x00004CB0
		public static HandleRef getCPtr(MatrixNode obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006ADC File Offset: 0x00004CDC
		~MatrixNode()
		{
			this.Dispose();
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00006B40 File Offset: 0x00004D40
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

		// Token: 0x0600017E RID: 382 RVA: 0x00006C38 File Offset: 0x00004E38
		public void init(CSVisualObject node)
		{
			CocoStudioEngineAdapterPINVOKE.MatrixNode_init(this.swigCPtr, CSVisualObject.getCPtr(node));
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006C4D File Offset: 0x00004E4D
		public void print()
		{
			CocoStudioEngineAdapterPINVOKE.MatrixNode_print(this.swigCPtr);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006C5C File Offset: 0x00004E5C
		public float X()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_X(this.swigCPtr);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006C7C File Offset: 0x00004E7C
		public float Y()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_Y(this.swigCPtr);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006C9C File Offset: 0x00004E9C
		public float ScaleX()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_ScaleX(this.swigCPtr);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006CBC File Offset: 0x00004EBC
		public float ScaleY()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_ScaleY(this.swigCPtr);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006CDC File Offset: 0x00004EDC
		public float SkewX()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_SkewX(this.swigCPtr);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006CFC File Offset: 0x00004EFC
		public float SkewY()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_SkewY(this.swigCPtr);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006D1C File Offset: 0x00004F1C
		public float AnchorPointX()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_AnchorPointX(this.swigCPtr);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006D3C File Offset: 0x00004F3C
		public float AnchorPointY()
		{
			return CocoStudioEngineAdapterPINVOKE.MatrixNode_AnchorPointY(this.swigCPtr);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00006D5B File Offset: 0x00004F5B
		public MatrixNode() : this(CocoStudioEngineAdapterPINVOKE.new_MatrixNode(), true)
		{
		}

		// Token: 0x0400001F RID: 31
		private HandleRef swigCPtr;

		// Token: 0x04000020 RID: 32
		protected bool swigCMemOwn;
	}
}
