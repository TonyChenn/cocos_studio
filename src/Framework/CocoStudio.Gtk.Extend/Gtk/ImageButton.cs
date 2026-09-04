using System;
using System.Collections.Generic;
using MonoDevelop.Components;
using MonoDevelopImageView = MonoDevelop.Components.ImageView;
using XwtImage = Xwt.Drawing.Image;

namespace Gtk
{
	// Token: 0x02000005 RID: 5
	public class ImageButton : BaseButton
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002440 File Offset: 0x00000640
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002458 File Offset: 0x00000658
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

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000247C File Offset: 0x0000067C
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002494 File Offset: 0x00000694
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

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000024B8 File Offset: 0x000006B8
		// (set) Token: 0x06000023 RID: 35 RVA: 0x000024D0 File Offset: 0x000006D0
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

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000024F4 File Offset: 0x000006F4
		// (set) Token: 0x06000025 RID: 37 RVA: 0x0000250C File Offset: 0x0000070C
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

		// Token: 0x06000026 RID: 38 RVA: 0x00002530 File Offset: 0x00000730
		public ImageButton()
		{
			this.imageView = new MonoDevelopImageView();
			base.Add(this.imageView);
			this.imageView.Show();
			this.imageDictionary = new Dictionary<ButtonState, XwtImage>();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000256C File Offset: 0x0000076C
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

		// Token: 0x06000028 RID: 40 RVA: 0x0000262C File Offset: 0x0000082C
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

		// Token: 0x0400000E RID: 14
		protected MonoDevelopImageView imageView;

		// Token: 0x0400000F RID: 15
		private Dictionary<ButtonState, XwtImage> imageDictionary;

		// Token: 0x04000010 RID: 16
		private XwtImage normalImg;

		// Token: 0x04000011 RID: 17
		private XwtImage hoverImg;

		// Token: 0x04000012 RID: 18
		private XwtImage pressedImg;

		// Token: 0x04000013 RID: 19
		private XwtImage disabledImg;
	}
}
