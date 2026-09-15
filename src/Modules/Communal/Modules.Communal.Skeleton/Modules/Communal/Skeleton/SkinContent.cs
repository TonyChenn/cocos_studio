using System;
using Gdk;
using Gtk;

namespace Modules.Communal.Skeleton
{
	public class SkinContent : NodeContent
	{
		public SkinContent(string contentStr = "default") : base(contentStr)
		{
			base.ModifyBg(StateType.Normal, SkinContent.bgColor);
		}

		public override void ResetBgColor()
		{
			this.isChoice = false;
			base.ModifyBg(StateType.Normal, SkinContent.bgColor);
		}

		public override void SetEntryBgColor()
		{
			if (this.isChoice)
			{
				return;
			}
			base.ModifyBg(StateType.Normal, SkinContent.movColor);
		}

		private static Color bgColor = new Color(240, 240, 50);

		private static Color movColor = new Color(250, 250, 190);
	}
}
