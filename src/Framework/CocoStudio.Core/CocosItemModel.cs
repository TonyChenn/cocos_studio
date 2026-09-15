using System;
using System.IO;

namespace CocoStudio.Core
{
	public class CocosItemModel : IEquatable<CocosItemModel>
	{
		public string Name { get; private set; }

		public string LocalPath { get; private set; }

		public CocosItemModel(string localPath)
		{
			this.LocalPath = localPath;
			this.Name = Path.GetFileNameWithoutExtension(localPath);
		}

		public bool Equals(CocosItemModel other)
		{
			return !string.IsNullOrEmpty(this.LocalPath) && this.LocalPath.Equals(other.LocalPath);
		}
	}
}
