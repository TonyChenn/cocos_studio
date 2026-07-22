using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Gdk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component.GUI
{
	// Token: 0x0200001D RID: 29
	[Extension(typeof(IJsonModel))]
	[DataContract]
	internal class ComGUIExportSurrogate : BaseEntitySurrogate
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00005715 File Offset: 0x00003915
		// (set) Token: 0x06000144 RID: 324 RVA: 0x0000571D File Offset: 0x0000391D
		[DataMember]
		public WidgetSurrogate options { get; set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00005726 File Offset: 0x00003926
		// (set) Token: 0x06000146 RID: 326 RVA: 0x0000572E File Offset: 0x0000392E
		[DataMember]
		public List<ComGUIExportSurrogate> children { get; set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00005737 File Offset: 0x00003937
		// (set) Token: 0x06000148 RID: 328 RVA: 0x0000573F File Offset: 0x0000393F
		public ComGUIExportSurrogate parent { get; set; }

		// Token: 0x06000149 RID: 329 RVA: 0x00005748 File Offset: 0x00003948
		protected ComGUIExportSurrogate()
		{
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005750 File Offset: 0x00003950
		public override void SetValue(object obj)
		{
			AbstractNodeObjectData abstractNodeObjectData = obj as AbstractNodeObjectData;
			AbstractNodeObjectData abstractNodeObjectData2;
			if (this.options.classname == "Panel")
			{
				abstractNodeObjectData2 = new PanelObjectData();
			}
			else if (this.options.classname == "Button")
			{
				abstractNodeObjectData2 = new ButtonObjectData();
			}
			else if (this.options.classname == "CheckBox")
			{
				abstractNodeObjectData2 = new CheckBoxObjectData();
			}
			else if (this.options.classname == "ImageView")
			{
				abstractNodeObjectData2 = new ImageViewObjectData();
			}
			else if (this.options.classname == "TextAtlas" || this.options.classname == "LabelAtlas")
			{
				abstractNodeObjectData2 = new TextAtlasObjectData();
			}
			else if (this.options.classname == "LabelBMFont" || this.options.classname == "TextBMFont")
			{
				abstractNodeObjectData2 = new TextBMFontObjectData();
			}
			else if (this.options.classname == "Text")
			{
				abstractNodeObjectData2 = new TextObjectData();
			}
			else if (this.options.classname == "LoadingBar")
			{
				abstractNodeObjectData2 = new LoadingBarObjectData();
			}
			else if (this.options.classname == "TextField")
			{
				abstractNodeObjectData2 = new TextFieldObjectData();
			}
			else if (this.options.classname == "Slider")
			{
				abstractNodeObjectData2 = new SliderObjectData();
			}
			else if (this.options.classname == "ScrollView")
			{
				abstractNodeObjectData2 = new ScrollViewObjectData();
			}
			else if (this.options.classname == "ListView")
			{
				abstractNodeObjectData2 = new ListViewObjectData();
			}
			else if (this.options.classname == "PageView")
			{
				abstractNodeObjectData2 = new PageViewObjectData();
			}
			else
			{
				if (!(this.options.classname == "Label"))
				{
					JsonFileHelp.ReportWarning(string.Format("{0} is {1}, {2}", this.options.name, this.options.classname, LanguageInfo.MessageBox208_NotSupported));
					return;
				}
				abstractNodeObjectData2 = new TextObjectData();
			}
			JsonFileHelp.ReportWarning(string.Format("{0} is {1}, {2}{3}", new object[]
			{
				this.options.name,
				this.options.classname,
				LanguageInfo.MessageBox208_NotSupported,
				LanguageInfo.UILayoutContainer_LayOut_Relative
			}));
			this.options.SetValue(abstractNodeObjectData2);
			abstractNodeObjectData.Children.Add(abstractNodeObjectData2);
			if (this.children != null && this.children.Count > 0)
			{
				if (abstractNodeObjectData2.Children == null)
				{
					abstractNodeObjectData2.Children = new List<AbstractNodeObjectData>();
				}
				foreach (ComGUIExportSurrogate comGUIExportSurrogate in this.children)
				{
					comGUIExportSurrogate.SetValue(abstractNodeObjectData2);
				}
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005A60 File Offset: 0x00003C60
		public void ConvertScrollViewChildrenPercentValue(AbstractNodeObjectData newObj)
		{
			float num = 1f;
			float num2 = 1f;
			bool flag = newObj is ScrollViewObjectData && !(newObj is ListViewObjectData) && !(newObj is PageViewObjectData);
			if (flag)
			{
				ScrollViewObjectData scrollViewObjectData = newObj as ScrollViewObjectData;
				num = scrollViewObjectData.Size.Width / (float)scrollViewObjectData.InnerNodeSize.Width;
				num2 = scrollViewObjectData.Size.Height / (float)scrollViewObjectData.InnerNodeSize.Height;
			}
			if (newObj.Children == null || newObj.Children.Count == 0)
			{
				return;
			}
			foreach (AbstractNodeObjectData abstractNodeObjectData in newObj.Children)
			{
				if (flag)
				{
					WidgetObjectData widgetObjectData = abstractNodeObjectData as WidgetObjectData;
					widgetObjectData.PrePosition.X *= num;
					widgetObjectData.PrePosition.Y *= num2;
					widgetObjectData.PreSize = new SizeF(widgetObjectData.PreSize.Width * num, widgetObjectData.PreSize.Height * num2);
				}
				this.ConvertScrollViewChildrenPercentValue(abstractNodeObjectData);
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005B90 File Offset: 0x00003D90
		public void ConvertWidgetPositionFromLayout(AbstractNodeObjectData newObj)
		{
			WidgetObjectData widgetObjectData = newObj as WidgetObjectData;
			if (widgetObjectData == null)
			{
				return;
			}
			WidgetSurrogate options = this.options;
			WidgetSurrogate widgetSurrogate = (this.parent == null) ? null : this.parent.options;
			LayoutSurrogate layoutParameter = options.layoutParameter;
			switch ((layoutParameter == null) ? -1 : layoutParameter.type)
			{
			case 0:
				widgetObjectData.Position = new PointF((float)((int)options.x), (float)((int)options.y));
				break;
			case 1:
				if (widgetSurrogate != null)
				{
					switch (layoutParameter.gravity)
					{
					case 1:
						widgetObjectData.Position.X = options.width * options.anchorPointX + (float)layoutParameter.marginLeft;
						widgetObjectData.Position.Y = widgetSurrogate.height - this.calculateLinearBrothersMargin(false) - options.height + options.height * options.anchorPointY - (float)layoutParameter.marginTop;
						break;
					case 2:
						widgetObjectData.Position.X = this.calculateLinearBrothersMargin(true) + options.width * options.anchorPointX + (float)layoutParameter.marginLeft;
						widgetObjectData.Position.Y = widgetSurrogate.height - options.height + options.height * options.anchorPointY - (float)layoutParameter.marginTop;
						break;
					case 3:
						widgetObjectData.Position.X = widgetSurrogate.width - options.width + options.width * options.anchorPointX + (float)layoutParameter.marginLeft;
						widgetObjectData.Position.Y = widgetSurrogate.height - this.calculateLinearBrothersMargin(false) - options.height + options.height * options.anchorPointY - (float)layoutParameter.marginTop;
						break;
					case 4:
						widgetObjectData.Position.X = this.calculateLinearBrothersMargin(true) + options.width * options.anchorPointX + (float)layoutParameter.marginLeft;
						widgetObjectData.Position.Y = options.height * options.anchorPointY - (float)layoutParameter.marginTop;
						break;
					case 5:
						widgetObjectData.Position.X = this.calculateLinearBrothersMargin(true) + options.width * options.anchorPointX + (float)layoutParameter.marginLeft;
						widgetObjectData.Position.Y = widgetSurrogate.height / 2f - options.height / 2f + options.height * options.anchorPointY;
						break;
					case 6:
						widgetObjectData.Position.X = widgetSurrogate.width / 2f - options.width / 2f + options.width * options.anchorPointX;
						widgetObjectData.Position.Y = widgetSurrogate.height - this.calculateLinearBrothersMargin(false) - options.height + options.height * options.anchorPointY - (float)layoutParameter.marginTop;
						break;
					}
				}
				break;
			case 2:
				if (widgetSurrogate != null && !(layoutParameter.relativeToName != widgetSurrogate.name))
				{
					switch (layoutParameter.align)
					{
					case 1:
						widgetObjectData.Position.X = options.width * options.anchorPointX + (float)layoutParameter.marginLeft;
						widgetObjectData.Position.Y = widgetSurrogate.height - options.height + options.height * options.anchorPointY - (float)layoutParameter.marginTop;
						break;
					case 2:
						widgetObjectData.Position.X = widgetSurrogate.width / 2f - options.width / 2f + options.width * options.anchorPointX;
						widgetObjectData.Position.Y = widgetSurrogate.height - options.height + options.height * options.anchorPointY - (float)layoutParameter.marginTop;
						break;
					case 3:
						widgetObjectData.Position.X = widgetSurrogate.width - options.width + options.width * options.anchorPointX - (float)layoutParameter.marginRight;
						widgetObjectData.Position.Y = widgetSurrogate.height - options.height + options.height * options.anchorPointY - (float)layoutParameter.marginTop;
						break;
					case 4:
						widgetObjectData.Position.X = options.width * options.anchorPointX + (float)layoutParameter.marginLeft;
						widgetObjectData.Position.Y = widgetSurrogate.height / 2f - options.height / 2f + options.height * options.anchorPointY;
						break;
					case 5:
						widgetObjectData.Position.X = widgetSurrogate.width / 2f - options.width / 2f + options.width * options.anchorPointX;
						widgetObjectData.Position.Y = widgetSurrogate.height / 2f - options.height / 2f + options.height * options.anchorPointY;
						break;
					case 6:
						widgetObjectData.Position.X = widgetSurrogate.width - options.width + options.width * options.anchorPointX - (float)layoutParameter.marginRight;
						widgetObjectData.Position.Y = widgetSurrogate.height / 2f - options.height / 2f + options.height * options.anchorPointY;
						break;
					case 7:
						widgetObjectData.Position.X = options.width * options.anchorPointX + (float)layoutParameter.marginLeft;
						widgetObjectData.Position.Y = options.height * options.anchorPointY + (float)layoutParameter.marginDown;
						break;
					case 8:
						widgetObjectData.Position.X = widgetSurrogate.width / 2f - options.width / 2f + options.width * options.anchorPointX;
						widgetObjectData.Position.Y = options.height * options.anchorPointY + (float)layoutParameter.marginDown;
						break;
					case 9:
						widgetObjectData.Position.X = widgetSurrogate.width - options.width + options.width * options.anchorPointX - (float)layoutParameter.marginRight;
						widgetObjectData.Position.Y = options.height * options.anchorPointY + (float)layoutParameter.marginDown;
						break;
					}
				}
				break;
			}
			if (newObj.Children == null || newObj.Children.Count < this.children.Count)
			{
				return;
			}
			for (int i = 0; i < this.children.Count; i++)
			{
				ComGUIExportSurrogate comGUIExportSurrogate = this.children[i];
				AbstractNodeObjectData newObj2 = newObj.Children[i];
				comGUIExportSurrogate.ConvertWidgetPositionFromLayout(newObj2);
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00006260 File Offset: 0x00004460
		private float calculateLinearBrothersMargin(bool isHorizontal)
		{
			float num = 0f;
			if (isHorizontal)
			{
				using (List<ComGUIExportSurrogate>.Enumerator enumerator = this.parent.children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ComGUIExportSurrogate comGUIExportSurrogate = enumerator.Current;
						if (this.options.name == comGUIExportSurrogate.options.name)
						{
							break;
						}
						num += comGUIExportSurrogate.options.width;
						num += (float)comGUIExportSurrogate.options.layoutParameter.marginLeft;
						num += (float)comGUIExportSurrogate.options.layoutParameter.marginRight;
					}
					return num;
				}
			}
			foreach (ComGUIExportSurrogate comGUIExportSurrogate2 in this.parent.children)
			{
				if (this.options.name == comGUIExportSurrogate2.options.name)
				{
					break;
				}
				num += comGUIExportSurrogate2.options.height;
				num += (float)comGUIExportSurrogate2.options.layoutParameter.marginTop;
				num += (float)comGUIExportSurrogate2.options.layoutParameter.marginDown;
			}
			return num;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000063A8 File Offset: 0x000045A8
		public virtual void RefreshChildPropertyAfferInitFor3X()
		{
			Point point = new Point((int)this.options.width, (int)this.options.height);
			ScaleValue scaleValue = new ScaleValue(this.options.anchorPointX, this.options.anchorPointY, 0.1, -99999999.0, 99999999.0);
			foreach (ComGUIExportSurrogate comGUIExportSurrogate in this.children)
			{
				comGUIExportSurrogate.parent = this;
				if (comGUIExportSurrogate.options.positionType == 1)
				{
					ScaleValue scaleValue2 = new ScaleValue(comGUIExportSurrogate.options.positionPercentX, comGUIExportSurrogate.options.positionPercentY, 0.1, -99999999.0, 99999999.0);
					comGUIExportSurrogate.options.positionPercentX = scaleValue2.ScaleX + scaleValue.ScaleX;
					comGUIExportSurrogate.options.positionPercentY = scaleValue2.ScaleY + scaleValue.ScaleY;
				}
				else
				{
					Point point2 = new Point((int)comGUIExportSurrogate.options.x, (int)comGUIExportSurrogate.options.y);
					int num = this.children.IndexOf(comGUIExportSurrogate);
					if (num != 0)
					{
						ListViewSurrogate listViewSurrogate = this.options as ListViewSurrogate;
						if (listViewSurrogate != null)
						{
							if (listViewSurrogate.direction == 2)
							{
								scaleValue.ScaleX = 0f;
							}
							else if (listViewSurrogate.direction == 1)
							{
								scaleValue.ScaleY = 0f;
							}
						}
					}
					comGUIExportSurrogate.options.x = (float)point2.X + (float)point.X * scaleValue.ScaleX;
					comGUIExportSurrogate.options.y = (float)point2.Y + (float)point.Y * scaleValue.ScaleY;
				}
				comGUIExportSurrogate.RefreshChildPropertyAfferInitFor3X();
			}
		}

		// Token: 0x04000084 RID: 132
		private const string classname_Panel = "Panel";

		// Token: 0x04000085 RID: 133
		private const string classname_Button = "Button";

		// Token: 0x04000086 RID: 134
		private const string classname_CheckBox = "CheckBox";

		// Token: 0x04000087 RID: 135
		private const string classname_ImageView = "ImageView";

		// Token: 0x04000088 RID: 136
		private const string classname_TextAtlas = "TextAtlas";

		// Token: 0x04000089 RID: 137
		private const string classname_LabelAtlas = "LabelAtlas";

		// Token: 0x0400008A RID: 138
		private const string classname_LabelBMFont = "LabelBMFont";

		// Token: 0x0400008B RID: 139
		private const string classname_TextBMFont = "TextBMFont";

		// Token: 0x0400008C RID: 140
		private const string classname_Text = "Text";

		// Token: 0x0400008D RID: 141
		private const string classname_LoadingBar = "LoadingBar";

		// Token: 0x0400008E RID: 142
		private const string classname_TextField = "TextField";

		// Token: 0x0400008F RID: 143
		private const string classname_Slider = "Slider";

		// Token: 0x04000090 RID: 144
		private const string classname_Layout = "Layout";

		// Token: 0x04000091 RID: 145
		private const string classname_ScrollView = "ScrollView";

		// Token: 0x04000092 RID: 146
		private const string classname_ListView = "ListView";

		// Token: 0x04000093 RID: 147
		private const string classname_PageView = "PageView";

		// Token: 0x04000094 RID: 148
		private const string classname_Widget = "Widget";

		// Token: 0x04000095 RID: 149
		private const string classname_Label = "Label";
	}
}
