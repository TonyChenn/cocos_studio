using System;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Event;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000F7 RID: 247
	public class UserCameraPreviewObject
	{
		// Token: 0x060008C7 RID: 2247 RVA: 0x00023140 File Offset: 0x00021340
		public UserCameraPreviewObject(CSUserCameraPreview preview)
		{
			this.innerNode = preview;
			Services.EventsService.GetEvent<CanvasSizeChangeEvent>().Subscribe(new Action<CanvasSizeChangeEventArgs>(this.OnCanvasSizeChange));
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00023178 File Offset: 0x00021378
		private CSUserCameraPreview GetInnerWidget()
		{
			return this.innerNode;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00023190 File Offset: 0x00021390
		public void SetCurrentCamera(CSCamera camera)
		{
			if (camera != null)
			{
				this.GetInnerWidget().SetCamera(camera);
				SizeF sceneSize = GameWindow.Current.GetCanvasObject().GetSceneSize();
				this.refreshPreview(sceneSize);
			}
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x000231CD File Offset: 0x000213CD
		public void Rander(bool Rander)
		{
			this.isRander = Rander;
			this.GetInnerWidget().Render(Rander);
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x000231E4 File Offset: 0x000213E4
		public void refreshPreview(SizeF size)
		{
			this.GetInnerWidget().OnPreviewChanged(size);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x000231F4 File Offset: 0x000213F4
		private void OnCanvasSizeChange(CanvasSizeChangeEventArgs obj)
		{
			if (this.isRander)
			{
				this.Rander(false);
				this.GetInnerWidget().OnPreviewChanged(obj.NewSize);
				this.Rander(true);
			}
		}

		// Token: 0x0400033D RID: 829
		private CSUserCameraPreview innerNode;

		// Token: 0x0400033E RID: 830
		private bool isRander = false;
	}
}
