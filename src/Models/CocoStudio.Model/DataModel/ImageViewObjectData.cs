using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(ImageViewObject))]
	public class ImageViewObjectData : WidgetObjectData
	{
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool FlipX { get; set; }

		[ItemProperty(DefaultValue = false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		public bool FlipY { get; set; }

		[JsonProperty]
		[ItemProperty]
		public ResourceItemData FileData
		{
			get
			{
				return this.fileData;
			}
			set
			{
				this.fileData = value;
				if (this.fileData == ResourceItemData.DefaultMarker)
				{
					this.fileData = ImageViewObjectData.DefaultFile;
				}
			}
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
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
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		public int Scale9OriginX { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(0)]
		[ItemProperty(DefaultValue = 0)]
		public int Scale9OriginY { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		public int Scale9Width { get; set; }

		[DefaultValue(0)]
		[ItemProperty(DefaultValue = 0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int Scale9Height { get; set; }

		protected override void OnDataInitialize(VisualObject vObject)
		{
			ImageViewObject imageViewObject = vObject as ImageViewObject;
			if (imageViewObject != null)
			{
				if (this.FileData != null && imageViewObject.FileData.GetResourceData().Type != this.FileData.Type)
				{
					imageViewObject.FileData = null;
				}
			}
		}

		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/ImageFile.png");

		private static readonly SizeF defaultImageSize = new SizeF(46f, 46f);

		private ResourceItemData fileData;
	}
}
