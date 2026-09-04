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
	// Token: 0x0200001F RID: 31
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class LabelAtlasSurrogate : WidgetSurrogate
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00006755 File Offset: 0x00004955
		// (set) Token: 0x06000164 RID: 356 RVA: 0x0000675D File Offset: 0x0000495D
		[DataMember]
		[DefaultValue("12345678")]
		public string stringValue { get; set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00006766 File Offset: 0x00004966
		// (set) Token: 0x06000166 RID: 358 RVA: 0x0000676E File Offset: 0x0000496E
		[DataMember]
		public string charMapFile { get; set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00006777 File Offset: 0x00004977
		// (set) Token: 0x06000168 RID: 360 RVA: 0x0000677F File Offset: 0x0000497F
		[DataMember]
		public ResourceDataSurrogate charMapFileData { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00006788 File Offset: 0x00004988
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00006790 File Offset: 0x00004990
		[DataMember]
		public string startCharMap { get; set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00006799 File Offset: 0x00004999
		// (set) Token: 0x0600016C RID: 364 RVA: 0x000067A1 File Offset: 0x000049A1
		[DefaultValue(24)]
		[DataMember]
		public int itemWidth { get; set; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600016D RID: 365 RVA: 0x000067AA File Offset: 0x000049AA
		// (set) Token: 0x0600016E RID: 366 RVA: 0x000067B2 File Offset: 0x000049B2
		[DefaultValue(32)]
		[DataMember]
		public int itemHeight { get; set; }

		// Token: 0x0600016F RID: 367 RVA: 0x000067BB File Offset: 0x000049BB
		protected LabelAtlasSurrogate()
		{
			this.itemWidth = 24;
			this.itemHeight = 32;
			this.stringValue = "12345678";
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000067E0 File Offset: 0x000049E0
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
