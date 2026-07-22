using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000025 RID: 37
	[DataModelExtension(typeof(SkeletonObject))]
	public class SkeletonNodeObjectData : BoneNodeObjectData
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x00008E11 File Offset: 0x00007011
		public SkeletonNodeObjectData()
		{
			this.ctype = "SkeletonNodeObjectData";
			base.Size = new SizeF(20f, 20f);
			base.Length = 20f;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00008E44 File Offset: 0x00007044
		public SkeletonNodeObjectData(AbstractNodeObjectData sNode)
		{
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
			if (base.Length == 0f)
			{
				base.Length = 20f;
			}
		}
	}
}
