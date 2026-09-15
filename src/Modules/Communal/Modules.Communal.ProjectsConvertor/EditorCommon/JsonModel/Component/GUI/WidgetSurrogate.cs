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
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class WidgetSurrogate : BaseEntitySurrogate
	{
		[DataMember]
		public virtual float x { get; set; }

		[DataMember]
		public virtual float y { get; set; }

		[DataMember]
		[DefaultValue(1f)]
		public virtual float scaleX { get; set; }

		[DefaultValue(1f)]
		[DataMember]
		public virtual float scaleY { get; set; }

		[DataMember]
		[DefaultValue(0.0)]
		public virtual float rotation { get; set; }

		[DataMember]
		public virtual bool flipX { get; set; }

		[DataMember]
		public virtual bool flipY { get; set; }

		[DefaultValue(255)]
		[DataMember]
		public virtual int colorR { get; set; }

		[DefaultValue(255)]
		[DataMember]
		public virtual int colorG { get; set; }

		[DefaultValue(255)]
		[DataMember]
		public virtual int colorB { get; set; }

		[DataMember]
		[DefaultValue(255)]
		public virtual int opacity { get; set; }

		[DataMember]
		public virtual bool touchAble { get; set; }

		[DataMember]
		public virtual bool visible { get; set; }

		[DataMember]
		public virtual int ZOrder { get; set; }

		[DataMember]
		public virtual string classType { get; set; }

		[DataMember]
		public virtual float width { get; set; }

		[DataMember]
		public virtual float height { get; set; }

		[DataMember]
		public virtual int positionType { get; set; }

		[DataMember]
		public virtual float positionPercentX { get; set; }

		[DataMember]
		public virtual float positionPercentY { get; set; }

		[DataMember]
		public virtual int sizeType { get; set; }

		[DataMember]
		public virtual float sizePercentX { get; set; }

		[DataMember]
		public virtual float sizePercentY { get; set; }

		[DataMember]
		public virtual bool useMergedTexture { get; set; }

		[DataMember]
		public virtual int actionTag { get; set; }

		[DataMember]
		public virtual int tag { get; set; }

		[DefaultValue(0.5f)]
		[DataMember]
		public virtual float anchorPointX { get; set; }

		[DataMember]
		[DefaultValue(0.5f)]
		public virtual float anchorPointY { get; set; }

		[DataMember]
		[DefaultValue(null)]
		public virtual bool ignoreSize { get; set; }

		[DataMember]
		public virtual LayoutSurrogate layoutParameter { get; set; }

		[DefaultValue("")]
		[DataMember]
		public virtual string customProperty { get; set; }

		[DataMember]
		[DefaultValue("")]
		public virtual string frameEvent { get; set; }

		protected WidgetSurrogate()
		{
			this.InitDefaultValue();
		}

		public WidgetSurrogate(string className)
		{
			this.classname = this.classname;
		}

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

		public bool bCanBatch = true;
	}
}
