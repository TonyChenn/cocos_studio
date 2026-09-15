using System;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Window;
using Gdk;

namespace CocoStudio.Model.ViewModel
{
	public class GameWindow : IDisposable
	{
		public static GameWindow Current { get; private set; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public Gdk.Window GdkWindow { get; private set; }

		private GameWindow()
		{
			GameWindow.Current = this;
		}

		public GameWindow(Gdk.Window gdkWindow) : this()
		{
			this.GdkWindow = gdkWindow;
			this.csWindow = WindowHelp.CreateCSWindow(gdkWindow);
			this.sceneEntity = new SceneObject(this.csWindow.GetScene());
			this.canvasEntity = new CanvasObject(this.csWindow.GetCanvas());
		}

		public GameWindow(IntPtr windowHandle, int width, int height) : this()
		{
			this.csWindow = WindowHelp.CreateCSWindow(windowHandle, width, height);
			this.sceneEntity = new SceneObject(this.csWindow.GetScene());
			this.canvasEntity = new CanvasObject(this.csWindow.GetCanvas());
		}

		public CanvasObject GetCanvasObject()
		{
			return this.canvasEntity;
		}

		public SceneObject GetSceneObject()
		{
			return this.sceneEntity;
		}

		public void Draw()
		{
			this.csWindow.Draw(0);
		}

		public void SetViewRect(int x, int y, int width, int height)
		{
			this.Width = width;
			this.Height = height;
			this.csWindow.SetViewRect(x, y, width, height);
		}

		public void SetResourcePath(string path)
		{
			CSCocosHelp.SetResourcePath(path);
		}

		public void UpdateGLContext(bool isShowing, Gdk.Window window)
		{
			this.GdkWindow = window;
			WindowHelp.UpdateOpenGLContext(isShowing, this.csWindow, window);
		}

		public void SetSceneMode(bool is2D)
		{
			this.csWindow.SetSceneMode(is2D);
		}

		public void Dispose()
		{
		}

		public PointF ConvertControlToScene(PointF controlPoint)
		{
			PointF pointF = new PointF(controlPoint.X, controlPoint.Y);
			pointF.Y = (float)this.Height - pointF.Y;
			return pointF;
		}

		private CSWindow csWindow;

		private CanvasObject canvasEntity;

		private SceneObject sceneEntity;
	}
}
