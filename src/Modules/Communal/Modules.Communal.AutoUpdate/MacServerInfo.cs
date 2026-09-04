using System;
using System.Xml.Linq;
using Modules.Communal.MultiLanguage;

namespace Modules.Communal.AutoUpdate
{
	// Token: 0x0200000A RID: 10
	public class MacServerInfo : ServerUpdateInfo
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000033F1 File Offset: 0x000015F1
		// (set) Token: 0x0600005C RID: 92 RVA: 0x000033F9 File Offset: 0x000015F9
		public string RuntimeDownloadLink { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00003402 File Offset: 0x00001602
		// (set) Token: 0x0600005E RID: 94 RVA: 0x0000340A File Offset: 0x0000160A
		public string RuntimeDesc { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003413 File Offset: 0x00001613
		// (set) Token: 0x06000060 RID: 96 RVA: 0x0000341B File Offset: 0x0000161B
		public string RuntimeSize { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00003424 File Offset: 0x00001624
		// (set) Token: 0x06000062 RID: 98 RVA: 0x0000342C File Offset: 0x0000162C
		public string RuntimeMD5 { get; private set; }

		// Token: 0x06000063 RID: 99 RVA: 0x00003435 File Offset: 0x00001635
		public MacServerInfo(string path) : base(path)
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003440 File Offset: 0x00001640
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
