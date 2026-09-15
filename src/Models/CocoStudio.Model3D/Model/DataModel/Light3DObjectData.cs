using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(Light3DObject))]
	public class Light3DObjectData : Node3DObjectData
	{
		[JsonProperty]
		[ItemProperty(DefaultValue = LightType.DIRECTIONAL)]
		[DefaultValue(LightType.DIRECTIONAL)]
		public LightType Type { get; set; }

		[ItemProperty]
		[JsonProperty]
		public LightFlag Flag { get; set; }

		[JsonProperty]
		[ItemProperty(DefaultValue = 1f)]
		[DefaultValue(1f)]
		public float Intensity { get; set; }

		[ItemProperty(DefaultValue = true)]
		[JsonProperty]
		[DefaultValue(true)]
		public bool Enable { get; set; }

		[JsonProperty]
		[ItemProperty(DefaultValue = 5f)]
		[DefaultValue(5f)]
		public float Range { get; set; }

		[ItemProperty(DefaultValue = 30f)]
		[JsonProperty]
		[DefaultValue(30f)]
		public float OuterAngle { get; set; }

		public Light3DObjectData()
		{
			this.Enable = true;
			this.Flag = LightFlag.LIGHT0;
			this.Type = LightType.DIRECTIONAL;
			this.Range = 5f;
			this.OuterAngle = 30f;
			this.Intensity = 1f;
		}

		protected override void OnDataInitialize(VisualObject vObject)
		{
		}
	}
}
