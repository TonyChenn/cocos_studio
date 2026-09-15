using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class BaseComSurrogate : BaseEntitySurrogate
	{
		protected BaseComSurrogate()
		{
		}
	}
}
