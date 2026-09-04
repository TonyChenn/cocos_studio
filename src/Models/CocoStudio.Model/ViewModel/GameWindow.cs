using System;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Window;
using Gdk;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000136 RID: 310
	public class GameWindow : IDisposable
	{
		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x0002D514 File Offset: 0x0002B714
		// (set) Token: 0x06000B79 RID: 2937 RVA: 0x0002D52A File Offset: 0x0002B72A
		public static GameWindow Current { get; private set; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000B7A RID: 2938 RVA: 0x0002D534 File Offset: 0x0002B734
		// (set) Token: 0x06000B7B RID: 2939 RVA: 0x0002D54B File Offset: 0x0002B74B
		public int Width { get; private set; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x0002D554 File Offset: 0x0002B754
		// (set) Token: 0x06000B7D RID: 2941 RVA: 0x0002D56B File Offset: 0x0002B76B
		public int Height { get; private set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x0002D574 File Offset: 0x0002B774
		// (set) Token: 0x06000B7F RID: 2943 RVA: 0x0002D58B File Offset: 0x0002B78B
		public Gdk.Window GdkWindow { get; private set; }

		// Token: 0x06000B80 RID: 2944 RVA: 0x0002D594 File Offset: 0x0002B794
		private GameWindow()
		{
			GameWindow.Current = this;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0002D5A8 File Offset: 0x0002B7A8
		public GameWindow(Gdk.Window gdkWindow) : this()
		{
			this.GdkWindow = gdkWindow;
			this.csWindow = WindowHelp.CreateCSWindow(gdkWindow);
			this.sceneEntity = new SceneObject(this.csWindow.GetScene());
			this.canvasEntity = new CanvasObject(this.csWindow.GetCanvas());
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0002D600 File Offset: 0x0002B800
		public GameWindow(IntPtr windowHandle, int width, int height) : this()
		{
			this.csWindow = WindowHelp.CreateCSWindow(windowHandle, width, height);
			this.sceneEntity = new SceneObject(this.csWindow.GetScene());
			this.canvasEntity = new CanvasObject(this.csWindow.GetCanvas());
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0002D650 File Offset: 0x0002B850
		public CanvasObject GetCanvasObject()
		{
			return this.canvasEntity;
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0002D668 File Offset: 0x0002B868
		public SceneObject GetSceneObject()
		{
			return this.sceneEntity;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0002D680 File Offset: 0x0002B880
		public void Draw()
		{
			this.csWindow.Draw(0);
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0002D690 File Offset: 0x0002B890
		public void SetViewRect(int x, int y, int width, int height)
		{
			this.Width = width;
			this.Height = height;
			this.csWindow.SetViewRect(x, y, width, height);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0002D6B5 File Offset: 0x0002B8B5
		public void SetResourcePath(string path)
		{
			CSCocosHelp.SetResourcePath(path);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0002D6BF File Offset: 0x0002B8BF
		public void UpdateGLContext(bool isShowing, Gdk.Window window)
		{
			this.GdkWindow = window;
			WindowHelp.UpdateOpenGLContext(isShowing, this.csWindow, window);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0002D6D8 File Offset: 0x0002B8D8
		public void SetSceneMode(bool is2D)
		{
			this.csWindow.SetSceneMode(is2D);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002D6E8 File Offset: 0x0002B8E8
		public void Dispose()
		{
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0002D6EC File Offset: 0x0002B8EC
		public PointF ConvertControlToScene(PointF controlPoint)
		{
			PointF pointF = new PointF(controlPoint.X, controlPoint.Y);
			pointF.Y = (float)this.Height - pointF.Y;
			return pointF;
		}

		// Token: 0x040004DF RID: 1247
		private CSWindow csWindow;

		// Token: 0x040004E0 RID: 1248
		private CanvasObject canvasEntity;

		// Token: 0x040004E1 RID: 1249
		private SceneObject sceneEntity;
	}
}
