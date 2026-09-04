using System;
using Gtk;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000029 RID: 41
	public class DownloadAnimationWindow : Window
	{
		// Token: 0x06000176 RID: 374 RVA: 0x000083A4 File Offset: 0x000065A4
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

		// Token: 0x04000074 RID: 116
		private ImageBin image;
	}
}
