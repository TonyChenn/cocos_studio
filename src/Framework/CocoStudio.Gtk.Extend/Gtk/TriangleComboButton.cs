using System;
using Gdk;
using MonoDevelop.Components;
using MonoDevelop.Core;
using Xwt.Drawing;

namespace Gtk
{
	// Token: 0x02000061 RID: 97
	public class TriangleComboButton : EventBox
	{
		// Token: 0x14000013 RID: 19
		// (add) Token: 0x0600020F RID: 527 RVA: 0x00009304 File Offset: 0x00007504
		// (remove) Token: 0x06000210 RID: 528 RVA: 0x00009340 File Offset: 0x00007540
		public event EventHandler Clicked;

		// Token: 0x06000211 RID: 529 RVA: 0x0000937C File Offset: 0x0000757C
		public TriangleComboButton()
		{
			base.ModifyBg(StateType.Normal, new Gdk.Color(50, 50, 54));
			base.ModifyBg(StateType.Insensitive, new Gdk.Color(62, 62, 66));
			if (Platform.IsWindows)
			{
				this.normalImg = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.TriangleButton.WinNormal.png");
				this.hoverImg = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.TriangleButton.WinHover.png");
			}
			else
			{
				this.normalImg = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.TriangleButton.MacNormal.png");
				this.hoverImg = ImageIcon.GetIcon("CocoStudio.DefaultResource.EditorResource.TriangleButton.MacHover.png");
			}
			this.imageView = new ImageView(this.normalImg);
			base.Add(this.imageView);
			base.ShowAll();
			base.EnterNotifyEvent += this.MouseEnterEventHandler;
			base.LeaveNotifyEvent += this.MouseLeaveEventHandler;
			base.ButtonReleaseEvent += this.ButtonReleasedHandler;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000946F File Offset: 0x0000766F
		private void MouseEnterEventHandler(object o, EnterNotifyEventArgs args)
		{
			this.imageView.Image = this.hoverImg;
			this.isMouseIn = true;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000948B File Offset: 0x0000768B
		private void MouseLeaveEventHandler(object o, LeaveNotifyEventArgs args)
		{
			this.imageView.Image = this.normalImg;
			this.isMouseIn = false;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x000094A8 File Offset: 0x000076A8
		private void ButtonReleasedHandler(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.Button == 1U || this.isMouseIn)
			{
				if (this.Clicked != null)
				{
					this.imageView.Image = this.normalImg;
					this.Clicked(this, new EventArgs());
				}
			}
		}

		// Token: 0x04000307 RID: 775
		private bool isMouseIn = false;

		// Token: 0x04000308 RID: 776
		private ImageView imageView;

		// Token: 0x04000309 RID: 777
		private Xwt.Drawing.Image normalImg;

		// Token: 0x0400030A RID: 778
		private Xwt.Drawing.Image hoverImg;
	}
}
