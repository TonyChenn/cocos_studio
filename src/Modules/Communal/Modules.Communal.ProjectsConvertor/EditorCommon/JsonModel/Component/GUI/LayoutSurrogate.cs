using System;
using System.Runtime.Serialization;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class LayoutSurrogate : BaseEntitySurrogate
	{
		[DataMember]
		public int type { get; set; }

		[DataMember]
		public int gravity { get; set; }

		[DataMember]
		public string relativeName { get; set; }

		[DataMember]
		public string relativeToName { get; set; }

		[DataMember]
		public int align { get; set; }

		[DataMember]
		public int marginLeft { get; set; }

		[DataMember]
		public int marginTop { get; set; }

		[DataMember]
		public int marginRight { get; set; }

		[DataMember]
		public int marginDown { get; set; }

		[DataMember]
		public int layoutEageType { get; set; }

		[DataMember]
		public int layoutNormalHorizontal { get; set; }

		[DataMember]
		public int layoutNormalVertical { get; set; }

		[DataMember]
		public int layoutParentHorizontal { get; set; }

		[DataMember]
		public int layoutParentVertical { get; set; }

		protected LayoutSurrogate()
		{
		}

		public LayoutSurrogate(string className)
		{
			this.classname = this.classname;
		}

		public override void SetValue(object obj)
		{
		}
	}
}
