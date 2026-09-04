using System;
using System.Xml.Linq;

namespace Modules.Communal.AutoUpdate
{
	// Token: 0x0200000B RID: 11
	public class WinServerInfo : ServerUpdateInfo
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000354B File Offset: 0x0000174B
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00003553 File Offset: 0x00001753
		public string SmallPackageLink { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000067 RID: 103 RVA: 0x0000355C File Offset: 0x0000175C
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00003564 File Offset: 0x00001764
		public string SmallPackageSize { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000069 RID: 105 RVA: 0x0000356D File Offset: 0x0000176D
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00003575 File Offset: 0x00001775
		public string SmallPackageMD5 { get; private set; }

		// Token: 0x0600006B RID: 107 RVA: 0x0000357E File Offset: 0x0000177E
		public WinServerInfo(string path) : base(path)
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003588 File Offset: 0x00001788
		protected override bool ReadPlatformProperty(XElement xml)
		{
			XElement xelement = xml.Element("SmallPackageLink");
			if (xelement == null)
			{
				return false;
			}
			this.SmallPackageLink = (string)xelement;
			xelement = xml.Element("SmallPackageSize");
			if (xelement == null)
			{
				return false;
			}
			this.SmallPackageSize = (string)xelement;
			xelement = xml.Element("SmallPackageMD5");
			if (xelement != null)
			{
				this.SmallPackageMD5 = (string)xelement;
				return true;
			}
			return false;
		}
	}
}
