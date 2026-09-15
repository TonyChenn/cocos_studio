using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(GameLayerObject))]
	public class GameLayerObjectData : AbstractNodeObjectData
	{
		public GameLayerObjectData()
		{
			this.ctype = "LayerObjectData";
		}

		public GameLayerObjectData(AbstractNodeObjectData objectData)
		{
			this.ctype = "LayerObjectData";
			base.CanEdit = objectData.CanEdit;
			base.ActionTag = objectData.ActionTag;
			base.CallBackName = objectData.CallBackName;
			base.CallBackType = objectData.CallBackType;
			base.CustomClassName = objectData.CustomClassName;
			base.FrameEvent = objectData.FrameEvent;
			base.InnerClassName = objectData.InnerClassName;
			base.IsAutoSize = objectData.IsAutoSize;
			base.Name = objectData.Name;
			base.ScriptData = objectData.ScriptData;
			base.Size = objectData.Size;
			base.Tag = objectData.Tag;
			base.UserData = objectData.UserData;
			base.Visible = objectData.Visible;
			base.ZOrder = objectData.ZOrder;
			base.Children = objectData.Children;
		}
	}
}
