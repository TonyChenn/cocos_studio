using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000018 RID: 24
	internal class MaterialManager
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000078 RID: 120 RVA: 0x0000317F File Offset: 0x0000137F
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00003187 File Offset: 0x00001387
		public ConcurrentDictionary<string, MaterialContainer> MaterialDatas { get; private set; }

		// Token: 0x0600007A RID: 122 RVA: 0x00003190 File Offset: 0x00001390
		public static MaterialManager GetInstance()
		{
			if (MaterialManager._mInstance == null)
			{
				MaterialManager._mInstance = new MaterialManager();
			}
			return MaterialManager._mInstance;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000031A8 File Offset: 0x000013A8
		public MaterialManager()
		{
			this.MaterialDatas = new ConcurrentDictionary<string, MaterialContainer>();
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000031CC File Offset: 0x000013CC
		private void UpdateMaterialContentDirectoryList(string path, MaterialContainer container)
		{
			this.MaterialDatas.AddOrUpdate(path, container, (string k, MaterialContainer v) => container);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00003208 File Offset: 0x00001408
		public void UpdateContainer(string path)
		{
			MaterialContainer materialContainer = new MaterialContainer();
			materialContainer.ScanResourceFolder(path);
			if (!materialContainer.MaterialList.IsEmpty)
			{
				this.UpdateMaterialContentDirectoryList(path, materialContainer);
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003237 File Offset: 0x00001437
		public void UpdateContainers(string[] folders)
		{
			Parallel.ForEach<string>(folders, new Action<string>(this.UpdateContainer));
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000324C File Offset: 0x0000144C
		public MaterialItem GetMaterialResource(string path, string materialName)
		{
			MaterialItem result = null;
			MaterialContainer materialContainer = null;
			string text = path;
			string extension = Path.GetExtension(path);
			if (extension != null && extension.ToLower().Equals(".pu"))
			{
				text = Path.GetDirectoryName(Path.GetDirectoryName(path));
			}
			if (!string.IsNullOrEmpty(text))
			{
				if (!this.MaterialDatas.ContainsKey(text))
				{
					this.UpdateContainer(text);
				}
				materialContainer = this.MaterialDatas[text];
			}
			if (materialContainer != null)
			{
				result = materialContainer.FindResource(materialName);
			}
			return result;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000032C0 File Offset: 0x000014C0
		public void RemoveContainer(string path)
		{
			if (!string.IsNullOrEmpty(path) && this.MaterialDatas.ContainsKey(path))
			{
				MaterialContainer materialContainer = this.MaterialDatas[path];
				if (materialContainer != null)
				{
					this.MaterialDatas.TryRemove(path, out materialContainer);
				}
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003304 File Offset: 0x00001504
		public void MoveContainer(string oldPath, string newPath)
		{
			if (!string.IsNullOrEmpty(oldPath) && !string.IsNullOrEmpty(newPath))
			{
				MaterialContainer materialContainer = this.MaterialDatas[oldPath];
				if (materialContainer != null)
				{
					this.MaterialDatas.TryRemove(oldPath, out materialContainer);
					this.MaterialDatas.TryAdd(newPath, materialContainer);
					return;
				}
				this.UpdateContainer(newPath);
			}
		}

		// Token: 0x04000020 RID: 32
		private static MaterialManager _mInstance;
	}
}
