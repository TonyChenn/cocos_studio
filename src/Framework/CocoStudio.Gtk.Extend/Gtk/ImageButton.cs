using System;
using System.Collections.Generic;
using MonoDevelop.Components;
using MonoDevelopImageView = MonoDevelop.Components.ImageView;
using XwtImage = Xwt.Drawing.Image;

namespace Gtk
{
	public class ImageButton : BaseButton
	{
		public XwtImage NormalImage
		{
			get
			{
				return this.normalImg;
			}
			set
			{
				this.normalImg = value;
				this.imageDictionary[ButtonState.Normal] = this.normalImg;
				base.RefreshUI();
			}
		}

		public XwtImage HoverImage
		{
			get
			{
				return this.hoverImg;
			}
			set
			{
				this.hoverImg = value;
				this.imageDictionary[ButtonState.Hover] = this.hoverImg;
				base.RefreshUI();
			}
		}

		public XwtImage PressedImage
		{
			get
			{
				return this.pressedImg;
			}
			set
			{
				this.pressedImg = value;
				this.imageDictionary[ButtonState.Pressed] = this.pressedImg;
				base.RefreshUI();
			}
		}

		public XwtImage DisabledImage
		{
			get
			{
				return this.disabledImg;
			}
			set
			{
				this.disabledImg = value;
				this.imageDictionary[ButtonState.Disabled] = this.disabledImg;
				base.RefreshUI();
			}
		}

		public ImageButton()
		{
			this.imageView = new MonoDevelopImageView();
			base.Add(this.imageView);
			this.imageView.Show();
			this.imageDictionary = new Dictionary<ButtonState, XwtImage>();
		}

		public void SetImages(string formatedId)
		{
			this.normalImg = ImageIcon.GetIcon(string.Format(formatedId, ButtonState.Normal));
			this.hoverImg = ImageIcon.GetIcon(string.Format(formatedId, ButtonState.Hover));
			this.pressedImg = ImageIcon.GetIcon(string.Format(formatedId, ButtonState.Pressed));
			this.disabledImg = ImageIcon.GetIcon(string.Format(formatedId, ButtonState.Disabled));
			this.imageDictionary[ButtonState.Normal] = this.normalImg;
			this.imageDictionary[ButtonState.Hover] = this.hoverImg;
			this.imageDictionary[ButtonState.Pressed] = this.pressedImg;
			this.imageDictionary[ButtonState.Disabled] = this.disabledImg;
			base.RefreshUI();
		}

		protected override void OnRefreshUI()
		{
			if (this.imageDictionary.ContainsKey(base.CurrentState))
			{
				XwtImage image = this.imageDictionary[base.CurrentState];
				if (image != null)
				{
					this.imageView.Image = image;
				}
			}
		}

		protected MonoDevelopImageView imageView;

		private Dictionary<ButtonState, XwtImage> imageDictionary;

		private XwtImage normalImg;

		private XwtImage hoverImg;

		private XwtImage pressedImg;

		private XwtImage disabledImg;
	}
}
