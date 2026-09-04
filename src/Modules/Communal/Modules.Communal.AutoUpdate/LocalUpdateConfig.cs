using System;
using System.IO;
using System.Xml.Linq;
using CocoStudio.Basic;

namespace Modules.Communal.AutoUpdate
{
	// Token: 0x0200000F RID: 15
	public class LocalUpdateConfig
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003A80 File Offset: 0x00001C80
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00003A88 File Offset: 0x00001C88
		public string AppVersion { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003A91 File Offset: 0x00001C91
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00003A99 File Offset: 0x00001C99
		public string RuntimeVersion { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00003AA2 File Offset: 0x00001CA2
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00003AAA File Offset: 0x00001CAA
		public DateTime InfoTime { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00003AB3 File Offset: 0x00001CB3
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00003ABB File Offset: 0x00001CBB
		public bool IsNeverRemind { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00003AC4 File Offset: 0x00001CC4
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00003ACC File Offset: 0x00001CCC
		public bool IsSkipToday { get; set; }

		// Token: 0x06000088 RID: 136 RVA: 0x00003AD5 File Offset: 0x00001CD5
		public LocalUpdateConfig()
		{
			this.InfoTime = DateTime.Now;
			this.IsNeverRemind = false;
			this.IsSkipToday = false;
			this.AppVersion = "0.0.1";
			this.RuntimeVersion = "0.0.1";
			this.ReadFromFile();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003B12 File Offset: 0x00001D12
		public LocalUpdateConfig(string version)
		{
			this.AppVersion = version;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003B24 File Offset: 0x00001D24
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

		// Token: 0x0600008B RID: 139 RVA: 0x00003C28 File Offset: 0x00001E28
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
