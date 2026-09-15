using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataInclude(typeof(LoadingBarDirectionType))]
	[DataModelExtension(typeof(LoadingBarObject))]
	public class LoadingBarObjectData : WidgetObjectData
	{
		[ItemProperty(DefaultValue = 80)]
		[DefaultValue(80)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int ProgressInfo { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(LoadingBarDirectionType.Left_To_Right)]
		[ItemProperty(DefaultValue = LoadingBarDirectionType.Left_To_Right)]
		public LoadingBarDirectionType ProgressType { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData ImageFileData
		{
			get
			{
				return this.imageFileData;
			}
			set
			{
				this.imageFileData = value;
				if (this.imageFileData == ResourceItemData.DefaultMarker)
				{
					this.imageFileData = LoadingBarObjectData.DefaultFile;
				}
			}
		}

		public LoadingBarObjectData()
		{
			this.ProgressInfo = 80;
		}

		protected override void OnDataInitialize(VisualObject vObject)
		{
			LoadingBarObject loadingBarObject = vObject as LoadingBarObject;
			if (loadingBarObject != null)
			{
				if (this.ImageFileData != null && loadingBarObject.ImageFileData.GetResourceData().Type != this.ImageFileData.Type)
				{
					loadingBarObject.ImageFileData = null;
				}
			}
		}

		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/LoadingBarFile.png");

		private ResourceItemData imageFileData;
	}
}
