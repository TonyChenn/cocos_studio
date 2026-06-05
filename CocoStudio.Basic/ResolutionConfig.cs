using System;
using System.Collections.Generic;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Basic
{
	// Token: 0x0200000D RID: 13
	public class ResolutionConfig : ICloneable
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000070 RID: 112 RVA: 0x000032C4 File Offset: 0x000014C4
		// (set) Token: 0x06000071 RID: 113 RVA: 0x000032DB File Offset: 0x000014DB
		[ItemProperty("Name")]
		public string Name { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000072 RID: 114 RVA: 0x000032E4 File Offset: 0x000014E4
		// (set) Token: 0x06000073 RID: 115 RVA: 0x000032FB File Offset: 0x000014FB
		[ItemProperty("Order")]
		public int Order { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00003304 File Offset: 0x00001504
		// (set) Token: 0x06000075 RID: 117 RVA: 0x0000331B File Offset: 0x0000151B
		[ItemProperty("Width")]
		public int Width { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00003324 File Offset: 0x00001524
		// (set) Token: 0x06000077 RID: 119 RVA: 0x0000333B File Offset: 0x0000153B
		[ItemProperty("Height")]
		public int Height { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003344 File Offset: 0x00001544
		// (set) Token: 0x06000079 RID: 121 RVA: 0x0000335B File Offset: 0x0000155B
		[ItemProperty("IsSelected")]
		public bool IsSelected { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003364 File Offset: 0x00001564
		public string Size
		{
			get
			{
				return string.Format("{0} * {1}", this.Width, this.Height);
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003396 File Offset: 0x00001596
		public ResolutionConfig()
		{
			this.Name = string.Empty;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000033B0 File Offset: 0x000015B0
		public ResolutionConfig(string size) : this()
		{
			if (string.IsNullOrEmpty(size))
			{
				this.Width = 960;
				this.Height = 640;
			}
			else
			{
				string[] array = size.Split(new char[]
				{
					'*'
				});
				if (array.Length != 2)
				{
					this.Width = 960;
					this.Height = 640;
				}
				else
				{
					int width;
					if (!int.TryParse(array[0], out width))
					{
						width = 960;
					}
					int height;
					if (!int.TryParse(array[1], out height))
					{
						height = 640;
					}
					this.Width = width;
					this.Height = height;
				}
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003464 File Offset: 0x00001664
		public void Reverse()
		{
			int height = this.Height;
			this.Height = this.Width;
			this.Width = height;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003490 File Offset: 0x00001690
		public string GetDisplayText()
		{
			return string.Format("{0}({1})", this.Name, this.Size);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000034B8 File Offset: 0x000016B8
		public static ResolutionConfig CreateDefaultResolution()
		{
			return new ResolutionConfig
			{
				IsSelected = true,
				Order = 1,
				Name = "iPhone 4/4S",
				Width = 960,
				Height = 640
			};
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003508 File Offset: 0x00001708
		public static List<ResolutionConfig> CreateDefaultList()
		{
			return new List<ResolutionConfig>
			{
				new ResolutionConfig
				{
					IsSelected = true,
					Order = 0,
					Name = "iPhone 3GS",
					Width = 480,
					Height = 320
				},
				new ResolutionConfig
				{
					IsSelected = true,
					Order = 1,
					Name = "iPhone 4/4S",
					Width = 960,
					Height = 640
				},
				new ResolutionConfig
				{
					IsSelected = true,
					Order = 2,
					Name = "iPhone 5/5S",
					Width = 1136,
					Height = 640
				},
				new ResolutionConfig
				{
					IsSelected = true,
					Order = 3,
					Name = "iPhone 6",
					Width = 1334,
					Height = 750
				},
				new ResolutionConfig
				{
					IsSelected = true,
					Order = 4,
					Name = "iPhone 6Plus",
					Width = 2208,
					Height = 1242
				},
				new ResolutionConfig
				{
					IsSelected = true,
					Order = 5,
					Name = "iPad",
					Width = 1024,
					Height = 768
				},
				new ResolutionConfig
				{
					IsSelected = true,
					Order = 6,
					Name = "iPad Retina",
					Width = 2048,
					Height = 1536
				},
				new ResolutionConfig
				{
					IsSelected = true,
					Order = 7,
					Name = "Android(WVGA)",
					Width = 800,
					Height = 480
				},
				new ResolutionConfig
				{
					IsSelected = true,
					Order = 8,
					Name = "Android(WVGA854)",
					Width = 854,
					Height = 480
				},
				new ResolutionConfig
				{
					IsSelected = true,
					Order = 9,
					Name = "Android(DVGA)",
					Width = 960,
					Height = 540
				},
				new ResolutionConfig
				{
					IsSelected = true,
					Order = 10,
					Name = "Android",
					Width = 1280,
					Height = 720
				},
				new ResolutionConfig
				{
					IsSelected = true,
					Order = 11,
					Name = "Android(WXGA2)",
					Width = 1280,
					Height = 800
				},
				new ResolutionConfig
				{
					IsSelected = true,
					Order = 12,
					Name = "Android",
					Width = 1280,
					Height = 1080
				}
			};
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000038C8 File Offset: 0x00001AC8
		public override bool Equals(object obj)
		{
			ResolutionConfig resolutionConfig = obj as ResolutionConfig;
			return resolutionConfig != null && this.Name.Equals(resolutionConfig.Name) && this.Size.Equals(resolutionConfig.Size);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003924 File Offset: 0x00001B24
		public object Clone()
		{
			return new ResolutionConfig
			{
				Name = this.Name,
				Width = this.Width,
				Height = this.Height,
				IsSelected = this.IsSelected,
				Order = this.Order
			};
		}

		// Token: 0x0400004F RID: 79
		private const int defaultWidth = 960;

		// Token: 0x04000050 RID: 80
		private const int defaultHeight = 640;
	}
}
