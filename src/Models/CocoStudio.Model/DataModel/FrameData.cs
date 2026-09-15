using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataItem(Name = "Frame")]
	[DataModelExtension(typeof(Frame))]
	public class FrameData : BaseObjectData, IFrameDataLuaExtend
	{
		[ItemProperty]
		[JsonProperty]
		public int FrameIndex { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = true)]
		[DefaultValue(true)]
		public bool Tween { get; set; }

		public FrameData()
		{
			this.Tween = true;
		}

		[ItemProperty]
		[JsonProperty]
		public EasingValue EasingData
		{
			get
			{
				EasingValue result;
				if (!this.Tween || null == this.easingValue)
				{
					result = null;
				}
				else
				{
					result = this.easingValue;
				}
				return result;
			}
			set
			{
				this.easingValue = value;
			}
		}

		public virtual bool FrameEquals(FrameData framedata)
		{
			return this.Tween == framedata.Tween && ((this.EasingData == null && framedata.EasingData == null) || ((this.EasingData != null || framedata.EasingData == null) && (this.EasingData == null || framedata.EasingData != null) && this.EasingData.Equals(framedata.EasingData)));
		}

		public static bool IsFloatEqual(float a, float b, float digit = 0.0001f)
		{
			return Math.Abs(a - b) < digit;
		}

		string IFrameDataLuaExtend.Property { get; set; }

		private EasingValue easingValue = null;
	}
}
