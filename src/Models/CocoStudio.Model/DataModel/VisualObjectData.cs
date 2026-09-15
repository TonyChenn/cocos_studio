using System;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.PropertyGrid;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(VisualObject))]
	public class VisualObjectData : BaseObjectData, IDataInitialize
	{
		[ItemProperty(DefaultValue = true)]
		public bool CanEdit { get; set; }

		[ItemProperty(DefaultValue = true)]
		public bool Visible { get; set; }

		[ItemProperty]
		[JsonProperty]
		public string InnerClassName { get; set; }

		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int ActionTag { get; set; }

		[DefaultValue(0)]
		[ItemProperty(DefaultValue = 0)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public int ZOrder { get; set; }

		[ItemProperty(DefaultValue = false)]
		[DefaultValue(false)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public bool IsAutoSize { get; set; }

		[PropertyOrder(2147483646)]
		[ItemProperty]
		[JsonProperty]
		public SizeF Size { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = true)]
		[DefaultValue(true)]
		public bool VisibleForFrame { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 255)]
		[DefaultValue(255)]
		public int Alpha { get; set; }

		public VisualObjectData()
		{
			this.CanEdit = true;
			this.Visible = true;
			this.VisibleForFrame = true;
			this.Alpha = 255;
		}

		public void DataInitialize(VisualObject vObject)
		{
			try
			{
				this.OnDataInitialize(vObject);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("VisualObject can not be Initialize directly", exception);
			}
		}

		protected virtual void OnDataInitialize(VisualObject vObject)
		{
		}
	}
}
