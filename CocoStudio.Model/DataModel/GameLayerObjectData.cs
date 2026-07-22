using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x0200001D RID: 29
	[DataModelExtension(typeof(GameLayerObject))]
	public class GameLayerObjectData : AbstractNodeObjectData
	{
		// Token: 0x0600013E RID: 318 RVA: 0x0000508E File Offset: 0x0000328E
		public GameLayerObjectData()
		{
			this.ctype = "LayerObjectData";
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000050A8 File Offset: 0x000032A8
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
