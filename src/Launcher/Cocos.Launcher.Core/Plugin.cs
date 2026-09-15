using System;
using System.Linq;
using System.Xml.Serialization;

namespace Cocos.Launcher.Core
{
	public class Plugin
	{
		[XmlElement("Name")]
		public string PluginName { get; set; }

		public string PluginPath { get; set; }

		[XmlElement("DownloadUrl")]
		public string PluginUrl
		{
			get
			{
				return this.pluginUrl;
			}
			set
			{
				this.pluginUrl = value;
				if (!string.IsNullOrEmpty(this.pluginUrl))
				{
					this.PluginFullName = this.pluginUrl.Split(new char[]
					{
						'/'
					}).LastOrDefault<string>();
				}
			}
		}

		public string PluginFullName
		{
			get
			{
				return this.pluginFullName;
			}
			set
			{
				this.pluginFullName = value;
			}
		}

		public float PluginFraction { get; set; }

		public float PluginSize { get; set; }

		public string ImageUrl { get; set; }

		public string ImagePath { get; set; }

		public bool IsLoading { get; set; }

		public string OpenType { get; set; }

		public double Score { get; set; }

		[XmlElement("Type")]
		public string PluginType { get; set; }

		public string PluginVersion { get; set; }

		public string Action { get; set; }

		public string DateTime { get; set; }

		public string UninstallName { get; set; }

		public bool IsUninstall { get; set; }

		public string MacPath { get; set; }

		public bool IsInstalled { get; set; }

		public string ProcedureExeName { get; set; }

		public string UnZipPath { get; set; }

		[XmlIgnore]
		public string PluginMD5 { get; set; }

		[XmlIgnore]
		public bool IsInstalling { get; set; }

		[XmlIgnore]
		public bool IsUninstalling { get; set; }

		[XmlIgnore]
		public string VersionFromService { get; set; }

		[XmlIgnore]
		public string DownloadUrlFromService { get; set; }

		public Plugin()
		{
			this.PluginName = (this.PluginUrl = (this.ImageUrl = (this.ImagePath = (this.PluginFullName = (this.PluginType = (this.PluginVersion = (this.DateTime = (this.PluginMD5 = string.Empty))))))));
			this.UninstallName = (this.MacPath = (this.ProcedureExeName = (this.UnZipPath = (this.VersionFromService = (this.DownloadUrlFromService = string.Empty)))));
			this.PluginFraction = 0f;
			this.PluginSize = 0f;
			this.IsLoading = false;
			this.OpenType = string.Empty;
			this.Action = ActionType.none.ToString();
			this.Score = 0.0;
		}

		private string pluginUrl;

		private string pluginFullName;
	}
}
