using System;
using Gdk;
using Gtk;

namespace Modules.Communal.Skeleton
{
	// Token: 0x02000021 RID: 33
	public class SkinContent : NodeContent
	{
		// Token: 0x06000167 RID: 359 RVA: 0x000080D2 File Offset: 0x000062D2
		public SkinContent(string contentStr = "default") : base(contentStr)
		{
			base.ModifyBg(StateType.Normal, SkinContent.bgColor);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000080E7 File Offset: 0x000062E7
		public override void ResetBgColor()
		{
			this.isChoice = false;
			base.ModifyBg(StateType.Normal, SkinContent.bgColor);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000080FC File Offset: 0x000062FC
		public override void SetEntryBgColor()
		{
			if (this.isChoice)
			{
				return;
			}
			base.ModifyBg(StateType.Normal, SkinContent.movColor);
		}

		// Token: 0x04000079 RID: 121
		private static Color bgColor = new Color(240, 240, 50);

		// Token: 0x0400007A RID: 122
		private static Color movColor = new Color(250, 250, 190);
	}
}
