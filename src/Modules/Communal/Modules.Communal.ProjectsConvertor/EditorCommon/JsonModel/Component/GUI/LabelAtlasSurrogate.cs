using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;
using MonoDevelop.Core;

namespace EditorCommon.JsonModel.Component.GUI
{
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class LabelAtlasSurrogate : WidgetSurrogate
	{
		[DataMember]
		[DefaultValue("12345678")]
		public string stringValue { get; set; }

		[DataMember]
		public string charMapFile { get; set; }

		[DataMember]
		public ResourceDataSurrogate charMapFileData { get; set; }

		[DataMember]
		public string startCharMap { get; set; }

		[DefaultValue(24)]
		[DataMember]
		public int itemWidth { get; set; }

		[DefaultValue(32)]
		[DataMember]
		public int itemHeight { get; set; }

		protected LabelAtlasSurrogate()
		{
			this.itemWidth = 24;
			this.itemHeight = 32;
			this.stringValue = "12345678";
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			TextAtlasObjectData textAtlasObjectData = obj as TextAtlasObjectData;
			ResourceItemData resourceItemData = WidgetSurrogate.ConvertResourceData(this.charMapFileData);
			if (resourceItemData == null)
			{
				this.itemWidth = 14;
				this.itemHeight = 18;
			}
			else
			{
				FilePath fullPath = Services.ProjectsService.GetFullPath(resourceItemData);
				using (FileStream fileStream = File.Open(fullPath, FileMode.Open, FileAccess.ReadWrite))
				{
					Bitmap bitmap = new Bitmap(fileStream);
					Bitmap bitmap2 = new Bitmap(this.itemWidth * 12, bitmap.Height);
					Graphics graphics = Graphics.FromImage(bitmap2);
					graphics.Clear(Color.Transparent);
					int num = bitmap.Width / this.itemWidth;
					if (num <= 12)
					{
						graphics.DrawImage(bitmap, this.itemWidth * (12 - num), 0, bitmap.Width, bitmap.Height);
					}
					else
					{
						graphics.DrawImage(bitmap, 0, 0, new Rectangle(this.itemWidth * (num - 12), 0, this.itemWidth * 12, this.itemHeight), GraphicsUnit.Pixel);
					}
					graphics.Dispose();
					bitmap.Dispose();
					fileStream.Seek(0L, SeekOrigin.Begin);
					bitmap2.Save(fileStream, ImageFormat.Png);
					bitmap2.Dispose();
				}
			}
			this.startCharMap = ".";
			textAtlasObjectData.LabelAtlasFileImage_CNB = resourceItemData;
			textAtlasObjectData.CharWidth = this.itemWidth;
			textAtlasObjectData.CharHeight = this.itemHeight;
			textAtlasObjectData.LabelText = this.stringValue;
			textAtlasObjectData.StartChar = this.startCharMap;
			textAtlasObjectData.PercentWidthEnable = false;
			textAtlasObjectData.PercentHeightEnable = false;
		}
	}
}
