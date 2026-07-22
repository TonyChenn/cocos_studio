using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200001B RID: 27
	[DataModelExtension(typeof(GameNodeObject))]
	public class GameNodeObjectData : AbstractNodeObjectData
	{
		// Token: 0x06000121 RID: 289 RVA: 0x00004D3E File Offset: 0x00002F3E
		public GameNodeObjectData()
		{
			this.ctype = "SingleNodeObjectData";
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00004D58 File Offset: 0x00002F58
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
