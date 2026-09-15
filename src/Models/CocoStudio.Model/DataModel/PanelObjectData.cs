using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(PanelObject))]
	public class PanelObjectData : WidgetObjectData
	{
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		public bool ClipAble { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(255)]
		[ItemProperty(DefaultValue = 255)]
		public int BackColorAlpha { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData FileData { get; set; }

		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0)]
		public int ComboBoxIndex { get; set; }

		[JsonProperty]
		[ItemProperty]
		public ColorData SingleColor { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ColorData FirstColor { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ColorData EndColor { get; set; }

		[JsonProperty]
		[ItemProperty]
		public ScaleValue ColorVector { get; set; }

		[ItemProperty(DefaultValue = 0)]
		public float ColorAngle { get; set; }

		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool Scale9Enable { get; set; }

		[ItemProperty(DefaultValue = 0)]
		public int LeftEage { get; set; }

		[ItemProperty(DefaultValue = 0)]
		public int RightEage { get; set; }

		[ItemProperty(DefaultValue = 0)]
		public int TopEage { get; set; }

		[ItemProperty(DefaultValue = 0)]
		public int BottomEage { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(0)]
		[ItemProperty(DefaultValue = 0)]
		public int Scale9OriginX { get; set; }

		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Scale9OriginY { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		public int Scale9Width { get; set; }

		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Scale9Height { get; set; }

		public PanelObjectData()
		{
			this.BackColorAlpha = 255;
			this.SingleColor = Color.FromArgb(255, 0, 0, 0);
		}

		public PanelObjectData(bool bWithColor) : this()
		{
			this.ComboBoxIndex = (bWithColor ? 1 : 0);
		}
	}
}
