using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(CheckBoxObject))]
	public class CheckBoxObjectData : WidgetObjectData
	{
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		public bool CheckedState { get; set; }

		[DefaultValue(true)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = true)]
		public bool DisplayState { get; set; }

		[JsonProperty]
		[ItemProperty]
		public ResourceItemData NormalBackFileData
		{
			get
			{
				return this.normalBackFileData;
			}
			set
			{
				this.normalBackFileData = value;
				if (this.normalBackFileData == ResourceItemData.DefaultMarker)
				{
					this.normalBackFileData = CheckBoxObjectData.Default_Normal;
				}
			}
		}

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData PressedBackFileData
		{
			get
			{
				return this.pressedBackFileData;
			}
			set
			{
				this.pressedBackFileData = value;
				if (this.pressedBackFileData == ResourceItemData.DefaultMarker)
				{
					this.pressedBackFileData = CheckBoxObjectData.Default_Press;
				}
			}
		}

		[JsonProperty]
		[ItemProperty]
		public ResourceItemData DisableBackFileData
		{
			get
			{
				return this.disableBackFileData;
			}
			set
			{
				this.disableBackFileData = value;
				if (this.disableBackFileData == ResourceItemData.DefaultMarker)
				{
					this.disableBackFileData = CheckBoxObjectData.Default_Disable;
				}
			}
		}

		[JsonProperty]
		[ItemProperty]
		public ResourceItemData NodeNormalFileData
		{
			get
			{
				return this.nodeNormalFileData;
			}
			set
			{
				this.nodeNormalFileData = value;
				if (this.nodeNormalFileData == ResourceItemData.DefaultMarker)
				{
					this.nodeNormalFileData = CheckBoxObjectData.Default_NodeNormal;
				}
			}
		}

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData NodeDisableFileData
		{
			get
			{
				return this.nodeDisableFileData;
			}
			set
			{
				this.nodeDisableFileData = value;
				if (this.nodeDisableFileData == ResourceItemData.DefaultMarker)
				{
					this.nodeDisableFileData = CheckBoxObjectData.Default_NodeDisable;
				}
			}
		}

		public CheckBoxObjectData()
		{
			this.DisplayState = true;
		}

		protected override void OnDataInitialize(VisualObject vObject)
		{
			CheckBoxObject checkBoxObject = vObject as CheckBoxObject;
			if (checkBoxObject != null)
			{
				if (this.NormalBackFileData != null && checkBoxObject.NormalBackFileData.GetResourceData().Type != this.NormalBackFileData.Type)
				{
					checkBoxObject.NormalBackFileData = null;
				}
				bool displayState = checkBoxObject.DisplayState;
				checkBoxObject.DisplayState = !displayState;
				checkBoxObject.DisplayState = displayState;
			}
		}

		internal static readonly ResourceItemData Default_Normal = new ResourceItemData(EnumResourceType.Default, "Default/CheckBox_Normal.png");

		internal static readonly ResourceItemData Default_Press = new ResourceItemData(EnumResourceType.Default, "Default/CheckBox_Press.png");

		internal static readonly ResourceItemData Default_Disable = new ResourceItemData(EnumResourceType.Default, "Default/CheckBox_Disable.png");

		internal static readonly ResourceItemData Default_NodeNormal = new ResourceItemData(EnumResourceType.Default, "Default/CheckBoxNode_Normal.png");

		internal static readonly ResourceItemData Default_NodeDisable = new ResourceItemData(EnumResourceType.Default, "Default/CheckBoxNode_Disable.png");

		private ResourceItemData normalBackFileData;

		private ResourceItemData pressedBackFileData;

		private ResourceItemData disableBackFileData;

		private ResourceItemData nodeNormalFileData;

		private ResourceItemData nodeDisableFileData;
	}
}
