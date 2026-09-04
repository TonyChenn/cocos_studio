using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using CocoStudio.Projects;
using CocoStudio.Projects.ExtensionModel;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x02000026 RID: 38
	[Extension(Type = typeof(ISolutionUpgrader))]
	internal class SolutionUpgrader_221 : SolutionUpgrader
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000623F File Offset: 0x0000443F
		public override Version Version
		{
			get
			{
				return SolutionUpgrader_221.version;
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006248 File Offset: 0x00004448
		protected override bool OnUpgrade(Solution sln)
		{
			bool result = false;
			if (this.RewriteRecentPublishRecord(sln))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006264 File Offset: 0x00004464
		private bool RewriteRecentPublishRecord(Solution sln)
		{
			string filePath = UserData.GetFilePath(sln.FileName);
			if (!File.Exists(filePath))
			{
				return false;
			}
			XElement xelement = XElement.Load(filePath);
			if (xelement == null)
			{
				return false;
			}
			xelement = xelement.Element("Properties");
			if (xelement == null)
			{
				return false;
			}
			IEnumerable<XElement> enumerable = xelement.Descendants("Item");
			xelement = null;
			foreach (XElement xelement2 in enumerable)
			{
				if (xelement2.Attribute("Key").Value.Equals("PackageParamsKey"))
				{
					xelement = xelement2;
					break;
				}
			}
			if (xelement == null)
			{
				return false;
			}
			xelement = xelement.Element("Value");
			if (xelement == null)
			{
				return false;
			}
			enumerable = xelement.Elements();
			xelement = null;
			foreach (XElement xelement3 in enumerable)
			{
				if (xelement3.Name.LocalName.Equals("PublishSetting"))
				{
					xelement = xelement3;
					break;
				}
			}
			if (xelement == null)
			{
				return false;
			}
			string value = xelement.Attribute("Value").Value;
			if (string.IsNullOrEmpty(value))
			{
				return false;
			}
			EnumPublishType lastPublishType;
			if (Enum.TryParse<EnumPublishType>(value, out lastPublishType))
			{
				CocosRecentServices.Instance.LastPublishType = lastPublishType;
				return true;
			}
			return false;
		}

		// Token: 0x04000078 RID: 120
		private static readonly Version version = new Version("2.2.1.0");
	}
}
