using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(GameNodeObject))]
	public class GameNodeObjectData : AbstractNodeObjectData
	{
		public GameNodeObjectData()
		{
			this.ctype = "SingleNodeObjectData";
		}

		public GameNodeObjectData(AbstractNodeObjectData sNode)
		{
			this.ctype = "SingleNodeObjectData";
			base.CanEdit = sNode.CanEdit;
			base.ActionTag = sNode.ActionTag;
			base.CallBackName = sNode.CallBackName;
			base.CallBackType = sNode.CallBackType;
			base.CustomClassName = sNode.CustomClassName;
			base.FrameEvent = sNode.FrameEvent;
			base.InnerClassName = sNode.InnerClassName;
			base.IsAutoSize = sNode.IsAutoSize;
			base.Name = sNode.Name;
			base.ScriptData = sNode.ScriptData;
			base.Size = sNode.Size;
			base.Tag = sNode.Tag;
			base.UserData = sNode.UserData;
			base.Visible = sNode.Visible;
			base.ZOrder = sNode.ZOrder;
			base.Children = sNode.Children;
		}
	}
}
