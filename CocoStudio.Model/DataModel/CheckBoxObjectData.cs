using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000017 RID: 23
	[DataModelExtension(typeof(CheckBoxObject))]
	public class CheckBoxObjectData : WidgetObjectData
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00003988 File Offset: 0x00001B88
		// (set) Token: 0x060000FA RID: 250 RVA: 0x0000399F File Offset: 0x00001B9F
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(false)]
		[ItemProperty(DefaultValue = false)]
		public bool CheckedState { get; set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000FB RID: 251 RVA: 0x000039A8 File Offset: 0x00001BA8
		// (set) Token: 0x060000FC RID: 252 RVA: 0x000039BF File Offset: 0x00001BBF
		[DefaultValue(true)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = true)]
		public bool DisplayState { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000FD RID: 253 RVA: 0x000039C8 File Offset: 0x00001BC8
		// (set) Token: 0x060000FE RID: 254 RVA: 0x000039E0 File Offset: 0x00001BE0
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

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00003A1C File Offset: 0x00001C1C
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00003A34 File Offset: 0x00001C34
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

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00003A70 File Offset: 0x00001C70
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00003A88 File Offset: 0x00001C88
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

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00003AC4 File Offset: 0x00001CC4
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00003ADC File Offset: 0x00001CDC
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

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00003B14 File Offset: 0x00001D14
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00003B2C File Offset: 0x00001D2C
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

		// Token: 0x06000107 RID: 263 RVA: 0x00003B63 File Offset: 0x00001D63
		public CheckBoxObjectData()
		{
			this.DisplayState = true;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00003B78 File Offset: 0x00001D78
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

		// Token: 0x04000065 RID: 101
		internal static readonly ResourceItemData Default_Normal = new ResourceItemData(EnumResourceType.Default, "Default/CheckBox_Normal.png");

		// Token: 0x04000066 RID: 102
		internal static readonly ResourceItemData Default_Press = new ResourceItemData(EnumResourceType.Default, "Default/CheckBox_Press.png");

		// Token: 0x04000067 RID: 103
		internal static readonly ResourceItemData Default_Disable = new ResourceItemData(EnumResourceType.Default, "Default/CheckBox_Disable.png");

		// Token: 0x04000068 RID: 104
		internal static readonly ResourceItemData Default_NodeNormal = new ResourceItemData(EnumResourceType.Default, "Default/CheckBoxNode_Normal.png");

		// Token: 0x04000069 RID: 105
		internal static readonly ResourceItemData Default_NodeDisable = new ResourceItemData(EnumResourceType.Default, "Default/CheckBoxNode_Disable.png");

		// Token: 0x0400006A RID: 106
		private ResourceItemData normalBackFileData;

		// Token: 0x0400006B RID: 107
		private ResourceItemData pressedBackFileData;

		// Token: 0x0400006C RID: 108
		private ResourceItemData disableBackFileData;

		// Token: 0x0400006D RID: 109
		private ResourceItemData nodeNormalFileData;

		// Token: 0x0400006E RID: 110
		private ResourceItemData nodeDisableFileData;
	}
}
