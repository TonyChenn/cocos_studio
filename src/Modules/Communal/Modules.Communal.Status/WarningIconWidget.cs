using System;
using Gtk;

namespace Modules.Communal.Status
{
	// Token: 0x02000009 RID: 9
	internal class WarningIconWidget : EventBox
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00003CAC File Offset: 0x00001EAC
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

		// Token: 0x06000035 RID: 53 RVA: 0x00003D24 File Offset: 0x00001F24
		private void ButtonPressedHandler(object o, ButtonPressEventArgs args)
		{
			this.warningInfo.OnClick();
		}

		// Token: 0x04000023 RID: 35
		private IStatusWarningInfo warningInfo;
	}
}
