using System;
using Gdk;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace Gtk
{
	public class IconButton : EventBox
	{
		public object Tag { get; set; }

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

		public event EventHandler<ButtonReleaseEventArgs> Clicked;

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

		public IconButton(Xwt.Drawing.Image icon) : this()
		{
			this.normalIcon = icon;
			this.imgView = new ImageView();
			this.bgBox.Add(this.imgView);
			this.OnSetStyle();
			this.OnRefreshUI();
			base.ShowAll();
		}

		public void ChangeImage(Xwt.Drawing.Image icon)
		{
			this.normalIcon = icon;
			this.OnRefreshIcon();
		}

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

		protected virtual void OnClicked(ButtonReleaseEventArgs args)
		{
			if (this.Clicked != null)
			{
				this.Clicked(this, args);
			}
		}

		private void ButtonStateChangedHandler(object o, StateChangedArgs args)
		{
			this.OnRefreshUI();
		}

		private void MouseLeftPressEventHandler(object o, ButtonPressEventArgs args)
		{
			if (args.Event.Button == 1U)
			{
				this.MousePressedHandler(o, args);
			}
		}

		protected void MousePressedHandler(object o, ButtonPressEventArgs args)
		{
			this.isMousePressed = true;
			this.isMouseIn = true;
			base.State = StateType.Selected;
			this.OnRefreshUI();
			args.RetVal = true;
			this.OnMousePressed(args);
		}

		protected virtual void OnMousePressed(ButtonPressEventArgs args)
		{
		}

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

		protected virtual void OnMouseReleased(ButtonReleaseEventArgs args)
		{
		}

		private void MouseEnterEventHandler(object o, EnterNotifyEventArgs args)
		{
			this.isMouseIn = true;
			base.State = StateType.Prelight;
			this.OnRefreshUI();
		}

		private void MouseLeaveEventHandler(object o, LeaveNotifyEventArgs args)
		{
			if (args.Event.Detail != NotifyType.Inferior)
			{
				this.isMouseIn = false;
				base.State = StateType.Normal;
				this.OnRefreshUI();
			}
		}

		private void ExposeEventHandler(object o, ExposeEventArgs args)
		{
			base.ExposeEvent -= this.ExposeEventHandler;
			this.OnRefreshUI();
		}

		private bool isMouseIn;

		protected bool isMousePressed = true;

		protected EventBox bgBox;

		protected ImageView imgView;

		protected Xwt.Drawing.Image normalIcon;

		private Gdk.Color? _NormalBg;
	}
}
