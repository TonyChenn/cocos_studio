using System;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class CheckBoxSurrogate : WidgetSurrogate
	{
		[DataMember]
		public string backGroundBox { get; set; }

		[DataMember]
		public string backGroundBoxSelected { get; set; }

		[DataMember]
		public string backGroundBoxDisabled { get; set; }

		[DataMember]
		public string frontCross { get; set; }

		[DataMember]
		public string frontCrossDisabled { get; set; }

		[DataMember]
		public ResourceDataSurrogate backGroundBoxData { get; set; }

		[DataMember]
		public ResourceDataSurrogate backGroundBoxSelectedData { get; set; }

		[DataMember]
		public ResourceDataSurrogate backGroundBoxDisabledData { get; set; }

		[DataMember]
		public ResourceDataSurrogate frontCrossData { get; set; }

		[DataMember]
		public ResourceDataSurrogate frontCrossDisabledData { get; set; }

		[DataMember]
		public bool selectedState { get; set; }

		protected CheckBoxSurrogate()
		{
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			CheckBoxObjectData checkBoxObjectData = obj as CheckBoxObjectData;
			checkBoxObjectData.CheckedState = this.selectedState;
			checkBoxObjectData.NormalBackFileData = WidgetSurrogate.ConvertResourceData(this.backGroundBoxData);
			checkBoxObjectData.PressedBackFileData = WidgetSurrogate.ConvertResourceData(this.backGroundBoxSelectedData);
			checkBoxObjectData.DisableBackFileData = WidgetSurrogate.ConvertResourceData(this.backGroundBoxDisabledData);
			checkBoxObjectData.NodeNormalFileData = WidgetSurrogate.ConvertResourceData(this.frontCrossData);
			checkBoxObjectData.NodeDisableFileData = WidgetSurrogate.ConvertResourceData(this.frontCrossDisabledData);
			if (checkBoxObjectData.NormalBackFileData == null)
			{
				checkBoxObjectData.NormalBackFileData = ResourceItemData.DefaultMarker;
			}
			if (checkBoxObjectData.NodeNormalFileData == null)
			{
				checkBoxObjectData.NodeNormalFileData = ResourceItemData.DefaultMarker;
			}
			checkBoxObjectData.DisplayState = true;
		}
	}
}
