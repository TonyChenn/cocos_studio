using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Mono.Addins;
using Xwt.Drawing;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x0200001A RID: 26
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class WidgetSurrogate : BaseEntitySurrogate
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004B9E File Offset: 0x00002D9E
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00004BA6 File Offset: 0x00002DA6
		[DataMember]
		public virtual float x { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004BAF File Offset: 0x00002DAF
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00004BB7 File Offset: 0x00002DB7
		[DataMember]
		public virtual float y { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004BC0 File Offset: 0x00002DC0
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00004BC8 File Offset: 0x00002DC8
		[DataMember]
		[DefaultValue(1f)]
		public virtual float scaleX { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00004BD1 File Offset: 0x00002DD1
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00004BD9 File Offset: 0x00002DD9
		[DefaultValue(1f)]
		[DataMember]
		public virtual float scaleY { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004BE2 File Offset: 0x00002DE2
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00004BEA File Offset: 0x00002DEA
		[DataMember]
		[DefaultValue(0.0)]
		public virtual float rotation { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00004BF3 File Offset: 0x00002DF3
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00004BFB File Offset: 0x00002DFB
		[DataMember]
		public virtual bool flipX { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004C04 File Offset: 0x00002E04
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00004C0C File Offset: 0x00002E0C
		[DataMember]
		public virtual bool flipY { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004C15 File Offset: 0x00002E15
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00004C1D File Offset: 0x00002E1D
		[DefaultValue(255)]
		[DataMember]
		public virtual int colorR { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004C26 File Offset: 0x00002E26
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00004C2E File Offset: 0x00002E2E
		[DefaultValue(255)]
		[DataMember]
		public virtual int colorG { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004C37 File Offset: 0x00002E37
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00004C3F File Offset: 0x00002E3F
		[DefaultValue(255)]
		[DataMember]
		public virtual int colorB { get; set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00004C48 File Offset: 0x00002E48
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00004C50 File Offset: 0x00002E50
		[DataMember]
		[DefaultValue(255)]
		public virtual int opacity { get; set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00004C59 File Offset: 0x00002E59
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00004C61 File Offset: 0x00002E61
		[DataMember]
		public virtual bool touchAble { get; set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00004C6A File Offset: 0x00002E6A
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00004C72 File Offset: 0x00002E72
		[DataMember]
		public virtual bool visible { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00004C7B File Offset: 0x00002E7B
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00004C83 File Offset: 0x00002E83
		[DataMember]
		public virtual int ZOrder { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004C8C File Offset: 0x00002E8C
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00004C94 File Offset: 0x00002E94
		[DataMember]
		public virtual string classType { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00004C9D File Offset: 0x00002E9D
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00004CA5 File Offset: 0x00002EA5
		[DataMember]
		public virtual float width { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00004CAE File Offset: 0x00002EAE
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00004CB6 File Offset: 0x00002EB6
		[DataMember]
		public virtual float height { get; set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00004CBF File Offset: 0x00002EBF
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00004CC7 File Offset: 0x00002EC7
		[DataMember]
		public virtual int positionType { get; set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00004CD0 File Offset: 0x00002ED0
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00004CD8 File Offset: 0x00002ED8
		[DataMember]
		public virtual float positionPercentX { get; set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00004CE1 File Offset: 0x00002EE1
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00004CE9 File Offset: 0x00002EE9
		[DataMember]
		public virtual float positionPercentY { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00004CF2 File Offset: 0x00002EF2
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00004CFA File Offset: 0x00002EFA
		[DataMember]
		public virtual int sizeType { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00004D03 File Offset: 0x00002F03
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00004D0B File Offset: 0x00002F0B
		[DataMember]
		public virtual float sizePercentX { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00004D14 File Offset: 0x00002F14
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00004D1C File Offset: 0x00002F1C
		[DataMember]
		public virtual float sizePercentY { get; set; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00004D25 File Offset: 0x00002F25
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00004D2D File Offset: 0x00002F2D
		[DataMember]
		public virtual bool useMergedTexture { get; set; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00004D36 File Offset: 0x00002F36
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00004D3E File Offset: 0x00002F3E
		[DataMember]
		public virtual int actionTag { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00004D47 File Offset: 0x00002F47
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00004D4F File Offset: 0x00002F4F
		[DataMember]
		public virtual int tag { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00004D58 File Offset: 0x00002F58
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00004D60 File Offset: 0x00002F60
		[DefaultValue(0.5f)]
		[DataMember]
		public virtual float anchorPointX { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00004D69 File Offset: 0x00002F69
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00004D71 File Offset: 0x00002F71
		[DataMember]
		[DefaultValue(0.5f)]
		public virtual float anchorPointY { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00004D7A File Offset: 0x00002F7A
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00004D82 File Offset: 0x00002F82
		[DataMember]
		[DefaultValue(null)]
		public virtual bool ignoreSize { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00004D8B File Offset: 0x00002F8B
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00004D93 File Offset: 0x00002F93
		[DataMember]
		public virtual LayoutSurrogate layoutParameter { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004D9C File Offset: 0x00002F9C
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00004DA4 File Offset: 0x00002FA4
		[DefaultValue("")]
		[DataMember]
		public virtual string customProperty { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00004DAD File Offset: 0x00002FAD
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00004DB5 File Offset: 0x00002FB5
		[DataMember]
		[DefaultValue("")]
		public virtual string frameEvent { get; set; }

		// Token: 0x060000FA RID: 250 RVA: 0x00004DBE File Offset: 0x00002FBE
		protected WidgetSurrogate()
		{
			this.InitDefaultValue();
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004DD3 File Offset: 0x00002FD3
		public WidgetSurrogate(string className)
		{
			this.classname = this.classname;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004DF0 File Offset: 0x00002FF0
		private void InitDefaultValue()
		{
			this.scaleX = 1f;
			this.scaleY = 1f;
			this.anchorPointX = 0.5f;
			this.anchorPointY = 0.5f;
			this.colorR = 255;
			this.colorG = 255;
			this.colorB = 255;
			this.opacity = 255;
			this.customProperty = "";
			this.frameEvent = "";
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004E6C File Offset: 0x0000306C
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			WidgetObjectData widgetObjectData = obj as WidgetObjectData;
			widgetObjectData.TouchEnable = this.touchAble;
			int sizeType = this.sizeType;
			widgetObjectData.Tag = this.tag;
			widgetObjectData.InnerClassName = widgetObjectData.InnerClassName;
			widgetObjectData.CanEdit = widgetObjectData.CanEdit;
			if (this.actionTag != 0)
			{
				widgetObjectData.ActionTag = this.actionTag;
			}
			widgetObjectData.Position = new PointF((float)((int)this.x), (float)((int)this.y));
			widgetObjectData.Scale = new ScaleValue(this.scaleX, this.scaleY, 0.1, -99999999.0, 99999999.0);
			widgetObjectData.RotationSkewX = (widgetObjectData.RotationSkewY = this.rotation);
			widgetObjectData.ZOrder = this.ZOrder;
			widgetObjectData.VisibleForFrame = this.visible;
			widgetObjectData.AnchorPoint = new ScaleValue(this.anchorPointX, this.anchorPointY, 0.1, -99999999.0, 99999999.0);
			widgetObjectData.Alpha = this.opacity;
			widgetObjectData.CColor = new ColorData((byte)this.opacity, (byte)this.colorR, (byte)this.colorG, (byte)this.colorB);
			widgetObjectData.IsAutoSize = this.ignoreSize;
			widgetObjectData.PositionPercentXEnabled = (this.positionType == 1);
			widgetObjectData.PositionPercentYEnabled = (this.positionType == 1);
			widgetObjectData.PrePosition = new PointF(this.positionPercentX, this.positionPercentY);
			widgetObjectData.PercentWidthEnable = (this.sizeType == 1);
			widgetObjectData.PercentHeightEnable = (this.sizeType == 1);
			widgetObjectData.PreSize = new SizeF(this.sizePercentX, this.sizePercentY);
			widgetObjectData.Size = new SizeF(this.width, this.height);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005040 File Offset: 0x00003240
		protected static ResourceItemData ConvertResourceData(ResourceDataSurrogate resData)
		{
			ResourceItemData resourceItemData = null;
			if (resData != null && !string.IsNullOrEmpty(resData.path) && resData.resourceType != 2 && resData.resourceType != -1)
			{
				if (resData.resourceType == 0 || !string.IsNullOrEmpty(resData.plistFile) || JsonFileHelp.plistfilehelper.isEmpty())
				{
					if (!JsonFileHelp.isBasedProject)
					{
						string resAbsPath = JsonFileHelp.GetResAbsPath(resData.path);
						string resRelativePath = JsonFileHelp.GetResRelativePath(resAbsPath);
						resourceItemData = new ResourceItemData((EnumResourceType)resData.resourceType, resRelativePath, resData.plistFile);
					}
					else
					{
						resourceItemData = new ResourceItemData((EnumResourceType)resData.resourceType, resData.path, resData.plistFile);
					}
				}
				else
				{
					string path = resData.path;
					string plistFile = JsonFileHelp.plistfilehelper.FindPlistFile(path);
					resourceItemData = new ResourceItemData(EnumResourceType.PlistSubImage, path, plistFile);
				}
			}
			if (resourceItemData != null && resourceItemData.Type != EnumResourceType.PlistSubImage)
			{
				string path2 = Services.ProjectsService.GetFullPath(resourceItemData);
				if (!File.Exists(path2))
				{
					resourceItemData = null;
				}
			}
			return resourceItemData;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005138 File Offset: 0x00003338
		protected void TransFormScale9Value(ResourceItemData fileData, int originX, int originY, int width, int height, out int left, out int right, out int top, out int bottom)
		{
			string text = fileData.Path;
			if (fileData.Type == EnumResourceType.PlistSubImage)
			{
				string directoryName = Path.GetDirectoryName(fileData.Plist);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileData.Plist);
				text = Path.Combine(directoryName, "." + fileNameWithoutExtension + "_Plist.Dir", text);
				text.Replace(':', '_');
				text.Replace('/', '_');
				text = ((MonoDevelop.Core.FilePath)text).ToAbsolute(Services.ProjectOperations.CurrentResourceGroup.RootFolder.FullPath);
			}
			else if (fileData.Type == EnumResourceType.Normal)
			{
				text = ((MonoDevelop.Core.FilePath)text).ToAbsolute(Services.ProjectOperations.CurrentResourceGroup.RootFolder.FullPath);
			}
			if (!File.Exists(text))
			{
				LogConfig.Logger.Error(string.Format("File Not Found {0}", text));
				left = 100;
				right = 100;
				bottom = 100;
				top = 100;
				return;
			}
			left = (right = (top = (bottom = 0)));
			Image image = Image.FromFile(text);
			left = originX;
			right = (int)image.Width - width - originX;
			bottom = originY;
			top = (int)image.Height - height - originY;
		}

		// Token: 0x04000044 RID: 68
		public bool bCanBatch = true;
	}
}
