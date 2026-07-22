using System;
using System.Drawing;

namespace Modules.Communal.Packer
{
	// Token: 0x0200000D RID: 13
	public struct ImageInfo
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00004178 File Offset: 0x00002378
		// (set) Token: 0x0600004D RID: 77 RVA: 0x0000418F File Offset: 0x0000238F
		public string Name { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00004198 File Offset: 0x00002398
		// (set) Token: 0x0600004F RID: 79 RVA: 0x000041AF File Offset: 0x000023AF
		public Rectangle Bounding { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000041B8 File Offset: 0x000023B8
		// (set) Token: 0x06000051 RID: 81 RVA: 0x000041CF File Offset: 0x000023CF
		public bool IsRotation { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000041D8 File Offset: 0x000023D8
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000041EF File Offset: 0x000023EF
		public Size SourceSize { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000041F8 File Offset: 0x000023F8
		// (set) Token: 0x06000055 RID: 85 RVA: 0x0000420F File Offset: 0x0000240F
		public Point SourceLocation { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00004218 File Offset: 0x00002418
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

		// Token: 0x06000057 RID: 87 RVA: 0x00004255 File Offset: 0x00002455
		public ImageInfo(string name, Rectangle bounding, Size sourceSize, Point sourceLocation, bool isRotation)
		{
			this = new ImageInfo(name, bounding, sourceSize, sourceLocation);
			this.IsRotation = isRotation;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x0000426E File Offset: 0x0000246E
		public ImageInfo(string name, Rectangle bounding, Size sourceSize, Point sourceLocation)
		{
			this = default(ImageInfo);
			this.Name = name;
			this.Bounding = bounding;
			this.SourceSize = sourceSize;
			this.SourceLocation = sourceLocation;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x0000429C File Offset: 0x0000249C
		private static string ConvertNameToFileName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException("名称不能为空");
			}
			string text = name.Replace('/', '_');
			return text.Replace(':', '_');
		}

		// Token: 0x04000020 RID: 32
		private string fileName;
	}
}
