using System;
using System.Reflection;
using CocoStudio.Core;
using CocoStudio.Projects;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200012E RID: 302
	public class TextureFrameExtender : BaseExtender
	{
		// Token: 0x06000B3A RID: 2874 RVA: 0x0002C710 File Offset: 0x0002A910
		public TextureFrameExtender(TextureFrame bindingObject)
		{
			this._frameInstance = bindingObject;
			bindingObject.ParentChanged += this.OnObjectParentChanged;
			this.RegisterResourceFile(this._frameInstance.TextureFile);
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0002C768 File Offset: 0x0002A968
		~TextureFrameExtender()
		{
			this.Dispose();
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0002C79C File Offset: 0x0002A99C
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

		// Token: 0x06000B3D RID: 2877 RVA: 0x0002C83C File Offset: 0x0002AA3C
		private void RegisterResourceFile(ResourceFile resfile)
		{
			if (resfile != null && !resfile.IsDefault)
			{
				resfile.Deleted += this.Resource_Deleted;
				resfile.ContentChanged += this.Resource_ContentChanged;
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0002C884 File Offset: 0x0002AA84
		private void UnRegisterResourceFile(ResourceFile resfile)
		{
			if (resfile != null && !resfile.IsDefault)
			{
				resfile.Deleted -= this.Resource_Deleted;
				resfile.ContentChanged -= this.Resource_ContentChanged;
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0002C8CC File Offset: 0x0002AACC
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

		// Token: 0x06000B40 RID: 2880 RVA: 0x0002C91C File Offset: 0x0002AB1C
		private void Resource_ContentChanged(object sender, EventArgs e)
		{
			ResourceFile resource = sender as ResourceFile;
			this.SetResource(resource);
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0002C93C File Offset: 0x0002AB3C
		private void Resource_Deleted(object sender, EventArgs e)
		{
			ResourceFile resfile = sender as ResourceFile;
			this.UnRegisterResourceFile(resfile);
			this.SetResource(null);
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x0002C961 File Offset: 0x0002AB61
		private void SetResource(ResourceFile resfile)
		{
			this._resChangeFromSelf = true;
			this._frameInstance.TextureFile = resfile;
			this._resFile = resfile;
			this._resChangeFromSelf = false;
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0002C988 File Offset: 0x0002AB88
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

		// Token: 0x06000B44 RID: 2884 RVA: 0x0002C9D4 File Offset: 0x0002ABD4
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

		// Token: 0x040004AE RID: 1198
		private TextureFrame _frameInstance = null;

		// Token: 0x040004AF RID: 1199
		private ResourceFile _resFile = null;

		// Token: 0x040004B0 RID: 1200
		private bool _resChangeFromSelf = false;
	}
}
