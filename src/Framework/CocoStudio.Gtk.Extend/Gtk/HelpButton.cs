using System;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;

namespace Gtk
{
	public class HelpButton : ImageButton
	{
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
