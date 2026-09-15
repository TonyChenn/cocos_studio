using System;
using System.ComponentModel;
using Cairo;
using CocoStudio.Basic;
using Gdk;
using MonoDevelop.Components;
using Stetic;
using Xwt.Drawing;

namespace Gtk
{
	[ToolboxItem(true)]
	public class TooltipIcon : Bin
	{
		public string Text
		{
			get
			{
				return this._Text;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this._Text = string.Empty;
				}
				else
				{
					this._Text = value;
				}
			}
		}

		public TooltipIcon()
		{
			this.Build();
			string resourceID = "CocoStudio.DefaultResource.Images.WarningIcon.Studio.png";
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				resourceID = "CocoStudio.DefaultResource.Images.WarningIcon.Launcher.png";
			}
			Xwt.Drawing.Image icon = ImageIcon.GetIcon(resourceID);
			this.imagebin_icon.SetImageView(icon);
		}

		public void ShowTooltip(string text = null)
		{
			string text2 = this.Text;
			if (!string.IsNullOrEmpty(text))
			{
				text2 = text;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				if (this.tooltipWindow != null)
				{
					this.tooltipWindow.Destroy();
				}
				this.tooltipWindow = new TooltipPopoverWindow();
				if (Option.CurrentApp == EnumApp.Launcher)
				{
					this.tooltipWindow.Theme.SetFlatColor(new Cairo.Color(1.0, 0.984375, 0.86328125));
					this.tooltipWindow.Theme.BorderColor = new Cairo.Color(0.99609375, 0.9765625, 0.83203125);
					this.tooltipWindow.Markup = "<b><span color= '#a07b23'>" + text2 + "</span></b>";
				}
				else
				{
					this.tooltipWindow.Theme.SetFlatColor(new Cairo.Color(0.03125, 0.4453125, 0.95703125));
					this.tooltipWindow.Theme.BorderColor = new Cairo.Color(0.0859375, 0.40625, 0.796875);
					this.tooltipWindow.Markup = "<b><span color='white'>" + text2 + "</span></b>";
				}
				this.tooltipWindow.Theme.Padding = 2;
				this.tooltipWindow.ShowPopup(this, PopupPosition.Bottom);
			}
		}

		public void HideTooltip()
		{
			if (this.tooltipWindow != null)
			{
				this.tooltipWindow.Destroy();
			}
			this.tooltipWindow = null;
		}

		public void SetBackgroundColor(Gdk.Color bgColor)
		{
			this.eventbox_base.ModifyBg(StateType.Normal, bgColor);
		}

		protected void EnterEventHandler(object o, EnterNotifyEventArgs args)
		{
			this.ShowTooltip(null);
		}

		protected void LeaveEventHandler(object o, LeaveNotifyEventArgs args)
		{
			this.HideTooltip();
		}

		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Gtk.TooltipIcon";
			this.eventbox_base = new EventBox();
			this.eventbox_base.Name = "eventbox_base";
			this.imagebin_icon = new ImageBin();
			this.imagebin_icon.Events = EventMask.ButtonPressMask;
			this.imagebin_icon.Name = "imagebin_icon";
			this.eventbox_base.Add(this.imagebin_icon);
			base.Add(this.eventbox_base);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
			this.eventbox_base.EnterNotifyEvent += this.EnterEventHandler;
			this.eventbox_base.LeaveNotifyEvent += this.LeaveEventHandler;
		}

		private string _Text;

		private TooltipPopoverWindow tooltipWindow;

		private EventBox eventbox_base;

		private ImageBin imagebin_icon;
	}
}
