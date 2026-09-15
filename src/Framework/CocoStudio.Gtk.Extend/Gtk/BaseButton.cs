using System;
using System.Diagnostics;
using CocoStudio.Basic;

namespace Gtk
{
	public class BaseButton : EventBox
	{
		protected bool IsMouseIn { get; private set; }

		protected bool IsMouseInWhenPressed { get; private set; }

		protected ButtonState CurrentState { get; private set; }

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

		public bool AllowRightClick { get; set; }

		public object ContentTag { get; set; }

		public string URL { get; set; }

		public event EventHandler<ButtonReleaseEventArgs> Clicked;

		public BaseButton()
		{
			this.CurrentState = ButtonState.Normal;
			base.VisibleWindow = false;
			base.EnterNotifyEvent += this.MouseEnteredHandler;
			base.LeaveNotifyEvent += this.MouseLeavedHandler;
			base.ButtonPressEvent += this.MousePressedHandler;
			base.ButtonReleaseEvent += this.MouseReleasedHandler;
		}

		protected void RefreshUI()
		{
			this.OnRefreshUI();
		}

		protected virtual void OnRefreshUI()
		{
		}

		protected virtual void OnButtonClicked()
		{
			this.OpenWebUrl();
		}

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

		protected void RaiseClickedEvent(ButtonReleaseEventArgs args)
		{
			if (this.Clicked != null)
			{
				this.Clicked(this, args);
			}
		}

		protected void MouseEnteredHandler(object o, EnterNotifyEventArgs args)
		{
			this.IsMouseIn = true;
			this.CurrentState = ButtonState.Hover;
			this.RefreshUI();
		}

		protected void MouseLeavedHandler(object o, LeaveNotifyEventArgs args)
		{
			this.IsMouseIn = false;
			this.CurrentState = ButtonState.Normal;
			this.RefreshUI();
		}

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
