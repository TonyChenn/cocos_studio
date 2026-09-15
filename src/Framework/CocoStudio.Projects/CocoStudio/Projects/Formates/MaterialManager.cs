using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace CocoStudio.Projects.Formates
{
	internal class MaterialManager
	{
		public ConcurrentDictionary<string, MaterialContainer> MaterialDatas { get; private set; }

		public static MaterialManager GetInstance()
		{
			if (MaterialManager._mInstance == null)
			{
				MaterialManager._mInstance = new MaterialManager();
			}
			return MaterialManager._mInstance;
		}

		public MaterialManager()
		{
			this.MaterialDatas = new ConcurrentDictionary<string, MaterialContainer>();
		}

		private void UpdateMaterialContentDirectoryList(string path, MaterialContainer container)
		{
			this.MaterialDatas.AddOrUpdate(path, container, (string k, MaterialContainer v) => container);
		}

		public void UpdateContainer(string path)
		{
			MaterialContainer materialContainer = new MaterialContainer();
			materialContainer.ScanResourceFolder(path);
			if (!materialContainer.MaterialList.IsEmpty)
			{
				this.UpdateMaterialContentDirectoryList(path, materialContainer);
			}
		}

		public void UpdateContainers(string[] folders)
		{
			Parallel.ForEach<string>(folders, new Action<string>(this.UpdateContainer));
		}

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

		private static MaterialManager _mInstance;
	}
}
