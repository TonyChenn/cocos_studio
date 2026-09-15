using System;
using Gtk;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	internal class DownloadAnimation
	{
		public static DownloadAnimation Instance
		{
			get
			{
				if (DownloadAnimation.instance == null)
				{
					DownloadAnimation.instance = new DownloadAnimation();
				}
				return DownloadAnimation.instance;
			}
		}

		public void StartDownloadAnimation(int point_x, int point_y)
		{
			int num;
			int num2;
			Services.MainWindow.GetPosition(out num, out num2);
			DownloadAnimationWindow downloadAnimationWindow = new DownloadAnimationWindow();
			downloadAnimationWindow.Show();
			int num3;
			int num4;
			downloadAnimationWindow.GetSize(out num3, out num4);
			int x;
			int y;
			if (Platform.IsWindows)
			{
				x = num + 190 + point_x - num3 / 2;
				y = num2 + 73 + point_y - num4 / 2;
			}
			else
			{
				x = num + point_x - num3 / 2;
				y = num2 + 542 - point_y - num4 / 2;
			}
			downloadAnimationWindow.Move(x, y);
			WidgetAnimationModel widgetAnimationModel = new WidgetAnimationModel();
			widgetAnimationModel.EndAnimationClick += this.model_EndAnimationClick;
			int num5 = 261;
			widgetAnimationModel.StartPositionAnimation(downloadAnimationWindow, num + 80, num2 + num5);
		}

		private void model_EndAnimationClick(object sender, EndAnimationClickEventArgs e)
		{
			WidgetAnimationModel widgetAnimationModel = sender as WidgetAnimationModel;
			widgetAnimationModel.EndAnimationClick -= this.model_EndAnimationClick;
			e.Window.Destroy();
			e.Window.Dispose();
		}

		private static DownloadAnimation instance;
	}
}
