using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using CocoStudio.Model.DataModel;
using EditorCommon.Editor;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ComPropertySurrogate : BaseComSurrogate
	{
		[DataMember]
		public ObservableCollection<CustomPropertyModel> keyvalues { get; set; }

		[DataMember]
		public ResourceDataSurrogate fileData { get; protected set; }

		public ComPropertySurrogate()
		{
			this.classname = "CCComAttribute";
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
		}

		protected override object CreateModelObject()
		{
			return new NodeObjectData();
		}
	}
}
