using System;
using Gtk;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	public class ScreenTypeContent : IRadioItemContent
	{
		public ScreenTypeContent(Xwt.Drawing.Image normal, Xwt.Drawing.Image select = null, Xwt.Drawing.Image hover = null)
		{
			this.imageView = new ImageView();
			this.normalImage = normal;
			this.selectImage = select;
			this.hoverImage = hover;
			this.SetImage(this.normalImage);
		}

		public Widget GetGtkWidget()
		{
			return this.imageView;
		}

		public void RefreshUI(bool isSelect, ButtonState currentState)
		{
			Xwt.Drawing.Image image = null;
			if (isSelect)
			{
				image = this.selectImage;
			}
			else
			{
				switch (currentState)
				{
				case ButtonState.Normal:
					image = this.normalImage;
					break;
				case ButtonState.Hover:
					image = this.hoverImage;
					break;
				case ButtonState.Pressed:
					image = this.selectImage;
					break;
				}
			}
			this.SetImage(image);
		}

		private void SetImage(Xwt.Drawing.Image newImage)
		{
			if (newImage == null)
			{
				return;
			}
			if (this.imageView.Image == newImage)
			{
				return;
			}
			this.imageView.Image = newImage;
		}

		private Xwt.Drawing.Image normalImage;

		private Xwt.Drawing.Image selectImage;

		private Xwt.Drawing.Image hoverImage;

		private ImageView imageView;
	}
}
