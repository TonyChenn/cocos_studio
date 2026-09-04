using System;
using Gtk;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace Modules.Communal.NewSolution
{
	// Token: 0x0200001E RID: 30
	public class ScreenTypeContent : IRadioItemContent
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x000081D6 File Offset: 0x000063D6
		public ScreenTypeContent(Xwt.Drawing.Image normal, Xwt.Drawing.Image select = null, Xwt.Drawing.Image hover = null)
		{
			this.imageView = new ImageView();
			this.normalImage = normal;
			this.selectImage = select;
			this.hoverImage = hover;
			this.SetImage(this.normalImage);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000820A File Offset: 0x0000640A
		public Widget GetGtkWidget()
		{
			return this.imageView;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00008214 File Offset: 0x00006414
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

		// Token: 0x060000E7 RID: 231 RVA: 0x00008265 File Offset: 0x00006465
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

		// Token: 0x040000B5 RID: 181
		private Xwt.Drawing.Image normalImage;

		// Token: 0x040000B6 RID: 182
		private Xwt.Drawing.Image selectImage;

		// Token: 0x040000B7 RID: 183
		private Xwt.Drawing.Image hoverImage;

		// Token: 0x040000B8 RID: 184
		private ImageView imageView;
	}
}
