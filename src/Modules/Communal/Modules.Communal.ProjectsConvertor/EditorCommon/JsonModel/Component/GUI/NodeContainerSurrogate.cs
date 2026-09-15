using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class NodeContainerSurrogate : WidgetSurrogate
	{
		protected NodeContainerSurrogate()
		{
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
		}
	}
}
