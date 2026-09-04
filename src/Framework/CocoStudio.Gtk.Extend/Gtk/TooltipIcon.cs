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
	// Token: 0x020000A5 RID: 165
	[ToolboxItem(true)]
	public class TooltipIcon : Bin
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600039E RID: 926 RVA: 0x000129DC File Offset: 0x00010BDC
		// (set) Token: 0x0600039F RID: 927 RVA: 0x000129F4 File Offset: 0x00010BF4
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

		// Token: 0x060003A0 RID: 928 RVA: 0x00012A24 File Offset: 0x00010C24
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

		// Token: 0x060003A1 RID: 929 RVA: 0x00012A70 File Offset: 0x00010C70
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

		// Token: 0x060003A2 RID: 930 RVA: 0x00012C00 File Offset: 0x00010E00
		public void HideTooltip()
		{
			if (this.tooltipWindow != null)
			{
				this.tooltipWindow.Destroy();
			}
			this.tooltipWindow = null;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00012C2E File Offset: 0x00010E2E
		public void SetBackgroundColor(Gdk.Color bgColor)
		{
			this.eventbox_base.ModifyBg(StateType.Normal, bgColor);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00012C3F File Offset: 0x00010E3F
		protected void EnterEventHandler(object o, EnterNotifyEventArgs args)
		{
			this.ShowTooltip(null);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00012C4A File Offset: 0x00010E4A
		protected void LeaveEventHandler(object o, LeaveNotifyEventArgs args)
		{
			this.HideTooltip();
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00012C54 File Offset: 0x00010E54
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

		// Token: 0x0400044F RID: 1103
		private string _Text;

		// Token: 0x04000450 RID: 1104
		private TooltipPopoverWindow tooltipWindow;

		// Token: 0x04000451 RID: 1105
		private EventBox eventbox_base;

		// Token: 0x04000452 RID: 1106
		private ImageBin imagebin_icon;
	}
}
