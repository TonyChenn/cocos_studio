using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using CocoStudio.Projects;
using CocoStudio.Projects.ExtensionModel;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	[Extension(Type = typeof(ISolutionUpgrader))]
	internal class SolutionUpgrader_221 : SolutionUpgrader
	{
		public override Version Version
		{
			get
			{
				return SolutionUpgrader_221.version;
			}
		}

		protected override bool OnUpgrade(Solution sln)
		{
			bool result = false;
			if (this.RewriteRecentPublishRecord(sln))
			{
				result = true;
			}
			return result;
		}

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

		private static readonly Version version = new Version("2.2.1.0");
	}
}
