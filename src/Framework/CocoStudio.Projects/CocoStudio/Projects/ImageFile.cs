using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	[DataItem(Name = "Image")]
	public class ImageFile : ResourceFile
	{
		public HashSet<CocosItem> PackedItems
		{
			get
			{
				if (this.packedItems == null)
				{
					this.packedItems = new HashSet<CocosItem>();
				}
				return this.packedItems;
			}
		}

		internal override string PreviewImagePath
		{
			get
			{
				return this.FullPath;
			}
		}

		public bool IsPacked()
		{
			return this.PackedItems.Count > 0;
		}

		public bool HasPackedTo(CocosItem cocosItem)
		{
			return this.PackedItems.Contains(cocosItem);
		}

		public void UnPackFrom(CocosItem cocosItem)
		{
			if (cocosItem == null || !this.PackedItems.Contains(cocosItem))
			{
				return;
			}
			ResourceChangeService.Instance.Register(this, false, null);
			this.PackedItems.Remove(cocosItem);
			this.OnPackedChanged();
			ResourceChangeService.Instance.NotifyResourceChanged();
		}

		public void PackTo(CocosItem cocosItem)
		{
			if (cocosItem == null || this.PackedItems.Contains(cocosItem))
			{
				return;
			}
			ResourceChangeService.Instance.Register(this, false, null);
			this.PackedItems.Add(cocosItem);
			this.OnPackedChanged();
			ResourceChangeService.Instance.NotifyResourceChanged();
		}

		public void InitializePack(CocosItem cocosItem)
		{
			if (ProjectsService.Instance.CurrentSolution == null || cocosItem == null || cocosItem.IsInitialized)
			{
				throw new InvalidOperationException("This function can only be called when solution is initializing.");
			}
			lock (this.lockTag)
			{
				if (!this.PackedItems.Contains(cocosItem))
				{
					this.PackedItems.Add(cocosItem);
				}
			}
		}

		private void OnPackedChanged()
		{
			if (this.PackedChanged != null)
			{
				this.PackedChanged(this, null);
			}
		}

		private ImageFile()
		{
		}

		public ImageFile(FilePath fileName) : base(fileName)
		{
		}

		public ImageFile(ResourceData resourceData) : base(resourceData)
		{
		}

		public event EventHandler<EventArgs> PackedChanged;

		protected override ResourceData CreateResourceData(FilePath filePath)
		{
			string cocosFilePath = null;
			if (this.packedItems != null && this.PackedItems.Count != 0)
			{
				cocosFilePath = this.PackedItems.First<CocosItem>().FileName;
			}
			return this.CreateResourceData(filePath, cocosFilePath);
		}

		private ResourceData CreateResourceData(FilePath filePath, string cocosFilePath)
		{
			if (cocosFilePath != null)
			{
				FilePath relativePath = ResourceItem.GetRelativePath(filePath);
				FilePath filePath2 = ResourceItem.GetRelativePath(cocosFilePath);
				filePath2 = Path.ChangeExtension(filePath2, ".plist");
				return new ResourceData(EnumResourceType.MarkedSubImage, relativePath, filePath2);
			}
			return base.CreateResourceData(filePath);
		}

		protected override DataError OnCheckDataError()
		{
			DataError dataError = base.OnCheckDataError();
			if (dataError != null)
			{
				return dataError;
			}
			if (!CSCocosHelp.CheckImageFormat(this.FullPath))
			{
				return new DataError(LanguageInfo.DataError3_ImageIsBroke);
			}
			return dataError;
		}

		protected override void OnRefresh()
		{
			CSCocosHelp.ReloadPngFileToCache(this.FullPath);
			base.OnRefresh();
		}

		protected override void OnSetLocation(FilePath newFilePath, bool isRename = true)
		{
			if (base.IsDefault)
			{
				throw new InvalidOperationException("Default resource can not be moved.");
			}
			string src = this.FileName;
			base.OnSetLocation(newFilePath, isRename);
			CSCocosHelp.RenamePngFileToCache(src, newFilePath);
		}

		protected override void OnMove(FilePath newMovePath)
		{
			if (base.IsDefault)
			{
				throw new InvalidOperationException("Default resource can not be moved.");
			}
			string src = this.FileName;
			base.OnMove(newMovePath);
			CSCocosHelp.RenamePngFileThreadSafe(src, newMovePath);
		}

		protected override void OnDelete(IProgressMonitor monitor)
		{
			CSCocosHelp.RemovePngFileThreadSafe(this.FullPath);
			base.OnDelete(monitor);
			if (this.packedItems != null)
			{
				this.packedItems = null;
			}
		}

		protected override void OnRemove(IProgressMonitor monitor)
		{
			CSCocosHelp.RemovePngFileThreadSafe(this.FullPath);
			base.OnRemove(monitor);
			if (this.packedItems != null)
			{
				this.packedItems = null;
			}
		}

		public void PackedProjectNameChanged(CocosItem cocosItem, string oldcocosFilePath)
		{
			ResourceData oldResourceData = this.CreateResourceData(this.FullPath, oldcocosFilePath);
			ResourceChangeService.Instance.Register(this, false, oldResourceData);
		}

		private HashSet<CocosItem> packedItems;

		private object lockTag = new object();
	}
}
