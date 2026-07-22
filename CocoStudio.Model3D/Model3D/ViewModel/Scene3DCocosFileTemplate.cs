using System;
using System.Collections.Generic;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace CocoStudio.Model3D.ViewModel
{
	// Token: 0x02000028 RID: 40
	[Extension(typeof(BaseProjectFileTemplate))]
	internal class Scene3DCocosFileTemplate : BaseProjectFileTemplate
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000644B File Offset: 0x0000464B
		public override string LabelName
		{
			get
			{
				return LanguageInfo.NewFile_Scene3D;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00006452 File Offset: 0x00004652
		public override NodeType FileType
		{
			get
			{
				return NodeType.Scene3D;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00006455 File Offset: 0x00004655
		public override int Order
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00006458 File Offset: 0x00004658
		public override string Description
		{
			get
			{
				return LanguageInfo.NewFile_Scene3DDes;
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000645F File Offset: 0x0000465F
		protected override string OnGetIconResource()
		{
			return "CocoStudio.DefaultResource.Images.ProjectFile.sence3d.png";
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00006466 File Offset: 0x00004666
		protected override void OnChangeView(CanvasObject canvas, CocosItem item)
		{
			canvas.SetCenterLineVisible(false);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000646F File Offset: 0x0000466F
		protected override void OnInitGameProejctData(GameFileData gameFileData)
		{
			gameFileData.ObjectData = new GameNode3DObjectData();
			if (gameFileData.ObjectData.Children == null)
			{
				gameFileData.ObjectData.Children = new List<AbstractNodeObjectData>();
			}
			this.CreateDefaultCamera(gameFileData);
			this.CreateDefaultLight(gameFileData);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000064A8 File Offset: 0x000046A8
		private void CreateDefaultCamera(GameFileData gameFileData)
		{
			UserCameraObjectData userCameraObjectData = new UserCameraObjectData();
			if (gameFileData.ObjectData.Children != null)
			{
				gameFileData.ObjectData.Children.Add(userCameraObjectData);
			}
			userCameraObjectData.Position3D = new Point3F(0f, 1f, -10f);
			userCameraObjectData.Fov = 60f;
			userCameraObjectData.Name = "UserCamera_0";
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000650C File Offset: 0x0000470C
		private void CreateDefaultLight(GameFileData gameFileData)
		{
			Light3DObjectData light3DObjectData = new Light3DObjectData();
			if (gameFileData.ObjectData.Children != null)
			{
				gameFileData.ObjectData.Children.Add(light3DObjectData);
			}
			light3DObjectData.Intensity = 1f;
			light3DObjectData.Position3D = new Point3F(0f, 3f, 0f);
			light3DObjectData.Rotation3D = new Point3F(50f, 180f, 0f);
			light3DObjectData.Enable = true;
			light3DObjectData.Flag = LightFlag.LIGHT0;
			light3DObjectData.Type = LightType.DIRECTIONAL;
			light3DObjectData.Range = 5f;
			light3DObjectData.OuterAngle = 30f;
			light3DObjectData.Name = "DirectionLight";
		}
	}
}
