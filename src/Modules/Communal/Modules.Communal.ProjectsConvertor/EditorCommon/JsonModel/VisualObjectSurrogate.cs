using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class VisualObjectSurrogate : BaseEntitySurrogate
	{
		protected VisualObjectSurrogate()
		{
		}
	}
}
