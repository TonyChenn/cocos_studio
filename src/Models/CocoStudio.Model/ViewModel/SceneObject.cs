using System;
using CocoStudio.EngineAdapterWrap;

namespace CocoStudio.Model.ViewModel
{
	public class SceneObject : VisualObject
	{
		internal SceneObject(CSScene entity)
		{
			this.csSceneEntity = entity;
			this.camera = new CameraObject(entity.GetCamera());
		}

		internal override CSVisualObject GetCSVisual()
		{
			return this.csSceneEntity;
		}

		public void AddChild(VisualObject cObject)
		{
			this.GetCSVisual().AddChild(cObject.GetCSVisual());
		}

		public CameraObject GetCamera()
		{
			return this.camera;
		}

		private CSScene csSceneEntity;

		private CameraObject camera;
	}
}
