using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.PropertyGrid;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataInclude(typeof(ScriptFileData))]
	[DataModelExtension(typeof(NodeObject))]
	[DataInclude(typeof(VerticalBerthEdge))]
	[DataInclude(typeof(HorizontalBerthEdge))]
	public class NodeObjectData : AbstractNodeObjectData
	{
		[ItemProperty]
		[PropertyOrder(-2147483648)]
		[JsonProperty]
		public ScaleValue AnchorPoint { get; set; }

		[ItemProperty]
		[JsonProperty]
		public PointF Position { get; set; }

		[JsonProperty]
		[ItemProperty]
		public ScaleValue Scale { get; set; }

		[DefaultValue(0f)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0f)]
		public float RotationSkewX { get; set; }

		[DefaultValue(0f)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0f)]
		public float RotationSkewY { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ColorData CColor { get; set; }

		[ItemProperty]
		[DefaultValue(true)]
		[JsonProperty]
		public bool IconVisible { get; set; }

		[ItemProperty(DefaultValue = false)]
		public bool PrePositionEnabled { get; set; }

		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool PositionPercentXEnabled { get; set; }

		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool PositionPercentYEnabled { get; set; }

		[JsonProperty]
		[ItemProperty]
		public PointF PrePosition { get; set; }

		[ItemProperty(DefaultValue = false)]
		public bool PreSizeEnable { get; set; }

		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		public bool PercentWidthEnable { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		public bool PercentHeightEnable { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		public bool PercentWidthEnabled
		{
			get
			{
				return this.PercentWidthEnable;
			}
			set
			{
			}
		}

		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool PercentHeightEnabled
		{
			get
			{
				return this.PercentHeightEnable;
			}
			set
			{
			}
		}

		[ItemProperty]
		[JsonProperty]
		public SizeF PreSize { get; set; }

		[DefaultValue(HorizontalBerthEdge.None)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = HorizontalBerthEdge.None)]
		public HorizontalBerthEdge HorizontalEdge { get; set; }

		[ItemProperty(DefaultValue = VerticalBerthEdge.None)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(VerticalBerthEdge.None)]
		public VerticalBerthEdge VerticalEdge { get; set; }

		[ItemProperty(DefaultValue = 0f)]
		[DefaultValue(0f)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public float LeftMargin { get; set; }

		[DefaultValue(0f)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0f)]
		public float RightMargin { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0f)]
		[DefaultValue(0f)]
		public float TopMargin { get; set; }

		[ItemProperty(DefaultValue = 0f)]
		[DefaultValue(0f)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public float BottomMargin { get; set; }

		public NodeObjectData()
		{
			base.Alpha = 255;
			this.RotationSkewX = 0f;
			this.RotationSkewY = 0f;
			base.VisibleForFrame = true;
			this.Scale = new ScaleValue(1f, 1f, 0.1, -99999999.0, 99999999.0);
			this.Position = new PointF(0f, 0f);
			this.CColor = new ColorData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			this.AnchorPoint = new ScaleValue(0.5f, 0.5f, 0.1, -99999999.0, 99999999.0);
			this.PreSize = new SizeF();
			base.Size = new SizeF();
			this.PrePosition = new PointF();
		}
	}
}
