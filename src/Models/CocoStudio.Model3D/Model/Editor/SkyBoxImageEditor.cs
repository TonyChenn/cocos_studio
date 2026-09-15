using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class SkyBoxImageEditor : BaseEditor
	{
		private string WarningText
		{
			get
			{
				return this.warningIcon.Text;
			}
			set
			{
				this.warningIcon.Text = value;
				if (string.IsNullOrEmpty(value))
				{
					this.alignmentWarning.RemoveChild();
					return;
				}
				if (this.alignmentWarning.Child == null)
				{
					this.alignmentWarning.Add(this.warningIcon);
					this.warningIcon.Show();
				}
			}
		}

		public override bool IsShowLabel
		{
			get
			{
				return false;
			}
		}

		public override bool CanCaching
		{
			get
			{
				return false;
			}
		}

		public override bool IsMultiLine
		{
			get
			{
				return true;
			}
		}

		protected override Widget OnCreateWidget()
		{
			this.instance = (PropertyItem.FirstObject as ISkyBox);
			this.imageEventBoxList = new List<ImageEventBox>();
			this.resourceList = (base.PropertyItem.Values[0] as List<string>);
			if (this.resourceList == null)
			{
				return new Alignment(0.5f, 0.5f, 1f, 1f);
			}
			Widget child = this.CreateSkyBoxWidget();
			HBox hbox = new HBox();
			hbox.PackStart(child, false, false, 0U);
			hbox.ShowAll();
			base.SetControl();
			return hbox;
		}

		private Widget CreateSkyBoxWidget()
		{
			Table table = new Table(3U, 6U, false);
			table.RowSpacing = 6U;
			table.ColumnSpacing = 11U;
			Label label = this.CreateLabel(LanguageInfo.Display_SkyBox_Left);
			Label widget = this.CreateLabel(LanguageInfo.Display_SkyBox_Right);
			Label widget2 = this.CreateLabel(LanguageInfo.Display_SkyBox_Up);
			Label widget3 = this.CreateLabel(LanguageInfo.Display_SkyBox_Down);
			Label widget4 = this.CreateLabel(LanguageInfo.Display_SkyBox_Front);
			Label widget5 = this.CreateLabel(LanguageInfo.Display_SkyBox_Back);
			table.Attach(label, 0U, 1U, 0U, 1U);
			table.Attach(widget, 3U, 4U, 0U, 1U);
			table.Attach(widget2, 0U, 1U, 2U, 3U);
			table.Attach(widget3, 3U, 4U, 2U, 3U);
			table.Attach(widget4, 0U, 1U, 1U, 2U);
			table.Attach(widget5, 3U, 4U, 1U, 2U);
			label.WidthRequest = PropertyPadStyle.propertyLabelWidth;
			foreach (string text in this.resourceList)
			{
				PropertyItem.FirstObject.GetType().GetProperty(text);
				PropertyDescriptor descriptor = TypeDescriptor.GetProperties(PropertyItem.FirstObject.GetType()).Find(text, false);
				Widget widget6 = this.CreateImageWidget(descriptor, text);
				if (text.Equals("LeftImage"))
				{
					table.Attach(widget6, 1U, 2U, 0U, 1U);
				}
				else if (text.Equals("RightImage"))
				{
					table.Attach(widget6, 4U, 5U, 0U, 1U);
				}
				else if (text.Equals("ForwardImage"))
				{
					table.Attach(widget6, 1U, 2U, 1U, 2U);
				}
				else if (text.Equals("BackImage"))
				{
					table.Attach(widget6, 4U, 5U, 1U, 2U);
				}
				else if (text.Equals("UpImage"))
				{
					table.Attach(widget6, 1U, 2U, 2U, 3U);
				}
				else if (text.Equals("DownImage"))
				{
					table.Attach(widget6, 4U, 5U, 2U, 3U);
				}
			}
			VBox vbox = new VBox();
			vbox.PackStart(this.alignmentWarning, false, false, 0U);
			table.Attach(vbox, 5U, 6U, 0U, 1U);
			this.alignmentWarning.LeftPadding = (this.alignmentWarning.TopPadding = 5U);
			return table;
		}

		private Label CreateLabel(string text)
		{
			Label label = new Label(text);
			label.Sensitive = false;
			label.SetFontSize(11.0);
			label.Xalign = 1f;
			return label;
		}

		private Widget CreateImageWidget(PropertyDescriptor descriptor, string data)
		{
			new Color(50, 49, 54);
			EventBox eventBox = new EventBox();
			eventBox.SetNormalBg(WindowStyle.WindowLineColor);
			eventBox.WidthRequest = (eventBox.HeightRequest = 60);
			Alignment alignment = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment.BorderWidth = 1U;
			eventBox.Add(alignment);
			ImageEventBox imageEventBox = new ImageEventBox(base.PropertyItem, descriptor, new int?(58));
			imageEventBox.SetDefaultFileMarker();
			imageEventBox.SetNormalBg(WindowStyle.WindowBgColor);
			this.imageEventBoxList.Add(imageEventBox);
			alignment.Add(imageEventBox);
			return eventBox;
		}

		protected override void OnSetControl()
		{
			if (this.instance != null)
			{
				this.WarningText = this.instance.SkyboxResourceError;
			}
		}

		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			foreach (string b in this.resourceList)
			{
				if (e.PropertyName == b)
				{
					base.SetControl();
					break;
				}
			}
			foreach (ImageEventBox imageEventBox in this.imageEventBoxList)
			{
				if (imageEventBox.DescriptorName == e.PropertyName)
				{
					imageEventBox.Refresh();
				}
			}
		}

		private TooltipIcon warningIcon = new TooltipIcon();

		private Alignment alignmentWarning = new Alignment(0.5f, 0.5f, 1f, 1f);

		private List<string> resourceList;

		private List<ImageEventBox> imageEventBoxList;

		private ISkyBox instance;
	}
}
