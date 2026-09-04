using System;
using System.IO;
using System.Net;
using System.Xml.Linq;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;

namespace Modules.Communal.AutoUpdate
{
	// Token: 0x02000009 RID: 9
	public abstract class ServerUpdateInfo
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003024 File Offset: 0x00001224
		// (set) Token: 0x0600003E RID: 62 RVA: 0x0000302C File Offset: 0x0000122C
		public string AppName { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00003035 File Offset: 0x00001235
		// (set) Token: 0x06000040 RID: 64 RVA: 0x0000303D File Offset: 0x0000123D
		public string AppVersion { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003046 File Offset: 0x00001246
		// (set) Token: 0x06000042 RID: 66 RVA: 0x0000304E File Offset: 0x0000124E
		public string DisplayVersion { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00003057 File Offset: 0x00001257
		// (set) Token: 0x06000044 RID: 68 RVA: 0x0000305F File Offset: 0x0000125F
		public string RequiredMinVersion { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00003068 File Offset: 0x00001268
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00003070 File Offset: 0x00001270
		public string DownloadUrl { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00003079 File Offset: 0x00001279
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00003081 File Offset: 0x00001281
		public string FullPackageUrl { get; private set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000049 RID: 73 RVA: 0x0000308A File Offset: 0x0000128A
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00003092 File Offset: 0x00001292
		public string Desc { get; private set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600004B RID: 75 RVA: 0x0000309B File Offset: 0x0000129B
		// (set) Token: 0x0600004C RID: 76 RVA: 0x000030A3 File Offset: 0x000012A3
		public string PackageSize { get; private set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600004D RID: 77 RVA: 0x000030AC File Offset: 0x000012AC
		// (set) Token: 0x0600004E RID: 78 RVA: 0x000030B4 File Offset: 0x000012B4
		public string MD5Value { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600004F RID: 79 RVA: 0x000030BD File Offset: 0x000012BD
		// (set) Token: 0x06000050 RID: 80 RVA: 0x000030C5 File Offset: 0x000012C5
		public string RuntimeName { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000030CE File Offset: 0x000012CE
		// (set) Token: 0x06000052 RID: 82 RVA: 0x000030D6 File Offset: 0x000012D6
		public string RuntimeVersion { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000053 RID: 83 RVA: 0x000030DF File Offset: 0x000012DF
		// (set) Token: 0x06000054 RID: 84 RVA: 0x000030E7 File Offset: 0x000012E7
		public bool LinkSuccess { get; protected set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000055 RID: 85 RVA: 0x000030F0 File Offset: 0x000012F0
		// (set) Token: 0x06000056 RID: 86 RVA: 0x000030F8 File Offset: 0x000012F8
		public bool LoadSuccess { get; protected set; }

		// Token: 0x06000057 RID: 87 RVA: 0x00003104 File Offset: 0x00001304
		public ServerUpdateInfo(string path)
		{
			this.LinkSuccess = (this.LoadSuccess = false);
			this.ReadFromFile(path);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003130 File Offset: 0x00001330
		private void ReadFromFile(string path)
		{
			try
			{
				Uri requestUri = new Uri(path);
				WebRequest webRequest = WebRequest.Create(requestUri);
				webRequest.Credentials = CredentialCache.DefaultCredentials;
				webRequest.Timeout = 1500;
				HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				this.LinkSuccess = true;
				XElement xelement = XElement.Load(responseStream);
				responseStream.Close();
				if (xelement != null)
				{
					if (this.ReadBaseProperty(xelement))
					{
						if (this.ReadPlatformProperty(xelement))
						{
							this.LoadSuccess = true;
						}
					}
				}
				else
				{
					this.LoadSuccess = false;
				}
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
				this.LoadSuccess = false;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000031E0 File Offset: 0x000013E0
		private bool ReadBaseProperty(XElement xml)
		{
			XElement xelement = xml.Element("AppName");
			if (xelement == null)
			{
				return false;
			}
			this.AppName = (string)xelement;
			xelement = xml.Element("AppVersion");
			if (xelement == null)
			{
				return false;
			}
			this.AppVersion = (string)xelement;
			xelement = xml.Element("RequiredMinVersion");
			if (xelement == null)
			{
				return false;
			}
			this.RequiredMinVersion = (string)xelement;
			xelement = xml.Element("DownloadUrl");
			if (xelement == null)
			{
				return false;
			}
			this.DownloadUrl = (string)xelement;
			xelement = xml.Element("FullPackageUrl");
			if (xelement == null)
			{
				return false;
			}
			this.FullPackageUrl = (string)xelement;
			xelement = xml.Element("PackageSize");
			if (xelement == null)
			{
				return false;
			}
			this.PackageSize = (string)xelement;
			xelement = xml.Element("MD5Value");
			if (xelement == null)
			{
				return false;
			}
			this.MD5Value = (string)xelement;
			xelement = xml.Element("DisplayVersion");
			if (xelement != null)
			{
				this.DisplayVersion = (string)xelement;
			}
			else
			{
				this.DisplayVersion = this.AppVersion;
			}
			xelement = xml.Element("RuntimeName");
			if (xelement == null)
			{
				return false;
			}
			this.RuntimeName = (string)xelement;
			xelement = xml.Element("RuntimeVersion");
			if (xelement != null)
			{
				this.RuntimeVersion = (string)xelement;
				string text;
				if (LanguageOption.CurrentLanguage == LanguageType.Chinese)
				{
					text = (string)xml.Element("ChineseDescription");
				}
				else if (LanguageOption.CurrentLanguage == LanguageType.Traditional)
				{
					text = (string)xml.Element("ChineseTraditionalDescription");
				}
				else
				{
					text = (string)xml.Element("EnglichDescription");
				}
				if (!string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text))
				{
					text = string.Join(Environment.NewLine, text.Split(new char[]
					{
						'\r',
						'\n'
					}, StringSplitOptions.RemoveEmptyEntries));
				}
				this.Desc = text;
				return true;
			}
			return false;
		}

		// Token: 0x0600005A RID: 90
		protected abstract bool ReadPlatformProperty(XElement xml);
	}
}
