using System;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;

namespace Gtk
{
	// Token: 0x02000006 RID: 6
	public class HelpButton : ImageButton
	{
		// Token: 0x06000029 RID: 41 RVA: 0x00002680 File Offset: 0x00000880
		public HelpButton(string url = null)
		{
			base.WidthRequest = (base.HeightRequest = 16);
			base.TooltipText = LanguageInfo.CCBReport_toggleButtonHelp;
			string images;
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				images = "CocoStudio.DefaultResource.Images.HelpIcon.Launcher{0}.png";
			}
			else
			{
				images = "CocoStudio.DefaultResource.Images.HelpIcon.{0}.png";
			}
			base.SetImages(images);
			base.URL = url;
		}
	}
}
