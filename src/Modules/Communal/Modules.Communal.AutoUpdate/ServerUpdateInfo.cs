using System;
using System.IO;
using System.Net;
using System.Xml.Linq;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;

namespace Modules.Communal.AutoUpdate
{
	public abstract class ServerUpdateInfo
	{
		public string AppName { get; private set; }

		public string AppVersion { get; private set; }

		public string DisplayVersion { get; private set; }

		public string RequiredMinVersion { get; private set; }

		public string DownloadUrl { get; private set; }

		public string FullPackageUrl { get; private set; }

		public string Desc { get; private set; }

		public string PackageSize { get; private set; }

		public string MD5Value { get; private set; }

		public string RuntimeName { get; private set; }

		public string RuntimeVersion { get; private set; }

		public bool LinkSuccess { get; protected set; }

		public bool LoadSuccess { get; protected set; }

		public ServerUpdateInfo(string path)
		{
			this.LinkSuccess = (this.LoadSuccess = false);
			this.ReadFromFile(path);
		}

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

		protected abstract bool ReadPlatformProperty(XElement xml);
	}
}
