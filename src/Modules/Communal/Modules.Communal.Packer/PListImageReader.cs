using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using CocoStudio.Basic;
using Gdk;
using Modules.Communal.Packer.PlistReader;
using Modules.Communal.PList;

namespace Modules.Communal.Packer
{
	public class PListImageReader
	{
		public List<ImageInfo> ImageList
		{
			get
			{
				if (this.imageList == null)
				{
					this.imageList = this.plistFormatAnalysis.ToImageList(this.rootElement);
				}
				return this.imageList;
			}
		}

		public string ImageFilePath { get; private set; }

		public PListImageReader(string plistFilePath)
		{
			PListImageReader.CheckFile(plistFilePath);
			PListRoot plistRoot = null;
			using (FileStream fileStream = File.Open(plistFilePath, FileMode.Open, FileAccess.Read))
			{
				plistRoot = PListRoot.Load(fileStream);
			}
			if (plistRoot == null || plistRoot.Root == null || !(plistRoot.Root is PListDict))
			{
				throw new ArgumentException("给定的PList文件格式不正确.");
			}
			if (!PListImageReader.CheckPList(plistRoot))
			{
				throw new ArgumentException("给定的PList文件,不是大图合并的文件.");
			}
			this.rootElement = (plistRoot.Root as PListDict);
			this.plistFilePath = plistFilePath;
			this.plistFormatAnalysis = PlistImageFormatFactory.CreatePlistFormat(this.rootElement);
			this.ImageFilePath = this.plistFormatAnalysis.GetImageFilePath(plistFilePath);
			if (this.ImageFilePath == null || !File.Exists(this.ImageFilePath))
			{
				throw new ArgumentException("对应的图片文件不存在.");
			}
		}

		private static bool CheckFile(string plistFilePath)
		{
			if (!File.Exists(plistFilePath))
			{
				throw new FileNotFoundException(plistFilePath);
			}
			if (!plistFilePath.ToLower().EndsWith(".plist"))
			{
				throw new ArgumentException("必须使用.plist文件初始化");
			}
			return true;
		}

		private static bool TryCheckFile(string plistFilePath)
		{
			return File.Exists(plistFilePath) && plistFilePath.ToLower().EndsWith(".plist");
		}

		private static bool CheckPList(PListRoot listRoot)
		{
			PListDict plistDict = (PListDict)listRoot.Root;
			return plistDict.ContainsKey("metadata");
		}

		public static bool CheckIsImage(string plistFilePath)
		{
			bool flag = PListImageReader.TryCheckFile(plistFilePath);
			bool result;
			if (!flag)
			{
				result = false;
			}
			else
			{
				try
				{
					PListRoot listRoot = null;
					using (FileStream fileStream = File.Open(plistFilePath, FileMode.Open, FileAccess.Read))
					{
						listRoot = PListRoot.Load(fileStream);
					}
					flag = PListImageReader.CheckPList(listRoot);
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("Read plist file failed.", exception);
					return false;
				}
				result = flag;
			}
			return result;
		}

		public static string GetMatchImageFile(string plistFilePath)
		{
			string result;
			if (!File.Exists(plistFilePath))
			{
				result = null;
			}
			else
			{
				PlistImageFormat plistImageFormat = PlistImageFormatFactory.CreatePlistFormat(plistFilePath);
				if (plistImageFormat != null)
				{
					result = plistImageFormat.GetImageFilePath(plistFilePath);
				}
				else
				{
					result = string.Empty;
				}
			}
			return result;
		}

		public void SaveAllSubImage(string dirPath)
		{
			this.SaveAllSubImage(dirPath, false, true);
		}

		public void SaveAllSubImage(string dirPath, bool isRetainEdge, bool isHideDir)
		{
			if (this.ImageList != null)
			{
				if (File.Exists(this.ImageFilePath))
				{
					if (!Directory.Exists(dirPath))
					{
						DirectoryInfo directoryInfo = Directory.CreateDirectory(dirPath);
						if (isHideDir)
						{
							directoryInfo.Attributes = FileAttributes.Hidden;
						}
					}
					using (FileStream fileStream = File.Open(this.ImageFilePath, FileMode.Open, FileAccess.Read))
					{
						Pixbuf bigImage = new Pixbuf(fileStream);
						Gdk.Size bigImageSize = new Gdk.Size(bigImage.Width, bigImage.Height);
						bool isFull = true;
						Parallel.ForEach<ImageInfo>(this.ImageList, delegate(ImageInfo item)
						{
							Pixbuf pixbuf = null;
							try
							{
								if (this.CheckSubImageBorder(item, bigImageSize) && isFull)
								{
									if (isRetainEdge)
									{
										pixbuf = this.CreateSubImageWithEdge(bigImage, item);
									}
									else
									{
										pixbuf = this.CreateSubImage(bigImage, item);
									}
									pixbuf.Save(Path.Combine(dirPath, item.FileName), "png");
									pixbuf.Dispose();
									pixbuf = null;
								}
								else
								{
									isFull = false;
								}
							}
							catch (Exception message)
							{
								LogConfig.Logger.Error(message);
							}
							finally
							{
								if (pixbuf != null)
								{
									pixbuf.Dispose();
									pixbuf = null;
								}
							}
						});
						bigImage.Dispose();
						bigImage = null;
						if (!isFull)
						{
							throw new InvalidOperationException("Plist file error");
						}
					}
					GCHelper.QuickCollect();
				}
			}
		}

		private bool CheckSubImageBorder(ImageInfo info, Gdk.Size bigImageSize)
		{
			int num = info.Bounding.Width;
			int num2 = info.Bounding.Height;
			if (info.IsRotation)
			{
				num = info.Bounding.Height;
				num2 = info.Bounding.Width;
			}
			return info.Bounding.X + num <= bigImageSize.Width && info.Bounding.Y + num2 <= bigImageSize.Height;
		}

		private Pixbuf CreateSubImage(Pixbuf bigImage, ImageInfo imageInfo)
		{
			System.Drawing.Rectangle bounding = imageInfo.Bounding;
			System.Drawing.Rectangle rectangle = bounding;
			if (imageInfo.IsRotation)
			{
				rectangle = new System.Drawing.Rectangle(bounding.X, bounding.Y, bounding.Height, bounding.Width);
			}
			Pixbuf pixbuf = new Pixbuf(bigImage, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
			if (imageInfo.IsRotation)
			{
				pixbuf = pixbuf.RotateSimple(PixbufRotation.Counterclockwise);
			}
			return pixbuf;
		}

		private Pixbuf CreateSubImageWithEdge(Pixbuf bigImage, ImageInfo imageInfo)
		{
			Pixbuf result;
			using (Pixbuf pixbuf = this.CreateSubImage(bigImage, imageInfo))
			{
				Pixbuf pixbuf2 = new Pixbuf(pixbuf.Colorspace, pixbuf.HasAlpha, pixbuf.BitsPerSample, imageInfo.SourceSize.Width, imageInfo.SourceSize.Height);
				pixbuf2.Fill(0U);
				pixbuf.CopyArea(0, 0, pixbuf.Width, pixbuf.Height, pixbuf2, imageInfo.SourceLocation.X, imageInfo.SourceLocation.Y);
				result = pixbuf2;
			}
			return result;
		}

		private PlistImageFormat plistFormatAnalysis;

		private PListDict rootElement;

		private string plistFilePath;

		private List<ImageInfo> imageList;
	}
}
