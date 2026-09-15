using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace OpenDialogs
{
	public class BaseDialogNative : NativeWindow, IDisposable
	{
		public event BaseDialogNative.FileNameChangedHandler FileNameChanged;

		public event BaseDialogNative.FileNameChangedHandler FolderNameChanged;

		public event BaseDialogNative.FilesChangedHandler FilesSelected;

		public BaseDialogNative(IntPtr handle)
		{
			this.mhandle = handle;
			base.AssignHandle(handle);
		}

		public void Dispose()
		{
			this.ReleaseHandle();
		}

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

		private IntPtr mhandle;

		public delegate void FileNameChangedHandler(BaseDialogNative sender, string filePath);

		public delegate void ComfirmChangedHandler(BaseDialogNative sender);

		public delegate void FilesChangedHandler(BaseDialogNative sender, IntPtr handle);
	}
}
