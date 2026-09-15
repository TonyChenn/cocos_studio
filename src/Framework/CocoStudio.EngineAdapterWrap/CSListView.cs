using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSListView : CSScrollView
	{
		public CSListView(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSListView_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSListView obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		~CSListView()
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
								CocoStudioEngineAdapterPINVOKE.delete_CSListView(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSListView(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		public CSListView() : this(CocoStudioEngineAdapterPINVOKE.new_CSListView(), true)
		{
		}

		public virtual int GetItemSpace()
		{
			return CocoStudioEngineAdapterPINVOKE.CSListView_GetItemSpace(this.swigCPtr);
		}

		public virtual void SetItemSpace(int space)
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_SetItemSpace(this.swigCPtr, space);
		}

		public virtual int GetGravityType()
		{
			return CocoStudioEngineAdapterPINVOKE.CSListView_GetGravityType(this.swigCPtr);
		}

		public virtual void SetGravityType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_SetGravityType(this.swigCPtr, iType);
		}

		public override void InsertChild(int index, CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_InsertChild(this.swigCPtr, index, CSVisualObject.getCPtr(child));
		}

		public override void AddChild(CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_AddChild(this.swigCPtr, CSVisualObject.getCPtr(child));
		}

		public override void RemoveChild(CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_RemoveChild(this.swigCPtr, CSVisualObject.getCPtr(child));
		}

		public override void SetDirectionType(int iType)
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_SetDirectionType(this.swigCPtr, iType);
		}

		public virtual void RefreshInnerLayout()
		{
			CocoStudioEngineAdapterPINVOKE.CSListView_RefreshInnerLayout(this.swigCPtr);
		}

		private HandleRef swigCPtr;
	}
}
