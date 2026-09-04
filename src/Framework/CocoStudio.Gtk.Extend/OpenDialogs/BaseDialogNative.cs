using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace OpenDialogs
{
	// Token: 0x02000020 RID: 32
	public class BaseDialogNative : NativeWindow, IDisposable
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600010A RID: 266 RVA: 0x00005F24 File Offset: 0x00004124
		// (remove) Token: 0x0600010B RID: 267 RVA: 0x00005F60 File Offset: 0x00004160
		public event BaseDialogNative.FileNameChangedHandler FileNameChanged;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600010C RID: 268 RVA: 0x00005F9C File Offset: 0x0000419C
		// (remove) Token: 0x0600010D RID: 269 RVA: 0x00005FD8 File Offset: 0x000041D8
		public event BaseDialogNative.FileNameChangedHandler FolderNameChanged;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600010E RID: 270 RVA: 0x00006014 File Offset: 0x00004214
		// (remove) Token: 0x0600010F RID: 271 RVA: 0x00006050 File Offset: 0x00004250
		public event BaseDialogNative.FilesChangedHandler FilesSelected;

		// Token: 0x06000110 RID: 272 RVA: 0x0000608C File Offset: 0x0000428C
		public BaseDialogNative(IntPtr handle)
		{
			this.mhandle = handle;
			base.AssignHandle(handle);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000060A6 File Offset: 0x000042A6
		public void Dispose()
		{
			this.ReleaseHandle();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000060B0 File Offset: 0x000042B0
		protected override void WndProc(ref Message m)
		{
			int msg = m.Msg;
			if (msg == 78)
			{
				OFNOTIFY ofnotify = (OFNOTIFY)Marshal.PtrToStructure(m.LParam, typeof(OFNOTIFY));
				if (ofnotify.hdr.code == 4294966694U)
				{
					StringBuilder stringBuilder = new StringBuilder(102400);
					NativeMethods.SendMessage(NativeMethods.GetParent(this.mhandle), 1125, 102400, stringBuilder);
					if (this.FileNameChanged != null)
					{
						this.FileNameChanged(this, stringBuilder.ToString());
					}
				}
				else if (ofnotify.hdr.code == 4294966693U)
				{
					StringBuilder stringBuilder2 = new StringBuilder(256);
					NativeMethods.SendMessage(NativeMethods.GetParent(this.mhandle), 1126, 256, stringBuilder2);
					if (this.FolderNameChanged != null)
					{
						this.FolderNameChanged(this, stringBuilder2.ToString());
					}
				}
				if (this.FilesSelected != null)
				{
					this.FilesSelected(this, this.mhandle);
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x0400007F RID: 127
		private IntPtr mhandle;

		// Token: 0x02000021 RID: 33
		// (Invoke) Token: 0x06000114 RID: 276
		public delegate void FileNameChangedHandler(BaseDialogNative sender, string filePath);

		// Token: 0x02000022 RID: 34
		// (Invoke) Token: 0x06000118 RID: 280
		public delegate void ComfirmChangedHandler(BaseDialogNative sender);

		// Token: 0x02000023 RID: 35
		// (Invoke) Token: 0x0600011C RID: 284
		public delegate void FilesChangedHandler(BaseDialogNative sender, IntPtr handle);
	}
}
