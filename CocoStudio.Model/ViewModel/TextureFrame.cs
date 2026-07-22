using System;
using CocoStudio.Model.ExtensionModel;
using CocoStudio.Projects;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000E3 RID: 227
	[FrameExtension(typeof(ResourceFile))]
	public class TextureFrame : Frame
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x0001D0D8 File Offset: 0x0001B2D8
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x0001D0F0 File Offset: 0x0001B2F0
		public ResourceFile TextureFile
		{
			get
			{
				return this.textureFile;
			}
			set
			{
				this.textureFile = value;
				this.RaisePropertyChanged<ResourceFile>(() => this.TextureFile);
			}
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0001D140 File Offset: 0x0001B340
		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this.TextureFile = (this.PropertyHandler.GetValue(node, null) as ResourceFile);
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0001D161 File Offset: 0x0001B361
		public TextureFrame()
		{
			this.Tween = false;
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0001D174 File Offset: 0x0001B374
		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			this.PropertyHandler.SetValue(this.Node, this.TextureFile, null);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0001D198 File Offset: 0x0001B398
		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			TextureFrame textureFrame = frame as TextureFrame;
			if (textureFrame != null)
			{
				textureFrame.TextureFile = this.TextureFile;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0001D1CC File Offset: 0x0001B3CC
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x0001D1DF File Offset: 0x0001B3DF
		public override bool Tween
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001D1E4 File Offset: 0x0001B3E4
		protected override void OnBindingRecorder()
		{
			ExtenderFactory.Binding(this, new BaseExtender[]
			{
				new TextureFrameExtender(this)
			});
		}

		// Token: 0x040002FD RID: 765
		private ResourceFile textureFile;
	}
}
