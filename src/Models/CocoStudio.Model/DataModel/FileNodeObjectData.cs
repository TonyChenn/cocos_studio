using System;
using CocoStudio.Model.ExtensionModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.PropertyGrid;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataItem("ProjectNodeObjectData")]
	[DataModelExtension(typeof(FileNodeObject))]
	public class FileNodeObjectData : NodeObjectData
	{
		[JsonConverter(typeof(CsdToJsonConvertor))]
		[ItemProperty]
		[JsonProperty]
		public ResourceItemData FileData { get; set; }

		[PropertyOrder(2147483647)]
		[ItemProperty]
		[JsonProperty]
		public bool StretchWidthEnable { get; set; }

		[PropertyOrder(2147483647)]
		[ItemProperty]
		[JsonProperty]
		public bool StretchHeightEnable { get; set; }

		[ItemProperty]
		[JsonProperty]
		public float InnerActionSpeed { get; set; }

		[ItemProperty]
		[PropertyOrder(2147483647)]
		public bool CustomSizeEnabled { get; set; }

		public FileNodeObjectData()
		{
			this.ctype = "ProjectNodeObjectData";
		}
	}
}
