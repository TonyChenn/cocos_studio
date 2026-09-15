using System;
using System.Runtime.Serialization;
using Mono.Addins;
using Newtonsoft.Json;

namespace EditorCommon.JsonModel.Component.GUI
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class SpriteSurrogate : WidgetSurrogate
	{
		[JsonIgnore]
		public override bool touchAble { get; set; }

		[JsonIgnore]
		public override int positionType { get; set; }

		[JsonIgnore]
		public override float positionPercentX { get; set; }

		[JsonIgnore]
		public override float positionPercentY { get; set; }

		[JsonIgnore]
		public override int sizeType { get; set; }

		[JsonIgnore]
		public override float sizePercentX { get; set; }

		[JsonIgnore]
		public override float sizePercentY { get; set; }

		[JsonIgnore]
		public override bool useMergedTexture { get; set; }

		[JsonIgnore]
		public override bool ignoreSize { get; set; }

		[JsonIgnore]
		public override LayoutSurrogate layoutParameter { get; set; }

		[JsonIgnore]
		public override string customProperty { get; set; }

		[DataMember]
		public string fileName { get; set; }

		[DataMember]
		public ResourceDataSurrogate fileNameData { get; set; }

		protected SpriteSurrogate()
		{
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
		}
	}
}
