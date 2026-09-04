using System;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000010 RID: 16
	public class CSGuides : CSObject
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00004E0F File Offset: 0x0000300F
		public CSGuides(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSGuides_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004E30 File Offset: 0x00003030
		public static HandleRef getCPtr(CSGuides obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004E5C File Offset: 0x0000305C
		~CSGuides()
		{
			this.Dispose();
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004EC0 File Offset: 0x000030C0
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
								CocoStudioEngineAdapterPINVOKE.delete_CSGuides(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSGuides(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004FC0 File Offset: 0x000031C0
		public CSGuides(LineDirection direction) : this(CocoStudioEngineAdapterPINVOKE.new_CSGuides((int)direction), true)
		{
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004FD4 File Offset: 0x000031D4
		public LineDirection GetDirection()
		{
			return (LineDirection)CocoStudioEngineAdapterPINVOKE.CSGuides_GetDirection(this.swigCPtr);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004FF3 File Offset: 0x000031F3
		public void SetDirection(LineDirection direction)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuides_SetDirection(this.swigCPtr, (int)direction);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005004 File Offset: 0x00003204
		public float GetPosition()
		{
			return CocoStudioEngineAdapterPINVOKE.CSGuides_GetPosition(this.swigCPtr);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005023 File Offset: 0x00003223
		public void SetPosition(float position)
		{
			CocoStudioEngineAdapterPINVOKE.CSGuides_SetPosition(this.swigCPtr, position);
		}

		// Token: 0x04000016 RID: 22
		private HandleRef swigCPtr;
	}
}
