using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ResourceDataSurrogate : ObjectSurrogate
	{
		[DefaultValue(-1)]
		[DataMember]
		public int resourceType { get; private set; }

		[DataMember]
		public string path { get; private set; }

		[DataMember]
		public string plistFile { get; private set; }

		protected ResourceDataSurrogate()
		{
		}
	}
}
