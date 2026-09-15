using System;
using System.IO;
using System.Xml.Linq;
using CocoStudio.Basic;

namespace Modules.Communal.AutoUpdate
{
	public class LocalUpdateConfig
	{
		public string AppVersion { get; set; }

		public string RuntimeVersion { get; set; }

		public DateTime InfoTime { get; set; }

		public bool IsNeverRemind { get; set; }

		public bool IsSkipToday { get; set; }

		public LocalUpdateConfig()
		{
			this.InfoTime = DateTime.Now;
			this.IsNeverRemind = false;
			this.IsSkipToday = false;
			this.AppVersion = "0.0.1";
			this.RuntimeVersion = "0.0.1";
			this.ReadFromFile();
		}

		public LocalUpdateConfig(string version)
		{
			this.AppVersion = version;
		}

		private void ReadFromFile()
		{
			if (File.Exists(PathHelper.UpdateLocalConfigPath))
			{
				try
				{
					XElement xelement = XElement.Load(PathHelper.UpdateLocalConfigPath);
					XElement xelement2 = xelement.Element("InfoTime");
					if (xelement2 != null)
					{
						this.InfoTime = (DateTime)xelement2;
					}
					xelement2 = xelement.Element("IsNeverRemind");
					if (xelement2 != null)
					{
						this.IsNeverRemind = (bool)xelement2;
					}
					xelement2 = xelement.Element("IsSkipToday");
					if (xelement2 != null)
					{
						this.IsSkipToday = (bool)xelement2;
					}
					xelement2 = xelement.Element("AppVersion");
					if (xelement2 != null && !string.IsNullOrEmpty((string)xelement2))
					{
						this.AppVersion = (string)xelement2;
					}
					xelement2 = xelement.Element("RuntimeVersion");
					if (xelement2 != null && !string.IsNullOrEmpty((string)xelement2))
					{
						this.RuntimeVersion = (string)xelement2;
					}
				}
				catch (Exception message)
				{
					LogConfig.Logger.Error(message);
				}
			}
		}

		public void SaveToFile()
		{
			try
			{
				this.InfoTime = DateTime.Now;
				XElement xelement = new XElement("UpdateLocalConfig", new object[]
				{
					new XElement("InfoTime", this.InfoTime),
					new XElement("IsNeverRemind", this.IsNeverRemind),
					new XElement("IsSkipToday", this.IsSkipToday),
					new XElement("AppVersion", this.AppVersion),
					new XElement("RuntimeVersion", this.RuntimeVersion)
				});
				xelement.Save(PathHelper.UpdateLocalConfigPath);
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
			}
		}
	}
}
