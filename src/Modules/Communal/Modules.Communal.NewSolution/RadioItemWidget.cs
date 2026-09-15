using System;
using System.ComponentModel;
using Gdk;
using Gtk;
using Stetic;

namespace Modules.Communal.NewSolution
{
	[ToolboxItem(true)]
	public class RadioItemWidget : Bin, IRadioItem
	{
		public bool UseBgColor { get; set; }

		public Color NormalBgColor { get; set; }

		public Color HoverBgColor { get; set; }

		public Color SelectedBgColor { get; set; }

		public Color BorderColor { get; set; }

		public bool IsSelected { get; private set; }

		public object Tag { get; set; }

		public IRadioItemContent GtkContent { get; private set; }

		public event EventHandler<RadioItemArgs> DoubleClicked;

		public event EventHandler<RadioItemArgs> Selected;

		public void Select()
		{
			this.IsSelected = true;
			if (this.Selected != null)
			{
				RadioItemArgs e = new RadioItemArgs(this);
				this.Selected(this, e);
			}
			this.RefreshUI();
		}

		public void Unselect()
		{
			this.IsSelected = false;
			this.currentState = ButtonState.Normal;
			this.RefreshUI();
		}

		public Widget GetGtkWidget()
		{
			return this;
		}

		public RadioItemWidget()
		{
			this.Build();
			this.UseBgColor = true;
		}

		public void SetContent(IRadioItemContent content)
		{
			this.GtkContent = content;
			Widget gtkWidget = this.GtkContent.GetGtkWidget();
			this.evtbx_bg.Add(gtkWidget);
			this.GtkContent.RefreshUI(this.IsSelected, this.currentState);
			base.ShowAll();
		}

		public void HideBorder()
		{
			this.evtbx_bg.BorderWidth = 0U;
		}

		public void ShowBorder()
		{
			this.evtbx_bg.BorderWidth = 1U;
		}

		private void RefreshUI()
		{
			if (this.UseBgColor)
			{
				if (this.IsSelected)
				{
					this.evtbx_border.ModifyBg(StateType.Normal, this.BorderColor);
				}
				else
				{
					switch (this.currentState)
					{
					case ButtonState.Normal:
						this.evtbx_border.ModifyBg(StateType.Normal, this.NormalBgColor);
						break;
					case ButtonState.Hover:
						this.evtbx_border.ModifyBg(StateType.Normal, this.HoverBgColor);
						break;
					}
				}
				if (this.IsSelected)
				{
					this.evtbx_bg.ModifyBg(StateType.Normal, this.SelectedBgColor);
				}
				else
				{
					switch (this.currentState)
					{
					case ButtonState.Normal:
						this.evtbx_bg.ModifyBg(StateType.Normal, this.NormalBgColor);
						break;
					case ButtonState.Hover:
						this.evtbx_bg.ModifyBg(StateType.Normal, this.HoverBgColor);
						break;
					}
				}
			}
			this.GtkContent.RefreshUI(this.IsSelected, this.currentState);
		}

		private void OnMouseEntered(object o, EnterNotifyEventArgs args)
		{
			this.isMouseIn = true;
			this.currentState = ButtonState.Hover;
			this.RefreshUI();
		}

		private void OnMouseLeaved(object o, LeaveNotifyEventArgs args)
		{
			this.isMouseIn = false;
			this.currentState = ButtonState.Normal;
			this.RefreshUI();
		}

		private void OnMousePressed(object o, ButtonPressEventArgs args)
		{
			if ((args.Event.Type == EventType.TwoButtonPress || args.Event.Type == EventType.ThreeButtonPress) && this.DoubleClicked != null)
			{
				RadioItemArgs e = new RadioItemArgs(this);
				this.DoubleClicked(this, e);
			}
			if (!this.IsSelected)
			{
				this.currentState = ButtonState.Pressed;
				this.Select();
			}
		}

		private void OnMouseReleased(object o, ButtonReleaseEventArgs args)
		{
			if (this.isMouseIn)
			{
				this.currentState = ButtonState.Hover;
			}
			else
			{
				this.currentState = ButtonState.Normal;
			}
			this.RefreshUI();
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.NewSolution.RadioItemWidget";
			this.evtbx_border = new EventBox();
			this.evtbx_border.Name = "evtbx_border";
			this.evtbx_bg = new EventBox();
			this.evtbx_bg.Name = "evtbx_bg";
			this.evtbx_bg.BorderWidth = 1U;
			this.evtbx_border.Add(this.evtbx_bg);
			base.Add(this.evtbx_border);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
			this.evtbx_border.LeaveNotifyEvent += this.OnMouseLeaved;
			this.evtbx_bg.ButtonPressEvent += this.OnMousePressed;
			this.evtbx_bg.ButtonReleaseEvent += this.OnMouseReleased;
			this.evtbx_bg.EnterNotifyEvent += this.OnMouseEntered;
			this.evtbx_bg.LeaveNotifyEvent += this.OnMouseLeaved;
		}

		private bool isMouseIn;

		private ButtonState currentState;

		private EventBox evtbx_border;

		private EventBox evtbx_bg;
	}
}
