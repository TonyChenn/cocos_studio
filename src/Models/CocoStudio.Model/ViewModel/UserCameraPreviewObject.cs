using System;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Event;

namespace CocoStudio.Model.ViewModel
{
	public class UserCameraPreviewObject
	{
		public UserCameraPreviewObject(CSUserCameraPreview preview)
		{
			this.innerNode = preview;
			Services.EventsService.GetEvent<CanvasSizeChangeEvent>().Subscribe(new Action<CanvasSizeChangeEventArgs>(this.OnCanvasSizeChange));
		}

		private CSUserCameraPreview GetInnerWidget()
		{
			return this.innerNode;
		}

		public void SetCurrentCamera(CSCamera camera)
		{
			if (camera != null)
			{
				this.GetInnerWidget().SetCamera(camera);
				SizeF sceneSize = GameWindow.Current.GetCanvasObject().GetSceneSize();
				this.refreshPreview(sceneSize);
			}
		}

		public void Rander(bool Rander)
		{
			this.isRander = Rander;
			this.GetInnerWidget().Render(Rander);
		}

		public void refreshPreview(SizeF size)
		{
			this.GetInnerWidget().OnPreviewChanged(size);
		}

		private void OnCanvasSizeChange(CanvasSizeChangeEventArgs obj)
		{
			if (this.isRander)
			{
				this.Rander(false);
				this.GetInnerWidget().OnPreviewChanged(obj.NewSize);
				this.Rander(true);
			}
		}

		private CSUserCameraPreview innerNode;

		private bool isRander = false;
	}
}
