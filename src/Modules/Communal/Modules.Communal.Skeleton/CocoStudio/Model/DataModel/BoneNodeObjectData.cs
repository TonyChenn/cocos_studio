using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(BoneObject))]
	public class BoneNodeObjectData : AbstractNodeObjectData
	{
		[JsonProperty]
		[ItemProperty]
		public float Length { get; set; }

		[ItemProperty]
		[JsonProperty]
		public PointF Position { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ScaleValue Scale { get; set; }

		[ItemProperty]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public float RotationSkewX { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty]
		public float RotationSkewY { get; set; }

		[JsonProperty]
		[ItemProperty]
		public ColorData CColor { get; set; }

		[JsonProperty]
		[ItemProperty]
		public BlendFuncValue BlendFunc { get; set; }

		[ItemProperty]
		public ColorData BoneColor { get; set; }

		public BoneNodeObjectData()
		{
			this.CColor = new ColorData();
			this.BoneColor = new ColorData(byte.MaxValue, 25, 25, 25);
			this.RotationSkewX = 0f;
			this.RotationSkewY = 0f;
			this.Scale = new ScaleValue(1f, 1f, 0.1, -99999999.0, 99999999.0);
			this.Position = new PointF(0f, 0f);
			base.Size = new SizeF(0f, 0f);
		}
	}
}
