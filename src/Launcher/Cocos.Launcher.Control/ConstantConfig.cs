using System;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Control
{
	public static class ConstantConfig
	{
		public static IConstsLink Constant { get; private set; }

		public static ConstsColor Colors { get; private set; }

		public static ConstsPath Paths { get; private set; }

		static ConstantConfig()
		{
			if (LanguageOption.CurrentLanguage == LanguageType.Chinese)
			{
				ConstantConfig.Constant = new ConstsLink();
			}
			else if (LanguageOption.CurrentLanguage == LanguageType.Traditional)
			{
				ConstantConfig.Constant = new ConstsLinkZhTW();
			}
			else
			{
				ConstantConfig.Constant = new ENConstsLink();
			}
			ConstantConfig.Colors = new ConstsColor();
			ConstantConfig.Paths = new ConstsPath();
		}
	}
}
