using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.ViewModel
{
	public class SkyBoxManager
	{
		public static void CheckSkyBoxResources(ISkyBox skyBox)
		{
			string text = string.Empty;
			if (skyBox != null)
			{
				List<ResourceFile> list = new List<ResourceFile>();
				if (skyBox.UpImage != null && !skyBox.UpImage.IsDefault && skyBox.UpImage.PreviewImageInfo != null)
				{
					list.Add(skyBox.UpImage);
				}
				if (skyBox.DownImage != null && !skyBox.DownImage.IsDefault && skyBox.DownImage.PreviewImageInfo != null)
				{
					list.Add(skyBox.DownImage);
				}
				if (skyBox.LeftImage != null && !skyBox.LeftImage.IsDefault && skyBox.LeftImage.PreviewImageInfo != null)
				{
					list.Add(skyBox.LeftImage);
				}
				if (skyBox.RightImage != null && !skyBox.RightImage.IsDefault && skyBox.RightImage.PreviewImageInfo != null)
				{
					list.Add(skyBox.RightImage);
				}
				if (skyBox.BackImage != null && !skyBox.BackImage.IsDefault && skyBox.BackImage.PreviewImageInfo != null)
				{
					list.Add(skyBox.BackImage);
				}
				if (skyBox.ForwardImage != null && !skyBox.ForwardImage.IsDefault && skyBox.ForwardImage.PreviewImageInfo != null)
				{
					list.Add(skyBox.ForwardImage);
				}
				if (list.Count == 6)
				{
					ResourceFile resourceFile = list[0];
					using (List<ResourceFile>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ResourceFile resourceFile2 = enumerator.Current;
							PreviewImageInfo previewImageInfo = resourceFile2.PreviewImageInfo;
							if (previewImageInfo.Size.Width != previewImageInfo.Size.Height)
							{
								text = LanguageInfo.SkyBoxError_Size;
								break;
							}
							if (previewImageInfo.Size.Width != resourceFile.PreviewImageInfo.Size.Width || previewImageInfo.Size.Height != resourceFile.PreviewImageInfo.Size.Height)
							{
								text = LanguageInfo.SkyBoxError_Size;
								break;
							}
							if (Path.GetExtension(resourceFile2.Name) != Path.GetExtension(resourceFile.Name))
							{
								text = LanguageInfo.SkyBoxError_Format;
								break;
							}
							if (previewImageInfo.Image != null && resourceFile.PreviewImageInfo.Image != null && (previewImageInfo.ImageFormat != resourceFile.PreviewImageInfo.ImageFormat || previewImageInfo.Image.NChannels != resourceFile.PreviewImageInfo.Image.NChannels || previewImageInfo.Image.BitsPerSample != resourceFile.PreviewImageInfo.Image.BitsPerSample))
							{
								text = LanguageInfo.SkyBoxError_Encoded;
								break;
							}
						}
						goto IL_286;
					}
				}
				if (list.Count > 0)
				{
					text = LanguageInfo.SkyBoxError_FullNum;
				}
				IL_286:
				skyBox.SkyboxResourceError = text;
				if (text == string.Empty && list.Count == 6)
				{
					skyBox.RefreshSkyBox();
					return;
				}
				skyBox.ResetSkyBox();
			}
		}

		private const int fullNum = 6;
	}
}
