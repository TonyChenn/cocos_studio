using System;
using CocoStudio.EngineAdapterWrap;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200011B RID: 283
	public class SceneObject : VisualObject
	{
		// Token: 0x06000ADA RID: 2778 RVA: 0x0002B3E2 File Offset: 0x000295E2
		internal SceneObject(CSScene entity)
		{
			this.csSceneEntity = entity;
			this.camera = new CameraObject(entity.GetCamera());
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0002B408 File Offset: 0x00029608
		internal override CSVisualObject GetCSVisual()
		{
			return this.csSceneEntity;
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0002B420 File Offset: 0x00029620
		public void AddChild(VisualObject cObject)
		{
			this.GetCSVisual().AddChild(cObject.GetCSVisual());
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x0002B438 File Offset: 0x00029638
		public CameraObject GetCamera()
		{
			return this.camera;
		}

		// Token: 0x0400047B RID: 1147
		private CSScene csSceneEntity;

		// Token: 0x0400047C RID: 1148
		private CameraObject camera;
	}
}
