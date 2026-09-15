using System;
using System.Xml.Linq;

namespace Modules.Communal.AutoUpdate
{
	public class WinServerInfo : ServerUpdateInfo
	{
		public string SmallPackageLink { get; private set; }

		public string SmallPackageSize { get; private set; }

		public string SmallPackageMD5 { get; private set; }

		public WinServerInfo(string path) : base(path)
		{
		}

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
