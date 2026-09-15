using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Model.ViewModel;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ListViewSurrogate : WidgetSurrogate
	{
		[DataMember]
		public string backGroundImage { get; set; }

		[DataMember]
		public ResourceDataSurrogate backGroundImageData { get; set; }

		[DefaultValue(150)]
		[DataMember]
		public int bgColorR { get; set; }

		[DataMember]
		[DefaultValue(150)]
		public int bgColorG { get; set; }

		[DataMember]
		[DefaultValue(255)]
		public int bgColorB { get; set; }

		[DefaultValue(255)]
		[DataMember]
		public int bgStartColorR { get; set; }

		[DataMember]
		[DefaultValue(255)]
		public int bgStartColorG { get; set; }

		[DataMember]
		[DefaultValue(255)]
		public int bgStartColorB { get; set; }

		[DefaultValue(150)]
		[DataMember]
		public int bgEndColorR { get; set; }

		[DataMember]
		[DefaultValue(150)]
		public int bgEndColorG { get; set; }

		[DefaultValue(255)]
		[DataMember]
		public int bgEndColorB { get; set; }

		[DataMember]
		[DefaultValue(1)]
		public int colorType { get; set; }

		[DefaultValue(100)]
		[DataMember]
		public int bgColorOpacity { get; set; }

		[DataMember]
		public float vectorX { get; set; }

		[DefaultValue(-0.5f)]
		[DataMember]
		public float vectorY { get; set; }

		[DataMember]
		public float capInsetsX { get; set; }

		[DataMember]
		public float capInsetsY { get; set; }

		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsWidth { get; set; }

		[DefaultValue(1f)]
		[DataMember]
		public float capInsetsHeight { get; set; }

		[DataMember]
		public bool backGroundScale9Enable { get; set; }

		[DataMember]
		public float innerWidth { get; set; }

		[DataMember]
		public float innerHeight { get; set; }

		[DataMember]
		public bool clipAble { get; set; }

		[DataMember]
		public bool bounceEnable { get; set; }

		[DefaultValue(2)]
		[DataMember]
		public int direction { get; set; }

		[DataMember]
		[DefaultValue(3)]
		public int gravity { get; set; }

		[DataMember]
		public int itemMargin { get; set; }

		protected ListViewSurrogate()
		{
			this.InitDefaultValue();
		}

		private void InitDefaultValue()
		{
			this.bgColorR = 150;
			this.bgColorG = 150;
			this.bgColorB = 255;
			this.bgStartColorR = 255;
			this.bgStartColorG = 255;
			this.bgStartColorB = 255;
			this.bgEndColorR = 150;
			this.bgEndColorG = 150;
			this.bgEndColorB = 255;
			this.colorType = 1;
			this.bgColorOpacity = 100;
			this.vectorY = -0.5f;
			this.capInsetsWidth = 1f;
			this.capInsetsHeight = 1f;
			this.direction = 2;
			this.gravity = 3;
		}

		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			ListViewObjectData listViewObjectData = obj as ListViewObjectData;
			listViewObjectData.ClipAble = this.clipAble;
			listViewObjectData.BackColorAlpha = this.bgColorOpacity;
			listViewObjectData.FileData = WidgetSurrogate.ConvertResourceData(this.backGroundImageData);
			listViewObjectData.ColorAngle = ValueConvertHelper.PointToAngle(this.vectorX, this.vectorY);
			listViewObjectData.ColorVector = ValueConvertHelper.AngleToVector(listViewObjectData.ColorAngle);
			listViewObjectData.ComboBoxIndex = this.colorType;
			listViewObjectData.SingleColor = new ColorData(byte.MaxValue, (byte)this.bgColorR, (byte)this.bgColorG, (byte)this.bgColorB);
			listViewObjectData.FirstColor = new ColorData(byte.MaxValue, (byte)this.bgStartColorR, (byte)this.bgStartColorG, (byte)this.bgStartColorB);
			listViewObjectData.EndColor = new ColorData(byte.MaxValue, (byte)this.bgEndColorR, (byte)this.bgEndColorG, (byte)this.bgEndColorB);
			listViewObjectData.IsBounceEnabled = this.bounceEnable;
			listViewObjectData.ScrollDirectionType = (ScrollViewDirectionType)this.direction;
			listViewObjectData.DirectionType = (ListViewDirectionType)this.direction;
			listViewObjectData.Size = new SizeF(this.width, this.height);
			listViewObjectData.InnerNodeSize = new SizeValue((int)this.innerWidth, (int)this.innerHeight);
			listViewObjectData.ItemMargin = this.itemMargin;
			listViewObjectData.HorizontalType = (ListViewHorizontal)this.gravity;
			listViewObjectData.VerticalType = (ListViewVertical)this.gravity;
			listViewObjectData.Scale9Enable = this.backGroundScale9Enable;
			if (listViewObjectData.Scale9Enable && listViewObjectData.FileData != null)
			{
				int leftEage;
				int rightEage;
				int topEage;
				int bottomEage;
				base.TransFormScale9Value(listViewObjectData.FileData, (int)this.capInsetsX, (int)this.capInsetsY, (int)this.capInsetsWidth, (int)this.capInsetsHeight, out leftEage, out rightEage, out topEage, out bottomEage);
				listViewObjectData.LeftEage = leftEage;
				listViewObjectData.RightEage = rightEage;
				listViewObjectData.TopEage = topEage;
				listViewObjectData.BottomEage = bottomEage;
				listViewObjectData.Scale9OriginX = (int)this.capInsetsX;
				listViewObjectData.Scale9OriginY = (int)this.capInsetsY;
				listViewObjectData.Scale9Width = (int)this.capInsetsWidth;
				listViewObjectData.Scale9Height = (int)this.capInsetsHeight;
			}
		}
	}
}
