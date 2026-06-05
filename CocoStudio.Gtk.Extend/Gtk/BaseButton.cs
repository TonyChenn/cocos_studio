using System;
using System.Diagnostics;
using CocoStudio.Basic;

namespace Gtk
{
	// Token: 0x02000004 RID: 4
	public class BaseButton : EventBox
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002080 File Offset: 0x00000280
		// (set) Token: 0x06000005 RID: 5 RVA: 0x00002097 File Offset: 0x00000297
		protected bool IsMouseIn { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020A0 File Offset: 0x000002A0
		// (set) Token: 0x06000007 RID: 7 RVA: 0x000020B7 File Offset: 0x000002B7
		protected bool IsMouseInWhenPressed { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020C0 File Offset: 0x000002C0
		// (set) Token: 0x06000009 RID: 9 RVA: 0x000020D7 File Offset: 0x000002D7
		protected ButtonState CurrentState { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000020E0 File Offset: 0x000002E0
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000020F8 File Offset: 0x000002F8
		public new bool Sensitive
		{
			get
			{
				return base.Sensitive;
			}
			set
			{
				base.Sensitive = value;
				this.RefreshUI();
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000210C File Offset: 0x0000030C
		// (set) Token: 0x0600000D RID: 13 RVA: 0x00002123 File Offset: 0x00000323
		public bool AllowRightClick { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000E RID: 14 RVA: 0x0000212C File Offset: 0x0000032C
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002143 File Offset: 0x00000343
		public object ContentTag { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000010 RID: 16 RVA: 0x0000214C File Offset: 0x0000034C
		// (set) Token: 0x06000011 RID: 17 RVA: 0x00002163 File Offset: 0x00000363
		public string URL { get; set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000012 RID: 18 RVA: 0x0000216C File Offset: 0x0000036C
		// (remove) Token: 0x06000013 RID: 19 RVA: 0x000021A8 File Offset: 0x000003A8
		public event EventHandler<ButtonReleaseEventArgs> Clicked;

		// Token: 0x06000014 RID: 20 RVA: 0x000021E4 File Offset: 0x000003E4
		public BaseButton()
		{
			this.CurrentState = ButtonState.Normal;
			base.VisibleWindow = false;
			base.EnterNotifyEvent += this.MouseEnteredHandler;
			base.LeaveNotifyEvent += this.MouseLeavedHandler;
			base.ButtonPressEvent += this.MousePressedHandler;
			base.ButtonReleaseEvent += this.MouseReleasedHandler;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002256 File Offset: 0x00000456
		protected void RefreshUI()
		{
			this.OnRefreshUI();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002260 File Offset: 0x00000460
		protected virtual void OnRefreshUI()
		{
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002263 File Offset: 0x00000463
		protected virtual void OnButtonClicked()
		{
			this.OpenWebUrl();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002270 File Offset: 0x00000470
		private void OpenWebUrl()
		{
			if (!string.IsNullOrWhiteSpace(this.URL))
			{
				try
				{
					Uri uri = new Uri(this.URL, UriKind.RelativeOrAbsolute);
					Process.Start(uri.ToString());
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error(string.Format("打开网址 {0} 时出错", this.URL), exception);
				}
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000022E4 File Offset: 0x000004E4
		protected void RaiseClickedEvent(ButtonReleaseEventArgs args)
		{
			if (this.Clicked != null)
			{
				this.Clicked(this, args);
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000230F File Offset: 0x0000050F
		protected void MouseEnteredHandler(object o, EnterNotifyEventArgs args)
		{
			this.IsMouseIn = true;
			this.CurrentState = ButtonState.Hover;
			this.RefreshUI();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002329 File Offset: 0x00000529
		protected void MouseLeavedHandler(object o, LeaveNotifyEventArgs args)
		{
			this.IsMouseIn = false;
			this.CurrentState = ButtonState.Normal;
			this.RefreshUI();
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002344 File Offset: 0x00000544
		protected void MousePressedHandler(object o, ButtonPressEventArgs args)
		{
			if (args.Event.Button == 1U)
			{
				this.IsMouseInWhenPressed = true;
				this.IsMouseIn = true;
				this.CurrentState = ButtonState.Pressed;
				this.RefreshUI();
				args.RetVal = true;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002394 File Offset: 0x00000594
		protected void MouseReleasedHandler(object o, ButtonReleaseEventArgs args)
		{
			bool flag = false;
			if (args.Event.Button == 1U || (this.AllowRightClick && args.Event.Button == 3U))
			{
				flag = true;
			}
			if (flag)
			{
				if (this.IsMouseIn)
				{
					this.CurrentState = ButtonState.Hover;
				}
				else
				{
					this.CurrentState = ButtonState.Normal;
				}
				this.RefreshUI();
				if (this.IsMouseIn && this.IsMouseInWhenPressed && this.Sensitive)
				{
					this.OnButtonClicked();
					this.RaiseClickedEvent(args);
				}
				this.IsMouseInWhenPressed = false;
			}
		}
	}
}
