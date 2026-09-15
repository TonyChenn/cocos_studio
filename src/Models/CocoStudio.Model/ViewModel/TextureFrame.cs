using System;
using CocoStudio.Model.ExtensionModel;
using CocoStudio.Projects;

namespace CocoStudio.Model.ViewModel
{
	[FrameExtension(typeof(ResourceFile))]
	public class TextureFrame : Frame
	{
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

		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this.TextureFile = (this.PropertyHandler.GetValue(node, null) as ResourceFile);
		}

		public TextureFrame()
		{
			this.Tween = false;
		}

		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			this.PropertyHandler.SetValue(this.Node, this.TextureFile, null);
		}

		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			TextureFrame textureFrame = frame as TextureFrame;
			if (textureFrame != null)
			{
				textureFrame.TextureFile = this.TextureFile;
			}
		}

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

		protected override void OnBindingRecorder()
		{
			ExtenderFactory.Binding(this, new BaseExtender[]
			{
				new TextureFrameExtender(this)
			});
		}

		private ResourceFile textureFile;
	}
}
