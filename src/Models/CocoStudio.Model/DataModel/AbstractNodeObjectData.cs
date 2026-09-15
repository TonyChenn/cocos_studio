using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataItem(Name = "AbstractNodeData")]
	[DataModelExtension(typeof(AbstractNodeObject))]
	public class AbstractNodeObjectData : VisualObjectData
	{
		[ItemProperty(DefaultValue = null)]
		public ScriptFileData ScriptData { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[DefaultValue(EnumCallBack.None)]
		[ItemProperty(DefaultValue = EnumCallBack.None)]
		public EnumCallBack CallBackType { get; set; }

		[ItemProperty(DefaultValue = "")]
		[DefaultValue("")]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string CallBackName { get; set; }

		[DefaultValue("")]
		[ItemProperty(DefaultValue = "")]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string CustomClassName { get; set; }

		[DefaultValue("")]
		[ItemProperty(DefaultValue = "")]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string UserData { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		[ItemProperty(DefaultValue = 0)]
		[DefaultValue(0)]
		public int Tag { get; set; }

		[ItemProperty(DefaultValue = "")]
		[DefaultValue("")]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string FrameEvent { get; set; }

		[ItemProperty]
		[JsonProperty]
		public List<AbstractNodeObjectData> Children { get; set; }

		public AbstractNodeObjectData()
		{
			this.ScriptData = null;
			this.CallBackType = EnumCallBack.None;
			this.CallBackName = string.Empty;
			this.CustomClassName = string.Empty;
			this.UserData = "";
			this.Tag = 0;
			this.FrameEvent = string.Empty;
		}
	}
}
