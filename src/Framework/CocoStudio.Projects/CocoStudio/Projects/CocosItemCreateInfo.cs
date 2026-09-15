using System;
using System.Collections.Generic;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	public class CocosItemCreateInfo
	{
		public FilePath FileName { get; private set; }

		public string ContentType { get; set; }

		public IList<ResourceItem> SelectedResourceItem { get; private set; }

		public float Width { get; private set; }

		public float Height { get; private set; }

		public CocosItemCreateInfo(FilePath fileName, IList<ResourceItem> selectedItems = null, float width = 0f, float height = 0f)
		{
			this.FileName = fileName;
			this.SelectedResourceItem = selectedItems;
			this.Width = width;
			this.Height = height;
		}
	}
}
