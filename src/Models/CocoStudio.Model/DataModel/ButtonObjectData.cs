using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(ButtonObject))]
	public class ButtonObjectData : WidgetObjectData
	{
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool FlipX { get; set; }

		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool FlipY { get; set; }

		[DefaultValue(0)]
		[JsonProperty]
		[ItemProperty(DefaultValue = 0)]
		public int FontSize { get; set; }

		[JsonProperty]
		[ItemProperty(DefaultValue = "")]
		[DefaultValue("")]
		public string ButtonText { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData FontResource { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ColorData TextColor { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData DisabledFileData
		{
			get
			{
				return this.disabledFileData;
			}
			set
			{
				this.disabledFileData = value;
				if (this.disabledFileData == ResourceItemData.DefaultMarker)
				{
					this.disabledFileData = ButtonObjectData.Default_DisabledFile;
				}
			}
		}

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData PressedFileData
		{
			get
			{
				return this.pressedFileData;
			}
			set
			{
				this.pressedFileData = value;
				if (this.pressedFileData == ResourceItemData.DefaultMarker)
				{
					this.pressedFileData = ButtonObjectData.Default_PressedFile;
				}
			}
		}

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData NormalFileData
		{
			get
			{
				return this.normalFileData;
			}
			set
			{
				this.normalFileData = value;
				if (this.normalFileData == ResourceItemData.DefaultMarker)
				{
					this.normalFileData = ButtonObjectData.Default_NormalFile;
				}
			}
		}

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

		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Scale9OriginX { get; set; }

		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Scale9OriginY { get; set; }

		[ItemProperty(DefaultValue = 0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(0)]
		public int Scale9Width { get; set; }

		[ItemProperty(DefaultValue = 0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(0)]
		public int Scale9Height { get; set; }

		[DefaultValue(true)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = true)]
		public bool DisplayState { get; set; }

		[ItemProperty(DefaultValue = 1)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(1)]
		public int OutlineSize { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ColorData OutlineColor { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		public bool OutlineEnabled { get; set; }

		[JsonProperty]
		[ItemProperty]
		public ColorData ShadowColor { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 2)]
		[DefaultValue(2)]
		public float ShadowOffsetX { get; set; }

		[DefaultValue(-2)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = -2)]
		public float ShadowOffsetY { get; set; }

		[DefaultValue(0)]
		[ItemProperty(DefaultValue = 0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int ShadowBlurRadius { get; set; }

		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		public bool ShadowEnabled { get; set; }

		public ButtonObjectData()
		{
			this.ButtonText = string.Empty;
			this.DisplayState = true;
			this.ShadowOffsetX = 2f;
			this.ShadowOffsetY = -2f;
			this.OutlineSize = 1;
		}

		protected override void OnDataInitialize(VisualObject vObject)
		{
			ButtonObject buttonObject = vObject as ButtonObject;
			if (buttonObject != null)
			{
				if (this.NormalFileData != null && buttonObject.NormalFileData != null && buttonObject.NormalFileData.GetResourceData().Type != this.NormalFileData.Type)
				{
					buttonObject.NormalFileData = ResourceFile.DefaultMarker;
				}
			}
		}

		internal static readonly ResourceItemData Default_NormalFile = new ResourceItemData(EnumResourceType.Default, "Default/Button_Normal.png");

		internal static readonly ResourceItemData Default_PressedFile = new ResourceItemData(EnumResourceType.Default, "Default/Button_Press.png");

		internal static readonly ResourceItemData Default_DisabledFile = new ResourceItemData(EnumResourceType.Default, "Default/Button_Disable.png");

		private ResourceItemData disabledFileData;

		private ResourceItemData pressedFileData;

		private ResourceItemData normalFileData;
	}
}
