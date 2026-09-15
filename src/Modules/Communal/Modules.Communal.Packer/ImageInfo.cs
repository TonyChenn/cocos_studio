using System;
using System.Drawing;

namespace Modules.Communal.Packer
{
	public struct ImageInfo
	{
		public string Name { get; private set; }

		public Rectangle Bounding { get; private set; }

		public bool IsRotation { get; private set; }

		public Size SourceSize { get; private set; }

		public Point SourceLocation { get; private set; }

		public string FileName
		{
			get
			{
				if (string.IsNullOrEmpty(this.fileName))
				{
					this.fileName = ImageInfo.ConvertNameToFileName(this.Name);
				}
				return this.fileName;
			}
		}

		public ImageInfo(string name, Rectangle bounding, Size sourceSize, Point sourceLocation, bool isRotation)
		{
			this = new ImageInfo(name, bounding, sourceSize, sourceLocation);
			this.IsRotation = isRotation;
		}

		public ImageInfo(string name, Rectangle bounding, Size sourceSize, Point sourceLocation)
		{
			this = default(ImageInfo);
			this.Name = name;
			this.Bounding = bounding;
			this.SourceSize = sourceSize;
			this.SourceLocation = sourceLocation;
		}

		private static string ConvertNameToFileName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("名称不能为空");
			}
			string text = name.Replace('/', '_');
			return text.Replace(':', '_');
		}

		private string fileName;
	}
}
