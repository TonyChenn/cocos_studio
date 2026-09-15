using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSMatrix : IDisposable
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

		public float CM11
		{
			get
			{
				return this.M11();
			}
		}

		public float CM12
		{
			get
			{
				return this.M12();
			}
		}

		public float CM21
		{
			get
			{
				return this.M21();
			}
		}

		public float CM22
		{
			get
			{
				return this.M22();
			}
		}

		public override bool Equals(object obj)
		{
			bool result;
			if (obj is CSMatrix)
			{
				CSMatrix csmatrix = (CSMatrix)obj;
				result = (obj != null && this.CX == csmatrix.CX && this.CY == csmatrix.CY && this.CM11 == csmatrix.CM11 && this.CM12 == csmatrix.CM12 && this.CM21 == csmatrix.CM21 && this.CM22 == csmatrix.CM22);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public override int GetHashCode()
		{
			return (this.CX.GetHashCode() ^ this.CY.GetHashCode()) | (this.CM11.GetHashCode() ^ this.CM12.GetHashCode()) | (this.CM21.GetHashCode() ^ this.CM22.GetHashCode());
		}

		public CSMatrix(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSMatrix obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSMatrix()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSMatrix(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSMatrix(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		public CSMatrix() : this(CocoStudioEngineAdapterPINVOKE.new_CSMatrix(), true)
		{
		}

		public float M11()
		{
			return CocoStudioEngineAdapterPINVOKE.CSMatrix_M11(this.swigCPtr);
		}

		public float M12()
		{
			return CocoStudioEngineAdapterPINVOKE.CSMatrix_M12(this.swigCPtr);
		}

		public float M21()
		{
			return CocoStudioEngineAdapterPINVOKE.CSMatrix_M21(this.swigCPtr);
		}

		public float M22()
		{
			return CocoStudioEngineAdapterPINVOKE.CSMatrix_M22(this.swigCPtr);
		}

		public float X()
		{
			return CocoStudioEngineAdapterPINVOKE.CSMatrix_X(this.swigCPtr);
		}

		public float Y()
		{
			return CocoStudioEngineAdapterPINVOKE.CSMatrix_Y(this.swigCPtr);
		}

		public void SetM11(float v)
		{
			CocoStudioEngineAdapterPINVOKE.CSMatrix_SetM11(this.swigCPtr, v);
		}

		public void SetM12(float v)
		{
			CocoStudioEngineAdapterPINVOKE.CSMatrix_SetM12(this.swigCPtr, v);
		}

		public void SetM21(float v)
		{
			CocoStudioEngineAdapterPINVOKE.CSMatrix_SetM21(this.swigCPtr, v);
		}

		public void SetM22(float v)
		{
			CocoStudioEngineAdapterPINVOKE.CSMatrix_SetM22(this.swigCPtr, v);
		}

		public void SetX(float v)
		{
			CocoStudioEngineAdapterPINVOKE.CSMatrix_SetX(this.swigCPtr, v);
		}

		public void SetY(float v)
		{
			CocoStudioEngineAdapterPINVOKE.CSMatrix_SetY(this.swigCPtr, v);
		}

		private HandleRef swigCPtr;

		protected bool swigCMemOwn;
	}
}
