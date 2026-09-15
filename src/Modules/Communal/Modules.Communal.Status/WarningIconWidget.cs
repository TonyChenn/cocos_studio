using System;
using Gtk;

namespace Modules.Communal.Status
{
	internal class WarningIconWidget : EventBox
	{
		public WarningIconWidget(IStatusWarningInfo info)
		{
			if (info != null)
			{
				this.warningInfo = info;
				TooltipIcon tooltipIcon = new TooltipIcon();
				tooltipIcon.Text = info.Tooltip;
				base.Add(tooltipIcon);
				tooltipIcon.SetBackgroundColor(WindowStyle.WindowBgColor);
				base.ModifyBg(StateType.Normal, WindowStyle.WindowBgColor);
				base.ButtonPressEvent += this.ButtonPressedHandler;
				base.ShowAll();
			}
		}

		private void ButtonPressedHandler(object o, ButtonPressEventArgs args)
		{
			this.warningInfo.OnClick();
		}

		private IStatusWarningInfo warningInfo;
	}
}
