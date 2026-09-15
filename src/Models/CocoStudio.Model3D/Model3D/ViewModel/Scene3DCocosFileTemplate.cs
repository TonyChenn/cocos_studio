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
	[Extension(typeof(BaseProjectFileTemplate))]
	internal class Scene3DCocosFileTemplate : BaseProjectFileTemplate
	{
		public override string LabelName
		{
			get
			{
				return LanguageInfo.NewFile_Scene3D;
			}
		}

		public override NodeType FileType
		{
			get
			{
				return NodeType.Scene3D;
			}
		}

		public override int Order
		{
			get
			{
				return 4;
			}
		}

		public override string Description
		{
			get
			{
				return LanguageInfo.NewFile_Scene3DDes;
			}
		}

		protected override string OnGetIconResource()
		{
			return "CocoStudio.DefaultResource.Images.ProjectFile.sence3d.png";
		}

		protected override void OnChangeView(CanvasObject canvas, CocosItem item)
		{
			canvas.SetCenterLineVisible(false);
		}

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
