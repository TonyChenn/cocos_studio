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
	// Token: 0x02000065 RID: 101
	[DataItem(Name = "Image")]
	public class ImageFile : ResourceFile
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000B0E8 File Offset: 0x000092E8
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

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000B103 File Offset: 0x00009303
		internal override string PreviewImagePath
		{
			get
			{
				return this.FullPath;
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000B10B File Offset: 0x0000930B
		public bool IsPacked()
		{
			return this.PackedItems.Count > 0;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000B11B File Offset: 0x0000931B
		public bool HasPackedTo(CocosItem cocosItem)
		{
			return this.PackedItems.Contains(cocosItem);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000B129 File Offset: 0x00009329
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

		// Token: 0x060002E8 RID: 744 RVA: 0x0000B167 File Offset: 0x00009367
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

		// Token: 0x060002E9 RID: 745 RVA: 0x0000B1A8 File Offset: 0x000093A8
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

		// Token: 0x060002EA RID: 746 RVA: 0x0000B220 File Offset: 0x00009420
		private void OnPackedChanged()
		{
			if (this.PackedChanged != null)
			{
				this.PackedChanged(this, null);
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000B237 File Offset: 0x00009437
		private ImageFile()
		{
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000B24A File Offset: 0x0000944A
		public ImageFile(FilePath fileName) : base(fileName)
		{
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000B25E File Offset: 0x0000945E
		public ImageFile(ResourceData resourceData) : base(resourceData)
		{
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060002EE RID: 750 RVA: 0x0000B274 File Offset: 0x00009474
		// (remove) Token: 0x060002EF RID: 751 RVA: 0x0000B2AC File Offset: 0x000094AC
		public event EventHandler<EventArgs> PackedChanged;

		// Token: 0x060002F0 RID: 752 RVA: 0x0000B2E4 File Offset: 0x000094E4
		protected override ResourceData CreateResourceData(FilePath filePath)
		{
			string cocosFilePath = null;
			if (this.packedItems != null && this.PackedItems.Count != 0)
			{
				cocosFilePath = this.PackedItems.First<CocosItem>().FileName;
			}
			return this.CreateResourceData(filePath, cocosFilePath);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000B328 File Offset: 0x00009528
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

		// Token: 0x060002F2 RID: 754 RVA: 0x0000B37C File Offset: 0x0000957C
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

		// Token: 0x060002F3 RID: 755 RVA: 0x0000B3AE File Offset: 0x000095AE
		protected override void OnRefresh()
		{
			CSCocosHelp.ReloadPngFileToCache(this.FullPath);
			base.OnRefresh();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000B3C4 File Offset: 0x000095C4
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

		// Token: 0x060002F5 RID: 757 RVA: 0x0000B404 File Offset: 0x00009604
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

		// Token: 0x060002F6 RID: 758 RVA: 0x0000B443 File Offset: 0x00009643
		protected override void OnDelete(IProgressMonitor monitor)
		{
			CSCocosHelp.RemovePngFileThreadSafe(this.FullPath);
			base.OnDelete(monitor);
			if (this.packedItems != null)
			{
				this.packedItems = null;
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000B466 File Offset: 0x00009666
		protected override void OnRemove(IProgressMonitor monitor)
		{
			CSCocosHelp.RemovePngFileThreadSafe(this.FullPath);
			base.OnRemove(monitor);
			if (this.packedItems != null)
			{
				this.packedItems = null;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000B48C File Offset: 0x0000968C
		public void PackedProjectNameChanged(CocosItem cocosItem, string oldcocosFilePath)
		{
			ResourceData oldResourceData = this.CreateResourceData(this.FullPath, oldcocosFilePath);
			ResourceChangeService.Instance.Register(this, false, oldResourceData);
		}

		// Token: 0x040000B1 RID: 177
		private HashSet<CocosItem> packedItems;

		// Token: 0x040000B2 RID: 178
		private object lockTag = new object();
	}
}
