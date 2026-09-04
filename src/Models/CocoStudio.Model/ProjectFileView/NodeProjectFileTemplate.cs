using System;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace CocoStudio.Model.ProjectFileView
{
	// Token: 0x020000C5 RID: 197
	[Extension(typeof(BaseProjectFileTemplate))]
	internal class NodeProjectFileTemplate : BaseProjectFileTemplate
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x000198D0 File Offset: 0x00017AD0
		public override string LabelName
		{
			get
			{
				return LanguageInfo.NewFile_AnimationNode;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x000198E8 File Offset: 0x00017AE8
		public override NodeType FileType
		{
			get
			{
				return NodeType.Node;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600063B RID: 1595 RVA: 0x000198FC File Offset: 0x00017AFC
		public override int Order
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x00019910 File Offset: 0x00017B10
		public override string Description
		{
			get
			{
				return LanguageInfo.NewFile_NodeDes;
			}
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00019928 File Offset: 0x00017B28
		protected override string OnGetIconResource()
		{
			return "CocoStudio.DefaultResource.Images.ProjectFile.node.png";
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x00019940 File Offset: 0x00017B40
		public override int MaxSize
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00019953 File Offset: 0x00017B53
		protected override void OnChangeView(CanvasObject canvas, CocosItem project)
		{
			canvas.Size = SizeF.Empty;
			canvas.SetCenterLineVisible(true);
		}
	}
}
