using System;
using System.Collections.Generic;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x0200002D RID: 45
	internal class FileTypeItemGroup
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060001BC RID: 444 RVA: 0x00009C3C File Offset: 0x00007E3C
		// (remove) Token: 0x060001BD RID: 445 RVA: 0x00009C74 File Offset: 0x00007E74
		public event EventHandler<FileTypeItemChangedArgs> SelectedChanged;

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00009CAC File Offset: 0x00007EAC
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

		// Token: 0x060001BF RID: 447 RVA: 0x00009D08 File Offset: 0x00007F08
		public FileTypeItemGroup()
		{
			this.itemList = new List<FileTypeItem>();
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00009D1C File Offset: 0x00007F1C
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

		// Token: 0x060001C1 RID: 449 RVA: 0x00009DB0 File Offset: 0x00007FB0
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

		// Token: 0x04000093 RID: 147
		private List<FileTypeItem> itemList;
	}
}
