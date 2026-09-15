using System;
using System.Collections.Generic;

namespace Modules.Communal.ResourcePanel
{
	internal class FileTypeItemGroup
	{
		public event EventHandler<FileTypeItemChangedArgs> SelectedChanged;

		public FileTypeItem SelectedItem
		{
			get
			{
				FileTypeItem result = null;
				foreach (FileTypeItem fileTypeItem in this.itemList)
				{
					if (fileTypeItem.IsSelected)
					{
						result = fileTypeItem;
						break;
					}
				}
				return result;
			}
		}

		public FileTypeItemGroup()
		{
			this.itemList = new List<FileTypeItem>();
		}

		public void Add(FileTypeItem newItem)
		{
			bool flag = false;
			foreach (FileTypeItem fileTypeItem in this.itemList)
			{
				if (fileTypeItem == newItem)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.itemList.Add(newItem);
				newItem.Selected += this.HandleFileTypeItemSelected;
				if (this.itemList.Count == 1)
				{
					newItem.Select();
					return;
				}
				newItem.UnSelect();
			}
		}

		private void HandleFileTypeItemSelected(object sender, EventArgs e)
		{
			FileTypeItem fileTypeItem = sender as FileTypeItem;
			foreach (FileTypeItem fileTypeItem2 in this.itemList)
			{
				if (fileTypeItem != fileTypeItem2)
				{
					fileTypeItem2.UnSelect();
				}
			}
			if (this.SelectedChanged != null)
			{
				FileTypeItemChangedArgs e2 = new FileTypeItemChangedArgs(fileTypeItem);
				this.SelectedChanged(this, e2);
			}
		}

		private List<FileTypeItem> itemList;
	}
}
