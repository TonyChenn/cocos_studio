using System;
using System.Collections.Generic;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Basic
{
	public class ResolutionConfig : ICloneable
	{
		[ItemProperty("Name")]
		public string Name { get; set; }

		[ItemProperty("Order")]
		public int Order { get; set; }

		[ItemProperty("Width")]
		public int Width { get; set; }

		[ItemProperty("Height")]
		public int Height { get; set; }

		[ItemProperty("IsSelected")]
		public bool IsSelected { get; set; }

		public string Size
		{
			get
			{
				return string.Format("{0} * {1}", this.Width, this.Height);
			}
		}

		public ResolutionConfig()
		{
			this.Name = string.Empty;
		}

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

		public void Reverse()
		{
			int height = this.Height;
			this.Height = this.Width;
			this.Width = height;
		}

		public string GetDisplayText()
		{
			return string.Format("{0}({1})", this.Name, this.Size);
		}

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

		public override bool Equals(object obj)
		{
			ResolutionConfig resolutionConfig = obj as ResolutionConfig;
			return resolutionConfig != null && this.Name.Equals(resolutionConfig.Name) && this.Size.Equals(resolutionConfig.Size);
		}

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

		private const int defaultWidth = 960;

		private const int defaultHeight = 640;
	}
}
