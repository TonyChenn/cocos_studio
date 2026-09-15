using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ObjectSurrogate : IExtensibleDataObject
	{
		public ExtensionDataObject ExtensionData
		{
			get
			{
				return this.extensionDataObject_value;
			}
			set
			{
				this.extensionDataObject_value = value;
			}
		}

		protected ObjectSurrogate()
		{
		}

		private ExtensionDataObject extensionDataObject_value;
	}
}
