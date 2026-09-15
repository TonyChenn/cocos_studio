using System;
using System.Collections.Generic;
using CocoStudio.Projects;

namespace CocoStudio.Model.ViewModel
{
	public interface ISkyBox
	{
		bool SkyBoxEnabled { get; set; }

		string SkyboxResourceError { get; set; }

		ResourceFile UpImage { get; set; }

		ResourceFile DownImage { get; set; }

		ResourceFile BackImage { get; set; }

		ResourceFile ForwardImage { get; set; }

		ResourceFile LeftImage { get; set; }

		ResourceFile RightImage { get; set; }

		List<string> ResourceValue { get; set; }

		void ResetSkyBox();

		void RefreshSkyBox();
	}
}
