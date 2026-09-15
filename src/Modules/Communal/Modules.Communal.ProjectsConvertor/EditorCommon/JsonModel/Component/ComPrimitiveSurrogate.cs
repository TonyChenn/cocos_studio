using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ComPrimitiveSurrogate : BaseComSurrogate
	{
		public ComPrimitiveSurrogate()
		{
			this.classname = "PrimitiveComponent";
		}
	}
}
