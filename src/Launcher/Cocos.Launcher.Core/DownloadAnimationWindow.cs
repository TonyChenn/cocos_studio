using System;
using Gtk;

namespace Cocos.Launcher.Core
{
	public class DownloadAnimationWindow : Window
	{
		public DownloadAnimationWindow() : base(WindowType.Popup)
		{
			base.SetSizeRequest(27, 21);
			base.TransientFor = Services.MainWindow;
			this.CenterToParentWindow(Services.MainWindow);
			this.image = new ImageBin();
			this.image.SetImageView(ImageIcon.GetIcon("Cocos.Launcher.Resource.LauncherResource.download.png"));
			base.Add(this.image);
			base.ShowAll();
		}

		private ImageBin image;
	}
}
