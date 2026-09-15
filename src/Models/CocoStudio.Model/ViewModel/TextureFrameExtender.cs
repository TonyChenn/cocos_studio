using System;
using System.Reflection;
using CocoStudio.Core;
using CocoStudio.Projects;

namespace CocoStudio.Model.ViewModel
{
	public class TextureFrameExtender : BaseExtender
	{
		public TextureFrameExtender(TextureFrame bindingObject)
		{
			this._frameInstance = bindingObject;
			bindingObject.ParentChanged += this.OnObjectParentChanged;
			this.RegisterResourceFile(this._frameInstance.TextureFile);
		}

		~TextureFrameExtender()
		{
			this.Dispose();
		}

		internal override void OnObjectPropertyChanged(PropertyInfo propertyInfo)
		{
			if (!this._resChangeFromSelf)
			{
				if (propertyInfo.Name == "TextureFile" && this._frameInstance.IsRaisePropertyChanged && TimelineActionManager.Instance.CanAutoKey)
				{
					this.UnRegisterResourceFile(this._resFile);
					this._resFile = this._frameInstance.TextureFile;
					if (!Services.TaskService.IsUndoing || this.CheckFileExists())
					{
						this.RegisterResourceFile(this._resFile);
					}
				}
			}
		}

		private void RegisterResourceFile(ResourceFile resfile)
		{
			if (resfile != null && !resfile.IsDefault)
			{
				resfile.Deleted += this.Resource_Deleted;
				resfile.ContentChanged += this.Resource_ContentChanged;
			}
		}

		private void UnRegisterResourceFile(ResourceFile resfile)
		{
			if (resfile != null && !resfile.IsDefault)
			{
				resfile.Deleted -= this.Resource_Deleted;
				resfile.ContentChanged -= this.Resource_ContentChanged;
			}
		}

		private bool CheckFileExists()
		{
			bool result;
			if (this._resFile != null && !this._resFile.IsDefault && this._resFile.Parent == null)
			{
				this.SetResource(null);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		private void Resource_ContentChanged(object sender, EventArgs e)
		{
			ResourceFile resource = sender as ResourceFile;
			this.SetResource(resource);
		}

		private void Resource_Deleted(object sender, EventArgs e)
		{
			ResourceFile resfile = sender as ResourceFile;
			this.UnRegisterResourceFile(resfile);
			this.SetResource(null);
		}

		private void SetResource(ResourceFile resfile)
		{
			this._resChangeFromSelf = true;
			this._frameInstance.TextureFile = resfile;
			this._resFile = resfile;
			this._resChangeFromSelf = false;
		}

		private void OnObjectParentChanged(object sender, EventArgs e)
		{
			TextureFrame textureFrame = sender as TextureFrame;
			if (textureFrame != null)
			{
				if (textureFrame.Timeline == null)
				{
					this._resFile = null;
				}
				else
				{
					this._resFile = this._frameInstance.TextureFile;
				}
			}
		}

		public override void Dispose()
		{
			TextureFrame frameInstance = this._frameInstance;
			if (frameInstance != null)
			{
				frameInstance.ParentChanged -= this.OnObjectParentChanged;
			}
			this._resFile = null;
			GC.SuppressFinalize(this);
		}

		private TextureFrame _frameInstance = null;

		private ResourceFile _resFile = null;

		private bool _resChangeFromSelf = false;
	}
}
