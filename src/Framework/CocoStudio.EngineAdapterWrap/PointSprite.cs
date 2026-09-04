using System;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200006D RID: 109
	public class PointSprite : IDisposable
	{
		// Token: 0x06000C1C RID: 3100 RVA: 0x00015C2D File Offset: 0x00013E2D
		public PointSprite(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x00015C4C File Offset: 0x00013E4C
		public static HandleRef getCPtr(PointSprite obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00015C78 File Offset: 0x00013E78
		~PointSprite()
		{
			this.Dispose();
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00015CDC File Offset: 0x00013EDC
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
								CocoStudioEngineAdapterPINVOKE.delete_PointSprite(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_PointSprite(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x00015DEC File Offset: 0x00013FEC
		// (set) Token: 0x06000C20 RID: 3104 RVA: 0x00015DD4 File Offset: 0x00013FD4
		public Vec2 pos
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.PointSprite_pos_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Vec2(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.PointSprite_pos_set(this.swigCPtr, Vec2.getCPtr(value));
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x00015E3C File Offset: 0x0001403C
		// (set) Token: 0x06000C22 RID: 3106 RVA: 0x00015E24 File Offset: 0x00014024
		public Color4B color
		{
			get
			{
				IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.PointSprite_color_get(this.swigCPtr);
				return (intPtr == IntPtr.Zero) ? null : new Color4B(intPtr, false);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.PointSprite_color_set(this.swigCPtr, Color4B.getCPtr(value));
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x00015E84 File Offset: 0x00014084
		// (set) Token: 0x06000C24 RID: 3108 RVA: 0x00015E74 File Offset: 0x00014074
		public float size
		{
			get
			{
				return CocoStudioEngineAdapterPINVOKE.PointSprite_size_get(this.swigCPtr);
			}
			set
			{
				CocoStudioEngineAdapterPINVOKE.PointSprite_size_set(this.swigCPtr, value);
			}
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00015EA3 File Offset: 0x000140A3
		public PointSprite() : this(CocoStudioEngineAdapterPINVOKE.new_PointSprite(), true)
		{
		}

		// Token: 0x040000CE RID: 206
		private HandleRef swigCPtr;

		// Token: 0x040000CF RID: 207
		protected bool swigCMemOwn;
	}
}
