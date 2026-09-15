using System;

namespace Cocos.Launcher.Core
{
	public class Advert
	{
		public string ImageUrl { get; set; }

		public string Url { get; set; }

		public Advert()
		{
			this.ImageUrl = (this.Url = string.Empty);
		}
	}
}
