using System;
using Gdk;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace Gtk
{
	// Token: 0x02000007 RID: 7
	public class IconButton : EventBox
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000026E4 File Offset: 0x000008E4
		// (set) Token: 0x0600002B RID: 43 RVA: 0x000026FB File Offset: 0x000008FB
		public object Tag { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002704 File Offset: 0x00000904
		// (set) Token: 0x0600002D RID: 45 RVA: 0x0000271C File Offset: 0x0000091C
		public Gdk.Color? NormalBg
		{
			get
			{
				return this._NormalBg;
			}
			set
			{
				this._NormalBg = value;
				if (this._NormalBg != null)
				{
					base.ModifyBg(StateType.Normal, this._NormalBg.Value);
					this.bgBox.ModifyBg(StateType.Normal, this._NormalBg.Value);
				}
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600002E RID: 46 RVA: 0x00002770 File Offset: 0x00000970
		// (remove) Token: 0x0600002F RID: 47 RVA: 0x000027AC File Offset: 0x000009AC
		public event EventHandler<ButtonReleaseEventArgs> Clicked;

		// Token: 0x06000030 RID: 48 RVA: 0x000027E8 File Offset: 0x000009E8
		protected IconButton()
		{
			this.bgBox = new EventBox();
			base.Add(this.bgBox);
			this.bgBox.BorderWidth = 1U;
			base.SetSizeRequest(24, 24);
			base.StateChanged += this.ButtonStateChangedHandler;
			base.ExposeEvent += this.ExposeEventHandler;
			this.bgBox.EnterNotifyEvent += this.MouseEnterEventHandler;
			this.bgBox.LeaveNotifyEvent += this.MouseLeaveEventHandler;
			this.bgBox.ButtonPressEvent += this.MouseLeftPressEventHandler;
			this.bgBox.ButtonReleaseEvent += this.MouseLeftReleaseEventHandler;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000028BC File Offset: 0x00000ABC
		public IconButton(Xwt.Drawing.Image icon) : this()
		{
			this.normalIcon = icon;
			this.imgView = new ImageView();
			this.bgBox.Add(this.imgView);
			this.OnSetStyle();
			this.OnRefreshUI();
			base.ShowAll();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000290B File Offset: 0x00000B0B
		public void ChangeImage(Xwt.Drawing.Image icon)
		{
			this.normalIcon = icon;
			this.OnRefreshIcon();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000291C File Offset: 0x00000B1C
		protected virtual void OnSetStyle()
		{
			base.ModifyBg(StateType.Normal, WindowStyle.WindowBgColor);
			this.bgBox.ModifyBg(StateType.Normal, WindowStyle.WindowBgColor);
			base.ModifyBg(StateType.Prelight, WindowStyle.WindowBlueColor);
			this.bgBox.ModifyBg(StateType.Prelight, WindowStyle.WindowBgColor);
			base.ModifyBg(StateType.Selected, WindowStyle.WindowBlueColor);
			this.bgBox.ModifyBg(StateType.Selected, WindowStyle.LineDarkColor);
			base.ModifyBg(StateType.Insensitive, WindowStyle.WindowPanelColor);
			this.bgBox.ModifyBg(StateType.Insensitive, WindowStyle.WindowPanelColor);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000029A8 File Offset: 0x00000BA8
		protected virtual void OnRefreshUI()
		{
			this.OnRefreshIcon();
			if (base.State == StateType.Insensitive)
			{
				base.VisibleWindow = (this.bgBox.VisibleWindow = false);
			}
			else
			{
				base.VisibleWindow = (this.bgBox.VisibleWindow = true);
				this.RefreshBackgroundColor();
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002A08 File Offset: 0x00000C08
		protected void RefreshBackgroundColor()
		{
			if (this.NormalBg == null)
			{
				Widget parent;
				for (parent = base.Parent; parent != null; parent = parent.Parent)
				{
					EventBox eventBox = parent as EventBox;
					if (eventBox != null && eventBox.VisibleWindow)
					{
						break;
					}
					if (parent is Window)
					{
						break;
					}
				}
				if (parent != null)
				{
					this.NormalBg = new Gdk.Color?(parent.Style.Backgrounds[0]);
				}
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002AA8 File Offset: 0x00000CA8
		protected virtual void OnRefreshIcon()
		{
			if (this.normalIcon != null)
			{
				if (base.State == StateType.Insensitive)
				{
					this.imgView.Image = this.normalIcon.WithAlpha(0.5);
				}
				else
				{
					this.imgView.Image = this.normalIcon;
				}
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002B0C File Offset: 0x00000D0C
		protected virtual void OnClicked(ButtonReleaseEventArgs args)
		{
			if (this.Clicked != null)
			{
				this.Clicked(this, args);
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002B35 File Offset: 0x00000D35
		private void ButtonStateChangedHandler(object o, StateChangedArgs args)
		{
			this.OnRefreshUI();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002B40 File Offset: 0x00000D40
		private void MouseLeftPressEventHandler(object o, ButtonPressEventArgs args)
		{
			if (args.Event.Button == 1U)
			{
				this.MousePressedHandler(o, args);
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002B6B File Offset: 0x00000D6B
		protected void MousePressedHandler(object o, ButtonPressEventArgs args)
		{
			this.isMousePressed = true;
			this.isMouseIn = true;
			base.State = StateType.Selected;
			this.OnRefreshUI();
			args.RetVal = true;
			this.OnMousePressed(args);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002BA0 File Offset: 0x00000DA0
		protected virtual void OnMousePressed(ButtonPressEventArgs args)
		{
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002BA4 File Offset: 0x00000DA4
		private void MouseLeftReleaseEventHandler(object o, ButtonReleaseEventArgs args)
		{
			if (args.Event.Button == 1U)
			{
				this.isMousePressed = false;
				if (this.isMouseIn)
				{
					base.State = StateType.Prelight;
				}
				else
				{
					base.State = StateType.Normal;
				}
				this.OnRefreshUI();
				if (this.isMouseIn && base.Sensitive)
				{
					this.OnClicked(args);
				}
				this.OnMouseReleased(args);
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002C1E File Offset: 0x00000E1E
		protected virtual void OnMouseReleased(ButtonReleaseEventArgs args)
		{
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002C21 File Offset: 0x00000E21
		private void MouseEnterEventHandler(object o, EnterNotifyEventArgs args)
		{
			this.isMouseIn = true;
			base.State = StateType.Prelight;
			this.OnRefreshUI();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002C3C File Offset: 0x00000E3C
		private void MouseLeaveEventHandler(object o, LeaveNotifyEventArgs args)
		{
			if (args.Event.Detail != NotifyType.Inferior)
			{
				this.isMouseIn = false;
				base.State = StateType.Normal;
				this.OnRefreshUI();
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002C77 File Offset: 0x00000E77
		private void ExposeEventHandler(object o, ExposeEventArgs args)
		{
			base.ExposeEvent -= this.ExposeEventHandler;
			this.OnRefreshUI();
		}

		// Token: 0x04000014 RID: 20
		private bool isMouseIn;

		// Token: 0x04000015 RID: 21
		protected bool isMousePressed = true;

		// Token: 0x04000016 RID: 22
		protected EventBox bgBox;

		// Token: 0x04000017 RID: 23
		protected ImageView imgView;

		// Token: 0x04000018 RID: 24
		protected Xwt.Drawing.Image normalIcon;

		// Token: 0x04000019 RID: 25
		private Gdk.Color? _NormalBg;
	}
}
