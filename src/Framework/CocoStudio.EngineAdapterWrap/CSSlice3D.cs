using System;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSSlice3D : CSNode3D
	{
		public CSSlice3D(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSSlice3D_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSSlice3D obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSSlice3D()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSSlice3D(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSSlice3D(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSSlice3D() : this(CocoStudioEngineAdapterPINVOKE.new_CSSlice3D(), true)
		{
		}

		public void SetFileData(ResourceData data)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetFileData(this.swigCPtr, CSResourceData.getCPtr(new CSResourceData(data.Path, (CSResourceData.CSEnumResourceType)data.Type, data.Plist)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public SizeF GetSliceSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetSliceSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new SizeF(size.width, size.height);
		}

		public void SetSliceSize(SizeF size)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetSliceSize(this.swigCPtr, Size.getCPtr(new Size(size.Width, size.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public bool GetFlipX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetFlipX(this.swigCPtr);
		}

		public void SetFlipX(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetFlipX(this.swigCPtr, flip);
		}

		public bool GetFlipY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetFlipY(this.swigCPtr);
		}

		public void SetFlipY(bool flip)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetFlipY(this.swigCPtr, flip);
		}

		public bool getUVActive()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlice3D_getUVActive(this.swigCPtr);
		}

		public void setUVActive(bool active)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_setUVActive(this.swigCPtr, active);
		}

		public void SetAnimationSpeed(PointF UV)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetAnimationSpeed(this.swigCPtr, Vec2.getCPtr(new Vec2(UV.X, UV.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public PointF GetAnimationSpeed()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetAnimationSpeed(this.swigCPtr);
			Vec2 vec = new Vec2(cPtr, true);
			return new PointF(vec.x, vec.y);
		}

		public PointF GetRowAndColumn()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetRowAndColumn(this.swigCPtr);
			Vec2 vec = new Vec2(cPtr, true);
			return new PointF(vec.x, vec.y);
		}

		public void SetRowAndColumn(PointF rowColumn)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetRowAndColumn(this.swigCPtr, Vec2.getCPtr(new Vec2(rowColumn.X, rowColumn.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public bool GetTextureActive()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetTextureActive(this.swigCPtr);
		}

		public void SetTextureActive(bool active)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetTextureActive(this.swigCPtr, active);
		}

		public void SetFramerate(float frame)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetFramerate(this.swigCPtr, frame);
		}

		public float GetFramerate()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlice3D_GetFramerate(this.swigCPtr);
		}

		public void SetBillBoardMode(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSSlice3D_SetBillBoardMode(this.swigCPtr, iType);
		}

		public int getBillBoardMode()
		{
			return CocoStudioEngineAdapterPINVOKE.CSSlice3D_getBillBoardMode(this.swigCPtr);
		}

		private HandleRef swigCPtr;
	}
}
