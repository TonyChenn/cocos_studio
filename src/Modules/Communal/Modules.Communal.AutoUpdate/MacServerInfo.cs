using System;
using System.Xml.Linq;
using Modules.Communal.MultiLanguage;

namespace Modules.Communal.AutoUpdate
{
	public class MacServerInfo : ServerUpdateInfo
	{
		public string RuntimeDownloadLink { get; private set; }

		public string RuntimeDesc { get; private set; }

		public string RuntimeSize { get; private set; }

		public string RuntimeMD5 { get; private set; }

		public MacServerInfo(string path) : base(path)
		{
		}

		protected override bool ReadPlatformProperty(XElement xml)
		{
			XElement xelement = xml.Element("RuntimeDownloadLink");
			if (xelement == null)
			{
				return false;
			}
			this.RuntimeDownloadLink = (string)xelement;
			xelement = xml.Element("RuntimeSize");
			if (xelement == null)
			{
				return false;
			}
			this.RuntimeSize = (string)xelement;
			xelement = xml.Element("RuntimeMD5");
			if (xelement != null)
			{
				this.RuntimeMD5 = (string)xelement;
				string text;
				if (LanguageOption.CurrentLanguage == LanguageType.Chinese)
				{
					text = (string)xml.Element("RuntimeDescriptionCHS");
				}
				else if (LanguageOption.CurrentLanguage == LanguageType.Traditional)
				{
					text = (string)xml.Element("RuntimeDescriptionCHT");
				}
				else
				{
					text = (string)xml.Element("RuntimeDescriptionEN");
				}
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text))
				{
					text = string.Join(Environment.NewLine, text.Split(new char[]
					{
						'\r',
						'\n'
					}, StringSplitOptions.RemoveEmptyEntries));
				}
				this.RuntimeDesc = text;
				return true;
			}
			return false;
		}
	}
}
