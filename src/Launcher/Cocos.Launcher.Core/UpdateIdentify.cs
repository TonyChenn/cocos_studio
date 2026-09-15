using System;

namespace Cocos.Launcher.Core
{
	public class UpdateIdentify
	{
		public string TutorialTime { get; set; }

		public string ToolsTime { get; set; }

		public UpdateIdentify()
		{
			this.InitDefaulValue();
		}

		private void InitDefaulValue()
		{
			this.TutorialTime = (this.ToolsTime = string.Empty);
		}
	}
}
