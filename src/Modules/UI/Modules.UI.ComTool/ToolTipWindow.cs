using System;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Unix;
using Pango;
using Stetic;

namespace Modules.UI.ComTool
{
	public class ToolTipWindow : Gtk.Window
	{
		protected virtual void Build()
		{
			Gui.Initialize(this);
			base.Name = "Modules.UI.ComTool.ToolTipWindow";
			base.Title = Catalog.GetString("ToolTipWindow");
			base.WindowPosition = WindowPosition.CenterOnParent;
			this.eventbox4 = new EventBox();
			this.eventbox4.Name = "eventbox4";
			this.alignment1 = new Gtk.Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment1.Name = "alignment1";
			this.vbox2 = new VBox();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			this.labName = new Label();
			this.labName.Name = "labName";
			this.labName.Xalign = 0f;
			this.labName.LabelProp = Catalog.GetString("按钮");
			this.vbox2.Add(this.labName);
			Box.BoxChild boxChild = (Box.BoxChild)this.vbox2[this.labName];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			this.labDescription = new Label();
			this.labDescription.Name = "labDescription";
			this.labDescription.Xalign = 0f;
			this.labDescription.LabelProp = Catalog.GetString("添加一个可以设置文本的按钮，可自定义按钮样式等属性");
			this.vbox2.Add(this.labDescription);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.vbox2[this.labDescription];
			boxChild2.Position = 1;
			boxChild2.Expand = true;
			boxChild2.Fill = true;
			this.labLink = new LabelLinkButton(null);
			this.labLink.Events = EventMask.ButtonPressMask;
			this.labLink.Name = "labLink";
			this.labLink.HeightRequest = 20;
			this.vbox2.Add(this.labLink);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.vbox2[this.labLink];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.alignment1.Add(this.vbox2);
			this.eventbox4.Add(this.alignment1);
			base.Add(this.eventbox4);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.DefaultWidth = 230;
			base.DefaultHeight = 110;
			this.labDescription.WidthRequest = 216;
		}

		public EnumToolTipOrientation ToolOrientation
		{
			get
			{
				return this.toolOrientation;
			}
			set
			{
				this.toolOrientation = value;
			}
		}

		public ToolTipWindow() : base(Gtk.WindowType.Popup)
		{
			this.Build();
			this.vbox2.Spacing = 4;
			base.SkipPagerHint = true;
			base.SkipTaskbarHint = true;
			base.BorderWidth = 6U;
			base.AllowGrow = true;
			base.AllowShrink = true;
			base.Title = "ToolTip";
			this.toolOrientation = EnumToolTipOrientation.Right;
			base.HeightRequest = 78;
			this.labLink.Label.Xalign = 0f;
			this.InitStyle();
		}

		private void InitStyle()
		{
			this.labDescription.LineWrap = true;
			FontDescription fontDescription = new FontDescription();
			fontDescription.AbsoluteSize = LabelStyleSetting.TooltipDesSize;
			FontDescription fontDescription2 = new FontDescription();
			fontDescription2.AbsoluteSize = LabelStyleSetting.TooltipDesSize;
			this.labName.ModifyFont(fontDescription2);
			this.labDescription.ModifyFont(fontDescription);
			base.ModifyBg(StateType.Normal, LabelStyleSetting.ToolTipBG);
			this.vbox2.ModifyBg(StateType.Normal, LabelStyleSetting.ToolTipBG);
			this.eventbox4.ModifyBg(StateType.Normal, LabelStyleSetting.ToolTipBG);
		}

		public void InitiToolTip(ComponentItem comitem)
		{
			this.componentItem = comitem;
			if (comitem != null && comitem.UIControlToolItem != null)
			{
				string displayName = comitem.UIControlToolItem.ModelMedaData.DisplayName;
				this.labName.LabelProp = comitem.UIControlToolItem.DisplayName;
				this.labDescription.LabelProp = LanguageOption.GetValueBykey(displayName + "_Description");
				if (comitem.UIControlToolItem.ModelMedaData.IsDefault)
				{
					string helpLink = this.GetHelpLink(comitem.UIControlToolItem.ModelMedaData.Type.Name);
					this.labLink.LabelText = LanguageInfo.guide_OnlineHelper;
					this.labLink.URL = helpLink;
					this.labLink.NormalColor = new Gdk.Color?(new Gdk.Color(104, 171, 254));
					this.labLink.Label.SetFontSize(12.0);
				}
				else
				{
					this.vbox2.Remove(this.labLink);
				}
			}
		}

		private string GetHelpLink(string widgetType)
		{
			string text = string.Empty;
			widgetType = widgetType.Substring(0, widgetType.Length - 6);
			string urlFormat = string.Format("http://cocostudio.org/help/2.0/{0}/", widgetType) + "{0}";
			text = LanguageAdapter.GetLocalizedUrl(urlFormat);
			return text.ToLower();
		}

		private EventBox eventbox4;

		private Gtk.Alignment alignment1;

		private VBox vbox2;

		private Label labName;

		private Label labDescription;

		private LabelLinkButton labLink;

		private EnumToolTipOrientation toolOrientation;

		private ComponentItem componentItem;
	}
}
