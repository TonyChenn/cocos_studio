using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Model;
using Gdk;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	// Token: 0x02000050 RID: 80
	public class PreviewImageService
	{
		// Token: 0x06000235 RID: 565 RVA: 0x00008D01 File Offset: 0x00006F01
		internal PreviewImageService()
		{
			PreviewImageService.previewImageDic = new Dictionary<ResourceItem, PreviewImageInfo>();
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00008D14 File Offset: 0x00006F14
		public PreviewImageInfo GetImage(ResourceItem image)
		{
			if (!File.Exists(image.PreviewImagePath))
			{
				return null;
			}
			try
			{
				if (image.IsNeedRefresh() || !PreviewImageService.previewImageDic.ContainsKey(image))
				{
					this.UpdateImage(image);
				}
				if (PreviewImageService.previewImageDic.ContainsKey(image))
				{
					return PreviewImageService.previewImageDic[image];
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Load preview image failed.", exception);
			}
			return null;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00008D90 File Offset: 0x00006F90
		internal void UpdateCachedImage(ResourceItem image)
		{
			if (!PreviewImageService.previewImageDic.ContainsKey(image))
			{
				return;
			}
			this.UpdateImage(image);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00008DA8 File Offset: 0x00006FA8
		private void UpdateImage(ResourceItem image)
		{
			FilePath fileName = image.PreviewImagePath;
			if (!File.Exists(image.PreviewImagePath) || !ProjectsService.Instance.IsPicture(fileName))
			{
				return;
			}
			string empty = string.Empty;
			Pixbuf pixbuf = PixbufHelper.Load(image.PreviewImagePath, out empty);
			if (pixbuf == null)
			{
				return;
			}
			PreviewImageInfo previewImageInfo = new PreviewImageInfo(pixbuf);
			if ((float)pixbuf.Height > 200f || (float)pixbuf.Width > 200f)
			{
				SizeF relativeSize = this.GetRelativeSize(pixbuf.Width, pixbuf.Height);
				Pixbuf pixbuf2 = pixbuf;
				pixbuf = pixbuf2.ScaleSimple((int)relativeSize.Width, (int)relativeSize.Height, InterpType.Bilinear);
				pixbuf2.Dispose();
			}
			previewImageInfo.Image = pixbuf;
			previewImageInfo.ImageFormat = empty;
			if (PreviewImageService.previewImageDic.ContainsKey(image))
			{
				PreviewImageService.previewImageDic[image] = previewImageInfo;
			}
			else
			{
				PreviewImageService.previewImageQueue.Enqueue(image);
				PreviewImageService.previewImageDic.Add(image, previewImageInfo);
			}
			if (PreviewImageService.previewImageQueue.Count > 30)
			{
				ResourceItem key = PreviewImageService.previewImageQueue.Dequeue();
				PreviewImageService.previewImageDic[key].Image.Dispose();
				PreviewImageService.previewImageDic.Remove(key);
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00008ED0 File Offset: 0x000070D0
		private SizeF GetRelativeSize(int width, int height)
		{
			int num = (width > height) ? width : height;
			if ((float)num > 200f)
			{
				float num2 = 200f / (float)num;
				width = (int)((float)width * num2);
				height = (int)((float)height * num2);
			}
			return new SizeF((float)width, (float)height);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00008F10 File Offset: 0x00007110
		internal void Clear()
		{
			try
			{
				PreviewImageService.previewImageQueue.Clear();
				foreach (KeyValuePair<ResourceItem, PreviewImageInfo> keyValuePair in PreviewImageService.previewImageDic)
				{
					if (keyValuePair.Value != null && keyValuePair.Value.Image != null)
					{
						keyValuePair.Value.Image.Dispose();
						keyValuePair.Value.Image = null;
					}
				}
				PreviewImageService.previewImageDic.Clear();
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Failed to clear preview image.", exception);
			}
		}

		// Token: 0x04000091 RID: 145
		private const float MaxSize = 200f;

		// Token: 0x04000092 RID: 146
		public const int MaxCount = 30;

		// Token: 0x04000093 RID: 147
		private static Queue<ResourceItem> previewImageQueue = new Queue<ResourceItem>();

		// Token: 0x04000094 RID: 148
		private static Dictionary<ResourceItem, PreviewImageInfo> previewImageDic = new Dictionary<ResourceItem, PreviewImageInfo>();
	}
}
