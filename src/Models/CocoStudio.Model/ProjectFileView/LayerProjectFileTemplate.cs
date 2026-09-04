using System;
using CocoStudio.Basic;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace CocoStudio.Model.ProjectFileView
{
	// Token: 0x020000C4 RID: 196
	[Extension(typeof(BaseProjectFileTemplate))]
	internal class LayerProjectFileTemplate : BaseProjectFileTemplate
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x000197A4 File Offset: 0x000179A4
		public override string LabelName
		{
			get
			{
				return LanguageInfo.NewFile_Layer;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x000197BC File Offset: 0x000179BC
		public override NodeType FileType
		{
			get
			{
				return NodeType.Layer;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x000197D0 File Offset: 0x000179D0
		public override int Order
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x000197E4 File Offset: 0x000179E4
		public override string Description
		{
			get
			{
				return LanguageInfo.NewFile_LayerDes;
			}
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x000197FC File Offset: 0x000179FC
		protected override string OnGetIconResource()
		{
			return "CocoStudio.DefaultResource.Images.ProjectFile.layer.png";
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x00019814 File Offset: 0x00017A14
		public override int MaxSize
		{
			get
			{
				return 4096;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x0001982C File Offset: 0x00017A2C
		public override bool CanEditSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001983F File Offset: 0x00017A3F
		protected override void OnInitGameProejctData(GameFileData gameProjectData)
		{
			gameProjectData.ObjectData = new GameLayerObjectData();
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00019850 File Offset: 0x00017A50
		protected override void OnChangeView(CanvasObject canvas, CocosItem project)
		{
			if (canvas != null && project != null)
			{
				try
				{
					AbstractNodeObject rootNode = project.GetRootNode();
					if (rootNode != null)
					{
						canvas.Size = rootNode.Size;
						canvas.SetCenterLineVisible(false);
					}
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("设置画布中心线可见性时出错", exception);
				}
			}
		}
	}
}
