using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ComSceneSurrogate : BaseComSurrogate
	{
		[DataMember]
		public string scenename { get; set; }

		protected ComSceneSurrogate()
		{
		}

		public ComSceneSurrogate(string scenename)
		{
			this.scenename = scenename;
			this.classname = "CCScene";
			this.name = this.classname;
		}
	}
}
