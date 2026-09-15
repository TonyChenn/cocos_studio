using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ComRenderSurrogate : BaseComSurrogate
	{
		[DataMember]
		public string file { get; protected set; }

		[DataMember]
		public ResourceDataSurrogate fileData { get; protected set; }

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
		}
	}
}
