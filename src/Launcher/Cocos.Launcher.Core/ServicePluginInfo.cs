using System;
using System.Runtime.Serialization;

namespace Cocos.Launcher.Core
{
	[DataContract]
	public class ServicePluginInfo
	{
		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Type { get; set; }

		[DataMember]
		public string Version { get; set; }

		[DataMember]
		public string DownloadUrl { get; set; }

		[DataMember]
		public bool IsInstalled { get; set; }

		public ServicePluginInfo(string name = null, string type = null, string version = null, bool isInstalled = false)
		{
			this.Name = name;
			this.Type = type;
			this.Version = version;
			this.IsInstalled = isInstalled;
		}
	}
}
