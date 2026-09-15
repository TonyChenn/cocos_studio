using System;
using System.Runtime.Serialization;
using Mono.Addins;
using Newtonsoft.Json;

namespace EditorCommon.JsonModel.Component.GUI
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class RootGUISurrogate : WidgetSurrogate
	{
		[JsonIgnore]
		public override float x { get; set; }

		[JsonIgnore]
		public override float y { get; set; }

		[JsonIgnore]
		public override float rotation { get; set; }

		[JsonIgnore]
		public override bool flipX { get; set; }

		[JsonIgnore]
		public override bool flipY { get; set; }

		[JsonIgnore]
		public override int colorR { get; set; }

		[JsonIgnore]
		public override int colorG { get; set; }

		[JsonIgnore]
		public override int colorB { get; set; }

		[JsonIgnore]
		public override int opacity { get; set; }

		[JsonIgnore]
		public override bool touchAble { get; set; }

		[JsonIgnore]
		public override int ZOrder { get; set; }

		[JsonIgnore]
		public override string classType { get; set; }

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
		public override int actionTag { get; set; }

		[JsonIgnore]
		public override int tag { get; set; }

		[JsonIgnore]
		public override float anchorPointX { get; set; }

		[JsonIgnore]
		public override float anchorPointY { get; set; }

		[JsonIgnore]
		public override bool ignoreSize { get; set; }

		[JsonIgnore]
		public override LayoutSurrogate layoutParameter { get; set; }

		[JsonIgnore]
		public override string customProperty { get; set; }

		protected RootGUISurrogate()
		{
			this.classname = "Node";
		}

		public override void SetValue(object obj)
		{
		}
	}
}
