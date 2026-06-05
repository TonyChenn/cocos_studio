using System;
using System.ComponentModel;
using Gdk;
using Gtk;

namespace Cocos.Launcher.Control
{
	// Token: 0x0200000A RID: 10
	[ToolboxItem(true)]
	public class ImageButtonView : EventBox
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002820 File Offset: 0x00000A20
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00002828 File Offset: 0x00000A28
		private string NormalNotifyPath { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002831 File Offset: 0x00000A31
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00002839 File Offset: 0x00000A39
		private string EnterNotifyPath { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002842 File Offset: 0x00000A42
		// (set) Token: 0x06000050 RID: 80 RVA: 0x0000284A File Offset: 0x00000A4A
		private string PressNotifyPath { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002853 File Offset: 0x00000A53
		// (set) Token: 0x06000052 RID: 82 RVA: 0x0000285B File Offset: 0x00000A5B
		public bool IsCursor { get; set; }

		// Token: 0x06000053 RID: 83 RVA: 0x00002864 File Offset: 0x00000A64
		public ImageButtonView()
		{
			this.image_button = new ImageBin();
			base.VisibleWindow = false;
			base.Add(this.image_button);
			this.InitEvent();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002890 File Offset: 0x00000A90
		private void InitEvent()
		{
			base.EnterNotifyEvent += this.LauncherButton_EnterNotifyEvent;
			base.LeaveNotifyEvent += this.LauncherButton_LeaveNotifyEvent;
			base.ButtonPressEvent += this.LauncherButton_ButtonPressEvent;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000028C8 File Offset: 0x00000AC8
		public void SetNormalBack(string path)
		{
			this.NormalNotifyPath = path;
			this.image_button.SetImageView(ImageIcon.GetIcon(path));
			base.ShowAll();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000028E8 File Offset: 0x00000AE8
		public void SetMoveBack(string path)
		{
			this.EnterNotifyPath = path;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000028F1 File Offset: 0x00000AF1
		public void SetPressBack(string path)
		{
			this.PressNotifyPath = path;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000028FA File Offset: 0x00000AFA
		private void LauncherButton_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			if (this.IsCursor)
			{
				base.GdkWindow.Cursor = null;
			}
			if (!string.IsNullOrEmpty(this.NormalNotifyPath))
			{
				this.image_button.SetImageView(ImageIcon.GetIcon(this.NormalNotifyPath));
				base.ShowAll();
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000293C File Offset: 0x00000B3C
		private void LauncherButton_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
		{
			if (this.IsCursor)
			{
				base.GdkWindow.Cursor = new Cursor(CursorType.Hand1);
			}
			if (!string.IsNullOrEmpty(this.EnterNotifyPath))
			{
				this.image_button.SetImageView(ImageIcon.GetIcon(this.EnterNotifyPath));
				base.ShowAll();
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000298C File Offset: 0x00000B8C
		private void LauncherButton_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (args.Event.GetMouseButton() != MouseButton.Left)
			{
				return;
			}
			if (!string.IsNullOrEmpty(this.PressNotifyPath))
			{
				this.image_button.SetImageView(ImageIcon.GetIcon(this.PressNotifyPath));
				base.ShowAll();
			}
			args.RetVal = true;
		}

		// Token: 0x0400002B RID: 43
		private ImageBin image_button;
	}
}
