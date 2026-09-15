using System;

namespace CocoStudio.Model.ViewModel
{
	public class FrameChangeTimelineRenderArgs : EventArgs
	{
		public FrameChangeTimelineRenderArgs(bool isUnRedoing, bool includeEasignPanel = false)
		{
			this.IsUnRedoing = isUnRedoing;
			this.IncludeEasingPanel = includeEasignPanel;
		}

		public bool IncludeEasingPanel = false;

		public bool IsUnRedoing = false;
	}
}
